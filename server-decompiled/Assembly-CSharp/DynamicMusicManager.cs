using System;
using DynamicMusic.Legacy;
using DynamicMusic.Legacy.ObjectModel;

// Token: 0x020003C3 RID: 963
public class DynamicMusicManager : IGamePrefsChangedListener
{
	// Token: 0x1700036D RID: 877
	// (get) Token: 0x06001CE0 RID: 7392 RVA: 0x000AE615 File Offset: 0x000AC815
	public bool MusicStarted
	{
		get
		{
			return this.IsMusicPlayingThisTick && !this.WasMusicPlayingLastTick;
		}
	}

	// Token: 0x1700036E RID: 878
	// (get) Token: 0x06001CE1 RID: 7393 RVA: 0x000AE62A File Offset: 0x000AC82A
	public bool MusicStopped
	{
		get
		{
			return !this.IsMusicPlayingThisTick && this.WasMusicPlayingLastTick;
		}
	}

	// Token: 0x1700036F RID: 879
	// (get) Token: 0x06001CE2 RID: 7394 RVA: 0x000AE63C File Offset: 0x000AC83C
	public bool IsDynamicMusicPlaying
	{
		get
		{
			return this.IsMusicPlayingThisTick;
		}
	}

	// Token: 0x17000370 RID: 880
	// (get) Token: 0x06001CE3 RID: 7395 RVA: 0x000AE644 File Offset: 0x000AC844
	public bool IsBeforeDuskPlayBan
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return SkyManager.GetTimeOfDayAsMinutes() < SkyManager.GetDuskTimeAsMinutes() - DynamicMusicManager.PlayBanThreshold;
		}
	}

	// Token: 0x17000371 RID: 881
	// (get) Token: 0x06001CE4 RID: 7396 RVA: 0x000AE658 File Offset: 0x000AC858
	public bool IsAfterDusk
	{
		get
		{
			return SkyManager.TimeOfDay() > SkyManager.GetDuskTime();
		}
	}

	// Token: 0x17000372 RID: 882
	// (get) Token: 0x06001CE5 RID: 7397 RVA: 0x000AE666 File Offset: 0x000AC866
	public bool IsAfterDuskWindow
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return SkyManager.GetTimeOfDayAsMinutes() > SkyManager.GetDuskTimeAsMinutes() + DynamicMusicManager.deadWindow;
		}
	}

	// Token: 0x17000373 RID: 883
	// (get) Token: 0x06001CE6 RID: 7398 RVA: 0x000AE67A File Offset: 0x000AC87A
	public bool IsBeforeDawnPlayBan
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return SkyManager.GetTimeOfDayAsMinutes() < SkyManager.GetDawnTimeAsMinutes() - DynamicMusicManager.PlayBanThreshold;
		}
	}

	// Token: 0x17000374 RID: 884
	// (get) Token: 0x06001CE7 RID: 7399 RVA: 0x000AE68E File Offset: 0x000AC88E
	public bool IsAfterDawn
	{
		get
		{
			return SkyManager.TimeOfDay() > SkyManager.GetDawnTime();
		}
	}

	// Token: 0x17000375 RID: 885
	// (get) Token: 0x06001CE8 RID: 7400 RVA: 0x000AE69C File Offset: 0x000AC89C
	public bool IsAfterDawnWindow
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return SkyManager.GetTimeOfDayAsMinutes() > SkyManager.GetDawnTimeAsMinutes() + DynamicMusicManager.deadWindow;
		}
	}

	// Token: 0x17000376 RID: 886
	// (get) Token: 0x06001CE9 RID: 7401 RVA: 0x000AE6B0 File Offset: 0x000AC8B0
	// (set) Token: 0x06001CEA RID: 7402 RVA: 0x000AE6B8 File Offset: 0x000AC8B8
	public bool IsInDeadWindow
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000377 RID: 887
	// (get) Token: 0x06001CEB RID: 7403 RVA: 0x000AE6C1 File Offset: 0x000AC8C1
	// (set) Token: 0x06001CEC RID: 7404 RVA: 0x000AE6C9 File Offset: 0x000AC8C9
	public bool IsPlayAllowed
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000378 RID: 888
	// (get) Token: 0x06001CED RID: 7405 RVA: 0x000AE6D2 File Offset: 0x000AC8D2
	// (set) Token: 0x06001CEE RID: 7406 RVA: 0x000AE6DA File Offset: 0x000AC8DA
	public float DistanceFromDeadWindow
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000379 RID: 889
	// (get) Token: 0x06001CEF RID: 7407 RVA: 0x000AE6E3 File Offset: 0x000AC8E3
	// (set) Token: 0x06001CF0 RID: 7408 RVA: 0x000AE6EB File Offset: 0x000AC8EB
	public bool IsPlayerInTraderStation
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06001CF1 RID: 7409 RVA: 0x000AE6F4 File Offset: 0x000AC8F4
	[PublicizedFrom(EAccessModifier.Private)]
	public DynamicMusicManager()
	{
		this.UpdateConditions = default(DMSUpdateConditions);
		this.UpdateConditions.IsDMSEnabled = GamePrefs.GetBool(EnumGamePrefs.OptionsDynamicMusicEnabled);
		this.UpdateConditions.IsGameUnPaused = true;
	}

	// Token: 0x06001CF2 RID: 7410 RVA: 0x000AE72C File Offset: 0x000AC92C
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

	// Token: 0x06001CF3 RID: 7411 RVA: 0x000AE7C8 File Offset: 0x000AC9C8
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

	// Token: 0x06001CF4 RID: 7412 RVA: 0x000AE93A File Offset: 0x000ACB3A
	public void CleanUpDynamicMembers()
	{
		if (this.StreamerMaster != null)
		{
			this.StreamerMaster.Cleanup();
		}
		this.UpdateConditions.IsDMSInitialized = false;
	}

	// Token: 0x06001CF5 RID: 7413 RVA: 0x000AE95B File Offset: 0x000ACB5B
	public static void Cleanup()
	{
		MusicGroup.Cleanup();
		ConfigSet.Cleanup();
	}

	// Token: 0x06001CF6 RID: 7414 RVA: 0x000AE968 File Offset: 0x000ACB68
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

	// Token: 0x06001CF7 RID: 7415 RVA: 0x000AEA46 File Offset: 0x000ACC46
	public void OnPlayerDeath()
	{
		this.StreamerMaster.Stop();
	}

	// Token: 0x06001CF8 RID: 7416 RVA: 0x000AEA53 File Offset: 0x000ACC53
	public void OnPlayerFirstSpawned()
	{
		this.FrequencyManager.OnPlayerFirstSpawned();
	}

	// Token: 0x06001CF9 RID: 7417 RVA: 0x000AEA60 File Offset: 0x000ACC60
	public void Pause()
	{
		this.UpdateConditions.IsGameUnPaused = false;
		this.StreamerMaster.Pause();
		this.FrequencyManager.OnPause();
	}

	// Token: 0x06001CFA RID: 7418 RVA: 0x000AEA84 File Offset: 0x000ACC84
	public void UnPause()
	{
		this.UpdateConditions.IsGameUnPaused = true;
		this.StreamerMaster.UnPause();
		this.FrequencyManager.OnUnPause();
	}

	// Token: 0x06001CFB RID: 7419 RVA: 0x000AEAA8 File Offset: 0x000ACCA8
	[PublicizedFrom(EAccessModifier.Private)]
	public bool IsPrimaryPlayerInTraderStation()
	{
		return GameManager.Instance.World.IsWithinTraderArea(this.PrimaryLocalPlayer.GetBlockPosition());
	}

	// Token: 0x06001CFC RID: 7420 RVA: 0x000AEAC4 File Offset: 0x000ACCC4
	public bool IsInDawnOrDuskRange(float _dawnOrDuskTime, float _currentTime)
	{
		return this.DistanceFromDawnOrDusk(_dawnOrDuskTime, _currentTime) <= DynamicMusicManager.deadWindow;
	}

	// Token: 0x06001CFD RID: 7421 RVA: 0x000AEAD8 File Offset: 0x000ACCD8
	public float DistanceFromDawnOrDusk(float _dawnOrDuskTime, float _currentTime)
	{
		return Math.Abs(_dawnOrDuskTime - _currentTime);
	}

	// Token: 0x06001CFE RID: 7422 RVA: 0x000AEAE4 File Offset: 0x000ACCE4
	[PublicizedFrom(EAccessModifier.Private)]
	public void IGamePrefsChangedListener.OnGamePrefChanged(EnumGamePrefs _enum)
	{
		if (_enum == EnumGamePrefs.OptionsDynamicMusicEnabled && !(this.UpdateConditions.IsDMSEnabled = GamePrefs.GetBool(EnumGamePrefs.OptionsDynamicMusicEnabled)))
		{
			this.StreamerMaster.Stop();
		}
	}

	// Token: 0x1700037A RID: 890
	// (get) Token: 0x06001CFF RID: 7423 RVA: 0x000AEB1E File Offset: 0x000ACD1E
	public float TimeToNextDayEvent
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return (SkyManager.IsDark() ? SkyManager.GetDawnTimeAsMinutes() : SkyManager.GetDuskTimeAsMinutes()) - SkyManager.GetTimeOfDayAsMinutes();
		}
	}

	// Token: 0x040012DC RID: 4828
	public EntityPlayerLocal PrimaryLocalPlayer;

	// Token: 0x040012DD RID: 4829
	public ThreatLevelTracker ThreatLevelTracker;

	// Token: 0x040012DE RID: 4830
	public FrequencyManager FrequencyManager;

	// Token: 0x040012DF RID: 4831
	public TransitionManager TransitionManager;

	// Token: 0x040012E0 RID: 4832
	public StreamerMaster StreamerMaster;

	// Token: 0x040012E1 RID: 4833
	public bool IsMusicPlayingThisTick;

	// Token: 0x040012E2 RID: 4834
	public bool WasMusicPlayingLastTick;

	// Token: 0x040012E3 RID: 4835
	public static readonly float PlayBanThreshold = 1f;

	// Token: 0x040012E4 RID: 4836
	public static readonly float deadWindow = 0.16666667f;

	// Token: 0x040012E5 RID: 4837
	public static GameRandom Random;

	// Token: 0x040012E6 RID: 4838
	public DMSUpdateConditions UpdateConditions;
}
