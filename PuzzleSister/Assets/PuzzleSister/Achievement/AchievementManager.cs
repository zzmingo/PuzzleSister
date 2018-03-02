using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Steamworks;
using PuzzleSister.UGCEditor;

namespace PuzzleSister {
	public class AchievementManager : MonoBehaviour {

		private static AchievementManager instance;
		public static AchievementManager Instance {
			get {
				if (instance == null) {
					return new GameObject("AchievementManager").AddComponent<AchievementManager>();
				} else {
					return instance;
				}
			}
		}

		private bool initedUserStats = false;
		private bool receivedUserStats = false;
		private bool needToStore = false;
		private bool checkedStartAchievement = false;
		private bool checkedCompletePackageAchievement = false;
		private bool checkedPublishUGCPackageAchievement = false;
		private bool initedUGCPackages = false;
		private Callback<UserStatsStored_t> userStatsStoredCallback;
		private Callback<UserAchievementStored_t> userAchievementStoredCallback;
		private Callback<UserStatsReceived_t> userStatsReceivedCallback;

		// Use this for initialization
		void Start() {
			if (instance != null) {
				Destroy(gameObject);
				return;
			}

			instance = this;
			userStatsStoredCallback = Callback<UserStatsStored_t>.Create(onUserStatsStored);
			userAchievementStoredCallback = Callback<UserAchievementStored_t>.Create(onUserAchievementStored);
			userStatsReceivedCallback = Callback<UserStatsReceived_t>.Create(OnUserStatsReceived);

			DontDestroyOnLoad(gameObject);
		}

		void Update() {
			if (!SteamManager.Initialized) {
				return;
			}
			if (!initedUserStats) {
				initedUserStats = SteamUserStats.RequestCurrentStats();
			}
			if (receivedUserStats) {
				if (!checkedStartAchievement) {
					checkStartAchievement ();
				}
				if (!checkedCompletePackageAchievement) {
					checkCompletePackageAchievement();
				}
				if (!checkedPublishUGCPackageAchievement) {
					checkPublishUGCPackageAchievement();
				}
			}
			if (receivedUserStats && needToStore) {
				Debug.Log("store user stats");
				needToStore = false;
				bool result = SteamUserStats.StoreStats();
				Debug.Log("store user stats result: " + result);
			}
		}

		void OnDestroy() {
			if (instance != this) {
				return;
			}

			instance = null;
		}

		private void onUserStatsStored(UserStatsStored_t pCallback) {
			if ((ulong)Const.STEAM_APP_ID == pCallback.m_nGameID) {
				if (EResult.k_EResultOK != pCallback.m_eResult) {
					needToStore = true;
				} else {
					Debug.Log("store user stats success");
				}
			}
		}

		private void onUserAchievementStored(UserAchievementStored_t pCallback) {
			if ((ulong)Const.STEAM_APP_ID == pCallback.m_nGameID) {
				if (0 == pCallback.m_nMaxProgress) {
					baseAchievements[(int)Enum.Parse (typeof(BaseAchievementEnum), pCallback.m_rgchAchievementName)].beenAchieved = true;
					Debug.Log ("store user achievement(" + pCallback.m_rgchAchievementName + ") success");
				}
			}
		}

		private void OnUserStatsReceived(UserStatsReceived_t pCallback) {
			receivedUserStats = true;
			if ((ulong)Const.STEAM_APP_ID == pCallback.m_nGameID) {
				if (EResult.k_EResultOK == pCallback.m_eResult) {
					foreach (Achievement achievement in baseAchievements) {
						string id = achievement.id.ToString ();
						bool result = SteamUserStats.GetAchievement (id, out achievement.beenAchieved);
						if (result) {
							achievement.name = SteamUserStats.GetAchievementDisplayAttribute (id, "name");
							achievement.description = SteamUserStats.GetAchievementDisplayAttribute (id, "desc");
						} else {
							Debug.LogWarning("SteamUserStats.GetAchievement failed for Achievement " + achievement.id + "\nIs it registered in the Steam Partner site?");
						}
					}
					for (int i = 1; i < 2000; i++) {
						string id = "A" + i;
						Achievement achievement = new Achievement(id, "", "");

					}
				}else {
					Debug.Log("RequestStats - failed, " + pCallback.m_eResult);
				}
			}
		}

		void unlockAchievement(BaseAchievementEnum aEnum) {
			if (!SteamManager.Initialized) {
				return;
			}
			foreach(Achievement achievement in baseAchievements) {
				if (achievement.id.Equals(aEnum) && !achievement.beenAchieved) {
					needToStore = SteamUserStats.SetAchievement(achievement.id.ToString());
					break;
				}
			}
		}

