using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x02001882 RID: 6274
	public class TwitchCommandTeleportBackpack : BaseTwitchCommand
	{
		// Token: 0x17001851 RID: 6225
		// (get) Token: 0x0600C169 RID: 49513 RVA: 0x00047076 File Offset: 0x00045276
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Mod;
			}
		}

		// Token: 0x17001852 RID: 6226
		// (get) Token: 0x0600C16A RID: 49514 RVA: 0x004706DB File Offset: 0x0046E8DB
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#tp_backpack", "#teleport_backpack" };
			}
		}

		// Token: 0x17001853 RID: 6227
		// (get) Token: 0x0600C16B RID: 49515 RVA: 0x004706F3 File Offset: 0x0046E8F3
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

		// Token: 0x0600C16C RID: 49516 RVA: 0x0047071C File Offset: 0x0046E91C
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

		// Token: 0x0600C16D RID: 49517 RVA: 0x004707B8 File Offset: 0x0046E9B8
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
