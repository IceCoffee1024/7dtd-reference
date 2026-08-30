using System;
using System.Collections.Generic;
using InControl;

namespace Platform
{
	// Token: 0x02001B50 RID: 6992
	public abstract class AbsPlatform : IPlatform
	{
		// Token: 0x0600D14C RID: 53580 RVA: 0x004C5DAC File Offset: 0x004C3FAC
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

		// Token: 0x170019B0 RID: 6576
		// (get) Token: 0x0600D14D RID: 53581 RVA: 0x004C5DFC File Offset: 0x004C3FFC
		// (set) Token: 0x0600D14E RID: 53582 RVA: 0x004C5E04 File Offset: 0x004C4004
		public bool AsServerOnly { get; set; }

		// Token: 0x170019B1 RID: 6577
		// (get) Token: 0x0600D14F RID: 53583 RVA: 0x004C5E0D File Offset: 0x004C400D
		// (set) Token: 0x0600D150 RID: 53584 RVA: 0x004C5E15 File Offset: 0x004C4015
		public bool IsCrossplatform { get; set; }

		// Token: 0x170019B2 RID: 6578
		// (get) Token: 0x0600D151 RID: 53585 RVA: 0x004C5E1E File Offset: 0x004C401E
		public string PlatformDisplayName
		{
			get
			{
				return PlatformManager.GetPlatformDisplayName(this.PlatformIdentifier);
			}
		}

		// Token: 0x0600D152 RID: 53586 RVA: 0x004C5E2C File Offset: 0x004C402C
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

		// Token: 0x0600D153 RID: 53587 RVA: 0x004C6160 File Offset: 0x004C4360
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

		// Token: 0x0600D154 RID: 53588 RVA: 0x0001FFE2 File Offset: 0x0001E1E2
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual IPlatformNetworkServer instantiateNetworkServer(ProtocolManager _protocolManager)
		{
			return null;
		}

		// Token: 0x0600D155 RID: 53589 RVA: 0x0001FFE2 File Offset: 0x0001E1E2
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual IPlatformNetworkClient instantiateNetworkClient(ProtocolManager _protocolManager)
		{
			return null;
		}

		// Token: 0x0600D156 RID: 53590 RVA: 0x004C61B8 File Offset: 0x004C43B8
		public INetworkServer GetNetworkingServer(ProtocolManager _protocolManager)
		{
			IPlatformNetworkServer platformNetworkServer;
			if ((platformNetworkServer = this.NetworkServer) == null)
			{
				platformNetworkServer = (this.NetworkServer = this.instantiateNetworkServer(_protocolManager));
			}
			return platformNetworkServer;
		}

		// Token: 0x0600D157 RID: 53591 RVA: 0x004C61E0 File Offset: 0x004C43E0
		public INetworkClient GetNetworkingClient(ProtocolManager _protocolManager)
		{
			IPlatformNetworkClient platformNetworkClient;
			if ((platformNetworkClient = this.NetworkClient) == null)
			{
				platformNetworkClient = (this.NetworkClient = this.instantiateNetworkClient(_protocolManager));
			}
			return platformNetworkClient;
		}

		// Token: 0x0600D158 RID: 53592 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void UserAdded(PlatformUserIdentifierAbs _id, bool _isPrimary)
		{
		}

		// Token: 0x0600D159 RID: 53593 RVA: 0x004C6207 File Offset: 0x004C4407
		public virtual string[] GetArgumentsForRelaunch()
		{
			return new string[0];
		}

		// Token: 0x0600D15A RID: 53594
		public abstract void CreateInstances();

		// Token: 0x0600D15B RID: 53595 RVA: 0x004C6210 File Offset: 0x004C4410
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

		// Token: 0x0600D15C RID: 53596 RVA: 0x000027FC File Offset: 0x000009FC
		public void LateUpdate()
		{
		}

		// Token: 0x0600D15D RID: 53597 RVA: 0x004C6284 File Offset: 0x004C4484
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

		// Token: 0x170019B3 RID: 6579
		// (get) Token: 0x0600D15E RID: 53598 RVA: 0x004C6424 File Offset: 0x004C4624
		public EPlatformIdentifier PlatformIdentifier { get; }

