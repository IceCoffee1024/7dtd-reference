using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace Platform.MultiPlatform
{
	// Token: 0x02001D1F RID: 7455
	[Preserve]
	public class Factory : IPlatform
	{
		// Token: 0x17001C13 RID: 7187
		// (get) Token: 0x0600DCD0 RID: 56528 RVA: 0x004EB2DF File Offset: 0x004E94DF
		// (set) Token: 0x0600DCD1 RID: 56529 RVA: 0x004EB2E7 File Offset: 0x004E94E7
		public bool AsServerOnly { get; set; }

		// Token: 0x17001C14 RID: 7188
		// (get) Token: 0x0600DCD2 RID: 56530 RVA: 0x004EB2F0 File Offset: 0x004E94F0
		// (set) Token: 0x0600DCD3 RID: 56531 RVA: 0x004EB2F8 File Offset: 0x004E94F8
		public bool IsCrossplatform { get; set; }

		// Token: 0x17001C15 RID: 7189
		// (get) Token: 0x0600DCD4 RID: 56532 RVA: 0x00010E62 File Offset: 0x0000F062
		public EPlatformIdentifier PlatformIdentifier
		{
			get
			{
				return EPlatformIdentifier.None;
			}
		}

		// Token: 0x17001C16 RID: 7190
		// (get) Token: 0x0600DCD5 RID: 56533 RVA: 0x000200DA File Offset: 0x0001E2DA
		public string PlatformDisplayName
		{
			get
			{
				return null;
			}
		}

		// Token: 0x0600DCD6 RID: 56534 RVA: 0x004EB301 File Offset: 0x004E9501
		public void Init()
		{
			this.User.Init(this);
			this.ServerListAnnouncer.Init(this);
			this.RichPresence.Init(this);
			this.UserDataRoaming.Init(this);
		}

		// Token: 0x0600DCD7 RID: 56535 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool HasNetworkingEnabled(IList<string> _disabledProtocolNames)
		{
			return false;
		}

		// Token: 0x0600DCD8 RID: 56536 RVA: 0x000882A0 File Offset: 0x000864A0
		public INetworkServer GetNetworkingServer(ProtocolManager _protocolManager)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600DCD9 RID: 56537 RVA: 0x000882A0 File Offset: 0x000864A0
		public INetworkClient GetNetworkingClient(ProtocolManager _protocolManager)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600DCDA RID: 56538 RVA: 0x004EB333 File Offset: 0x004E9533
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

		// Token: 0x0600DCDB RID: 56539 RVA: 0x004CE817 File Offset: 0x004CCA17
		public string[] GetArgumentsForRelaunch()
		{
			return new string[0];
		}

		// Token: 0x0600DCDC RID: 56540 RVA: 0x004EB352 File Offset: 0x004E9552
		public void CreateInstances()
		{
			this.User = new User();
			this.ServerListAnnouncer = new ServerListAnnouncer();
			this.RichPresence = new RichPresence();
			this.PlayerInteractionsRecorder = new PlayerInteractionsRecorderMulti();
			this.UserDataRoaming = new UserDataRoamingMultiPlatform();
		}

		// Token: 0x0600DCDD RID: 56541 RVA: 0x000027FC File Offset: 0x000009FC
		public void Update()
		{
		}

		// Token: 0x0600DCDE RID: 56542 RVA: 0x000027FC File Offset: 0x000009FC
		public void LateUpdate()
		{
		}

		// Token: 0x0600DCDF RID: 56543 RVA: 0x004EB38B File Offset: 0x004E958B
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

		// Token: 0x17001C17 RID: 7191
		// (get) Token: 0x0600DCE0 RID: 56544 RVA: 0x000200DA File Offset: 0x0001E2DA
		public IPlatformApi Api
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001C18 RID: 7192
		// (get) Token: 0x0600DCE1 RID: 56545 RVA: 0x004EB3AC File Offset: 0x004E95AC
		// (set) Token: 0x0600DCE2 RID: 56546 RVA: 0x004EB3B4 File Offset: 0x004E95B4
		public IUserClient User
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001C19 RID: 7193
		// (get) Token: 0x0600DCE3 RID: 56547 RVA: 0x004EB3BD File Offset: 0x004E95BD
		// (set) Token: 0x0600DCE4 RID: 56548 RVA: 0x004EB3C5 File Offset: 0x004E95C5
		public IUserClient UserServer
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001C1A RID: 7194
		// (get) Token: 0x0600DCE5 RID: 56549 RVA: 0x000200DA File Offset: 0x0001E2DA
		public IAuthenticationClient AuthenticationClient
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001C1B RID: 7195
		// (get) Token: 0x0600DCE6 RID: 56550 RVA: 0x000200DA File Offset: 0x0001E2DA
		public IAuthenticationServer AuthenticationServer
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001C1C RID: 7196
		// (get) Token: 0x0600DCE7 RID: 56551 RVA: 0x004EB3D0 File Offset: 0x004E95D0
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

		// Token: 0x17001C1D RID: 7197
		// (get) Token: 0x0600DCE8 RID: 56552 RVA: 0x000200DA File Offset: 0x0001E2DA
		public IServerListInterface ServerLookupInterface
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001C1E RID: 7198
		// (get) Token: 0x0600DCE9 RID: 56553 RVA: 0x004EB438 File Offset: 0x004E9638
		// (set) Token: 0x0600DCEA RID: 56554 RVA: 0x004EB440 File Offset: 0x004E9640
		public IMasterServerAnnouncer ServerListAnnouncer
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001C1F RID: 7199
		// (get) Token: 0x0600DCEB RID: 56555 RVA: 0x000200DA File Offset: 0x0001E2DA
		public ILobbyHost LobbyHost
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001C20 RID: 7200
		// (get) Token: 0x0600DCEC RID: 56556 RVA: 0x004EB449 File Offset: 0x004E9649
		// (set) Token: 0x0600DCED RID: 56557 RVA: 0x004EB451 File Offset: 0x004E9651
		public IPlayerInteractionsRecorder PlayerInteractionsRecorder
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001C21 RID: 7201
		// (get) Token: 0x0600DCEE RID: 56558 RVA: 0x004EB45A File Offset: 0x004E965A
		// (set) Token: 0x0600DCEF RID: 56559 RVA: 0x004EB462 File Offset: 0x004E9662
		public IGameplayNotifier GameplayNotifier
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001C22 RID: 7202
		// (get) Token: 0x0600DCF0 RID: 56560 RVA: 0x004EB46B File Offset: 0x004E966B
		public IList<IJoinSessionGameInviteListener> InviteListeners
		{
			get
			{
				return PlatformManager.NativePlatform.InviteListeners;
			}
		}

		// Token: 0x17001C23 RID: 7203
		// (get) Token: 0x0600DCF1 RID: 56561 RVA: 0x004EB477 File Offset: 0x004E9677
		public IMultiplayerInvitationDialog MultiplayerInvitationDialog
		{
			get
			{
				return PlatformManager.NativePlatform.MultiplayerInvitationDialog;
			}
		}

		// Token: 0x17001C24 RID: 7204
		// (get) Token: 0x0600DCF2 RID: 56562 RVA: 0x004EB483 File Offset: 0x004E9683
		public IPartyVoice PartyVoice
		{
			get
			{
				IPlatform crossplatformPlatform = PlatformManager.CrossplatformPlatform;
				return ((crossplatformPlatform != null) ? crossplatformPlatform.PartyVoice : null) ?? PlatformManager.NativePlatform.PartyVoice;
			}
		}

		// Token: 0x17001C25 RID: 7205
		// (get) Token: 0x0600DCF3 RID: 56563 RVA: 0x000200DA File Offset: 0x0001E2DA
		public IUtils Utils
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001C26 RID: 7206
		// (get) Token: 0x0600DCF4 RID: 56564 RVA: 0x004EB4A4 File Offset: 0x004E96A4
		public IPlatformMemory Memory
		{
			get
			{
				return PlatformManager.NativePlatform.Memory;
			}
		}

		// Token: 0x17001C27 RID: 7207
		// (get) Token: 0x0600DCF5 RID: 56565 RVA: 0x004EB4B0 File Offset: 0x004E96B0
		public IAntiCheatClient AntiCheatClient
		{
			get
			{
				IPlatform crossplatformPlatform = PlatformManager.CrossplatformPlatform;
				return ((crossplatformPlatform != null) ? crossplatformPlatform.AntiCheatClient : null) ?? PlatformManager.NativePlatform.AntiCheatClient;
			}
		}

		// Token: 0x17001C28 RID: 7208
		// (get) Token: 0x0600DCF6 RID: 56566 RVA: 0x004EB4D1 File Offset: 0x004E96D1
		public IAntiCheatServer AntiCheatServer
		{
			get
			{
				IPlatform crossplatformPlatform = PlatformManager.CrossplatformPlatform;
				return ((crossplatformPlatform != null) ? crossplatformPlatform.AntiCheatServer : null) ?? PlatformManager.NativePlatform.AntiCheatServer;
			}
		}

		// Token: 0x17001C29 RID: 7209
		// (get) Token: 0x0600DCF7 RID: 56567 RVA: 0x000200DA File Offset: 0x0001E2DA
		public IUserIdentifierMappingService IdMappingService
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001C2A RID: 7210
		// (get) Token: 0x0600DCF8 RID: 56568 RVA: 0x000200DA File Offset: 0x0001E2DA
		public IUserDetailsService UserDetailsService
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001C2B RID: 7211
		// (get) Token: 0x0600DCF9 RID: 56569 RVA: 0x004EB4F2 File Offset: 0x004E96F2
		public IPlayerReporting PlayerReporting
		{
			get
			{
				IPlatform crossplatformPlatform = PlatformManager.CrossplatformPlatform;
				return ((crossplatformPlatform != null) ? crossplatformPlatform.PlayerReporting : null) ?? PlatformManager.NativePlatform.PlayerReporting;
			}
		}

		// Token: 0x17001C2C RID: 7212
		// (get) Token: 0x0600DCFA RID: 56570 RVA: 0x004EB513 File Offset: 0x004E9713
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

		// Token: 0x17001C2D RID: 7213
		// (get) Token: 0x0600DCFB RID: 56571 RVA: 0x004EB53A File Offset: 0x004E973A
		public IRemoteFileStorage RemoteFileStorage
		{
			get
			{
				IPlatform crossplatformPlatform = PlatformManager.CrossplatformPlatform;
				return ((crossplatformPlatform != null) ? crossplatformPlatform.RemoteFileStorage : null) ?? PlatformManager.NativePlatform.RemoteFileStorage;
			}
		}

		// Token: 0x17001C2E RID: 7214
		// (get) Token: 0x0600DCFC RID: 56572 RVA: 0x004EB55B File Offset: 0x004E975B
		public IRemotePlayerFileStorage RemotePlayerFileStorage
		{
			get
			{
				IPlatform crossplatformPlatform = PlatformManager.CrossplatformPlatform;
				return ((crossplatformPlatform != null) ? crossplatformPlatform.RemotePlayerFileStorage : null) ?? PlatformManager.NativePlatform.RemotePlayerFileStorage;
			}
		}

		// Token: 0x17001C2F RID: 7215
		// (get) Token: 0x0600DCFD RID: 56573 RVA: 0x004EB57C File Offset: 0x004E977C
		public IList<IEntitlementValidator> EntitlementValidators
		{
			get
			{
				return PlatformManager.NativePlatform.EntitlementValidators;
			}
		}

		// Token: 0x17001C30 RID: 7216
		// (get) Token: 0x0600DCFE RID: 56574 RVA: 0x000200DA File Offset: 0x0001E2DA
		public IPlatformNetworkServer NetworkServer
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001C31 RID: 7217
		// (get) Token: 0x0600DCFF RID: 56575 RVA: 0x000200DA File Offset: 0x0001E2DA
		public PlayerInputManager Input
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001C32 RID: 7218
		// (get) Token: 0x0600DD00 RID: 56576 RVA: 0x000200DA File Offset: 0x0001E2DA
		public IVirtualKeyboard VirtualKeyboard
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001C33 RID: 7219
		// (get) Token: 0x0600DD01 RID: 56577 RVA: 0x004EB588 File Offset: 0x004E9788
		public IPlatformSaveGameProvider SaveGameProvider
		{
			get
			{
				return PlatformManager.NativePlatform.SaveGameProvider;
			}
		}

		// Token: 0x17001C34 RID: 7220
		// (get) Token: 0x0600DD02 RID: 56578 RVA: 0x004EB594 File Offset: 0x004E9794
		// (set) Token: 0x0600DD03 RID: 56579 RVA: 0x004EB59C File Offset: 0x004E979C
		public IUserDataRoaming UserDataRoaming
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001C35 RID: 7221
		// (get) Token: 0x0600DD04 RID: 56580 RVA: 0x000200DA File Offset: 0x0001E2DA
		public IAchievementManager AchievementManager
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001C36 RID: 7222
		// (get) Token: 0x0600DD05 RID: 56581 RVA: 0x004EB5A5 File Offset: 0x004E97A5
		// (set) Token: 0x0600DD06 RID: 56582 RVA: 0x004EB5AD File Offset: 0x004E97AD
		public IRichPresence RichPresence
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001C37 RID: 7223
		// (get) Token: 0x0600DD07 RID: 56583 RVA: 0x004EB5B6 File Offset: 0x004E97B6
		public IApplicationStateController ApplicationState
		{
			get
			{
				return PlatformManager.NativePlatform.ApplicationState;
			}
		}

		// Token: 0x0400A71D RID: 42781
		[PublicizedFrom(EAccessModifier.Private)]
		public List<IServerListInterface> serverListInterfaces;
	}
}
