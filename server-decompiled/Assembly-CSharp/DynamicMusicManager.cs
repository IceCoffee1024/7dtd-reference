using System;
using DynamicMusic.Legacy;
using DynamicMusic.Legacy.ObjectModel;

// Token: 0x020003BF RID: 959
public class DynamicMusicManager : IGamePrefsChangedListener
{
	// Token: 0x17000369 RID: 873
	// (get) Token: 0x06001CC3 RID: 7363 RVA: 0x000ADED5 File Offset: 0x000AC0D5
	public bool MusicStarted
	{
		get
		{
			return this.IsMusicPlayingThisTick && !this.WasMusicPlayingLastTick;
		}
	}

	// Token: 0x1700036A RID: 874
	// (get) Token: 0x06001CC4 RID: 7364 RVA: 0x000ADEEA File Offset: 0x000AC0EA
	public bool MusicStopped
	{
		get
		{
			return !this.IsMusicPlayingThisTick && this.WasMusicPlayingLastTick;
		}
	}

	// Token: 0x1700036B RID: 875
	// (get) Token: 0x06001CC5 RID: 7365 RVA: 0x000ADEFC File Offset: 0x000AC0FC
	public bool IsDynamicMusicPlaying
	{
		get
		{
			return this.IsMusicPlayingThisTick;
		}
	}

