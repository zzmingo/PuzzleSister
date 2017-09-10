﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


namespace PuzzleSister {

	public class PackageListView : MonoBehaviour {

		public GameObject packageItemPrefab;

		public void InitList () {
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
			var packages = Repository.shared.GetAllPackages();
			for(int i=0; i<12; i++) {
				var item = Instantiate(packageItemPrefab, transform.position, Quaternion.identity);
				item.transform.SetParent(transform);
				item.transform.localScale = new Vector3(1, 1, 1);
				if (i < packages.Length) {
					AdaptItem(item, packages[i]);
				} else {
					item.transform.Find("Name").GetComponent<Text>().text = "获取DLC";
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
			item.transform.Find("Name").GetComponent<Text>().text = package.name;
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
