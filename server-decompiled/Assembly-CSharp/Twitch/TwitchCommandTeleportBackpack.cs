using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x02001851 RID: 6225
	public class TwitchCommandTeleportBackpack : BaseTwitchCommand
	{
		// Token: 0x17001809 RID: 6153
		// (get) Token: 0x0600C055 RID: 49237 RVA: 0x000470B6 File Offset: 0x000452B6
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Mod;
			}
		}

		// Token: 0x1700180A RID: 6154
		// (get) Token: 0x0600C056 RID: 49238 RVA: 0x0046CD2F File Offset: 0x0046AF2F
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#tp_backpack", "#teleport_backpack" };
			}
		}

		// Token: 0x1700180B RID: 6155
		// (get) Token: 0x0600C057 RID: 49239 RVA: 0x0046CD47 File Offset: 0x0046AF47
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

		// Token: 0x0600C058 RID: 49240 RVA: 0x0046CD70 File Offset: 0x0046AF70
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

		// Token: 0x0600C059 RID: 49241 RVA: 0x0046CE0C File Offset: 0x0046B00C
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
