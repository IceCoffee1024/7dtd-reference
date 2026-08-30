using System;
using System.Collections.Generic;
using MusicUtils.Enums;
using UnityEngine;

namespace DynamicMusic
{
	// Token: 0x02001A56 RID: 6742
	public class MusicTimeTracker : AbstractMusicTimeTracker, IMultiNotifiableFilter, INotifiable, INotifiableFilter<MusicActionType, SectionType>, INotifiable<MusicActionType>, IFilter<SectionType>, IGamePrefsChangedListener
	{
		// Token: 0x0600CC54 RID: 52308 RVA: 0x004A95BC File Offset: 0x004A77BC
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

		// Token: 0x0600CC55 RID: 52309 RVA: 0x004A9680 File Offset: 0x004A7880
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnPlay()
		{
			this.musicStartTime = (float)AudioSettings.dspTime;
			this.IsMusicPlaying = true;
			this.pauseStartTime = (this.pauseDuration = 0f);
		}

		// Token: 0x0600CC56 RID: 52310 RVA: 0x004A96B4 File Offset: 0x004A78B4
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnPause()
		{
			this.pauseStartTime = (float)AudioSettings.dspTime;
		}

		// Token: 0x0600CC57 RID: 52311 RVA: 0x004A96C2 File Offset: 0x004A78C2
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnUnPause()
		{
			this.pauseDuration += (float)AudioSettings.dspTime - this.pauseStartTime;
			this.pauseStartTime = 0f;
		}

		// Token: 0x0600CC58 RID: 52312 RVA: 0x004A96EC File Offset: 0x004A78EC
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnStop()
		{
			this.dailyPlayTimeUsed += (float)AudioSettings.dspTime - this.musicStartTime - this.pauseDuration;
			this.musicStartTime = (this.pauseDuration = 0f);
			this.IsMusicPlaying = false;
		}

		// Token: 0x0600CC59 RID: 52313 RVA: 0x004A9735 File Offset: 0x004A7935
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnFadeIn()
		{
			if (!this.IsMusicPlaying)
			{
				this.musicStartTime = (float)AudioSettings.dspTime;
			}
		}

		// Token: 0x0600CC5A RID: 52314 RVA: 0x004A974B File Offset: 0x004A794B
		public void OnGamePrefChanged(EnumGamePrefs _enum)
		{
			if (_enum == EnumGamePrefs.OptionsDynamicMusicDailyTime || _enum == EnumGamePrefs.DayNightLength)
			{
				this.dailyAllottedPlayTime = GamePrefs.GetFloat(EnumGamePrefs.OptionsDynamicMusicDailyTime) * (float)GamePrefs.GetInt(EnumGamePrefs.DayNightLength);
			}
		}

		// Token: 0x0600CC5B RID: 52315 RVA: 0x000027FC File Offset: 0x000009FC
		public void Cleanup()
		{
		}

		// Token: 0x0600CC5C RID: 52316 RVA: 0x004A9774 File Offset: 0x004A7974
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

		// Token: 0x0600CC5D RID: 52317 RVA: 0x004A97C7 File Offset: 0x004A79C7
		public void Notify()
		{
			this.dailyPlayTimeUsed = 0f;
			this.FrequencyLimiter.Notify();
		}

		// Token: 0x0600CC5E RID: 52318 RVA: 0x004A97E0 File Offset: 0x004A79E0
		public void Notify(MusicActionType _state)
		{
			Action action;
			if (this.MusicActions.TryGetValue(_state, out action))
			{
				action();
			}
			this.FrequencyLimiter.Notify(_state);
		}

		// Token: 0x0600CC5F RID: 52319 RVA: 0x004A9810 File Offset: 0x004A7A10
		public override string ToString()
		{
			return string.Format("Daily Play Time Allotted: {0}\nPlay Time Used: {1}\nIs Music Playing: {2}\nMusic Start Time: {3}\nPause Start Time: {4}\nPause Duration: {5}\n", new object[] { this.dailyAllottedPlayTime, this.dailyPlayTimeUsed, this.IsMusicPlaying, this.musicStartTime, this.pauseStartTime, this.pauseDuration });
		}

		// Token: 0x04009B3E RID: 39742
		[PublicizedFrom(EAccessModifier.Private)]
		public IMultiNotifiableFilter FrequencyLimiter;

		// Token: 0x04009B3F RID: 39743
		[PublicizedFrom(EAccessModifier.Private)]
		public EnumDictionary<MusicActionType, Action> MusicActions;
	}
}
