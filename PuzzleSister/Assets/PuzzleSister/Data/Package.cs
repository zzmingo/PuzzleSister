using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PuzzleSister {

  public class Package {

    public interface Reader {
      object Read(Package package);
    }

    public interface Parser {
      List<Question> Parse(Package package, object data);
    }

    public enum Type {
      CSV, JSON
    }

    public enum Source {
      Resources,
      DLC,
      QEditor
    }
    
    public string id;
    public string name;
    public string thumb;
    public string path;
    public Type type;
    public Source source;

    public int CountQuestions() {
      return Load().Count;
    }

    public List<Question> Load() {
      object data;
      switch(source) {
        case Source.DLC:
          data = new DLCReader().Read(this);
          break;
        default:
          data = new ResourcesReader().Read(this);
          break;
      }
      List<Question> list;
      switch(type) {
        default:
          list = new CSVParser().Parse(this, data);
          break;
      }
      return list;
    }

    public void FromDict(Dictionary<string, object> dict, string path, Type type = Type.CSV, Source source = Source.Resources) {
      this.id = dict["id"].ToString();
      this.name = dict["name"].ToString();
      this.thumb = dict["thumb"].ToString();
      this.path = path;
      this.type = type;
      this.source = source;
    }

  }

}