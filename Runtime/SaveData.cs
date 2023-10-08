using System.Collections.Generic;
using UnityEngine;

namespace Aarthificial.Safekeeper {
  /// <summary>
  /// A basic implementation of <see cref="ISaveData"/> that uses JSON
  /// serialization.
  /// </summary>
  public class SaveData : ISaveData {
    private const string _defaultChunkId = "default";

    private readonly Dictionary<string, Dictionary<string, string>> _data =
      new();

    public T Read<T>(SaveLocation location) where T : new() {
      var realChunkId = location.ChunkId ?? _defaultChunkId;

      return _data.ContainsKey(realChunkId)
        && _data[realChunkId].ContainsKey(location.ObjectId)
          ? JsonUtility.FromJson<T>(_data[realChunkId][location.ObjectId])
          : new T();
    }

    public bool Read<T>(SaveLocation location, T target) {
      var realChunkId = location.ChunkId ?? _defaultChunkId;

      if (location.ObjectId == null
        || !_data.ContainsKey(realChunkId)
        || !_data[realChunkId].ContainsKey(location.ObjectId)) {
        return false;
      }

      JsonUtility.FromJsonOverwrite(
        _data[realChunkId][location.ObjectId],
        target
      );

      return true;
    }

    public void Write(SaveLocation location, object value) {
#if UNITY_EDITOR
      if (location.ChunkId == "00000000000000000000000000000000") {
        Debug.LogFormat(
          "[Safekeeper]: Unexpected prefab ({0})",
          location.ObjectId
        );
        return;
      }
#endif
      var realChunkId = location.ChunkId ?? _defaultChunkId;
      if (!_data.ContainsKey(realChunkId)) {
        _data[realChunkId] = new Dictionary<string, string>();
      }

      _data[realChunkId][location.ObjectId] = JsonUtility.ToJson(value);
    }

    public Dictionary<string, string> GetChunk(string chunkId = null) {
      var realChunkId = chunkId ?? _defaultChunkId;

      if (!_data.ContainsKey(realChunkId)) {
        _data[realChunkId] = new Dictionary<string, string>();
      }

      return _data[realChunkId];
    }

    public IEnumerable<string> GetChunkIds() {
      return _data.Keys;
    }
  }
}
