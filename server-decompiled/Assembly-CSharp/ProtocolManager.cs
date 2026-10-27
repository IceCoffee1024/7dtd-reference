using System;
using System.Collections;
using System.Collections.Generic;
using Platform;
using UnityEngine;

// Token: 0x020008A0 RID: 2208
public class ProtocolManager : IProtocolManagerProtocolInterface
{
	// Token: 0x170006C4 RID: 1732
	// (get) Token: 0x06003F88 RID: 16264 RVA: 0x00190861 File Offset: 0x0018EA61
	// (set) Token: 0x06003F89 RID: 16265 RVA: 0x00190869 File Offset: 0x0018EA69
	public bool HasRunningServers
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170006C5 RID: 1733
	// (get) Token: 0x06003F8A RID: 16266 RVA: 0x00190872 File Offset: 0x0018EA72
	// (set) Token: 0x06003F8B RID: 16267 RVA: 0x0019087A File Offset: 0x0018EA7A
	public ProtocolManager.NetworkType CurrentMode
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170006C6 RID: 1734
	// (get) Token: 0x06003F8C RID: 16268 RVA: 0x00190883 File Offset: 0x0018EA83
	public bool IsServer
	{
		get
		{
			return this.CurrentMode == ProtocolManager.NetworkType.Server || this.CurrentMode == ProtocolManager.NetworkType.OfflineServer;
		}
	}

	// Token: 0x170006C7 RID: 1735
	// (get) Token: 0x06003F8D RID: 16269 RVA: 0x00190899 File Offset: 0x0018EA99
	public bool IsClient
	{
		get
		{
			return this.CurrentMode == ProtocolManager.NetworkType.Client;
		}
	}

	// Token: 0x06003F8E RID: 16270 RVA: 0x001908A4 File Offset: 0x0018EAA4
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetupProtocols()
	{
		if (this.servers.Count == 0)
		{
			string @string = GamePrefs.GetString(EnumGamePrefs.ServerDisabledNetworkProtocols);
			List<string> list = new List<string>();
			if (!string.IsNullOrEmpty(@string))
			{
				list.AddRange(@string.ToLower().Split(',', StringSplitOptions.None));
			}
			if (GameUtils.GetLaunchArgument("nounet") != null)
			{
				list.Add("unet");
			}
			if (GameUtils.GetLaunchArgument("noraknet") != null)
			{
				list.Add("raknet");
			}
			if (GameUtils.GetLaunchArgument("nolitenetlib") != null)
			{
				list.Add("litenetlib");
			}
			if (!list.Contains("litenetlib"))
			{
				this.servers.Add(new NetworkServerLiteNetLib(this));
				this.clients.Add(new NetworkClientLiteNetLib(this));
			}
			else
			{
				Log.Out("[NET] Disabling protocol: LiteNetLib");
			}
			if (PlatformManager.NativePlatform.HasNetworkingEnabled(list))
			{
				this.servers.Add(PlatformManager.NativePlatform.GetNetworkingServer(this));
				if (!GameManager.IsDedicatedServer)
				{
					this.clients.Add(PlatformManager.NativePlatform.GetNetworkingClient(this));
				}
			}
			IPlatform crossplatformPlatform = PlatformManager.CrossplatformPlatform;
			if (crossplatformPlatform != null && crossplatformPlatform.HasNetworkingEnabled(list))
			{
				this.servers.Add(PlatformManager.CrossplatformPlatform.GetNetworkingServer(this));
				if (!GameManager.IsDedicatedServer)
				{
					this.clients.Add(PlatformManager.CrossplatformPlatform.GetNetworkingClient(this));
				}
			}
			foreach (KeyValuePair<EPlatformIdentifier, IPlatform> keyValuePair in PlatformManager.ServerPlatforms)
			{
				if (keyValuePair.Value.AsServerOnly && keyValuePair.Value.HasNetworkingEnabled(list))
				{
					this.servers.Add(keyValuePair.Value.GetNetworkingServer(this));
				}
			}
		}
	}

