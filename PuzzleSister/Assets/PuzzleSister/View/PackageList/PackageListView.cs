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
			for(int i=0; i<12; i++) {
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
				StartCoroutine(AnimateShowItem(item, i * 0.05f));
			}
		}

		void AdaptItem(GameObject item, Package package) {
			var progress = PackageProgressService.shared.GetProgress(package.id);
			item.transform.Find("Progress/Text").GetComponent<Text>().text = progress.progress + "/" + progress.total;
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
		}

		IEnumerator AnimateShowItem(GameObject item, float delay) {
			yield return new WaitForSeconds(delay);
			item.SetActive(true);
		}

	}

}
