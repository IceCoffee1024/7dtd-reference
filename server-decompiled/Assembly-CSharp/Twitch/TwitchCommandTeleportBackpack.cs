using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x020017FF RID: 6143
	public class TwitchCommandTeleportBackpack : BaseTwitchCommand
	{
		// Token: 0x170016F5 RID: 5877
		// (get) Token: 0x0600BD78 RID: 48504 RVA: 0x00046C7A File Offset: 0x00044E7A
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Mod;
			}
		}

		// Token: 0x170016F6 RID: 5878
		// (get) Token: 0x0600BD79 RID: 48505 RVA: 0x00462A57 File Offset: 0x00460C57
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#tp_backpack", "#teleport_backpack" };
			}
		}

		// Token: 0x170016F7 RID: 5879
		// (get) Token: 0x0600BD7A RID: 48506 RVA: 0x00462A6F File Offset: 0x00460C6F
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

		// Token: 0x0600BD7B RID: 48507 RVA: 0x00462A98 File Offset: 0x00460C98
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

		// Token: 0x0600BD7C RID: 48508 RVA: 0x00462B34 File Offset: 0x00460D34
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
