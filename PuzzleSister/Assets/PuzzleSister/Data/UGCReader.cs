using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.IO;

namespace PuzzleSister {

  public class UGCReader : Package.Reader {

    public object Read(Package package) {
      return CryptoUtils.Decript(File.ReadAllText(Utils.Path(package.path, Const.QUESTION_FILENAME)));
    }

  }

}