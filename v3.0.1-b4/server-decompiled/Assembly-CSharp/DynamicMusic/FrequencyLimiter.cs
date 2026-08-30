using System;
using System.Collections.Generic;
using MusicUtils.Enums;
using UnityEngine;

namespace DynamicMusic
{
	// Token: 0x02001A25 RID: 6693
	public class FrequencyLimiter : AbstractFilter, IMultiNotifiableFilter, INotifiable, INotifiableFilter<MusicActionType, SectionType>, INotifiable<MusicActionType>, IFilter<SectionType>, IGamePrefsChangedListener
	{
		// Token: 0x0600CA90 RID: 51856 RVA: 0x004A2296 File Offset: 0x004A0496
		public FrequencyLimiter()
		{
			this.rng = GameRandomManager.Instance.CreateGameRandom();
			GamePrefs.AddChangeListener(this);
			this.UpdateParameters();
			this.UpdateRollTime();
		}

		// Token: 0x0600CA91 RID: 51857 RVA: 0x004A22C0 File Offset: 0x004A04C0
		public override List<SectionType> Filter(List<SectionType> _sectionTypes)
		{
			if (AudioSettings.dspTime <= this.rollTime || this.rng.RandomRange(1f) > this.chanceOfPositiveRoll)
			{
				for (int i = _sectionTypes.Count - 1; i >= 0; i--)
				{
					if (_sectionTypes[i] != SectionType.None && _sectionTypes[i] != SectionType.Combat)
					{
						_sectionTypes.RemoveAt(i);
					}
				}
			}
			return _sectionTypes;
		}

		// Token: 0x0600CA92 RID: 51858 RVA: 0x004A2320 File Offset: 0x004A0520
		[PublicizedFrom(EAccessModifier.Private)]
		public void UpdateRollTime()
		{
			this.rollTime = AudioSettings.dspTime + (double)this.cooldown;
		}

		// Token: 0x0600CA93 RID: 51859 RVA: 0x004A2335 File Offset: 0x004A0535
		public void Notify(MusicActionType _state)
		{
			if (_state.Equals(MusicActionType.Stop))
			{
				this.UpdateRollTime();
			}
		}

		// Token: 0x0600CA94 RID: 51860 RVA: 0x004A2352 File Offset: 0x004A0552
		public void Notify()
		{
			this.UpdateRollTime();
		}

		// Token: 0x0600CA95 RID: 51861 RVA: 0x004A235A File Offset: 0x004A055A
		public void OnGamePrefChanged(EnumGamePrefs _enum)
		{
			if (_enum.Equals(EnumGamePrefs.OptionsDynamicMusicDailyTime) || _enum.Equals(EnumGamePrefs.DayNightLength))
			{
				this.UpdateParameters();
			}
		}

		// Token: 0x0600CA96 RID: 51862 RVA: 0x004A2394 File Offset: 0x004A0594
		[PublicizedFrom(EAccessModifier.Private)]
		public void UpdateParameters()
		{
			this.dayLengthInSeconds = (float)GamePrefs.GetInt(EnumGamePrefs.DayNightLength) * 60f;
			this.dailyAllottedPlaySeconds = GamePrefs.GetFloat(EnumGamePrefs.OptionsDynamicMusicDailyTime) * this.dayLengthInSeconds;
			this.rollsPerDay = Mathf.Ceil(this.dailyAllottedPlaySeconds / 168f);
			float num = this.dayLengthInSeconds / this.rollsPerDay;
			this.cooldown = Mathf.Max(num - 168f, 30f);
			this.chanceOfPositiveRoll = (float)Math.Pow(0.8999999761581421, 1.0 / (double)this.rollsPerDay);
		}

		// Token: 0x040099DE RID: 39390
		[PublicizedFrom(EAccessModifier.Private)]
		public const float cMinCooldown = 30f;

		// Token: 0x040099DF RID: 39391
		[PublicizedFrom(EAccessModifier.Private)]
		public const float cProbOfFailingToReachDailyAllotted = 0.1f;

		// Token: 0x040099E0 RID: 39392
		[PublicizedFrom(EAccessModifier.Private)]
		public const float cPlayTime = 168f;

		// Token: 0x040099E1 RID: 39393
		[PublicizedFrom(EAccessModifier.Private)]
		public float dayLengthInSeconds;

		// Token: 0x040099E2 RID: 39394
		[PublicizedFrom(EAccessModifier.Private)]
		public float dailyAllottedPlaySeconds;

		// Token: 0x040099E3 RID: 39395
		[PublicizedFrom(EAccessModifier.Private)]
		public float rollsPerDay;

		// Token: 0x040099E4 RID: 39396
		[PublicizedFrom(EAccessModifier.Private)]
		public float cooldown;

		// Token: 0x040099E5 RID: 39397
		[PublicizedFrom(EAccessModifier.Private)]
		public float chanceOfPositiveRoll;

		// Token: 0x040099E6 RID: 39398
		[PublicizedFrom(EAccessModifier.Private)]
		public double rollTime;

		// Token: 0x040099E7 RID: 39399
		[PublicizedFrom(EAccessModifier.Private)]
		public GameRandom rng;
	}
}
