using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using BhvrAnalyticsServices.Interfaces;
using Platform;
using Services;
using Services.Analytics.Events;
using UnityEngine;

// Token: 0x02000764 RID: 1892
public class ConnectionManager : SingletonMonoBehaviour<ConnectionManager>
{
	// Token: 0x14000041 RID: 65
	// (add) Token: 0x0600380E RID: 14350 RVA: 0x0016FECC File Offset: 0x0016E0CC
	// (remove) Token: 0x0600380F RID: 14351 RVA: 0x0016FF04 File Offset: 0x0016E104
	public event Action OnDisconnectFromServer;

	// Token: 0x14000042 RID: 66
	// (add) Token: 0x06003810 RID: 14352 RVA: 0x0016FF3C File Offset: 0x0016E13C
	// (remove) Token: 0x06003811 RID: 14353 RVA: 0x0016FF70 File Offset: 0x0016E170
	public static event ConnectionManager.ClientConnectionAction OnClientAdded;

	// Token: 0x14000043 RID: 67
	// (add) Token: 0x06003812 RID: 14354 RVA: 0x0016FFA4 File Offset: 0x0016E1A4
	// (remove) Token: 0x06003813 RID: 14355 RVA: 0x0016FFD8 File Offset: 0x0016E1D8
	public static event ConnectionManager.ClientConnectionAction OnClientDisconnected;

	// Token: 0x1700058D RID: 1421
	// (get) Token: 0x06003814 RID: 14356 RVA: 0x0017000B File Offset: 0x0016E20B
	public bool HasRunningServers
	{
		get
		{
			return this.protocolManager.HasRunningServers;
		}
	}

	// Token: 0x1700058E RID: 1422
	// (get) Token: 0x06003815 RID: 14357 RVA: 0x00170018 File Offset: 0x0016E218
	public ProtocolManager.NetworkType CurrentMode
	{
		get
		{
			return this.protocolManager.CurrentMode;
		}
	}

	// Token: 0x1700058F RID: 1423
	// (get) Token: 0x06003816 RID: 14358 RVA: 0x00170025 File Offset: 0x0016E225
	public bool IsServer
	{
		get
		{
			return this.protocolManager.IsServer;
		}
	}

	// Token: 0x17000590 RID: 1424
	// (get) Token: 0x06003817 RID: 14359 RVA: 0x00170032 File Offset: 0x0016E232
	public bool IsClient
	{
		get
		{
			return this.protocolManager.IsClient;
		}
	}

	// Token: 0x17000591 RID: 1425
	// (get) Token: 0x06003818 RID: 14360 RVA: 0x0017003F File Offset: 0x0016E23F
	public bool IsSinglePlayer
	{
		get
		{
			return this.IsServer && this.ClientCount() == 0;
		}
	}

	// Token: 0x17000592 RID: 1426
	// (get) Token: 0x06003819 RID: 14361 RVA: 0x00170054 File Offset: 0x0016E254
	// (set) Token: 0x0600381A RID: 14362 RVA: 0x0017005C File Offset: 0x0016E25C
	public GameServerInfo LastGameServerInfo { get; set; }

	// Token: 0x17000593 RID: 1427
	// (get) Token: 0x0600381B RID: 14363 RVA: 0x00170065 File Offset: 0x0016E265
	// (set) Token: 0x0600381C RID: 14364 RVA: 0x0017006D File Offset: 0x0016E26D
	public GameServerInfo LocalServerInfo { get; set; }

	// Token: 0x17000594 RID: 1428
	// (get) Token: 0x0600381D RID: 14365 RVA: 0x00170076 File Offset: 0x0016E276
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

	// Token: 0x0600381E RID: 14366 RVA: 0x00170098 File Offset: 0x0016E298
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

	// Token: 0x0600381F RID: 14367 RVA: 0x001700F2 File Offset: 0x0016E2F2
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void singletonDestroy()
	{
		base.singletonDestroy();
		GamePrefs.OnGamePrefChanged -= this.OnGamePrefChanged;
	}

	// Token: 0x06003820 RID: 14368 RVA: 0x0017010B File Offset: 0x0016E30B
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnGamePrefChanged(EnumGamePrefs _pref)
	{
		if (_pref == EnumGamePrefs.ServerPassword)
		{
			this.protocolManager.SetServerPassword(GamePrefs.GetString(EnumGamePrefs.ServerPassword));
		}
	}

	// Token: 0x06003821 RID: 14369 RVA: 0x00170124 File Offset: 0x0016E324
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

