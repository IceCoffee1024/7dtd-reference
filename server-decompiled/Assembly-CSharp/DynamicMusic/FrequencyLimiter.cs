using System;
using System.Collections.Generic;
using MusicUtils.Enums;
using UnityEngine;

namespace DynamicMusic
{
	// Token: 0x02001A4C RID: 6732
	public class FrequencyLimiter : AbstractFilter, IMultiNotifiableFilter, INotifiable, INotifiableFilter<MusicActionType, SectionType>, INotifiable<MusicActionType>, IFilter<SectionType>, IGamePrefsChangedListener
	{
		// Token: 0x0600CC19 RID: 52249 RVA: 0x004A868A File Offset: 0x004A688A
		public FrequencyLimiter()
		{
			this.rng = GameRandomManager.Instance.CreateGameRandom();
			GamePrefs.AddChangeListener(this);
			this.UpdateParameters();
			this.UpdateRollTime();
		}

		// Token: 0x0600CC1A RID: 52250 RVA: 0x004A86B4 File Offset: 0x004A68B4
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

		// Token: 0x0600CC1B RID: 52251 RVA: 0x004A8714 File Offset: 0x004A6914
		[PublicizedFrom(EAccessModifier.Private)]
		public void UpdateRollTime()
		{
			this.rollTime = AudioSettings.dspTime + (double)this.cooldown;
		}

		// Token: 0x0600CC1C RID: 52252 RVA: 0x004A8729 File Offset: 0x004A6929
		public void Notify(MusicActionType _state)
		{
			if (_state.Equals(MusicActionType.Stop))
			{
				this.UpdateRollTime();
			}
		}

		// Token: 0x0600CC1D RID: 52253 RVA: 0x004A8746 File Offset: 0x004A6946
		public void Notify()
		{
			this.UpdateRollTime();
		}

		// Token: 0x0600CC1E RID: 52254 RVA: 0x004A874E File Offset: 0x004A694E
		public void OnGamePrefChanged(EnumGamePrefs _enum)
		{
			if (_enum.Equals(EnumGamePrefs.OptionsDynamicMusicDailyTime) || _enum.Equals(EnumGamePrefs.DayNightLength))
			{
				this.UpdateParameters();
			}
		}

		// Token: 0x0600CC1F RID: 52255 RVA: 0x004A8788 File Offset: 0x004A6988
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

		// Token: 0x04009B0F RID: 39695
		[PublicizedFrom(EAccessModifier.Private)]
		public const float cMinCooldown = 30f;

		// Token: 0x04009B10 RID: 39696
		[PublicizedFrom(EAccessModifier.Private)]
		public const float cProbOfFailingToReachDailyAllotted = 0.1f;

		// Token: 0x04009B11 RID: 39697
		[PublicizedFrom(EAccessModifier.Private)]
		public const float cPlayTime = 168f;

		// Token: 0x04009B12 RID: 39698
		[PublicizedFrom(EAccessModifier.Private)]
		public float dayLengthInSeconds;

		// Token: 0x04009B13 RID: 39699
		[PublicizedFrom(EAccessModifier.Private)]
		public float dailyAllottedPlaySeconds;

		// Token: 0x04009B14 RID: 39700
		[PublicizedFrom(EAccessModifier.Private)]
		public float rollsPerDay;

		// Token: 0x04009B15 RID: 39701
		[PublicizedFrom(EAccessModifier.Private)]
		public float cooldown;

		// Token: 0x04009B16 RID: 39702
		[PublicizedFrom(EAccessModifier.Private)]
		public float chanceOfPositiveRoll;

		// Token: 0x04009B17 RID: 39703
		[PublicizedFrom(EAccessModifier.Private)]
		public double rollTime;

		// Token: 0x04009B18 RID: 39704
		[PublicizedFrom(EAccessModifier.Private)]
		public GameRandom rng;
	}
}