		// Token: 0x170019B4 RID: 6580
		// (get) Token: 0x0600D15F RID: 53599 RVA: 0x004C642C File Offset: 0x004C462C
		// (set) Token: 0x0600D160 RID: 53600 RVA: 0x004C6434 File Offset: 0x004C4634
		public IPlatformApi Api
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x170019B5 RID: 6581
		// (get) Token: 0x0600D161 RID: 53601 RVA: 0x004C643D File Offset: 0x004C463D
		// (set) Token: 0x0600D162 RID: 53602 RVA: 0x004C6445 File Offset: 0x004C4645
		public IUserClient User
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x170019B6 RID: 6582
		// (get) Token: 0x0600D163 RID: 53603 RVA: 0x004C644E File Offset: 0x004C464E
		// (set) Token: 0x0600D164 RID: 53604 RVA: 0x004C6456 File Offset: 0x004C4656
		public IUserClient UserServer
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x170019B7 RID: 6583
		// (get) Token: 0x0600D165 RID: 53605 RVA: 0x004C645F File Offset: 0x004C465F
		// (set) Token: 0x0600D166 RID: 53606 RVA: 0x004C6467 File Offset: 0x004C4667
		public IAuthenticationClient AuthenticationClient
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x170019B8 RID: 6584
		// (get) Token: 0x0600D167 RID: 53607 RVA: 0x004C6470 File Offset: 0x004C4670
		// (set) Token: 0x0600D168 RID: 53608 RVA: 0x004C6478 File Offset: 0x004C4678
		public IAuthenticationServer AuthenticationServer
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x170019B9 RID: 6585
		// (get) Token: 0x0600D169 RID: 53609 RVA: 0x004C6481 File Offset: 0x004C4681
		// (set) Token: 0x0600D16A RID: 53610 RVA: 0x004C6489 File Offset: 0x004C4689
		public IList<IServerListInterface> ServerListInterfaces
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x170019BA RID: 6586
		// (get) Token: 0x0600D16B RID: 53611 RVA: 0x004C6492 File Offset: 0x004C4692
		// (set) Token: 0x0600D16C RID: 53612 RVA: 0x004C649A File Offset: 0x004C469A
		public IServerListInterface ServerLookupInterface
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x170019BB RID: 6587
		// (get) Token: 0x0600D16D RID: 53613 RVA: 0x004C64A3 File Offset: 0x004C46A3
		// (set) Token: 0x0600D16E RID: 53614 RVA: 0x004C64AB File Offset: 0x004C46AB
		public IMasterServerAnnouncer ServerListAnnouncer
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x170019BC RID: 6588
		// (get) Token: 0x0600D16F RID: 53615 RVA: 0x004C64B4 File Offset: 0x004C46B4
		// (set) Token: 0x0600D170 RID: 53616 RVA: 0x004C64BC File Offset: 0x004C46BC
		public ILobbyHost LobbyHost
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x170019BD RID: 6589
		// (get) Token: 0x0600D171 RID: 53617 RVA: 0x004C64C5 File Offset: 0x004C46C5
		// (set) Token: 0x0600D172 RID: 53618 RVA: 0x004C64CD File Offset: 0x004C46CD
		public IPlayerInteractionsRecorder PlayerInteractionsRecorder
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x170019BE RID: 6590
		// (get) Token: 0x0600D173 RID: 53619 RVA: 0x004C64D6 File Offset: 0x004C46D6
		// (set) Token: 0x0600D174 RID: 53620 RVA: 0x004C64DE File Offset: 0x004C46DE
		public IGameplayNotifier GameplayNotifier
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x170019BF RID: 6591
		// (get) Token: 0x0600D175 RID: 53621 RVA: 0x004C64E7 File Offset: 0x004C46E7
		// (set) Token: 0x0600D176 RID: 53622 RVA: 0x004C64EF File Offset: 0x004C46EF
		public IList<IJoinSessionGameInviteListener> InviteListeners
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x170019C0 RID: 6592
		// (get) Token: 0x0600D177 RID: 53623 RVA: 0x004C64F8 File Offset: 0x004C46F8
		// (set) Token: 0x0600D178 RID: 53624 RVA: 0x004C6500 File Offset: 0x004C4700
		public IMultiplayerInvitationDialog MultiplayerInvitationDialog
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x170019C1 RID: 6593
		// (get) Token: 0x0600D179 RID: 53625 RVA: 0x004C6509 File Offset: 0x004C4709
		// (set) Token: 0x0600D17A RID: 53626 RVA: 0x004C6511 File Offset: 0x004C4711
		public IPartyVoice PartyVoice
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x170019C2 RID: 6594
		// (get) Token: 0x0600D17B RID: 53627 RVA: 0x004C651A File Offset: 0x004C471A
		// (set) Token: 0x0600D17C RID: 53628 RVA: 0x004C6522 File Offset: 0x004C4722
		public IUtils Utils
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x170019C3 RID: 6595
		// (get) Token: 0x0600D17D RID: 53629 RVA: 0x004C652B File Offset: 0x004C472B
		// (set) Token: 0x0600D17E RID: 53630 RVA: 0x004C6533 File Offset: 0x004C4733
		public IPlatformMemory Memory
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x170019C4 RID: 6596
		// (get) Token: 0x0600D17F RID: 53631 RVA: 0x004C653C File Offset: 0x004C473C
		// (set) Token: 0x0600D180 RID: 53632 RVA: 0x004C6544 File Offset: 0x004C4744
		public IAntiCheatClient AntiCheatClient
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x170019C5 RID: 6597
		// (get) Token: 0x0600D181 RID: 53633 RVA: 0x004C654D File Offset: 0x004C474D
		// (set) Token: 0x0600D182 RID: 53634 RVA: 0x004C6555 File Offset: 0x004C4755
		public IAntiCheatServer AntiCheatServer
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x170019C6 RID: 6598
		// (get) Token: 0x0600D183 RID: 53635 RVA: 0x004C655E File Offset: 0x004C475E
		// (set) Token: 0x0600D184 RID: 53636 RVA: 0x004C6566 File Offset: 0x004C4766
		public IUserIdentifierMappingService IdMappingService
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x170019C7 RID: 6599
		// (get) Token: 0x0600D185 RID: 53637 RVA: 0x004C656F File Offset: 0x004C476F
		// (set) Token: 0x0600D186 RID: 53638 RVA: 0x004C6577 File Offset: 0x004C4777
		public IUserDetailsService UserDetailsService
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x170019C8 RID: 6600
		// (get) Token: 0x0600D187 RID: 53639 RVA: 0x004C6580 File Offset: 0x004C4780
		// (set) Token: 0x0600D188 RID: 53640 RVA: 0x004C6588 File Offset: 0x004C4788
		public IPlayerReporting PlayerReporting
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x170019C9 RID: 6601
		// (get) Token: 0x0600D189 RID: 53641 RVA: 0x004C6591 File Offset: 0x004C4791
		// (set) Token: 0x0600D18A RID: 53642 RVA: 0x004C6599 File Offset: 0x004C4799
		public ITextCensor TextCensor
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x170019CA RID: 6602
		// (get) Token: 0x0600D18B RID: 53643 RVA: 0x004C65A2 File Offset: 0x004C47A2
		// (set) Token: 0x0600D18C RID: 53644 RVA: 0x004C65AA File Offset: 0x004C47AA
		public IRemoteFileStorage RemoteFileStorage
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x170019CB RID: 6603
		// (get) Token: 0x0600D18D RID: 53645 RVA: 0x004C65B3 File Offset: 0x004C47B3
		// (set) Token: 0x0600D18E RID: 53646 RVA: 0x004C65BB File Offset: 0x004C47BB
		public IRemotePlayerFileStorage RemotePlayerFileStorage
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x170019CC RID: 6604
		// (get) Token: 0x0600D18F RID: 53647 RVA: 0x004C65C4 File Offset: 0x004C47C4
		// (set) Token: 0x0600D190 RID: 53648 RVA: 0x004C65CC File Offset: 0x004C47CC
		public IList<IEntitlementValidator> EntitlementValidators
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x170019CD RID: 6605
		// (get) Token: 0x0600D191 RID: 53649 RVA: 0x004C65D5 File Offset: 0x004C47D5
		// (set) Token: 0x0600D192 RID: 53650 RVA: 0x004C65DD File Offset: 0x004C47DD
		public PlayerInputManager Input
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x170019CE RID: 6606
		// (get) Token: 0x0600D193 RID: 53651 RVA: 0x004C65E6 File Offset: 0x004C47E6
		// (set) Token: 0x0600D194 RID: 53652 RVA: 0x004C65EE File Offset: 0x004C47EE
		public IVirtualKeyboard VirtualKeyboard
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x170019CF RID: 6607
		// (get) Token: 0x0600D195 RID: 53653 RVA: 0x004C65F7 File Offset: 0x004C47F7
		// (set) Token: 0x0600D196 RID: 53654 RVA: 0x004C65FF File Offset: 0x004C47FF
		public IAchievementManager AchievementManager
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x170019D0 RID: 6608
		// (get) Token: 0x0600D197 RID: 53655 RVA: 0x004C6608 File Offset: 0x004C4808
		// (set) Token: 0x0600D198 RID: 53656 RVA: 0x004C6610 File Offset: 0x004C4810
		public IRichPresence RichPresence
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x170019D1 RID: 6609
		// (get) Token: 0x0600D199 RID: 53657 RVA: 0x004C6619 File Offset: 0x004C4819
		// (set) Token: 0x0600D19A RID: 53658 RVA: 0x004C6621 File Offset: 0x004C4821
		public IApplicationStateController ApplicationState
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x170019D2 RID: 6610
		// (get) Token: 0x0600D19B RID: 53659 RVA: 0x004C662A File Offset: 0x004C482A
		// (set) Token: 0x0600D19C RID: 53660 RVA: 0x004C6632 File Offset: 0x004C4832
		public IPlatformSaveGameProvider SaveGameProvider
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x170019D3 RID: 6611
		// (get) Token: 0x0600D19D RID: 53661 RVA: 0x004C663B File Offset: 0x004C483B
		// (set) Token: 0x0600D19E RID: 53662 RVA: 0x004C6643 File Offset: 0x004C4843
		public IUserDataRoaming UserDataRoaming
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x170019D4 RID: 6612
		// (get) Token: 0x0600D19F RID: 53663 RVA: 0x004C664C File Offset: 0x004C484C
		public virtual string NetworkProtocolName
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get;
		}

		// Token: 0x0400A072 RID: 41074
		[PublicizedFrom(EAccessModifier.Protected)]
		public IPlatformNetworkServer NetworkServer;

		// Token: 0x0400A073 RID: 41075
		[PublicizedFrom(EAccessModifier.Protected)]
		public IPlatformNetworkClient NetworkClient;
	}
}
