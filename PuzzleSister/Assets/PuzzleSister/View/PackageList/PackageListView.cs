using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


namespace PuzzleSister {

	public class PackageListView : MonoBehaviour {

		[NotNull] public GameObject oLoading;
		[NotNull] public GameObject packageItemPrefab;
		[NotNull] public Sprite normalThumb;

		private bool destroyed = false;

		void OnDestroy() {
			DestroyList();
			destroyed = true;
		}

		public void InitList () {
			PackageProgressService.shared.Load();
			foreach(Transform tItem in transform) {
				Destroy(tItem.gameObject);
			}
			if (Repository.shared.IsPackagesLoaded) {
				InitPackageList();
			} else {
				Repository.shared.OnPackagesLoaded.AddListener(InitPackageList);
			}
		}

		public void DestroyList() {
			foreach(Transform tItem in transform) {
				Destroy(tItem.gameObject);
			}
		}

		void InitPackageList() {
			oLoading.SetActive(true);
			var packages = Repository.shared.GetAllPackages();
			oLoading.SetActive(false);
			for(int i=0; i<6; i++) {
				var item = Instantiate(packageItemPrefab, transform.position, Quaternion.identity);
				item.transform.SetParent(transform);
				item.transform.localScale = new Vector3(1, 1, 1);
				// item.SetActive(false);
				if (i < packages.Length) {
					AdaptItem(item, packages[i]);
				} else {
					item.transform.Find("Name").GetComponent<Text>().text = "获取DLC";
					item.transform.Find("Progress").gameObject.SetActive(false);
					item.GetComponent<Button>().onClick.AddListener(() => {
						var data = new PackageClickEventData();
						data.type = EventType.PackageItemClick;
						GlobalEvent.shared.Invoke(data);
					});
				}
				item.SetActive(false);
				CoroutineUtils.Start(AnimateShowItem(item, i * 0.05f));
			}
		}

		void AdaptItem(GameObject item, Package package) {
			item.transform.Find("Name").GetComponent<Text>().text = package.name;
			if (package.thumb == null || string.IsNullOrEmpty(package.thumb.Trim())) {
				item.transform.Find("Image").GetComponent<Image>().sprite = normalThumb;
			} else {
				item.transform.Find("Image").GetComponent<Image>().sprite = SpriteExtensions.Base64ToSprite(package.thumb);
			}
			item.GetComponent<Button>().onClick.AddListener(() => {
				var data = new PackageClickEventData();
				data.type = EventType.PackageItemClick;
				data.package = package;
				GlobalEvent.shared.Invoke(data);
			});
			
			if (package.temporary) {
				item.transform.Find("Progress").GetComponent<Text>().text = "来自编辑器";
			} else {
				var progress = PackageProgressService.shared.GetProgress(package.id);
				item.transform.Find("Progress").GetComponent<Text>().text = progress.Percentage();
			}
		}
		

		IEnumerator AnimateShowItem(GameObject item, float delay) {
			yield return new WaitForSeconds(delay);
			if (!destroyed && item != null) {
				item.SetActive(true);
			}
		}

	}

}
