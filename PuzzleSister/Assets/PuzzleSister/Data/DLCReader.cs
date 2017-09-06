using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.IO;

namespace PuzzleSister {

  public class DLCReader : Package.Reader {

    public object Read(Package package) {
      return DecriptUtils.Descript(File.ReadAllText(package.path));
    }

  }

}