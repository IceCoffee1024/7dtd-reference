using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using BhvrAnalyticsServices.Interfaces;
using Platform;
using Services;
using Services.Analytics;
using Services.Analytics.Events;
using UnityEngine;

// Token: 0x02000778 RID: 1912
public class ConnectionManager : SingletonMonoBehaviour<ConnectionManager>
{
	// Token: 0x14000042 RID: 66
	// (add) Token: 0x060038BE RID: 14526 RVA: 0x001739E4 File Offset: 0x00171BE4
	// (remove) Token: 0x060038BF RID: 14527 RVA: 0x00173A1C File Offset: 0x00171C1C
	public event Action OnDisconnectFromServer;

	// Token: 0x14000043 RID: 67
	// (add) Token: 0x060038C0 RID: 14528 RVA: 0x00173A54 File Offset: 0x00171C54
	// (remove) Token: 0x060038C1 RID: 14529 RVA: 0x00173A88 File Offset: 0x00171C88
	public static event ConnectionManager.ClientConnectionAction OnClientAdded;

	// Token: 0x14000044 RID: 68
	// (add) Token: 0x060038C2 RID: 14530 RVA: 0x00173ABC File Offset: 0x00171CBC
	// (remove) Token: 0x060038C3 RID: 14531 RVA: 0x00173AF0 File Offset: 0x00171CF0
	public static event ConnectionManager.ClientConnectionAction OnClientDisconnected;

	// Token: 0x170005C0 RID: 1472
	// (get) Token: 0x060038C4 RID: 14532 RVA: 0x00173B23 File Offset: 0x00171D23
	public bool HasRunningServers
	{
		get
		{
			return this.protocolManager.HasRunningServers;
		}
	}

	// Token: 0x170005C1 RID: 1473
	// (get) Token: 0x060038C5 RID: 14533 RVA: 0x00173B30 File Offset: 0x00171D30
	public ProtocolManager.NetworkType CurrentMode
	{
		get
		{
			return this.protocolManager.CurrentMode;
		}
	}

	// Token: 0x170005C2 RID: 1474
	// (get) Token: 0x060038C6 RID: 14534 RVA: 0x00173B3D File Offset: 0x00171D3D
	public bool IsServer
	{
		get
		{
			return this.protocolManager.IsServer;
		}
	}

	// Token: 0x170005C3 RID: 1475
	// (get) Token: 0x060038C7 RID: 14535 RVA: 0x00173B4A File Offset: 0x00171D4A
	public bool IsClient
	{
		get
		{
			return this.protocolManager.IsClient;
		}
	}

	// Token: 0x170005C4 RID: 1476
	// (get) Token: 0x060038C8 RID: 14536 RVA: 0x00173B57 File Offset: 0x00171D57
	public bool IsSinglePlayer
	{
		get
		{
			return this.IsServer && this.ClientCount() == 0;
		}
	}

	// Token: 0x170005C5 RID: 1477
	// (get) Token: 0x060038C9 RID: 14537 RVA: 0x00173B6C File Offset: 0x00171D6C
	// (set) Token: 0x060038CA RID: 14538 RVA: 0x00173B74 File Offset: 0x00171D74
	public GameServerInfo LastGameServerInfo { get; set; }

	// Token: 0x170005C6 RID: 1478
	// (get) Token: 0x060038CB RID: 14539 RVA: 0x00173B7D File Offset: 0x00171D7D
	// (set) Token: 0x060038CC RID: 14540 RVA: 0x00173B85 File Offset: 0x00171D85
	public GameServerInfo LocalServerInfo { get; set; }

	// Token: 0x170005C7 RID: 1479
	// (get) Token: 0x060038CD RID: 14541 RVA: 0x00173B8E File Offset: 0x00171D8E
	public GameServerInfo CurrentGameServerInfoServerOrClient
	{
		get
		{
			if (this.IsServer)
			{
				return this.LocalServerInfo;
			}
			if (!this.IsClient)
			{
				return null;
			}
			return this.LastGameServerInfo;
		}
	}

	// Token: 0x170005C8 RID: 1480
	// (get) Token: 0x060038CE RID: 14542 RVA: 0x00173BAF File Offset: 0x00171DAF
	// (set) Token: 0x060038CF RID: 14543 RVA: 0x00173BB7 File Offset: 0x00171DB7
	public string LastJoinSource { get; set; } = "Unknown";

	// Token: 0x060038D0 RID: 14544 RVA: 0x00173BC0 File Offset: 0x00171DC0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void singletonAwake()
	{
		this.windowManager = (GUIWindowManager)global::UnityEngine.Object.FindObjectOfType(typeof(GUIWindowManager));
		if (GameUtils.GetLaunchArgument("debugnet") != null)
		{
			ConnectionManager.VerboseNetLogging = true;
		}
		this.protocolManager = new ProtocolManager();
		GamePrefs.OnGamePrefChanged += this.OnGamePrefChanged;
		NetPackageLogger.Init();
	}

	// Token: 0x060038D1 RID: 14545 RVA: 0x00173C1A File Offset: 0x00171E1A
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void singletonDestroy()
	{
		base.singletonDestroy();
		GamePrefs.OnGamePrefChanged -= this.OnGamePrefChanged;
	}

	// Token: 0x060038D2 RID: 14546 RVA: 0x00173C33 File Offset: 0x00171E33
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnGamePrefChanged(EnumGamePrefs _pref)
	{
		if (_pref == EnumGamePrefs.ServerPassword)
		{
			this.protocolManager.SetServerPassword(GamePrefs.GetString(EnumGamePrefs.ServerPassword));
		}
	}

