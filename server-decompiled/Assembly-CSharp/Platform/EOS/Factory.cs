using System;
using System.Collections.Generic;
using Platform.Shared;
using UnityEngine.Scripting;

namespace Platform.EOS
{
	// Token: 0x02001D57 RID: 7511
	[Preserve]
	[PlatformFactory(EPlatformIdentifier.EOS)]
	public class Factory : AbsPlatform
	{
		// Token: 0x0600DE6C RID: 56940 RVA: 0x004F1F74 File Offset: 0x004F0174
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

		// Token: 0x0600DE6D RID: 56941 RVA: 0x004F2098 File Offset: 0x004F0298
		public override bool HasNetworkingEnabled(IList<string> _disabledProtocolNames)
		{
			if (!GameManager.IsDedicatedServer && base.HasNetworkingEnabled(_disabledProtocolNames) && PlatformManager.NativePlatform.User.UserStatus == EUserStatus.LoggedIn && PlatformManager.NativePlatform.User.Permissions.HasMultiplayer())
			{
				User user = (User)base.User;
				return user != null && user.Permissions.HasMultiplayer();
			}
			return false;
		}

		// Token: 0x17001C59 RID: 7257
		// (get) Token: 0x0600DE6E RID: 56942 RVA: 0x004F20F9 File Offset: 0x004F02F9
		public override string NetworkProtocolName
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return "EosNetworking";
			}
		}

		// Token: 0x0600DE6F RID: 56943 RVA: 0x004F2100 File Offset: 0x004F0300
		[PublicizedFrom(EAccessModifier.Protected)]
		public override IPlatformNetworkServer instantiateNetworkServer(ProtocolManager _protocolManager)
		{
			return new NetworkServerEos(this, _protocolManager);
		}

		// Token: 0x0600DE70 RID: 56944 RVA: 0x004F2109 File Offset: 0x004F0309
		[PublicizedFrom(EAccessModifier.Protected)]
		public override IPlatformNetworkClient instantiateNetworkClient(ProtocolManager _protocolManager)
		{
			return new NetworkClientEos(this, _protocolManager);
		}

		// Token: 0x0600DE71 RID: 56945 RVA: 0x004F2112 File Offset: 0x004F0312
		public override void Destroy()
		{
			base.Destroy();
			EosBindingsUnityEditor.Shutdown();
		}
	}
}
