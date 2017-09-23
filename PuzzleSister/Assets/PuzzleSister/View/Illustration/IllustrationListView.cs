using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


namespace PuzzleSister {

	public class IllustrationListView : MonoBehaviour {

		[NotNull] public GameObject illustrationItemPrefab;
		[NotNull] public IllustrationSettings settings;

		public void Start() {
			InitList();
		}

		public void InitList () {
			foreach(var itemData in settings.items) {
				var item = Instantiate(illustrationItemPrefab, Vector3.zero, Quaternion.identity, transform);
				AdaptItem(item, itemData);
			}
		}

		void AdaptItem(GameObject item, IllustrationItem itemData) {
			item.Query<Image>("Image").sprite = itemData.image;
			item.Query<Text>("Name").text = itemData.name;
		}

	}

}
