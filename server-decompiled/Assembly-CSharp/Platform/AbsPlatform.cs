using System;
using System.Collections.Generic;
using InControl;

namespace Platform
{
	// Token: 0x02001B82 RID: 7042
	public abstract class AbsPlatform : IPlatform
	{
		// Token: 0x0600D2C3 RID: 53955 RVA: 0x004CA894 File Offset: 0x004C8A94
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

		// Token: 0x17001A56 RID: 6742
		// (get) Token: 0x0600D2C4 RID: 53956 RVA: 0x004CA8E4 File Offset: 0x004C8AE4
		// (set) Token: 0x0600D2C5 RID: 53957 RVA: 0x004CA8EC File Offset: 0x004C8AEC
		public bool AsServerOnly { get; set; }

		// Token: 0x17001A57 RID: 6743
		// (get) Token: 0x0600D2C6 RID: 53958 RVA: 0x004CA8F5 File Offset: 0x004C8AF5
		// (set) Token: 0x0600D2C7 RID: 53959 RVA: 0x004CA8FD File Offset: 0x004C8AFD
		public bool IsCrossplatform { get; set; }

		// Token: 0x17001A58 RID: 6744
		// (get) Token: 0x0600D2C8 RID: 53960 RVA: 0x004CA906 File Offset: 0x004C8B06
		public string PlatformDisplayName
		{
			get
			{
				return PlatformManager.GetPlatformDisplayName(this.PlatformIdentifier);
			}
		}

		// Token: 0x0600D2C9 RID: 53961 RVA: 0x004CA914 File Offset: 0x004C8B14
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

		// Token: 0x0600D2CA RID: 53962 RVA: 0x004CAC48 File Offset: 0x004C8E48
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

		// Token: 0x0600D2CB RID: 53963 RVA: 0x000200DA File Offset: 0x0001E2DA
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual IPlatformNetworkServer instantiateNetworkServer(ProtocolManager _protocolManager)
		{
			return null;
		}

		// Token: 0x0600D2CC RID: 53964 RVA: 0x000200DA File Offset: 0x0001E2DA
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual IPlatformNetworkClient instantiateNetworkClient(ProtocolManager _protocolManager)
		{
			return null;
		}

		// Token: 0x0600D2CD RID: 53965 RVA: 0x004CACA0 File Offset: 0x004C8EA0
		public INetworkServer GetNetworkingServer(ProtocolManager _protocolManager)
		{
			IPlatformNetworkServer platformNetworkServer;
			if ((platformNetworkServer = this.NetworkServer) == null)
			{
				platformNetworkServer = (this.NetworkServer = this.instantiateNetworkServer(_protocolManager));
			}
			return platformNetworkServer;
		}

		// Token: 0x0600D2CE RID: 53966 RVA: 0x004CACC8 File Offset: 0x004C8EC8
		public INetworkClient GetNetworkingClient(ProtocolManager _protocolManager)
		{
			IPlatformNetworkClient platformNetworkClient;
			if ((platformNetworkClient = this.NetworkClient) == null)
			{
				platformNetworkClient = (this.NetworkClient = this.instantiateNetworkClient(_protocolManager));
			}
			return platformNetworkClient;
		}

		// Token: 0x0600D2CF RID: 53967 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void UserAdded(PlatformUserIdentifierAbs _id, bool _isPrimary)
		{
		}

		// Token: 0x0600D2D0 RID: 53968 RVA: 0x004CACEF File Offset: 0x004C8EEF
		public virtual string[] GetArgumentsForRelaunch()
		{
			return new string[0];
		}

		// Token: 0x0600D2D1 RID: 53969
		public abstract void CreateInstances();

		// Token: 0x0600D2D2 RID: 53970 RVA: 0x004CACF8 File Offset: 0x004C8EF8
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

		// Token: 0x0600D2D3 RID: 53971 RVA: 0x000027FC File Offset: 0x000009FC
		public void LateUpdate()
		{
		}

