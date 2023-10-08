using System.Collections.Generic;

namespace Aarthificial.Safekeeper.Stores {
  /// <summary>
  /// A static registry for <see cref="ISaveStore"/>s.
  /// </summary>
  public static class SaveStoreRegistry {
    private static readonly LinkedList<ISaveStore> _stores = new();

    /// <summary>
    /// Register a store.
    /// </summary>
    /// <param name="store">The store to register.</param>
    public static void Register(ISaveStore store) {
      _stores.AddFirst(store);
    }

    /// <summary>
    /// Unregister a store. 
    /// </summary>
    /// <param name="store">The store to unregister.</param>
    public static void Unregister(ISaveStore store) {
      _stores.Remove(store);
    }

    /// <summary>
    /// Invoke <see cref="ISaveStore.OnLoad"/> on all registered stores.
    /// </summary>
    /// <param name="save">The current save controller.</param>
    public static void OnLoad(SaveControllerBase save) {
      var store = _stores.First;
      while (store != null) {
        store.Value.OnLoad(save);
        store = store.Next;
      }
    }

    /// <summary>
    /// Invoke <see cref="ISaveStore.OnSave"/> on all registered stores.
    /// </summary>
    public static void OnSave(SaveControllerBase save) {
      var store = _stores.First;
      while (store != null) {
        store.Value.OnSave(save);
        store = store.Next;
      }
    }

    /// <summary>
    /// Clear the registry.
    /// </summary>
    /// <remarks>
    /// Ideally, stores should remove themselves from the registry.
    /// During development, it may be useful to use this method to
    /// check if all stores are properly removed and log a warning
    /// otherwise.
    /// </remarks>
    /// <returns>Whether any stores were removed.</returns>
    public static bool Clear() {
      var hasStores = _stores.Count > 0;
      _stores.Clear();
      return hasStores;
    }
  }
}
