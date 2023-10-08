using System;

namespace Aarthificial.Safekeeper {
  /// <summary>
  /// Represents a location in the save data.
  /// </summary>
  [Serializable]
  public struct SaveLocation {
    /// <summary>
    /// The ID of the chunk.
    /// </summary>
    public string ChunkId;
    /// <summary>
    /// The ID of the object. 
    /// </summary>
    public string ObjectId;

    public SaveLocation(string chunkId, string objectId) {
      ChunkId = chunkId;
      ObjectId = objectId;
    }
  }
}
