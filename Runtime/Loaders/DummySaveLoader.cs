using System.Threading.Tasks;

namespace Aarthificial.Safekeeper.Loaders {
  /// <summary>
  /// A dummy save loader that does nothing.
  /// </summary>
  public class DummySaveLoader : ISaveLoader {
    private bool _exists;

    public Task<string> GetName() {
      return Task.FromResult("Dummy Save");
    }

    public Task<bool> Exists() {
      return Task.FromResult(_exists);
    }

    public Task<ISaveData> Load() {
      return Task.FromResult<ISaveData>(new SaveData());
    }

    public Task Save(ISaveData data) {
      return Task.CompletedTask;
    }

    public Task<ISaveData> Create() {
      _exists = true;
      return Task.FromResult<ISaveData>(new SaveData());
    }

    public Task Delete() {
      _exists = false;
      return Task.CompletedTask;
    }
  }
}
