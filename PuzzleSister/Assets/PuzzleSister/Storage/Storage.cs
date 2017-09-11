using Newtonsoft.Json;

namespace PuzzleSister {

  public abstract class Storage {

    public static Storage shared {
      get {
        if (internalShared == null) {
#if UNITY_STANDALONE
          internalShared = new StandaloneStorage();
#else
          internalShared = null;
#endif
        } 
        return internalShared;
      }
    }

    private static Storage internalShared = null;

    public abstract void Save(string path, string data, bool encript = true);
    public abstract string Load(string path, bool decript = true);

    public void SerializeSave<T>(string path, T data, bool encript = true) {
      Save(path, JsonConvert.SerializeObject(data), encript);
    }
    public T DeserializeLoad<T>(string path, T defaults, bool decript = true) {
      string data = Load(path, decript);
      if (data == null) {
        return defaults;
      } else {
        return JsonConvert.DeserializeObject<T>(data);
      }
    }

  }

}