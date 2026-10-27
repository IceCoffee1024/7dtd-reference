using System;
using System.Collections.Generic;

namespace Platform
{
	// Token: 0x02001BA2 RID: 7074
	public class ClientLobbyManager
	{
		// Token: 0x0600D3AC RID: 54188 RVA: 0x004CD42F File Offset: 0x004CB62F
		public ClientLobbyManager()
		{
			ConnectionManager.OnClientDisconnected += this.OnClientDisconnected;
		}

		// Token: 0x0600D3AD RID: 54189 RVA: 0x004CD460 File Offset: 0x004CB660
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

		// Token: 0x0600D3AE RID: 54190 RVA: 0x004CD4BC File Offset: 0x004CB6BC
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

		// Token: 0x0600D3AF RID: 54191 RVA: 0x004CD68C File Offset: 0x004CB88C
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

		// Token: 0x0400A1CA RID: 41418
		[PublicizedFrom(EAccessModifier.Private)]
		public object lockObj = new object();

		// Token: 0x0400A1CB RID: 41419
		[PublicizedFrom(EAccessModifier.Private)]
		public Dictionary<EPlatformIdentifier, ClientLobbyManager.Lobby> lobbies = new Dictionary<EPlatformIdentifier, ClientLobbyManager.Lobby>();

		// Token: 0x02001BA3 RID: 7075
		[PublicizedFrom(EAccessModifier.Private)]
		public class Lobby
		{
			// Token: 0x17001A98 RID: 6808
			// (get) Token: 0x0600D3B0 RID: 54192 RVA: 0x004CD730 File Offset: 0x004CB930
			public PlatformLobbyId Id
			{
				get
				{
					return this.id;
				}
			}

			// Token: 0x17001A99 RID: 6809
			// (get) Token: 0x0600D3B1 RID: 54193 RVA: 0x004CD738 File Offset: 0x004CB938
			public bool IsEmpty
			{
				get
				{
					return this.clients.Count == 0;
				}
			}

			// Token: 0x17001A9A RID: 6810
			// (get) Token: 0x0600D3B2 RID: 54194 RVA: 0x004CD748 File Offset: 0x004CB948
			public IReadOnlyList<ClientInfo> Clients
			{
				get
				{
					return this.clients;
				}
			}

			// Token: 0x0600D3B3 RID: 54195 RVA: 0x004CD750 File Offset: 0x004CB950
			public Lobby(PlatformLobbyId id)
			{
				this.id = id;
			}

			// Token: 0x0600D3B4 RID: 54196 RVA: 0x004CD76A File Offset: 0x004CB96A
			public Lobby(EPlatformIdentifier platform, string lobbyId)
			{
				this.id = new PlatformLobbyId(platform, lobbyId);
			}

			// Token: 0x0600D3B5 RID: 54197 RVA: 0x004CD78C File Offset: 0x004CB98C
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

			// Token: 0x0600D3B6 RID: 54198 RVA: 0x004CD7F8 File Offset: 0x004CB9F8
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

			// Token: 0x0400A1CC RID: 41420
			[PublicizedFrom(EAccessModifier.Private)]
			public readonly PlatformLobbyId id;

			// Token: 0x0400A1CD RID: 41421
			[PublicizedFrom(EAccessModifier.Private)]
			public readonly List<ClientInfo> clients = new List<ClientInfo>();
		}
	}
}
