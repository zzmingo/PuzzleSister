using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using PuzzleSister.UGCEditor;
using Steamworks;


namespace PuzzleSister {

	public class PackageListView : MonoBehaviour {

		[NotNull] public GameObject oLoading;
		[NotNull] public GameObject packageItemPrefab;
		[NotNull] public Sprite normalThumb;

		[NotNull] public PackageDialogView dialogueView;

		private bool destroyed = false;
		private Coroutine loadPackageCoroutine;

		void Awake() {
			dialogueView.gameObject.SetActive(false);
		}

		void OnEnable() {
			Debug.Log("Enable");
			loadPackageCoroutine = StartCoroutine(LoadPackageList());
		}
		
		IEnumerator LoadPackageList() {
			oLoading.SetActive(true);

			if (GameState.isShowBuiltins) {
				Repository.shared.LoadBuildtins();
				Package[] packages = Repository.shared.GetBuiltinPackages(Settings.GetString (Settings.PACKAGE_LANGUAGE, Settings.SupportLanguageCodes ()));
				InitPackageList(packages);
			} else {
				EResult loadResult = EResult.k_EResultOK;
				yield return UGCService.shared.LoadSubscribed(result => loadResult = result);
				loadPackageCoroutine = null;
				Repository.shared.LoadPackages();
				foreach(Transform tItem in transform) {
					Destroy(tItem.gameObject);
				}
				Package[] packages = Repository.shared.GetUGCPackages(Settings.GetString(Settings.PACKAGE_LANGUAGE, Settings.SupportLanguageCodes()));
				InitPackageList(packages);
			}
		}

		void OnDisable() {
			if (loadPackageCoroutine != null) {
				StopCoroutine(loadPackageCoroutine);
			}
			DestroyList();
		}

		void OnDestroy() {
			DestroyList();
			destroyed = true;
		}

		public void DestroyList() {
			foreach(Transform tItem in transform) {
				Destroy(tItem.gameObject);
			}
		}

		void InitPackageList(Package[] packages) {
			oLoading.SetActive(false);
			for(int i=0,len = packages.Length; i<len; i++) {
				var item = Instantiate(packageItemPrefab, transform.position, Quaternion.identity);
				item.transform.SetParent(transform);
				item.transform.localScale = new Vector3(1, 1, 1);
				// item.SetActive(false);
				if (i < packages.Length) {
					AdaptItem(item, packages[i]);
				}
				item.SetActive(false);
				CoroutineUtils.Start(AnimateShowItem(item, i * 0.05f));
			}
			var dlcItem = Instantiate(packageItemPrefab, transform.position, Quaternion.identity);
			dlcItem.transform.SetParent(transform);
			dlcItem.transform.localScale = new Vector3(1, 1, 1);
			dlcItem.transform.Find("Name").GetComponent<Text>().text = "获取DLC";
			dlcItem.transform.Find("Progress").gameObject.SetActive(false);
			dlcItem.GetComponent<Button>().onClick.AddListener(() => {
				var data = new PackageClickEventData();
				data.type = EventType.PackageItemClick;
				GlobalEvent.shared.Invoke(data);
			});
		}

		void AdaptItem(GameObject item, Package package) {
			item.transform.Find("Name").GetComponent<Text>().text = package.name;
			if (package.thumb == null || string.IsNullOrEmpty(package.thumb.Trim())) {
				item.transform.Find("Image").GetComponent<Image>().sprite = normalThumb;
			} else if(package.thumb.StartsWith("http") || package.thumb.StartsWith("https")) {
				StartCoroutine(LoadThumbAsync(item, package));
			} else {
				item.transform.Find("Image").GetComponent<Image>().sprite = package.ThumbSprite;
			}
			item.GetComponent<Button>().onClick.AddListener(() => {
				UIController.singleton.PushPopup(dialogueView.gameObject);
				dialogueView.SetPackage(package);
			});
			
			if (package.temporary) {
				item.transform.Find("Progress").GetComponent<Text>().text = "来自编辑器";
			} else {
				var progress = PackageProgressService.shared.GetProgress(package.id);
				item.transform.Find("Progress").GetComponent<Text>().text = progress.Percentage();
			}
		}
		
		IEnumerator LoadThumbAsync(GameObject item, Package package) {
			WWW www = new WWW(package.thumb);
			yield return www;
			item.transform.Find("Image").GetComponent<Image>().sprite = SpriteExtensions.fromTexture(www.texture);
		}

		IEnumerator AnimateShowItem(GameObject item, float delay) {
			yield return new WaitForSeconds(delay);
			if (!destroyed && item != null) {
				item.SetActive(true);
			}
		}

	}

}