	// Token: 0x06003822 RID: 14370 RVA: 0x001701F6 File Offset: 0x0016E3F6
	public void BeginHeartbeat()
	{
		this.countdownAnalyticsHeartbeat.ResetAndRestart();
	}

	// Token: 0x06003823 RID: 14371 RVA: 0x00170204 File Offset: 0x0016E404
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

	// Token: 0x06003824 RID: 14372 RVA: 0x00170310 File Offset: 0x0016E510
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

	// Token: 0x06003825 RID: 14373 RVA: 0x001703A9 File Offset: 0x0016E5A9
	public void SetConnectionToServer(INetConnection[] _cons)
	{
		this.connectionToServer = _cons;
	}

	// Token: 0x06003826 RID: 14374 RVA: 0x001703B2 File Offset: 0x0016E5B2
	public INetConnection[] GetConnectionToServer()
	{
		return this.connectionToServer;
	}

	// Token: 0x06003827 RID: 14375 RVA: 0x001703BA File Offset: 0x0016E5BA
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

	// Token: 0x06003828 RID: 14376 RVA: 0x001703FC File Offset: 0x0016E5FC
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

	// Token: 0x06003829 RID: 14377 RVA: 0x0017044C File Offset: 0x0016E64C
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

	// Token: 0x0600382A RID: 14378 RVA: 0x001704E6 File Offset: 0x0016E6E6
	public void MakeServerOffline()
	{
		this.protocolManager.MakeServerOffline();
	}

	// Token: 0x0600382B RID: 14379 RVA: 0x001704F4 File Offset: 0x0016E6F4
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

	// Token: 0x0600382C RID: 14380 RVA: 0x00170561 File Offset: 0x0016E761
	public void ServerReady()
	{
		if (!this.IsConnected)
		{
			this.Clients.Clear();
		}
		this.IsConnected = true;
	}

	// Token: 0x0600382D RID: 14381 RVA: 0x0017057D File Offset: 0x0016E77D
	public int ClientCount()
	{
		return this.Clients.Count;
	}

	// Token: 0x0600382E RID: 14382 RVA: 0x0017058C File Offset: 0x0016E78C
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

	// Token: 0x0600382F RID: 14383 RVA: 0x001705E4 File Offset: 0x0016E7E4
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

	// Token: 0x06003830 RID: 14384 RVA: 0x00170826 File Offset: 0x0016EA26
	public void SetClientEntityId(ClientInfo _cInfo, int _entityId, PlayerDataFile _pdf)
	{
		_cInfo.entityId = _entityId;
		_cInfo.bAttachedToEntity = true;
		_cInfo.latestPlayerData = _pdf;
	}

	// Token: 0x06003831 RID: 14385 RVA: 0x00170840 File Offset: 0x0016EA40
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

	// Token: 0x06003832 RID: 14386 RVA: 0x00170A00 File Offset: 0x0016EC00
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

	// Token: 0x06003833 RID: 14387 RVA: 0x00170B28 File Offset: 0x0016ED28
	public void FlushClientSendQueues()
	{
		for (int i = 0; i < this.Clients.List.Count; i++)
		{
			ClientInfo clientInfo = this.Clients.List[i];
			clientInfo.netConnection[0].FlushSendQueue();
			clientInfo.netConnection[1].FlushSendQueue();
		}
	}

	// Token: 0x06003834 RID: 14388 RVA: 0x00170B7C File Offset: 0x0016ED7C
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdatePings()
	{
		for (int i = 0; i < this.Clients.List.Count; i++)
		{
			this.Clients.List[i].UpdatePing();
		}
	}

	// Token: 0x06003835 RID: 14389 RVA: 0x00170BBA File Offset: 0x0016EDBA
	public string GetRequiredPortsString()
	{
		return this.protocolManager.GetGamePortsString();
	}

	// Token: 0x06003836 RID: 14390 RVA: 0x00170BC8 File Offset: 0x0016EDC8
	public void SendToClientsOrServer(NetPackage _package)
	{
		if (!this.IsServer)
		{
			this.SendToServer(_package, false);
			return;
		}
		this.SendPackage(_package, false, -1, -1, -1, null, 192, false);
	}

