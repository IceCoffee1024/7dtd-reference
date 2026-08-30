using System;
using System.Collections.Generic;

namespace Platform
{
	// Token: 0x02001B3F RID: 6975
	public class ClientLobbyManager
	{
		// Token: 0x0600D121 RID: 53537 RVA: 0x004C4E1F File Offset: 0x004C301F
		public ClientLobbyManager()
		{
			ConnectionManager.OnClientDisconnected += this.OnClientDisconnected;
		}

		// Token: 0x0600D122 RID: 53538 RVA: 0x004C4E50 File Offset: 0x004C3050
		public bool TryGetLobbyId(EPlatformIdentifier platform, out PlatformLobbyId lobbyId)
		{
			object obj = this.lockObj;
			bool flag2;
			lock (obj)
			{
				ClientLobbyManager.Lobby lobby;
				if (this.lobbies.TryGetValue(platform, out lobby))
				{
					lobbyId = lobby.Id;
					flag2 = true;
				}
				else
				{
					lobbyId = null;
					flag2 = false;
				}
			}
			return flag2;
		}

		// Token: 0x0600D123 RID: 53539 RVA: 0x004C4EAC File Offset: 0x004C30AC
		public void RegisterLobbyClient(PlatformLobbyId platformLobbyId, ClientInfo client, bool overwrite = false)
		{
			if (!SingletonMonoBehaviour<ConnectionManager>.Instance.Clients.Contains(client))
			{
				Log.Warning(string.Format("[ClientLobbyManager] could not register {0} for client lobby {1} : {2} as they are no longer connected", client.playerName, platformLobbyId.PlatformIdentifier, platformLobbyId.LobbyId));
				return;
			}
			object obj = this.lockObj;
			lock (obj)
			{
				ClientLobbyManager.Lobby lobby;
				if (!this.lobbies.TryGetValue(platformLobbyId.PlatformIdentifier, out lobby))
				{
					Log.Out(string.Format("[ClientLobbyManager] registering new lobby for client platform {0} : {1}", platformLobbyId.PlatformIdentifier, platformLobbyId.LobbyId));
					lobby = new ClientLobbyManager.Lobby(platformLobbyId);
					lobby.AddClient(client);
					this.lobbies.Add(platformLobbyId.PlatformIdentifier, lobby);
				}
				else if (lobby.Id.LobbyId.Equals(platformLobbyId.LobbyId))
				{
					lobby.AddClient(client);
				}
				else if (overwrite)
				{
					Log.Warning(string.Format("[ClientLobbyManager] overwriting existing lobby for {0}", platformLobbyId.PlatformIdentifier));
					ClientLobbyManager.Lobby lobby2 = new ClientLobbyManager.Lobby(platformLobbyId);
					lobby2.AddClient(client);
					foreach (ClientInfo clientInfo in lobby.Clients)
					{
						clientInfo.SendPackage(NetPackageManager.GetPackage<NetPackageLobbyJoin>().Setup(platformLobbyId));
						lobby2.AddClient(clientInfo);
					}
					this.lobbies[platformLobbyId.PlatformIdentifier] = lobby2;
				}
				else
				{
					Log.Warning(string.Format("[ClientLobbyManager] a different client lobby already registered for {0}, sending to client", platformLobbyId.PlatformIdentifier));
					client.SendPackage(NetPackageManager.GetPackage<NetPackageLobbyJoin>().Setup(lobby.Id));
				}
			}
		}

		// Token: 0x0600D124 RID: 53540 RVA: 0x004C507C File Offset: 0x004C327C
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnClientDisconnected(ClientInfo client)
		{
			object obj = this.lockObj;
			lock (obj)
			{
				ClientLobbyManager.Lobby lobby;
				if (this.lobbies.TryGetValue(client.PlatformId.PlatformIdentifier, out lobby))
				{
					lobby.RemoveClient(client);
					if (lobby.IsEmpty)
					{
						Log.Out(string.Format("[ClientLobbyManager] removing registered lobby {0} : {1}", lobby.Id.PlatformIdentifier, lobby.Id.LobbyId));
						this.lobbies.Remove(client.PlatformId.PlatformIdentifier);
					}
				}
			}
		}

		// Token: 0x0400A00C RID: 40972
		[PublicizedFrom(EAccessModifier.Private)]
		public object lockObj = new object();

		// Token: 0x0400A00D RID: 40973
		[PublicizedFrom(EAccessModifier.Private)]
		public Dictionary<EPlatformIdentifier, ClientLobbyManager.Lobby> lobbies = new Dictionary<EPlatformIdentifier, ClientLobbyManager.Lobby>();

		// Token: 0x02001B40 RID: 6976
		[PublicizedFrom(EAccessModifier.Private)]
		public class Lobby
		{
			// Token: 0x170019AA RID: 6570
			// (get) Token: 0x0600D125 RID: 53541 RVA: 0x004C5120 File Offset: 0x004C3320
			public PlatformLobbyId Id
			{
				get
				{
					return this.id;
				}
			}

			// Token: 0x170019AB RID: 6571
			// (get) Token: 0x0600D126 RID: 53542 RVA: 0x004C5128 File Offset: 0x004C3328
			public bool IsEmpty
			{
				get
				{
					return this.clients.Count == 0;
				}
			}

			// Token: 0x170019AC RID: 6572
			// (get) Token: 0x0600D127 RID: 53543 RVA: 0x004C5138 File Offset: 0x004C3338
			public IReadOnlyList<ClientInfo> Clients
			{
				get
				{
					return this.clients;
				}
			}

			// Token: 0x0600D128 RID: 53544 RVA: 0x004C5140 File Offset: 0x004C3340
			public Lobby(PlatformLobbyId id)
			{
				this.id = id;
			}

			// Token: 0x0600D129 RID: 53545 RVA: 0x004C515A File Offset: 0x004C335A
			public Lobby(EPlatformIdentifier platform, string lobbyId)
			{
				this.id = new PlatformLobbyId(platform, lobbyId);
			}

			// Token: 0x0600D12A RID: 53546 RVA: 0x004C517C File Offset: 0x004C337C
			public void AddClient(ClientInfo client)
			{
				this.clients.Add(client);
				Log.Out(string.Format("[ClientLobbyManager] registered member {0} for client lobby {1} : {2}. Total members: {3}", new object[]
				{
					client.playerName,
					this.id.PlatformIdentifier,
					this.id.LobbyId,
					this.clients.Count
				}));
			}

			// Token: 0x0600D12B RID: 53547 RVA: 0x004C51E8 File Offset: 0x004C33E8
			public void RemoveClient(ClientInfo client)
			{
				if (this.clients.Remove(client))
				{
					Log.Out(string.Format("[ClientLobbyManager] removed member {0} from client lobby {1} : {2}. Total members: {3}", new object[]
					{
						client.playerName,
						this.id.PlatformIdentifier,
						this.id.LobbyId,
						this.clients.Count
					}));
					return;
				}
				Log.Warning(string.Format("[ClientLobbyManager] remove member {0} from client lobby {1} : {2} failed. They are not a member", client.playerName, this.id.PlatformIdentifier, this.id.LobbyId));
			}

			// Token: 0x0400A00E RID: 40974
			[PublicizedFrom(EAccessModifier.Private)]
			public readonly PlatformLobbyId id;

			// Token: 0x0400A00F RID: 40975
			[PublicizedFrom(EAccessModifier.Private)]
			public readonly List<ClientInfo> clients = new List<ClientInfo>();
		}
	}
}
