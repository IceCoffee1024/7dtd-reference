using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x0200180C RID: 6156
	public class BaseTwitchCommand
	{
		// Token: 0x17001722 RID: 5922
		// (get) Token: 0x0600BE6A RID: 48746 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Everyone;
			}
		}

		// Token: 0x17001723 RID: 5923
		// (get) Token: 0x0600BE6B RID: 48747 RVA: 0x0001FFE2 File Offset: 0x0001E1E2
		public virtual string[] CommandText
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001724 RID: 5924
		// (get) Token: 0x0600BE6C RID: 48748 RVA: 0x0001FFE2 File Offset: 0x0001E1E2
		public virtual string[] LocalizedCommandNames
		{
			get
			{
				return null;
			}
		}

		// Token: 0x0600BE6E RID: 48750 RVA: 0x00466F7E File Offset: 0x0046517E
		public static void ClearCommandPermissionOverrides()
		{
			BaseTwitchCommand.CommandPermissionOverrides.Clear();
		}

		// Token: 0x0600BE6F RID: 48751 RVA: 0x00466F8A File Offset: 0x0046518A
		public static void AddCommandPermissionOverride(string commandName, BaseTwitchCommand.PermissionLevels permissionLevel)
		{
			if (!BaseTwitchCommand.CommandPermissionOverrides.ContainsKey(commandName))
			{
				BaseTwitchCommand.CommandPermissionOverrides.Add(commandName, permissionLevel);
			}
		}

		// Token: 0x0600BE70 RID: 48752 RVA: 0x00466FA5 File Offset: 0x004651A5
		public static BaseTwitchCommand.PermissionLevels GetPermission(BaseTwitchCommand cmd)
		{
			if (BaseTwitchCommand.CommandPermissionOverrides.ContainsKey(cmd.CommandText[0]))
			{
				return BaseTwitchCommand.CommandPermissionOverrides[cmd.CommandText[0]];
			}
			return cmd.RequiredPermission;
		}

		// Token: 0x0600BE71 RID: 48753 RVA: 0x00466FD4 File Offset: 0x004651D4
		public BaseTwitchCommand()
		{
			this.SetupCommandTextList();
			if (BaseTwitchCommand.allText == "")
			{
				BaseTwitchCommand.allText = Localization.Get("lblAll", false, null);
			}
		}

		// Token: 0x0600BE72 RID: 48754 RVA: 0x00467010 File Offset: 0x00465210
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

		// Token: 0x0600BE73 RID: 48755 RVA: 0x00467064 File Offset: 0x00465264
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

		// Token: 0x0600BE74 RID: 48756 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void Execute(ViewerEntry entry, TwitchIRCClient.TwitchChatMessage message)
		{
		}

		// Token: 0x0600BE75 RID: 48757 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void ExecuteConsole(List<string> arguments)
		{
		}

		// Token: 0x04008F52 RID: 36690
		public List<string> CommandTextList = new List<string>();

		// Token: 0x04008F53 RID: 36691
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string allText = "";

		// Token: 0x04008F54 RID: 36692
		[PublicizedFrom(EAccessModifier.Protected)]
		public static Dictionary<string, BaseTwitchCommand.PermissionLevels> CommandPermissionOverrides = new Dictionary<string, BaseTwitchCommand.PermissionLevels>();

		// Token: 0x0200180D RID: 6157
		public enum PermissionLevels
		{
			// Token: 0x04008F56 RID: 36694
			Everyone,
			// Token: 0x04008F57 RID: 36695
			VIP,
			// Token: 0x04008F58 RID: 36696
			Sub,
			// Token: 0x04008F59 RID: 36697
			Mod,
			// Token: 0x04008F5A RID: 36698
			Broadcaster
		}
	}
}
