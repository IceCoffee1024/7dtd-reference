using System;
using System.Collections.Generic;
using InControl;

namespace Platform
{
	// Token: 0x02001B29 RID: 6953
	public abstract class AbsPlatform : IPlatform
	{
		// Token: 0x0600CFC3 RID: 53187 RVA: 0x004BF9B8 File Offset: 0x004BDBB8
		[PublicizedFrom(EAccessModifier.Protected)]
		public AbsPlatform()
		{
			Type typeFromHandle = typeof(PlatformFactoryAttribute);
			object[] customAttributes = base.GetType().GetCustomAttributes(typeFromHandle, false);
			if (customAttributes.Length != 1)
			{
				throw new Exception("Platform has no PlatformFactory attribute");
			}
			PlatformFactoryAttribute platformFactoryAttribute = (PlatformFactoryAttribute)customAttributes[0];
			this.PlatformIdentifier = platformFactoryAttribute.TargetPlatform;
		}

		// Token: 0x1700193E RID: 6462
		// (get) Token: 0x0600CFC4 RID: 53188 RVA: 0x004BFA08 File Offset: 0x004BDC08
		// (set) Token: 0x0600CFC5 RID: 53189 RVA: 0x004BFA10 File Offset: 0x004BDC10
		public bool AsServerOnly { get; set; }

		// Token: 0x1700193F RID: 6463
		// (get) Token: 0x0600CFC6 RID: 53190 RVA: 0x004BFA19 File Offset: 0x004BDC19
		// (set) Token: 0x0600CFC7 RID: 53191 RVA: 0x004BFA21 File Offset: 0x004BDC21
		public bool IsCrossplatform { get; set; }

		// Token: 0x17001940 RID: 6464
		// (get) Token: 0x0600CFC8 RID: 53192 RVA: 0x004BFA2A File Offset: 0x004BDC2A
		public string PlatformDisplayName
		{
			get
			{
				return PlatformManager.GetPlatformDisplayName(this.PlatformIdentifier);
			}
		}

