using System;
using System.Collections.Generic;
using Platform.Shared;
using UnityEngine.Scripting;

namespace Platform.EOS
{
	// Token: 0x02001CF4 RID: 7412
	[Preserve]
	[PlatformFactory(EPlatformIdentifier.EOS)]
	public class Factory : AbsPlatform
	{
		// Token: 0x0600DBDD RID: 56285 RVA: 0x004E988C File Offset: 0x004E7A8C
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

		// Token: 0x0600DBDE RID: 56286 RVA: 0x004E99B0 File Offset: 0x004E7BB0
		public override bool HasNetworkingEnabled(IList<string> _disabledProtocolNames)
		{
			if (!GameManager.IsDedicatedServer && base.HasNetworkingEnabled(_disabledProtocolNames) && PlatformManager.NativePlatform.User.UserStatus == EUserStatus.LoggedIn && PlatformManager.NativePlatform.User.Permissions.HasMultiplayer())
			{
				User user = (User)base.User;
				return user != null && user.Permissions.HasMultiplayer();
			}
			return false;
		}

		// Token: 0x17001B6B RID: 7019
		// (get) Token: 0x0600DBDF RID: 56287 RVA: 0x004E9A11 File Offset: 0x004E7C11
		public override string NetworkProtocolName
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return "EosNetworking";
			}
		}

		// Token: 0x0600DBE0 RID: 56288 RVA: 0x004E9A18 File Offset: 0x004E7C18
		[PublicizedFrom(EAccessModifier.Protected)]
		public override IPlatformNetworkServer instantiateNetworkServer(ProtocolManager _protocolManager)
		{
			return new NetworkServerEos(this, _protocolManager);
		}

		// Token: 0x0600DBE1 RID: 56289 RVA: 0x004E9A21 File Offset: 0x004E7C21
		[PublicizedFrom(EAccessModifier.Protected)]
		public override IPlatformNetworkClient instantiateNetworkClient(ProtocolManager _protocolManager)
		{
			return new NetworkClientEos(this, _protocolManager);
		}

		// Token: 0x0600DBE2 RID: 56290 RVA: 0x004E9A2A File Offset: 0x004E7C2A
		public override void Destroy()
		{
			base.Destroy();
			EosBindingsUnityEditor.Shutdown();
		}
	}
}
