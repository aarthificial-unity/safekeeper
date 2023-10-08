using System.Threading.Tasks;

namespace Aarthificial.Safekeeper.Loaders {
  /// <summary>
  /// A common interface for saving and loading data in the persistent storage.
  /// </summary>
  public interface ISaveLoader {
    /// <summary>
    /// Get the name of this save.
    /// </summary>
    /// <remarks>
    /// Used exclusively for debugging purposes.
    /// </remarks>
    /// <returns>The name of this save.</returns>
    Task<string> GetName();

    /// <summary>
    /// Check if the save has already been created.
    /// </summary>
    /// <returns>Whether the save exists.</returns>
    Task<bool> Exists();

    /// <summary>
    /// Load the data from the persistent storage.
    /// </summary>
    /// <returns>The loaded data.</returns>
    Task<ISaveData> Load();

    /// <summary>
    /// Save the data to the persistent storage.
    /// </summary>
    /// <param name="data">The data to save.</param>
    Task Save(ISaveData data);

    /// <summary>
    /// Create the save data for this save.
    /// </summary>
    /// <remarks>
    /// This method is used if the save does not exist yet.
    /// It should not save the data to the persistent storage, only create it.
    /// This data will later be passed to <see cref="Save"/>.
    /// </remarks>
    /// <returns>The newly created save data.</returns>
    Task<ISaveData> Create();

    /// <summary>
    /// Remove this save from the persistent storage.
    /// </summary>
    Task Delete();
  }
}
