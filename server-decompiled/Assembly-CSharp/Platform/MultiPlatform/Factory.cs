using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace Platform.MultiPlatform
{
	// Token: 0x02001CEE RID: 7406
	[Preserve]
	public class Factory : IPlatform
	{
		// Token: 0x17001BCB RID: 7115
		// (get) Token: 0x0600DBBC RID: 56252 RVA: 0x004E773F File Offset: 0x004E593F
		// (set) Token: 0x0600DBBD RID: 56253 RVA: 0x004E7747 File Offset: 0x004E5947
		public bool AsServerOnly { get; set; }

		// Token: 0x17001BCC RID: 7116
		// (get) Token: 0x0600DBBE RID: 56254 RVA: 0x004E7750 File Offset: 0x004E5950
		// (set) Token: 0x0600DBBF RID: 56255 RVA: 0x004E7758 File Offset: 0x004E5958
		public bool IsCrossplatform { get; set; }

		// Token: 0x17001BCD RID: 7117
		// (get) Token: 0x0600DBC0 RID: 56256 RVA: 0x00010E62 File Offset: 0x0000F062
		public EPlatformIdentifier PlatformIdentifier
		{
			get
			{
				return EPlatformIdentifier.None;
			}
		}

		// Token: 0x17001BCE RID: 7118
		// (get) Token: 0x0600DBC1 RID: 56257 RVA: 0x000200DA File Offset: 0x0001E2DA
		public string PlatformDisplayName
		{
			get
			{
				return null;
			}
		}

		// Token: 0x0600DBC2 RID: 56258 RVA: 0x004E7761 File Offset: 0x004E5961
		public void Init()
		{
			this.User.Init(this);
			this.ServerListAnnouncer.Init(this);
			this.RichPresence.Init(this);
			this.UserDataRoaming.Init(this);
		}

		// Token: 0x0600DBC3 RID: 56259 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool HasNetworkingEnabled(IList<string> _disabledProtocolNames)
		{
			return false;
		}

		// Token: 0x0600DBC4 RID: 56260 RVA: 0x0008806C File Offset: 0x0008626C
		public INetworkServer GetNetworkingServer(ProtocolManager _protocolManager)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600DBC5 RID: 56261 RVA: 0x0008806C File Offset: 0x0008626C
		public INetworkClient GetNetworkingClient(ProtocolManager _protocolManager)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600DBC6 RID: 56262 RVA: 0x004E7793 File Offset: 0x004E5993
		public void UserAdded(PlatformUserIdentifierAbs _id, bool _isPrimary)
		{
			PlatformManager.NativePlatform.UserAdded(_id, _isPrimary);
			IPlatform crossplatformPlatform = PlatformManager.CrossplatformPlatform;
			if (crossplatformPlatform == null)
			{
				return;
			}
			crossplatformPlatform.UserAdded(_id, _isPrimary);
		}

		// Token: 0x0600DBC7 RID: 56263 RVA: 0x004CACEF File Offset: 0x004C8EEF
		public string[] GetArgumentsForRelaunch()
		{
			return new string[0];
		}

		// Token: 0x0600DBC8 RID: 56264 RVA: 0x004E77B2 File Offset: 0x004E59B2
		public void CreateInstances()
		{
			this.User = new User();
			this.ServerListAnnouncer = new ServerListAnnouncer();
			this.RichPresence = new RichPresence();
			this.PlayerInteractionsRecorder = new PlayerInteractionsRecorderMulti();
			this.UserDataRoaming = new UserDataRoamingMultiPlatform();
		}

		// Token: 0x0600DBC9 RID: 56265 RVA: 0x000027FC File Offset: 0x000009FC
		public void Update()
		{
		}

		// Token: 0x0600DBCA RID: 56266 RVA: 0x000027FC File Offset: 0x000009FC
		public void LateUpdate()
		{
		}

		// Token: 0x0600DBCB RID: 56267 RVA: 0x004E77EB File Offset: 0x004E59EB
		public void Destroy()
		{
			this.ServerListAnnouncer = null;
			IUserClient user = this.User;
			if (user != null)
			{
				user.Destroy();
			}
			this.User = null;
		}

		// Token: 0x17001BCF RID: 7119
		// (get) Token: 0x0600DBCC RID: 56268 RVA: 0x000200DA File Offset: 0x0001E2DA
		public IPlatformApi Api
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001BD0 RID: 7120
		// (get) Token: 0x0600DBCD RID: 56269 RVA: 0x004E780C File Offset: 0x004E5A0C
		// (set) Token: 0x0600DBCE RID: 56270 RVA: 0x004E7814 File Offset: 0x004E5A14
		public IUserClient User
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001BD1 RID: 7121
		// (get) Token: 0x0600DBCF RID: 56271 RVA: 0x004E781D File Offset: 0x004E5A1D
		// (set) Token: 0x0600DBD0 RID: 56272 RVA: 0x004E7825 File Offset: 0x004E5A25
		public IUserClient UserServer
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001BD2 RID: 7122
		// (get) Token: 0x0600DBD1 RID: 56273 RVA: 0x000200DA File Offset: 0x0001E2DA
		public IAuthenticationClient AuthenticationClient
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001BD3 RID: 7123
		// (get) Token: 0x0600DBD2 RID: 56274 RVA: 0x000200DA File Offset: 0x0001E2DA
		public IAuthenticationServer AuthenticationServer
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001BD4 RID: 7124
		// (get) Token: 0x0600DBD3 RID: 56275 RVA: 0x004E7830 File Offset: 0x004E5A30
		public IList<IServerListInterface> ServerListInterfaces
		{
			get
			{
				if (this.serverListInterfaces != null)
				{
					return this.serverListInterfaces;
				}
				this.serverListInterfaces = new List<IServerListInterface>();
				IPlatform crossplatformPlatform = PlatformManager.CrossplatformPlatform;
				IList<IServerListInterface> list = ((crossplatformPlatform != null) ? crossplatformPlatform.ServerListInterfaces : null);
				if (list != null)
				{
					this.serverListInterfaces.AddRange(list);
				}
				list = PlatformManager.NativePlatform.ServerListInterfaces;
				if (list != null)
				{
					this.serverListInterfaces.AddRange(list);
				}
				return this.serverListInterfaces;
			}
		}

		// Token: 0x17001BD5 RID: 7125
		// (get) Token: 0x0600DBD4 RID: 56276 RVA: 0x000200DA File Offset: 0x0001E2DA
		public IServerListInterface ServerLookupInterface
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001BD6 RID: 7126
		// (get) Token: 0x0600DBD5 RID: 56277 RVA: 0x004E7898 File Offset: 0x004E5A98
		// (set) Token: 0x0600DBD6 RID: 56278 RVA: 0x004E78A0 File Offset: 0x004E5AA0
		public IMasterServerAnnouncer ServerListAnnouncer
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001BD7 RID: 7127
		// (get) Token: 0x0600DBD7 RID: 56279 RVA: 0x000200DA File Offset: 0x0001E2DA
		public ILobbyHost LobbyHost
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001BD8 RID: 7128
		// (get) Token: 0x0600DBD8 RID: 56280 RVA: 0x004E78A9 File Offset: 0x004E5AA9
		// (set) Token: 0x0600DBD9 RID: 56281 RVA: 0x004E78B1 File Offset: 0x004E5AB1
		public IPlayerInteractionsRecorder PlayerInteractionsRecorder
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001BD9 RID: 7129
		// (get) Token: 0x0600DBDA RID: 56282 RVA: 0x004E78BA File Offset: 0x004E5ABA
		// (set) Token: 0x0600DBDB RID: 56283 RVA: 0x004E78C2 File Offset: 0x004E5AC2
		public IGameplayNotifier GameplayNotifier
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001BDA RID: 7130
		// (get) Token: 0x0600DBDC RID: 56284 RVA: 0x004E78CB File Offset: 0x004E5ACB
		public IList<IJoinSessionGameInviteListener> InviteListeners
		{
			get
			{
				return PlatformManager.NativePlatform.InviteListeners;
			}
		}

		// Token: 0x17001BDB RID: 7131
		// (get) Token: 0x0600DBDD RID: 56285 RVA: 0x004E78D7 File Offset: 0x004E5AD7
		public IMultiplayerInvitationDialog MultiplayerInvitationDialog
		{
			get
			{
				return PlatformManager.NativePlatform.MultiplayerInvitationDialog;
			}
		}

		// Token: 0x17001BDC RID: 7132
		// (get) Token: 0x0600DBDE RID: 56286 RVA: 0x004E78E3 File Offset: 0x004E5AE3
		public IPartyVoice PartyVoice
		{
			get
			{
				IPlatform crossplatformPlatform = PlatformManager.CrossplatformPlatform;
				return ((crossplatformPlatform != null) ? crossplatformPlatform.PartyVoice : null) ?? PlatformManager.NativePlatform.PartyVoice;
			}
		}

		// Token: 0x17001BDD RID: 7133
		// (get) Token: 0x0600DBDF RID: 56287 RVA: 0x000200DA File Offset: 0x0001E2DA
		public IUtils Utils
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001BDE RID: 7134
		// (get) Token: 0x0600DBE0 RID: 56288 RVA: 0x004E7904 File Offset: 0x004E5B04
		public IPlatformMemory Memory
		{
			get
			{
				return PlatformManager.NativePlatform.Memory;
			}
		}

		// Token: 0x17001BDF RID: 7135
		// (get) Token: 0x0600DBE1 RID: 56289 RVA: 0x004E7910 File Offset: 0x004E5B10
		public IAntiCheatClient AntiCheatClient
		{
			get
			{
				IPlatform crossplatformPlatform = PlatformManager.CrossplatformPlatform;
				return ((crossplatformPlatform != null) ? crossplatformPlatform.AntiCheatClient : null) ?? PlatformManager.NativePlatform.AntiCheatClient;
			}
		}

		// Token: 0x17001BE0 RID: 7136
		// (get) Token: 0x0600DBE2 RID: 56290 RVA: 0x004E7931 File Offset: 0x004E5B31
		public IAntiCheatServer AntiCheatServer
		{
			get
			{
				IPlatform crossplatformPlatform = PlatformManager.CrossplatformPlatform;
				return ((crossplatformPlatform != null) ? crossplatformPlatform.AntiCheatServer : null) ?? PlatformManager.NativePlatform.AntiCheatServer;
			}
		}

		// Token: 0x17001BE1 RID: 7137
		// (get) Token: 0x0600DBE3 RID: 56291 RVA: 0x000200DA File Offset: 0x0001E2DA
		public IUserIdentifierMappingService IdMappingService
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001BE2 RID: 7138
		// (get) Token: 0x0600DBE4 RID: 56292 RVA: 0x000200DA File Offset: 0x0001E2DA
		public IUserDetailsService UserDetailsService
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001BE3 RID: 7139
		// (get) Token: 0x0600DBE5 RID: 56293 RVA: 0x004E7952 File Offset: 0x004E5B52
		public IPlayerReporting PlayerReporting
		{
			get
			{
				IPlatform crossplatformPlatform = PlatformManager.CrossplatformPlatform;
				return ((crossplatformPlatform != null) ? crossplatformPlatform.PlayerReporting : null) ?? PlatformManager.NativePlatform.PlayerReporting;
			}
		}

		// Token: 0x17001BE4 RID: 7140
		// (get) Token: 0x0600DBE6 RID: 56294 RVA: 0x004E7973 File Offset: 0x004E5B73
		public ITextCensor TextCensor
		{
			get
			{
				IPlatform crossplatformPlatform = PlatformManager.CrossplatformPlatform;
				ITextCensor textCensor;
				if ((textCensor = ((crossplatformPlatform != null) ? crossplatformPlatform.TextCensor : null)) == null)
				{
					IPlatform nativePlatform = PlatformManager.NativePlatform;
					if (nativePlatform == null)
					{
						return null;
					}
					textCensor = nativePlatform.TextCensor;
				}
				return textCensor;
			}
		}

		// Token: 0x17001BE5 RID: 7141
		// (get) Token: 0x0600DBE7 RID: 56295 RVA: 0x004E799A File Offset: 0x004E5B9A
		public IRemoteFileStorage RemoteFileStorage
		{
			get
			{
				IPlatform crossplatformPlatform = PlatformManager.CrossplatformPlatform;
				return ((crossplatformPlatform != null) ? crossplatformPlatform.RemoteFileStorage : null) ?? PlatformManager.NativePlatform.RemoteFileStorage;
			}
		}

		// Token: 0x17001BE6 RID: 7142
		// (get) Token: 0x0600DBE8 RID: 56296 RVA: 0x004E79BB File Offset: 0x004E5BBB
		public IRemotePlayerFileStorage RemotePlayerFileStorage
		{
			get
			{
				IPlatform crossplatformPlatform = PlatformManager.CrossplatformPlatform;
				return ((crossplatformPlatform != null) ? crossplatformPlatform.RemotePlayerFileStorage : null) ?? PlatformManager.NativePlatform.RemotePlayerFileStorage;
			}
		}

		// Token: 0x17001BE7 RID: 7143
		// (get) Token: 0x0600DBE9 RID: 56297 RVA: 0x004E79DC File Offset: 0x004E5BDC
		public IList<IEntitlementValidator> EntitlementValidators
		{
			get
			{
				return PlatformManager.NativePlatform.EntitlementValidators;
			}
		}

		// Token: 0x17001BE8 RID: 7144
		// (get) Token: 0x0600DBEA RID: 56298 RVA: 0x000200DA File Offset: 0x0001E2DA
		public IPlatformNetworkServer NetworkServer
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001BE9 RID: 7145
		// (get) Token: 0x0600DBEB RID: 56299 RVA: 0x000200DA File Offset: 0x0001E2DA
		public PlayerInputManager Input
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001BEA RID: 7146
		// (get) Token: 0x0600DBEC RID: 56300 RVA: 0x000200DA File Offset: 0x0001E2DA
		public IVirtualKeyboard VirtualKeyboard
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001BEB RID: 7147
		// (get) Token: 0x0600DBED RID: 56301 RVA: 0x004E79E8 File Offset: 0x004E5BE8
		public IPlatformSaveGameProvider SaveGameProvider
		{
			get
			{
				return PlatformManager.NativePlatform.SaveGameProvider;
			}
		}

		// Token: 0x17001BEC RID: 7148
		// (get) Token: 0x0600DBEE RID: 56302 RVA: 0x004E79F4 File Offset: 0x004E5BF4
		// (set) Token: 0x0600DBEF RID: 56303 RVA: 0x004E79FC File Offset: 0x004E5BFC
		public IUserDataRoaming UserDataRoaming
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001BED RID: 7149
		// (get) Token: 0x0600DBF0 RID: 56304 RVA: 0x000200DA File Offset: 0x0001E2DA
		public IAchievementManager AchievementManager
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001BEE RID: 7150
		// (get) Token: 0x0600DBF1 RID: 56305 RVA: 0x004E7A05 File Offset: 0x004E5C05
		// (set) Token: 0x0600DBF2 RID: 56306 RVA: 0x004E7A0D File Offset: 0x004E5C0D
		public IRichPresence RichPresence
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001BEF RID: 7151
		// (get) Token: 0x0600DBF3 RID: 56307 RVA: 0x004E7A16 File Offset: 0x004E5C16
		public IApplicationStateController ApplicationState
		{
			get
			{
				return PlatformManager.NativePlatform.ApplicationState;
			}
		}

		// Token: 0x0400A634 RID: 42548
		[PublicizedFrom(EAccessModifier.Private)]
		public List<IServerListInterface> serverListInterfaces;
	}
}
