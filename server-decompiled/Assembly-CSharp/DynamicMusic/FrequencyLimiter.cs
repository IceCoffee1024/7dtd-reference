using System;
using System.Collections.Generic;
using MusicUtils.Enums;
using UnityEngine;

namespace DynamicMusic
{
	// Token: 0x02001A78 RID: 6776
	public class FrequencyLimiter : AbstractFilter, IMultiNotifiableFilter, INotifiable, INotifiableFilter<MusicActionType, SectionType>, INotifiable<MusicActionType>, IFilter<SectionType>, IGamePrefsChangedListener
	{
		// Token: 0x0600CD71 RID: 52593 RVA: 0x004ACB56 File Offset: 0x004AAD56
		public FrequencyLimiter()
		{
			this.rng = GameRandomManager.Instance.CreateGameRandom();
			GamePrefs.AddChangeListener(this);
			this.UpdateParameters();
			this.UpdateRollTime();
		}

		// Token: 0x0600CD72 RID: 52594 RVA: 0x004ACB80 File Offset: 0x004AAD80
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

		// Token: 0x0600CD73 RID: 52595 RVA: 0x004ACBE0 File Offset: 0x004AADE0
		[PublicizedFrom(EAccessModifier.Private)]
		public void UpdateRollTime()
		{
			this.rollTime = AudioSettings.dspTime + (double)this.cooldown;
		}

		// Token: 0x0600CD74 RID: 52596 RVA: 0x004ACBF5 File Offset: 0x004AADF5
		public void Notify(MusicActionType _state)
		{
			if (_state.Equals(MusicActionType.Stop))
			{
				this.UpdateRollTime();
			}
		}

		// Token: 0x0600CD75 RID: 52597 RVA: 0x004ACC12 File Offset: 0x004AAE12
		public void Notify()
		{
			this.UpdateRollTime();
		}

		// Token: 0x0600CD76 RID: 52598 RVA: 0x004ACC1A File Offset: 0x004AAE1A
		public void OnGamePrefChanged(EnumGamePrefs _enum)
		{
			if (_enum.Equals(EnumGamePrefs.OptionsDynamicMusicDailyTime) || _enum.Equals(EnumGamePrefs.DayNightLength))
			{
				this.UpdateParameters();
			}
		}

		// Token: 0x0600CD77 RID: 52599 RVA: 0x004ACC54 File Offset: 0x004AAE54
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

		// Token: 0x04009BD5 RID: 39893
		[PublicizedFrom(EAccessModifier.Private)]
		public const float cMinCooldown = 30f;

		// Token: 0x04009BD6 RID: 39894
		[PublicizedFrom(EAccessModifier.Private)]
		public const float cProbOfFailingToReachDailyAllotted = 0.1f;

		// Token: 0x04009BD7 RID: 39895
		[PublicizedFrom(EAccessModifier.Private)]
		public const float cPlayTime = 168f;

		// Token: 0x04009BD8 RID: 39896
		[PublicizedFrom(EAccessModifier.Private)]
		public float dayLengthInSeconds;

		// Token: 0x04009BD9 RID: 39897
		[PublicizedFrom(EAccessModifier.Private)]
		public float dailyAllottedPlaySeconds;

		// Token: 0x04009BDA RID: 39898
		[PublicizedFrom(EAccessModifier.Private)]
		public float rollsPerDay;

		// Token: 0x04009BDB RID: 39899
		[PublicizedFrom(EAccessModifier.Private)]
		public float cooldown;

		// Token: 0x04009BDC RID: 39900
		[PublicizedFrom(EAccessModifier.Private)]
		public float chanceOfPositiveRoll;

		// Token: 0x04009BDD RID: 39901
		[PublicizedFrom(EAccessModifier.Private)]
		public double rollTime;

		// Token: 0x04009BDE RID: 39902
		[PublicizedFrom(EAccessModifier.Private)]
		public GameRandom rng;
	}
}
