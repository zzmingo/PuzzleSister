using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


namespace PuzzleSister {

	public class PackageDialogView : MonoBehaviour {

    private Package package;

    public void Awake() {
      transform.Query<Button>("Content/BtnContinue").onClick.AddListener(() => {
        gameObject.SetActive(false);
        var data = new PackageClickEventData();
				data.type = EventType.PackageItemClick;
				data.package = package;
				GlobalEvent.shared.Invoke(data);
      });

      transform.Query<Button>("Content/BtnChanllenge").onClick.AddListener(() => {
        gameObject.SetActive(false);
        var data = new PackageClickEventData();
				data.type = EventType.PackageChanllengeClick;
				data.package = package;
				GlobalEvent.shared.Invoke(data);
      });
    }
    
    public void SetPackage(Package package) {
      this.package = package;
      transform.Query<Image>("Content/Thumb").sprite = package.ThumbSprite;
      transform.Query<Text>("Content/Name").text = package.name;
      transform.Query<Text>("Content/Author").text = "编辑：" + (package.author == null ? "未知" : package.author);
      transform.Query<Text>("Content/Description").text = 
        package.description == null ? string.Format("一个关于\"{0}\"的题库", package.name) : package.description;

      UpdateButtonChanllenge();
    }

    void UpdateButtonChanllenge() {
      var progress = PackageProgressService.shared.GetProgress(package.id);
      transform.Query<Button>("Content/BtnChanllenge").interactable = progress.Completed;
      transform.Query<Text>("Content/BtnChanllenge/Text").text = progress.Completed ? "连答挑战" : "100%后开启";
    }

  }

}