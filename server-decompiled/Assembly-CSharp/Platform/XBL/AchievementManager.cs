using System;
using System.Collections.Generic;
using Unity.XGamingRuntime;
using UnityEngine;

namespace Platform.XBL
{
	// Token: 0x02001C8D RID: 7309
	public class AchievementManager : IAchievementManager
	{
		// Token: 0x0600D891 RID: 55441 RVA: 0x004D903C File Offset: 0x004D723C
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

		// Token: 0x0600D892 RID: 55442 RVA: 0x004D9078 File Offset: 0x004D7278
		public void Init(IPlatform _owner)
		{
			_owner.User.UserLoggedIn += delegate(IPlatform _sender)
			{
				this.xblUser = (User)_owner.User;
			};
		}

		// Token: 0x0600D893 RID: 55443 RVA: 0x004D90B5 File Offset: 0x004D72B5
		public void ShowAchievementsUi()
		{
			SDK.XGameUiShowAchievementsAsync(this.xblUser.UserHandle, 1745806870U, delegate(int _hresult)
			{
				XblHelpers.Succeeded(_hresult, "Open achievements UI", true, false);
			});
		}

		// Token: 0x0600D894 RID: 55444 RVA: 0x004D90EC File Offset: 0x004D72EC
		public bool IsAchievementStatSupported(EnumAchievementDataStat _stat)
		{
			return _stat != EnumAchievementDataStat.HighestPlayerLevel;
		}

		// Token: 0x0600D895 RID: 55445 RVA: 0x004D9108 File Offset: 0x004D7308
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

		// Token: 0x0600D896 RID: 55446 RVA: 0x004D9274 File Offset: 0x004D7474
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

		// Token: 0x0600D897 RID: 55447 RVA: 0x000027FC File Offset: 0x000009FC
		public void ResetStats(bool _andAchievements)
		{
		}

		// Token: 0x0600D898 RID: 55448 RVA: 0x004D93E4 File Offset: 0x004D75E4
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

		// Token: 0x0600D899 RID: 55449 RVA: 0x000027FC File Offset: 0x000009FC
		public void Destroy()
		{
		}

		// Token: 0x0400A4C7 RID: 42183
		[PublicizedFrom(EAccessModifier.Private)]
		public const int suppressRepeatedNotSentWarningsTime = 30;

		// Token: 0x0400A4C8 RID: 42184
		[PublicizedFrom(EAccessModifier.Private)]
		public User xblUser;

		// Token: 0x0400A4C9 RID: 42185
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly AchievementManager.EDebugLevel debug = AchievementManager.EDebugLevel.Off;

		// Token: 0x0400A4CA RID: 42186
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<EnumAchievementDataStat, AchievementManager.StatCacheEntry> sentStatsCache = new EnumDictionary<EnumAchievementDataStat, AchievementManager.StatCacheEntry>();

		// Token: 0x0400A4CB RID: 42187
		[PublicizedFrom(EAccessModifier.Private)]
		public float lastAchievementsDisabledWarningTime;

		// Token: 0x02001C8E RID: 7310
		[PublicizedFrom(EAccessModifier.Private)]
		public enum EDebugLevel
		{
			// Token: 0x0400A4CD RID: 42189
			Off,
			// Token: 0x0400A4CE RID: 42190
			Normal,
			// Token: 0x0400A4CF RID: 42191
			Verbose
		}

		// Token: 0x02001C8F RID: 7311
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly struct StatCacheEntry
		{
			// Token: 0x0600D89B RID: 55451 RVA: 0x004D9472 File Offset: 0x004D7672
			public StatCacheEntry(int _iValue, float _fValue, float _lastSendTime)
			{
				this.iValue = _iValue;
				this.fValue = _fValue;
				this.lastSendTime = _lastSendTime;
			}

			// Token: 0x0400A4D0 RID: 42192
			public readonly int iValue;

			// Token: 0x0400A4D1 RID: 42193
			public readonly float fValue;

			// Token: 0x0400A4D2 RID: 42194
			public readonly float lastSendTime;
		}
	}
}
