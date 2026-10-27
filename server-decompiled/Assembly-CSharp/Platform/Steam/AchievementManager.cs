using System;
using System.Collections.Generic;
using Steamworks;

namespace Platform.Steam
{
	// Token: 0x02001CDE RID: 7390
	public class AchievementManager : IAchievementManager
	{
		// Token: 0x0600DABC RID: 55996 RVA: 0x004E4030 File Offset: 0x004E2230
		public AchievementManager()
		{
			this.m_UserStatsReceived = Callback<UserStatsReceived_t>.Create(new Callback<UserStatsReceived_t>.DispatchDelegate(this.UserStatsReceived_Callback));
			this.m_UserStatsStored = Callback<UserStatsStored_t>.Create(new Callback<UserStatsStored_t>.DispatchDelegate(this.UserStatsStored_Callback));
			this.m_UserAchievementStored_t = Callback<UserAchievementStored_t>.Create(new Callback<UserAchievementStored_t>.DispatchDelegate(this.UserAchievementStored_Callback));
		}

		// Token: 0x0600DABD RID: 55997 RVA: 0x004E409E File Offset: 0x004E229E
		public void Init(IPlatform _owner)
		{
			_owner.User.UserLoggedIn += delegate(IPlatform _sender)
			{
				SteamUserStats.RequestCurrentStats();
			};
		}

		// Token: 0x0600DABE RID: 55998 RVA: 0x004E40CC File Offset: 0x004E22CC
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

		// Token: 0x0600DABF RID: 55999 RVA: 0x004E4204 File Offset: 0x004E2404
		[PublicizedFrom(EAccessModifier.Private)]
		public void UserStatsStored_Callback(UserStatsStored_t _result)
		{
			Log.Out("AchievementManager.UserStatsStored_Callback, result={0}", new object[] { _result.m_eResult.ToStringCached<EResult>() });
		}

		// Token: 0x0600DAC0 RID: 56000 RVA: 0x004E4224 File Offset: 0x004E2424
		[PublicizedFrom(EAccessModifier.Private)]
		public void UserAchievementStored_Callback(UserAchievementStored_t _result)
		{
			Log.Out("AchievementManager.UserAchievementStored_Callback, name={0}, cur={1}, max={2}", new object[] { _result.m_rgchAchievementName, _result.m_nCurProgress, _result.m_nMaxProgress });
		}

		// Token: 0x0600DAC1 RID: 56001 RVA: 0x004E425C File Offset: 0x004E245C
		public void ShowAchievementsUi()
		{
			Log.Out("AchievementManager.ShowAchievementsUI");
			SteamFriends.ActivateGameOverlay("Achievements");
		}

		// Token: 0x0600DAC2 RID: 56002 RVA: 0x004E4274 File Offset: 0x004E2474
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

		// Token: 0x0600DAC3 RID: 56003 RVA: 0x004E42E0 File Offset: 0x004E24E0
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

		// Token: 0x0600DAC4 RID: 56004 RVA: 0x004E4330 File Offset: 0x004E2530
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

		// Token: 0x0600DAC5 RID: 56005 RVA: 0x004E4380 File Offset: 0x004E2580
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

		// Token: 0x0600DAC6 RID: 56006 RVA: 0x004E43B4 File Offset: 0x004E25B4
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

		// Token: 0x0600DAC7 RID: 56007 RVA: 0x004E43E4 File Offset: 0x004E25E4
		[PublicizedFrom(EAccessModifier.Private)]
		public bool IsAchievementLocked(EnumAchievementManagerAchievement _achievement)
		{
			AchievementManager.AchievementCacheEntry achievementCacheEntry;
			return this.steamAchievementsCache.TryGetValue(_achievement, out achievementCacheEntry) && achievementCacheEntry.locked;
		}

		// Token: 0x0600DAC8 RID: 56008 RVA: 0x004E440C File Offset: 0x004E260C
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

		// Token: 0x0600DAC9 RID: 56009 RVA: 0x004E4464 File Offset: 0x004E2664
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

		// Token: 0x0600DACA RID: 56010 RVA: 0x004E44BC File Offset: 0x004E26BC
		public bool IsAchievementStatSupported(EnumAchievementDataStat _stat)
		{
			return _stat != EnumAchievementDataStat.HighestGamestage;
		}

		// Token: 0x0600DACB RID: 56011 RVA: 0x004E44D8 File Offset: 0x004E26D8
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

		// Token: 0x0600DACC RID: 56012 RVA: 0x004E4580 File Offset: 0x004E2780
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

		// Token: 0x0600DACD RID: 56013 RVA: 0x004E4626 File Offset: 0x004E2826
		public void ResetStats(bool _andAchievements)
		{
			SteamUserStats.ResetAllStats(_andAchievements);
		}

		// Token: 0x0600DACE RID: 56014 RVA: 0x004E4630 File Offset: 0x004E2830
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

		// Token: 0x0600DACF RID: 56015 RVA: 0x004E46AB File Offset: 0x004E28AB
		public void Destroy()
		{
			Log.Out("AchievementManager.Cleanup");
		}

		// Token: 0x0400A606 RID: 42502
		[PublicizedFrom(EAccessModifier.Private)]
		public Callback<UserStatsReceived_t> m_UserStatsReceived;

		// Token: 0x0400A607 RID: 42503
		[PublicizedFrom(EAccessModifier.Private)]
		public Callback<UserStatsStored_t> m_UserStatsStored;

		// Token: 0x0400A608 RID: 42504
		[PublicizedFrom(EAccessModifier.Private)]
		public Callback<UserAchievementStored_t> m_UserAchievementStored_t;

		// Token: 0x0400A609 RID: 42505
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<EnumAchievementDataStat, AchievementManager.StatCacheEntry> steamStatsCache = new EnumDictionary<EnumAchievementDataStat, AchievementManager.StatCacheEntry>();

		// Token: 0x0400A60A RID: 42506
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<EnumAchievementManagerAchievement, AchievementManager.AchievementCacheEntry> steamAchievementsCache = new EnumDictionary<EnumAchievementManagerAchievement, AchievementManager.AchievementCacheEntry>();

		// Token: 0x02001CDF RID: 7391
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly struct StatCacheEntry
		{
			// Token: 0x0600DAD0 RID: 56016 RVA: 0x004E46B7 File Offset: 0x004E28B7
			public StatCacheEntry(string _name, int _iValue, float _fValue)
			{
				this.name = _name;
				this.iValue = _iValue;
				this.fValue = _fValue;
			}

			// Token: 0x0400A60B RID: 42507
			public readonly string name;

			// Token: 0x0400A60C RID: 42508
			public readonly int iValue;

			// Token: 0x0400A60D RID: 42509
			public readonly float fValue;
		}

		// Token: 0x02001CE0 RID: 7392
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly struct AchievementCacheEntry
		{
			// Token: 0x0600DAD1 RID: 56017 RVA: 0x004E46CE File Offset: 0x004E28CE
			public AchievementCacheEntry(string _name, bool _locked)
			{
				this.name = _name;
				this.locked = _locked;
			}

			// Token: 0x0400A60E RID: 42510
			public readonly string name;

			// Token: 0x0400A60F RID: 42511
			public readonly bool locked;
		}
	}
}