	// Token: 0x060038D3 RID: 14547 RVA: 0x00173C4C File Offset: 0x00171E4C
	public void Disconnect()
	{
		if (this.Clients != null)
		{
			for (int i = 0; i < this.Clients.List.Count; i++)
			{
				ClientInfo clientInfo = this.Clients.List[i];
				this.DisconnectClient(clientInfo, true, false);
			}
			this.Clients.Clear();
		}
		if (this.connectionToServer[0] != null)
		{
			IAntiCheatClient antiCheatClient = PlatformManager.MultiPlatform.AntiCheatClient;
			if (antiCheatClient != null)
			{
				antiCheatClient.DisconnectFromServer();
			}
			this.connectionToServer[0].Disconnect(false);
			this.LastGameServerInfo = null;
		}
		INetConnection netConnection = this.connectionToServer[1];
		if (netConnection != null)
		{
			netConnection.Disconnect(false);
		}
		this.connectionToServer[0] = null;
		this.connectionToServer[1] = null;
		if (this.IsConnected && !this.IsServer)
		{
			this.protocolManager.Disconnect();
		}
		this.IsConnected = false;
	}

	// Token: 0x060038D4 RID: 14548 RVA: 0x00173D1E File Offset: 0x00171F1E
	public void BeginHeartbeat(float seconds)
	{
		this.countdownAnalyticsHeartbeat.SetTimeout(seconds);
		this.countdownAnalyticsHeartbeat.ResetAndRestart();
	}

	// Token: 0x060038D5 RID: 14549 RVA: 0x00173D38 File Offset: 0x00171F38
	[PublicizedFrom(EAccessModifier.Private)]
	public void openConnectProgressWindow(GameServerInfo _gameServerInfo)
	{
		string text = (GeneratedTextManager.IsFiltered(_gameServerInfo.ServerDisplayName) ? GeneratedTextManager.GetDisplayTextImmediately(_gameServerInfo.ServerDisplayName, false, GeneratedTextManager.TextFilteringMode.Filter, GeneratedTextManager.BbCodeSupportMode.SupportedAndAddEscapes) : _gameServerInfo.GetValue(GameInfoString.GameHost));
		string text2;
		if (!string.IsNullOrEmpty(text))
		{
			Log.Out("Connecting to server " + text + "...");
			text2 = string.Format(Localization.Get("msgConnectingToServer", false, null), Utils.EscapeBbCodes(text, false, false));
		}
		else
		{
			Log.Out(string.Concat(new string[]
			{
				"Connecting to server ",
				_gameServerInfo.GetValue(GameInfoString.IP),
				":",
				_gameServerInfo.GetValue(GameInfoInt.Port).ToString(),
				"..."
			}));
			text2 = string.Format(Localization.Get("msgConnectingToServer", false, null), _gameServerInfo.GetValue(GameInfoString.IP) + ":" + _gameServerInfo.GetValue(GameInfoInt.Port).ToString());
		}
		text2 = text2 + "\n\n[FFFFFF]" + Utils.GetCancellationMessage();
		XUiC_ProgressWindow.Open(LocalPlayerUI.primaryUI, text2, delegate
		{
			this.Disconnect();
			LocalPlayerUI.primaryUI.windowManager.Open(XUiC_MainMenu.ID, true);
		}, true, true, false);
	}

	// Token: 0x060038D6 RID: 14550 RVA: 0x00173E44 File Offset: 0x00172044
	public void Connect(GameServerInfo _gameServerInfo)
	{
		if (PlatformApplicationManager.IsRestartRequired)
		{
			Log.Warning("A restart was pending when attempting to connect to a server.");
			this.Net_ConnectionFailed(Localization.Get("app_restartRequired", false, null));
			return;
		}
		if (!PermissionsManager.IsMultiplayerAllowed())
		{
			this.Net_ConnectionFailed(Localization.Get("xuiConnectFailed_MpNotAllowed", false, null));
			return;
		}
		if (ProfileSDF.CurrentProfileName().Length == 0)
		{
			string[] profiles = ProfileSDF.GetProfiles();
			if (profiles.Length != 0)
			{
				ProfileSDF.SetSelectedProfile(profiles[global::UnityEngine.Random.Range(0, profiles.Length - 1)]);
			}
		}
		this.IsConnected = true;
		this.LastGameServerInfo = _gameServerInfo;
		this.openConnectProgressWindow(_gameServerInfo);
		NetPackageManager.StartClient();
		this.protocolManager.ConnectToServer(_gameServerInfo);
	}

	// Token: 0x060038D7 RID: 14551 RVA: 0x00173EDD File Offset: 0x001720DD
	public void SetConnectionToServer(INetConnection[] _cons)
	{
		this.connectionToServer = _cons;
	}

	// Token: 0x060038D8 RID: 14552 RVA: 0x00173EE6 File Offset: 0x001720E6
	public INetConnection[] GetConnectionToServer()
	{
		return this.connectionToServer;
	}

	// Token: 0x060038D9 RID: 14553 RVA: 0x00173EEE File Offset: 0x001720EE
	public void DisconnectFromServer()
	{
		Action onDisconnectFromServer = this.OnDisconnectFromServer;
		if (onDisconnectFromServer != null)
		{
			onDisconnectFromServer();
		}
		this.Disconnect();
		if (GameManager.Instance != null)
		{
			GameManager.Instance.SaveAndCleanupWorld();
		}
		if (GamePrefs.GetInt(EnumGamePrefs.AutopilotMode) > 0)
		{
			Application.Quit();
		}
	}