		// Token: 0x0600CFC9 RID: 53193 RVA: 0x004BFA38 File Offset: 0x004BDC38
		public virtual void Init()
		{
			Log.Out("[Platform] Initializing " + this.PlatformIdentifier.ToString());
			IPlatformApi api = this.Api;
			if (api != null)
			{
				api.Init(this);
			}
			IUserClient user = this.User;
			if (user != null)
			{
				user.Init(this);
			}
			IUserClient userServer = this.UserServer;
			if (userServer != null)
			{
				userServer.Init(this);
			}
			IAuthenticationClient authenticationClient = this.AuthenticationClient;
			if (authenticationClient != null)
			{
				authenticationClient.Init(this);
			}
			IAuthenticationServer authenticationServer = this.AuthenticationServer;
			if (authenticationServer != null)
			{
				authenticationServer.Init(this);
			}
			if (this.ServerListInterfaces != null)
			{
				foreach (IServerListInterface serverListInterface in this.ServerListInterfaces)
				{
					serverListInterface.Init(this);
				}
			}
			IMasterServerAnnouncer serverListAnnouncer = this.ServerListAnnouncer;
			if (serverListAnnouncer != null)
			{
				serverListAnnouncer.Init(this);
			}
			if (this.InviteListeners != null)
			{
				foreach (IJoinSessionGameInviteListener joinSessionGameInviteListener in this.InviteListeners)
				{
					if (joinSessionGameInviteListener != null)
					{
						joinSessionGameInviteListener.Init(this);
					}
				}
			}
			IMultiplayerInvitationDialog multiplayerInvitationDialog = this.MultiplayerInvitationDialog;
			if (multiplayerInvitationDialog != null)
			{
				multiplayerInvitationDialog.Init(this);
			}
			ILobbyHost lobbyHost = this.LobbyHost;
			if (lobbyHost != null)
			{
				lobbyHost.Init(this);
			}
			IPlayerInteractionsRecorder playerInteractionsRecorder = this.PlayerInteractionsRecorder;
			if (playerInteractionsRecorder != null)
			{
				playerInteractionsRecorder.Init(this);
			}
			IGameplayNotifier gameplayNotifier = this.GameplayNotifier;
			if (gameplayNotifier != null)
			{
				gameplayNotifier.Init(this);
			}
			IPartyVoice partyVoice = this.PartyVoice;
			if (partyVoice != null)
			{
				partyVoice.Init(this);
			}
			IUtils utils = this.Utils;
			if (utils != null)
			{
				utils.Init(this);
			}
			IUtils utils2 = this.Utils;
			if (utils2 != null)
			{
				utils2.ClearTempFiles();
			}
			if (this.Utils != null)
			{
				InputManager.OnDeviceDetached += this.Utils.ControllerDisconnected;
			}
			IAntiCheatClient antiCheatClient = this.AntiCheatClient;
			if (antiCheatClient != null)
			{
				antiCheatClient.Init(this);
			}
			IAntiCheatServer antiCheatServer = this.AntiCheatServer;
			if (antiCheatServer != null)
			{
				antiCheatServer.Init(this);
			}
			IPlayerReporting playerReporting = this.PlayerReporting;
			if (playerReporting != null)
			{
				playerReporting.Init(this);
			}
			ITextCensor textCensor = this.TextCensor;
			if (textCensor != null)
			{
				textCensor.Init(this);
			}
			IVirtualKeyboard virtualKeyboard = this.VirtualKeyboard;
			if (virtualKeyboard != null)
			{
				virtualKeyboard.Init(this);
			}
			IPlatformSaveGameProvider saveGameProvider = this.SaveGameProvider;
			if (saveGameProvider != null)
			{
				saveGameProvider.Init(this);
			}
			IUserDataRoaming userDataRoaming = this.UserDataRoaming;
			if (userDataRoaming != null)
			{
				userDataRoaming.Init(this);
			}
			IAchievementManager achievementManager = this.AchievementManager;
			if (achievementManager != null)
			{
				achievementManager.Init(this);
			}
			IRichPresence richPresence = this.RichPresence;
			if (richPresence != null)
			{
				richPresence.Init(this);
			}
			IRemoteFileStorage remoteFileStorage = this.RemoteFileStorage;
			if (remoteFileStorage != null)
			{
				remoteFileStorage.Init(this);
			}
			IRemotePlayerFileStorage remotePlayerFileStorage = this.RemotePlayerFileStorage;
			if (remotePlayerFileStorage != null)
			{
				remotePlayerFileStorage.Init(this);
			}
			if (this.EntitlementValidators != null)
			{
				foreach (IEntitlementValidator entitlementValidator in this.EntitlementValidators)
				{
					if (entitlementValidator != null)
					{
						entitlementValidator.Init(this);
					}
				}
			}
			IApplicationStateController applicationState = this.ApplicationState;
			if (applicationState != null)
			{
				applicationState.Init(this);
			}
			IUserDetailsService userDetailsService = this.UserDetailsService;
			if (userDetailsService != null)
			{
				userDetailsService.Init(this);
			}
			if (!GameManager.IsDedicatedServer && !this.AsServerOnly)
			{
				IPlatformApi api2 = this.Api;
				if (api2 != null)
				{
					api2.InitClientApis();
				}
			}
			if (GameManager.IsDedicatedServer)
			{
				IPlatformApi api3 = this.Api;
				if (api3 == null)
				{
					return;
				}
				api3.InitServerApis();
			}
		}

		// Token: 0x0600CFCA RID: 53194 RVA: 0x004BFD6C File Offset: 0x004BDF6C
		public virtual bool HasNetworkingEnabled(IList<string> _disabledProtocolNames)
		{
			string networkProtocolName = this.NetworkProtocolName;
			if (string.IsNullOrEmpty(networkProtocolName))
			{
				return false;
			}
			string text = networkProtocolName.ToLowerInvariant();
			bool flag = GameUtils.GetLaunchArgument("no" + text) == null && !_disabledProtocolNames.Contains(text);
			if (!flag)
			{
				Log.Out("[NET] Disabling protocol: " + networkProtocolName);
			}
			return flag;
		}

		// Token: 0x0600CFCB RID: 53195 RVA: 0x0001FF1E File Offset: 0x0001E11E
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual IPlatformNetworkServer instantiateNetworkServer(ProtocolManager _protocolManager)
		{
			return null;
		}

