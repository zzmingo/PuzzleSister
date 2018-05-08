using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


namespace PuzzleSister {

	public class IllustrationListView : MonoBehaviour {

		[NotNull] public GameObject illustrationItemPrefab;
		[NotNull] public Sprite thumbPlaceholder;
		[NotNull] public IllustrationSettings settings;
		[NotNull] public Image bigImage;
		[NotNull] public GameObject exitBtn;

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
			itemData.rewarded = IllustrationService.shared.IsRewarded(itemData);
			item.Query<Image>("Image").sprite = itemData.rewarded ? itemData.image : thumbPlaceholder;
			item.Query<Text>("Name").text = itemData.rewarded ? "已解锁" : itemData.name;
			item.GetComponent<Button>().onClick.AddListener(() => {
				if (itemData.rewarded) {
					ShowIllustration(itemData);
				}
			});
		}

		void ShowIllustration(IllustrationItem itemData) {
			exitBtn.SetActive(false);
			bigImage.sprite = itemData.image;
			bigImage.gameObject.SetActive(true);
			bigImage.gameObject.ScaleFrom(Vector3.zero, 0.4f, 0f);
		}

		public void OnDisable() {
			bigImage.gameObject.SetActive(false);
		}
	}

}