	// Token: 0x060038DA RID: 14554 RVA: 0x00173F30 File Offset: 0x00172130
	public void SendToServer(NetPackage _package, bool _flush = false)
	{
		int channel = _package.Channel;
		if (this.connectionToServer[channel] == null)
		{
			if (this.IsConnected)
			{
				Log.Error("Can not queue package for server: NetConnection null");
			}
			return;
		}
		this.connectionToServer[channel].AddToSendQueue(_package);
		if (_flush)
		{
			this.connectionToServer[channel].FlushSendQueue();
		}
	}

	// Token: 0x060038DB RID: 14555 RVA: 0x00173F80 File Offset: 0x00172180
	public NetworkConnectionError StartServers(string _password, bool _offline)
	{
		if (PlatformApplicationManager.IsRestartRequired)
		{
			Log.Warning("A restart was pending when attempting to start servers.");
			return NetworkConnectionError.RestartRequired;
		}
		NetworkConnectionError networkConnectionError = NetworkConnectionError.NoError;
		if (!GameManager.IsDedicatedServer && !_offline)
		{
			if (PlatformManager.MultiPlatform.User.UserStatus == EUserStatus.OfflineMode)
			{
				Log.Out("Can not start servers in online mode because user is in offline mode. Starting server in offline mode.");
				_offline = true;
			}
			else if (!PermissionsManager.IsMultiplayerAllowed() || !PermissionsManager.CanHostMultiplayer())
			{
				Log.Out("Can not start servers in online mode because user does not have multiplayer hosting permissions. Starting in offline mode.");
				_offline = true;
			}
		}
		if (_offline)
		{
			this.protocolManager.StartOfflineServer();
		}
		else
		{
			networkConnectionError = this.protocolManager.StartServers(_password);
		}
		if (networkConnectionError == NetworkConnectionError.NoError)
		{
			GameManager.Instance.StartGame(_offline);
		}
		NetPackageManager.StartServer();
		return networkConnectionError;
	}

	// Token: 0x060038DC RID: 14556 RVA: 0x0017401A File Offset: 0x0017221A
	public void MakeServerOffline()
	{
		this.protocolManager.MakeServerOffline();
	}

	// Token: 0x060038DD RID: 14557 RVA: 0x00174028 File Offset: 0x00172228
	public void StopServers()
	{
		Log.Out("[NET] ServerShutdown");
		this.protocolManager.StopServers();
		this.Disconnect();
		if (GameManager.Instance != null)
		{
			GameManager.Instance.SaveAndCleanupWorld();
		}
		if (this.LocalServerInfo != null)
		{
			this.LocalServerInfo.ClearOnChanged();
			this.LocalServerInfo = null;
		}
		NetPackageManager.ResetMappings();
		if (GamePrefs.GetInt(EnumGamePrefs.AutopilotMode) > 0)
		{
			Application.Quit();
		}
	}

	// Token: 0x060038DE RID: 14558 RVA: 0x00174095 File Offset: 0x00172295
	public void ServerReady()
	{
		if (!this.IsConnected)
		{
			this.Clients.Clear();
		}
		this.IsConnected = true;
	}

	// Token: 0x060038DF RID: 14559 RVA: 0x001740B1 File Offset: 0x001722B1
	public int ClientCount()
	{
		return this.Clients.Count;
	}

	// Token: 0x060038E0 RID: 14560 RVA: 0x001740C0 File Offset: 0x001722C0
	public void AddClient(ClientInfo _cInfo)
	{
		ConnectionManager.ClientConnectionAction onClientAdded = ConnectionManager.OnClientAdded;
		if (onClientAdded != null)
		{
			onClientAdded(_cInfo);
		}
		this.Clients.Add(_cInfo);
		GameSparksCollector.SetMax(GameSparksCollector.GSDataKey.PeakConcurrentClients, null, this.ClientCount(), false, GameSparksCollector.GSDataCollection.SessionTotal);
		GameSparksCollector.SetMax(GameSparksCollector.GSDataKey.PeakConcurrentPlayers, null, this.ClientCount() + (GameManager.IsDedicatedServer ? 0 : 1), false, GameSparksCollector.GSDataCollection.SessionTotal);
	}