		private void checkStartAchievement() {
			checkedStartAchievement = true;
			if (!baseAchievements[(int)BaseAchievementEnum.BASE_12].beenAchieved) {
				unlockAchievement(BaseAchievementEnum.BASE_12);
			}
		}

		private void checkCompletePackageAchievement() {
			if (!PackageProgressService.shared.loaded && !Repository.shared.IsPackagesLoaded) {
				return;
			}
			checkedCompletePackageAchievement = true;
		}

		private void checkPublishUGCPackageAchievement() {
			if (!UGCService.shared.Loaded) {
				if (!initedUGCPackages) {
					initedUGCPackages = true;
					StartCoroutine(UGCService.shared.LoadPackages((error) => {
						if (error != EResult.k_EResultOK) {
							initedUGCPackages = false;
						}
					}));
				}
				return;
			}
			checkedPublishUGCPackageAchievement = true;
			List<PackageItem> packageItems = UGCService.shared.GetAllPackages();
			int p100Count = 0;
			int pCount = 0;
			foreach(PackageItem package in packageItems) {
				uint state = SteamUGC.GetItemState(package.publishedFileId);
				bool downloaded = (state & (uint)EItemState.k_EItemStateInstalled) == (uint)EItemState.k_EItemStateInstalled;
				bool updateRequired = (state & (uint)EItemState.k_EItemStateNeedsUpdate) == (uint)EItemState.k_EItemStateNeedsUpdate;
				var pState = (downloaded && !updateRequired) ? Package.State.Ready : Package.State.Prepare;
				if (pState == Package.State.Ready) {
					pCount++;
					if (package.questionCount == 100) {
						p100Count++;
					}
				}
			}
			do {
				if (pCount < 1) {
					break;
				}
				unlockAchievement(BaseAchievementEnum.BASE_2);
				if (p100Count < 1) {
					break;
				}
				unlockAchievement(BaseAchievementEnum.BASE_3);
				if (p100Count < 2) {
					break;
				}
				unlockAchievement(BaseAchievementEnum.BASE_4);
				if (p100Count < 3) {
					break;
				}
				unlockAchievement(BaseAchievementEnum.BASE_5);
			} while (false);
		}

		private class Achievement {
			public BaseAchievementEnum id;
			public string name;
			public string description;
			public bool beenAchieved;

			/// <summary>
			/// Creates an Achievement. You must also mirror the data provided here in https://partner.steamgames.com/apps/achievements/yourappid
			/// </summary>
			/// <param name="achievement">The "API Name Progress Stat" used to uniquely identify the achievement.</param>
			/// <param name="name">The "Display Name" that will be shown to players in game and on the Steam Community.</param>
			/// <param name="desc">The "Description" that will be shown to players in game and on the Steam Community.</param>
			public Achievement(BaseAchievementEnum achievementID, string name, string desc) {
				id = achievementID;
				this.name = name;
				description = desc;
				beenAchieved = false;
			}
		}

		private Achievement[] baseAchievements = new Achievement[]{
			new Achievement(BaseAchievementEnum.BASE_0, "轻轻松松", ""),
			new Achievement(BaseAchievementEnum.BASE_1, "小菜一碟", ""),
			new Achievement(BaseAchievementEnum.BASE_2, "会打字就能参与制作游戏！", ""),
			new Achievement(BaseAchievementEnum.BASE_3, "一起来玩啊！", ""),
			new Achievement(BaseAchievementEnum.BASE_4, "毛遂自荐美滋滋", ""),
			new Achievement(BaseAchievementEnum.BASE_5, "我也是制作人！", ""),
			new Achievement(BaseAchievementEnum.BASE_6, "免费获取新知识", ""),
			new Achievement(BaseAchievementEnum.BASE_7, "精挑细选", ""),
			new Achievement(BaseAchievementEnum.BASE_8, "新姿势GET!", ""),
			new Achievement(BaseAchievementEnum.BASE_9, "集思广益", ""),
			new Achievement(BaseAchievementEnum.BASE_10, "知识收藏家", ""),
			new Achievement(BaseAchievementEnum.BASE_11, "脑子短路", ""),
			new Achievement(BaseAchievementEnum.BASE_12, "充电开始", ""),
			new Achievement(BaseAchievementEnum.BASE_13, "博学家", ""),
		};

		private Achievement[] completePackageAchievements = new Achievement[]{};
	}
}
