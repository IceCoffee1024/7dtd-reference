using System;
using System.Collections;
using System.Collections.Generic;
using Platform;
using UnityEngine;

// Token: 0x02000893 RID: 2195
public class ProtocolManager : IProtocolManagerProtocolInterface
{
	// Token: 0x1700069F RID: 1695
	// (get) Token: 0x06003FD7 RID: 16343 RVA: 0x0018F9C9 File Offset: 0x0018DBC9
	// (set) Token: 0x06003FD8 RID: 16344 RVA: 0x0018F9D1 File Offset: 0x0018DBD1
	public bool HasRunningServers
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170006A0 RID: 1696
	// (get) Token: 0x06003FD9 RID: 16345 RVA: 0x0018F9DA File Offset: 0x0018DBDA
	// (set) Token: 0x06003FDA RID: 16346 RVA: 0x0018F9E2 File Offset: 0x0018DBE2
	public ProtocolManager.NetworkType CurrentMode
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170006A1 RID: 1697
	// (get) Token: 0x06003FDB RID: 16347 RVA: 0x0018F9EB File Offset: 0x0018DBEB
	public bool IsServer
	{
		get
		{
			return this.CurrentMode == ProtocolManager.NetworkType.Server || this.CurrentMode == ProtocolManager.NetworkType.OfflineServer;
		}
	}

	// Token: 0x170006A2 RID: 1698
	// (get) Token: 0x06003FDC RID: 16348 RVA: 0x0018FA01 File Offset: 0x0018DC01
	public bool IsClient
	{
		get
		{
			return this.CurrentMode == ProtocolManager.NetworkType.Client;
		}
	}

	// Token: 0x06003FDD RID: 16349 RVA: 0x0018FA0C File Offset: 0x0018DC0C
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

	// Token: 0x06003FDE RID: 16350 RVA: 0x0018FBCC File Offset: 0x0018DDCC
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

	// Token: 0x06003FDF RID: 16351 RVA: 0x0018FCA8 File Offset: 0x0018DEA8
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

	// Token: 0x06003FE0 RID: 16352 RVA: 0x0018FD04 File Offset: 0x0018DF04
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

	// Token: 0x06003FE1 RID: 16353 RVA: 0x0018FD5F File Offset: 0x0018DF5F
	public void StartOfflineServer()
	{
		Log.Out("NET: Starting offline server.");
		this.CurrentMode = ProtocolManager.NetworkType.OfflineServer;
	}

	// Token: 0x06003FE2 RID: 16354 RVA: 0x0018FD74 File Offset: 0x0018DF74
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

	// Token: 0x06003FE3 RID: 16355 RVA: 0x0018FEAB File Offset: 0x0018E0AB
	public void MakeServerOffline()
	{
		if (this.CurrentMode != ProtocolManager.NetworkType.Server)
		{
			return;
		}
		this.StopServersOnly();
		this.CurrentMode = ProtocolManager.NetworkType.OfflineServer;
	}

	// Token: 0x06003FE4 RID: 16356 RVA: 0x0018FEC4 File Offset: 0x0018E0C4
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

	// Token: 0x06003FE5 RID: 16357 RVA: 0x0018FF20 File Offset: 0x0018E120
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

	// Token: 0x06003FE6 RID: 16358 RVA: 0x0018FF84 File Offset: 0x0018E184
	public void StopServers()
	{
		this.StopServersOnly();
		ThreadManager.StartCoroutine(this.resetStateLater(0.25f));
	}

	// Token: 0x06003FE7 RID: 16359 RVA: 0x0018FF9D File Offset: 0x0018E19D
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator resetStateLater(float _delay)
	{
		yield return new WaitForSeconds(_delay);
		this.CurrentMode = ProtocolManager.NetworkType.None;
		yield break;
	}

	// Token: 0x06003FE8 RID: 16360 RVA: 0x0018FFB3 File Offset: 0x0018E1B3
	public void ConnectToServer(GameServerInfo _gameServerInfo)
	{
		this.SetupProtocols();
		this.CurrentMode = ProtocolManager.NetworkType.Client;
		this.currentGameServerInfo = _gameServerInfo;
		this.clients[this.currentConnectionAttemptIndex].Connect(_gameServerInfo);
	}

	// Token: 0x06003FE9 RID: 16361 RVA: 0x0018FFE0 File Offset: 0x0018E1E0
	public void InvalidPasswordEv()
	{
		this.CurrentMode = ProtocolManager.NetworkType.None;
		this.currentGameServerInfo = null;
		SingletonMonoBehaviour<ConnectionManager>.Instance.Net_InvalidPassword();
	}

	// Token: 0x06003FEA RID: 16362 RVA: 0x0018FFFC File Offset: 0x0018E1FC
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

	// Token: 0x06003FEB RID: 16363 RVA: 0x00190067 File Offset: 0x0018E267
	public void DisconnectedFromServerEv(string _msg)
	{
		this.CurrentMode = ProtocolManager.NetworkType.None;
		SingletonMonoBehaviour<ConnectionManager>.Instance.Net_DisconnectedFromServer(_msg);
	}

	// Token: 0x06003FEC RID: 16364 RVA: 0x0019007C File Offset: 0x0018E27C
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

	// Token: 0x06003FED RID: 16365 RVA: 0x001900D0 File Offset: 0x0018E2D0
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

	// Token: 0x06003FEE RID: 16366 RVA: 0x00190134 File Offset: 0x0018E334
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

	// Token: 0x06003FEF RID: 16367 RVA: 0x00190194 File Offset: 0x0018E394
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

	// Token: 0x06003FF0 RID: 16368 RVA: 0x001901F0 File Offset: 0x0018E3F0
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

	// Token: 0x06003FF1 RID: 16369 RVA: 0x0019024C File Offset: 0x0018E44C
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

	// Token: 0x06003FF2 RID: 16370 RVA: 0x001902F8 File Offset: 0x0018E4F8
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

	// Token: 0x0400338A RID: 13194
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<INetworkClient> clients = new List<INetworkClient>();

	// Token: 0x0400338B RID: 13195
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<INetworkServer> servers = new List<INetworkServer>();

	// Token: 0x0400338E RID: 13198
	[PublicizedFrom(EAccessModifier.Private)]
	public GameServerInfo currentGameServerInfo;

	// Token: 0x0400338F RID: 13199
	[PublicizedFrom(EAccessModifier.Private)]
	public int currentConnectionAttemptIndex;

	// Token: 0x02000894 RID: 2196
	public enum NetworkType
	{
		// Token: 0x04003391 RID: 13201
		None,
		// Token: 0x04003392 RID: 13202
		Client,
		// Token: 0x04003393 RID: 13203
		Server,
		// Token: 0x04003394 RID: 13204
		OfflineServer
	}
}