	// Token: 0x060038E1 RID: 14561 RVA: 0x00174118 File Offset: 0x00172318
	public void DisconnectClient(ClientInfo _cInfo, bool _bShutdown = false, bool _clientDisconnect = false)
	{
		if (!ThreadManager.IsMainThread())
		{
			ThreadManager.AddSingleTaskMainThread("CM.DisconnectClient-" + _cInfo.ClientNumber.ToString(), delegate(object _parameter)
			{
				ValueTuple<ClientInfo, bool, bool> valueTuple = (ValueTuple<ClientInfo, bool, bool>)_parameter;
				ClientInfo item = valueTuple.Item1;
				bool item2 = valueTuple.Item2;
				bool item3 = valueTuple.Item3;
				this.DisconnectClient(item, item2, item3);
			}, new ValueTuple<ClientInfo, bool, bool>(_cInfo, _bShutdown, _clientDisconnect));
			return;
		}
		if (_cInfo == null)
		{
			Log.Error("DisconnectClient: ClientInfo is null");
			return;
		}
		if (!this.Clients.Contains(_cInfo))
		{
			Log.Warning("DisconnectClient: Player " + _cInfo.InternalId.CombinedString + " not found");
			Log.Out("From: " + StackTraceUtility.ExtractStackTrace());
			return;
		}
		ConnectionManager.ClientConnectionAction onClientDisconnected = ConnectionManager.OnClientDisconnected;
		if (onClientDisconnected != null)
		{
			onClientDisconnected(_cInfo);
		}
		ModEvents.SPlayerDisconnectedData splayerDisconnectedData = new ModEvents.SPlayerDisconnectedData(_cInfo, _bShutdown);
		ModEvents.PlayerDisconnected.Invoke(ref splayerDisconnectedData);
		Log.Out(string.Format("Player disconnected: {0}", _cInfo));
		if (_cInfo.latestPlayerData != null)
		{
			PlayerDataFile latestPlayerData = _cInfo.latestPlayerData;
			if (latestPlayerData.bModifiedSinceLastSave)
			{
				latestPlayerData.Save(GameIO.GetPlayerDataDir(), _cInfo.InternalId.CombinedString);
			}
		}
		INetConnection netConnection = _cInfo.netConnection[0];
		if (netConnection != null)
		{
			netConnection.Disconnect(false);
		}
		INetConnection netConnection2 = _cInfo.netConnection[1];
		if (netConnection2 != null)
		{
			netConnection2.Disconnect(false);
		}
		AuthorizationManager.Instance.Disconnect(_cInfo);
		if (!_bShutdown)
		{
			World world = GameManager.Instance.World;
			EntityPlayer entityPlayer = ((EntityAlive)((world != null) ? world.GetEntity(_cInfo.entityId) : null)) as EntityPlayer;
			if (entityPlayer != null)
			{
				entityPlayer.bWillRespawn = false;
				entityPlayer.PartyDisconnect();
				QuestEventManager.Current.HandlePlayerDisconnect(entityPlayer);
				LockManager.Instance.ForceUnlockByPlayer(_cInfo.entityId);
				GameManager.Instance.GameMessage(EnumGameMessages.LeftGame, entityPlayer, null);
				if (GameManager.Instance.World.m_ChunkManager != null)
				{
					GameManager.Instance.World.m_ChunkManager.RemoveChunkObserver(entityPlayer.ChunkObserver);
				}
				GameManager.Instance.World.RemoveEntity(_cInfo.entityId, EnumRemoveEntityReason.Unloaded);
				GameEventManager.Current.HandleForceBossDespawn(entityPlayer);
			}
		}
		else
		{
			World world2 = GameManager.Instance.World;
			EntityAlive entityAlive = (EntityAlive)((world2 != null) ? world2.GetEntity(_cInfo.entityId) : null);
			if (entityAlive != null)
			{
				QuestEventManager.Current.HandlePlayerDisconnect(entityAlive as EntityPlayer);
			}
		}
		if (!_bShutdown)
		{
			this.Clients.Remove(_cInfo);
			_cInfo.network.DropClient(_cInfo, _clientDisconnect);
		}
	}

	// Token: 0x060038E2 RID: 14562 RVA: 0x0017435A File Offset: 0x0017255A
	public void SetClientEntityId(ClientInfo _cInfo, int _entityId, PlayerDataFile _pdf)
	{
		_cInfo.entityId = _entityId;
		_cInfo.bAttachedToEntity = true;
		_cInfo.latestPlayerData = _pdf;
	}

	// Token: 0x060038E3 RID: 14563 RVA: 0x00174374 File Offset: 0x00172574
	public void SendPackage(List<NetPackage> _packages, bool _onlyClientsAttachedToAnEntity = false, int _attachedToEntityId = -1, int _allButAttachedToEntityId = -1, int _entitiesInRangeOfEntity = -1, Vector3? _entitiesInRangeOfWorldPos = null, int _range = 192, bool _onlyClientsNotAttachedToAnEntity = false)
	{
		if (this.Clients == null)
		{
			return;
		}
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		for (int i = 0; i < _packages.Count; i++)
		{
			_packages[i].RegisterSendQueue();
		}
		for (int j = 0; j < this.Clients.List.Count; j++)
		{
			ClientInfo clientInfo = this.Clients.List[j];
			if (clientInfo.loginDone && (!_onlyClientsAttachedToAnEntity || clientInfo.bAttachedToEntity) && (!_onlyClientsNotAttachedToAnEntity || !clientInfo.bAttachedToEntity) && (_attachedToEntityId == -1 || (clientInfo.bAttachedToEntity && clientInfo.entityId == _attachedToEntityId)) && (_allButAttachedToEntityId == -1 || (clientInfo.bAttachedToEntity && clientInfo.entityId != _allButAttachedToEntityId)) && (_entitiesInRangeOfEntity == -1 || GameManager.Instance.World.IsEntityInRange(_entitiesInRangeOfEntity, clientInfo.entityId, _range)) && (_entitiesInRangeOfWorldPos == null || GameManager.Instance.World.IsEntityInRange(clientInfo.entityId, _entitiesInRangeOfWorldPos.Value, _range)))
			{
				for (int k = 0; k < _packages.Count; k++)
				{
					NetPackage netPackage = _packages[k];
					clientInfo.netConnection[netPackage.Channel].AddToSendQueue(netPackage);
					if (netPackage.Channel == 1)
					{
						flag2 = true;
					}
					else
					{
						flag = true;
					}
					flag3 |= netPackage.FlushQueue;
				}
				if (flag3)
				{
					if (flag)
					{
						clientInfo.netConnection[0].FlushSendQueue();
					}
					if (flag2)
					{
						clientInfo.netConnection[1].FlushSendQueue();
					}
				}
			}
		}
		for (int l = 0; l < _packages.Count; l++)
		{
			_packages[l].SendQueueHandled();
		}
	}

