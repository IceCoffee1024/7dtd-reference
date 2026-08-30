using System;
using System.Collections.Generic;
using Steamworks;

namespace Platform.Steam
{
	// Token: 0x02001C7B RID: 7291
	public class AchievementManager : IAchievementManager
	{
		// Token: 0x0600D82F RID: 55343 RVA: 0x004DB9A4 File Offset: 0x004D9BA4
		public AchievementManager()
		{
			this.m_UserStatsReceived = Callback<UserStatsReceived_t>.Create(new Callback<UserStatsReceived_t>.DispatchDelegate(this.UserStatsReceived_Callback));
			this.m_UserStatsStored = Callback<UserStatsStored_t>.Create(new Callback<UserStatsStored_t>.DispatchDelegate(this.UserStatsStored_Callback));
			this.m_UserAchievementStored_t = Callback<UserAchievementStored_t>.Create(new Callback<UserAchievementStored_t>.DispatchDelegate(this.UserAchievementStored_Callback));
		}

		// Token: 0x0600D830 RID: 55344 RVA: 0x004DBA12 File Offset: 0x004D9C12
		public void Init(IPlatform _owner)
		{
			_owner.User.UserLoggedIn += delegate(IPlatform _sender)
			{
				SteamUserStats.RequestCurrentStats();
			};
		}

		// Token: 0x0600D831 RID: 55345 RVA: 0x004DBA40 File Offset: 0x004D9C40
		[PublicizedFrom(EAccessModifier.Private)]
		public void UserStatsReceived_Callback(UserStatsReceived_t _result)
		{
			if (_result.m_nGameID != 251570UL)
			{
				return;
			}
			if (_result.m_eResult != EResult.k_EResultOK)
			{
				Log.Error("AchievementManager: RequestStats failed: {0}", new object[] { _result.m_eResult.ToStringCached<EResult>() });
				return;
			}
			if (this.steamStatsCache.Count > 0)
			{
				return;
			}
			Log.Out("AchievementManager: Received stats and achievements from Steam");
			for (int i = 0; i < 19; i++)
			{
				EnumAchievementDataStat enumAchievementDataStat = (EnumAchievementDataStat)i;
				if (enumAchievementDataStat.IsSupported())
				{
					switch (AchievementData.GetStatType(enumAchievementDataStat))
					{
					case EnumStatType.Int:
					{
						int num;
						if (SteamUserStats.GetStat(enumAchievementDataStat.ToStringCached<EnumAchievementDataStat>(), out num))
						{
							this.steamStatsCache.Add(enumAchievementDataStat, new AchievementManager.StatCacheEntry(enumAchievementDataStat.ToStringCached<EnumAchievementDataStat>(), num, 0f));
						}
						break;
					}
					case EnumStatType.Float:
					{
						float num2;
						if (SteamUserStats.GetStat(enumAchievementDataStat.ToStringCached<EnumAchievementDataStat>(), out num2))
						{
							this.steamStatsCache.Add(enumAchievementDataStat, new AchievementManager.StatCacheEntry(enumAchievementDataStat.ToStringCached<EnumAchievementDataStat>(), 0, num2));
						}
						break;
					}
					}
				}
			}
			for (int j = 0; j < 48; j++)
			{
				EnumAchievementManagerAchievement enumAchievementManagerAchievement = (EnumAchievementManagerAchievement)j;
				bool flag;
				if (enumAchievementManagerAchievement.IsSupported() && SteamUserStats.GetAchievement(enumAchievementManagerAchievement.ToStringCached<EnumAchievementManagerAchievement>(), out flag))
				{
					this.steamAchievementsCache.Add(enumAchievementManagerAchievement, new AchievementManager.AchievementCacheEntry(enumAchievementManagerAchievement.ToStringCached<EnumAchievementManagerAchievement>(), flag));
				}
			}
		}

		// Token: 0x0600D832 RID: 55346 RVA: 0x004DBB78 File Offset: 0x004D9D78
		[PublicizedFrom(EAccessModifier.Private)]
		public void UserStatsStored_Callback(UserStatsStored_t _result)
		{
			Log.Out("AchievementManager.UserStatsStored_Callback, result={0}", new object[] { _result.m_eResult.ToStringCached<EResult>() });
		}

