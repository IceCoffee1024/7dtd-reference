using System;
using System.Collections;
using System.Collections.Generic;
using Platform;
using UnityEngine;

// Token: 0x02000887 RID: 2183
public class ProtocolManager : IProtocolManagerProtocolInterface
{
	// Token: 0x17000693 RID: 1683
	// (get) Token: 0x06003F72 RID: 16242 RVA: 0x0018D8F9 File Offset: 0x0018BAF9
	// (set) Token: 0x06003F73 RID: 16243 RVA: 0x0018D901 File Offset: 0x0018BB01
	public bool HasRunningServers
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000694 RID: 1684
	// (get) Token: 0x06003F74 RID: 16244 RVA: 0x0018D90A File Offset: 0x0018BB0A
	// (set) Token: 0x06003F75 RID: 16245 RVA: 0x0018D912 File Offset: 0x0018BB12
	public ProtocolManager.NetworkType CurrentMode
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000695 RID: 1685
	// (get) Token: 0x06003F76 RID: 16246 RVA: 0x0018D91B File Offset: 0x0018BB1B
	public bool IsServer
	{
		get
		{
			return this.CurrentMode == ProtocolManager.NetworkType.Server || this.CurrentMode == ProtocolManager.NetworkType.OfflineServer;
		}
	}

	// Token: 0x17000696 RID: 1686
	// (get) Token: 0x06003F77 RID: 16247 RVA: 0x0018D931 File Offset: 0x0018BB31
	public bool IsClient
	{
		get
		{
			return this.CurrentMode == ProtocolManager.NetworkType.Client;
		}
	}

	// Token: 0x06003F78 RID: 16248 RVA: 0x0018D93C File Offset: 0x0018BB3C
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

	// Token: 0x06003F79 RID: 16249 RVA: 0x0018DAFC File Offset: 0x0018BCFC
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

	// Token: 0x06003F7A RID: 16250 RVA: 0x0018DBD8 File Offset: 0x0018BDD8
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

	// Token: 0x06003F7B RID: 16251 RVA: 0x0018DC34 File Offset: 0x0018BE34
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

	// Token: 0x06003F7C RID: 16252 RVA: 0x0018DC8F File Offset: 0x0018BE8F
	public void StartOfflineServer()
	{
		Log.Out("NET: Starting offline server.");
		this.CurrentMode = ProtocolManager.NetworkType.OfflineServer;
	}

	// Token: 0x06003F7D RID: 16253 RVA: 0x0018DCA4 File Offset: 0x0018BEA4
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

	// Token: 0x06003F7E RID: 16254 RVA: 0x0018DDDB File Offset: 0x0018BFDB
	public void MakeServerOffline()
	{
		if (this.CurrentMode != ProtocolManager.NetworkType.Server)
		{
			return;
		}
		this.StopServersOnly();
		this.CurrentMode = ProtocolManager.NetworkType.OfflineServer;
	}

	// Token: 0x06003F7F RID: 16255 RVA: 0x0018DDF4 File Offset: 0x0018BFF4
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

	// Token: 0x06003F80 RID: 16256 RVA: 0x0018DE50 File Offset: 0x0018C050
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

	// Token: 0x06003F81 RID: 16257 RVA: 0x0018DEB4 File Offset: 0x0018C0B4
	public void StopServers()
	{
		this.StopServersOnly();
		ThreadManager.StartCoroutine(this.resetStateLater(0.25f));
	}

	// Token: 0x06003F82 RID: 16258 RVA: 0x0018DECD File Offset: 0x0018C0CD
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator resetStateLater(float _delay)
	{
		yield return new WaitForSeconds(_delay);
		this.CurrentMode = ProtocolManager.NetworkType.None;
		yield break;
	}

	// Token: 0x06003F83 RID: 16259 RVA: 0x0018DEE3 File Offset: 0x0018C0E3
	public void ConnectToServer(GameServerInfo _gameServerInfo)
	{
		this.SetupProtocols();
		this.CurrentMode = ProtocolManager.NetworkType.Client;
		this.currentGameServerInfo = _gameServerInfo;
		this.clients[this.currentConnectionAttemptIndex].Connect(_gameServerInfo);
	}

	// Token: 0x06003F84 RID: 16260 RVA: 0x0018DF10 File Offset: 0x0018C110
	public void InvalidPasswordEv()
	{
		this.CurrentMode = ProtocolManager.NetworkType.None;
		this.currentGameServerInfo = null;
		SingletonMonoBehaviour<ConnectionManager>.Instance.Net_InvalidPassword();
	}

	// Token: 0x06003F85 RID: 16261 RVA: 0x0018DF2C File Offset: 0x0018C12C
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

	// Token: 0x06003F86 RID: 16262 RVA: 0x0018DF97 File Offset: 0x0018C197
	public void DisconnectedFromServerEv(string _msg)
	{
		this.CurrentMode = ProtocolManager.NetworkType.None;
		SingletonMonoBehaviour<ConnectionManager>.Instance.Net_DisconnectedFromServer(_msg);
	}

	// Token: 0x06003F87 RID: 16263 RVA: 0x0018DFAC File Offset: 0x0018C1AC
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

	// Token: 0x06003F88 RID: 16264 RVA: 0x0018E000 File Offset: 0x0018C200
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

	// Token: 0x06003F89 RID: 16265 RVA: 0x0018E064 File Offset: 0x0018C264
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

	// Token: 0x06003F8A RID: 16266 RVA: 0x0018E0C4 File Offset: 0x0018C2C4
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

	// Token: 0x06003F8B RID: 16267 RVA: 0x0018E120 File Offset: 0x0018C320
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

	// Token: 0x06003F8C RID: 16268 RVA: 0x0018E17C File Offset: 0x0018C37C
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

	// Token: 0x06003F8D RID: 16269 RVA: 0x0018E228 File Offset: 0x0018C428
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

	// Token: 0x04003309 RID: 13065
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<INetworkClient> clients = new List<INetworkClient>();

	// Token: 0x0400330A RID: 13066
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<INetworkServer> servers = new List<INetworkServer>();

	// Token: 0x0400330D RID: 13069
	[PublicizedFrom(EAccessModifier.Private)]
	public GameServerInfo currentGameServerInfo;

	// Token: 0x0400330E RID: 13070
	[PublicizedFrom(EAccessModifier.Private)]
	public int currentConnectionAttemptIndex;

	// Token: 0x02000888 RID: 2184
	public enum NetworkType
	{
		// Token: 0x04003310 RID: 13072
		None,
		// Token: 0x04003311 RID: 13073
		Client,
		// Token: 0x04003312 RID: 13074
		Server,
		// Token: 0x04003313 RID: 13075
		OfflineServer
	}
}
