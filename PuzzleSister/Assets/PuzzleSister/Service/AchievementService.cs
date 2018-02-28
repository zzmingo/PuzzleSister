using UnityEngine;
using System.Collections;
using Steamworks;

namespace PuzzleSister {
	public class AchievementService : MonoBehaviour {

		public static AchievementService instance = new AchievementService();

		private enum Achievement : int {
			NEW_ACHIEVEMENT_1_0,//完成一次十连对
			NEW_ACHIEVEMENT_1_1,//完成一次百题连答挑战
			NEW_ACHIEVEMENT_1_2,//编辑并发布1个题库。（最低30题即可，可以和100题那个成就同时解锁）
			NEW_ACHIEVEMENT_1_3,//编辑并发布1个百题题库。
			NEW_ACHIEVEMENT_1_4,//编辑并发布2个百题题库。
			NEW_ACHIEVEMENT_1_5,//编辑并发布3个百题题库。
			NEW_ACHIEVEMENT_1_6,//完成1套题库。（变量大于）
			NEW_ACHIEVEMENT_1_7,//完成2套题库。（变量大于）
			NEW_ACHIEVEMENT_1_8,//完成3套题库。
			NEW_ACHIEVEMENT_1_9,//完成4套题库。
			NEW_ACHIEVEMENT_1_10,//完成5套题库。
			NEW_ACHIEVEMENT_1_11,//完成6套题库。
			NEW_ACHIEVEMENT_1_12,//完成7套题库。
			NEW_ACHIEVEMENT_1_13,//完成8套题库。
			NEW_ACHIEVEMENT_1_14,//下载1个创意工坊题库
			NEW_ACHIEVEMENT_1_15,//下载3个创意工坊题库（变量大于）
			NEW_ACHIEVEMENT_1_16,//下载5个创意工坊题库（变量大于）
			NEW_ACHIEVEMENT_1_17,//下载8个创意工坊题库（变量大于）
			NEW_ACHIEVEMENT_1_18,//下载10个创意工坊题库（变量大于）
			NEW_ACHIEVEMENT_1_19,//（基础答题模式中）挑战失败导致题库姬菲迩电量耗尽。
			NEW_ACHIEVEMENT_1_20,//启动游戏
			NEW_ACHIEVEMENT_1_21,//完成9套题库。
			NEW_ACHIEVEMENT_1_22,//完成10套题库。
			NEW_ACHIEVEMENT_1_23,//完成11套题库。
			NEW_ACHIEVEMENT_1_24,//下载12个创意工坊题库（变量大于）
		};

