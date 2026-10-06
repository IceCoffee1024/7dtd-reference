using System;
using System.Collections.Generic;
using MusicUtils.Enums;
using UnityEngine;

namespace DynamicMusic
{
	// Token: 0x02001A82 RID: 6786
	public class MusicTimeTracker : AbstractMusicTimeTracker, IMultiNotifiableFilter, INotifiable, INotifiableFilter<MusicActionType, SectionType>, INotifiable<MusicActionType>, IFilter<SectionType>, IGamePrefsChangedListener
	{
		// Token: 0x0600CDAC RID: 52652 RVA: 0x004ADA88 File Offset: 0x004ABC88
		public MusicTimeTracker()
		{
			this.dailyAllottedPlayTime = GamePrefs.GetFloat(EnumGamePrefs.OptionsDynamicMusicDailyTime) * (float)GamePrefs.GetInt(EnumGamePrefs.DayNightLength) * 60f;
			this.MusicActions = new EnumDictionary<MusicActionType, Action>(4);
			this.MusicActions.Add(MusicActionType.Play, new Action(this.OnPlay));
			this.MusicActions.Add(MusicActionType.Pause, new Action(this.OnPause));
			this.MusicActions.Add(MusicActionType.UnPause, new Action(this.OnUnPause));
			this.MusicActions.Add(MusicActionType.Stop, new Action(this.OnStop));
			this.MusicActions.Add(MusicActionType.FadeIn, new Action(this.OnFadeIn));
			this.FrequencyLimiter = new FrequencyLimiter();
		}

		// Token: 0x0600CDAD RID: 52653 RVA: 0x004ADB4C File Offset: 0x004ABD4C
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnPlay()
		{
			this.musicStartTime = (float)AudioSettings.dspTime;
			this.IsMusicPlaying = true;
			this.pauseStartTime = (this.pauseDuration = 0f);
		}

		// Token: 0x0600CDAE RID: 52654 RVA: 0x004ADB80 File Offset: 0x004ABD80
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnPause()
		{
			this.pauseStartTime = (float)AudioSettings.dspTime;
		}

		// Token: 0x0600CDAF RID: 52655 RVA: 0x004ADB8E File Offset: 0x004ABD8E
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnUnPause()
		{
			this.pauseDuration += (float)AudioSettings.dspTime - this.pauseStartTime;
			this.pauseStartTime = 0f;
		}

		// Token: 0x0600CDB0 RID: 52656 RVA: 0x004ADBB8 File Offset: 0x004ABDB8
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnStop()
		{
			this.dailyPlayTimeUsed += (float)AudioSettings.dspTime - this.musicStartTime - this.pauseDuration;
			this.musicStartTime = (this.pauseDuration = 0f);
			this.IsMusicPlaying = false;
		}

		// Token: 0x0600CDB1 RID: 52657 RVA: 0x004ADC01 File Offset: 0x004ABE01
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnFadeIn()
		{
			if (!this.IsMusicPlaying)
			{
				this.musicStartTime = (float)AudioSettings.dspTime;
			}
		}

		// Token: 0x0600CDB2 RID: 52658 RVA: 0x004ADC17 File Offset: 0x004ABE17
		public void OnGamePrefChanged(EnumGamePrefs _enum)
		{
			if (_enum == EnumGamePrefs.OptionsDynamicMusicDailyTime || _enum == EnumGamePrefs.DayNightLength)
			{
				this.dailyAllottedPlayTime = GamePrefs.GetFloat(EnumGamePrefs.OptionsDynamicMusicDailyTime) * (float)GamePrefs.GetInt(EnumGamePrefs.DayNightLength);
			}
		}

		// Token: 0x0600CDB3 RID: 52659 RVA: 0x000027FC File Offset: 0x000009FC
		public void Cleanup()
		{
		}

		// Token: 0x0600CDB4 RID: 52660 RVA: 0x004ADC40 File Offset: 0x004ABE40
		public override List<SectionType> Filter(List<SectionType> _sectionTypes)
		{
			if (!this.IsMusicPlaying)
			{
				if (this.dailyPlayTimeUsed < this.dailyAllottedPlayTime)
				{
					this.FrequencyLimiter.Filter(_sectionTypes);
				}
				else
				{
					_sectionTypes.Remove(SectionType.HomeDay);
					_sectionTypes.Remove(SectionType.HomeNight);
					_sectionTypes.Remove(SectionType.Exploration);
					_sectionTypes.Remove(SectionType.Suspense);
				}
			}
			return _sectionTypes;
		}

		// Token: 0x0600CDB5 RID: 52661 RVA: 0x004ADC93 File Offset: 0x004ABE93
		public void Notify()
		{
			this.dailyPlayTimeUsed = 0f;
			this.FrequencyLimiter.Notify();
		}

		// Token: 0x0600CDB6 RID: 52662 RVA: 0x004ADCAC File Offset: 0x004ABEAC
		public void Notify(MusicActionType _state)
		{
			Action action;
			if (this.MusicActions.TryGetValue(_state, out action))
			{
				action();
			}
			this.FrequencyLimiter.Notify(_state);
		}

		// Token: 0x0600CDB7 RID: 52663 RVA: 0x004ADCDC File Offset: 0x004ABEDC
		public override string ToString()
		{
			return string.Format("Daily Play Time Allotted: {0}\nPlay Time Used: {1}\nIs Music Playing: {2}\nMusic Start Time: {3}\nPause Start Time: {4}\nPause Duration: {5}\n", new object[] { this.dailyAllottedPlayTime, this.dailyPlayTimeUsed, this.IsMusicPlaying, this.musicStartTime, this.pauseStartTime, this.pauseDuration });
		}

		// Token: 0x04009C04 RID: 39940
		[PublicizedFrom(EAccessModifier.Private)]
		public IMultiNotifiableFilter FrequencyLimiter;

		// Token: 0x04009C05 RID: 39941
		[PublicizedFrom(EAccessModifier.Private)]
		public EnumDictionary<MusicActionType, Action> MusicActions;
	}
}