	// Token: 0x06003F8F RID: 16271 RVA: 0x00190A64 File Offset: 0x0018EC64
	public string GetGamePortsString()
	{
		string text = "";
		string serverPorts = ServerInformationTcpProvider.Instance.GetServerPorts();
		if (!string.IsNullOrEmpty(serverPorts))
		{
			if (!string.IsNullOrEmpty(text))
			{
				text += ", ";
			}
			text += serverPorts;
		}
		IMasterServerAnnouncer serverListAnnouncer = PlatformManager.MultiPlatform.ServerListAnnouncer;
		string text2 = ((serverListAnnouncer != null) ? serverListAnnouncer.GetServerPorts() : null);
		if (!string.IsNullOrEmpty(text2))
		{
			if (!string.IsNullOrEmpty(text))
			{
				text += ", ";
			}
			text += text2;
		}
		int @int = GamePrefs.GetInt(EnumGamePrefs.ServerPort);
		for (int i = 0; i < this.servers.Count; i++)
		{
			string serverPorts2 = this.servers[i].GetServerPorts(@int);
			if (!string.IsNullOrEmpty(serverPorts2))
			{
				if (!string.IsNullOrEmpty(text))
				{
					text += ", ";
				}
				text += serverPorts2;
			}
		}
		return text;
	}

	// Token: 0x06003F90 RID: 16272 RVA: 0x00190B40 File Offset: 0x0018ED40
	public void Update()
	{
		for (int i = 0; i < this.servers.Count; i++)
		{
			this.servers[i].Update();
		}
		for (int j = 0; j < this.clients.Count; j++)
		{
			this.clients[j].Update();
		}
	}

	// Token: 0x06003F91 RID: 16273 RVA: 0x00190B9C File Offset: 0x0018ED9C
	public void LateUpdate()
	{
		for (int i = 0; i < this.servers.Count; i++)
		{
			this.servers[i].LateUpdate();
		}
		for (int j = 0; j < this.clients.Count; j++)
		{
			this.clients[j].LateUpdate();
		}
	}

	// Token: 0x06003F92 RID: 16274 RVA: 0x00190BF7 File Offset: 0x0018EDF7
	public void StartOfflineServer()
	{
		Log.Out("NET: Starting offline server.");
		this.CurrentMode = ProtocolManager.NetworkType.OfflineServer;
	}

	// Token: 0x06003F93 RID: 16275 RVA: 0x00190C0C File Offset: 0x0018EE0C
	public NetworkConnectionError StartServers(string _password)
	{
		if (PlatformManager.MultiPlatform.User.UserStatus == EUserStatus.OfflineMode || !PermissionsManager.IsMultiplayerAllowed() || !PermissionsManager.CanHostMultiplayer())
		{
			Log.Warning(string.Format("NET: User unable to create online server. User status: {0}, Multiplayer allowed: {1}, Host Multiplayer allowed: {2}", PlatformManager.MultiPlatform.User.UserStatus, PermissionsManager.IsMultiplayerAllowed(), PermissionsManager.CanHostMultiplayer()));
			this.StartOfflineServer();
			return NetworkConnectionError.NoError;
		}
		Log.Out("NET: Starting server protocols");
		this.SetupProtocols();
		this.CurrentMode = ProtocolManager.NetworkType.Server;
		NetworkConnectionError networkConnectionError = NetworkConnectionError.NoError;
		int @int = GamePrefs.GetInt(EnumGamePrefs.ServerPort);
		if (@int < 1024 || @int > 65530)
		{
			Log.Error(string.Format("NET: Starting server protocols failed: Invalid ServerPort {0}, must be within 1024 and 65530", @int));
			return NetworkConnectionError.InvalidPort;
		}
		for (int i = 0; i < this.servers.Count; i++)
		{
			networkConnectionError = this.servers[i].StartServer(@int, _password);
			if (networkConnectionError != NetworkConnectionError.NoError)
			{
				break;
			}
			this.HasRunningServers = true;
		}
		if (networkConnectionError != NetworkConnectionError.NoError)
		{
			for (int j = 0; j < this.servers.Count; j++)
			{
				this.servers[j].StopServer();
			}
			this.HasRunningServers = false;
			this.CurrentMode = ProtocolManager.NetworkType.None;
			Log.Error("NET: Starting server protocols failed: " + networkConnectionError.ToStringCached<NetworkConnectionError>());
		}
		return networkConnectionError;
	}

