using System;
using System.Collections.Generic;
using Steamworks;

namespace Platform.Steam
{
	// Token: 0x02001C54 RID: 7252
	public class AchievementManager : IAchievementManager
	{
		// Token: 0x0600D69F RID: 54943 RVA: 0x004D5580 File Offset: 0x004D3780
		public AchievementManager()
		{
			this.m_UserStatsReceived = Callback<UserStatsReceived_t>.Create(new Callback<UserStatsReceived_t>.DispatchDelegate(this.UserStatsReceived_Callback));
			this.m_UserStatsStored = Callback<UserStatsStored_t>.Create(new Callback<UserStatsStored_t>.DispatchDelegate(this.UserStatsStored_Callback));
			this.m_UserAchievementStored_t = Callback<UserAchievementStored_t>.Create(new Callback<UserAchievementStored_t>.DispatchDelegate(this.UserAchievementStored_Callback));
		}

		// Token: 0x0600D6A0 RID: 54944 RVA: 0x004D55EE File Offset: 0x004D37EE
		public void Init(IPlatform _owner)
		{
			_owner.User.UserLoggedIn += delegate(IPlatform _sender)
			{
				SteamUserStats.RequestCurrentStats();
			};
		}

		// Token: 0x0600D6A1 RID: 54945 RVA: 0x004D561C File Offset: 0x004D381C
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

		// Token: 0x0600D6A2 RID: 54946 RVA: 0x004D5754 File Offset: 0x004D3954
		[PublicizedFrom(EAccessModifier.Private)]
		public void UserStatsStored_Callback(UserStatsStored_t _result)
		{
			Log.Out("AchievementManager.UserStatsStored_Callback, result={0}", new object[] { _result.m_eResult.ToStringCached<EResult>() });
		}

		// Token: 0x0600D6A3 RID: 54947 RVA: 0x004D5774 File Offset: 0x004D3974
		[PublicizedFrom(EAccessModifier.Private)]
		public void UserAchievementStored_Callback(UserAchievementStored_t _result)
		{
			Log.Out("AchievementManager.UserAchievementStored_Callback, name={0}, cur={1}, max={2}", new object[] { _result.m_rgchAchievementName, _result.m_nCurProgress, _result.m_nMaxProgress });
		}

		// Token: 0x0600D6A4 RID: 54948 RVA: 0x004D57AC File Offset: 0x004D39AC
		public void ShowAchievementsUi()
		{
			Log.Out("AchievementManager.ShowAchievementsUI");
			SteamFriends.ActivateGameOverlay("Achievements");
		}

		// Token: 0x0600D6A5 RID: 54949 RVA: 0x004D57C4 File Offset: 0x004D39C4
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

		// Token: 0x0600D6A6 RID: 54950 RVA: 0x004D5830 File Offset: 0x004D3A30
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

		// Token: 0x0600D6A7 RID: 54951 RVA: 0x004D5880 File Offset: 0x004D3A80
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

		// Token: 0x0600D6A8 RID: 54952 RVA: 0x004D58D0 File Offset: 0x004D3AD0
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

		// Token: 0x0600D6A9 RID: 54953 RVA: 0x004D5904 File Offset: 0x004D3B04
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

		// Token: 0x0600D6AA RID: 54954 RVA: 0x004D5934 File Offset: 0x004D3B34
		[PublicizedFrom(EAccessModifier.Private)]
		public bool IsAchievementLocked(EnumAchievementManagerAchievement _achievement)
		{
			AchievementManager.AchievementCacheEntry achievementCacheEntry;
			return this.steamAchievementsCache.TryGetValue(_achievement, out achievementCacheEntry) && achievementCacheEntry.locked;
		}

		// Token: 0x0600D6AB RID: 54955 RVA: 0x004D595C File Offset: 0x004D3B5C
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

		// Token: 0x0600D6AC RID: 54956 RVA: 0x004D59B4 File Offset: 0x004D3BB4
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

		// Token: 0x0600D6AD RID: 54957 RVA: 0x004D5A0C File Offset: 0x004D3C0C
		public bool IsAchievementStatSupported(EnumAchievementDataStat _stat)
		{
			return _stat != EnumAchievementDataStat.HighestGamestage;
		}

		// Token: 0x0600D6AE RID: 54958 RVA: 0x004D5A28 File Offset: 0x004D3C28
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

		// Token: 0x0600D6AF RID: 54959 RVA: 0x004D5AD0 File Offset: 0x004D3CD0
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

		// Token: 0x0600D6B0 RID: 54960 RVA: 0x004D5B76 File Offset: 0x004D3D76
		public void ResetStats(bool _andAchievements)
		{
			SteamUserStats.ResetAllStats(_andAchievements);
		}

		// Token: 0x0600D6B1 RID: 54961 RVA: 0x004D5B80 File Offset: 0x004D3D80
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

		// Token: 0x0600D6B2 RID: 54962 RVA: 0x004D5BFB File Offset: 0x004D3DFB
		public void Destroy()
		{
			Log.Out("AchievementManager.Cleanup");
		}

		// Token: 0x0400A317 RID: 41751
		[PublicizedFrom(EAccessModifier.Private)]
		public Callback<UserStatsReceived_t> m_UserStatsReceived;

		// Token: 0x0400A318 RID: 41752
		[PublicizedFrom(EAccessModifier.Private)]
		public Callback<UserStatsStored_t> m_UserStatsStored;

		// Token: 0x0400A319 RID: 41753
		[PublicizedFrom(EAccessModifier.Private)]
		public Callback<UserAchievementStored_t> m_UserAchievementStored_t;

		// Token: 0x0400A31A RID: 41754
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<EnumAchievementDataStat, AchievementManager.StatCacheEntry> steamStatsCache = new EnumDictionary<EnumAchievementDataStat, AchievementManager.StatCacheEntry>();

		// Token: 0x0400A31B RID: 41755
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<EnumAchievementManagerAchievement, AchievementManager.AchievementCacheEntry> steamAchievementsCache = new EnumDictionary<EnumAchievementManagerAchievement, AchievementManager.AchievementCacheEntry>();

		// Token: 0x02001C55 RID: 7253
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly struct StatCacheEntry
		{
			// Token: 0x0600D6B3 RID: 54963 RVA: 0x004D5C07 File Offset: 0x004D3E07
			public StatCacheEntry(string _name, int _iValue, float _fValue)
			{
				this.name = _name;
				this.iValue = _iValue;
				this.fValue = _fValue;
			}

			// Token: 0x0400A31C RID: 41756
			public readonly string name;

			// Token: 0x0400A31D RID: 41757
			public readonly int iValue;

			// Token: 0x0400A31E RID: 41758
			public readonly float fValue;
		}

		// Token: 0x02001C56 RID: 7254
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly struct AchievementCacheEntry
		{
			// Token: 0x0600D6B4 RID: 54964 RVA: 0x004D5C1E File Offset: 0x004D3E1E
			public AchievementCacheEntry(string _name, bool _locked)
			{
				this.name = _name;
				this.locked = _locked;
			}

			// Token: 0x0400A31F RID: 41759
			public readonly string name;

			// Token: 0x0400A320 RID: 41760
			public readonly bool locked;
		}
	}
}
