using System;
using System.Collections.Generic;
using Platform.LAN;
using Twitch;
using UnityEngine.Scripting;

namespace Platform.Steam
{
	// Token: 0x02001CB9 RID: 7353
	[Preserve]
	[PlatformFactory(EPlatformIdentifier.Steam)]
	public class Factory : AbsPlatform
	{
		// Token: 0x0600DA00 RID: 55808 RVA: 0x004E1910 File Offset: 0x004DFB10
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

		// Token: 0x17001B85 RID: 7045
		// (get) Token: 0x0600DA01 RID: 55809 RVA: 0x004E1A88 File Offset: 0x004DFC88
		public override string NetworkProtocolName
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return "SteamNetworking";
			}
		}

		// Token: 0x0600DA02 RID: 55810 RVA: 0x004E1A8F File Offset: 0x004DFC8F
		[PublicizedFrom(EAccessModifier.Protected)]
		public override IPlatformNetworkServer instantiateNetworkServer(ProtocolManager _protocolManager)
		{
			return new NetworkServerSteam(this, _protocolManager);
		}

		// Token: 0x0600DA03 RID: 55811 RVA: 0x004E1A98 File Offset: 0x004DFC98
		[PublicizedFrom(EAccessModifier.Protected)]
		public override IPlatformNetworkClient instantiateNetworkClient(ProtocolManager _protocolManager)
		{
			return new NetworkClientSteam(this, _protocolManager);
		}
	}
}
