using UnityEngine;

namespace PuzzleSister {

  public class VoiceSuite : ScriptableObject {

    private static string currentName;
    private static VoiceSuite currentSuite;

    public static VoiceSuite LoadBySetting() {
      string styleName = Settings.GetVoiceStyle(suites[0].name);
      if (styleName.Equals(currentName)) {
        return currentSuite;
      }
      if (currentSuite != null) {
        Destroy(currentSuite);
      }
      foreach(var desc in suites) {
        if (desc.name.Equals(styleName)) {
          currentName = desc.name;
          currentSuite = Resources.Load<VoiceSuite>(desc.path);
          return currentSuite;
        }
      }
      return null;
    }

    public static readonly Descriptor[] suites = new Descriptor[] {
      new Descriptor("默认", "VoiceSuites/PuzzleSisters")
    };

    public class Descriptor {
      public readonly string name;
      public readonly string path;
      internal Descriptor(string name, string path) {
        this.name = name;
        this.path = path;
      }
    }

    public AudioClip[] XYClips;

    public AudioClip[] X0Clips;
    public AudioClip[] X1Clips;
    public AudioClip[] X2Clips;
    public AudioClip[] X3Clips;
    public AudioClip[] X4Clips;
    public AudioClip[] X5Clips;

    public AudioClip[] Y1Clips;
    public AudioClip[] Y2Clips;
    public AudioClip[] Y3Clips;
    public AudioClip[] Y4Clips;
    public AudioClip[] Y5Clips;
    public AudioClip[] Y6Clips;
    public AudioClip[] Y7Clips;
    public AudioClip[] Y8Clips;
    public AudioClip[] Y9Clips;
    public AudioClip[] Y10Clips;

  }

}