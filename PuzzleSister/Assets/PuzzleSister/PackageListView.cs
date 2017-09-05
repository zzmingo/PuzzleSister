using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


namespace PuzzleSister {

	public class PackageListView : MonoBehaviour {

		public class ItemClickEvent : UnityEvent<Package> {}

		public readonly ItemClickEvent OnItemClick = new ItemClickEvent();

		public GameObject packageItemPrefab;

		void Start () {
			if (Repository.shared.IsPackagesLoaded) {
				InitPackageList();
			} else {
				Repository.shared.OnPackagesLoaded.AddListener(InitPackageList);
			}
		}

		void InitPackageList() {
			Debug.Log("init");
			var packages = Repository.shared.GetAllPackages();
			for(int i=0; i<12; i++) {
				var item = Instantiate(packageItemPrefab, transform.position, Quaternion.identity);
				item.transform.SetParent(transform);
				item.transform.localScale = new Vector3(1, 1, 1);
				if (i < packages.Length) {
					AdaptItem(item, packages[i]);
				}
			}
		}

		void AdaptItem(GameObject item, Package package) {
			item.transform.Find("Name").GetComponent<Text>().text = package.name;
			item.GetComponent<Button>().onClick.AddListener(() => {
				OnItemClick.Invoke(package);
			});
		}

	}

}
