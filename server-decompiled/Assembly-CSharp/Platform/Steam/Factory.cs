using System;
using System.Collections.Generic;
using Platform.LAN;
using Twitch;
using UnityEngine.Scripting;

namespace Platform.Steam
{
	// Token: 0x02001C60 RID: 7264
	[Preserve]
	[PlatformFactory(EPlatformIdentifier.Steam)]
	public class Factory : AbsPlatform
	{
		// Token: 0x0600D6F4 RID: 55028 RVA: 0x004D68AC File Offset: 0x004D4AAC
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

		// Token: 0x17001A6D RID: 6765
		// (get) Token: 0x0600D6F5 RID: 55029 RVA: 0x004D6A24 File Offset: 0x004D4C24
		public override string NetworkProtocolName
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return "SteamNetworking";
			}
		}

		// Token: 0x0600D6F6 RID: 55030 RVA: 0x004D6A2B File Offset: 0x004D4C2B
		[PublicizedFrom(EAccessModifier.Protected)]
		public override IPlatformNetworkServer instantiateNetworkServer(ProtocolManager _protocolManager)
		{
			return new NetworkServerSteam(this, _protocolManager);
		}

		// Token: 0x0600D6F7 RID: 55031 RVA: 0x004D6A34 File Offset: 0x004D4C34
		[PublicizedFrom(EAccessModifier.Protected)]
		public override IPlatformNetworkClient instantiateNetworkClient(ProtocolManager _protocolManager)
		{
			return new NetworkClientSteam(this, _protocolManager);
		}
	}
}
