using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

namespace PuzzleSister {

  public class Settings {

    public enum Key {
      Music, Sound, Voice, VoiceStyle, Resolution, Fullscreen, QuestionLang
    }
    
    public const string DEFAULT_RESOLUTION = "800x600 (60hz)";
		public const string DEFAULT_QUESTION_LANG = "简体中文";

    public const string MUSIC = "settings.music";
    public const string SOUND = "settings.sound";
    public const string VOICE = "settings.voice";
    public const string VOICE_STYLE = "settings.voice.style";
    public const string RESOLUTION = "settings.resolution";
    public const string FULLSCREEN = "settings.fullscreen";
		public const string QUESTION_LANG = "settings.question.lang";

    public sealed class SettingChangeEvent : UnityEvent<string> {}

    public static readonly SettingChangeEvent OnChange = new SettingChangeEvent();

    public static Resolution[] GetAvailableResolutions() {
      return Screen.resolutions;
    }

    public static bool IsFullscreen() {
      return GetInt(FULLSCREEN, 0) == 1;
    }

    public static Resolution ParseResolution(string resolution) {
      if (!resolution.Contains("hz")) {
        resolution += " (60hz)";
      }
      string[] split = resolution.Split(' ');
      string[] size = split[0].Split('x');
      Resolution resol = new Resolution();
      resol.width = int.Parse(size[0]);
      resol.height = int.Parse(size[1]);
      resol.refreshRate = int.Parse(split[1].Replace("(", "").Replace(")", "").Replace("hz", ""));
      return resol;
    }

		public static List<string> QuestionLangs() {
			return new List<string>(GetString(QUESTION_LANG, DEFAULT_QUESTION_LANG).Split(','));
		}

		public static void SaveQuestionLangs(List<string> langs) {
			SetString(QUESTION_LANG, string.Join(",", langs.ToArray()));
		}

    public static string GetVoiceStyle(string defaults) {
      return Settings.GetString(VOICE_STYLE, defaults);
    }

    public static int GetInt(string key, int defaults = 0) {
      if (!PlayerPrefs.HasKey(key)) {
        return defaults;
      }
      return PlayerPrefs.GetInt(key);
    }

    public static float GetFloat(string key, float defaults = 0) {
      if (!PlayerPrefs.HasKey(key)) {
        return defaults;
      }
      return PlayerPrefs.GetFloat(key);
    }

    public static string GetString(string key, string defaults = "") {
      if (!PlayerPrefs.HasKey(key)) {
        return defaults;
      }
      return PlayerPrefs.GetString(key);
    }

    public static void SetInt(string key, int value) {
      PlayerPrefs.SetInt(key, value);
      PlayerPrefs.Save();
      OnChange.Invoke(key);
    }

    public static void SetFloat(string key, float value) {
      PlayerPrefs.SetFloat(key, value);
      PlayerPrefs.Save();
      OnChange.Invoke(key);
    }

    public static void SetString(string key, string value) {
      PlayerPrefs.SetString(key, value);
      PlayerPrefs.Save();
      OnChange.Invoke(key);
    }

  }

  public static class SettingKeyExtensions {

    public static string Strings(this Settings.Key key) {
      switch(key) {
        case Settings.Key.Music: return Settings.MUSIC;
        case Settings.Key.Sound: return Settings.SOUND;
        case Settings.Key.Voice: return Settings.VOICE;
        case Settings.Key.VoiceStyle: return Settings.VOICE_STYLE;
        case Settings.Key.Resolution: return Settings.RESOLUTION;
				case Settings.Key.QuestionLang: return Settings.QUESTION_LANG;
        default: throw new UnityException();
      }
    }

  }

}