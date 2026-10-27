using System;
using System.Collections.Generic;
using Platform.LAN;
using Platform.Shared;
using Twitch;
using UnityEngine;
using UnityEngine.Scripting;

namespace Platform.XBL
{
	// Token: 0x02001C98 RID: 7320
	[Preserve]
	[PlatformFactory(EPlatformIdentifier.XBL)]
	public class Factory : AbsPlatform
	{
		// Token: 0x0600D8C6 RID: 55494 RVA: 0x004D9CBC File Offset: 0x004D7EBC
		public override void CreateInstances()
		{
			if (!base.AsServerOnly)
			{
				IPlatform crossplatformPlatform = PlatformManager.CrossplatformPlatform;
				if (crossplatformPlatform == null || crossplatformPlatform.PlatformIdentifier != EPlatformIdentifier.EOS)
				{
					Application.Quit(1);
					throw new Exception("[XBL] This platform requires EOS as cross platform provider");
				}
			}
			base.Api = new Api();
			base.Utils = new Platform.XBL.Utils();
			if (!base.AsServerOnly)
			{
				XblXuidMapper.Enable();
				base.User = new User();
				base.SaveGameProvider = new SaveGameProviderGameCore(4294967296L);
				base.UserDataRoaming = Platform.Shared.UserDataRoaming.OptionalSaveRoaming;
				base.ApplicationState = new ApplicationStateController();
				base.AchievementManager = new AchievementManager();
				base.Input = new PlayerInputManager();
				base.TextCensor = new TextCensor();
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
				base.ServerListInterfaces = new List<IServerListInterface>
				{
					new ServerListFriendsMultiplayerActivity(),
					new LANServerList()
				};
			}
		}
	}
}
