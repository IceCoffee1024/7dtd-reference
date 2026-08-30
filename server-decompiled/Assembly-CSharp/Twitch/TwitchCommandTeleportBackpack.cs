using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x02001825 RID: 6181
	public class TwitchCommandTeleportBackpack : BaseTwitchCommand
	{
		// Token: 0x17001767 RID: 5991
		// (get) Token: 0x0600BEFD RID: 48893 RVA: 0x00046EDA File Offset: 0x000450DA
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Mod;
			}
		}

		// Token: 0x17001768 RID: 5992
		// (get) Token: 0x0600BEFE RID: 48894 RVA: 0x004687F3 File Offset: 0x004669F3
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#tp_backpack", "#teleport_backpack" };
			}
		}

		// Token: 0x17001769 RID: 5993
		// (get) Token: 0x0600BEFF RID: 48895 RVA: 0x0046880B File Offset: 0x00466A0B
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[]
				{
					Localization.Get("TwitchCommand_TeleportBackpack1", false, null),
					Localization.Get("TwitchCommand_TeleportBackpack2", false, null)
				};
			}
		}

		// Token: 0x0600BF00 RID: 48896 RVA: 0x00468834 File Offset: 0x00466A34
		public override void Execute(ViewerEntry entry, TwitchIRCClient.TwitchChatMessage message)
		{
			string[] array = message.Message.Split(' ', StringSplitOptions.None);
			EntityPlayer entityPlayer = TwitchManager.Current.LocalPlayer;
			if (array.Length != 2)
			{
				GameEventManager.Current.HandleAction("action_teleport_backpack", entityPlayer, entityPlayer, false, "", "", false, true, "", null);
				return;
			}
			int num = -1;
			if (StringParsers.TryParseSInt32(array[1], out num) && TwitchManager.Current.LocalPlayer.Party != null)
			{
				entityPlayer = TwitchManager.Current.LocalPlayer.Party.GetMemberAtIndex(num, TwitchManager.Current.LocalPlayer);
				entityPlayer == null;
				return;
			}
		}

		// Token: 0x0600BF01 RID: 48897 RVA: 0x004688D0 File Offset: 0x00466AD0
		public override void ExecuteConsole(List<string> arguments)
		{
			EntityPlayer entityPlayer = TwitchManager.Current.LocalPlayer;
			if (arguments.Count != 2)
			{
				GameEventManager.Current.HandleAction("action_teleport_backpack", entityPlayer, entityPlayer, false, "", "", false, true, "", null);
				return;
			}
			int num = -1;
			if (StringParsers.TryParseSInt32(arguments[1], out num) && TwitchManager.Current.LocalPlayer.Party != null)
			{
				entityPlayer = TwitchManager.Current.LocalPlayer.Party.GetMemberAtIndex(num, TwitchManager.Current.LocalPlayer);
				entityPlayer == null;
				return;
			}
		}
	}
}
