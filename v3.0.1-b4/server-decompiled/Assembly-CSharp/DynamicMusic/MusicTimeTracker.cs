using System;
using System.Collections.Generic;
using MusicUtils.Enums;
using UnityEngine;

namespace DynamicMusic
{
	// Token: 0x02001A2F RID: 6703
	public class MusicTimeTracker : AbstractMusicTimeTracker, IMultiNotifiableFilter, INotifiable, INotifiableFilter<MusicActionType, SectionType>, INotifiable<MusicActionType>, IFilter<SectionType>, IGamePrefsChangedListener
	{
		// Token: 0x0600CACB RID: 51915 RVA: 0x004A31C8 File Offset: 0x004A13C8
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

		// Token: 0x0600CACC RID: 51916 RVA: 0x004A328C File Offset: 0x004A148C
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnPlay()
		{
			this.musicStartTime = (float)AudioSettings.dspTime;
			this.IsMusicPlaying = true;
			this.pauseStartTime = (this.pauseDuration = 0f);
		}

		// Token: 0x0600CACD RID: 51917 RVA: 0x004A32C0 File Offset: 0x004A14C0
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnPause()
		{
			this.pauseStartTime = (float)AudioSettings.dspTime;
		}

		// Token: 0x0600CACE RID: 51918 RVA: 0x004A32CE File Offset: 0x004A14CE
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnUnPause()
		{
			this.pauseDuration += (float)AudioSettings.dspTime - this.pauseStartTime;
			this.pauseStartTime = 0f;
		}

		// Token: 0x0600CACF RID: 51919 RVA: 0x004A32F8 File Offset: 0x004A14F8
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnStop()
		{
			this.dailyPlayTimeUsed += (float)AudioSettings.dspTime - this.musicStartTime - this.pauseDuration;
			this.musicStartTime = (this.pauseDuration = 0f);
			this.IsMusicPlaying = false;
		}

		// Token: 0x0600CAD0 RID: 51920 RVA: 0x004A3341 File Offset: 0x004A1541
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnFadeIn()
		{
			if (!this.IsMusicPlaying)
			{
				this.musicStartTime = (float)AudioSettings.dspTime;
			}
		}

		// Token: 0x0600CAD1 RID: 51921 RVA: 0x004A3357 File Offset: 0x004A1557
		public void OnGamePrefChanged(EnumGamePrefs _enum)
		{
			if (_enum == EnumGamePrefs.OptionsDynamicMusicDailyTime || _enum == EnumGamePrefs.DayNightLength)
			{
				this.dailyAllottedPlayTime = GamePrefs.GetFloat(EnumGamePrefs.OptionsDynamicMusicDailyTime) * (float)GamePrefs.GetInt(EnumGamePrefs.DayNightLength);
			}
		}

		// Token: 0x0600CAD2 RID: 51922 RVA: 0x000027FC File Offset: 0x000009FC
		public void Cleanup()
		{
		}

		// Token: 0x0600CAD3 RID: 51923 RVA: 0x004A3380 File Offset: 0x004A1580
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

		// Token: 0x0600CAD4 RID: 51924 RVA: 0x004A33D3 File Offset: 0x004A15D3
		public void Notify()
		{
			this.dailyPlayTimeUsed = 0f;
			this.FrequencyLimiter.Notify();
		}

		// Token: 0x0600CAD5 RID: 51925 RVA: 0x004A33EC File Offset: 0x004A15EC
		public void Notify(MusicActionType _state)
		{
			Action action;
			if (this.MusicActions.TryGetValue(_state, out action))
			{
				action();
			}
			this.FrequencyLimiter.Notify(_state);
		}

		// Token: 0x0600CAD6 RID: 51926 RVA: 0x004A341C File Offset: 0x004A161C
		public override string ToString()
		{
			return string.Format("Daily Play Time Allotted: {0}\nPlay Time Used: {1}\nIs Music Playing: {2}\nMusic Start Time: {3}\nPause Start Time: {4}\nPause Duration: {5}\n", new object[] { this.dailyAllottedPlayTime, this.dailyPlayTimeUsed, this.IsMusicPlaying, this.musicStartTime, this.pauseStartTime, this.pauseDuration });
		}

		// Token: 0x04009A0D RID: 39437
		[PublicizedFrom(EAccessModifier.Private)]
		public IMultiNotifiableFilter FrequencyLimiter;

		// Token: 0x04009A0E RID: 39438
		[PublicizedFrom(EAccessModifier.Private)]
		public EnumDictionary<MusicActionType, Action> MusicActions;
	}
}
