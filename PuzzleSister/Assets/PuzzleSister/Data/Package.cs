using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace PuzzleSister {

  public class Package {

    public interface Reader {
      object Read(Package package);
    }

    public interface Parser {
      List<Question> Parse(Package package, object data);
    }

    public enum Type {
      None, CSV, JSON
    }

    public enum Source {
      Memory,
      Resources,
      DLC,
      UGC
    }

    public enum State {
      Prepare,
      Ready
    }
    
    public string id;
    public string name;
    public string author;
		public string language;
    public string description;
    public string thumb;
    public string path;
    public State state;
    public Type type;
    public Source source;
    public List<Question> questionList;
    public bool temporary = false;

    public Sprite ThumbSprite {
      get {
        if (thumbSprite == null) {
          thumbSprite = SpriteExtensions.Base64ToSprite(thumb);
        }
        return thumbSprite;
      }
    }
    private Sprite thumbSprite;


    public int CountQuestions() {
      return Load().Count;
    }

    public List<Question> Load() {
      object data = null;
      switch(source) {
        case Source.DLC:
          data = new DLCReader().Read(this);
          break;
        case Source.Resources:
          data = new ResourcesReader().Read(this);
          break;
        case Source.UGC:
          data = new UGCReader().Read(this);
          break;
        default:
          break;
      }
      if (data == null) {
        return questionList;
      }
      List<Question> list;
      switch(type) {
        case Type.JSON:
          list = JsonConvert.DeserializeObject<List<Question>>((string) data);
          break;
        default:
          list = new CSVParser().Parse(this, data);
          break;
      }
      return list;
    }

    public void FromDict(Dictionary<string, object> dict, string path, Type type = Type.CSV, Source source = Source.Resources) {
      this.id = dict["id"].ToString();
      this.name = dict["name"].ToString();
			this.author = dict.ContainsKey("author") ? dict["author"].ToString() : "未知";
			this.language = dict["language"].ToString();
      this.thumb = dict["thumb"].ToString();
      this.path = path;
      this.type = type;
      this.source = source;
    }

  }

}