		// Token: 0x0600D833 RID: 55347 RVA: 0x004DBB98 File Offset: 0x004D9D98
		[PublicizedFrom(EAccessModifier.Private)]
		public void UserAchievementStored_Callback(UserAchievementStored_t _result)
		{
			Log.Out("AchievementManager.UserAchievementStored_Callback, name={0}, cur={1}, max={2}", new object[] { _result.m_rgchAchievementName, _result.m_nCurProgress, _result.m_nMaxProgress });
		}

		// Token: 0x0600D834 RID: 55348 RVA: 0x004DBBD0 File Offset: 0x004D9DD0
		public void ShowAchievementsUi()
		{
			Log.Out("AchievementManager.ShowAchievementsUI");
			SteamFriends.ActivateGameOverlay("Achievements");
		}

		// Token: 0x0600D835 RID: 55349 RVA: 0x004DBBE8 File Offset: 0x004D9DE8
		[PublicizedFrom(EAccessModifier.Private)]
		public void SendAchievementEvent(EnumAchievementManagerAchievement _achievement)
		{
			if (AchievementUtils.IsCreativeModeActive())
			{
				return;
			}
			Log.Out("AchievementManager.SendAchievementEvent (" + _achievement.ToStringCached<EnumAchievementManagerAchievement>() + ")");
			AchievementManager.AchievementCacheEntry achievementCacheEntry;
			if (this.steamAchievementsCache.TryGetValue(_achievement, out achievementCacheEntry))
			{
				SteamUserStats.SetAchievement(achievementCacheEntry.name);
				this.steamAchievementsCache[_achievement] = new AchievementManager.AchievementCacheEntry(achievementCacheEntry.name, true);
				SteamUserStats.StoreStats();
			}
		}

		// Token: 0x0600D836 RID: 55350 RVA: 0x004DBC54 File Offset: 0x004D9E54
		[PublicizedFrom(EAccessModifier.Private)]
		public void SetAchievementStatValueFloat(EnumAchievementDataStat _stat, float _value)
		{
			AchievementManager.StatCacheEntry statCacheEntry;
			if (!this.steamStatsCache.TryGetValue(_stat, out statCacheEntry) || AchievementData.GetStatType(_stat) != EnumStatType.Float)
			{
				return;
			}
			this.steamStatsCache[_stat] = new AchievementManager.StatCacheEntry(statCacheEntry.name, 0, _value);
			SteamUserStats.SetStat(statCacheEntry.name, _value);
		}

		// Token: 0x0600D837 RID: 55351 RVA: 0x004DBCA4 File Offset: 0x004D9EA4
		[PublicizedFrom(EAccessModifier.Private)]
		public void SetAchievementStatValueInt(EnumAchievementDataStat _stat, int _value)
		{
			AchievementManager.StatCacheEntry statCacheEntry;
			if (!this.steamStatsCache.TryGetValue(_stat, out statCacheEntry) || AchievementData.GetStatType(_stat) != EnumStatType.Int)
			{
				return;
			}
			this.steamStatsCache[_stat] = new AchievementManager.StatCacheEntry(statCacheEntry.name, _value, 0f);
			SteamUserStats.SetStat(statCacheEntry.name, _value);
		}

		// Token: 0x0600D838 RID: 55352 RVA: 0x004DBCF4 File Offset: 0x004D9EF4
		[PublicizedFrom(EAccessModifier.Private)]
		public float GetAchievementStatValueFloat(EnumAchievementDataStat _stat)
		{
			AchievementManager.StatCacheEntry statCacheEntry;
			if (this.steamStatsCache.TryGetValue(_stat, out statCacheEntry) && AchievementData.GetStatType(_stat) == EnumStatType.Float)
			{
				return statCacheEntry.fValue;
			}
			return 0f;
		}

		// Token: 0x0600D839 RID: 55353 RVA: 0x004DBD28 File Offset: 0x004D9F28
		[PublicizedFrom(EAccessModifier.Private)]
		public int GetAchievementStatValueInt(EnumAchievementDataStat _stat)
		{
			AchievementManager.StatCacheEntry statCacheEntry;
			if (this.steamStatsCache.TryGetValue(_stat, out statCacheEntry) && AchievementData.GetStatType(_stat) == EnumStatType.Int)
			{
				return statCacheEntry.iValue;
			}
			return 0;
		}