	// Token: 0x060038E4 RID: 14564 RVA: 0x00174534 File Offset: 0x00172734
	public void SendPackage(NetPackage _package, bool _onlyClientsAttachedToAnEntity = false, int _attachedToEntityId = -1, int _allButAttachedToEntityId = -1, int _entitiesInRangeOfEntity = -1, Vector3? _entitiesInRangeOfWorldPos = null, int _range = 192, bool _onlyClientsNotAttachedToAnEntity = false)
	{
		if (this.Clients == null)
		{
			return;
		}
		_package.RegisterSendQueue();
		for (int i = 0; i < this.Clients.List.Count; i++)
		{
			ClientInfo clientInfo = this.Clients.List[i];
			if (clientInfo.loginDone && (!_onlyClientsAttachedToAnEntity || clientInfo.bAttachedToEntity) && (!_onlyClientsNotAttachedToAnEntity || !clientInfo.bAttachedToEntity) && (_attachedToEntityId == -1 || (clientInfo.bAttachedToEntity && clientInfo.entityId == _attachedToEntityId)) && (_allButAttachedToEntityId == -1 || (clientInfo.bAttachedToEntity && clientInfo.entityId != _allButAttachedToEntityId)) && (_entitiesInRangeOfEntity == -1 || GameManager.Instance.World.IsEntityInRange(_entitiesInRangeOfEntity, clientInfo.entityId, _range)) && (_entitiesInRangeOfWorldPos == null || GameManager.Instance.World.IsEntityInRange(clientInfo.entityId, _entitiesInRangeOfWorldPos.Value, _range)))
			{
				clientInfo.netConnection[_package.Channel].AddToSendQueue(_package);
				if (_package.FlushQueue)
				{
					clientInfo.netConnection[_package.Channel].FlushSendQueue();
				}
			}
		}
		_package.SendQueueHandled();
	}

	// Token: 0x060038E5 RID: 14565 RVA: 0x0017465C File Offset: 0x0017285C
	public void FlushClientSendQueues()
	{
		for (int i = 0; i < this.Clients.List.Count; i++)
		{
			ClientInfo clientInfo = this.Clients.List[i];
			clientInfo.netConnection[0].FlushSendQueue();
			clientInfo.netConnection[1].FlushSendQueue();
		}
	}

	// Token: 0x060038E6 RID: 14566 RVA: 0x001746B0 File Offset: 0x001728B0
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdatePings()
	{
		for (int i = 0; i < this.Clients.List.Count; i++)
		{
			this.Clients.List[i].UpdatePing();
		}
	}

	// Token: 0x060038E7 RID: 14567 RVA: 0x001746EE File Offset: 0x001728EE
	public string GetRequiredPortsString()
	{
		return this.protocolManager.GetGamePortsString();
	}

	// Token: 0x060038E8 RID: 14568 RVA: 0x001746FC File Offset: 0x001728FC
	public void SendToClientsOrServer(NetPackage _package)
	{
		if (!this.IsServer)
		{
			this.SendToServer(_package, false);
			return;
		}
		this.SendPackage(_package, false, -1, -1, -1, null, 192, false);
	}

	// Token: 0x060038E9 RID: 14569 RVA: 0x00174734 File Offset: 0x00172934
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Update()
	{
		this.protocolManager.Update();
		if (this.IsServer)
		{
			bool flag = Time.time - this.lastBadPacketCheck > 1f;
			if (flag)
			{
				this.lastBadPacketCheck = Time.time;
			}
			for (int i = 0; i < this.Clients.Count; i++)
			{
				ClientInfo clientInfo = this.Clients.List[i];
				if (!clientInfo.netConnection[0].IsDisconnected())
				{
					if (flag && clientInfo.entityId != -1 && !clientInfo.disconnecting && clientInfo.network.GetBadPacketCount(clientInfo) >= 3)
					{
						GameUtils.KickPlayerForClientInfo(clientInfo, new GameUtils.KickPlayerData(GameUtils.EKickReason.BadMTUPackets, 0, default(DateTime), ""));
					}
					else
					{
						this.ProcessPackages(clientInfo.netConnection[0], NetPackageDirection.ToClient, clientInfo);
						if (i < this.Clients.Count)
						{
							this.ProcessPackages(clientInfo.netConnection[1], NetPackageDirection.ToClient, clientInfo);
						}
					}
				}
			}
			this.FlushClientSendQueues();
			if (this.updateClientInfo.HasPassed() && GameManager.Instance.World != null && this.ClientCount() > 0)
			{
				this.UpdatePings();
				this.updateClientInfo.ResetAndRestart();
				this.SendPackage(NetPackageManager.GetPackage<NetPackageClientInfo>().Setup(GameManager.Instance.World, this.Clients.List), true, -1, -1, -1, null, 192, false);
			}
		}
		else
		{
			if (this.connectionToServer[0] != null && !this.connectionToServer[0].IsDisconnected())
			{
				this.ProcessPackages(this.connectionToServer[0], NetPackageDirection.ToServer, null);
				INetConnection netConnection = this.connectionToServer[0];
				if (netConnection != null)
				{
					netConnection.FlushSendQueue();
				}
			}
			if (this.connectionToServer[1] != null && !this.connectionToServer[1].IsDisconnected())
			{
				this.ProcessPackages(this.connectionToServer[1], NetPackageDirection.ToServer, null);
				INetConnection netConnection2 = this.connectionToServer[1];
				if (netConnection2 != null)
				{
					netConnection2.FlushSendQueue();
				}
			}
		}
		if (!GameManager.IsDedicatedServer && this.countdownAnalyticsHeartbeat.HasPassed())
		{
			this.countdownAnalyticsHeartbeat.ResetAndRestart();
			HeartbeatEventData heartbeatEventData = new HeartbeatEventData
			{
				HeartbeatTimestamp = DateTime.UtcNow.ToString("O"),
				ServerId = Helper.GetServerId(),
				SaveId = Helper.GetSaveId(),
				PlayerCount = Helper.GetServerPlayerCount()
			};
			ServiceProvider.Instance.Get<IAnalyticsService>().LogEvent(heartbeatEventData);
		}
	}