	// Token: 0x06003F94 RID: 16276 RVA: 0x00190D43 File Offset: 0x0018EF43
	public void MakeServerOffline()
	{
		if (this.CurrentMode != ProtocolManager.NetworkType.Server)
		{
			return;
		}
		this.StopServersOnly();
		this.CurrentMode = ProtocolManager.NetworkType.OfflineServer;
	}

	// Token: 0x06003F95 RID: 16277 RVA: 0x00190D5C File Offset: 0x0018EF5C
	public void SetServerPassword(string _password)
	{
		if (this.CurrentMode != ProtocolManager.NetworkType.Server)
		{
			return;
		}
		foreach (INetworkServer networkServer in this.servers)
		{
			networkServer.SetServerPassword(_password);
		}
	}

	// Token: 0x06003F96 RID: 16278 RVA: 0x00190DB8 File Offset: 0x0018EFB8
	[PublicizedFrom(EAccessModifier.Private)]
	public void StopServersOnly()
	{
		Log.Out("NET: Stopping server protocols");
		foreach (INetworkServer networkServer in this.servers)
		{
			networkServer.StopServer();
		}
		this.HasRunningServers = false;
	}

	// Token: 0x06003F97 RID: 16279 RVA: 0x00190E1C File Offset: 0x0018F01C
	public void StopServers()
	{
		this.StopServersOnly();
		ThreadManager.StartCoroutine(this.resetStateLater(0.25f));
	}

	// Token: 0x06003F98 RID: 16280 RVA: 0x00190E35 File Offset: 0x0018F035
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator resetStateLater(float _delay)
	{
		yield return new WaitForSeconds(_delay);
		this.CurrentMode = ProtocolManager.NetworkType.None;
		yield break;
	}

	// Token: 0x06003F99 RID: 16281 RVA: 0x00190E4B File Offset: 0x0018F04B
	public void ConnectToServer(GameServerInfo _gameServerInfo)
	{
		this.SetupProtocols();
		this.CurrentMode = ProtocolManager.NetworkType.Client;
		this.currentGameServerInfo = _gameServerInfo;
		this.clients[this.currentConnectionAttemptIndex].Connect(_gameServerInfo);
	}

	// Token: 0x06003F9A RID: 16282 RVA: 0x00190E78 File Offset: 0x0018F078
	public void InvalidPasswordEv()
	{
		this.CurrentMode = ProtocolManager.NetworkType.None;
		this.currentGameServerInfo = null;
		SingletonMonoBehaviour<ConnectionManager>.Instance.Net_InvalidPassword();
	}

	// Token: 0x06003F9B RID: 16283 RVA: 0x00190E94 File Offset: 0x0018F094
	public void ConnectionFailedEv(string _msg)
	{
		this.currentConnectionAttemptIndex++;
		if (this.currentConnectionAttemptIndex < this.clients.Count)
		{
			this.clients[this.currentConnectionAttemptIndex].Connect(this.currentGameServerInfo);
			return;
		}
		this.CurrentMode = ProtocolManager.NetworkType.None;
		this.currentConnectionAttemptIndex = 0;
		this.currentGameServerInfo = null;
		SingletonMonoBehaviour<ConnectionManager>.Instance.Net_ConnectionFailed(_msg);
	}

	// Token: 0x06003F9C RID: 16284 RVA: 0x00190EFF File Offset: 0x0018F0FF
	public void DisconnectedFromServerEv(string _msg)
	{
		this.CurrentMode = ProtocolManager.NetworkType.None;
		SingletonMonoBehaviour<ConnectionManager>.Instance.Net_DisconnectedFromServer(_msg);
	}

