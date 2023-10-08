namespace Aarthificial.Safekeeper.Stores {
  /// <summary>
  /// An interface with callbacks for saving and loading.
  /// </summary>
  /// <remarks>
  /// Objects implementing this interface should be registered with
  /// <see cref="SaveStoreRegistry"/>.
  /// It can be done manually or by adding the <see cref="SaveStoreDispatcher"/>
  /// component to the same game object.
  /// </remarks>
  /// <example>
  /// Implementing the interface:
  /// <code>
  /// public class SavedTransform : MonoBehaviour, ISaveStore {
  ///   private class StoredData {
  ///     public Vector3 position;
  ///     public Quaternion rotation;
  ///   }
  ///
  ///   [ObjectLocation]
  ///   [SerializeField]
  ///   private SaveLocation _location;
  ///   private StoredData _data = new();
  ///
  ///   public void OnEnable() {
  ///     SaveStoreRegistry.Register(this);
  ///   }
  ///
  ///   public void OnDisable() {
  ///     SaveStoreRegistry.Unregister(this);
  ///   }
  ///
  ///   public void OnLoad(SaveControllerBase save) {
  ///     if (save.Data.Read(_location, _data)) {
  ///       transform.position = _data.position;
  ///       transform.rotation = _data.rotation;
  ///     }
  ///   }
  ///
  ///   public void OnSave(SaveControllerBase save) {
  ///     _data.position = transform.position;
  ///     _data.rotation = transform.rotation;
  ///     save.Data.Write(_location, data);
  ///   }
  /// }
  /// </code>
  /// <example>
  public interface ISaveStore {
    /// <summary>
    /// Invoked when the data is loaded from the memory.
    /// </summary>
    /// <param name="save">The current save controller.</param>
    void OnLoad(SaveControllerBase save);

    /// <summary>
    /// Invoked right before the data is saved to the memory.
    /// </summary>
    /// <param name="save">The current save controller.</param>
    void OnSave(SaveControllerBase save);
  }
}