		// Token: 0x0600D83A RID: 55354 RVA: 0x004DBD58 File Offset: 0x004D9F58
		[PublicizedFrom(EAccessModifier.Private)]
		public bool IsAchievementLocked(EnumAchievementManagerAchievement _achievement)
		{
			AchievementManager.AchievementCacheEntry achievementCacheEntry;
			return this.steamAchievementsCache.TryGetValue(_achievement, out achievementCacheEntry) && achievementCacheEntry.locked;
		}

		// Token: 0x0600D83B RID: 55355 RVA: 0x004DBD80 File Offset: 0x004D9F80
		[PublicizedFrom(EAccessModifier.Private)]
		public void UpdateAchievement(EnumAchievementDataStat _stat, float _newValue)
		{
			List<AchievementData.AchievementInfo> achievementInfos = AchievementData.GetAchievementInfos(_stat);
			for (int i = 0; i < achievementInfos.Count; i++)
			{
				EnumAchievementManagerAchievement achievement = achievementInfos[i].achievement;
				if (_newValue >= Convert.ToSingle(achievementInfos[i].triggerPoint) && !this.IsAchievementLocked(achievement))
				{
					this.SendAchievementEvent(achievement);
				}
			}
		}

		// Token: 0x0600D83C RID: 55356 RVA: 0x004DBDD8 File Offset: 0x004D9FD8
		[PublicizedFrom(EAccessModifier.Private)]
		public void UpdateAchievement(EnumAchievementDataStat _stat, int _newValue)
		{
			List<AchievementData.AchievementInfo> achievementInfos = AchievementData.GetAchievementInfos(_stat);
			for (int i = 0; i < achievementInfos.Count; i++)
			{
				EnumAchievementManagerAchievement achievement = achievementInfos[i].achievement;
				if (_newValue >= Convert.ToInt32(achievementInfos[i].triggerPoint) && !this.IsAchievementLocked(achievement))
				{
					this.SendAchievementEvent(achievement);
				}
			}
		}

		// Token: 0x0600D83D RID: 55357 RVA: 0x004DBE30 File Offset: 0x004DA030
		public bool IsAchievementStatSupported(EnumAchievementDataStat _stat)
		{
			return _stat != EnumAchievementDataStat.HighestGamestage;
		}

		// Token: 0x0600D83E RID: 55358 RVA: 0x004DBE4C File Offset: 0x004DA04C
		public void SetAchievementStat(EnumAchievementDataStat _stat, int _value)
		{
			if (!_stat.IsSupported())
			{
				return;
			}
			if (AchievementUtils.IsCreativeModeActive())
			{
				return;
			}
			AchievementData.EnumUpdateType updateType = AchievementData.GetUpdateType(_stat);
			EnumStatType statType = AchievementData.GetStatType(_stat);
			if (!this.steamStatsCache.ContainsKey(_stat))
			{
				return;
			}
			if (statType != EnumStatType.Int)
			{
				Log.Warning("AchievementManager.SetAchievementStat, int given for float type stat {0}", new object[] { _stat.ToStringCached<EnumAchievementDataStat>() });
				return;
			}
			int achievementStatValueInt = this.GetAchievementStatValueInt(_stat);
			int num;
			switch (updateType)
			{
			case AchievementData.EnumUpdateType.Sum:
				num = achievementStatValueInt + _value;
				break;
			case AchievementData.EnumUpdateType.Replace:
				num = _value;
				break;
			case AchievementData.EnumUpdateType.Max:
				num = Math.Max(achievementStatValueInt, _value);
				break;
			default:
				num = 0;
				break;
			}
			int num2 = num;
			if (achievementStatValueInt != num2)
			{
				this.SetAchievementStatValueInt(_stat, num2);
				this.UpdateAchievement(_stat, num2);
			}
		}

		// Token: 0x0600D83F RID: 55359 RVA: 0x004DBEF4 File Offset: 0x004DA0F4
		public void SetAchievementStat(EnumAchievementDataStat _stat, float _value)
		{
			if (!_stat.IsSupported())
			{
				return;
			}
			if (AchievementUtils.IsCreativeModeActive())
			{
				return;
			}
			AchievementData.EnumUpdateType updateType = AchievementData.GetUpdateType(_stat);
			EnumStatType statType = AchievementData.GetStatType(_stat);
			if (!this.steamStatsCache.ContainsKey(_stat))
			{
				return;
			}
			if (statType != EnumStatType.Float)
			{
				Log.Warning("AchievementManager.SetAchievementStat, float given for int type stat {0}", new object[] { _stat.ToStringCached<EnumAchievementDataStat>() });
				return;
			}
			float achievementStatValueFloat = this.GetAchievementStatValueFloat(_stat);
			float num;
			switch (updateType)
			{
			case AchievementData.EnumUpdateType.Sum:
				num = achievementStatValueFloat + _value;
				break;
			case AchievementData.EnumUpdateType.Replace:
				num = _value;
				break;
			case AchievementData.EnumUpdateType.Max:
				num = Math.Max(achievementStatValueFloat, _value);
				break;
			default:
				num = achievementStatValueFloat;
				break;
			}
			float num2 = num;
			if (achievementStatValueFloat != num2)
			{
				this.SetAchievementStatValueFloat(_stat, num2);
				this.UpdateAchievement(_stat, num2);
			}
		}

