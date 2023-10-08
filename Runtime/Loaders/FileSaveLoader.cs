using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Aarthificial.Safekeeper.Loaders {
  /// <summary>
  /// Stores saves in the persistent data path. 
  /// </summary>
  ///
  /// <remarks>
  /// The saves are stored locally in a `saves` directory located in the
  /// persistent data path.
  /// </remarks>
  public class FileSaveLoader : ISaveLoader {
    private readonly string _directory;
    private readonly string _filePath;
    private readonly string _name;
    private readonly SemaphoreSlim _lock = new(1);

    public FileSaveLoader(string fileName) {
      _directory = Path.Combine(Application.persistentDataPath, "saves");
      _filePath = Path.Combine(_directory, fileName + ".data");
      _name = $"File Save \"{fileName}\"";
    }

    public Task<string> GetName() {
      return Task.FromResult(_name);
    }

    public Task<bool> Exists() {
      return Task.FromResult(File.Exists(_filePath));
    }

    public async Task<ISaveData> Load() {
      await _lock.WaitAsync();
      if (!File.Exists(_filePath)) {
        _lock.Release();
        return new SaveData();
      }

      await using var loadStream = new FileStream(
        _filePath,
        FileMode.Open,
        FileAccess.Read,
        FileShare.Read
      );
      using var reader = new BinaryReader(loadStream, Encoding.UTF8, false);
      var save = new SaveData();
      while (reader.ReadBoolean()) {
        var chunkId = reader.ReadString();
        var chunk = save.GetChunk(chunkId);
        var count = reader.ReadInt32();
        for (var i = 0; i < count; i++) {
          chunk.Add(reader.ReadString(), reader.ReadString());
        }
      }

      _lock.Release();

      return save;
    }

    public async Task Save(ISaveData data) {
      await _lock.WaitAsync();
      await Task.Run(
        () => {
          if (!Directory.Exists(_directory)) {
            Directory.CreateDirectory(_directory);
          }

          using var saveStream = new FileStream(_filePath, FileMode.Create);
          using var writer = new BinaryWriter(saveStream, Encoding.UTF8, false);
          foreach (var chunkId in data.GetChunkIds()) {
            var chunk = data.GetChunk(chunkId);
            writer.Write(true);
            writer.Write(chunkId);
            writer.Write(chunk.Count);
            foreach (var (key, value) in chunk) {
              writer.Write(key);
              writer.Write(value);
            }
          }
          writer.Write(false);
        }
      );

      _lock.Release();
    }

    public Task<ISaveData> Create() {
      return Task.FromResult((ISaveData)new SaveData());
    }

    public async Task Delete() {
      await _lock.WaitAsync();

      if (File.Exists(_filePath)) {
        File.Delete(_filePath);
      }

      _lock.Release();
    }
  }
}
