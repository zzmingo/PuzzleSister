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
      CSV
    }

    public enum Source {
      Resources
    }
    
    public string id;
    public string name;
    public Sprite thumb;
    public string path;
    public Type type;
    public Source source;

    public List<Question> Load() {
      object data;
      switch(source) {
        default:
          data = new ResourcesReader().Read(this);
          break;
      }
      switch(type) {
        default:
          return new CSVParser().Parse(this, data);
      }
    }

  }

}