		// Token: 0x0600D840 RID: 55360 RVA: 0x004DBF9A File Offset: 0x004DA19A
		public void ResetStats(bool _andAchievements)
		{
			SteamUserStats.ResetAllStats(_andAchievements);
		}

		// Token: 0x0600D841 RID: 55361 RVA: 0x004DBFA4 File Offset: 0x004DA1A4
		public void UnlockAllAchievements()
		{
			for (int i = 0; i < 19; i++)
			{
				EnumAchievementDataStat enumAchievementDataStat = (EnumAchievementDataStat)i;
				if (enumAchievementDataStat.IsSupported())
				{
					List<AchievementData.AchievementInfo> achievementInfos = AchievementData.GetAchievementInfos(enumAchievementDataStat);
					AchievementData.AchievementInfo achievementInfo = achievementInfos[achievementInfos.Count - 1];
					switch (AchievementData.GetStatType(enumAchievementDataStat))
					{
					case EnumStatType.Int:
						this.SetAchievementStat(enumAchievementDataStat, Convert.ToInt32(achievementInfo.triggerPoint));
						break;
					case EnumStatType.Float:
						this.SetAchievementStat(enumAchievementDataStat, Convert.ToSingle(achievementInfo.triggerPoint));
						break;
					}
				}
			}
		}

		// Token: 0x0600D842 RID: 55362 RVA: 0x004DC01F File Offset: 0x004DA21F
		public void Destroy()
		{
			Log.Out("AchievementManager.Cleanup");
		}

		// Token: 0x0400A448 RID: 42056
		[PublicizedFrom(EAccessModifier.Private)]
		public Callback<UserStatsReceived_t> m_UserStatsReceived;

		// Token: 0x0400A449 RID: 42057
		[PublicizedFrom(EAccessModifier.Private)]
		public Callback<UserStatsStored_t> m_UserStatsStored;

		// Token: 0x0400A44A RID: 42058
		[PublicizedFrom(EAccessModifier.Private)]
		public Callback<UserAchievementStored_t> m_UserAchievementStored_t;

		// Token: 0x0400A44B RID: 42059
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<EnumAchievementDataStat, AchievementManager.StatCacheEntry> steamStatsCache = new EnumDictionary<EnumAchievementDataStat, AchievementManager.StatCacheEntry>();

		// Token: 0x0400A44C RID: 42060
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<EnumAchievementManagerAchievement, AchievementManager.AchievementCacheEntry> steamAchievementsCache = new EnumDictionary<EnumAchievementManagerAchievement, AchievementManager.AchievementCacheEntry>();

		// Token: 0x02001C7C RID: 7292
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly struct StatCacheEntry
		{
			// Token: 0x0600D843 RID: 55363 RVA: 0x004DC02B File Offset: 0x004DA22B
			public StatCacheEntry(string _name, int _iValue, float _fValue)
			{
				this.name = _name;
				this.iValue = _iValue;
				this.fValue = _fValue;
			}

			// Token: 0x0400A44D RID: 42061
			public readonly string name;

			// Token: 0x0400A44E RID: 42062
			public readonly int iValue;

			// Token: 0x0400A44F RID: 42063
			public readonly float fValue;
		}

		// Token: 0x02001C7D RID: 7293
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly struct AchievementCacheEntry
		{
			// Token: 0x0600D844 RID: 55364 RVA: 0x004DC042 File Offset: 0x004DA242
			public AchievementCacheEntry(string _name, bool _locked)
			{
				this.name = _name;
				this.locked = _locked;
			}

			// Token: 0x0400A450 RID: 42064
			public readonly string name;

			// Token: 0x0400A451 RID: 42065
			public readonly bool locked;
		}
	}
}