		// Token: 0x0600D2D4 RID: 53972 RVA: 0x004CAD6C File Offset: 0x004C8F6C
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

		// Token: 0x17001A59 RID: 6745
		// (get) Token: 0x0600D2D5 RID: 53973 RVA: 0x004CAF0C File Offset: 0x004C910C
		public EPlatformIdentifier PlatformIdentifier { get; }

		// Token: 0x17001A5A RID: 6746
		// (get) Token: 0x0600D2D6 RID: 53974 RVA: 0x004CAF14 File Offset: 0x004C9114
		// (set) Token: 0x0600D2D7 RID: 53975 RVA: 0x004CAF1C File Offset: 0x004C911C
		public IPlatformApi Api
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001A5B RID: 6747
		// (get) Token: 0x0600D2D8 RID: 53976 RVA: 0x004CAF25 File Offset: 0x004C9125
		// (set) Token: 0x0600D2D9 RID: 53977 RVA: 0x004CAF2D File Offset: 0x004C912D
		public IUserClient User
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001A5C RID: 6748
		// (get) Token: 0x0600D2DA RID: 53978 RVA: 0x004CAF36 File Offset: 0x004C9136
		// (set) Token: 0x0600D2DB RID: 53979 RVA: 0x004CAF3E File Offset: 0x004C913E
		public IUserClient UserServer
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001A5D RID: 6749
		// (get) Token: 0x0600D2DC RID: 53980 RVA: 0x004CAF47 File Offset: 0x004C9147
		// (set) Token: 0x0600D2DD RID: 53981 RVA: 0x004CAF4F File Offset: 0x004C914F
		public IAuthenticationClient AuthenticationClient
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001A5E RID: 6750
		// (get) Token: 0x0600D2DE RID: 53982 RVA: 0x004CAF58 File Offset: 0x004C9158
		// (set) Token: 0x0600D2DF RID: 53983 RVA: 0x004CAF60 File Offset: 0x004C9160
		public IAuthenticationServer AuthenticationServer
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001A5F RID: 6751
		// (get) Token: 0x0600D2E0 RID: 53984 RVA: 0x004CAF69 File Offset: 0x004C9169
		// (set) Token: 0x0600D2E1 RID: 53985 RVA: 0x004CAF71 File Offset: 0x004C9171
		public IList<IServerListInterface> ServerListInterfaces
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001A60 RID: 6752
		// (get) Token: 0x0600D2E2 RID: 53986 RVA: 0x004CAF7A File Offset: 0x004C917A
		// (set) Token: 0x0600D2E3 RID: 53987 RVA: 0x004CAF82 File Offset: 0x004C9182
		public IServerListInterface ServerLookupInterface
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001A61 RID: 6753
		// (get) Token: 0x0600D2E4 RID: 53988 RVA: 0x004CAF8B File Offset: 0x004C918B
		// (set) Token: 0x0600D2E5 RID: 53989 RVA: 0x004CAF93 File Offset: 0x004C9193
		public IMasterServerAnnouncer ServerListAnnouncer
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001A62 RID: 6754
		// (get) Token: 0x0600D2E6 RID: 53990 RVA: 0x004CAF9C File Offset: 0x004C919C
		// (set) Token: 0x0600D2E7 RID: 53991 RVA: 0x004CAFA4 File Offset: 0x004C91A4
		public ILobbyHost LobbyHost
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001A63 RID: 6755
		// (get) Token: 0x0600D2E8 RID: 53992 RVA: 0x004CAFAD File Offset: 0x004C91AD
		// (set) Token: 0x0600D2E9 RID: 53993 RVA: 0x004CAFB5 File Offset: 0x004C91B5
		public IPlayerInteractionsRecorder PlayerInteractionsRecorder
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001A64 RID: 6756
		// (get) Token: 0x0600D2EA RID: 53994 RVA: 0x004CAFBE File Offset: 0x004C91BE
		// (set) Token: 0x0600D2EB RID: 53995 RVA: 0x004CAFC6 File Offset: 0x004C91C6
		public IGameplayNotifier GameplayNotifier
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001A65 RID: 6757
		// (get) Token: 0x0600D2EC RID: 53996 RVA: 0x004CAFCF File Offset: 0x004C91CF
		// (set) Token: 0x0600D2ED RID: 53997 RVA: 0x004CAFD7 File Offset: 0x004C91D7
		public IList<IJoinSessionGameInviteListener> InviteListeners
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001A66 RID: 6758
		// (get) Token: 0x0600D2EE RID: 53998 RVA: 0x004CAFE0 File Offset: 0x004C91E0
		// (set) Token: 0x0600D2EF RID: 53999 RVA: 0x004CAFE8 File Offset: 0x004C91E8
		public IMultiplayerInvitationDialog MultiplayerInvitationDialog
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001A67 RID: 6759
		// (get) Token: 0x0600D2F0 RID: 54000 RVA: 0x004CAFF1 File Offset: 0x004C91F1
		// (set) Token: 0x0600D2F1 RID: 54001 RVA: 0x004CAFF9 File Offset: 0x004C91F9
		public IPartyVoice PartyVoice
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001A68 RID: 6760
		// (get) Token: 0x0600D2F2 RID: 54002 RVA: 0x004CB002 File Offset: 0x004C9202
		// (set) Token: 0x0600D2F3 RID: 54003 RVA: 0x004CB00A File Offset: 0x004C920A
		public IUtils Utils
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001A69 RID: 6761
		// (get) Token: 0x0600D2F4 RID: 54004 RVA: 0x004CB013 File Offset: 0x004C9213
		// (set) Token: 0x0600D2F5 RID: 54005 RVA: 0x004CB01B File Offset: 0x004C921B
		public IPlatformMemory Memory
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001A6A RID: 6762
		// (get) Token: 0x0600D2F6 RID: 54006 RVA: 0x004CB024 File Offset: 0x004C9224
		// (set) Token: 0x0600D2F7 RID: 54007 RVA: 0x004CB02C File Offset: 0x004C922C
		public IAntiCheatClient AntiCheatClient
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001A6B RID: 6763
		// (get) Token: 0x0600D2F8 RID: 54008 RVA: 0x004CB035 File Offset: 0x004C9235
		// (set) Token: 0x0600D2F9 RID: 54009 RVA: 0x004CB03D File Offset: 0x004C923D
		public IAntiCheatServer AntiCheatServer
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001A6C RID: 6764
		// (get) Token: 0x0600D2FA RID: 54010 RVA: 0x004CB046 File Offset: 0x004C9246
		// (set) Token: 0x0600D2FB RID: 54011 RVA: 0x004CB04E File Offset: 0x004C924E
		public IUserIdentifierMappingService IdMappingService
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001A6D RID: 6765
		// (get) Token: 0x0600D2FC RID: 54012 RVA: 0x004CB057 File Offset: 0x004C9257
		// (set) Token: 0x0600D2FD RID: 54013 RVA: 0x004CB05F File Offset: 0x004C925F
		public IUserDetailsService UserDetailsService
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001A6E RID: 6766
		// (get) Token: 0x0600D2FE RID: 54014 RVA: 0x004CB068 File Offset: 0x004C9268
		// (set) Token: 0x0600D2FF RID: 54015 RVA: 0x004CB070 File Offset: 0x004C9270
		public IPlayerReporting PlayerReporting
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001A6F RID: 6767
		// (get) Token: 0x0600D300 RID: 54016 RVA: 0x004CB079 File Offset: 0x004C9279
		// (set) Token: 0x0600D301 RID: 54017 RVA: 0x004CB081 File Offset: 0x004C9281
		public ITextCensor TextCensor
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001A70 RID: 6768
		// (get) Token: 0x0600D302 RID: 54018 RVA: 0x004CB08A File Offset: 0x004C928A
		// (set) Token: 0x0600D303 RID: 54019 RVA: 0x004CB092 File Offset: 0x004C9292
		public IRemoteFileStorage RemoteFileStorage
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001A71 RID: 6769
		// (get) Token: 0x0600D304 RID: 54020 RVA: 0x004CB09B File Offset: 0x004C929B
		// (set) Token: 0x0600D305 RID: 54021 RVA: 0x004CB0A3 File Offset: 0x004C92A3
		public IRemotePlayerFileStorage RemotePlayerFileStorage
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001A72 RID: 6770
		// (get) Token: 0x0600D306 RID: 54022 RVA: 0x004CB0AC File Offset: 0x004C92AC
		// (set) Token: 0x0600D307 RID: 54023 RVA: 0x004CB0B4 File Offset: 0x004C92B4
		public IList<IEntitlementValidator> EntitlementValidators
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001A73 RID: 6771
		// (get) Token: 0x0600D308 RID: 54024 RVA: 0x004CB0BD File Offset: 0x004C92BD
		// (set) Token: 0x0600D309 RID: 54025 RVA: 0x004CB0C5 File Offset: 0x004C92C5
		public PlayerInputManager Input
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001A74 RID: 6772
		// (get) Token: 0x0600D30A RID: 54026 RVA: 0x004CB0CE File Offset: 0x004C92CE
		// (set) Token: 0x0600D30B RID: 54027 RVA: 0x004CB0D6 File Offset: 0x004C92D6
		public IVirtualKeyboard VirtualKeyboard
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001A75 RID: 6773
		// (get) Token: 0x0600D30C RID: 54028 RVA: 0x004CB0DF File Offset: 0x004C92DF
		// (set) Token: 0x0600D30D RID: 54029 RVA: 0x004CB0E7 File Offset: 0x004C92E7
		public IAchievementManager AchievementManager
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001A76 RID: 6774
		// (get) Token: 0x0600D30E RID: 54030 RVA: 0x004CB0F0 File Offset: 0x004C92F0
		// (set) Token: 0x0600D30F RID: 54031 RVA: 0x004CB0F8 File Offset: 0x004C92F8
		public IRichPresence RichPresence
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001A77 RID: 6775
		// (get) Token: 0x0600D310 RID: 54032 RVA: 0x004CB101 File Offset: 0x004C9301
		// (set) Token: 0x0600D311 RID: 54033 RVA: 0x004CB109 File Offset: 0x004C9309
		public IApplicationStateController ApplicationState
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001A78 RID: 6776
		// (get) Token: 0x0600D312 RID: 54034 RVA: 0x004CB112 File Offset: 0x004C9312
		// (set) Token: 0x0600D313 RID: 54035 RVA: 0x004CB11A File Offset: 0x004C931A
		public IPlatformSaveGameProvider SaveGameProvider
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001A79 RID: 6777
		// (get) Token: 0x0600D314 RID: 54036 RVA: 0x004CB123 File Offset: 0x004C9323
		// (set) Token: 0x0600D315 RID: 54037 RVA: 0x004CB12B File Offset: 0x004C932B
		public IUserDataRoaming UserDataRoaming
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001A7A RID: 6778
		// (get) Token: 0x0600D316 RID: 54038 RVA: 0x004CB134 File Offset: 0x004C9334
		public virtual string NetworkProtocolName
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get;
		}

		// Token: 0x0400A147 RID: 41287
		[PublicizedFrom(EAccessModifier.Protected)]
		public IPlatformNetworkServer NetworkServer;

		// Token: 0x0400A148 RID: 41288
		[PublicizedFrom(EAccessModifier.Protected)]
		public IPlatformNetworkClient NetworkClient;
	}
}
