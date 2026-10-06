using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x02001838 RID: 6200
	public class BaseTwitchCommand
	{
		// Token: 0x170017C4 RID: 6084
		// (get) Token: 0x0600BFC2 RID: 49090 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Everyone;
			}
		}

		// Token: 0x170017C5 RID: 6085
		// (get) Token: 0x0600BFC3 RID: 49091 RVA: 0x000200DA File Offset: 0x0001E2DA
		public virtual string[] CommandText
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170017C6 RID: 6086
		// (get) Token: 0x0600BFC4 RID: 49092 RVA: 0x000200DA File Offset: 0x0001E2DA
		public virtual string[] LocalizedCommandNames
		{
			get
			{
				return null;
			}
		}

		// Token: 0x0600BFC6 RID: 49094 RVA: 0x0046B4BA File Offset: 0x004696BA
		public static void ClearCommandPermissionOverrides()
		{
			BaseTwitchCommand.CommandPermissionOverrides.Clear();
		}

		// Token: 0x0600BFC7 RID: 49095 RVA: 0x0046B4C6 File Offset: 0x004696C6
		public static void AddCommandPermissionOverride(string commandName, BaseTwitchCommand.PermissionLevels permissionLevel)
		{
			if (!BaseTwitchCommand.CommandPermissionOverrides.ContainsKey(commandName))
			{
				BaseTwitchCommand.CommandPermissionOverrides.Add(commandName, permissionLevel);
			}
		}

		// Token: 0x0600BFC8 RID: 49096 RVA: 0x0046B4E1 File Offset: 0x004696E1
		public static BaseTwitchCommand.PermissionLevels GetPermission(BaseTwitchCommand cmd)
		{
			if (BaseTwitchCommand.CommandPermissionOverrides.ContainsKey(cmd.CommandText[0]))
			{
				return BaseTwitchCommand.CommandPermissionOverrides[cmd.CommandText[0]];
			}
			return cmd.RequiredPermission;
		}

		// Token: 0x0600BFC9 RID: 49097 RVA: 0x0046B510 File Offset: 0x00469710
		public BaseTwitchCommand()
		{
			this.SetupCommandTextList();
			if (BaseTwitchCommand.allText == "")
			{
				BaseTwitchCommand.allText = Localization.Get("lblAll", false, null);
			}
		}

		// Token: 0x0600BFCA RID: 49098 RVA: 0x0046B54C File Offset: 0x0046974C
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

		// Token: 0x0600BFCB RID: 49099 RVA: 0x0046B5A0 File Offset: 0x004697A0
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

		// Token: 0x0600BFCC RID: 49100 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void Execute(ViewerEntry entry, TwitchIRCClient.TwitchChatMessage message)
		{
		}

		// Token: 0x0600BFCD RID: 49101 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void ExecuteConsole(List<string> arguments)
		{
		}

		// Token: 0x04009018 RID: 36888
		public List<string> CommandTextList = new List<string>();

		// Token: 0x04009019 RID: 36889
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string allText = "";

		// Token: 0x0400901A RID: 36890
		[PublicizedFrom(EAccessModifier.Protected)]
		public static Dictionary<string, BaseTwitchCommand.PermissionLevels> CommandPermissionOverrides = new Dictionary<string, BaseTwitchCommand.PermissionLevels>();

		// Token: 0x02001839 RID: 6201
		public enum PermissionLevels
		{
			// Token: 0x0400901C RID: 36892
			Everyone,
			// Token: 0x0400901D RID: 36893
			VIP,
			// Token: 0x0400901E RID: 36894
			Sub,
			// Token: 0x0400901F RID: 36895
			Mod,
			// Token: 0x04009020 RID: 36896
			Broadcaster
		}
	}
}
