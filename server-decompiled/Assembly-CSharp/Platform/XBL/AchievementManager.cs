using System;
using System.Collections.Generic;
using Unity.XGamingRuntime;
using UnityEngine;

namespace Platform.XBL
{
	// Token: 0x02001C2A RID: 7210
	public class AchievementManager : IAchievementManager
	{
		// Token: 0x0600D605 RID: 54789 RVA: 0x004D0A2C File Offset: 0x004CEC2C
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

		// Token: 0x0600D606 RID: 54790 RVA: 0x004D0A68 File Offset: 0x004CEC68
		public void Init(IPlatform _owner)
		{
			_owner.User.UserLoggedIn += delegate(IPlatform _sender)
			{
				this.xblUser = (User)_owner.User;
			};
		}

		// Token: 0x0600D607 RID: 54791 RVA: 0x004D0AA5 File Offset: 0x004CECA5
		public void ShowAchievementsUi()
		{
			SDK.XGameUiShowAchievementsAsync(this.xblUser.UserHandle, 1745806870U, delegate(int _hresult)
			{
				XblHelpers.Succeeded(_hresult, "Open achievements UI", true, false);
			});
		}

		// Token: 0x0600D608 RID: 54792 RVA: 0x004D0ADC File Offset: 0x004CECDC
		public bool IsAchievementStatSupported(EnumAchievementDataStat _stat)
		{
			return _stat != EnumAchievementDataStat.HighestPlayerLevel;
		}

		// Token: 0x0600D609 RID: 54793 RVA: 0x004D0AF8 File Offset: 0x004CECF8
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

		// Token: 0x0600D60A RID: 54794 RVA: 0x004D0C64 File Offset: 0x004CEE64
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

		// Token: 0x0600D60B RID: 54795 RVA: 0x000027FC File Offset: 0x000009FC
		public void ResetStats(bool _andAchievements)
		{
		}

		// Token: 0x0600D60C RID: 54796 RVA: 0x004D0DD4 File Offset: 0x004CEFD4
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

		// Token: 0x0600D60D RID: 54797 RVA: 0x000027FC File Offset: 0x000009FC
		public void Destroy()
		{
		}

		// Token: 0x0400A309 RID: 41737
		[PublicizedFrom(EAccessModifier.Private)]
		public const int suppressRepeatedNotSentWarningsTime = 30;

		// Token: 0x0400A30A RID: 41738
		[PublicizedFrom(EAccessModifier.Private)]
		public User xblUser;

		// Token: 0x0400A30B RID: 41739
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly AchievementManager.EDebugLevel debug = AchievementManager.EDebugLevel.Off;

		// Token: 0x0400A30C RID: 41740
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<EnumAchievementDataStat, AchievementManager.StatCacheEntry> sentStatsCache = new EnumDictionary<EnumAchievementDataStat, AchievementManager.StatCacheEntry>();

		// Token: 0x0400A30D RID: 41741
		[PublicizedFrom(EAccessModifier.Private)]
		public float lastAchievementsDisabledWarningTime;

		// Token: 0x02001C2B RID: 7211
		[PublicizedFrom(EAccessModifier.Private)]
		public enum EDebugLevel
		{
			// Token: 0x0400A30F RID: 41743
			Off,
			// Token: 0x0400A310 RID: 41744
			Normal,
			// Token: 0x0400A311 RID: 41745
			Verbose
		}

		// Token: 0x02001C2C RID: 7212
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly struct StatCacheEntry
		{
			// Token: 0x0600D60F RID: 54799 RVA: 0x004D0E62 File Offset: 0x004CF062
			public StatCacheEntry(int _iValue, float _fValue, float _lastSendTime)
			{
				this.iValue = _iValue;
				this.fValue = _fValue;
				this.lastSendTime = _lastSendTime;
			}

			// Token: 0x0400A312 RID: 41746
			public readonly int iValue;

			// Token: 0x0400A313 RID: 41747
			public readonly float fValue;

			// Token: 0x0400A314 RID: 41748
			public readonly float lastSendTime;
		}
	}
}
