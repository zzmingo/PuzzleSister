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
		public bool checkedCompletePackageAchievement = false;
		public bool checkedPublishUGCPackageAchievement = false;
		private bool initedUGCPackages = false;
		private Callback<UserStatsStored_t> userStatsStoredCallback;
		private Callback<UserAchievementStored_t> userAchievementStoredCallback;
		private Callback<UserStatsReceived_t> userStatsReceivedCallback;

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
					checkStartAchievement();
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
					string achiName = pCallback.m_rgchAchievementName;
					if (Enum.IsDefined(typeof(BaseAchievementEnum), achiName)) {
						baseAchievements[(int)Enum.Parse (typeof(BaseAchievementEnum), achiName)].beenAchieved = true;
					} else if (Enum.IsDefined(typeof(AAchievementEnum), achiName)) {
						aAchievements[(int)Enum.Parse(typeof(AAchievementEnum), achiName)].beenAchieved = true;
					}
					Debug.Log ("store user achievement(" + pCallback.m_rgchAchievementName + ") success");
				}
			}
		}

		private bool initAchivementInfo<T> (Achievement<T> achievement) {
			string id = achievement.id.ToString();
			bool result = SteamUserStats.GetAchievement(id, out achievement.beenAchieved);
			if (result) {
				achievement.name = SteamUserStats.GetAchievementDisplayAttribute(id, "name");
				achievement.description = SteamUserStats.GetAchievementDisplayAttribute(id, "desc");
			} else {
				Debug.LogWarning("SteamUserStats.GetAchievement failed for Achievement " + achievement.id + "\nIs it registered in the Steam Partner site?");
			}
			return result;
		}

		private void OnUserStatsReceived(UserStatsReceived_t pCallback) {
			receivedUserStats = true;
			if ((ulong)Const.STEAM_APP_ID == pCallback.m_nGameID) {
				if (EResult.k_EResultOK == pCallback.m_eResult) {
					foreach (Achievement<BaseAchievementEnum> achievement in baseAchievements) {
						initAchivementInfo(achievement);
					}
					for (int i = 1, len = (int)AAchievementEnum.MAX; i < len; i++) {
						string id = "A" + i;
						Achievement<AAchievementEnum> achievement = new Achievement<AAchievementEnum>((AAchievementEnum)Enum.Parse(typeof(AAchievementEnum), id), "", "");
						if (!initAchivementInfo(achievement)) {
							break;
						}
						aAchievements.Add(achievement);
					}
					Debug.Log("aAchievements Count: " + aAchievements.Count);
				}else {
					Debug.Log("RequestStats - failed, " + pCallback.m_eResult);
				}
			}
		}

		public void unlockAchievement(Enum aEnum) {
			if (!SteamManager.Initialized) {
				return;
			}
			Type tType = aEnum.GetType();
			if (tType.Name.Equals("BaseAchievementEnum")) {
				foreach(var achievement in baseAchievements) {
					if (achievement.id.Equals(aEnum) && !achievement.beenAchieved) {
						needToStore = SteamUserStats.SetAchievement(achievement.id.ToString());
						break;
					}
				}
			} else if (tType.Name.Equals("AAchievementEnum") && Convert.ToInt32(aEnum) < aAchievements.Count) {
				foreach(var achievement in aAchievements) {
					if (achievement.id.Equals(aEnum) && !achievement.beenAchieved) {
						needToStore = SteamUserStats.SetAchievement(achievement.id.ToString());
						break;
					}
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
			int completedCount = 0;
			Package[] packages = Repository.shared.GetAllPackages();
			foreach(Package package in packages) {
				if (PackageProgressService.shared.GetProgress(package.id).Completed) {
					completedCount++;
				}
			}
			int index = 0, len = aAchievements.Count, last = completedCount * 10;
			if (last > len) {
				last = len;
			}
			for (;index < len;index++) {
				if (!aAchievements[index].beenAchieved) {
					break;
				}
			}
			if (index < len) {
				for (int i = index; i < last; i++) {
					unlockAchievement(aAchievements[i].id);
				}
			}
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

		private class Achievement<T> {
			public T id;
			public string name;
			public string description;
			public bool beenAchieved;

			/// <summary>
			/// Creates an Achievement. You must also mirror the data provided here in https://partner.steamgames.com/apps/achievements/yourappid
			/// </summary>
			/// <param name="achievement">The "API Name Progress Stat" used to uniquely identify the achievement.</param>
			/// <param name="name">The "Display Name" that will be shown to players in game and on the Steam Community.</param>
			/// <param name="desc">The "Description" that will be shown to players in game and on the Steam Community.</param>
			public Achievement(T achievementID, string name, string desc) {
				id = achievementID;
				this.name = name;
				description = desc;
				beenAchieved = false;
			}
		}

		private Achievement<BaseAchievementEnum>[] baseAchievements = new Achievement<BaseAchievementEnum>[]{
			new Achievement<BaseAchievementEnum>(BaseAchievementEnum.BASE_0, "轻轻松松", ""),
			new Achievement<BaseAchievementEnum>(BaseAchievementEnum.BASE_1, "小菜一碟", ""),
			new Achievement<BaseAchievementEnum>(BaseAchievementEnum.BASE_2, "会打字就能参与制作游戏！", ""),
			new Achievement<BaseAchievementEnum>(BaseAchievementEnum.BASE_3, "一起来玩啊！", ""),
			new Achievement<BaseAchievementEnum>(BaseAchievementEnum.BASE_4, "毛遂自荐美滋滋", ""),
			new Achievement<BaseAchievementEnum>(BaseAchievementEnum.BASE_5, "我也是制作人！", ""),
			new Achievement<BaseAchievementEnum>(BaseAchievementEnum.BASE_6, "免费获取新知识", ""),
			new Achievement<BaseAchievementEnum>(BaseAchievementEnum.BASE_7, "精挑细选", ""),
			new Achievement<BaseAchievementEnum>(BaseAchievementEnum.BASE_8, "新姿势GET!", ""),
			new Achievement<BaseAchievementEnum>(BaseAchievementEnum.BASE_9, "集思广益", ""),
			new Achievement<BaseAchievementEnum>(BaseAchievementEnum.BASE_10, "知识收藏家", ""),
			new Achievement<BaseAchievementEnum>(BaseAchievementEnum.BASE_11, "脑子短路", ""),
			new Achievement<BaseAchievementEnum>(BaseAchievementEnum.BASE_12, "充电开始", ""),
			new Achievement<BaseAchievementEnum>(BaseAchievementEnum.BASE_13, "博学家", ""),
		};

		private List<Achievement<AAchievementEnum>> aAchievements = new List<Achievement<AAchievementEnum>>();
	}
}
