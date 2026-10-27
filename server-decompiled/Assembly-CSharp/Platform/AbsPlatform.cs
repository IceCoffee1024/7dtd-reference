using System;
using System.Collections.Generic;
using InControl;

namespace Platform
{
	// Token: 0x02001BB3 RID: 7091
	public abstract class AbsPlatform : IPlatform
	{
		// Token: 0x0600D3D7 RID: 54231 RVA: 0x004CE3BC File Offset: 0x004CC5BC
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

		// Token: 0x17001A9E RID: 6814
		// (get) Token: 0x0600D3D8 RID: 54232 RVA: 0x004CE40C File Offset: 0x004CC60C
		// (set) Token: 0x0600D3D9 RID: 54233 RVA: 0x004CE414 File Offset: 0x004CC614
		public bool AsServerOnly { get; set; }

		// Token: 0x17001A9F RID: 6815
		// (get) Token: 0x0600D3DA RID: 54234 RVA: 0x004CE41D File Offset: 0x004CC61D
		// (set) Token: 0x0600D3DB RID: 54235 RVA: 0x004CE425 File Offset: 0x004CC625
		public bool IsCrossplatform { get; set; }

		// Token: 0x17001AA0 RID: 6816
		// (get) Token: 0x0600D3DC RID: 54236 RVA: 0x004CE42E File Offset: 0x004CC62E
		public string PlatformDisplayName
		{
			get
			{
				return PlatformManager.GetPlatformDisplayName(this.PlatformIdentifier);
			}
		}

		// Token: 0x0600D3DD RID: 54237 RVA: 0x004CE43C File Offset: 0x004CC63C
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

		// Token: 0x0600D3DE RID: 54238 RVA: 0x004CE770 File Offset: 0x004CC970
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

		// Token: 0x0600D3DF RID: 54239 RVA: 0x000200DA File Offset: 0x0001E2DA
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual IPlatformNetworkServer instantiateNetworkServer(ProtocolManager _protocolManager)
		{
			return null;
		}

		// Token: 0x0600D3E0 RID: 54240 RVA: 0x000200DA File Offset: 0x0001E2DA
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual IPlatformNetworkClient instantiateNetworkClient(ProtocolManager _protocolManager)
		{
			return null;
		}

		// Token: 0x0600D3E1 RID: 54241 RVA: 0x004CE7C8 File Offset: 0x004CC9C8
		public INetworkServer GetNetworkingServer(ProtocolManager _protocolManager)
		{
			IPlatformNetworkServer platformNetworkServer;
			if ((platformNetworkServer = this.NetworkServer) == null)
			{
				platformNetworkServer = (this.NetworkServer = this.instantiateNetworkServer(_protocolManager));
			}
			return platformNetworkServer;
		}

		// Token: 0x0600D3E2 RID: 54242 RVA: 0x004CE7F0 File Offset: 0x004CC9F0
		public INetworkClient GetNetworkingClient(ProtocolManager _protocolManager)
		{
			IPlatformNetworkClient platformNetworkClient;
			if ((platformNetworkClient = this.NetworkClient) == null)
			{
				platformNetworkClient = (this.NetworkClient = this.instantiateNetworkClient(_protocolManager));
			}
			return platformNetworkClient;
		}

		// Token: 0x0600D3E3 RID: 54243 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void UserAdded(PlatformUserIdentifierAbs _id, bool _isPrimary)
		{
		}

		// Token: 0x0600D3E4 RID: 54244 RVA: 0x004CE817 File Offset: 0x004CCA17
		public virtual string[] GetArgumentsForRelaunch()
		{
			return new string[0];
		}

		// Token: 0x0600D3E5 RID: 54245
		public abstract void CreateInstances();

		// Token: 0x0600D3E6 RID: 54246 RVA: 0x004CE820 File Offset: 0x004CCA20
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

		// Token: 0x0600D3E7 RID: 54247 RVA: 0x000027FC File Offset: 0x000009FC
		public void LateUpdate()
		{
		}

