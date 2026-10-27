using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x02001869 RID: 6249
	public class BaseTwitchCommand
	{
		// Token: 0x1700180C RID: 6156
		// (get) Token: 0x0600C0D6 RID: 49366 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Everyone;
			}
		}

		// Token: 0x1700180D RID: 6157
		// (get) Token: 0x0600C0D7 RID: 49367 RVA: 0x000200DA File Offset: 0x0001E2DA
		public virtual string[] CommandText
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700180E RID: 6158
		// (get) Token: 0x0600C0D8 RID: 49368 RVA: 0x000200DA File Offset: 0x0001E2DA
		public virtual string[] LocalizedCommandNames
		{
			get
			{
				return null;
			}
		}

		// Token: 0x0600C0DA RID: 49370 RVA: 0x0046EE66 File Offset: 0x0046D066
		public static void ClearCommandPermissionOverrides()
		{
			BaseTwitchCommand.CommandPermissionOverrides.Clear();
		}

		// Token: 0x0600C0DB RID: 49371 RVA: 0x0046EE72 File Offset: 0x0046D072
		public static void AddCommandPermissionOverride(string commandName, BaseTwitchCommand.PermissionLevels permissionLevel)
		{
			if (!BaseTwitchCommand.CommandPermissionOverrides.ContainsKey(commandName))
			{
				BaseTwitchCommand.CommandPermissionOverrides.Add(commandName, permissionLevel);
			}
		}

		// Token: 0x0600C0DC RID: 49372 RVA: 0x0046EE8D File Offset: 0x0046D08D
		public static BaseTwitchCommand.PermissionLevels GetPermission(BaseTwitchCommand cmd)
		{
			if (BaseTwitchCommand.CommandPermissionOverrides.ContainsKey(cmd.CommandText[0]))
			{
				return BaseTwitchCommand.CommandPermissionOverrides[cmd.CommandText[0]];
			}
			return cmd.RequiredPermission;
		}

		// Token: 0x0600C0DD RID: 49373 RVA: 0x0046EEBC File Offset: 0x0046D0BC
		public BaseTwitchCommand()
		{
			this.SetupCommandTextList();
			if (BaseTwitchCommand.allText == "")
			{
				BaseTwitchCommand.allText = Localization.Get("lblAll", false, null);
			}
		}

		// Token: 0x0600C0DE RID: 49374 RVA: 0x0046EEF8 File Offset: 0x0046D0F8
		[PublicizedFrom(EAccessModifier.Private)]
		public void SetupCommandTextList()
		{
			this.CommandTextList.AddRange(this.CommandText);
			string[] localizedCommandNames = this.LocalizedCommandNames;
			for (int i = 0; i < localizedCommandNames.Length; i++)
			{
				if (!this.CommandTextList.Contains(localizedCommandNames[i]))
				{
					this.CommandTextList.Add(localizedCommandNames[i]);
				}
			}
		}

		// Token: 0x0600C0DF RID: 49375 RVA: 0x0046EF4C File Offset: 0x0046D14C
		public virtual bool CheckAllowed(TwitchIRCClient.TwitchChatMessage message)
		{
			BaseTwitchCommand.PermissionLevels permission = BaseTwitchCommand.GetPermission(this);
			if (permission == BaseTwitchCommand.PermissionLevels.Everyone)
			{
				return true;
			}
			if (permission == BaseTwitchCommand.PermissionLevels.Mod)
			{
				return message.isMod;
			}
			if (permission == BaseTwitchCommand.PermissionLevels.Broadcaster)
			{
				return message.isBroadcaster;
			}
			if (permission == BaseTwitchCommand.PermissionLevels.VIP)
			{
				return message.isVIP;
			}
			return permission == BaseTwitchCommand.PermissionLevels.Sub && message.isSub;
		}

		// Token: 0x0600C0E0 RID: 49376 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void Execute(ViewerEntry entry, TwitchIRCClient.TwitchChatMessage message)
		{
		}

		// Token: 0x0600C0E1 RID: 49377 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void ExecuteConsole(List<string> arguments)
		{
		}

		// Token: 0x04009101 RID: 37121
		public List<string> CommandTextList = new List<string>();

		// Token: 0x04009102 RID: 37122
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string allText = "";

		// Token: 0x04009103 RID: 37123
		[PublicizedFrom(EAccessModifier.Protected)]
		public static Dictionary<string, BaseTwitchCommand.PermissionLevels> CommandPermissionOverrides = new Dictionary<string, BaseTwitchCommand.PermissionLevels>();

		// Token: 0x0200186A RID: 6250
		public enum PermissionLevels
		{
			// Token: 0x04009105 RID: 37125
			Everyone,
			// Token: 0x04009106 RID: 37126
			VIP,
			// Token: 0x04009107 RID: 37127
			Sub,
			// Token: 0x04009108 RID: 37128
			Mod,
			// Token: 0x04009109 RID: 37129
			Broadcaster
		}
	}
}