	// Token: 0x1700036C RID: 876
	// (get) Token: 0x06001CC6 RID: 7366 RVA: 0x000ADF04 File Offset: 0x000AC104
	public bool IsBeforeDuskPlayBan
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return SkyManager.GetTimeOfDayAsMinutes() < SkyManager.GetDuskTimeAsMinutes() - DynamicMusicManager.PlayBanThreshold;
		}
	}

	// Token: 0x1700036D RID: 877
	// (get) Token: 0x06001CC7 RID: 7367 RVA: 0x000ADF18 File Offset: 0x000AC118
	public bool IsAfterDusk
	{
		get
		{
			return SkyManager.TimeOfDay() > SkyManager.GetDuskTime();
		}
	}

	// Token: 0x1700036E RID: 878
	// (get) Token: 0x06001CC8 RID: 7368 RVA: 0x000ADF26 File Offset: 0x000AC126
	public bool IsAfterDuskWindow
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return SkyManager.GetTimeOfDayAsMinutes() > SkyManager.GetDuskTimeAsMinutes() + DynamicMusicManager.deadWindow;
		}
	}

	// Token: 0x1700036F RID: 879
	// (get) Token: 0x06001CC9 RID: 7369 RVA: 0x000ADF3A File Offset: 0x000AC13A
	public bool IsBeforeDawnPlayBan
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return SkyManager.GetTimeOfDayAsMinutes() < SkyManager.GetDawnTimeAsMinutes() - DynamicMusicManager.PlayBanThreshold;
		}
	}

	// Token: 0x17000370 RID: 880
	// (get) Token: 0x06001CCA RID: 7370 RVA: 0x000ADF4E File Offset: 0x000AC14E
	public bool IsAfterDawn
	{
		get
		{
			return SkyManager.TimeOfDay() > SkyManager.GetDawnTime();
		}
	}

	// Token: 0x17000371 RID: 881
	// (get) Token: 0x06001CCB RID: 7371 RVA: 0x000ADF5C File Offset: 0x000AC15C
	public bool IsAfterDawnWindow
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return SkyManager.GetTimeOfDayAsMinutes() > SkyManager.GetDawnTimeAsMinutes() + DynamicMusicManager.deadWindow;
		}
	}

	// Token: 0x17000372 RID: 882
	// (get) Token: 0x06001CCC RID: 7372 RVA: 0x000ADF70 File Offset: 0x000AC170
	// (set) Token: 0x06001CCD RID: 7373 RVA: 0x000ADF78 File Offset: 0x000AC178
	public bool IsInDeadWindow
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000373 RID: 883
	// (get) Token: 0x06001CCE RID: 7374 RVA: 0x000ADF81 File Offset: 0x000AC181
	// (set) Token: 0x06001CCF RID: 7375 RVA: 0x000ADF89 File Offset: 0x000AC189
	public bool IsPlayAllowed
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000374 RID: 884
	// (get) Token: 0x06001CD0 RID: 7376 RVA: 0x000ADF92 File Offset: 0x000AC192
	// (set) Token: 0x06001CD1 RID: 7377 RVA: 0x000ADF9A File Offset: 0x000AC19A
	public float DistanceFromDeadWindow
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000375 RID: 885
	// (get) Token: 0x06001CD2 RID: 7378 RVA: 0x000ADFA3 File Offset: 0x000AC1A3
	// (set) Token: 0x06001CD3 RID: 7379 RVA: 0x000ADFAB File Offset: 0x000AC1AB
	public bool IsPlayerInTraderStation
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06001CD4 RID: 7380 RVA: 0x000ADFB4 File Offset: 0x000AC1B4
	[PublicizedFrom(EAccessModifier.Private)]
	public DynamicMusicManager()
	{
		this.UpdateConditions = default(DMSUpdateConditions);
		this.UpdateConditions.IsDMSEnabled = GamePrefs.GetBool(EnumGamePrefs.OptionsDynamicMusicEnabled);
		this.UpdateConditions.IsGameUnPaused = true;
	}

	// Token: 0x06001CD5 RID: 7381 RVA: 0x000ADFEC File Offset: 0x000AC1EC
	public static void Init(EntityPlayerLocal _epLocal)
	{
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Initializing Dynamic Music System");
		_epLocal.DynamicMusicManager = new DynamicMusicManager();
		GamePrefs.AddChangeListener(_epLocal.DynamicMusicManager);
		_epLocal.DynamicMusicManager.PrimaryLocalPlayer = _epLocal;
		DynamicMusicManager.Random = GameRandomManager.Instance.CreateGameRandom();
		ThreatLevelTracker.Init(_epLocal.DynamicMusicManager);
		FrequencyManager.Init(_epLocal.DynamicMusicManager);
		StreamerMaster.Init(_epLocal.DynamicMusicManager);
		TransitionManager.Init(_epLocal.DynamicMusicManager);
		_epLocal.DynamicMusicManager.UpdateConditions.IsDMSInitialized = true;
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Finished initializing Dynamic Music System");
	}

	// Token: 0x06001CD6 RID: 7382 RVA: 0x000AE088 File Offset: 0x000AC288
	public void Tick()
	{
		if (this.UpdateConditions.CanUpdate)
		{
			if (StreamerMaster.currentStreamer != null)
			{
				this.IsMusicPlayingThisTick = StreamerMaster.currentStreamer.IsPlaying;
			}
			if (this.IsAfterDusk)
			{
				this.IsInDeadWindow = !(this.IsPlayAllowed = this.IsAfterDuskWindow);
				this.DistanceFromDeadWindow = (float)GamePrefs.GetInt(EnumGamePrefs.DayNightLength) - SkyManager.GetTimeOfDayAsMinutes() + SkyManager.GetDawnTimeAsMinutes();
			}
			else if (this.IsAfterDawn)
			{
				if (this.IsAfterDawnWindow)
				{
					this.DistanceFromDeadWindow = Utils.FastMax(SkyManager.GetDuskTimeAsMinutes() - DynamicMusicManager.deadWindow - SkyManager.GetTimeOfDayAsMinutes(), 0f);
					if (this.IsBeforeDuskPlayBan)
					{
						this.IsInDeadWindow = false;
						this.IsPlayAllowed = true;
					}
					else
					{
						this.IsPlayAllowed = false;
						this.IsInDeadWindow = this.DistanceFromDeadWindow == 0f;
					}
				}
			}
			else
			{
				this.DistanceFromDeadWindow = Utils.FastMax(SkyManager.GetDawnTimeAsMinutes() - DynamicMusicManager.deadWindow - SkyManager.GetTimeOfDayAsMinutes(), 0f);
				if (this.IsBeforeDawnPlayBan)
				{
					this.IsPlayAllowed = true;
					this.IsInDeadWindow = false;
				}
				else
				{
					this.IsPlayAllowed = false;
					this.IsInDeadWindow = this.DistanceFromDeadWindow == 0f;
				}
			}
			this.IsPlayerInTraderStation = this.IsPrimaryPlayerInTraderStation();
			this.ThreatLevelTracker.Tick();
			this.FrequencyManager.Tick();
			this.TransitionManager.Tick();
			this.StreamerMaster.Tick();
			this.WasMusicPlayingLastTick = this.IsMusicPlayingThisTick;
		}
	}

	// Token: 0x06001CD7 RID: 7383 RVA: 0x000AE1FA File Offset: 0x000AC3FA
	public void CleanUpDynamicMembers()
	{
		if (this.StreamerMaster != null)
		{
			this.StreamerMaster.Cleanup();
		}
		this.UpdateConditions.IsDMSInitialized = false;
	}

	// Token: 0x06001CD8 RID: 7384 RVA: 0x000AE21B File Offset: 0x000AC41B
	public static void Cleanup()
	{
		MusicGroup.Cleanup();
		ConfigSet.Cleanup();
	}

	// Token: 0x06001CD9 RID: 7385 RVA: 0x000AE228 File Offset: 0x000AC428
	public void Event(MinEventTypes _eventType, MinEventParams _eventParms)
	{
		if (_eventType <= MinEventTypes.onSelfDied)
		{
			switch (_eventType)
			{
			case MinEventTypes.onOtherDamagedSelf:
				this.ThreatLevelTracker.Event(_eventType, _eventParms);
				return;
			case MinEventTypes.onOtherAttackedSelf:
				this.ThreatLevelTracker.Event(_eventType, _eventParms);
				return;
			case MinEventTypes.onOtherHealedSelf:
				break;
			case MinEventTypes.onSelfDamagedOther:
				this.ThreatLevelTracker.Event(_eventType, _eventParms);
				return;
			case MinEventTypes.onSelfAttackedOther:
				this.ThreatLevelTracker.Event(_eventType, _eventParms);
				return;
			default:
				if (_eventType != MinEventTypes.onSelfDied)
				{
					return;
				}
				Log.Out("DMS Died!");
				this.UpdateConditions.DoesPlayerExist = false;
				return;
			}
		}
		else
		{
			switch (_eventType)
			{
			case MinEventTypes.onSelfRespawn:
				Log.Out("DMS Respawn!");
				this.UpdateConditions.DoesPlayerExist = true;
				return;
			case MinEventTypes.onSelfLeaveGame:
				Log.Out("DMS Left Game!");
				return;
			case MinEventTypes.onSelfEnteredGame:
				Log.Out("DMS Entered Game!");
				this.UpdateConditions.DoesPlayerExist = true;
				break;
			default:
				if (_eventType != MinEventTypes.onSelfEnteredBiome)
				{
					return;
				}
				break;
			}
		}
	}

	// Token: 0x06001CDA RID: 7386 RVA: 0x000AE306 File Offset: 0x000AC506
	public void OnPlayerDeath()
	{
		this.StreamerMaster.Stop();
	}

	// Token: 0x06001CDB RID: 7387 RVA: 0x000AE313 File Offset: 0x000AC513
	public void OnPlayerFirstSpawned()
	{
		this.FrequencyManager.OnPlayerFirstSpawned();
	}

	// Token: 0x06001CDC RID: 7388 RVA: 0x000AE320 File Offset: 0x000AC520
	public void Pause()
	{
		this.UpdateConditions.IsGameUnPaused = false;
		this.StreamerMaster.Pause();
		this.FrequencyManager.OnPause();
	}

	// Token: 0x06001CDD RID: 7389 RVA: 0x000AE344 File Offset: 0x000AC544
	public void UnPause()
	{
		this.UpdateConditions.IsGameUnPaused = true;
		this.StreamerMaster.UnPause();
		this.FrequencyManager.OnUnPause();
	}

	// Token: 0x06001CDE RID: 7390 RVA: 0x000AE368 File Offset: 0x000AC568
	[PublicizedFrom(EAccessModifier.Private)]
	public bool IsPrimaryPlayerInTraderStation()
	{
		return GameManager.Instance.World.IsWithinTraderArea(this.PrimaryLocalPlayer.GetBlockPosition());
	}

	// Token: 0x06001CDF RID: 7391 RVA: 0x000AE384 File Offset: 0x000AC584
	public bool IsInDawnOrDuskRange(float _dawnOrDuskTime, float _currentTime)
	{
		return this.DistanceFromDawnOrDusk(_dawnOrDuskTime, _currentTime) <= DynamicMusicManager.deadWindow;
	}

	// Token: 0x06001CE0 RID: 7392 RVA: 0x000AE398 File Offset: 0x000AC598
	public float DistanceFromDawnOrDusk(float _dawnOrDuskTime, float _currentTime)
	{
		return Math.Abs(_dawnOrDuskTime - _currentTime);
	}

	// Token: 0x06001CE1 RID: 7393 RVA: 0x000AE3A4 File Offset: 0x000AC5A4
	[PublicizedFrom(EAccessModifier.Private)]
	public void IGamePrefsChangedListener.OnGamePrefChanged(EnumGamePrefs _enum)
	{
		if (_enum == EnumGamePrefs.OptionsDynamicMusicEnabled && !(this.UpdateConditions.IsDMSEnabled = GamePrefs.GetBool(EnumGamePrefs.OptionsDynamicMusicEnabled)))
		{
			this.StreamerMaster.Stop();
		}
	}

	// Token: 0x17000376 RID: 886
	// (get) Token: 0x06001CE2 RID: 7394 RVA: 0x000AE3DE File Offset: 0x000AC5DE
	public float TimeToNextDayEvent
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return (SkyManager.IsDark() ? SkyManager.GetDawnTimeAsMinutes() : SkyManager.GetDuskTimeAsMinutes()) - SkyManager.GetTimeOfDayAsMinutes();
		}
	}

	// Token: 0x040012C7 RID: 4807
	public EntityPlayerLocal PrimaryLocalPlayer;

	// Token: 0x040012C8 RID: 4808
	public ThreatLevelTracker ThreatLevelTracker;

	// Token: 0x040012C9 RID: 4809
	public FrequencyManager FrequencyManager;

	// Token: 0x040012CA RID: 4810
	public TransitionManager TransitionManager;

	// Token: 0x040012CB RID: 4811
	public StreamerMaster StreamerMaster;

	// Token: 0x040012CC RID: 4812
	public bool IsMusicPlayingThisTick;

	// Token: 0x040012CD RID: 4813
	public bool WasMusicPlayingLastTick;

	// Token: 0x040012CE RID: 4814
	public static readonly float PlayBanThreshold = 1f;

	// Token: 0x040012CF RID: 4815
	public static readonly float deadWindow = 0.16666667f;

	// Token: 0x040012D0 RID: 4816
	public static GameRandom Random;

	// Token: 0x040012D1 RID: 4817
	public DMSUpdateConditions UpdateConditions;
}
