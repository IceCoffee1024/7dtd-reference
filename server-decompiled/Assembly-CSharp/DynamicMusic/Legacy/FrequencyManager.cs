using System;
using UnityEngine;

namespace DynamicMusic.Legacy
{
	// Token: 0x02001AF1 RID: 6897
	public class FrequencyManager
	{
		// Token: 0x17001A28 RID: 6696
		// (get) Token: 0x0600CFB4 RID: 53172 RVA: 0x004B328C File Offset: 0x004B148C
		public float DailyTimePercentage
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return GamePrefs.GetFloat(EnumGamePrefs.OptionsDynamicMusicDailyTime);
			}
		}

		// Token: 0x17001A29 RID: 6697
		// (get) Token: 0x0600CFB5 RID: 53173 RVA: 0x004B3298 File Offset: 0x004B1498
		public int MinutesPerDay
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return GamePrefs.GetInt(EnumGamePrefs.DayNightLength);
			}
		}

		// Token: 0x17001A2A RID: 6698
		// (get) Token: 0x0600CFB6 RID: 53174 RVA: 0x004B32A1 File Offset: 0x004B14A1
		public bool IsMusicPlayingThisTick
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return this.dynamicMusicManager.IsMusicPlayingThisTick;
			}
		}

		// Token: 0x17001A2B RID: 6699
		// (get) Token: 0x0600CFB7 RID: 53175 RVA: 0x004B32AE File Offset: 0x004B14AE
		public bool MusicStarted
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return this.dynamicMusicManager.MusicStarted;
			}
		}

		// Token: 0x17001A2C RID: 6700
		// (get) Token: 0x0600CFB8 RID: 53176 RVA: 0x004B32BB File Offset: 0x004B14BB
		public bool MusicStopped
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return this.dynamicMusicManager.MusicStopped;
			}
		}

		// Token: 0x17001A2D RID: 6701
		// (get) Token: 0x0600CFB9 RID: 53177 RVA: 0x004B32C8 File Offset: 0x004B14C8
		public bool IsMusicScheduled
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return this.CanScheduleTrack && !this.IsInCoolDown;
			}
		}

		// Token: 0x17001A2E RID: 6702
		// (get) Token: 0x0600CFBA RID: 53178 RVA: 0x004B32DD File Offset: 0x004B14DD
		public bool DidDayChange
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return this.currentDay != GameUtils.WorldTimeToDays(GameManager.Instance.World.worldTime);
			}
		}

		// Token: 0x17001A2F RID: 6703
		// (get) Token: 0x0600CFBB RID: 53179 RVA: 0x004B32FE File Offset: 0x004B14FE
		public float DailyTimeAllotted
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return this.DailyTimePercentage * (float)this.MinutesPerDay;
			}
		}

		// Token: 0x17001A30 RID: 6704
		// (get) Token: 0x0600CFBC RID: 53180 RVA: 0x004B330E File Offset: 0x004B150E
		public bool HasExceededDailyAllotted
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return this.DailyPlayTimeUsed >= this.DailyTimeAllotted;
			}
		}

		// Token: 0x17001A31 RID: 6705
		// (get) Token: 0x0600CFBD RID: 53181 RVA: 0x004B3321 File Offset: 0x004B1521
		public float RealTimeInMinutes
		{
			get
			{
				return (float)(AudioSettings.dspTime / 60.0);
			}
		}

		// Token: 0x17001A32 RID: 6706
		// (get) Token: 0x0600CFBE RID: 53182 RVA: 0x004B3333 File Offset: 0x004B1533
		public bool IsInCoolDown
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return this.RealTimeInMinutes < this.NextScheduleChance;
			}
		}

		// Token: 0x17001A33 RID: 6707
		// (get) Token: 0x0600CFBF RID: 53183 RVA: 0x004B3343 File Offset: 0x004B1543
		public float CoolDownTime
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return DynamicMusicManager.Random.RandomRange(GamePrefs.GetFloat(EnumGamePrefs.OptionsPlayChanceFrequency) - 1f, GamePrefs.GetFloat(EnumGamePrefs.OptionsPlayChanceFrequency) + 1f);
			}
		}

		// Token: 0x17001A34 RID: 6708
		// (get) Token: 0x0600CFC0 RID: 53184 RVA: 0x004B336F File Offset: 0x004B156F
		// (set) Token: 0x0600CFC1 RID: 53185 RVA: 0x004B3377 File Offset: 0x004B1577
		public bool CanScheduleTrack
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600CFC2 RID: 53186 RVA: 0x004B3380 File Offset: 0x004B1580
		public static void Init(DynamicMusicManager _dynamicMusicManager)
		{
			_dynamicMusicManager.FrequencyManager = new FrequencyManager();
			_dynamicMusicManager.FrequencyManager.dynamicMusicManager = _dynamicMusicManager;
			_dynamicMusicManager.FrequencyManager.CanScheduleTrack = false;
			FrequencyManager.PlayChance = GamePrefs.GetFloat(EnumGamePrefs.OptionsPlayChanceProbability);
		}

		// Token: 0x0600CFC3 RID: 53187 RVA: 0x004B33B4 File Offset: 0x004B15B4
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

		// Token: 0x0600CFC4 RID: 53188 RVA: 0x004B34C2 File Offset: 0x004B16C2
		public void OnPlayerFirstSpawned()
		{
			this.StartCoolDown();
		}

		// Token: 0x0600CFC5 RID: 53189 RVA: 0x004B34CA File Offset: 0x004B16CA
		public void StartCoolDown()
		{
			this.CanScheduleTrack = false;
			this.NextScheduleChance = this.RealTimeInMinutes + this.CoolDownTime;
		}

		// Token: 0x0600CFC6 RID: 53190 RVA: 0x004B34E6 File Offset: 0x004B16E6
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnMusicStarted()
		{
			this.musicStartTime = this.RealTimeInMinutes;
		}

		// Token: 0x0600CFC7 RID: 53191 RVA: 0x004B34F4 File Offset: 0x004B16F4
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnMusicStopped()
		{
			this.DailyPlayTimeUsed += this.RealTimeInMinutes - this.musicStartTime - this.PauseTime;
			this.StartCoolDown();
			this.PauseTime = 0f;
		}

		// Token: 0x0600CFC8 RID: 53192 RVA: 0x004B3528 File Offset: 0x004B1728
		public void OnPause()
		{
			this.pauseStart = this.RealTimeInMinutes;
		}

		// Token: 0x0600CFC9 RID: 53193 RVA: 0x004B3538 File Offset: 0x004B1738
		public void OnUnPause()
		{
			this.pauseEnd = this.RealTimeInMinutes;
			this.PauseTime += this.pauseEnd - this.pauseStart;
			this.pauseEnd = (this.pauseStart = 0f);
		}

		// Token: 0x04009D48 RID: 40264
		[PublicizedFrom(EAccessModifier.Private)]
		public DynamicMusicManager dynamicMusicManager;

		// Token: 0x04009D49 RID: 40265
		[PublicizedFrom(EAccessModifier.Private)]
		public static float PlayChance;

		// Token: 0x04009D4A RID: 40266
		[PublicizedFrom(EAccessModifier.Private)]
		public int currentDay;

		// Token: 0x04009D4B RID: 40267
		[PublicizedFrom(EAccessModifier.Private)]
		public float musicStartTime;

		// Token: 0x04009D4C RID: 40268
		public float DailyPlayTimeUsed;

		// Token: 0x04009D4D RID: 40269
		public float NextScheduleChance;

		// Token: 0x04009D4E RID: 40270
		public float PauseTime;

		// Token: 0x04009D4F RID: 40271
		[PublicizedFrom(EAccessModifier.Private)]
		public float pauseStart;

		// Token: 0x04009D50 RID: 40272
		[PublicizedFrom(EAccessModifier.Private)]
		public float pauseEnd;

		// Token: 0x04009D51 RID: 40273
		[PublicizedFrom(EAccessModifier.Private)]
		public bool RollIsSuccessful;
	}
}
