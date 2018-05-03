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
    }

    IEnumerator LoadThumbAsync(Package package) {
			WWW www = new WWW(package.thumb);
			yield return www;
			transform.Query<Image>("Content/Thumb").sprite = SpriteExtensions.fromTexture(www.texture);
		}
    
    public void SetPackage(Package package) {
      this.package = package;
      var progress = PackageProgressService.shared.GetProgress(package.id);
      if(package.thumb.StartsWith("http") || package.thumb.StartsWith("https")) {
				StartCoroutine(LoadThumbAsync(package));
			} else {
        transform.Query<Image>("Content/Thumb").sprite = package.ThumbSprite;
      }
      var localizationManager = TinyLocalization.LocalizationManager.Instance;

      transform.Query<Text>("Content/Name").text = package.name;
      transform.Query<Text>("Content/Author").text = localizationManager.GetLocalizedText("作者") + "：" + (package.author == null ? "???" : package.author);
      transform.Query<Text>("Content/Description").text = 
        package.description == null ? "???" : package.description;

      if (package.state == Package.State.Ready) {
        transform.Query<Button>("Content/BtnContinue").interactable = true;
        transform.Query<Text>("Content/BtnContinue/Text").text = 
					progress.Completed ? localizationManager.GetLocalizedText("连答挑战") : string.Format(localizationManager.GetLocalizedText("继续答题") + "({0})", progress.Percentage());
      } else {
        transform.Query<Button>("Content/BtnContinue").interactable = false;
        transform.Query<Text>("Content/BtnContinue/Text").text = "Loading...";
      }
    }

  }

}