		private Achievement_t[] m_Achievements = new Achievement_t[]{
			new Achievement_t(Achievement.NEW_ACHIEVEMENT_1_0, "轻轻松松", ""),
			new Achievement_t(Achievement.NEW_ACHIEVEMENT_1_1, "小菜一碟", ""),
			new Achievement_t(Achievement.NEW_ACHIEVEMENT_1_2, "会打字就能参与制作游戏！", ""),
			new Achievement_t(Achievement.NEW_ACHIEVEMENT_1_3, "一起来玩啊！", ""),
			new Achievement_t(Achievement.NEW_ACHIEVEMENT_1_4, "毛遂自荐美滋滋", ""),
			new Achievement_t(Achievement.NEW_ACHIEVEMENT_1_5, "我也是制作人！", ""),
			new Achievement_t(Achievement.NEW_ACHIEVEMENT_1_6, "小试牛刀", ""),
			new Achievement_t(Achievement.NEW_ACHIEVEMENT_1_7, "马马虎虎", ""),
			new Achievement_t(Achievement.NEW_ACHIEVEMENT_1_8, "渐入佳境", ""),
			new Achievement_t(Achievement.NEW_ACHIEVEMENT_1_9, "颇有见地", ""),
			new Achievement_t(Achievement.NEW_ACHIEVEMENT_1_10, "涉猎广泛", ""),
			new Achievement_t(Achievement.NEW_ACHIEVEMENT_1_11, "足智多谋", ""),
			new Achievement_t(Achievement.NEW_ACHIEVEMENT_1_12, "才华横溢", ""),
			new Achievement_t(Achievement.NEW_ACHIEVEMENT_1_13, "聪明绝顶", ""),
			new Achievement_t(Achievement.NEW_ACHIEVEMENT_1_14, "免费获取新知识", ""),
			new Achievement_t(Achievement.NEW_ACHIEVEMENT_1_15, "精挑细选", ""),
			new Achievement_t(Achievement.NEW_ACHIEVEMENT_1_16, "新姿势GET!", ""),
			new Achievement_t(Achievement.NEW_ACHIEVEMENT_1_17, "集思广益", ""),
			new Achievement_t(Achievement.NEW_ACHIEVEMENT_1_18, "知识收藏家", ""),
			new Achievement_t(Achievement.NEW_ACHIEVEMENT_1_19, "脑子短路", ""),
			new Achievement_t(Achievement.NEW_ACHIEVEMENT_1_20, "充电开始", ""),
			new Achievement_t(Achievement.NEW_ACHIEVEMENT_1_21, "无所不能", ""),
			new Achievement_t(Achievement.NEW_ACHIEVEMENT_1_22, "博大精深", ""),
			new Achievement_t(Achievement.NEW_ACHIEVEMENT_1_23, "全知全能", ""),
			new Achievement_t(Achievement.NEW_ACHIEVEMENT_1_24, "博学家", ""),
		};

		private class Achievement_t {
			public Achievement m_eAchievementID;
			public string m_strName;
			public string m_strDescription;
			public bool m_bAchieved;

			/// <summary>
			/// Creates an Achievement. You must also mirror the data provided here in https://partner.steamgames.com/apps/achievements/yourappid
			/// </summary>
			/// <param name="achievement">The "API Name Progress Stat" used to uniquely identify the achievement.</param>
			/// <param name="name">The "Display Name" that will be shown to players in game and on the Steam Community.</param>
			/// <param name="desc">The "Description" that will be shown to players in game and on the Steam Community.</param>
			public Achievement_t(Achievement achievementID, string name, string desc) {
				m_eAchievementID = achievementID;
				m_strName = name;
				m_strDescription = desc;
				m_bAchieved = false;
			}
		}

		private bool steamUserStatsInited = false;
		private Callback<UserStatsReceived_t> m_UserStatsReceived;

		// Use this for initialization
		void Start() {
			if (!SteamManager.Initialized) {
				return;
			}
			m_UserStatsReceived = Callback<UserStatsReceived_t>.Create(OnUserStatsReceived);
		}

		void Update() {
			if (!SteamManager.Initialized || steamUserStatsInited) {
				return;
			}
			steamUserStatsInited = SteamUserStats.RequestCurrentStats();
		}

		private void OnUserStatsReceived(UserStatsReceived_t pCallback) {
			if ((ulong)Const.STEAM_APP_ID == pCallback.m_nGameID) {
				if (EResult.k_EResultOK == pCallback.m_eResult) {
					foreach (Achievement_t achievement in m_Achievements) {
						string id = achievement.m_eAchievementID.ToString ();
						bool result = SteamUserStats.GetAchievement (id, out achievement.m_bAchieved);
						if (result) {
							achievement.m_strName = SteamUserStats.GetAchievementDisplayAttribute (id, "name");
							achievement.m_strDescription = SteamUserStats.GetAchievementDisplayAttribute (id, "desc");
						} else {
							Debug.LogWarning("SteamUserStats.GetAchievement failed for Achievement " + achievement.m_eAchievementID + "\nIs it registered in the Steam Partner site?");
						}
					}
				}else {
					Debug.Log("RequestStats - failed, " + pCallback.m_eResult);
				}
			}
		}
	}
}