		// Token: 0x0600D3E8 RID: 54248 RVA: 0x004CE894 File Offset: 0x004CCA94
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

		// Token: 0x17001AA1 RID: 6817
		// (get) Token: 0x0600D3E9 RID: 54249 RVA: 0x004CEA34 File Offset: 0x004CCC34
		public EPlatformIdentifier PlatformIdentifier { get; }

		// Token: 0x17001AA2 RID: 6818
		// (get) Token: 0x0600D3EA RID: 54250 RVA: 0x004CEA3C File Offset: 0x004CCC3C
		// (set) Token: 0x0600D3EB RID: 54251 RVA: 0x004CEA44 File Offset: 0x004CCC44
		public IPlatformApi Api
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001AA3 RID: 6819
		// (get) Token: 0x0600D3EC RID: 54252 RVA: 0x004CEA4D File Offset: 0x004CCC4D
		// (set) Token: 0x0600D3ED RID: 54253 RVA: 0x004CEA55 File Offset: 0x004CCC55
		public IUserClient User
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001AA4 RID: 6820
		// (get) Token: 0x0600D3EE RID: 54254 RVA: 0x004CEA5E File Offset: 0x004CCC5E
		// (set) Token: 0x0600D3EF RID: 54255 RVA: 0x004CEA66 File Offset: 0x004CCC66
		public IUserClient UserServer
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001AA5 RID: 6821
		// (get) Token: 0x0600D3F0 RID: 54256 RVA: 0x004CEA6F File Offset: 0x004CCC6F
		// (set) Token: 0x0600D3F1 RID: 54257 RVA: 0x004CEA77 File Offset: 0x004CCC77
		public IAuthenticationClient AuthenticationClient
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001AA6 RID: 6822
		// (get) Token: 0x0600D3F2 RID: 54258 RVA: 0x004CEA80 File Offset: 0x004CCC80
		// (set) Token: 0x0600D3F3 RID: 54259 RVA: 0x004CEA88 File Offset: 0x004CCC88
		public IAuthenticationServer AuthenticationServer
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001AA7 RID: 6823
		// (get) Token: 0x0600D3F4 RID: 54260 RVA: 0x004CEA91 File Offset: 0x004CCC91
		// (set) Token: 0x0600D3F5 RID: 54261 RVA: 0x004CEA99 File Offset: 0x004CCC99
		public IList<IServerListInterface> ServerListInterfaces
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001AA8 RID: 6824
		// (get) Token: 0x0600D3F6 RID: 54262 RVA: 0x004CEAA2 File Offset: 0x004CCCA2
		// (set) Token: 0x0600D3F7 RID: 54263 RVA: 0x004CEAAA File Offset: 0x004CCCAA
		public IServerListInterface ServerLookupInterface
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001AA9 RID: 6825
		// (get) Token: 0x0600D3F8 RID: 54264 RVA: 0x004CEAB3 File Offset: 0x004CCCB3
		// (set) Token: 0x0600D3F9 RID: 54265 RVA: 0x004CEABB File Offset: 0x004CCCBB
		public IMasterServerAnnouncer ServerListAnnouncer
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001AAA RID: 6826
		// (get) Token: 0x0600D3FA RID: 54266 RVA: 0x004CEAC4 File Offset: 0x004CCCC4
		// (set) Token: 0x0600D3FB RID: 54267 RVA: 0x004CEACC File Offset: 0x004CCCCC
		public ILobbyHost LobbyHost
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001AAB RID: 6827
		// (get) Token: 0x0600D3FC RID: 54268 RVA: 0x004CEAD5 File Offset: 0x004CCCD5
		// (set) Token: 0x0600D3FD RID: 54269 RVA: 0x004CEADD File Offset: 0x004CCCDD
		public IPlayerInteractionsRecorder PlayerInteractionsRecorder
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001AAC RID: 6828
		// (get) Token: 0x0600D3FE RID: 54270 RVA: 0x004CEAE6 File Offset: 0x004CCCE6
		// (set) Token: 0x0600D3FF RID: 54271 RVA: 0x004CEAEE File Offset: 0x004CCCEE
		public IGameplayNotifier GameplayNotifier
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001AAD RID: 6829
		// (get) Token: 0x0600D400 RID: 54272 RVA: 0x004CEAF7 File Offset: 0x004CCCF7
		// (set) Token: 0x0600D401 RID: 54273 RVA: 0x004CEAFF File Offset: 0x004CCCFF
		public IList<IJoinSessionGameInviteListener> InviteListeners
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001AAE RID: 6830
		// (get) Token: 0x0600D402 RID: 54274 RVA: 0x004CEB08 File Offset: 0x004CCD08
		// (set) Token: 0x0600D403 RID: 54275 RVA: 0x004CEB10 File Offset: 0x004CCD10
		public IMultiplayerInvitationDialog MultiplayerInvitationDialog
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001AAF RID: 6831
		// (get) Token: 0x0600D404 RID: 54276 RVA: 0x004CEB19 File Offset: 0x004CCD19
		// (set) Token: 0x0600D405 RID: 54277 RVA: 0x004CEB21 File Offset: 0x004CCD21
		public IPartyVoice PartyVoice
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001AB0 RID: 6832
		// (get) Token: 0x0600D406 RID: 54278 RVA: 0x004CEB2A File Offset: 0x004CCD2A
		// (set) Token: 0x0600D407 RID: 54279 RVA: 0x004CEB32 File Offset: 0x004CCD32
		public IUtils Utils
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001AB1 RID: 6833
		// (get) Token: 0x0600D408 RID: 54280 RVA: 0x004CEB3B File Offset: 0x004CCD3B
		// (set) Token: 0x0600D409 RID: 54281 RVA: 0x004CEB43 File Offset: 0x004CCD43
		public IPlatformMemory Memory
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001AB2 RID: 6834
		// (get) Token: 0x0600D40A RID: 54282 RVA: 0x004CEB4C File Offset: 0x004CCD4C
		// (set) Token: 0x0600D40B RID: 54283 RVA: 0x004CEB54 File Offset: 0x004CCD54
		public IAntiCheatClient AntiCheatClient
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001AB3 RID: 6835
		// (get) Token: 0x0600D40C RID: 54284 RVA: 0x004CEB5D File Offset: 0x004CCD5D
		// (set) Token: 0x0600D40D RID: 54285 RVA: 0x004CEB65 File Offset: 0x004CCD65
		public IAntiCheatServer AntiCheatServer
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001AB4 RID: 6836
		// (get) Token: 0x0600D40E RID: 54286 RVA: 0x004CEB6E File Offset: 0x004CCD6E
		// (set) Token: 0x0600D40F RID: 54287 RVA: 0x004CEB76 File Offset: 0x004CCD76
		public IUserIdentifierMappingService IdMappingService
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001AB5 RID: 6837
		// (get) Token: 0x0600D410 RID: 54288 RVA: 0x004CEB7F File Offset: 0x004CCD7F
		// (set) Token: 0x0600D411 RID: 54289 RVA: 0x004CEB87 File Offset: 0x004CCD87
		public IUserDetailsService UserDetailsService
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001AB6 RID: 6838
		// (get) Token: 0x0600D412 RID: 54290 RVA: 0x004CEB90 File Offset: 0x004CCD90
		// (set) Token: 0x0600D413 RID: 54291 RVA: 0x004CEB98 File Offset: 0x004CCD98
		public IPlayerReporting PlayerReporting
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001AB7 RID: 6839
		// (get) Token: 0x0600D414 RID: 54292 RVA: 0x004CEBA1 File Offset: 0x004CCDA1
		// (set) Token: 0x0600D415 RID: 54293 RVA: 0x004CEBA9 File Offset: 0x004CCDA9
		public ITextCensor TextCensor
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001AB8 RID: 6840
		// (get) Token: 0x0600D416 RID: 54294 RVA: 0x004CEBB2 File Offset: 0x004CCDB2
		// (set) Token: 0x0600D417 RID: 54295 RVA: 0x004CEBBA File Offset: 0x004CCDBA
		public IRemoteFileStorage RemoteFileStorage
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001AB9 RID: 6841
		// (get) Token: 0x0600D418 RID: 54296 RVA: 0x004CEBC3 File Offset: 0x004CCDC3
		// (set) Token: 0x0600D419 RID: 54297 RVA: 0x004CEBCB File Offset: 0x004CCDCB
		public IRemotePlayerFileStorage RemotePlayerFileStorage
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001ABA RID: 6842
		// (get) Token: 0x0600D41A RID: 54298 RVA: 0x004CEBD4 File Offset: 0x004CCDD4
		// (set) Token: 0x0600D41B RID: 54299 RVA: 0x004CEBDC File Offset: 0x004CCDDC
		public IList<IEntitlementValidator> EntitlementValidators
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001ABB RID: 6843
		// (get) Token: 0x0600D41C RID: 54300 RVA: 0x004CEBE5 File Offset: 0x004CCDE5
		// (set) Token: 0x0600D41D RID: 54301 RVA: 0x004CEBED File Offset: 0x004CCDED
		public PlayerInputManager Input
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001ABC RID: 6844
		// (get) Token: 0x0600D41E RID: 54302 RVA: 0x004CEBF6 File Offset: 0x004CCDF6
		// (set) Token: 0x0600D41F RID: 54303 RVA: 0x004CEBFE File Offset: 0x004CCDFE
		public IVirtualKeyboard VirtualKeyboard
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001ABD RID: 6845
		// (get) Token: 0x0600D420 RID: 54304 RVA: 0x004CEC07 File Offset: 0x004CCE07
		// (set) Token: 0x0600D421 RID: 54305 RVA: 0x004CEC0F File Offset: 0x004CCE0F
		public IAchievementManager AchievementManager
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001ABE RID: 6846
		// (get) Token: 0x0600D422 RID: 54306 RVA: 0x004CEC18 File Offset: 0x004CCE18
		// (set) Token: 0x0600D423 RID: 54307 RVA: 0x004CEC20 File Offset: 0x004CCE20
		public IRichPresence RichPresence
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001ABF RID: 6847
		// (get) Token: 0x0600D424 RID: 54308 RVA: 0x004CEC29 File Offset: 0x004CCE29
		// (set) Token: 0x0600D425 RID: 54309 RVA: 0x004CEC31 File Offset: 0x004CCE31
		public IApplicationStateController ApplicationState
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001AC0 RID: 6848
		// (get) Token: 0x0600D426 RID: 54310 RVA: 0x004CEC3A File Offset: 0x004CCE3A
		// (set) Token: 0x0600D427 RID: 54311 RVA: 0x004CEC42 File Offset: 0x004CCE42
		public IPlatformSaveGameProvider SaveGameProvider
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001AC1 RID: 6849
		// (get) Token: 0x0600D428 RID: 54312 RVA: 0x004CEC4B File Offset: 0x004CCE4B
		// (set) Token: 0x0600D429 RID: 54313 RVA: 0x004CEC53 File Offset: 0x004CCE53
		public IUserDataRoaming UserDataRoaming
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001AC2 RID: 6850
		// (get) Token: 0x0600D42A RID: 54314 RVA: 0x004CEC5C File Offset: 0x004CCE5C
		public virtual string NetworkProtocolName
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get;
		}

		// Token: 0x0400A230 RID: 41520
		[PublicizedFrom(EAccessModifier.Protected)]
		public IPlatformNetworkServer NetworkServer;

		// Token: 0x0400A231 RID: 41521
		[PublicizedFrom(EAccessModifier.Protected)]
		public IPlatformNetworkClient NetworkClient;
	}
}
