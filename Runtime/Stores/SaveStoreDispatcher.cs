using UnityEngine;

namespace Aarthificial.Safekeeper.Stores {
  /// <summary>
  /// A helper component that registers and unregisters all
  /// <see cref="ISaveStore"/>s on the same game object.
  /// </summary>
  public class SaveStoreDispatcher : MonoBehaviour {
    private ISaveStore[] _stores;

    private void Awake() {
      _stores = GetComponents<ISaveStore>();
    }

    private void OnEnable() {
      for (var i = 0; i < _stores.Length; i++) {
        var store = _stores[i];
        SaveStoreRegistry.Register(store);
      }
    }

    private void OnDisable() {
      for (var i = 0; i < _stores.Length; i++) {
        var store = _stores[i];
        SaveStoreRegistry.Unregister(store);
      }
    }
  }
}
