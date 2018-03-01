using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Steamworks;
using PuzzleSister.UGCEditor;

namespace PuzzleSister {
	public class AchievementManager : MonoBehaviour {

		public static AchievementManager instance;

		private Achievement[] achievements = new Achievement[]{
			new Achievement(AchievementEnum.NEW_ACHIEVEMENT_1_0, "轻轻松松", ""),
			new Achievement(AchievementEnum.NEW_ACHIEVEMENT_1_1, "小菜一碟", ""),
			new Achievement(AchievementEnum.NEW_ACHIEVEMENT_1_2, "会打字就能参与制作游戏！", ""),
			new Achievement(AchievementEnum.NEW_ACHIEVEMENT_1_3, "一起来玩啊！", ""),
			new Achievement(AchievementEnum.NEW_ACHIEVEMENT_1_4, "毛遂自荐美滋滋", ""),
			new Achievement(AchievementEnum.NEW_ACHIEVEMENT_1_5, "我也是制作人！", ""),
			new Achievement(AchievementEnum.NEW_ACHIEVEMENT_1_6, "小试牛刀", ""),
			new Achievement(AchievementEnum.NEW_ACHIEVEMENT_1_7, "马马虎虎", ""),
			new Achievement(AchievementEnum.NEW_ACHIEVEMENT_1_8, "渐入佳境", ""),
			new Achievement(AchievementEnum.NEW_ACHIEVEMENT_1_9, "颇有见地", ""),
			new Achievement(AchievementEnum.NEW_ACHIEVEMENT_1_10, "涉猎广泛", ""),
			new Achievement(AchievementEnum.NEW_ACHIEVEMENT_1_11, "足智多谋", ""),
			new Achievement(AchievementEnum.NEW_ACHIEVEMENT_1_12, "才华横溢", ""),
			new Achievement(AchievementEnum.NEW_ACHIEVEMENT_1_13, "聪明绝顶", ""),
			new Achievement(AchievementEnum.NEW_ACHIEVEMENT_1_14, "免费获取新知识", ""),
			new Achievement(AchievementEnum.NEW_ACHIEVEMENT_1_15, "精挑细选", ""),
			new Achievement(AchievementEnum.NEW_ACHIEVEMENT_1_16, "新姿势GET!", ""),
			new Achievement(AchievementEnum.NEW_ACHIEVEMENT_1_17, "集思广益", ""),
			new Achievement(AchievementEnum.NEW_ACHIEVEMENT_1_18, "知识收藏家", ""),
			new Achievement(AchievementEnum.NEW_ACHIEVEMENT_1_19, "脑子短路", ""),
			new Achievement(AchievementEnum.NEW_ACHIEVEMENT_1_20, "充电开始", ""),
			new Achievement(AchievementEnum.NEW_ACHIEVEMENT_1_21, "无所不能", ""),
			new Achievement(AchievementEnum.NEW_ACHIEVEMENT_1_22, "博大精深", ""),
			new Achievement(AchievementEnum.NEW_ACHIEVEMENT_1_23, "全知全能", ""),
			new Achievement(AchievementEnum.NEW_ACHIEVEMENT_1_24, "博学家", ""),
		};

		private class Achievement {
			public AchievementEnum id;
			public string name;
			public string description;
			public bool beenAchieved;

			/// <summary>
			/// Creates an Achievement. You must also mirror the data provided here in https://partner.steamgames.com/apps/achievements/yourappid
			/// </summary>
			/// <param name="achievement">The "API Name Progress Stat" used to uniquely identify the achievement.</param>
			/// <param name="name">The "Display Name" that will be shown to players in game and on the Steam Community.</param>
			/// <param name="desc">The "Description" that will be shown to players in game and on the Steam Community.</param>
			public Achievement(AchievementEnum achievementID, string name, string desc) {
				id = achievementID;
				this.name = name;
				description = desc;
				beenAchieved = false;
			}
		}

		private bool initedUserStats = false;
		private bool receivedUserStats = false;
		private bool needToStore = false;
		private bool checkedStartAchievement = false;
		private bool checkedPackageAchievement = false;
		private bool initedUGCPackages = false;
		private Callback<UserStatsStored_t> userStatsStoredCallback;
		private Callback<UserAchievementStored_t> userAchievementStoredCallback;
		private Callback<UserStatsReceived_t> userStatsReceivedCallback;

