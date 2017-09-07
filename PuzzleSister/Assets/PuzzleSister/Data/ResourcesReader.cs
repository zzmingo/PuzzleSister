using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PuzzleSister {

  public class ResourcesReader : Package.Reader {

    public object Read(Package package) {
      return CryptoUtils.Decript(Resources.Load<TextAsset>(package.path).text);
    }

  }

}