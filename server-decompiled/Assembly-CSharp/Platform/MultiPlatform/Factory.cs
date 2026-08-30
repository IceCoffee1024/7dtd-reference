using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace Platform.MultiPlatform
{
	// Token: 0x02001CBC RID: 7356
	[Preserve]
	public class Factory : IPlatform
	{
		// Token: 0x17001B25 RID: 6949
		// (get) Token: 0x0600DA42 RID: 55874 RVA: 0x004E2BF7 File Offset: 0x004E0DF7
		// (set) Token: 0x0600DA43 RID: 55875 RVA: 0x004E2BFF File Offset: 0x004E0DFF
		public bool AsServerOnly { get; set; }

		// Token: 0x17001B26 RID: 6950
		// (get) Token: 0x0600DA44 RID: 55876 RVA: 0x004E2C08 File Offset: 0x004E0E08
		// (set) Token: 0x0600DA45 RID: 55877 RVA: 0x004E2C10 File Offset: 0x004E0E10
		public bool IsCrossplatform { get; set; }

		// Token: 0x17001B27 RID: 6951
		// (get) Token: 0x0600DA46 RID: 55878 RVA: 0x00010E62 File Offset: 0x0000F062
		public EPlatformIdentifier PlatformIdentifier
		{
			get
			{
				return EPlatformIdentifier.None;
			}
		}

		// Token: 0x17001B28 RID: 6952
		// (get) Token: 0x0600DA47 RID: 55879 RVA: 0x0001FFE2 File Offset: 0x0001E1E2
		public string PlatformDisplayName
		{
			get
			{
				return null;
			}
		}

		// Token: 0x0600DA48 RID: 55880 RVA: 0x004E2C19 File Offset: 0x004E0E19
		public void Init()
		{
			this.User.Init(this);
			this.ServerListAnnouncer.Init(this);
			this.RichPresence.Init(this);
			this.UserDataRoaming.Init(this);
		}

		// Token: 0x0600DA49 RID: 55881 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool HasNetworkingEnabled(IList<string> _disabledProtocolNames)
		{
			return false;
		}

		// Token: 0x0600DA4A RID: 55882 RVA: 0x00087DB8 File Offset: 0x00085FB8
		public INetworkServer GetNetworkingServer(ProtocolManager _protocolManager)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600DA4B RID: 55883 RVA: 0x00087DB8 File Offset: 0x00085FB8
		public INetworkClient GetNetworkingClient(ProtocolManager _protocolManager)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600DA4C RID: 55884 RVA: 0x004E2C4B File Offset: 0x004E0E4B
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

		// Token: 0x0600DA4D RID: 55885 RVA: 0x004C6207 File Offset: 0x004C4407
		public string[] GetArgumentsForRelaunch()
		{
			return new string[0];
		}

		// Token: 0x0600DA4E RID: 55886 RVA: 0x004E2C6A File Offset: 0x004E0E6A
		public void CreateInstances()
		{
			this.User = new User();
			this.ServerListAnnouncer = new ServerListAnnouncer();
			this.RichPresence = new RichPresence();
			this.PlayerInteractionsRecorder = new PlayerInteractionsRecorderMulti();
			this.UserDataRoaming = new UserDataRoamingMultiPlatform();
		}

		// Token: 0x0600DA4F RID: 55887 RVA: 0x000027FC File Offset: 0x000009FC
		public void Update()
		{
		}

		// Token: 0x0600DA50 RID: 55888 RVA: 0x000027FC File Offset: 0x000009FC
		public void LateUpdate()
		{
		}

		// Token: 0x0600DA51 RID: 55889 RVA: 0x004E2CA3 File Offset: 0x004E0EA3
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

		// Token: 0x17001B29 RID: 6953
		// (get) Token: 0x0600DA52 RID: 55890 RVA: 0x0001FFE2 File Offset: 0x0001E1E2
		public IPlatformApi Api
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001B2A RID: 6954
		// (get) Token: 0x0600DA53 RID: 55891 RVA: 0x004E2CC4 File Offset: 0x004E0EC4
		// (set) Token: 0x0600DA54 RID: 55892 RVA: 0x004E2CCC File Offset: 0x004E0ECC
		public IUserClient User
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001B2B RID: 6955
		// (get) Token: 0x0600DA55 RID: 55893 RVA: 0x004E2CD5 File Offset: 0x004E0ED5
		// (set) Token: 0x0600DA56 RID: 55894 RVA: 0x004E2CDD File Offset: 0x004E0EDD
		public IUserClient UserServer
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001B2C RID: 6956
		// (get) Token: 0x0600DA57 RID: 55895 RVA: 0x0001FFE2 File Offset: 0x0001E1E2
		public IAuthenticationClient AuthenticationClient
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001B2D RID: 6957
		// (get) Token: 0x0600DA58 RID: 55896 RVA: 0x0001FFE2 File Offset: 0x0001E1E2
		public IAuthenticationServer AuthenticationServer
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001B2E RID: 6958
		// (get) Token: 0x0600DA59 RID: 55897 RVA: 0x004E2CE8 File Offset: 0x004E0EE8
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

		// Token: 0x17001B2F RID: 6959
		// (get) Token: 0x0600DA5A RID: 55898 RVA: 0x0001FFE2 File Offset: 0x0001E1E2
		public IServerListInterface ServerLookupInterface
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001B30 RID: 6960
		// (get) Token: 0x0600DA5B RID: 55899 RVA: 0x004E2D50 File Offset: 0x004E0F50
		// (set) Token: 0x0600DA5C RID: 55900 RVA: 0x004E2D58 File Offset: 0x004E0F58
		public IMasterServerAnnouncer ServerListAnnouncer
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001B31 RID: 6961
		// (get) Token: 0x0600DA5D RID: 55901 RVA: 0x0001FFE2 File Offset: 0x0001E1E2
		public ILobbyHost LobbyHost
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001B32 RID: 6962
		// (get) Token: 0x0600DA5E RID: 55902 RVA: 0x004E2D61 File Offset: 0x004E0F61
		// (set) Token: 0x0600DA5F RID: 55903 RVA: 0x004E2D69 File Offset: 0x004E0F69
		public IPlayerInteractionsRecorder PlayerInteractionsRecorder
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001B33 RID: 6963
		// (get) Token: 0x0600DA60 RID: 55904 RVA: 0x004E2D72 File Offset: 0x004E0F72
		// (set) Token: 0x0600DA61 RID: 55905 RVA: 0x004E2D7A File Offset: 0x004E0F7A
		public IGameplayNotifier GameplayNotifier
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001B34 RID: 6964
		// (get) Token: 0x0600DA62 RID: 55906 RVA: 0x004E2D83 File Offset: 0x004E0F83
		public IList<IJoinSessionGameInviteListener> InviteListeners
		{
			get
			{
				return PlatformManager.NativePlatform.InviteListeners;
			}
		}

		// Token: 0x17001B35 RID: 6965
		// (get) Token: 0x0600DA63 RID: 55907 RVA: 0x004E2D8F File Offset: 0x004E0F8F
		public IMultiplayerInvitationDialog MultiplayerInvitationDialog
		{
			get
			{
				return PlatformManager.NativePlatform.MultiplayerInvitationDialog;
			}
		}

		// Token: 0x17001B36 RID: 6966
		// (get) Token: 0x0600DA64 RID: 55908 RVA: 0x004E2D9B File Offset: 0x004E0F9B
		public IPartyVoice PartyVoice
		{
			get
			{
				IPlatform crossplatformPlatform = PlatformManager.CrossplatformPlatform;
				return ((crossplatformPlatform != null) ? crossplatformPlatform.PartyVoice : null) ?? PlatformManager.NativePlatform.PartyVoice;
			}
		}

		// Token: 0x17001B37 RID: 6967
		// (get) Token: 0x0600DA65 RID: 55909 RVA: 0x0001FFE2 File Offset: 0x0001E1E2
		public IUtils Utils
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001B38 RID: 6968
		// (get) Token: 0x0600DA66 RID: 55910 RVA: 0x004E2DBC File Offset: 0x004E0FBC
		public IPlatformMemory Memory
		{
			get
			{
				return PlatformManager.NativePlatform.Memory;
			}
		}

		// Token: 0x17001B39 RID: 6969
		// (get) Token: 0x0600DA67 RID: 55911 RVA: 0x004E2DC8 File Offset: 0x004E0FC8
		public IAntiCheatClient AntiCheatClient
		{
			get
			{
				IPlatform crossplatformPlatform = PlatformManager.CrossplatformPlatform;
				return ((crossplatformPlatform != null) ? crossplatformPlatform.AntiCheatClient : null) ?? PlatformManager.NativePlatform.AntiCheatClient;
			}
		}

		// Token: 0x17001B3A RID: 6970
		// (get) Token: 0x0600DA68 RID: 55912 RVA: 0x004E2DE9 File Offset: 0x004E0FE9
		public IAntiCheatServer AntiCheatServer
		{
			get
			{
				IPlatform crossplatformPlatform = PlatformManager.CrossplatformPlatform;
				return ((crossplatformPlatform != null) ? crossplatformPlatform.AntiCheatServer : null) ?? PlatformManager.NativePlatform.AntiCheatServer;
			}
		}

		// Token: 0x17001B3B RID: 6971
		// (get) Token: 0x0600DA69 RID: 55913 RVA: 0x0001FFE2 File Offset: 0x0001E1E2
		public IUserIdentifierMappingService IdMappingService
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001B3C RID: 6972
		// (get) Token: 0x0600DA6A RID: 55914 RVA: 0x0001FFE2 File Offset: 0x0001E1E2
		public IUserDetailsService UserDetailsService
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001B3D RID: 6973
		// (get) Token: 0x0600DA6B RID: 55915 RVA: 0x004E2E0A File Offset: 0x004E100A
		public IPlayerReporting PlayerReporting
		{
			get
			{
				IPlatform crossplatformPlatform = PlatformManager.CrossplatformPlatform;
				return ((crossplatformPlatform != null) ? crossplatformPlatform.PlayerReporting : null) ?? PlatformManager.NativePlatform.PlayerReporting;
			}
		}

		// Token: 0x17001B3E RID: 6974
		// (get) Token: 0x0600DA6C RID: 55916 RVA: 0x004E2E2B File Offset: 0x004E102B
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

		// Token: 0x17001B3F RID: 6975
		// (get) Token: 0x0600DA6D RID: 55917 RVA: 0x004E2E52 File Offset: 0x004E1052
		public IRemoteFileStorage RemoteFileStorage
		{
			get
			{
				IPlatform crossplatformPlatform = PlatformManager.CrossplatformPlatform;
				return ((crossplatformPlatform != null) ? crossplatformPlatform.RemoteFileStorage : null) ?? PlatformManager.NativePlatform.RemoteFileStorage;
			}
		}

		// Token: 0x17001B40 RID: 6976
		// (get) Token: 0x0600DA6E RID: 55918 RVA: 0x004E2E73 File Offset: 0x004E1073
		public IRemotePlayerFileStorage RemotePlayerFileStorage
		{
			get
			{
				IPlatform crossplatformPlatform = PlatformManager.CrossplatformPlatform;
				return ((crossplatformPlatform != null) ? crossplatformPlatform.RemotePlayerFileStorage : null) ?? PlatformManager.NativePlatform.RemotePlayerFileStorage;
			}
		}

		// Token: 0x17001B41 RID: 6977
		// (get) Token: 0x0600DA6F RID: 55919 RVA: 0x004E2E94 File Offset: 0x004E1094
		public IList<IEntitlementValidator> EntitlementValidators
		{
			get
			{
				return PlatformManager.NativePlatform.EntitlementValidators;
			}
		}

		// Token: 0x17001B42 RID: 6978
		// (get) Token: 0x0600DA70 RID: 55920 RVA: 0x0001FFE2 File Offset: 0x0001E1E2
		public IPlatformNetworkServer NetworkServer
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001B43 RID: 6979
		// (get) Token: 0x0600DA71 RID: 55921 RVA: 0x0001FFE2 File Offset: 0x0001E1E2
		public PlayerInputManager Input
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001B44 RID: 6980
		// (get) Token: 0x0600DA72 RID: 55922 RVA: 0x0001FFE2 File Offset: 0x0001E1E2
		public IVirtualKeyboard VirtualKeyboard
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001B45 RID: 6981
		// (get) Token: 0x0600DA73 RID: 55923 RVA: 0x004E2EA0 File Offset: 0x004E10A0
		public IPlatformSaveGameProvider SaveGameProvider
		{
			get
			{
				return PlatformManager.NativePlatform.SaveGameProvider;
			}
		}

		// Token: 0x17001B46 RID: 6982
		// (get) Token: 0x0600DA74 RID: 55924 RVA: 0x004E2EAC File Offset: 0x004E10AC
		// (set) Token: 0x0600DA75 RID: 55925 RVA: 0x004E2EB4 File Offset: 0x004E10B4
		public IUserDataRoaming UserDataRoaming
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001B47 RID: 6983
		// (get) Token: 0x0600DA76 RID: 55926 RVA: 0x0001FFE2 File Offset: 0x0001E1E2
		public IAchievementManager AchievementManager
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001B48 RID: 6984
		// (get) Token: 0x0600DA77 RID: 55927 RVA: 0x004E2EBD File Offset: 0x004E10BD
		// (set) Token: 0x0600DA78 RID: 55928 RVA: 0x004E2EC5 File Offset: 0x004E10C5
		public IRichPresence RichPresence
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001B49 RID: 6985
		// (get) Token: 0x0600DA79 RID: 55929 RVA: 0x004E2ECE File Offset: 0x004E10CE
		public IApplicationStateController ApplicationState
		{
			get
			{
				return PlatformManager.NativePlatform.ApplicationState;
			}
		}

		// Token: 0x0400A55F RID: 42335
		[PublicizedFrom(EAccessModifier.Private)]
		public List<IServerListInterface> serverListInterfaces;
	}
}
