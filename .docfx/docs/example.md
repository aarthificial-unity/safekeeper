# Full example

Below is a minimal example of a scene manager that uses safekeeper. Presumably,
this MonoBehaviour is attached to a game object that persists throughout the
whole game:

```csharp
public class SceneManager : MonoBehaviour {
  private SaveControllerBase _controller;

  private void Awake() {
    _controller = new SaveControllerBase(
      new FileSaveLoader("slot1")
    );
    _controller.Initialize();
  }

  public IEnumerator LoadScene(string scenePath) {
    // Save the current state to memory
    yield return WaitForTask(_controller.Save());
    // Switch the scene
    yield return SceneManager.LoadSceneAsync(scenePath);
    // Load the state from memory
    yield return WaitForTask(_controller.Load());
  }

  public IEnumerator SaveGame() {
    // Save the current state to memory and commit it to the persistent storage
    yield return WaitForTask(_controller.Save(SaveMode.Full));
  }

  public IEnumerator ResetGame() {
    // Reset memory with the data from the persistent storage
    yield return WaitForTask(_controller.Load(SaveMode.PersistentOnly));
    // Reload the scene
    yield return SceneManager.LoadSceneAsync(
      SceneManager.GetActiveScene().path
    );
    // Load the state from memory
    yield return WaitForTask(_controller.Load());
  }

  private IEnumerator WaitForTask(Task task) {
    while (!task.IsCompleted) {
      yield return null;
    }

    if (task.IsFaulted) {
      ExceptionDispatchInfo.Capture(task.Exception).Throw();
    }
  }
}
```

With this setup, you can easily save the state of your game by implementing the
[`ISaveStore`](xref:Aarthificial.Safekeeper.Stores.ISaveStore) interface and
registering it with the
[`SaveStoreRegistry`](xref:Aarthificial.Safekeeper.Stores.SaveStoreRegistry):

```csharp
public class SavedTransform : MonoBehaviour, ISaveStore {
  private class StoredData {
    public Vector3 position;
    public Quaternion rotation;
  }

  [ObjectLocation]
  [SerializeField]
  private SaveLocation _location;
  private StoredData _data = new();

  public void OnEnable() {
    SaveStoreRegistry.Register(this);
  }

  public void OnDisable() {
    SaveStoreRegistry.Unregister(this);
  }

  // OnLoad will be invoked right after the scene is loaded.
  // Before `Start` but after `OnEnable`.
  public void OnLoad(SaveControllerBase save) {
    if (save.Data.Read(_location, _data)) {
      transform.position = _data.position;
      transform.rotation = _data.rotation;
    }
  }

  // OnLoad will be invoked right before the scene in unloaded or whenever
  // the game is saved.
  public void OnSave(SaveControllerBase save) {
    _data.position = transform.position;
    _data.rotation = transform.rotation;
    save.Data.Write(_location, data);
  }
}
```
