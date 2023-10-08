namespace Aarthificial.Safekeeper {
  /// <summary>
  /// Controls where the data is saved/loaded from.
  /// </summary>
  public enum SaveMode {
    /// <summary>
    /// When loading, the data from memory is loaded onto game objects.
    /// When saving, the data from game objects is saved to memory.
    /// </summary>
    MemoryOnly,
    /// <summary>
    /// When loading, the data from the persistent storage is fetched to memory.
    /// When saving, the data from memory is committed to the persistent storage.
    /// </summary>
    PersistentOnly,
    /// <summary>
    /// When loading, the data from the persistent storage is fetched to memory and then loaded onto game objects.
    /// When saving, the data from game objects is saved to memory and then committed to the persistent storage.
    /// </summary>
    Full,
  }
}
