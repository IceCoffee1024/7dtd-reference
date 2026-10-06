using System;
using System.Collections.Generic;
using Unity.XGamingRuntime;
using UnityEngine;

namespace Platform.XBL
{
	// Token: 0x02001C5C RID: 7260
	public class AchievementManager : IAchievementManager
	{
		// Token: 0x0600D77D RID: 55165 RVA: 0x004D5514 File Offset: 0x004D3714
		[PublicizedFrom(EAccessModifier.Private)]
		static AchievementManager()
		{
			string launchArgument = GameUtils.GetLaunchArgument("debugachievements");
			if (launchArgument != null)
			{
				if (launchArgument == "verbose")
				{
					AchievementManager.debug = AchievementManager.EDebugLevel.Verbose;
					return;
				}
				AchievementManager.debug = AchievementManager.EDebugLevel.Normal;
			}
		}

		// Token: 0x0600D77E RID: 55166 RVA: 0x004D5550 File Offset: 0x004D3750
		public void Init(IPlatform _owner)
		{
			_owner.User.UserLoggedIn += delegate(IPlatform _sender)
			{
				this.xblUser = (User)_owner.User;
			};
		}

		// Token: 0x0600D77F RID: 55167 RVA: 0x004D558D File Offset: 0x004D378D
		public void ShowAchievementsUi()
		{
			SDK.XGameUiShowAchievementsAsync(this.xblUser.UserHandle, 1745806870U, delegate(int _hresult)
			{
				XblHelpers.Succeeded(_hresult, "Open achievements UI", true, false);
			});
		}

		// Token: 0x0600D780 RID: 55168 RVA: 0x004D55C4 File Offset: 0x004D37C4
		public bool IsAchievementStatSupported(EnumAchievementDataStat _stat)
		{
			return _stat != EnumAchievementDataStat.HighestPlayerLevel;
		}

		// Token: 0x0600D781 RID: 55169 RVA: 0x004D55E0 File Offset: 0x004D37E0
		public void SetAchievementStat(EnumAchievementDataStat _stat, int _value)
		{
			if (!_stat.IsSupported())
			{
				return;
			}
			if (AchievementUtils.IsCreativeModeActive())
			{
				if (AchievementManager.debug != AchievementManager.EDebugLevel.Off && Time.unscaledTime - this.lastAchievementsDisabledWarningTime > 30f)
				{
					this.lastAchievementsDisabledWarningTime = Time.unscaledTime;
					Log.Warning("[XBL] Achievements disabled due to creative mode, creative menu or debug menu enabled");
				}
				return;
			}
			if (AchievementData.GetStatType(_stat) != EnumStatType.Int)
			{
				Log.Warning("AchievementManager.SetAchievementStat, int given for float type stat {0}", new object[] { _stat.ToStringCached<EnumAchievementDataStat>() });
				return;
			}
			AchievementManager.StatCacheEntry statCacheEntry;
			if (AchievementData.GetUpdateType(_stat) != AchievementData.EnumUpdateType.Sum && this.sentStatsCache.TryGetValue(_stat, out statCacheEntry) && statCacheEntry.iValue == _value)
			{
				if (AchievementManager.debug == AchievementManager.EDebugLevel.Verbose && Time.unscaledTime - statCacheEntry.lastSendTime > 30f)
				{
					this.sentStatsCache[_stat] = new AchievementManager.StatCacheEntry(_value, 0f, Time.unscaledTime);
					Log.Warning(string.Format("[XBL] Not sending achievement {0}, already sent with value {1}", _stat.ToStringCached<EnumAchievementDataStat>(), _value));
				}
				return;
			}
			if (XblHelpers.Succeeded(SDK.XBL.XblEventsWriteInGameEvent(this.xblUser.XblContextHandle, _stat.ToStringCached<EnumAchievementDataStat>(), string.Format("{{\"Value\":{0}}}", _value), "{}"), "Send int stat event '" + _stat.ToStringCached<EnumAchievementDataStat>() + "'", true, false))
			{
				this.sentStatsCache[_stat] = new AchievementManager.StatCacheEntry(_value, 0f, Time.unscaledTime);
				if (AchievementManager.debug == AchievementManager.EDebugLevel.Verbose)
				{
					Log.Out(string.Format("[XBL] Sent achievement update: {0} = {1}", _stat.ToStringCached<EnumAchievementDataStat>(), _value));
				}
			}
		}