	// Token: 0x06003F9D RID: 16285 RVA: 0x00190F14 File Offset: 0x0018F114
	public void Disconnect()
	{
		this.currentConnectionAttemptIndex = 0;
		for (int i = 0; i < this.clients.Count; i++)
		{
			this.clients[i].Disconnect();
		}
		if (this.IsClient)
		{
			this.CurrentMode = ProtocolManager.NetworkType.None;
			SingletonMonoBehaviour<ConnectionManager>.Instance.DisconnectFromServer();
		}
	}

	// Token: 0x06003F9E RID: 16286 RVA: 0x00190F68 File Offset: 0x0018F168
	public void SetLatencySimulation(bool _enable, int _min, int _max)
	{
		for (int i = 0; i < this.clients.Count; i++)
		{
			this.clients[i].SetLatencySimulation(_enable, _min, _max);
		}
		for (int j = 0; j < this.servers.Count; j++)
		{
			this.servers[j].SetLatencySimulation(_enable, _min, _max);
		}
	}

	// Token: 0x06003F9F RID: 16287 RVA: 0x00190FCC File Offset: 0x0018F1CC
	public void SetPacketLossSimulation(bool _enable, int _chance)
	{
		for (int i = 0; i < this.clients.Count; i++)
		{
			this.clients[i].SetPacketLossSimulation(_enable, _chance);
		}
		for (int j = 0; j < this.servers.Count; j++)
		{
			this.servers[j].SetPacketLossSimulation(_enable, _chance);
		}
	}

	// Token: 0x06003FA0 RID: 16288 RVA: 0x0019102C File Offset: 0x0018F22C
	public void EnableNetworkStatistics()
	{
		for (int i = 0; i < this.clients.Count; i++)
		{
			this.clients[i].EnableStatistics();
		}
		for (int j = 0; j < this.servers.Count; j++)
		{
			this.servers[j].EnableStatistics();
		}
	}

	// Token: 0x06003FA1 RID: 16289 RVA: 0x00191088 File Offset: 0x0018F288
	public void DisableNetworkStatistics()
	{
		for (int i = 0; i < this.clients.Count; i++)
		{
			this.clients[i].DisableStatistics();
		}
		for (int j = 0; j < this.servers.Count; j++)
		{
			this.servers[j].DisableStatistics();
		}
	}

	// Token: 0x06003FA2 RID: 16290 RVA: 0x001910E4 File Offset: 0x0018F2E4
	public string PrintNetworkStatistics()
	{
		string text = "";
		for (int i = 0; i < this.clients.Count; i++)
		{
			text = text + "CLIENT " + i.ToString() + "\n";
			text = text + this.clients[i].PrintNetworkStatistics() + "\n";
		}
		for (int j = 0; j < this.servers.Count; j++)
		{
			text = text + "SERVER " + j.ToString() + "\n";
			text = text + this.servers[j].PrintNetworkStatistics() + "\n";
		}
		return text;
	}

	// Token: 0x06003FA3 RID: 16291 RVA: 0x00191190 File Offset: 0x0018F390
	public void ResetNetworkStatistics()
	{
		for (int i = 0; i < this.clients.Count; i++)
		{
			this.clients[i].ResetNetworkStatistics();
		}
		for (int j = 0; j < this.servers.Count; j++)
		{
			this.servers[j].ResetNetworkStatistics();
		}
	}

	// Token: 0x04003395 RID: 13205
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<INetworkClient> clients = new List<INetworkClient>();

	// Token: 0x04003396 RID: 13206
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<INetworkServer> servers = new List<INetworkServer>();

	// Token: 0x04003399 RID: 13209
	[PublicizedFrom(EAccessModifier.Private)]
	public GameServerInfo currentGameServerInfo;

	// Token: 0x0400339A RID: 13210
	[PublicizedFrom(EAccessModifier.Private)]
	public int currentConnectionAttemptIndex;

	// Token: 0x020008A1 RID: 2209
	public enum NetworkType
	{
		// Token: 0x0400339C RID: 13212
		None,
		// Token: 0x0400339D RID: 13213
		Client,
		// Token: 0x0400339E RID: 13214
		Server,
		// Token: 0x0400339F RID: 13215
		OfflineServer
	}
}
