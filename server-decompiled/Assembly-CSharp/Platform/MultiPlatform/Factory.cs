using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace Platform.MultiPlatform
{
	// Token: 0x02001C95 RID: 7317
	[Preserve]
	public class Factory : IPlatform
	{
		// Token: 0x17001AB3 RID: 6835
		// (get) Token: 0x0600D8AA RID: 55466 RVA: 0x004DC5FB File Offset: 0x004DA7FB
		// (set) Token: 0x0600D8AB RID: 55467 RVA: 0x004DC603 File Offset: 0x004DA803
		public bool AsServerOnly { get; set; }

		// Token: 0x17001AB4 RID: 6836
		// (get) Token: 0x0600D8AC RID: 55468 RVA: 0x004DC60C File Offset: 0x004DA80C
		// (set) Token: 0x0600D8AD RID: 55469 RVA: 0x004DC614 File Offset: 0x004DA814
		public bool IsCrossplatform { get; set; }

		// Token: 0x17001AB5 RID: 6837
		// (get) Token: 0x0600D8AE RID: 55470 RVA: 0x00010E62 File Offset: 0x0000F062
		public EPlatformIdentifier PlatformIdentifier
		{
			get
			{
				return EPlatformIdentifier.None;
			}
		}

		// Token: 0x17001AB6 RID: 6838
		// (get) Token: 0x0600D8AF RID: 55471 RVA: 0x0001FF1E File Offset: 0x0001E11E
		public string PlatformDisplayName
		{
			get
			{
				return null;
			}
		}

		// Token: 0x0600D8B0 RID: 55472 RVA: 0x004DC61D File Offset: 0x004DA81D
		public void Init()
		{
			this.User.Init(this);
			this.ServerListAnnouncer.Init(this);
			this.RichPresence.Init(this);
			this.UserDataRoaming.Init(this);
		}

		// Token: 0x0600D8B1 RID: 55473 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool HasNetworkingEnabled(IList<string> _disabledProtocolNames)
		{
			return false;
		}

		// Token: 0x0600D8B2 RID: 55474 RVA: 0x000876FD File Offset: 0x000858FD
		public INetworkServer GetNetworkingServer(ProtocolManager _protocolManager)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600D8B3 RID: 55475 RVA: 0x000876FD File Offset: 0x000858FD
		public INetworkClient GetNetworkingClient(ProtocolManager _protocolManager)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600D8B4 RID: 55476 RVA: 0x004DC64F File Offset: 0x004DA84F
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

		// Token: 0x0600D8B5 RID: 55477 RVA: 0x004BFE13 File Offset: 0x004BE013
		public string[] GetArgumentsForRelaunch()
		{
			return new string[0];
		}

		// Token: 0x0600D8B6 RID: 55478 RVA: 0x004DC66E File Offset: 0x004DA86E
		public void CreateInstances()
		{
			this.User = new User();
			this.ServerListAnnouncer = new ServerListAnnouncer();
			this.RichPresence = new RichPresence();
			this.PlayerInteractionsRecorder = new PlayerInteractionsRecorderMulti();
			this.UserDataRoaming = new UserDataRoamingMultiPlatform();
		}

		// Token: 0x0600D8B7 RID: 55479 RVA: 0x000027FC File Offset: 0x000009FC
		public void Update()
		{
		}

		// Token: 0x0600D8B8 RID: 55480 RVA: 0x000027FC File Offset: 0x000009FC
		public void LateUpdate()
		{
		}

		// Token: 0x0600D8B9 RID: 55481 RVA: 0x004DC6A7 File Offset: 0x004DA8A7
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

		// Token: 0x17001AB7 RID: 6839
		// (get) Token: 0x0600D8BA RID: 55482 RVA: 0x0001FF1E File Offset: 0x0001E11E
		public IPlatformApi Api
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001AB8 RID: 6840
		// (get) Token: 0x0600D8BB RID: 55483 RVA: 0x004DC6C8 File Offset: 0x004DA8C8
		// (set) Token: 0x0600D8BC RID: 55484 RVA: 0x004DC6D0 File Offset: 0x004DA8D0
		public IUserClient User
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001AB9 RID: 6841
		// (get) Token: 0x0600D8BD RID: 55485 RVA: 0x004DC6D9 File Offset: 0x004DA8D9
		// (set) Token: 0x0600D8BE RID: 55486 RVA: 0x004DC6E1 File Offset: 0x004DA8E1
		public IUserClient UserServer
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001ABA RID: 6842
		// (get) Token: 0x0600D8BF RID: 55487 RVA: 0x0001FF1E File Offset: 0x0001E11E
		public IAuthenticationClient AuthenticationClient
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001ABB RID: 6843
		// (get) Token: 0x0600D8C0 RID: 55488 RVA: 0x0001FF1E File Offset: 0x0001E11E
		public IAuthenticationServer AuthenticationServer
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001ABC RID: 6844
		// (get) Token: 0x0600D8C1 RID: 55489 RVA: 0x004DC6EC File Offset: 0x004DA8EC
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

		// Token: 0x17001ABD RID: 6845
		// (get) Token: 0x0600D8C2 RID: 55490 RVA: 0x0001FF1E File Offset: 0x0001E11E
		public IServerListInterface ServerLookupInterface
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001ABE RID: 6846
		// (get) Token: 0x0600D8C3 RID: 55491 RVA: 0x004DC754 File Offset: 0x004DA954
		// (set) Token: 0x0600D8C4 RID: 55492 RVA: 0x004DC75C File Offset: 0x004DA95C
		public IMasterServerAnnouncer ServerListAnnouncer
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001ABF RID: 6847
		// (get) Token: 0x0600D8C5 RID: 55493 RVA: 0x0001FF1E File Offset: 0x0001E11E
		public ILobbyHost LobbyHost
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001AC0 RID: 6848
		// (get) Token: 0x0600D8C6 RID: 55494 RVA: 0x004DC765 File Offset: 0x004DA965
		// (set) Token: 0x0600D8C7 RID: 55495 RVA: 0x004DC76D File Offset: 0x004DA96D
		public IPlayerInteractionsRecorder PlayerInteractionsRecorder
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001AC1 RID: 6849
		// (get) Token: 0x0600D8C8 RID: 55496 RVA: 0x004DC776 File Offset: 0x004DA976
		// (set) Token: 0x0600D8C9 RID: 55497 RVA: 0x004DC77E File Offset: 0x004DA97E
		public IGameplayNotifier GameplayNotifier
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001AC2 RID: 6850
		// (get) Token: 0x0600D8CA RID: 55498 RVA: 0x004DC787 File Offset: 0x004DA987
		public IList<IJoinSessionGameInviteListener> InviteListeners
		{
			get
			{
				return PlatformManager.NativePlatform.InviteListeners;
			}
		}

		// Token: 0x17001AC3 RID: 6851
		// (get) Token: 0x0600D8CB RID: 55499 RVA: 0x004DC793 File Offset: 0x004DA993
		public IMultiplayerInvitationDialog MultiplayerInvitationDialog
		{
			get
			{
				return PlatformManager.NativePlatform.MultiplayerInvitationDialog;
			}
		}

		// Token: 0x17001AC4 RID: 6852
		// (get) Token: 0x0600D8CC RID: 55500 RVA: 0x004DC79F File Offset: 0x004DA99F
		public IPartyVoice PartyVoice
		{
			get
			{
				IPlatform crossplatformPlatform = PlatformManager.CrossplatformPlatform;
				return ((crossplatformPlatform != null) ? crossplatformPlatform.PartyVoice : null) ?? PlatformManager.NativePlatform.PartyVoice;
			}
		}

		// Token: 0x17001AC5 RID: 6853
		// (get) Token: 0x0600D8CD RID: 55501 RVA: 0x0001FF1E File Offset: 0x0001E11E
		public IUtils Utils
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001AC6 RID: 6854
		// (get) Token: 0x0600D8CE RID: 55502 RVA: 0x004DC7C0 File Offset: 0x004DA9C0
		public IPlatformMemory Memory
		{
			get
			{
				return PlatformManager.NativePlatform.Memory;
			}
		}

		// Token: 0x17001AC7 RID: 6855
		// (get) Token: 0x0600D8CF RID: 55503 RVA: 0x004DC7CC File Offset: 0x004DA9CC
		public IAntiCheatClient AntiCheatClient
		{
			get
			{
				IPlatform crossplatformPlatform = PlatformManager.CrossplatformPlatform;
				return ((crossplatformPlatform != null) ? crossplatformPlatform.AntiCheatClient : null) ?? PlatformManager.NativePlatform.AntiCheatClient;
			}
		}

		// Token: 0x17001AC8 RID: 6856
		// (get) Token: 0x0600D8D0 RID: 55504 RVA: 0x004DC7ED File Offset: 0x004DA9ED
		public IAntiCheatServer AntiCheatServer
		{
			get
			{
				IPlatform crossplatformPlatform = PlatformManager.CrossplatformPlatform;
				return ((crossplatformPlatform != null) ? crossplatformPlatform.AntiCheatServer : null) ?? PlatformManager.NativePlatform.AntiCheatServer;
			}
		}

		// Token: 0x17001AC9 RID: 6857
		// (get) Token: 0x0600D8D1 RID: 55505 RVA: 0x0001FF1E File Offset: 0x0001E11E
		public IUserIdentifierMappingService IdMappingService
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001ACA RID: 6858
		// (get) Token: 0x0600D8D2 RID: 55506 RVA: 0x0001FF1E File Offset: 0x0001E11E
		public IUserDetailsService UserDetailsService
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001ACB RID: 6859
		// (get) Token: 0x0600D8D3 RID: 55507 RVA: 0x004DC80E File Offset: 0x004DAA0E
		public IPlayerReporting PlayerReporting
		{
			get
			{
				IPlatform crossplatformPlatform = PlatformManager.CrossplatformPlatform;
				return ((crossplatformPlatform != null) ? crossplatformPlatform.PlayerReporting : null) ?? PlatformManager.NativePlatform.PlayerReporting;
			}
		}

		// Token: 0x17001ACC RID: 6860
		// (get) Token: 0x0600D8D4 RID: 55508 RVA: 0x004DC82F File Offset: 0x004DAA2F
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

		// Token: 0x17001ACD RID: 6861
		// (get) Token: 0x0600D8D5 RID: 55509 RVA: 0x004DC856 File Offset: 0x004DAA56
		public IRemoteFileStorage RemoteFileStorage
		{
			get
			{
				IPlatform crossplatformPlatform = PlatformManager.CrossplatformPlatform;
				return ((crossplatformPlatform != null) ? crossplatformPlatform.RemoteFileStorage : null) ?? PlatformManager.NativePlatform.RemoteFileStorage;
			}
		}

		// Token: 0x17001ACE RID: 6862
		// (get) Token: 0x0600D8D6 RID: 55510 RVA: 0x004DC877 File Offset: 0x004DAA77
		public IRemotePlayerFileStorage RemotePlayerFileStorage
		{
			get
			{
				IPlatform crossplatformPlatform = PlatformManager.CrossplatformPlatform;
				return ((crossplatformPlatform != null) ? crossplatformPlatform.RemotePlayerFileStorage : null) ?? PlatformManager.NativePlatform.RemotePlayerFileStorage;
			}
		}

		// Token: 0x17001ACF RID: 6863
		// (get) Token: 0x0600D8D7 RID: 55511 RVA: 0x004DC898 File Offset: 0x004DAA98
		public IList<IEntitlementValidator> EntitlementValidators
		{
			get
			{
				return PlatformManager.NativePlatform.EntitlementValidators;
			}
		}

		// Token: 0x17001AD0 RID: 6864
		// (get) Token: 0x0600D8D8 RID: 55512 RVA: 0x0001FF1E File Offset: 0x0001E11E
		public IPlatformNetworkServer NetworkServer
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001AD1 RID: 6865
		// (get) Token: 0x0600D8D9 RID: 55513 RVA: 0x0001FF1E File Offset: 0x0001E11E
		public PlayerInputManager Input
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001AD2 RID: 6866
		// (get) Token: 0x0600D8DA RID: 55514 RVA: 0x0001FF1E File Offset: 0x0001E11E
		public IVirtualKeyboard VirtualKeyboard
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001AD3 RID: 6867
		// (get) Token: 0x0600D8DB RID: 55515 RVA: 0x004DC8A4 File Offset: 0x004DAAA4
		public IPlatformSaveGameProvider SaveGameProvider
		{
			get
			{
				return PlatformManager.NativePlatform.SaveGameProvider;
			}
		}

		// Token: 0x17001AD4 RID: 6868
		// (get) Token: 0x0600D8DC RID: 55516 RVA: 0x004DC8B0 File Offset: 0x004DAAB0
		// (set) Token: 0x0600D8DD RID: 55517 RVA: 0x004DC8B8 File Offset: 0x004DAAB8
		public IUserDataRoaming UserDataRoaming
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001AD5 RID: 6869
		// (get) Token: 0x0600D8DE RID: 55518 RVA: 0x0001FF1E File Offset: 0x0001E11E
		public IAchievementManager AchievementManager
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001AD6 RID: 6870
		// (get) Token: 0x0600D8DF RID: 55519 RVA: 0x004DC8C1 File Offset: 0x004DAAC1
		// (set) Token: 0x0600D8E0 RID: 55520 RVA: 0x004DC8C9 File Offset: 0x004DAAC9
		public IRichPresence RichPresence
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001AD7 RID: 6871
		// (get) Token: 0x0600D8E1 RID: 55521 RVA: 0x004DC8D2 File Offset: 0x004DAAD2
		public IApplicationStateController ApplicationState
		{
			get
			{
				return PlatformManager.NativePlatform.ApplicationState;
			}
		}

		// Token: 0x0400A42C RID: 42028
		[PublicizedFrom(EAccessModifier.Private)]
		public List<IServerListInterface> serverListInterfaces;
	}
}
