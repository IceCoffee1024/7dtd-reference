using System;
using DynamicMusic.Legacy;
using DynamicMusic.Legacy.ObjectModel;

// Token: 0x020003C6 RID: 966
public class DynamicMusicManager : IGamePrefsChangedListener
{
	// Token: 0x17000365 RID: 869
	// (get) Token: 0x06001CD8 RID: 7384 RVA: 0x000AE823 File Offset: 0x000ACA23
	public bool MusicStarted
	{
		get
		{
			return this.IsMusicPlayingThisTick && !this.WasMusicPlayingLastTick;
		}
	}

	// Token: 0x17000366 RID: 870
	// (get) Token: 0x06001CD9 RID: 7385 RVA: 0x000AE838 File Offset: 0x000ACA38
	public bool MusicStopped
	{
		get
		{
			return !this.IsMusicPlayingThisTick && this.WasMusicPlayingLastTick;
		}
	}

	// Token: 0x17000367 RID: 871
	// (get) Token: 0x06001CDA RID: 7386 RVA: 0x000AE84A File Offset: 0x000ACA4A
	public bool IsDynamicMusicPlaying
	{
		get
		{
			return this.IsMusicPlayingThisTick;
		}
	}

	// Token: 0x17000368 RID: 872
	// (get) Token: 0x06001CDB RID: 7387 RVA: 0x000AE852 File Offset: 0x000ACA52
	public bool IsBeforeDuskPlayBan
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return SkyManager.GetTimeOfDayAsMinutes() < SkyManager.GetDuskTimeAsMinutes() - DynamicMusicManager.PlayBanThreshold;
		}
	}

	// Token: 0x17000369 RID: 873
	// (get) Token: 0x06001CDC RID: 7388 RVA: 0x000AE866 File Offset: 0x000ACA66
	public bool IsAfterDusk
	{
		get
		{
			return SkyManager.TimeOfDay() > SkyManager.GetDuskTime();
		}
	}

	// Token: 0x1700036A RID: 874
	// (get) Token: 0x06001CDD RID: 7389 RVA: 0x000AE874 File Offset: 0x000ACA74
	public bool IsAfterDuskWindow
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return SkyManager.GetTimeOfDayAsMinutes() > SkyManager.GetDuskTimeAsMinutes() + DynamicMusicManager.deadWindow;
		}
	}

	// Token: 0x1700036B RID: 875
	// (get) Token: 0x06001CDE RID: 7390 RVA: 0x000AE888 File Offset: 0x000ACA88
	public bool IsBeforeDawnPlayBan
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return SkyManager.GetTimeOfDayAsMinutes() < SkyManager.GetDawnTimeAsMinutes() - DynamicMusicManager.PlayBanThreshold;
		}
	}

	// Token: 0x1700036C RID: 876
	// (get) Token: 0x06001CDF RID: 7391 RVA: 0x000AE89C File Offset: 0x000ACA9C
	public bool IsAfterDawn
	{
		get
		{
			return SkyManager.TimeOfDay() > SkyManager.GetDawnTime();
		}
	}

	// Token: 0x1700036D RID: 877
	// (get) Token: 0x06001CE0 RID: 7392 RVA: 0x000AE8AA File Offset: 0x000ACAAA
	public bool IsAfterDawnWindow
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return SkyManager.GetTimeOfDayAsMinutes() > SkyManager.GetDawnTimeAsMinutes() + DynamicMusicManager.deadWindow;
		}
	}

	// Token: 0x1700036E RID: 878
	// (get) Token: 0x06001CE1 RID: 7393 RVA: 0x000AE8BE File Offset: 0x000ACABE
	// (set) Token: 0x06001CE2 RID: 7394 RVA: 0x000AE8C6 File Offset: 0x000ACAC6
	public bool IsInDeadWindow
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x1700036F RID: 879
	// (get) Token: 0x06001CE3 RID: 7395 RVA: 0x000AE8CF File Offset: 0x000ACACF
	// (set) Token: 0x06001CE4 RID: 7396 RVA: 0x000AE8D7 File Offset: 0x000ACAD7
	public bool IsPlayAllowed
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000370 RID: 880
	// (get) Token: 0x06001CE5 RID: 7397 RVA: 0x000AE8E0 File Offset: 0x000ACAE0
	// (set) Token: 0x06001CE6 RID: 7398 RVA: 0x000AE8E8 File Offset: 0x000ACAE8
	public float DistanceFromDeadWindow
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000371 RID: 881
	// (get) Token: 0x06001CE7 RID: 7399 RVA: 0x000AE8F1 File Offset: 0x000ACAF1
	// (set) Token: 0x06001CE8 RID: 7400 RVA: 0x000AE8F9 File Offset: 0x000ACAF9
	public bool IsPlayerInTraderStation
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06001CE9 RID: 7401 RVA: 0x000AE902 File Offset: 0x000ACB02
	[PublicizedFrom(EAccessModifier.Private)]
	public DynamicMusicManager()
	{
		this.UpdateConditions = default(DMSUpdateConditions);
		this.UpdateConditions.IsDMSEnabled = GamePrefs.GetBool(EnumGamePrefs.OptionsDynamicMusicEnabled);
		this.UpdateConditions.IsGameUnPaused = true;
	}

	// Token: 0x06001CEA RID: 7402 RVA: 0x000AE938 File Offset: 0x000ACB38
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

	// Token: 0x06001CEB RID: 7403 RVA: 0x000AE9D4 File Offset: 0x000ACBD4
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

	// Token: 0x06001CEC RID: 7404 RVA: 0x000AEB46 File Offset: 0x000ACD46
	public void CleanUpDynamicMembers()
	{
		if (this.StreamerMaster != null)
		{
			this.StreamerMaster.Cleanup();
		}
		this.UpdateConditions.IsDMSInitialized = false;
	}

	// Token: 0x06001CED RID: 7405 RVA: 0x000AEB67 File Offset: 0x000ACD67
	public static void Cleanup()
	{
		MusicGroup.Cleanup();
		ConfigSet.Cleanup();
	}

	// Token: 0x06001CEE RID: 7406 RVA: 0x000AEB74 File Offset: 0x000ACD74
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

	// Token: 0x06001CEF RID: 7407 RVA: 0x000AEC52 File Offset: 0x000ACE52
	public void OnPlayerDeath()
	{
		this.StreamerMaster.Stop();
	}

	// Token: 0x06001CF0 RID: 7408 RVA: 0x000AEC5F File Offset: 0x000ACE5F
	public void OnPlayerFirstSpawned()
	{
		this.FrequencyManager.OnPlayerFirstSpawned();
	}

	// Token: 0x06001CF1 RID: 7409 RVA: 0x000AEC6C File Offset: 0x000ACE6C
	public void Pause()
	{
		this.UpdateConditions.IsGameUnPaused = false;
		this.StreamerMaster.Pause();
		this.FrequencyManager.OnPause();
	}

	// Token: 0x06001CF2 RID: 7410 RVA: 0x000AEC90 File Offset: 0x000ACE90
	public void UnPause()
	{
		this.UpdateConditions.IsGameUnPaused = true;
		this.StreamerMaster.UnPause();
		this.FrequencyManager.OnUnPause();
	}

	// Token: 0x06001CF3 RID: 7411 RVA: 0x000AECB4 File Offset: 0x000ACEB4
	[PublicizedFrom(EAccessModifier.Private)]
	public bool IsPrimaryPlayerInTraderStation()
	{
		return GameManager.Instance.World.IsWithinTraderArea(this.PrimaryLocalPlayer.GetBlockPosition());
	}

	// Token: 0x06001CF4 RID: 7412 RVA: 0x000AECD0 File Offset: 0x000ACED0
	public bool IsInDawnOrDuskRange(float _dawnOrDuskTime, float _currentTime)
	{
		return this.DistanceFromDawnOrDusk(_dawnOrDuskTime, _currentTime) <= DynamicMusicManager.deadWindow;
	}

	// Token: 0x06001CF5 RID: 7413 RVA: 0x000AECE4 File Offset: 0x000ACEE4
	public float DistanceFromDawnOrDusk(float _dawnOrDuskTime, float _currentTime)
	{
		return Math.Abs(_dawnOrDuskTime - _currentTime);
	}

	// Token: 0x06001CF6 RID: 7414 RVA: 0x000AECF0 File Offset: 0x000ACEF0
	[PublicizedFrom(EAccessModifier.Private)]
	public void IGamePrefsChangedListener.OnGamePrefChanged(EnumGamePrefs _enum)
	{
		if (_enum == EnumGamePrefs.OptionsDynamicMusicEnabled && !(this.UpdateConditions.IsDMSEnabled = GamePrefs.GetBool(EnumGamePrefs.OptionsDynamicMusicEnabled)))
		{
			this.StreamerMaster.Stop();
		}
	}

	// Token: 0x17000372 RID: 882
	// (get) Token: 0x06001CF7 RID: 7415 RVA: 0x000AED2A File Offset: 0x000ACF2A
	public float TimeToNextDayEvent
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return (SkyManager.IsDark() ? SkyManager.GetDawnTimeAsMinutes() : SkyManager.GetDuskTimeAsMinutes()) - SkyManager.GetTimeOfDayAsMinutes();
		}
	}

	// Token: 0x040012D5 RID: 4821
	public EntityPlayerLocal PrimaryLocalPlayer;

	// Token: 0x040012D6 RID: 4822
	public ThreatLevelTracker ThreatLevelTracker;

	// Token: 0x040012D7 RID: 4823
	public FrequencyManager FrequencyManager;

	// Token: 0x040012D8 RID: 4824
	public TransitionManager TransitionManager;

	// Token: 0x040012D9 RID: 4825
	public StreamerMaster StreamerMaster;

	// Token: 0x040012DA RID: 4826
	public bool IsMusicPlayingThisTick;

	// Token: 0x040012DB RID: 4827
	public bool WasMusicPlayingLastTick;

	// Token: 0x040012DC RID: 4828
	public static readonly float PlayBanThreshold = 1f;

	// Token: 0x040012DD RID: 4829
	public static readonly float deadWindow = 0.16666667f;

	// Token: 0x040012DE RID: 4830
	public static GameRandom Random;

	// Token: 0x040012DF RID: 4831
	public DMSUpdateConditions UpdateConditions;
}
