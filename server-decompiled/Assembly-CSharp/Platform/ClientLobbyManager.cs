using System;
using System.Collections.Generic;

namespace Platform
{
	// Token: 0x02001B71 RID: 7025
	public class ClientLobbyManager
	{
		// Token: 0x0600D298 RID: 53912 RVA: 0x004C9907 File Offset: 0x004C7B07
		public ClientLobbyManager()
		{
			ConnectionManager.OnClientDisconnected += this.OnClientDisconnected;
		}

		// Token: 0x0600D299 RID: 53913 RVA: 0x004C9938 File Offset: 0x004C7B38
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

		// Token: 0x0600D29A RID: 53914 RVA: 0x004C9994 File Offset: 0x004C7B94
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

		// Token: 0x0600D29B RID: 53915 RVA: 0x004C9B64 File Offset: 0x004C7D64
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

		// Token: 0x0400A0E1 RID: 41185
		[PublicizedFrom(EAccessModifier.Private)]
		public object lockObj = new object();

		// Token: 0x0400A0E2 RID: 41186
		[PublicizedFrom(EAccessModifier.Private)]
		public Dictionary<EPlatformIdentifier, ClientLobbyManager.Lobby> lobbies = new Dictionary<EPlatformIdentifier, ClientLobbyManager.Lobby>();

		// Token: 0x02001B72 RID: 7026
		[PublicizedFrom(EAccessModifier.Private)]
		public class Lobby
		{
			// Token: 0x17001A50 RID: 6736
			// (get) Token: 0x0600D29C RID: 53916 RVA: 0x004C9C08 File Offset: 0x004C7E08
			public PlatformLobbyId Id
			{
				get
				{
					return this.id;
				}
			}

			// Token: 0x17001A51 RID: 6737
			// (get) Token: 0x0600D29D RID: 53917 RVA: 0x004C9C10 File Offset: 0x004C7E10
			public bool IsEmpty
			{
				get
				{
					return this.clients.Count == 0;
				}
			}

			// Token: 0x17001A52 RID: 6738
			// (get) Token: 0x0600D29E RID: 53918 RVA: 0x004C9C20 File Offset: 0x004C7E20
			public IReadOnlyList<ClientInfo> Clients
			{
				get
				{
					return this.clients;
				}
			}

			// Token: 0x0600D29F RID: 53919 RVA: 0x004C9C28 File Offset: 0x004C7E28
			public Lobby(PlatformLobbyId id)
			{
				this.id = id;
			}

			// Token: 0x0600D2A0 RID: 53920 RVA: 0x004C9C42 File Offset: 0x004C7E42
			public Lobby(EPlatformIdentifier platform, string lobbyId)
			{
				this.id = new PlatformLobbyId(platform, lobbyId);
			}

			// Token: 0x0600D2A1 RID: 53921 RVA: 0x004C9C64 File Offset: 0x004C7E64
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

			// Token: 0x0600D2A2 RID: 53922 RVA: 0x004C9CD0 File Offset: 0x004C7ED0
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

			// Token: 0x0400A0E3 RID: 41187
			[PublicizedFrom(EAccessModifier.Private)]
			public readonly PlatformLobbyId id;

			// Token: 0x0400A0E4 RID: 41188
			[PublicizedFrom(EAccessModifier.Private)]
			public readonly List<ClientInfo> clients = new List<ClientInfo>();
		}
	}
}
