using System;
using UnityEngine;

namespace DynamicMusic.Legacy
{
	// Token: 0x02001A6D RID: 6765
	public class FrequencyManager
	{
		// Token: 0x170018CC RID: 6348
		// (get) Token: 0x0600CBBF RID: 52159 RVA: 0x004A4ED0 File Offset: 0x004A30D0
		public float DailyTimePercentage
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return GamePrefs.GetFloat(EnumGamePrefs.OptionsDynamicMusicDailyTime);
			}
		}

		// Token: 0x170018CD RID: 6349
		// (get) Token: 0x0600CBC0 RID: 52160 RVA: 0x004A4EDC File Offset: 0x004A30DC
		public int MinutesPerDay
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return GamePrefs.GetInt(EnumGamePrefs.DayNightLength);
			}
		}

		// Token: 0x170018CE RID: 6350
		// (get) Token: 0x0600CBC1 RID: 52161 RVA: 0x004A4EE5 File Offset: 0x004A30E5
		public bool IsMusicPlayingThisTick
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return this.dynamicMusicManager.IsMusicPlayingThisTick;
			}
		}

		// Token: 0x170018CF RID: 6351
		// (get) Token: 0x0600CBC2 RID: 52162 RVA: 0x004A4EF2 File Offset: 0x004A30F2
		public bool MusicStarted
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return this.dynamicMusicManager.MusicStarted;
			}
		}

		// Token: 0x170018D0 RID: 6352
		// (get) Token: 0x0600CBC3 RID: 52163 RVA: 0x004A4EFF File Offset: 0x004A30FF
		public bool MusicStopped
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return this.dynamicMusicManager.MusicStopped;
			}
		}

		// Token: 0x170018D1 RID: 6353
		// (get) Token: 0x0600CBC4 RID: 52164 RVA: 0x004A4F0C File Offset: 0x004A310C
		public bool IsMusicScheduled
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return this.CanScheduleTrack && !this.IsInCoolDown;
			}
		}

		// Token: 0x170018D2 RID: 6354
		// (get) Token: 0x0600CBC5 RID: 52165 RVA: 0x004A4F21 File Offset: 0x004A3121
		public bool DidDayChange
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return this.currentDay != GameUtils.WorldTimeToDays(GameManager.Instance.World.worldTime);
			}
		}

		// Token: 0x170018D3 RID: 6355
		// (get) Token: 0x0600CBC6 RID: 52166 RVA: 0x004A4F42 File Offset: 0x004A3142
		public float DailyTimeAllotted
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return this.DailyTimePercentage * (float)this.MinutesPerDay;
			}
		}

		// Token: 0x170018D4 RID: 6356
		// (get) Token: 0x0600CBC7 RID: 52167 RVA: 0x004A4F52 File Offset: 0x004A3152
		public bool HasExceededDailyAllotted
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return this.DailyPlayTimeUsed >= this.DailyTimeAllotted;
			}
		}

		// Token: 0x170018D5 RID: 6357
		// (get) Token: 0x0600CBC8 RID: 52168 RVA: 0x004A4F65 File Offset: 0x004A3165
		public float RealTimeInMinutes
		{
			get
			{
				return (float)(AudioSettings.dspTime / 60.0);
			}
		}

		// Token: 0x170018D6 RID: 6358
		// (get) Token: 0x0600CBC9 RID: 52169 RVA: 0x004A4F77 File Offset: 0x004A3177
		public bool IsInCoolDown
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return this.RealTimeInMinutes < this.NextScheduleChance;
			}
		}

		// Token: 0x170018D7 RID: 6359
		// (get) Token: 0x0600CBCA RID: 52170 RVA: 0x004A4F87 File Offset: 0x004A3187
		public float CoolDownTime
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return DynamicMusicManager.Random.RandomRange(GamePrefs.GetFloat(EnumGamePrefs.OptionsPlayChanceFrequency) - 1f, GamePrefs.GetFloat(EnumGamePrefs.OptionsPlayChanceFrequency) + 1f);
			}
		}

		// Token: 0x170018D8 RID: 6360
		// (get) Token: 0x0600CBCB RID: 52171 RVA: 0x004A4FB3 File Offset: 0x004A31B3
		// (set) Token: 0x0600CBCC RID: 52172 RVA: 0x004A4FBB File Offset: 0x004A31BB
		public bool CanScheduleTrack
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600CBCD RID: 52173 RVA: 0x004A4FC4 File Offset: 0x004A31C4
		public static void Init(DynamicMusicManager _dynamicMusicManager)
		{
			_dynamicMusicManager.FrequencyManager = new FrequencyManager();
			_dynamicMusicManager.FrequencyManager.dynamicMusicManager = _dynamicMusicManager;
			_dynamicMusicManager.FrequencyManager.CanScheduleTrack = false;
			FrequencyManager.PlayChance = GamePrefs.GetFloat(EnumGamePrefs.OptionsPlayChanceProbability);
		}

		// Token: 0x0600CBCE RID: 52174 RVA: 0x004A4FF8 File Offset: 0x004A31F8
		public void Tick()
		{
			this.CanScheduleTrack = !this.IsMusicPlayingThisTick && !this.IsInCoolDown && this.RollIsSuccessful;
			if (this.IsMusicPlayingThisTick)
			{
				this.RollIsSuccessful = false;
			}
			if (this.DidDayChange)
			{
				this.currentDay = GameUtils.WorldTimeToDays(GameManager.Instance.World.worldTime);
				this.DailyPlayTimeUsed = 0f;
				if (this.IsMusicPlayingThisTick)
				{
					this.musicStartTime = this.RealTimeInMinutes;
				}
				else if (!this.CanScheduleTrack)
				{
					this.StartCoolDown();
				}
			}
			if (this.dynamicMusicManager.MusicStarted)
			{
				this.OnMusicStarted();
				return;
			}
			if (this.dynamicMusicManager.MusicStopped)
			{
				this.OnMusicStopped();
				return;
			}
			if (this.HasExceededDailyAllotted || this.IsInCoolDown || this.IsMusicPlayingThisTick || this.dynamicMusicManager.IsAfterDusk || !this.dynamicMusicManager.IsAfterDawn)
			{
				return;
			}
			if (!(this.RollIsSuccessful = DynamicMusicManager.Random.RandomRange(1f) < FrequencyManager.PlayChance))
			{
				this.StartCoolDown();
			}
		}

		// Token: 0x0600CBCF RID: 52175 RVA: 0x004A5106 File Offset: 0x004A3306
		public void OnPlayerFirstSpawned()
		{
			this.StartCoolDown();
		}

		// Token: 0x0600CBD0 RID: 52176 RVA: 0x004A510E File Offset: 0x004A330E
		public void StartCoolDown()
		{
			this.CanScheduleTrack = false;
			this.NextScheduleChance = this.RealTimeInMinutes + this.CoolDownTime;
		}

		// Token: 0x0600CBD1 RID: 52177 RVA: 0x004A512A File Offset: 0x004A332A
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnMusicStarted()
		{
			this.musicStartTime = this.RealTimeInMinutes;
		}

		// Token: 0x0600CBD2 RID: 52178 RVA: 0x004A5138 File Offset: 0x004A3338
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnMusicStopped()
		{
			this.DailyPlayTimeUsed += this.RealTimeInMinutes - this.musicStartTime - this.PauseTime;
			this.StartCoolDown();
			this.PauseTime = 0f;
		}

		// Token: 0x0600CBD3 RID: 52179 RVA: 0x004A516C File Offset: 0x004A336C
		public void OnPause()
		{
			this.pauseStart = this.RealTimeInMinutes;
		}

		// Token: 0x0600CBD4 RID: 52180 RVA: 0x004A517C File Offset: 0x004A337C
		public void OnUnPause()
		{
			this.pauseEnd = this.RealTimeInMinutes;
			this.PauseTime += this.pauseEnd - this.pauseStart;
			this.pauseEnd = (this.pauseStart = 0f);
		}

		// Token: 0x04009A68 RID: 39528
		[PublicizedFrom(EAccessModifier.Private)]
		public DynamicMusicManager dynamicMusicManager;

		// Token: 0x04009A69 RID: 39529
		[PublicizedFrom(EAccessModifier.Private)]
		public static float PlayChance;

		// Token: 0x04009A6A RID: 39530
		[PublicizedFrom(EAccessModifier.Private)]
		public int currentDay;

		// Token: 0x04009A6B RID: 39531
		[PublicizedFrom(EAccessModifier.Private)]
		public float musicStartTime;

		// Token: 0x04009A6C RID: 39532
		public float DailyPlayTimeUsed;

		// Token: 0x04009A6D RID: 39533
		public float NextScheduleChance;

		// Token: 0x04009A6E RID: 39534
		public float PauseTime;

		// Token: 0x04009A6F RID: 39535
		[PublicizedFrom(EAccessModifier.Private)]
		public float pauseStart;

		// Token: 0x04009A70 RID: 39536
		[PublicizedFrom(EAccessModifier.Private)]
		public float pauseEnd;

		// Token: 0x04009A71 RID: 39537
		[PublicizedFrom(EAccessModifier.Private)]
		public bool RollIsSuccessful;
	}
}
