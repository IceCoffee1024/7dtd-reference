using System;
using System.Collections.Generic;
using Platform.LAN;
using Twitch;
using UnityEngine.Scripting;

namespace Platform.Steam
{
	// Token: 0x02001C87 RID: 7303
	[Preserve]
	[PlatformFactory(EPlatformIdentifier.Steam)]
	public class Factory : AbsPlatform
	{
		// Token: 0x0600D886 RID: 55430 RVA: 0x004DCDC8 File Offset: 0x004DAFC8
		public override void CreateInstances()
		{
			base.Api = new Api();
			base.User = new User();
			base.AuthenticationServer = new AuthenticationServer();
			base.ServerListAnnouncer = new MasterServerAnnouncer();
			base.LobbyHost = new LobbyHost();
			base.MultiplayerInvitationDialog = new MultiplayerInvitationDialogSteam();
			base.Utils = new Utils();
			if (!base.AsServerOnly && !GameManager.IsDedicatedServer)
			{
				base.AchievementManager = new AchievementManager();
				base.RichPresence = new RichPresence();
				base.InviteListeners = new List<IJoinSessionGameInviteListener>
				{
					new JoinSessionGameInviteListener(),
					DiscordInviteListener.ListenerInstance
				};
				base.EntitlementValidators = new List<IEntitlementValidator>
				{
					new DownloadableContentValidator(),
					new TwitchEntitlementManager()
				};
				base.AuthenticationClient = new AuthenticationClient();
				base.ServerListInterfaces = new List<IServerListInterface>
				{
					new LobbyListInternet(),
					new LobbyListFriends(),
					new LANServerList()
				};
				if (PlatformManager.CrossplatformPlatform == null)
				{
					base.ServerListInterfaces.Add(new MasterServerList(EServerRelationType.Internet));
					base.ServerListInterfaces.Add(new MasterServerList(EServerRelationType.LAN));
					base.ServerListInterfaces.Add(new MasterServerList(EServerRelationType.Friends));
					base.ServerListInterfaces.Add(new MasterServerList(EServerRelationType.Favorites));
					base.ServerListInterfaces.Add(new MasterServerList(EServerRelationType.History));
				}
				base.VirtualKeyboard = new VirtualKeyboard();
			}
			if (!base.AsServerOnly)
			{
				base.Input = new PlayerInputManager();
			}
		}

		// Token: 0x17001ADF RID: 6879
		// (get) Token: 0x0600D887 RID: 55431 RVA: 0x004DCF40 File Offset: 0x004DB140
		public override string NetworkProtocolName
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return "SteamNetworking";
			}
		}

		// Token: 0x0600D888 RID: 55432 RVA: 0x004DCF47 File Offset: 0x004DB147
		[PublicizedFrom(EAccessModifier.Protected)]
		public override IPlatformNetworkServer instantiateNetworkServer(ProtocolManager _protocolManager)
		{
			return new NetworkServerSteam(this, _protocolManager);
		}

		// Token: 0x0600D889 RID: 55433 RVA: 0x004DCF50 File Offset: 0x004DB150
		[PublicizedFrom(EAccessModifier.Protected)]
		public override IPlatformNetworkClient instantiateNetworkClient(ProtocolManager _protocolManager)
		{
			return new NetworkClientSteam(this, _protocolManager);
		}
	}
}
