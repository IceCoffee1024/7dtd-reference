using System;
using System.Collections.Generic;
using Platform.Shared;
using UnityEngine.Scripting;

namespace Platform.EOS
{
	// Token: 0x02001D26 RID: 7462
	[Preserve]
	[PlatformFactory(EPlatformIdentifier.EOS)]
	public class Factory : AbsPlatform
	{
		// Token: 0x0600DD58 RID: 56664 RVA: 0x004EE3D4 File Offset: 0x004EC5D4
		public override void CreateInstances()
		{
			EosBindingsUnityEditor.Init();
			base.Api = new Api();
			base.AuthenticationServer = new AuthServer();
			base.ServerListAnnouncer = new SessionsHost();
			IAntiCheatServer antiCheatServer2;
			if (!GameManager.IsDedicatedServer)
			{
				IAntiCheatServer antiCheatServer = new AntiCheatServerP2P();
				antiCheatServer2 = antiCheatServer;
			}
			else
			{
				IAntiCheatServer antiCheatServer = new AntiCheatServer();
				antiCheatServer2 = antiCheatServer;
			}
			base.AntiCheatServer = antiCheatServer2;
			if (!base.AsServerOnly && !GameManager.IsDedicatedServer)
			{
				base.User = new User();
				base.AuthenticationClient = new AuthClient();
				base.AntiCheatClient = new AntiCheatClientManager();
				base.PlayerReporting = new PlayerReporting();
				SessionsClient sessionsClient = new SessionsClient();
				base.ServerListInterfaces = new List<IServerListInterface>
				{
					sessionsClient,
					new FavoriteServers()
				};
				base.ServerLookupInterface = sessionsClient;
				base.PartyVoice = new Voice();
				base.RemotePlayerFileStorage = new RemotePlayerFileStorage();
				base.IdMappingService = new EosUserIdMapper((Api)base.Api, (User)base.User);
				base.UserDetailsService = new UserDetailsServiceEos();
			}
			else
			{
				base.UserServer = new UserServer();
			}
			if (!base.AsServerOnly)
			{
				base.RemoteFileStorage = new RemoteFileStorage();
			}
			AntiCheatCommon.Init();
		}

		// Token: 0x0600DD59 RID: 56665 RVA: 0x004EE4F8 File Offset: 0x004EC6F8
		public override bool HasNetworkingEnabled(IList<string> _disabledProtocolNames)
		{
			if (!GameManager.IsDedicatedServer && base.HasNetworkingEnabled(_disabledProtocolNames) && PlatformManager.NativePlatform.User.UserStatus == EUserStatus.LoggedIn && PlatformManager.NativePlatform.User.Permissions.HasMultiplayer())
			{
				User user = (User)base.User;
				return user != null && user.Permissions.HasMultiplayer();
			}
			return false;
		}

		// Token: 0x17001C11 RID: 7185
		// (get) Token: 0x0600DD5A RID: 56666 RVA: 0x004EE559 File Offset: 0x004EC759
		public override string NetworkProtocolName
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return "EosNetworking";
			}
		}

		// Token: 0x0600DD5B RID: 56667 RVA: 0x004EE560 File Offset: 0x004EC760
		[PublicizedFrom(EAccessModifier.Protected)]
		public override IPlatformNetworkServer instantiateNetworkServer(ProtocolManager _protocolManager)
		{
			return new NetworkServerEos(this, _protocolManager);
		}

		// Token: 0x0600DD5C RID: 56668 RVA: 0x004EE569 File Offset: 0x004EC769
		[PublicizedFrom(EAccessModifier.Protected)]
		public override IPlatformNetworkClient instantiateNetworkClient(ProtocolManager _protocolManager)
		{
			return new NetworkClientEos(this, _protocolManager);
		}

		// Token: 0x0600DD5D RID: 56669 RVA: 0x004EE572 File Offset: 0x004EC772
		public override void Destroy()
		{
			base.Destroy();
			EosBindingsUnityEditor.Shutdown();
		}
	}
}