		// Use this for initialization
		void Start() {
			instance = this;
			userStatsStoredCallback = Callback<UserStatsStored_t>.Create(onUserStatsStored);
			userAchievementStoredCallback = Callback<UserAchievementStored_t>.Create(onUserAchievementStored);
			userStatsReceivedCallback = Callback<UserStatsReceived_t>.Create(OnUserStatsReceived);
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
				if (!checkedPackageAchievement) {
					checkPackageAchievement();
				}
			}
			if (receivedUserStats && needToStore) {
				Debug.Log("store user stats");
				needToStore = false;
				bool result = SteamUserStats.StoreStats();
				Debug.Log("store user stats result: " + result);
			}
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
					achievements[(int)Enum.Parse (typeof(AchievementEnum), pCallback.m_rgchAchievementName)].beenAchieved = true;
					Debug.Log ("store user achievement(" + pCallback.m_rgchAchievementName + ") success");
				}
			}
		}

		private void OnUserStatsReceived(UserStatsReceived_t pCallback) {
			receivedUserStats = true;
			if ((ulong)Const.STEAM_APP_ID == pCallback.m_nGameID) {
				if (EResult.k_EResultOK == pCallback.m_eResult) {
					foreach (Achievement achievement in achievements) {
						string id = achievement.id.ToString ();
						bool result = SteamUserStats.GetAchievement (id, out achievement.beenAchieved);
						if (result) {
							achievement.name = SteamUserStats.GetAchievementDisplayAttribute (id, "name");
							achievement.description = SteamUserStats.GetAchievementDisplayAttribute (id, "desc");
						} else {
							Debug.LogWarning("SteamUserStats.GetAchievement failed for Achievement " + achievement.id + "\nIs it registered in the Steam Partner site?");
						}
					}
				}else {
					Debug.Log("RequestStats - failed, " + pCallback.m_eResult);
				}
			}
		}

		void unlockAchievement(AchievementEnum aEnum) {
			if (!SteamManager.Initialized) {
				return;
			}
			foreach(Achievement achievement in achievements) {
				if (achievement.id.Equals(aEnum) && !achievement.beenAchieved) {
					needToStore = SteamUserStats.SetAchievement(achievement.id.ToString());
					break;
				}
			}
		}

		private void checkStartAchievement() {
			checkedStartAchievement = true;
			if (!achievements[(int)AchievementEnum.NEW_ACHIEVEMENT_1_20].beenAchieved) {
				unlockAchievement(AchievementEnum.NEW_ACHIEVEMENT_1_20);
			}
		}

		private void checkPackageAchievement() {
			if (!PackageProgressService.shared.loaded && !Repository.shared.IsPackagesLoaded) {
				return;
			}
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
			checkedPackageAchievement = true;
			Package[] packages = Repository.shared.GetUGCPackages();
			if (packages.Length >= 1) {
				unlockAchievement(AchievementEnum.NEW_ACHIEVEMENT_1_14);
			}
			if (packages.Length >= 3) {
				unlockAchievement(AchievementEnum.NEW_ACHIEVEMENT_1_15);
			}
			if (packages.Length >= 5) {
				unlockAchievement(AchievementEnum.NEW_ACHIEVEMENT_1_16);
			}
			if (packages.Length >= 8) {
				unlockAchievement(AchievementEnum.NEW_ACHIEVEMENT_1_17);
			}
			if (packages.Length >= 10) {
				unlockAchievement(AchievementEnum.NEW_ACHIEVEMENT_1_18);
			}
			if (packages.Length >= 12) {
				unlockAchievement(AchievementEnum.NEW_ACHIEVEMENT_1_24);
			}
			packages = Repository.shared.GetAllPackages();
			int completeCount = 0;
			foreach(Package package in packages) {
				var progress = PackageProgressService.shared.GetProgress(package.id);
				if (progress != null && progress.total == progress.progress) {
					completeCount++;
				}
			}
			if (completeCount >= 1) {
				unlockAchievement(AchievementEnum.NEW_ACHIEVEMENT_1_6);
			}
			if (completeCount >= 2) {
				unlockAchievement(AchievementEnum.NEW_ACHIEVEMENT_1_7);
			}
			if (completeCount >= 3) {
				unlockAchievement(AchievementEnum.NEW_ACHIEVEMENT_1_8);
			}
			if (completeCount >= 4) {
				unlockAchievement(AchievementEnum.NEW_ACHIEVEMENT_1_9);
			}
			if (completeCount >= 5) {
				unlockAchievement(AchievementEnum.NEW_ACHIEVEMENT_1_10);
			}
			if (completeCount >= 6) {
				unlockAchievement(AchievementEnum.NEW_ACHIEVEMENT_1_11);
			}
			if (completeCount >= 7) {
				unlockAchievement(AchievementEnum.NEW_ACHIEVEMENT_1_12);
			}
			if (completeCount >= 8) {
				unlockAchievement(AchievementEnum.NEW_ACHIEVEMENT_1_13);
			}
			if (completeCount >= 9) {
				unlockAchievement(AchievementEnum.NEW_ACHIEVEMENT_1_21);
			}
			if (completeCount >= 10) {
				unlockAchievement(AchievementEnum.NEW_ACHIEVEMENT_1_22);
			}
			if (completeCount >= 11) {
				unlockAchievement(AchievementEnum.NEW_ACHIEVEMENT_1_23);
			}
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
			if (pCount >= 1) {
				unlockAchievement(AchievementEnum.NEW_ACHIEVEMENT_1_2);
			}
			if (p100Count >= 1) {
				unlockAchievement(AchievementEnum.NEW_ACHIEVEMENT_1_3);
			}
			if (p100Count >= 2) {
				unlockAchievement(AchievementEnum.NEW_ACHIEVEMENT_1_4);
			}
			if (p100Count >= 3) {
				unlockAchievement(AchievementEnum.NEW_ACHIEVEMENT_1_5);
			}
		}
	}
}
