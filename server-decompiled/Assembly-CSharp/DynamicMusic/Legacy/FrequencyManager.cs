using System;
using UnityEngine;

namespace DynamicMusic.Legacy
{
	// Token: 0x02001AC0 RID: 6848
	public class FrequencyManager
	{
		// Token: 0x170019E0 RID: 6624
		// (get) Token: 0x0600CEA0 RID: 52896 RVA: 0x004AF790 File Offset: 0x004AD990
		public float DailyTimePercentage
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return GamePrefs.GetFloat(EnumGamePrefs.OptionsDynamicMusicDailyTime);
			}
		}

		// Token: 0x170019E1 RID: 6625
		// (get) Token: 0x0600CEA1 RID: 52897 RVA: 0x004AF79C File Offset: 0x004AD99C
		public int MinutesPerDay
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return GamePrefs.GetInt(EnumGamePrefs.DayNightLength);
			}
		}

		// Token: 0x170019E2 RID: 6626
		// (get) Token: 0x0600CEA2 RID: 52898 RVA: 0x004AF7A5 File Offset: 0x004AD9A5
		public bool IsMusicPlayingThisTick
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return this.dynamicMusicManager.IsMusicPlayingThisTick;
			}
		}

		// Token: 0x170019E3 RID: 6627
		// (get) Token: 0x0600CEA3 RID: 52899 RVA: 0x004AF7B2 File Offset: 0x004AD9B2
		public bool MusicStarted
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return this.dynamicMusicManager.MusicStarted;
			}
		}

		// Token: 0x170019E4 RID: 6628
		// (get) Token: 0x0600CEA4 RID: 52900 RVA: 0x004AF7BF File Offset: 0x004AD9BF
		public bool MusicStopped
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return this.dynamicMusicManager.MusicStopped;
			}
		}

		// Token: 0x170019E5 RID: 6629
		// (get) Token: 0x0600CEA5 RID: 52901 RVA: 0x004AF7CC File Offset: 0x004AD9CC
		public bool IsMusicScheduled
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return this.CanScheduleTrack && !this.IsInCoolDown;
			}
		}

		// Token: 0x170019E6 RID: 6630
		// (get) Token: 0x0600CEA6 RID: 52902 RVA: 0x004AF7E1 File Offset: 0x004AD9E1
		public bool DidDayChange
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return this.currentDay != GameUtils.WorldTimeToDays(GameManager.Instance.World.worldTime);
			}
		}

		// Token: 0x170019E7 RID: 6631
		// (get) Token: 0x0600CEA7 RID: 52903 RVA: 0x004AF802 File Offset: 0x004ADA02
		public float DailyTimeAllotted
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return this.DailyTimePercentage * (float)this.MinutesPerDay;
			}
		}

		// Token: 0x170019E8 RID: 6632
		// (get) Token: 0x0600CEA8 RID: 52904 RVA: 0x004AF812 File Offset: 0x004ADA12
		public bool HasExceededDailyAllotted
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return this.DailyPlayTimeUsed >= this.DailyTimeAllotted;
			}
		}

		// Token: 0x170019E9 RID: 6633
		// (get) Token: 0x0600CEA9 RID: 52905 RVA: 0x004AF825 File Offset: 0x004ADA25
		public float RealTimeInMinutes
		{
			get
			{
				return (float)(AudioSettings.dspTime / 60.0);
			}
		}

		// Token: 0x170019EA RID: 6634
		// (get) Token: 0x0600CEAA RID: 52906 RVA: 0x004AF837 File Offset: 0x004ADA37
		public bool IsInCoolDown
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return this.RealTimeInMinutes < this.NextScheduleChance;
			}
		}

		// Token: 0x170019EB RID: 6635
		// (get) Token: 0x0600CEAB RID: 52907 RVA: 0x004AF847 File Offset: 0x004ADA47
		public float CoolDownTime
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return DynamicMusicManager.Random.RandomRange(GamePrefs.GetFloat(EnumGamePrefs.OptionsPlayChanceFrequency) - 1f, GamePrefs.GetFloat(EnumGamePrefs.OptionsPlayChanceFrequency) + 1f);
			}
		}

		// Token: 0x170019EC RID: 6636
		// (get) Token: 0x0600CEAC RID: 52908 RVA: 0x004AF873 File Offset: 0x004ADA73
		// (set) Token: 0x0600CEAD RID: 52909 RVA: 0x004AF87B File Offset: 0x004ADA7B
		public bool CanScheduleTrack
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600CEAE RID: 52910 RVA: 0x004AF884 File Offset: 0x004ADA84
		public static void Init(DynamicMusicManager _dynamicMusicManager)
		{
			_dynamicMusicManager.FrequencyManager = new FrequencyManager();
			_dynamicMusicManager.FrequencyManager.dynamicMusicManager = _dynamicMusicManager;
			_dynamicMusicManager.FrequencyManager.CanScheduleTrack = false;
			FrequencyManager.PlayChance = GamePrefs.GetFloat(EnumGamePrefs.OptionsPlayChanceProbability);
		}

		// Token: 0x0600CEAF RID: 52911 RVA: 0x004AF8B8 File Offset: 0x004ADAB8
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

		// Token: 0x0600CEB0 RID: 52912 RVA: 0x004AF9C6 File Offset: 0x004ADBC6
		public void OnPlayerFirstSpawned()
		{
			this.StartCoolDown();
		}

		// Token: 0x0600CEB1 RID: 52913 RVA: 0x004AF9CE File Offset: 0x004ADBCE
		public void StartCoolDown()
		{
			this.CanScheduleTrack = false;
			this.NextScheduleChance = this.RealTimeInMinutes + this.CoolDownTime;
		}

		// Token: 0x0600CEB2 RID: 52914 RVA: 0x004AF9EA File Offset: 0x004ADBEA
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnMusicStarted()
		{
			this.musicStartTime = this.RealTimeInMinutes;
		}

		// Token: 0x0600CEB3 RID: 52915 RVA: 0x004AF9F8 File Offset: 0x004ADBF8
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnMusicStopped()
		{
			this.DailyPlayTimeUsed += this.RealTimeInMinutes - this.musicStartTime - this.PauseTime;
			this.StartCoolDown();
			this.PauseTime = 0f;
		}

		// Token: 0x0600CEB4 RID: 52916 RVA: 0x004AFA2C File Offset: 0x004ADC2C
		public void OnPause()
		{
			this.pauseStart = this.RealTimeInMinutes;
		}

		// Token: 0x0600CEB5 RID: 52917 RVA: 0x004AFA3C File Offset: 0x004ADC3C
		public void OnUnPause()
		{
			this.pauseEnd = this.RealTimeInMinutes;
			this.PauseTime += this.pauseEnd - this.pauseStart;
			this.pauseEnd = (this.pauseStart = 0f);
		}

		// Token: 0x04009C5F RID: 40031
		[PublicizedFrom(EAccessModifier.Private)]
		public DynamicMusicManager dynamicMusicManager;

		// Token: 0x04009C60 RID: 40032
		[PublicizedFrom(EAccessModifier.Private)]
		public static float PlayChance;

		// Token: 0x04009C61 RID: 40033
		[PublicizedFrom(EAccessModifier.Private)]
		public int currentDay;

		// Token: 0x04009C62 RID: 40034
		[PublicizedFrom(EAccessModifier.Private)]
		public float musicStartTime;

		// Token: 0x04009C63 RID: 40035
		public float DailyPlayTimeUsed;

		// Token: 0x04009C64 RID: 40036
		public float NextScheduleChance;

		// Token: 0x04009C65 RID: 40037
		public float PauseTime;

		// Token: 0x04009C66 RID: 40038
		[PublicizedFrom(EAccessModifier.Private)]
		public float pauseStart;

		// Token: 0x04009C67 RID: 40039
		[PublicizedFrom(EAccessModifier.Private)]
		public float pauseEnd;

		// Token: 0x04009C68 RID: 40040
		[PublicizedFrom(EAccessModifier.Private)]
		public bool RollIsSuccessful;
	}
}
