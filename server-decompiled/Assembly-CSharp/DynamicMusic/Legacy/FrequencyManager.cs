using System;
using UnityEngine;

namespace DynamicMusic.Legacy
{
	// Token: 0x02001A94 RID: 6804
	public class FrequencyManager
	{
		// Token: 0x1700193E RID: 6462
		// (get) Token: 0x0600CD48 RID: 52552 RVA: 0x004AB2C4 File Offset: 0x004A94C4
		public float DailyTimePercentage
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return GamePrefs.GetFloat(EnumGamePrefs.OptionsDynamicMusicDailyTime);
			}
		}

		// Token: 0x1700193F RID: 6463
		// (get) Token: 0x0600CD49 RID: 52553 RVA: 0x004AB2D0 File Offset: 0x004A94D0
		public int MinutesPerDay
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return GamePrefs.GetInt(EnumGamePrefs.DayNightLength);
			}
		}

		// Token: 0x17001940 RID: 6464
		// (get) Token: 0x0600CD4A RID: 52554 RVA: 0x004AB2D9 File Offset: 0x004A94D9
		public bool IsMusicPlayingThisTick
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return this.dynamicMusicManager.IsMusicPlayingThisTick;
			}
		}

		// Token: 0x17001941 RID: 6465
		// (get) Token: 0x0600CD4B RID: 52555 RVA: 0x004AB2E6 File Offset: 0x004A94E6
		public bool MusicStarted
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return this.dynamicMusicManager.MusicStarted;
			}
		}

		// Token: 0x17001942 RID: 6466
		// (get) Token: 0x0600CD4C RID: 52556 RVA: 0x004AB2F3 File Offset: 0x004A94F3
		public bool MusicStopped
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return this.dynamicMusicManager.MusicStopped;
			}
		}

		// Token: 0x17001943 RID: 6467
		// (get) Token: 0x0600CD4D RID: 52557 RVA: 0x004AB300 File Offset: 0x004A9500
		public bool IsMusicScheduled
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return this.CanScheduleTrack && !this.IsInCoolDown;
			}
		}

		// Token: 0x17001944 RID: 6468
		// (get) Token: 0x0600CD4E RID: 52558 RVA: 0x004AB315 File Offset: 0x004A9515
		public bool DidDayChange
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return this.currentDay != GameUtils.WorldTimeToDays(GameManager.Instance.World.worldTime);
			}
		}

		// Token: 0x17001945 RID: 6469
		// (get) Token: 0x0600CD4F RID: 52559 RVA: 0x004AB336 File Offset: 0x004A9536
		public float DailyTimeAllotted
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return this.DailyTimePercentage * (float)this.MinutesPerDay;
			}
		}

		// Token: 0x17001946 RID: 6470
		// (get) Token: 0x0600CD50 RID: 52560 RVA: 0x004AB346 File Offset: 0x004A9546
		public bool HasExceededDailyAllotted
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return this.DailyPlayTimeUsed >= this.DailyTimeAllotted;
			}
		}

		// Token: 0x17001947 RID: 6471
		// (get) Token: 0x0600CD51 RID: 52561 RVA: 0x004AB359 File Offset: 0x004A9559
		public float RealTimeInMinutes
		{
			get
			{
				return (float)(AudioSettings.dspTime / 60.0);
			}
		}

		// Token: 0x17001948 RID: 6472
		// (get) Token: 0x0600CD52 RID: 52562 RVA: 0x004AB36B File Offset: 0x004A956B
		public bool IsInCoolDown
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return this.RealTimeInMinutes < this.NextScheduleChance;
			}
		}

		// Token: 0x17001949 RID: 6473
		// (get) Token: 0x0600CD53 RID: 52563 RVA: 0x004AB37B File Offset: 0x004A957B
		public float CoolDownTime
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return DynamicMusicManager.Random.RandomRange(GamePrefs.GetFloat(EnumGamePrefs.OptionsPlayChanceFrequency) - 1f, GamePrefs.GetFloat(EnumGamePrefs.OptionsPlayChanceFrequency) + 1f);
			}
		}

		// Token: 0x1700194A RID: 6474
		// (get) Token: 0x0600CD54 RID: 52564 RVA: 0x004AB3A7 File Offset: 0x004A95A7
		// (set) Token: 0x0600CD55 RID: 52565 RVA: 0x004AB3AF File Offset: 0x004A95AF
		public bool CanScheduleTrack
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600CD56 RID: 52566 RVA: 0x004AB3B8 File Offset: 0x004A95B8
		public static void Init(DynamicMusicManager _dynamicMusicManager)
		{
			_dynamicMusicManager.FrequencyManager = new FrequencyManager();
			_dynamicMusicManager.FrequencyManager.dynamicMusicManager = _dynamicMusicManager;
			_dynamicMusicManager.FrequencyManager.CanScheduleTrack = false;
			FrequencyManager.PlayChance = GamePrefs.GetFloat(EnumGamePrefs.OptionsPlayChanceProbability);
		}

		// Token: 0x0600CD57 RID: 52567 RVA: 0x004AB3EC File Offset: 0x004A95EC
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

		// Token: 0x0600CD58 RID: 52568 RVA: 0x004AB4FA File Offset: 0x004A96FA
		public void OnPlayerFirstSpawned()
		{
			this.StartCoolDown();
		}

		// Token: 0x0600CD59 RID: 52569 RVA: 0x004AB502 File Offset: 0x004A9702
		public void StartCoolDown()
		{
			this.CanScheduleTrack = false;
			this.NextScheduleChance = this.RealTimeInMinutes + this.CoolDownTime;
		}

		// Token: 0x0600CD5A RID: 52570 RVA: 0x004AB51E File Offset: 0x004A971E
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnMusicStarted()
		{
			this.musicStartTime = this.RealTimeInMinutes;
		}

		// Token: 0x0600CD5B RID: 52571 RVA: 0x004AB52C File Offset: 0x004A972C
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnMusicStopped()
		{
			this.DailyPlayTimeUsed += this.RealTimeInMinutes - this.musicStartTime - this.PauseTime;
			this.StartCoolDown();
			this.PauseTime = 0f;
		}

		// Token: 0x0600CD5C RID: 52572 RVA: 0x004AB560 File Offset: 0x004A9760
		public void OnPause()
		{
			this.pauseStart = this.RealTimeInMinutes;
		}

		// Token: 0x0600CD5D RID: 52573 RVA: 0x004AB570 File Offset: 0x004A9770
		public void OnUnPause()
		{
			this.pauseEnd = this.RealTimeInMinutes;
			this.PauseTime += this.pauseEnd - this.pauseStart;
			this.pauseEnd = (this.pauseStart = 0f);
		}

		// Token: 0x04009B99 RID: 39833
		[PublicizedFrom(EAccessModifier.Private)]
		public DynamicMusicManager dynamicMusicManager;

		// Token: 0x04009B9A RID: 39834
		[PublicizedFrom(EAccessModifier.Private)]
		public static float PlayChance;

		// Token: 0x04009B9B RID: 39835
		[PublicizedFrom(EAccessModifier.Private)]
		public int currentDay;

		// Token: 0x04009B9C RID: 39836
		[PublicizedFrom(EAccessModifier.Private)]
		public float musicStartTime;

		// Token: 0x04009B9D RID: 39837
		public float DailyPlayTimeUsed;

		// Token: 0x04009B9E RID: 39838
		public float NextScheduleChance;

		// Token: 0x04009B9F RID: 39839
		public float PauseTime;

		// Token: 0x04009BA0 RID: 39840
		[PublicizedFrom(EAccessModifier.Private)]
		public float pauseStart;

		// Token: 0x04009BA1 RID: 39841
		[PublicizedFrom(EAccessModifier.Private)]
		public float pauseEnd;

		// Token: 0x04009BA2 RID: 39842
		[PublicizedFrom(EAccessModifier.Private)]
		public bool RollIsSuccessful;
	}
}
