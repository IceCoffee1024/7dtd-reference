using System;
using System.Collections;
using System.Collections.Generic;
using Platform;
using UnityEngine;

// Token: 0x02000896 RID: 2198
public class ProtocolManager : IProtocolManagerProtocolInterface
{
	// Token: 0x170006BE RID: 1726
	// (get) Token: 0x06003F65 RID: 16229 RVA: 0x0018F7B9 File Offset: 0x0018D9B9
	// (set) Token: 0x06003F66 RID: 16230 RVA: 0x0018F7C1 File Offset: 0x0018D9C1
	public bool HasRunningServers
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170006BF RID: 1727
	// (get) Token: 0x06003F67 RID: 16231 RVA: 0x0018F7CA File Offset: 0x0018D9CA
	// (set) Token: 0x06003F68 RID: 16232 RVA: 0x0018F7D2 File Offset: 0x0018D9D2
	public ProtocolManager.NetworkType CurrentMode
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170006C0 RID: 1728
	// (get) Token: 0x06003F69 RID: 16233 RVA: 0x0018F7DB File Offset: 0x0018D9DB
	public bool IsServer
	{
		get
		{
			return this.CurrentMode == ProtocolManager.NetworkType.Server || this.CurrentMode == ProtocolManager.NetworkType.OfflineServer;
		}
	}

	// Token: 0x170006C1 RID: 1729
	// (get) Token: 0x06003F6A RID: 16234 RVA: 0x0018F7F1 File Offset: 0x0018D9F1
	public bool IsClient
	{
		get
		{
			return this.CurrentMode == ProtocolManager.NetworkType.Client;
		}
	}

	// Token: 0x06003F6B RID: 16235 RVA: 0x0018F7FC File Offset: 0x0018D9FC
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

	// Token: 0x06003F6C RID: 16236 RVA: 0x0018F9BC File Offset: 0x0018DBBC
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

	// Token: 0x06003F6D RID: 16237 RVA: 0x0018FA98 File Offset: 0x0018DC98
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

	// Token: 0x06003F6E RID: 16238 RVA: 0x0018FAF4 File Offset: 0x0018DCF4
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

	// Token: 0x06003F6F RID: 16239 RVA: 0x0018FB4F File Offset: 0x0018DD4F
	public void StartOfflineServer()
	{
		Log.Out("NET: Starting offline server.");
		this.CurrentMode = ProtocolManager.NetworkType.OfflineServer;
	}

	// Token: 0x06003F70 RID: 16240 RVA: 0x0018FB64 File Offset: 0x0018DD64
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

	// Token: 0x06003F71 RID: 16241 RVA: 0x0018FC9B File Offset: 0x0018DE9B
	public void MakeServerOffline()
	{
		if (this.CurrentMode != ProtocolManager.NetworkType.Server)
		{
			return;
		}
		this.StopServersOnly();
		this.CurrentMode = ProtocolManager.NetworkType.OfflineServer;
	}

	// Token: 0x06003F72 RID: 16242 RVA: 0x0018FCB4 File Offset: 0x0018DEB4
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

	// Token: 0x06003F73 RID: 16243 RVA: 0x0018FD10 File Offset: 0x0018DF10
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

	// Token: 0x06003F74 RID: 16244 RVA: 0x0018FD74 File Offset: 0x0018DF74
	public void StopServers()
	{
		this.StopServersOnly();
		ThreadManager.StartCoroutine(this.resetStateLater(0.25f));
	}

	// Token: 0x06003F75 RID: 16245 RVA: 0x0018FD8D File Offset: 0x0018DF8D
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator resetStateLater(float _delay)
	{
		yield return new WaitForSeconds(_delay);
		this.CurrentMode = ProtocolManager.NetworkType.None;
		yield break;
	}

	// Token: 0x06003F76 RID: 16246 RVA: 0x0018FDA3 File Offset: 0x0018DFA3
	public void ConnectToServer(GameServerInfo _gameServerInfo)
	{
		this.SetupProtocols();
		this.CurrentMode = ProtocolManager.NetworkType.Client;
		this.currentGameServerInfo = _gameServerInfo;
		this.clients[this.currentConnectionAttemptIndex].Connect(_gameServerInfo);
	}

	// Token: 0x06003F77 RID: 16247 RVA: 0x0018FDD0 File Offset: 0x0018DFD0
	public void InvalidPasswordEv()
	{
		this.CurrentMode = ProtocolManager.NetworkType.None;
		this.currentGameServerInfo = null;
		SingletonMonoBehaviour<ConnectionManager>.Instance.Net_InvalidPassword();
	}

	// Token: 0x06003F78 RID: 16248 RVA: 0x0018FDEC File Offset: 0x0018DFEC
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

	// Token: 0x06003F79 RID: 16249 RVA: 0x0018FE57 File Offset: 0x0018E057
	public void DisconnectedFromServerEv(string _msg)
	{
		this.CurrentMode = ProtocolManager.NetworkType.None;
		SingletonMonoBehaviour<ConnectionManager>.Instance.Net_DisconnectedFromServer(_msg);
	}

	// Token: 0x06003F7A RID: 16250 RVA: 0x0018FE6C File Offset: 0x0018E06C
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

	// Token: 0x06003F7B RID: 16251 RVA: 0x0018FEC0 File Offset: 0x0018E0C0
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

	// Token: 0x06003F7C RID: 16252 RVA: 0x0018FF24 File Offset: 0x0018E124
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

	// Token: 0x06003F7D RID: 16253 RVA: 0x0018FF84 File Offset: 0x0018E184
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

	// Token: 0x06003F7E RID: 16254 RVA: 0x0018FFE0 File Offset: 0x0018E1E0
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

	// Token: 0x06003F7F RID: 16255 RVA: 0x0019003C File Offset: 0x0018E23C
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

	// Token: 0x06003F80 RID: 16256 RVA: 0x001900E8 File Offset: 0x0018E2E8
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

	// Token: 0x04003375 RID: 13173
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<INetworkClient> clients = new List<INetworkClient>();

	// Token: 0x04003376 RID: 13174
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<INetworkServer> servers = new List<INetworkServer>();

	// Token: 0x04003379 RID: 13177
	[PublicizedFrom(EAccessModifier.Private)]
	public GameServerInfo currentGameServerInfo;

	// Token: 0x0400337A RID: 13178
	[PublicizedFrom(EAccessModifier.Private)]
	public int currentConnectionAttemptIndex;

	// Token: 0x02000897 RID: 2199
	public enum NetworkType
	{
		// Token: 0x0400337C RID: 13180
		None,
		// Token: 0x0400337D RID: 13181
		Client,
		// Token: 0x0400337E RID: 13182
		Server,
		// Token: 0x0400337F RID: 13183
		OfflineServer
	}
}