	// Token: 0x060038EA RID: 14570 RVA: 0x0017498A File Offset: 0x00172B8A
	[PublicizedFrom(EAccessModifier.Private)]
	public void LateUpdate()
	{
		this.protocolManager.LateUpdate();
	}

	// Token: 0x060038EB RID: 14571 RVA: 0x00174998 File Offset: 0x00172B98
	[PublicizedFrom(EAccessModifier.Private)]
	public void ProcessPackages(INetConnection _connection, NetPackageDirection _disallowedDirection, ClientInfo _clientInfo = null)
	{
		if (_connection == null)
		{
			Log.Error("ProcessPackages: connection == null");
			return;
		}
		_connection.GetPackages(this.packagesToProcess);
		if (this.packagesToProcess == null)
		{
			Log.Error("ProcessPackages: packages == null");
			return;
		}
		for (int i = 0; i < this.packagesToProcess.Count; i++)
		{
			NetPackage netPackage = this.packagesToProcess[i];
			if (netPackage == null)
			{
				Log.Error(string.Concat(new string[]
				{
					"ProcessPackages: packages [",
					i.ToString(),
					"] == null (packages.Count == ",
					this.packagesToProcess.Count.ToString(),
					")"
				}));
			}
			else if (netPackage.PackageDirection == _disallowedDirection)
			{
				if (_clientInfo == null)
				{
					Log.Warning(string.Format("[NET] Received package {0} which is only allowed to be sent to the server", netPackage));
				}
				else
				{
					Log.Warning(string.Format("[NET] Received package {0} which is only allowed to be sent to clients from client {1}", netPackage, _clientInfo));
				}
			}
			else if (_clientInfo != null && !netPackage.AllowedBeforeAuth && !_clientInfo.loginDone)
			{
				Log.Warning(string.Format("[NET] Received an unexpected package ({0}) before authentication was finished from client {1}", netPackage, _clientInfo));
			}
			else if (netPackage.ShouldProcess(GameManager.Instance.World, GameManager.Instance))
			{
				netPackage.ProcessPackage(GameManager.Instance.World, GameManager.Instance);
				NetPackageManager.FreePackage(netPackage);
			}
			else
			{
				netPackage.HandleSkipped(GameManager.Instance.World, GameManager.Instance);
			}
		}
	}

	// Token: 0x060038EC RID: 14572 RVA: 0x00174AF0 File Offset: 0x00172CF0
	public void PlayerAllowed(string _gameInfo, PlatformLobbyId _platformLobbyId, [TupleElementNames(new string[] { "userId", "token" })] ValueTuple<PlatformUserIdentifierAbs, string> _platformUserAndToken, [TupleElementNames(new string[] { "userId", "token" })] ValueTuple<PlatformUserIdentifierAbs, string> _crossplatformUserAndToken)
	{
		ConnectionManager.<>c__DisplayClass77_0 CS$<>8__locals1 = new ConnectionManager.<>c__DisplayClass77_0();
		CS$<>8__locals1.<>4__this = this;
		CS$<>8__locals1._platformUserAndToken = _platformUserAndToken;
		CS$<>8__locals1._crossplatformUserAndToken = _crossplatformUserAndToken;
		CS$<>8__locals1._platformLobbyId = _platformLobbyId;
		if (!this.IsClient)
		{
			return;
		}
		Log.Out("Player allowed");
		if (this.LastGameServerInfo.GetValue(GameInfoBool.IsDedicated))
		{
			ServerInfoCache.Instance.AddHistory(this.LastGameServerInfo);
		}
		this.LastGameServerInfo.GetValue(GameInfoString.IP);
		this.LastGameServerInfo.GetValue(GameInfoInt.Port);
		this.LastGameServerInfo = new GameServerInfo(_gameInfo);
		if ((!LaunchPrefs.AllowJoinConfigModded.Value && this.LastGameServerInfo.GetValue(GameInfoBool.ModdedConfig)) || ((DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5).IsCurrent() && this.LastGameServerInfo.GetValue(GameInfoBool.RequiresMod)))
		{
			CS$<>8__locals1.<PlayerAllowed>g__AuthorizerDisconnect|2(Localization.Get("auth_moddedconfigdetected", false, null));
			return;
		}
		PlatformUserIdentifierAbs item = CS$<>8__locals1._platformUserAndToken.Item1;
		if (item != null)
		{
			item.DecodeTicket(CS$<>8__locals1._platformUserAndToken.Item2);
		}
		PlatformUserIdentifierAbs item2 = CS$<>8__locals1._crossplatformUserAndToken.Item1;
		if (item2 != null)
		{
			item2.DecodeTicket(CS$<>8__locals1._crossplatformUserAndToken.Item2);
		}
		CS$<>8__locals1.authorizers = ConnectionManager.<PlayerAllowed>g__GetAuthenticationClients|77_3();
		CS$<>8__locals1.authorizerIndex = -1;
		CS$<>8__locals1.<PlayerAllowed>g__NextAuthorizer|0();
	}

	// Token: 0x060038ED RID: 14573 RVA: 0x00174C1C File Offset: 0x00172E1C
	public void PlayerDenied(string _reason)
	{
		if (this.IsClient)
		{
			this.protocolManager.Disconnect();
			Log.Out("Player denied: " + _reason);
			XUiC_MessageBoxWindowGroup.ShowOk(this.windowManager.playerUI.xui, Localization.Get("mmLblErrorConnectionDeniedTitle", false, null), _reason, "", null, true, true, false);
		}
	}

