using System.Collections.Generic;

namespace Aarthificial.Safekeeper {
  /// <summary>
  /// A common interface for representing the save data.
  /// </summary>
  public interface ISaveData {
    /// <summary>
    /// Read the save data from the given location.
    /// </summary>
    /// <param name="location">The location to read from.</param>
    /// <typeparam name="T">The type of the data.</typeparam>
    /// <returns>A new instance of the data type.</returns>
    T Read<T>(SaveLocation location) where T : new();

    /// <summary>
    /// Read the save data from the given location into the given target.
    /// </summary>
    /// <param name="location">The location to read from.</param>
    /// <param name="target">The target to read into.</param>
    /// <typeparam name="T">The type of the data.</typeparam>
    /// <returns>Whether the data was read.</returns>
    bool Read<T>(SaveLocation location, T target);

    /// <summary>
    /// Write the given value to the given location.
    /// </summary>
    /// <param name="location">The location to write to.</param>
    /// <param name="value">The value to write.</param>
    void Write(SaveLocation location, object value);

    /// <summary>
    /// Get the chunk with the given ID or create a new one if it doesn't exist.
    /// </summary>
    /// <param name="chunkId">The chunk ID.</param>
    /// <returns>The retrieved chunk.</returns>
    Dictionary<string, string> GetChunk(string chunkId);

    /// <summary>
    /// Get the IDs of all existing chunks.
    /// </summary>
    IEnumerable<string> GetChunkIds();
  }
}
