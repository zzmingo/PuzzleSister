using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

namespace PuzzleSister.QEditor {

  public class QEditorService {

    public static readonly QEditorService shared = new QEditorService();

    public UnityEvent OnPackageChange = new UnityEvent();
    public UnityEvent OnQuestionChange = new UnityEvent();

    public bool packageDirty { get; private set; }
    public bool questionDirty { get; private set; }

    private List<PackageItem> packageList = new List<PackageItem>();
    private List<Question> questionList = new List<Question>();

    private PackageItem managingPackage;

    public void LoadPackages() {
      packageList = Storage.shared.DeserializeLoad(GetPackagesSavePath(), new List<PackageItem>());
    }

    public void SavePackages() {
      Storage.shared.SerializeSave(GetPackagesSavePath(), packageList);
      packageDirty = false;
    }

    public void AddPackage(PackageItem package) {
      package.id = Guid.NewGuid().ToString();
      packageList.Add(package);
      packageDirty = true;
      OnPackageChange.Invoke();
    }

    public PackageItem GetPackageById(string id) {
      return packageList.Find((item) => item.id.Equals(id));
    }

    public void RemovePackageById(string id) {
      var package = packageList.Find((item) => item.id.Equals(id));
      if (package == null) return;
      packageList.Remove(package);
      packageDirty = true;
      OnPackageChange.Invoke();
    }

    public void UpdatePackage(PackageItem package) {
      packageDirty = true;
      OnPackageChange.Invoke();
    }

    public List<PackageItem> GetAllPackages() {
      return packageList;
    }

    public PackageItem GetManagingPackage() {
      return managingPackage;
    }

    public void ManagePackage(PackageItem package) {
      questionDirty = false;
      managingPackage = package;
      questionList = Storage.shared.DeserializeLoad(GetQuestionsSavePath(), new List<Question>());
      OnQuestionChange.Invoke();
    }

    public List<Question> GetQuestions() {
      return questionList;
    }

    public void AddQuestion(Question question) {
      question.id = Guid.NewGuid().ToString();
      questionList.Add(question);
      questionDirty = true;
      OnQuestionChange.Invoke();
    }

    public Question GetQuestionById(string id) {
      return questionList.Find((item) => item.id.Equals(id));
    }

    public void RemoveQuestionById(string id) {
      Question question = questionList.Find((item) => item.id.Equals(id));
      questionList.Remove(question);
      questionDirty = true;
      OnQuestionChange.Invoke();
    }

    public void UpdateQuestion(Question question) {
      questionDirty = true;
      OnQuestionChange.Invoke();
    }

    public void SaveQuestions() {
      Storage.shared.SerializeSave(GetQuestionsSavePath(), questionList);
      questionDirty = false;
      OnQuestionChange.Invoke();
    }

    private string GetPackagesSavePath() {
      return Utils.Path(Const.QEDITOR_SAVE_DIR, Const.QEDITOR_PACKAGES_FILE);
    }

    private string GetQuestionsSavePath() {
      return Utils.Path(Const.QEDITOR_SAVE_DIR, managingPackage.id);
    }

    public class PackageItem {
      public string id;
      public string name;
      public string author;
      public string description;
      public string thumb;
    }
    

  }

}