		// Token: 0x0600CFCC RID: 53196 RVA: 0x0001FF1E File Offset: 0x0001E11E
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual IPlatformNetworkClient instantiateNetworkClient(ProtocolManager _protocolManager)
		{
			return null;
		}

		// Token: 0x0600CFCD RID: 53197 RVA: 0x004BFDC4 File Offset: 0x004BDFC4
		public INetworkServer GetNetworkingServer(ProtocolManager _protocolManager)
		{
			IPlatformNetworkServer platformNetworkServer;
			if ((platformNetworkServer = this.NetworkServer) == null)
			{
				platformNetworkServer = (this.NetworkServer = this.instantiateNetworkServer(_protocolManager));
			}
			return platformNetworkServer;
		}

		// Token: 0x0600CFCE RID: 53198 RVA: 0x004BFDEC File Offset: 0x004BDFEC
		public INetworkClient GetNetworkingClient(ProtocolManager _protocolManager)
		{
			IPlatformNetworkClient platformNetworkClient;
			if ((platformNetworkClient = this.NetworkClient) == null)
			{
				platformNetworkClient = (this.NetworkClient = this.instantiateNetworkClient(_protocolManager));
			}
			return platformNetworkClient;
		}

		// Token: 0x0600CFCF RID: 53199 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void UserAdded(PlatformUserIdentifierAbs _id, bool _isPrimary)
		{
		}

		// Token: 0x0600CFD0 RID: 53200 RVA: 0x004BFE13 File Offset: 0x004BE013
		public virtual string[] GetArgumentsForRelaunch()
		{
			return new string[0];
		}

		// Token: 0x0600CFD1 RID: 53201
		public abstract void CreateInstances();

		// Token: 0x0600CFD2 RID: 53202 RVA: 0x004BFE1C File Offset: 0x004BE01C
		public virtual void Update()
		{
			IPlatformApi api = this.Api;
			if (api != null)
			{
				api.Update();
			}
			IMasterServerAnnouncer serverListAnnouncer = this.ServerListAnnouncer;
			if (serverListAnnouncer != null)
			{
				serverListAnnouncer.Update();
			}
			IAntiCheatServer antiCheatServer = this.AntiCheatServer;
			if (antiCheatServer != null)
			{
				antiCheatServer.Update();
			}
			PlayerInputManager input = this.Input;
			if (input != null)
			{
				input.Update();
			}
			ITextCensor textCensor = this.TextCensor;
			if (textCensor != null)
			{
				textCensor.Update();
			}
			IApplicationStateController applicationState = this.ApplicationState;
			if (applicationState == null)
			{
				return;
			}
			applicationState.Update();
		}

		// Token: 0x0600CFD3 RID: 53203 RVA: 0x000027FC File Offset: 0x000009FC
		public void LateUpdate()
		{
		}

		// Token: 0x0600CFD4 RID: 53204 RVA: 0x004BFE90 File Offset: 0x004BE090
		public virtual void Destroy()
		{
			this.RichPresence = null;
			IAchievementManager achievementManager = this.AchievementManager;
			if (achievementManager != null)
			{
				achievementManager.Destroy();
			}
			this.AchievementManager = null;
			this.Input = null;
			IPlatformSaveGameProvider saveGameProvider = this.SaveGameProvider;
			if (saveGameProvider != null)
			{
				saveGameProvider.Destroy();
			}
			this.SaveGameProvider = null;
			this.UserDataRoaming = null;
			IApplicationStateController applicationState = this.ApplicationState;
			if (applicationState != null)
			{
				applicationState.Destroy();
			}
			this.ApplicationState = null;
			IVirtualKeyboard virtualKeyboard = this.VirtualKeyboard;
			if (virtualKeyboard != null)
			{
				virtualKeyboard.Destroy();
			}
			this.VirtualKeyboard = null;
			this.NetworkClient = null;
			this.NetworkServer = null;
			this.PlayerReporting = null;
			this.TextCensor = null;
			IAntiCheatServer antiCheatServer = this.AntiCheatServer;
			if (antiCheatServer != null)
			{
				antiCheatServer.Destroy();
			}
			this.AntiCheatServer = null;
			IAntiCheatClient antiCheatClient = this.AntiCheatClient;
			if (antiCheatClient != null)
			{
				antiCheatClient.Destroy();
			}
			this.AntiCheatClient = null;
			this.Memory = null;
			IUtils utils = this.Utils;
			if (utils != null)
			{
				utils.ClearTempFiles();
			}
			this.Utils = null;
			IPartyVoice partyVoice = this.PartyVoice;
			if (partyVoice != null)
			{
				partyVoice.Destroy();
			}
			this.PartyVoice = null;
			this.LobbyHost = null;
			IPlayerInteractionsRecorder playerInteractionsRecorder = this.PlayerInteractionsRecorder;
			if (playerInteractionsRecorder != null)
			{
				playerInteractionsRecorder.Destroy();
			}
			this.PlayerInteractionsRecorder = null;
			this.InviteListeners = null;
			this.ServerListAnnouncer = null;
			this.ServerListInterfaces = null;
			this.AuthenticationServer = null;
			IAuthenticationClient authenticationClient = this.AuthenticationClient;
			if (authenticationClient != null)
			{
				authenticationClient.Destroy();
			}
			this.AuthenticationClient = null;
			IUserClient userServer = this.UserServer;
			if (userServer != null)
			{
				userServer.Destroy();
			}
			this.UserServer = null;
			IUserClient user = this.User;
			if (user != null)
			{
				user.Destroy();
			}
			this.User = null;
			IPlatformApi api = this.Api;
			if (api != null)
			{
				api.Destroy();
			}
			this.Api = null;
		}

