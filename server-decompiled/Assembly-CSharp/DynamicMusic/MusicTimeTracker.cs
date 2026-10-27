using System;
using System.Collections.Generic;
using MusicUtils.Enums;
using UnityEngine;

namespace DynamicMusic
{
	// Token: 0x02001AB3 RID: 6835
	public class MusicTimeTracker : AbstractMusicTimeTracker, IMultiNotifiableFilter, INotifiable, INotifiableFilter<MusicActionType, SectionType>, INotifiable<MusicActionType>, IFilter<SectionType>, IGamePrefsChangedListener
	{
		// Token: 0x0600CEC0 RID: 52928 RVA: 0x004B1584 File Offset: 0x004AF784
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

		// Token: 0x0600CEC1 RID: 52929 RVA: 0x004B1648 File Offset: 0x004AF848
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnPlay()
		{
			this.musicStartTime = (float)AudioSettings.dspTime;
			this.IsMusicPlaying = true;
			this.pauseStartTime = (this.pauseDuration = 0f);
		}

		// Token: 0x0600CEC2 RID: 52930 RVA: 0x004B167C File Offset: 0x004AF87C
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnPause()
		{
			this.pauseStartTime = (float)AudioSettings.dspTime;
		}

		// Token: 0x0600CEC3 RID: 52931 RVA: 0x004B168A File Offset: 0x004AF88A
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnUnPause()
		{
			this.pauseDuration += (float)AudioSettings.dspTime - this.pauseStartTime;
			this.pauseStartTime = 0f;
		}

		// Token: 0x0600CEC4 RID: 52932 RVA: 0x004B16B4 File Offset: 0x004AF8B4
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnStop()
		{
			this.dailyPlayTimeUsed += (float)AudioSettings.dspTime - this.musicStartTime - this.pauseDuration;
			this.musicStartTime = (this.pauseDuration = 0f);
			this.IsMusicPlaying = false;
		}

		// Token: 0x0600CEC5 RID: 52933 RVA: 0x004B16FD File Offset: 0x004AF8FD
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnFadeIn()
		{
			if (!this.IsMusicPlaying)
			{
				this.musicStartTime = (float)AudioSettings.dspTime;
			}
		}

		// Token: 0x0600CEC6 RID: 52934 RVA: 0x004B1713 File Offset: 0x004AF913
		public void OnGamePrefChanged(EnumGamePrefs _enum)
		{
			if (_enum == EnumGamePrefs.OptionsDynamicMusicDailyTime || _enum == EnumGamePrefs.DayNightLength)
			{
				this.dailyAllottedPlayTime = GamePrefs.GetFloat(EnumGamePrefs.OptionsDynamicMusicDailyTime) * (float)GamePrefs.GetInt(EnumGamePrefs.DayNightLength);
			}
		}

		// Token: 0x0600CEC7 RID: 52935 RVA: 0x000027FC File Offset: 0x000009FC
		public void Cleanup()
		{
		}

		// Token: 0x0600CEC8 RID: 52936 RVA: 0x004B173C File Offset: 0x004AF93C
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

		// Token: 0x0600CEC9 RID: 52937 RVA: 0x004B178F File Offset: 0x004AF98F
		public void Notify()
		{
			this.dailyPlayTimeUsed = 0f;
			this.FrequencyLimiter.Notify();
		}

		// Token: 0x0600CECA RID: 52938 RVA: 0x004B17A8 File Offset: 0x004AF9A8
		public void Notify(MusicActionType _state)
		{
			Action action;
			if (this.MusicActions.TryGetValue(_state, out action))
			{
				action();
			}
			this.FrequencyLimiter.Notify(_state);
		}

		// Token: 0x0600CECB RID: 52939 RVA: 0x004B17D8 File Offset: 0x004AF9D8
		public override string ToString()
		{
			return string.Format("Daily Play Time Allotted: {0}\nPlay Time Used: {1}\nIs Music Playing: {2}\nMusic Start Time: {3}\nPause Start Time: {4}\nPause Duration: {5}\n", new object[] { this.dailyAllottedPlayTime, this.dailyPlayTimeUsed, this.IsMusicPlaying, this.musicStartTime, this.pauseStartTime, this.pauseDuration });
		}

		// Token: 0x04009CED RID: 40173
		[PublicizedFrom(EAccessModifier.Private)]
		public IMultiNotifiableFilter FrequencyLimiter;

		// Token: 0x04009CEE RID: 40174
		[PublicizedFrom(EAccessModifier.Private)]
		public EnumDictionary<MusicActionType, Action> MusicActions;
	}
}
