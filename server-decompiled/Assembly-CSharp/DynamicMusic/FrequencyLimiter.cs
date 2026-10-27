using System;
using System.Collections.Generic;
using MusicUtils.Enums;
using UnityEngine;

namespace DynamicMusic
{
	// Token: 0x02001AA9 RID: 6825
	public class FrequencyLimiter : AbstractFilter, IMultiNotifiableFilter, INotifiable, INotifiableFilter<MusicActionType, SectionType>, INotifiable<MusicActionType>, IFilter<SectionType>, IGamePrefsChangedListener
	{
		// Token: 0x0600CE85 RID: 52869 RVA: 0x004B0652 File Offset: 0x004AE852
		public FrequencyLimiter()
		{
			this.rng = GameRandomManager.Instance.CreateGameRandom();
			GamePrefs.AddChangeListener(this);
			this.UpdateParameters();
			this.UpdateRollTime();
		}

		// Token: 0x0600CE86 RID: 52870 RVA: 0x004B067C File Offset: 0x004AE87C
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

		// Token: 0x0600CE87 RID: 52871 RVA: 0x004B06DC File Offset: 0x004AE8DC
		[PublicizedFrom(EAccessModifier.Private)]
		public void UpdateRollTime()
		{
			this.rollTime = AudioSettings.dspTime + (double)this.cooldown;
		}

		// Token: 0x0600CE88 RID: 52872 RVA: 0x004B06F1 File Offset: 0x004AE8F1
		public void Notify(MusicActionType _state)
		{
			if (_state.Equals(MusicActionType.Stop))
			{
				this.UpdateRollTime();
			}
		}

		// Token: 0x0600CE89 RID: 52873 RVA: 0x004B070E File Offset: 0x004AE90E
		public void Notify()
		{
			this.UpdateRollTime();
		}

		// Token: 0x0600CE8A RID: 52874 RVA: 0x004B0716 File Offset: 0x004AE916
		public void OnGamePrefChanged(EnumGamePrefs _enum)
		{
			if (_enum.Equals(EnumGamePrefs.OptionsDynamicMusicDailyTime) || _enum.Equals(EnumGamePrefs.DayNightLength))
			{
				this.UpdateParameters();
			}
		}

		// Token: 0x0600CE8B RID: 52875 RVA: 0x004B0750 File Offset: 0x004AE950
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

		// Token: 0x04009CBE RID: 40126
		[PublicizedFrom(EAccessModifier.Private)]
		public const float cMinCooldown = 30f;

		// Token: 0x04009CBF RID: 40127
		[PublicizedFrom(EAccessModifier.Private)]
		public const float cProbOfFailingToReachDailyAllotted = 0.1f;

		// Token: 0x04009CC0 RID: 40128
		[PublicizedFrom(EAccessModifier.Private)]
		public const float cPlayTime = 168f;

		// Token: 0x04009CC1 RID: 40129
		[PublicizedFrom(EAccessModifier.Private)]
		public float dayLengthInSeconds;

		// Token: 0x04009CC2 RID: 40130
		[PublicizedFrom(EAccessModifier.Private)]
		public float dailyAllottedPlaySeconds;

		// Token: 0x04009CC3 RID: 40131
		[PublicizedFrom(EAccessModifier.Private)]
		public float rollsPerDay;

		// Token: 0x04009CC4 RID: 40132
		[PublicizedFrom(EAccessModifier.Private)]
		public float cooldown;

		// Token: 0x04009CC5 RID: 40133
		[PublicizedFrom(EAccessModifier.Private)]
		public float chanceOfPositiveRoll;

		// Token: 0x04009CC6 RID: 40134
		[PublicizedFrom(EAccessModifier.Private)]
		public double rollTime;

		// Token: 0x04009CC7 RID: 40135
		[PublicizedFrom(EAccessModifier.Private)]
		public GameRandom rng;
	}
}