		// Token: 0x17001941 RID: 6465
		// (get) Token: 0x0600CFD5 RID: 53205 RVA: 0x004C0030 File Offset: 0x004BE230
		public EPlatformIdentifier PlatformIdentifier { get; }

		// Token: 0x17001942 RID: 6466
		// (get) Token: 0x0600CFD6 RID: 53206 RVA: 0x004C0038 File Offset: 0x004BE238
		// (set) Token: 0x0600CFD7 RID: 53207 RVA: 0x004C0040 File Offset: 0x004BE240
		public IPlatformApi Api
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001943 RID: 6467
		// (get) Token: 0x0600CFD8 RID: 53208 RVA: 0x004C0049 File Offset: 0x004BE249
		// (set) Token: 0x0600CFD9 RID: 53209 RVA: 0x004C0051 File Offset: 0x004BE251
		public IUserClient User
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001944 RID: 6468
		// (get) Token: 0x0600CFDA RID: 53210 RVA: 0x004C005A File Offset: 0x004BE25A
		// (set) Token: 0x0600CFDB RID: 53211 RVA: 0x004C0062 File Offset: 0x004BE262
		public IUserClient UserServer
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001945 RID: 6469
		// (get) Token: 0x0600CFDC RID: 53212 RVA: 0x004C006B File Offset: 0x004BE26B
		// (set) Token: 0x0600CFDD RID: 53213 RVA: 0x004C0073 File Offset: 0x004BE273
		public IAuthenticationClient AuthenticationClient
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001946 RID: 6470
		// (get) Token: 0x0600CFDE RID: 53214 RVA: 0x004C007C File Offset: 0x004BE27C
		// (set) Token: 0x0600CFDF RID: 53215 RVA: 0x004C0084 File Offset: 0x004BE284
		public IAuthenticationServer AuthenticationServer
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001947 RID: 6471
		// (get) Token: 0x0600CFE0 RID: 53216 RVA: 0x004C008D File Offset: 0x004BE28D
		// (set) Token: 0x0600CFE1 RID: 53217 RVA: 0x004C0095 File Offset: 0x004BE295
		public IList<IServerListInterface> ServerListInterfaces
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001948 RID: 6472
		// (get) Token: 0x0600CFE2 RID: 53218 RVA: 0x004C009E File Offset: 0x004BE29E
		// (set) Token: 0x0600CFE3 RID: 53219 RVA: 0x004C00A6 File Offset: 0x004BE2A6
		public IServerListInterface ServerLookupInterface
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001949 RID: 6473
		// (get) Token: 0x0600CFE4 RID: 53220 RVA: 0x004C00AF File Offset: 0x004BE2AF
		// (set) Token: 0x0600CFE5 RID: 53221 RVA: 0x004C00B7 File Offset: 0x004BE2B7
		public IMasterServerAnnouncer ServerListAnnouncer
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x1700194A RID: 6474
		// (get) Token: 0x0600CFE6 RID: 53222 RVA: 0x004C00C0 File Offset: 0x004BE2C0
		// (set) Token: 0x0600CFE7 RID: 53223 RVA: 0x004C00C8 File Offset: 0x004BE2C8
		public ILobbyHost LobbyHost
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x1700194B RID: 6475
		// (get) Token: 0x0600CFE8 RID: 53224 RVA: 0x004C00D1 File Offset: 0x004BE2D1
		// (set) Token: 0x0600CFE9 RID: 53225 RVA: 0x004C00D9 File Offset: 0x004BE2D9
		public IPlayerInteractionsRecorder PlayerInteractionsRecorder
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x1700194C RID: 6476
		// (get) Token: 0x0600CFEA RID: 53226 RVA: 0x004C00E2 File Offset: 0x004BE2E2
		// (set) Token: 0x0600CFEB RID: 53227 RVA: 0x004C00EA File Offset: 0x004BE2EA
		public IGameplayNotifier GameplayNotifier
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x1700194D RID: 6477
		// (get) Token: 0x0600CFEC RID: 53228 RVA: 0x004C00F3 File Offset: 0x004BE2F3
		// (set) Token: 0x0600CFED RID: 53229 RVA: 0x004C00FB File Offset: 0x004BE2FB
		public IList<IJoinSessionGameInviteListener> InviteListeners
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x1700194E RID: 6478
		// (get) Token: 0x0600CFEE RID: 53230 RVA: 0x004C0104 File Offset: 0x004BE304
		// (set) Token: 0x0600CFEF RID: 53231 RVA: 0x004C010C File Offset: 0x004BE30C
		public IMultiplayerInvitationDialog MultiplayerInvitationDialog
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x1700194F RID: 6479
		// (get) Token: 0x0600CFF0 RID: 53232 RVA: 0x004C0115 File Offset: 0x004BE315
		// (set) Token: 0x0600CFF1 RID: 53233 RVA: 0x004C011D File Offset: 0x004BE31D
		public IPartyVoice PartyVoice
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001950 RID: 6480
		// (get) Token: 0x0600CFF2 RID: 53234 RVA: 0x004C0126 File Offset: 0x004BE326
		// (set) Token: 0x0600CFF3 RID: 53235 RVA: 0x004C012E File Offset: 0x004BE32E
		public IUtils Utils
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001951 RID: 6481
		// (get) Token: 0x0600CFF4 RID: 53236 RVA: 0x004C0137 File Offset: 0x004BE337
		// (set) Token: 0x0600CFF5 RID: 53237 RVA: 0x004C013F File Offset: 0x004BE33F
		public IPlatformMemory Memory
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001952 RID: 6482
		// (get) Token: 0x0600CFF6 RID: 53238 RVA: 0x004C0148 File Offset: 0x004BE348
		// (set) Token: 0x0600CFF7 RID: 53239 RVA: 0x004C0150 File Offset: 0x004BE350
		public IAntiCheatClient AntiCheatClient
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001953 RID: 6483
		// (get) Token: 0x0600CFF8 RID: 53240 RVA: 0x004C0159 File Offset: 0x004BE359
		// (set) Token: 0x0600CFF9 RID: 53241 RVA: 0x004C0161 File Offset: 0x004BE361
		public IAntiCheatServer AntiCheatServer
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001954 RID: 6484
		// (get) Token: 0x0600CFFA RID: 53242 RVA: 0x004C016A File Offset: 0x004BE36A
		// (set) Token: 0x0600CFFB RID: 53243 RVA: 0x004C0172 File Offset: 0x004BE372
		public IUserIdentifierMappingService IdMappingService
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001955 RID: 6485
		// (get) Token: 0x0600CFFC RID: 53244 RVA: 0x004C017B File Offset: 0x004BE37B
		// (set) Token: 0x0600CFFD RID: 53245 RVA: 0x004C0183 File Offset: 0x004BE383
		public IUserDetailsService UserDetailsService
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001956 RID: 6486
		// (get) Token: 0x0600CFFE RID: 53246 RVA: 0x004C018C File Offset: 0x004BE38C
		// (set) Token: 0x0600CFFF RID: 53247 RVA: 0x004C0194 File Offset: 0x004BE394
		public IPlayerReporting PlayerReporting
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001957 RID: 6487
		// (get) Token: 0x0600D000 RID: 53248 RVA: 0x004C019D File Offset: 0x004BE39D
		// (set) Token: 0x0600D001 RID: 53249 RVA: 0x004C01A5 File Offset: 0x004BE3A5
		public ITextCensor TextCensor
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001958 RID: 6488
		// (get) Token: 0x0600D002 RID: 53250 RVA: 0x004C01AE File Offset: 0x004BE3AE
		// (set) Token: 0x0600D003 RID: 53251 RVA: 0x004C01B6 File Offset: 0x004BE3B6
		public IRemoteFileStorage RemoteFileStorage
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001959 RID: 6489
		// (get) Token: 0x0600D004 RID: 53252 RVA: 0x004C01BF File Offset: 0x004BE3BF
		// (set) Token: 0x0600D005 RID: 53253 RVA: 0x004C01C7 File Offset: 0x004BE3C7
		public IRemotePlayerFileStorage RemotePlayerFileStorage
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x1700195A RID: 6490
		// (get) Token: 0x0600D006 RID: 53254 RVA: 0x004C01D0 File Offset: 0x004BE3D0
		// (set) Token: 0x0600D007 RID: 53255 RVA: 0x004C01D8 File Offset: 0x004BE3D8
		public IList<IEntitlementValidator> EntitlementValidators
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x1700195B RID: 6491
		// (get) Token: 0x0600D008 RID: 53256 RVA: 0x004C01E1 File Offset: 0x004BE3E1
		// (set) Token: 0x0600D009 RID: 53257 RVA: 0x004C01E9 File Offset: 0x004BE3E9
		public PlayerInputManager Input
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x1700195C RID: 6492
		// (get) Token: 0x0600D00A RID: 53258 RVA: 0x004C01F2 File Offset: 0x004BE3F2
		// (set) Token: 0x0600D00B RID: 53259 RVA: 0x004C01FA File Offset: 0x004BE3FA
		public IVirtualKeyboard VirtualKeyboard
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x1700195D RID: 6493
		// (get) Token: 0x0600D00C RID: 53260 RVA: 0x004C0203 File Offset: 0x004BE403
		// (set) Token: 0x0600D00D RID: 53261 RVA: 0x004C020B File Offset: 0x004BE40B
		public IAchievementManager AchievementManager
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x1700195E RID: 6494
		// (get) Token: 0x0600D00E RID: 53262 RVA: 0x004C0214 File Offset: 0x004BE414
		// (set) Token: 0x0600D00F RID: 53263 RVA: 0x004C021C File Offset: 0x004BE41C
		public IRichPresence RichPresence
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x1700195F RID: 6495
		// (get) Token: 0x0600D010 RID: 53264 RVA: 0x004C0225 File Offset: 0x004BE425
		// (set) Token: 0x0600D011 RID: 53265 RVA: 0x004C022D File Offset: 0x004BE42D
		public IApplicationStateController ApplicationState
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001960 RID: 6496
		// (get) Token: 0x0600D012 RID: 53266 RVA: 0x004C0236 File Offset: 0x004BE436
		// (set) Token: 0x0600D013 RID: 53267 RVA: 0x004C023E File Offset: 0x004BE43E
		public IPlatformSaveGameProvider SaveGameProvider
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001961 RID: 6497
		// (get) Token: 0x0600D014 RID: 53268 RVA: 0x004C0247 File Offset: 0x004BE447
		// (set) Token: 0x0600D015 RID: 53269 RVA: 0x004C024F File Offset: 0x004BE44F
		public IUserDataRoaming UserDataRoaming
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001962 RID: 6498
		// (get) Token: 0x0600D016 RID: 53270 RVA: 0x004C0258 File Offset: 0x004BE458
		public virtual string NetworkProtocolName
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get;
		}

		// Token: 0x04009F41 RID: 40769
		[PublicizedFrom(EAccessModifier.Protected)]
		public IPlatformNetworkServer NetworkServer;

		// Token: 0x04009F42 RID: 40770
		[PublicizedFrom(EAccessModifier.Protected)]
		public IPlatformNetworkClient NetworkClient;
	}
}
