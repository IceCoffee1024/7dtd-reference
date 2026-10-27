using System;
using DynamicMusic.Legacy;
using DynamicMusic.Legacy.ObjectModel;

// Token: 0x020003C8 RID: 968
public class DynamicMusicManager : IGamePrefsChangedListener
{
	// Token: 0x17000365 RID: 869
	// (get) Token: 0x06001CDC RID: 7388 RVA: 0x000AEA33 File Offset: 0x000ACC33
	public bool MusicStarted
	{
		get
		{
			return this.IsMusicPlayingThisTick && !this.WasMusicPlayingLastTick;
		}
	}

	// Token: 0x17000366 RID: 870
	// (get) Token: 0x06001CDD RID: 7389 RVA: 0x000AEA48 File Offset: 0x000ACC48
	public bool MusicStopped
	{
		get
		{
			return !this.IsMusicPlayingThisTick && this.WasMusicPlayingLastTick;
		}
	}

	// Token: 0x17000367 RID: 871
	// (get) Token: 0x06001CDE RID: 7390 RVA: 0x000AEA5A File Offset: 0x000ACC5A
	public bool IsDynamicMusicPlaying
	{
		get
		{
			return this.IsMusicPlayingThisTick;
		}
	}

	// Token: 0x17000368 RID: 872
	// (get) Token: 0x06001CDF RID: 7391 RVA: 0x000AEA62 File Offset: 0x000ACC62
	public bool IsBeforeDuskPlayBan
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return SkyManager.GetTimeOfDayAsMinutes() < SkyManager.GetDuskTimeAsMinutes() - DynamicMusicManager.PlayBanThreshold;
		}
	}

	// Token: 0x17000369 RID: 873
	// (get) Token: 0x06001CE0 RID: 7392 RVA: 0x000AEA76 File Offset: 0x000ACC76
	public bool IsAfterDusk
	{
		get
		{
			return SkyManager.TimeOfDay() > SkyManager.GetDuskTime();
		}
	}

	// Token: 0x1700036A RID: 874
	// (get) Token: 0x06001CE1 RID: 7393 RVA: 0x000AEA84 File Offset: 0x000ACC84
	public bool IsAfterDuskWindow
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return SkyManager.GetTimeOfDayAsMinutes() > SkyManager.GetDuskTimeAsMinutes() + DynamicMusicManager.deadWindow;
		}
	}

	// Token: 0x1700036B RID: 875
	// (get) Token: 0x06001CE2 RID: 7394 RVA: 0x000AEA98 File Offset: 0x000ACC98
	public bool IsBeforeDawnPlayBan
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return SkyManager.GetTimeOfDayAsMinutes() < SkyManager.GetDawnTimeAsMinutes() - DynamicMusicManager.PlayBanThreshold;
		}
	}

	// Token: 0x1700036C RID: 876
	// (get) Token: 0x06001CE3 RID: 7395 RVA: 0x000AEAAC File Offset: 0x000ACCAC
	public bool IsAfterDawn
	{
		get
		{
			return SkyManager.TimeOfDay() > SkyManager.GetDawnTime();
		}
	}

	// Token: 0x1700036D RID: 877
	// (get) Token: 0x06001CE4 RID: 7396 RVA: 0x000AEABA File Offset: 0x000ACCBA
	public bool IsAfterDawnWindow
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return SkyManager.GetTimeOfDayAsMinutes() > SkyManager.GetDawnTimeAsMinutes() + DynamicMusicManager.deadWindow;
		}
	}

	// Token: 0x1700036E RID: 878
	// (get) Token: 0x06001CE5 RID: 7397 RVA: 0x000AEACE File Offset: 0x000ACCCE
	// (set) Token: 0x06001CE6 RID: 7398 RVA: 0x000AEAD6 File Offset: 0x000ACCD6
	public bool IsInDeadWindow
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x1700036F RID: 879
	// (get) Token: 0x06001CE7 RID: 7399 RVA: 0x000AEADF File Offset: 0x000ACCDF
	// (set) Token: 0x06001CE8 RID: 7400 RVA: 0x000AEAE7 File Offset: 0x000ACCE7
	public bool IsPlayAllowed
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000370 RID: 880
	// (get) Token: 0x06001CE9 RID: 7401 RVA: 0x000AEAF0 File Offset: 0x000ACCF0
	// (set) Token: 0x06001CEA RID: 7402 RVA: 0x000AEAF8 File Offset: 0x000ACCF8
	public float DistanceFromDeadWindow
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000371 RID: 881
	// (get) Token: 0x06001CEB RID: 7403 RVA: 0x000AEB01 File Offset: 0x000ACD01
	// (set) Token: 0x06001CEC RID: 7404 RVA: 0x000AEB09 File Offset: 0x000ACD09
	public bool IsPlayerInTraderStation
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06001CED RID: 7405 RVA: 0x000AEB12 File Offset: 0x000ACD12
	[PublicizedFrom(EAccessModifier.Private)]
	public DynamicMusicManager()
	{
		this.UpdateConditions = default(DMSUpdateConditions);
		this.UpdateConditions.IsDMSEnabled = GamePrefs.GetBool(EnumGamePrefs.OptionsDynamicMusicEnabled);
		this.UpdateConditions.IsGameUnPaused = true;
	}

	// Token: 0x06001CEE RID: 7406 RVA: 0x000AEB48 File Offset: 0x000ACD48
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

	// Token: 0x06001CEF RID: 7407 RVA: 0x000AEBE4 File Offset: 0x000ACDE4
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

	// Token: 0x06001CF0 RID: 7408 RVA: 0x000AED56 File Offset: 0x000ACF56
	public void CleanUpDynamicMembers()
	{
		if (this.StreamerMaster != null)
		{
			this.StreamerMaster.Cleanup();
		}
		this.UpdateConditions.IsDMSInitialized = false;
	}

	// Token: 0x06001CF1 RID: 7409 RVA: 0x000AED77 File Offset: 0x000ACF77
	public static void Cleanup()
	{
		MusicGroup.Cleanup();
		ConfigSet.Cleanup();
	}

	// Token: 0x06001CF2 RID: 7410 RVA: 0x000AED84 File Offset: 0x000ACF84
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

	// Token: 0x06001CF3 RID: 7411 RVA: 0x000AEE62 File Offset: 0x000AD062
	public void OnPlayerDeath()
	{
		this.StreamerMaster.Stop();
	}

	// Token: 0x06001CF4 RID: 7412 RVA: 0x000AEE6F File Offset: 0x000AD06F
	public void OnPlayerFirstSpawned()
	{
		this.FrequencyManager.OnPlayerFirstSpawned();
	}

	// Token: 0x06001CF5 RID: 7413 RVA: 0x000AEE7C File Offset: 0x000AD07C
	public void Pause()
	{
		this.UpdateConditions.IsGameUnPaused = false;
		this.StreamerMaster.Pause();
		this.FrequencyManager.OnPause();
	}

	// Token: 0x06001CF6 RID: 7414 RVA: 0x000AEEA0 File Offset: 0x000AD0A0
	public void UnPause()
	{
		this.UpdateConditions.IsGameUnPaused = true;
		this.StreamerMaster.UnPause();
		this.FrequencyManager.OnUnPause();
	}

	// Token: 0x06001CF7 RID: 7415 RVA: 0x000AEEC4 File Offset: 0x000AD0C4
	[PublicizedFrom(EAccessModifier.Private)]
	public bool IsPrimaryPlayerInTraderStation()
	{
		return GameManager.Instance.World.IsWithinTraderArea(this.PrimaryLocalPlayer.GetBlockPosition());
	}

	// Token: 0x06001CF8 RID: 7416 RVA: 0x000AEEE0 File Offset: 0x000AD0E0
	public bool IsInDawnOrDuskRange(float _dawnOrDuskTime, float _currentTime)
	{
		return this.DistanceFromDawnOrDusk(_dawnOrDuskTime, _currentTime) <= DynamicMusicManager.deadWindow;
	}

	// Token: 0x06001CF9 RID: 7417 RVA: 0x000AEEF4 File Offset: 0x000AD0F4
	public float DistanceFromDawnOrDusk(float _dawnOrDuskTime, float _currentTime)
	{
		return Math.Abs(_dawnOrDuskTime - _currentTime);
	}

	// Token: 0x06001CFA RID: 7418 RVA: 0x000AEF00 File Offset: 0x000AD100
	[PublicizedFrom(EAccessModifier.Private)]
	public void IGamePrefsChangedListener.OnGamePrefChanged(EnumGamePrefs _enum)
	{
		if (_enum == EnumGamePrefs.OptionsDynamicMusicEnabled && !(this.UpdateConditions.IsDMSEnabled = GamePrefs.GetBool(EnumGamePrefs.OptionsDynamicMusicEnabled)))
		{
			this.StreamerMaster.Stop();
		}
	}

	// Token: 0x17000372 RID: 882
	// (get) Token: 0x06001CFB RID: 7419 RVA: 0x000AEF3A File Offset: 0x000AD13A
	public float TimeToNextDayEvent
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return (SkyManager.IsDark() ? SkyManager.GetDawnTimeAsMinutes() : SkyManager.GetDuskTimeAsMinutes()) - SkyManager.GetTimeOfDayAsMinutes();
		}
	}

	// Token: 0x040012E5 RID: 4837
	public EntityPlayerLocal PrimaryLocalPlayer;

	// Token: 0x040012E6 RID: 4838
	public ThreatLevelTracker ThreatLevelTracker;

	// Token: 0x040012E7 RID: 4839
	public FrequencyManager FrequencyManager;

	// Token: 0x040012E8 RID: 4840
	public TransitionManager TransitionManager;

	// Token: 0x040012E9 RID: 4841
	public StreamerMaster StreamerMaster;

	// Token: 0x040012EA RID: 4842
	public bool IsMusicPlayingThisTick;

	// Token: 0x040012EB RID: 4843
	public bool WasMusicPlayingLastTick;

	// Token: 0x040012EC RID: 4844
	public static readonly float PlayBanThreshold = 1f;

	// Token: 0x040012ED RID: 4845
	public static readonly float deadWindow = 0.16666667f;

	// Token: 0x040012EE RID: 4846
	public static GameRandom Random;

	// Token: 0x040012EF RID: 4847
	public DMSUpdateConditions UpdateConditions;
}
