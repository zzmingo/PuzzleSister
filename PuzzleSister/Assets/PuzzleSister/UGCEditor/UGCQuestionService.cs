using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using PuzzleSister;
using System.Text;
#if UNITY_STANDALONE
using Steamworks;
#endif

namespace PuzzleSister.UGCEditor {

  public class UGCQuestionService {

    public static UGCQuestionService shared = new UGCQuestionService();

    private PackageItem package;
    private List<Question> questionList = new List<Question>();
    
    private Callback<DownloadItemResult_t> downloadItemCallback;

    public PackageItem GetPackage() {
      return package;
    }

    public List<Question> GetQuestionList() {
      return new List<Question>(questionList);
    }

    public void AddQuestionList(List<Question> newList) {
      foreach(var question in newList) {
        // 针对名字去重
        bool repeat = false;
        foreach(var origin in this.questionList) {
          repeat = origin.title.Equals(question.title);
          if (repeat) {
            break;
          }
        }
        if (!repeat) {
          question.id = System.Guid.NewGuid().ToString();
          this.questionList.Add(question);
        }
      }
      Save();
    }

    public void AddQuestion(Question question) {
      question.id = System.Guid.NewGuid().ToString();
      questionList.Add(question);
      Save();
    }

    public void UpdateQuestion(Question question) {
      for(int i=0; i<questionList.Count; i++) {
        var current = questionList[i];
        if (current.id == question.id) {
          questionList[i] = question;
          break;
        }
      }
      Save();
    }

    public void RemoveQuestion(Question question) {
      for(int i=0; i<questionList.Count; i++) {
        var current = questionList[i];
        if (current.id == question.id) {
          questionList.RemoveAt(i);
          break;
        }
      }
      Save();
    }

    public IEnumerator EditPackage(PackageItem package, Action<EResult> onError) {
      bool subscribed = false;
      var subscribeCallResult = CallResult<RemoteStorageSubscribePublishedFileResult_t>.Create((callback, error) => {
        if (error) {
          onError(EResult.k_EResultIOFailure);
          return;
        }
        subscribed = true;
      });
      var subscribeHandle = SteamUGC.SubscribeItem(package.publishedFileId);
      subscribeCallResult.Set(subscribeHandle);

      while (!subscribed) {
        yield return null;
      }

      uint itemState = SteamUGC.GetItemState(package.publishedFileId);
      Debug.LogFormat("item state {0}", itemState);

      bool downloaded = (itemState & (uint)EItemState.k_EItemStateInstalled) == (uint)EItemState.k_EItemStateInstalled;
      bool updateRequired = (itemState & (uint)EItemState.k_EItemStateNeedsUpdate) == (uint)EItemState.k_EItemStateNeedsUpdate;
      
      string folder = null;

      if (downloaded && !updateRequired) {
        Debug.Log("not need update");
        // read data
        ulong size;
        uint timestamp;
        SteamUGC.GetItemInstallInfo(package.publishedFileId, out size, out folder, 1024, out timestamp);

        Debug.LogFormat("install folder {0}", folder);
      } else {
        Debug.LogFormat("downloading {0}", package.name);

        bool completed = false;
        EResult result = EResult.k_EResultOK;

        this.package = package;
        this.questionList.Clear();

        if (downloadItemCallback != null) {
          downloadItemCallback.Dispose();
        }
        downloadItemCallback = Callback<DownloadItemResult_t>.Create((callback) => {
          if (callback.m_unAppID.m_AppId == Const.STEAM_APP_ID && callback.m_nPublishedFileId == package.publishedFileId) {
            completed = true;
            result = callback.m_eResult;
          }
        });

        SteamUGC.DownloadItem(package.publishedFileId, true);

        while(!completed) {
          yield return null;
        }

        Debug.LogFormat("downloaded {0}", package.name);

        if (result != EResult.k_EResultOK) {
          onError(result);
        } else {
          
          // read data
          ulong size;
          uint timestamp;
          SteamUGC.GetItemInstallInfo(package.publishedFileId, out size, out folder, 1024, out timestamp);

          Debug.LogFormat("download folder {0}", folder);
        }
      }

      while(folder == null) {
        yield return null;
      }

      string contentDir = Utils.Path(Utils.GetAppInstallDir(), Const.UGC_CONTENT_DIR, package.id);
      CopyFiles(folder, contentDir);

      string questionPath = Utils.Path(contentDir, Const.QUESTION_FILENAME);
      questionList = Storage.shared.DeserializeLoad(questionPath, new List<Question>());

      this.package = package;
    }

    private void Save() {
      string contentDir = Utils.Path(Utils.GetAppInstallDir(), Const.UGC_CONTENT_DIR, package.id);
      if (!Directory.Exists(contentDir)) {
        Directory.CreateDirectory(contentDir);
      }
      string questionPath = Utils.Path(contentDir, Const.QUESTION_FILENAME);
      Storage.shared.SerializeSave(questionPath, questionList);
    }

    private void CopyFiles(string sourceDir, string targetDir) {
      if (!Directory.Exists(targetDir)) {
        Directory.CreateDirectory(targetDir);
      }

      foreach(var file in Directory.GetFiles(sourceDir)) {
        string targetFile = Path.Combine(targetDir, Path.GetFileName(file));
        if (!File.Exists(targetFile)) {
          File.Copy(file, targetFile, false);
        }
      }

      foreach(var directory in Directory.GetDirectories(sourceDir)) {
        CopyFiles(directory, Path.Combine(targetDir, Path.GetFileName(directory)));
      }
    }

  }
  
}