	// Token: 0x06003837 RID: 14391 RVA: 0x00170C00 File Offset: 0x0016EE00
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
			HeartbeatEventData heartbeatEventData = new HeartbeatEventData();
			heartbeatEventData.HeartbeatTimestamp = DateTime.UtcNow.ToString("O");
			GameServerInfo currentGameServerInfoServerOrClient = SingletonMonoBehaviour<ConnectionManager>.Instance.CurrentGameServerInfoServerOrClient;
			heartbeatEventData.ServerId = ((currentGameServerInfoServerOrClient != null) ? currentGameServerInfoServerOrClient.GetValue(GameInfoString.UniqueId) : null);
			HeartbeatEventData heartbeatEventData2 = heartbeatEventData;
			ServiceProvider.Instance.Get<IAnalyticsService>().LogEvent(heartbeatEventData2);
		}
	}

	// Token: 0x06003838 RID: 14392 RVA: 0x00170E53 File Offset: 0x0016F053
	[PublicizedFrom(EAccessModifier.Private)]
	public void LateUpdate()
	{
		this.protocolManager.LateUpdate();
	}

	// Token: 0x06003839 RID: 14393 RVA: 0x00170E60 File Offset: 0x0016F060
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

	// Token: 0x0600383A RID: 14394 RVA: 0x00170FB8 File Offset: 0x0016F1B8
	public void PlayerAllowed(string _gameInfo, PlatformLobbyId _platformLobbyId, [TupleElementNames(new string[] { "userId", "token" })] ValueTuple<PlatformUserIdentifierAbs, string> _platformUserAndToken, [TupleElementNames(new string[] { "userId", "token" })] ValueTuple<PlatformUserIdentifierAbs, string> _crossplatformUserAndToken)
	{
		ConnectionManager.<>c__DisplayClass73_0 CS$<>8__locals1 = new ConnectionManager.<>c__DisplayClass73_0();
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
		CS$<>8__locals1.authorizers = ConnectionManager.<PlayerAllowed>g__GetAuthenticationClients|73_3();
		CS$<>8__locals1.authorizerIndex = -1;
		CS$<>8__locals1.<PlayerAllowed>g__NextAuthorizer|0();
	}

	// Token: 0x0600383B RID: 14395 RVA: 0x001710E4 File Offset: 0x0016F2E4
	public void PlayerDenied(string _reason)
	{
		if (this.IsClient)
		{
			this.protocolManager.Disconnect();
			Log.Out("Player denied: " + _reason);
			XUiC_MessageBoxWindowGroup.ShowOk(this.windowManager.playerUI.xui, Localization.Get("mmLblErrorConnectionDeniedTitle", false, null), _reason, "", null, true, true, false);
		}
	}

	// Token: 0x0600383C RID: 14396 RVA: 0x00171140 File Offset: 0x0016F340
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

	// Token: 0x0600383D RID: 14397 RVA: 0x0017129C File Offset: 0x0016F49C
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

	// Token: 0x0600383E RID: 14398 RVA: 0x00171360 File Offset: 0x0016F560
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

	// Token: 0x0600383F RID: 14399 RVA: 0x001713C3 File Offset: 0x0016F5C3
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

	// Token: 0x06003840 RID: 14400 RVA: 0x00171400 File Offset: 0x0016F600
	public void Net_DisconnectedFromServer(string _reason)
	{
		Log.Out("[NET] DisconnectedFromServer: " + _reason);
		this.DisconnectFromServer();
		XUiC_MessageBoxWindowGroup.ShowOk(this.windowManager.playerUI.xui, Localization.Get("mmLblErrorConnectionLost", false, null), _reason, "", null, true, true, false);
	}

	// Token: 0x06003841 RID: 14401 RVA: 0x0017144E File Offset: 0x0016F64E
	public void Net_DataReceivedClient(int _channel, byte[] _data, int _size)
	{
		if (this.connectionToServer[_channel] != null)
		{
			this.connectionToServer[_channel].AppendToReaderStream(_data, _size);
		}
	}

	// Token: 0x06003842 RID: 14402 RVA: 0x00171469 File Offset: 0x0016F669
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

	// Token: 0x06003843 RID: 14403 RVA: 0x00171483 File Offset: 0x0016F683
	public void Net_PlayerConnected(ClientInfo _cInfo)
	{
		Log.Out(string.Format("[NET] PlayerConnected {0}", _cInfo));
		_cInfo.netConnection[0].AddToSendQueue(NetPackageManager.GetPackage<NetPackagePackageIds>().Setup());
	}

	// Token: 0x06003844 RID: 14404 RVA: 0x001714AC File Offset: 0x0016F6AC
	public void Net_PlayerDisconnected(ClientInfo _cInfo)
	{
		if (_cInfo != null)
		{
			Log.Out(string.Format("[NET] PlayerDisconnected {0}", _cInfo));
			this.DisconnectClient(_cInfo, false, false);
		}
	}

	// Token: 0x06003845 RID: 14405 RVA: 0x001714CA File Offset: 0x0016F6CA
	public void SetLatencySimulation(bool _enable, int _min, int _max)
	{
		this.protocolManager.SetLatencySimulation(_enable, _min, _max);
	}

	// Token: 0x06003846 RID: 14406 RVA: 0x001714DA File Offset: 0x0016F6DA
	public void SetPacketLossSimulation(bool _enable, int _chance)
	{
		this.protocolManager.SetPacketLossSimulation(_enable, _chance);
	}

	// Token: 0x06003847 RID: 14407 RVA: 0x001714E9 File Offset: 0x0016F6E9
	public void EnableNetworkStatistics()
	{
		this.protocolManager.EnableNetworkStatistics();
	}

	// Token: 0x06003848 RID: 14408 RVA: 0x001714F6 File Offset: 0x0016F6F6
	public void DisableNetworkStatistics()
	{
		this.protocolManager.DisableNetworkStatistics();
	}

	// Token: 0x06003849 RID: 14409 RVA: 0x00171503 File Offset: 0x0016F703
	public string PrintNetworkStatistics()
	{
		return this.protocolManager.PrintNetworkStatistics();
	}

	// Token: 0x0600384A RID: 14410 RVA: 0x00171510 File Offset: 0x0016F710
	public void ResetNetworkStatistics()
	{
		this.protocolManager.ResetNetworkStatistics();
		this.protocolManager.DisableNetworkStatistics();
	}

	// Token: 0x0600384E RID: 14414 RVA: 0x001715E4 File Offset: 0x0016F7E4
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static IAuthenticationClient[] <PlayerAllowed>g__GetAuthenticationClients|73_3()
	{
		IAuthenticationClient[] array = new IAuthenticationClient[2];
		array[0] = PlatformManager.NativePlatform.AuthenticationClient;
		int num = 1;
		IPlatform crossplatformPlatform = PlatformManager.CrossplatformPlatform;
		array[num] = ((crossplatformPlatform != null) ? crossplatformPlatform.AuthenticationClient : null);
		return array.Where<IAuthenticationClient>((IAuthenticationClient authorizer) => authorizer != null).ToArray<IAuthenticationClient>();
	}

	// Token: 0x04002DF1 RID: 11761
	public const int CHANNELCOUNT = 2;

	// Token: 0x04002DF2 RID: 11762
	public static bool VerboseNetLogging;

	// Token: 0x04002DF3 RID: 11763
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GUIWindowManager windowManager;

	// Token: 0x04002DF4 RID: 11764
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public INetConnection[] connectionToServer = new INetConnection[2];

	// Token: 0x04002DF6 RID: 11766
	public readonly AntiCheatEncryptionAuthClient AntiCheatEncryptionAuthClient = new AntiCheatEncryptionAuthClient();

	// Token: 0x04002DF7 RID: 11767
	public readonly ClientInfoCollection Clients = new ClientInfoCollection();

	// Token: 0x04002DFA RID: 11770
	public readonly AntiCheatEncryptionAuthServer AntiCheatEncryptionAuthServer = new AntiCheatEncryptionAuthServer();

	// Token: 0x04002DFB RID: 11771
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float lastBadPacketCheck;

	// Token: 0x04002DFC RID: 11772
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const int badPacketDisconnectThreshold = 3;

	// Token: 0x04002DFD RID: 11773
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public ProtocolManager protocolManager;

	// Token: 0x04002DFE RID: 11774
	public bool IsConnected;

	// Token: 0x04002DFF RID: 11775
	public int ReceivedBytesThisFrame;

	// Token: 0x04002E00 RID: 11776
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly CountdownTimer updateClientInfo = new CountdownTimer(5f, true);

	// Token: 0x04002E01 RID: 11777
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public CountdownTimer countdownAnalyticsHeartbeat = new CountdownTimer(300f, false);

	// Token: 0x04002E04 RID: 11780
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly List<NetPackage> packagesToProcess = new List<NetPackage>();

	// Token: 0x02000765 RID: 1893
	// (Invoke) Token: 0x06003852 RID: 14418
	public delegate void ClientConnectionAction(ClientInfo _clientInfo);
}