	// Token: 0x060038EE RID: 14574 RVA: 0x00174C78 File Offset: 0x00172E78
	public void ServerConsoleCommand(ClientInfo _cInfo, string _cmd)
	{
		if (GameManager.Instance == null)
		{
			return;
		}
		if (_cmd.Length > 300)
		{
			Log.Warning("Client tried to execute command with {0} characters. First 20: '{1}'", new object[]
			{
				_cmd.Length,
				_cmd.Substring(0, 20)
			});
			return;
		}
		IConsoleCommand command = SingletonMonoBehaviour<SdtdConsole>.Instance.GetCommand(_cmd, false);
		if (command == null)
		{
			_cInfo.SendPackage(NetPackageManager.GetPackage<NetPackageConsoleCmdClient>().Setup("Unknown command", false));
			return;
		}
		if (!command.CanExecuteForDevice)
		{
			_cInfo.SendPackage(NetPackageManager.GetPackage<NetPackageConsoleCmdClient>().Setup("Command not permitted on the server's device", false));
			return;
		}
		string[] commands = command.GetCommands();
		AdminTools adminTools = GameManager.Instance.adminTools;
		if (adminTools == null || !adminTools.CommandAllowedFor(commands, _cInfo))
		{
			Log.Out(string.Format("Denying command '{0}' from client {1}", _cmd, _cInfo));
			_cInfo.SendPackage(NetPackageManager.GetPackage<NetPackageConsoleCmdClient>().Setup(string.Format(Localization.Get("msgServer25", false, null), _cmd, _cInfo.playerName), false));
			return;
		}
		if (command.IsExecuteOnClient)
		{
			Log.Out("Client {0}/{1} executing client side command: {2}", new object[]
			{
				_cInfo.InternalId.CombinedString,
				_cInfo.playerName,
				_cmd
			});
			_cInfo.SendPackage(NetPackageManager.GetPackage<NetPackageConsoleCmdClient>().Setup(_cmd, true));
			return;
		}
		List<string> list = SingletonMonoBehaviour<SdtdConsole>.Instance.ExecuteSync(_cmd, _cInfo);
		_cInfo.SendPackage(NetPackageManager.GetPackage<NetPackageConsoleCmdClient>().Setup(list, false));
	}

	// Token: 0x060038EF RID: 14575 RVA: 0x00174DD4 File Offset: 0x00172FD4
	public void SendLogin()
	{
		PlatformUserIdentifierAbs platformUserId = PlatformManager.NativePlatform.User.PlatformUserId;
		IAuthenticationClient authenticationClient = PlatformManager.NativePlatform.AuthenticationClient;
		ValueTuple<PlatformUserIdentifierAbs, string> valueTuple = new ValueTuple<PlatformUserIdentifierAbs, string>(platformUserId, (authenticationClient != null) ? authenticationClient.GetAuthTicket() : null);
		IPlatform crossplatformPlatform = PlatformManager.CrossplatformPlatform;
		PlatformUserIdentifierAbs platformUserIdentifierAbs = ((crossplatformPlatform != null) ? crossplatformPlatform.User.PlatformUserId : null);
		IPlatform crossplatformPlatform2 = PlatformManager.CrossplatformPlatform;
		ValueTuple<PlatformUserIdentifierAbs, string> valueTuple2 = new ValueTuple<PlatformUserIdentifierAbs, string>(platformUserIdentifierAbs, ((crossplatformPlatform2 != null) ? crossplatformPlatform2.AuthenticationClient.GetAuthTicket() : null) ?? "");
		ulong num = (DiscordManager.Instance.IsReady ? DiscordManager.Instance.LocalUser.ID : 0UL);
		this.SendToServer(NetPackageManager.GetPackage<NetPackagePlayerLogin>().Setup(GamePrefs.GetString(EnumGamePrefs.PlayerName), valueTuple, valueTuple2, Constants.cVersionInformation.LongStringNoBuild, Constants.cVersionInformation.LongStringNoBuild, num), false);
	}

	// Token: 0x060038F0 RID: 14576 RVA: 0x00174E98 File Offset: 0x00173098
	public void Net_ConnectionFailed(string _message)
	{
		Log.Error("[NET] Connection to server failed: " + _message);
		XUiC_MessageBoxWindowGroup.ShowOk(this.windowManager.playerUI.xui, Localization.Get("mmLblErrorConnectionFailed", false, null), _message, "", null, true, true, false);
		this.IsConnected = false;
		IAntiCheatClient antiCheatClient = PlatformManager.MultiPlatform.AntiCheatClient;
		if (antiCheatClient == null)
		{
			return;
		}
		antiCheatClient.DisconnectFromServer();
	}

	// Token: 0x060038F1 RID: 14577 RVA: 0x00174EFB File Offset: 0x001730FB
	public void Net_InvalidPassword()
	{
		XUiC_ServerPasswordWindow.OpenPasswordWindow(LocalPlayerUI.primaryUI.xui, true, ServerInfoCache.Instance.GetPassword(this.LastGameServerInfo), true, delegate(string _pwd)
		{
			ServerInfoCache.Instance.SavePassword(this.LastGameServerInfo, _pwd);
			this.Connect(this.LastGameServerInfo);
		}, delegate
		{
			this.windowManager.Open(XUiC_ServerBrowser.ID, true);
			this.Disconnect();
		});
	}

	// Token: 0x060038F2 RID: 14578 RVA: 0x00174F38 File Offset: 0x00173138
	public void Net_DisconnectedFromServer(string _reason)
	{
		Log.Out("[NET] DisconnectedFromServer: " + _reason);
		this.DisconnectFromServer();
		XUiC_MessageBoxWindowGroup.ShowOk(this.windowManager.playerUI.xui, Localization.Get("mmLblErrorConnectionLost", false, null), _reason, "", null, true, true, false);
	}

