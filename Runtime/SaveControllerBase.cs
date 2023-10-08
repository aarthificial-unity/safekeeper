using Aarthificial.Safekeeper.Loaders;
using Aarthificial.Safekeeper.Stores;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Aarthificial.Safekeeper {
  /// <summary>
  /// A handle to the save data.
  /// </summary>
  public class SaveControllerBase {
#if UNITY_EDITOR
    public static readonly List<WeakReference<SaveControllerBase>>
      ExistingSaves = new();
#endif

    private readonly ISaveLoader _loader;
    private readonly SemaphoreSlim _lock = new(1);

    public string Name;

    private bool _isInitialized;

    public SaveControllerBase(ISaveLoader loader) {
      _loader = loader;
    }

    /// <summary>
    /// Whether the save is currently being loaded.
    /// </summary>
    public bool IsLoading => _lock.CurrentCount == 0 || !_isInitialized;
    /// <summary>
    /// Whether the save exists.
    /// </summary>
    public bool Exists { get; private set; }
    private ISaveData _data = new SaveData();
    /// <summary>
    /// The save data.
    /// </summary>
    /// <remarks>
    /// The data can only be accessed after the save has been loaded for the
    /// first time. Otherwise it returns an empty <see cref="SaveData"/>.
    /// </remarks>
    public ISaveData Data {
      get {
        if (_data == null) {
          Debug.LogError(
            "[Safekeeper]: Tried to access the data before the save was loaded.\n Make sure to call `save.Load(SaveMode.Full)` before accessing the data"
          );
          return new SaveData();
        }
        return _data;
      }
      private set => _data = value;
    }
    public event Action Saving;
    public event Action Saved;

    /// <summary>
    /// Initialize this save controller.
    /// </summary>
    /// <remarks>
    /// This process loads the basic information about the save, such as its
    /// name and whether it exists.
    /// It does not load the data itself.
    /// </remarks>
    public async void Initialize() {
      if (_isInitialized) {
        return;
      }
      _isInitialized = true;
#if UNITY_EDITOR
      ExistingSaves.RemoveAll(wr => !wr.TryGetTarget(out _));
      ExistingSaves.Add(new WeakReference<SaveControllerBase>(this));
#endif

      await Lock();
      Exists = await _loader.Exists();
      Name = await _loader.GetName();
      Unlock();
    }

    /// <summary>
    /// Invoked when the save is being saved to the memory.
    /// </summary>
    /// <remarks>
    /// This method is called after <see cref="ISaveStore"/>s are notified.
    /// It can be used to save global data unrelated to scenes.
    /// </remarks>
    protected virtual void OnSave() { }

    /// <summary>
    /// Invoked when the save is loaded from the memory.
    /// </summary>
    /// <remarks>
    /// This method is guaranteed to be called before the
    /// <see cref="ISaveStore"/>s are notified.
    /// It can be used to load global data unrelated to scenes.
    /// </remarks>
    protected virtual void OnLoad() { }

    /// <summary>
    /// Invoked when the save is deleted.
    /// </summary>
    protected virtual void OnDelete() { }

    /// <summary>
    /// Save the game state. 
    /// </summary>
    public async Task Save(SaveMode mode = SaveMode.MemoryOnly) {
      await Lock();

      if (mode != SaveMode.PersistentOnly) {
        SaveStoreRegistry.OnSave(this);
        OnSave();
      }

      if (mode != SaveMode.MemoryOnly) {
        await _loader.Save(Data);
      }

      Unlock();
    }

    /// <summary>
    /// Load the game state.
    /// </summary>
    public async Task Load(SaveMode mode = SaveMode.MemoryOnly) {
      await Lock();

      if (mode != SaveMode.MemoryOnly) {
        Data = await _loader.Load();
      }

      if (mode != SaveMode.PersistentOnly) {
        OnLoad();
        SaveStoreRegistry.OnLoad(this);
      }

      Unlock();
    }

    /// <summary>
    /// Create the save if it doesn't exist.
    /// </summary>
    public async Task Create() {
      await Lock();
      if (!Exists) {
        Exists = true;
        Data = await _loader.Create();
        OnLoad();
      }

      Unlock();
    }

    /// <summary>
    /// Delete the save.
    /// </summary>
    public async Task Delete() {
      await Lock();
      if (Exists) {
        Exists = false;
        OnDelete();
        Data = null;
        await _loader.Delete();
      }

      Unlock();
    }

    private async Task Lock() {
      await _lock.WaitAsync();
      Saving?.Invoke();
    }

    private void Unlock() {
      _lock.Release();
      Saved?.Invoke();
    }
  }
}