		// Token: 0x0600D782 RID: 55170 RVA: 0x004D574C File Offset: 0x004D394C
		public void SetAchievementStat(EnumAchievementDataStat _stat, float _value)
		{
			if (!_stat.IsSupported())
			{
				return;
			}
			if (AchievementUtils.IsCreativeModeActive())
			{
				if (AchievementManager.debug != AchievementManager.EDebugLevel.Off && Time.unscaledTime - this.lastAchievementsDisabledWarningTime > 30f)
				{
					this.lastAchievementsDisabledWarningTime = Time.unscaledTime;
					Log.Warning("[XBL] Achievements disabled due to creative mode, creative menu or debug menu enabled");
				}
				return;
			}
			if (AchievementData.GetStatType(_stat) != EnumStatType.Float)
			{
				Log.Warning("AchievementManager.SetAchievementStat, float given for int type stat {0}", new object[] { _stat.ToStringCached<EnumAchievementDataStat>() });
				return;
			}
			AchievementManager.StatCacheEntry statCacheEntry;
			if (AchievementData.GetUpdateType(_stat) != AchievementData.EnumUpdateType.Sum && this.sentStatsCache.TryGetValue(_stat, out statCacheEntry) && statCacheEntry.fValue == _value)
			{
				if (AchievementManager.debug == AchievementManager.EDebugLevel.Verbose && Time.unscaledTime - statCacheEntry.lastSendTime > 30f)
				{
					this.sentStatsCache[_stat] = new AchievementManager.StatCacheEntry(0, _value, Time.unscaledTime);
					Log.Warning("[XBL] Not sending achievement " + _stat.ToStringCached<EnumAchievementDataStat>() + ", already sent with value " + _value.ToCultureInvariantString());
				}
				return;
			}
			if (XblHelpers.Succeeded(SDK.XBL.XblEventsWriteInGameEvent(this.xblUser.XblContextHandle, _stat.ToStringCached<EnumAchievementDataStat>(), "{\"Value\":" + _value.ToCultureInvariantString() + "}", "{}"), "Send float stat event '" + _stat.ToStringCached<EnumAchievementDataStat>() + "'", true, false))
			{
				this.sentStatsCache[_stat] = new AchievementManager.StatCacheEntry(0, _value, Time.unscaledTime);
				if (AchievementManager.debug == AchievementManager.EDebugLevel.Verbose)
				{
					Log.Out(string.Format("[XBL] Sent achievement update: {0} = {1}", _stat.ToStringCached<EnumAchievementDataStat>(), _value));
				}
			}
		}

		// Token: 0x0600D783 RID: 55171 RVA: 0x000027FC File Offset: 0x000009FC
		public void ResetStats(bool _andAchievements)
		{
		}

		// Token: 0x0600D784 RID: 55172 RVA: 0x004D58BC File Offset: 0x004D3ABC
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

		// Token: 0x0600D785 RID: 55173 RVA: 0x000027FC File Offset: 0x000009FC
		public void Destroy()
		{
		}

		// Token: 0x0400A3DE RID: 41950
		[PublicizedFrom(EAccessModifier.Private)]
		public const int suppressRepeatedNotSentWarningsTime = 30;

		// Token: 0x0400A3DF RID: 41951
		[PublicizedFrom(EAccessModifier.Private)]
		public User xblUser;

		// Token: 0x0400A3E0 RID: 41952
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly AchievementManager.EDebugLevel debug = AchievementManager.EDebugLevel.Off;

		// Token: 0x0400A3E1 RID: 41953
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<EnumAchievementDataStat, AchievementManager.StatCacheEntry> sentStatsCache = new EnumDictionary<EnumAchievementDataStat, AchievementManager.StatCacheEntry>();

		// Token: 0x0400A3E2 RID: 41954
		[PublicizedFrom(EAccessModifier.Private)]
		public float lastAchievementsDisabledWarningTime;

		// Token: 0x02001C5D RID: 7261
		[PublicizedFrom(EAccessModifier.Private)]
		public enum EDebugLevel
		{
			// Token: 0x0400A3E4 RID: 41956
			Off,
			// Token: 0x0400A3E5 RID: 41957
			Normal,
			// Token: 0x0400A3E6 RID: 41958
			Verbose
		}

		// Token: 0x02001C5E RID: 7262
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly struct StatCacheEntry
		{
			// Token: 0x0600D787 RID: 55175 RVA: 0x004D594A File Offset: 0x004D3B4A
			public StatCacheEntry(int _iValue, float _fValue, float _lastSendTime)
			{
				this.iValue = _iValue;
				this.fValue = _fValue;
				this.lastSendTime = _lastSendTime;
			}

			// Token: 0x0400A3E7 RID: 41959
			public readonly int iValue;

			// Token: 0x0400A3E8 RID: 41960
			public readonly float fValue;

			// Token: 0x0400A3E9 RID: 41961
			public readonly float lastSendTime;
		}
	}
}