	// Token: 0x060038F3 RID: 14579 RVA: 0x00174F86 File Offset: 0x00173186
	public void Net_DataReceivedClient(int _channel, byte[] _data, int _size)
	{
		if (this.connectionToServer[_channel] != null)
		{
			this.connectionToServer[_channel].AppendToReaderStream(_data, _size);
		}
	}

	// Token: 0x060038F4 RID: 14580 RVA: 0x00174FA1 File Offset: 0x001731A1
	public void Net_DataReceivedServer(ClientInfo _cInfo, int _channel, byte[] _data, int _size)
	{
		if (_cInfo != null)
		{
			INetConnection netConnection = _cInfo.netConnection[_channel];
			if (netConnection == null)
			{
				return;
			}
			netConnection.AppendToReaderStream(_data, _size);
		}
	}

	// Token: 0x060038F5 RID: 14581 RVA: 0x00174FBB File Offset: 0x001731BB
	public void Net_PlayerConnected(ClientInfo _cInfo)
	{
		Log.Out(string.Format("[NET] PlayerConnected {0}", _cInfo));
		_cInfo.netConnection[0].AddToSendQueue(NetPackageManager.GetPackage<NetPackagePackageIds>().Setup());
	}

	// Token: 0x060038F6 RID: 14582 RVA: 0x00174FE4 File Offset: 0x001731E4
	public void Net_PlayerDisconnected(ClientInfo _cInfo)
	{
		if (_cInfo != null)
		{
			Log.Out(string.Format("[NET] PlayerDisconnected {0}", _cInfo));
			this.DisconnectClient(_cInfo, false, false);
		}
	}

	// Token: 0x060038F7 RID: 14583 RVA: 0x00175002 File Offset: 0x00173202
	public void SetLatencySimulation(bool _enable, int _min, int _max)
	{
		this.protocolManager.SetLatencySimulation(_enable, _min, _max);
	}

	// Token: 0x060038F8 RID: 14584 RVA: 0x00175012 File Offset: 0x00173212
	public void SetPacketLossSimulation(bool _enable, int _chance)
	{
		this.protocolManager.SetPacketLossSimulation(_enable, _chance);
	}

	// Token: 0x060038F9 RID: 14585 RVA: 0x00175021 File Offset: 0x00173221
	public void EnableNetworkStatistics()
	{
		this.protocolManager.EnableNetworkStatistics();
	}

	// Token: 0x060038FA RID: 14586 RVA: 0x0017502E File Offset: 0x0017322E
	public void DisableNetworkStatistics()
	{
		this.protocolManager.DisableNetworkStatistics();
	}

	// Token: 0x060038FB RID: 14587 RVA: 0x0017503B File Offset: 0x0017323B
	public string PrintNetworkStatistics()
	{
		return this.protocolManager.PrintNetworkStatistics();
	}

	// Token: 0x060038FC RID: 14588 RVA: 0x00175048 File Offset: 0x00173248
	public void ResetNetworkStatistics()
	{
		this.protocolManager.ResetNetworkStatistics();
		this.protocolManager.DisableNetworkStatistics();
	}

	// Token: 0x06003900 RID: 14592 RVA: 0x00175128 File Offset: 0x00173328
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static IAuthenticationClient[] <PlayerAllowed>g__GetAuthenticationClients|77_3()
	{
		IAuthenticationClient[] array = new IAuthenticationClient[2];
		array[0] = PlatformManager.NativePlatform.AuthenticationClient;
		int num = 1;
		IPlatform crossplatformPlatform = PlatformManager.CrossplatformPlatform;
		array[num] = ((crossplatformPlatform != null) ? crossplatformPlatform.AuthenticationClient : null);
		return array.Where<IAuthenticationClient>((IAuthenticationClient authorizer) => authorizer != null).ToArray<IAuthenticationClient>();
	}

	// Token: 0x04002E7E RID: 11902
	public const int CHANNELCOUNT = 2;

	// Token: 0x04002E7F RID: 11903
	public static bool VerboseNetLogging;

	// Token: 0x04002E80 RID: 11904
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GUIWindowManager windowManager;

	// Token: 0x04002E81 RID: 11905
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public INetConnection[] connectionToServer = new INetConnection[2];

	// Token: 0x04002E83 RID: 11907
	public readonly AntiCheatEncryptionAuthClient AntiCheatEncryptionAuthClient = new AntiCheatEncryptionAuthClient();

	// Token: 0x04002E84 RID: 11908
	public readonly ClientInfoCollection Clients = new ClientInfoCollection();

	// Token: 0x04002E87 RID: 11911
	public readonly AntiCheatEncryptionAuthServer AntiCheatEncryptionAuthServer = new AntiCheatEncryptionAuthServer();

	// Token: 0x04002E88 RID: 11912
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float lastBadPacketCheck;

	// Token: 0x04002E89 RID: 11913
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const int badPacketDisconnectThreshold = 3;

	// Token: 0x04002E8A RID: 11914
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public ProtocolManager protocolManager;

	// Token: 0x04002E8B RID: 11915
	public bool IsConnected;

	// Token: 0x04002E8C RID: 11916
	public int ReceivedBytesThisFrame;

	// Token: 0x04002E8D RID: 11917
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly CountdownTimer updateClientInfo = new CountdownTimer(5f, true);

	// Token: 0x04002E8E RID: 11918
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public CountdownTimer countdownAnalyticsHeartbeat = new CountdownTimer(300f, false);

	// Token: 0x04002E92 RID: 11922
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly List<NetPackage> packagesToProcess = new List<NetPackage>();

	// Token: 0x02000779 RID: 1913
	// (Invoke) Token: 0x06003904 RID: 14596
	public delegate void ClientConnectionAction(ClientInfo _clientInfo);
}
