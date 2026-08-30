using System;
using System.Collections.Generic;
using Platform.Shared;
using UnityEngine.Scripting;

namespace Platform.EOS
{
	// Token: 0x02001CCD RID: 7373
	[Preserve]
	[PlatformFactory(EPlatformIdentifier.EOS)]
	public class Factory : AbsPlatform
	{
		// Token: 0x0600DA42 RID: 55874 RVA: 0x004E3278 File Offset: 0x004E1478
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

		// Token: 0x0600DA43 RID: 55875 RVA: 0x004E339C File Offset: 0x004E159C
		public override bool HasNetworkingEnabled(IList<string> _disabledProtocolNames)
		{
			if (!GameManager.IsDedicatedServer && base.HasNetworkingEnabled(_disabledProtocolNames) && PlatformManager.NativePlatform.User.UserStatus == EUserStatus.LoggedIn && PlatformManager.NativePlatform.User.Permissions.HasMultiplayer())
			{
				User user = (User)base.User;
				return user != null && user.Permissions.HasMultiplayer();
			}
			return false;
		}

		// Token: 0x17001AF9 RID: 6905
		// (get) Token: 0x0600DA44 RID: 55876 RVA: 0x004E33FD File Offset: 0x004E15FD
		public override string NetworkProtocolName
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return "EosNetworking";
			}
		}

		// Token: 0x0600DA45 RID: 55877 RVA: 0x004E3404 File Offset: 0x004E1604
		[PublicizedFrom(EAccessModifier.Protected)]
		public override IPlatformNetworkServer instantiateNetworkServer(ProtocolManager _protocolManager)
		{
			return new NetworkServerEos(this, _protocolManager);
		}

		// Token: 0x0600DA46 RID: 55878 RVA: 0x004E340D File Offset: 0x004E160D
		[PublicizedFrom(EAccessModifier.Protected)]
		public override IPlatformNetworkClient instantiateNetworkClient(ProtocolManager _protocolManager)
		{
			return new NetworkClientEos(this, _protocolManager);
		}

		// Token: 0x0600DA47 RID: 55879 RVA: 0x004E3416 File Offset: 0x004E1616
		public override void Destroy()
		{
			base.Destroy();
			EosBindingsUnityEditor.Shutdown();
		}
	}
}
