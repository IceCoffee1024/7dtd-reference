using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x020017E6 RID: 6118
	public class BaseTwitchCommand
	{
		// Token: 0x170016B0 RID: 5808
		// (get) Token: 0x0600BCE5 RID: 48357 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Everyone;
			}
		}

		// Token: 0x170016B1 RID: 5809
		// (get) Token: 0x0600BCE6 RID: 48358 RVA: 0x0001FF1E File Offset: 0x0001E11E
		public virtual string[] CommandText
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170016B2 RID: 5810
		// (get) Token: 0x0600BCE7 RID: 48359 RVA: 0x0001FF1E File Offset: 0x0001E11E
		public virtual string[] LocalizedCommandNames
		{
			get
			{
				return null;
			}
		}

		// Token: 0x0600BCE9 RID: 48361 RVA: 0x004611E2 File Offset: 0x0045F3E2
		public static void ClearCommandPermissionOverrides()
		{
			BaseTwitchCommand.CommandPermissionOverrides.Clear();
		}

		// Token: 0x0600BCEA RID: 48362 RVA: 0x004611EE File Offset: 0x0045F3EE
		public static void AddCommandPermissionOverride(string commandName, BaseTwitchCommand.PermissionLevels permissionLevel)
		{
			if (!BaseTwitchCommand.CommandPermissionOverrides.ContainsKey(commandName))
			{
				BaseTwitchCommand.CommandPermissionOverrides.Add(commandName, permissionLevel);
			}
		}

		// Token: 0x0600BCEB RID: 48363 RVA: 0x00461209 File Offset: 0x0045F409
		public static BaseTwitchCommand.PermissionLevels GetPermission(BaseTwitchCommand cmd)
		{
			if (BaseTwitchCommand.CommandPermissionOverrides.ContainsKey(cmd.CommandText[0]))
			{
				return BaseTwitchCommand.CommandPermissionOverrides[cmd.CommandText[0]];
			}
			return cmd.RequiredPermission;
		}

		// Token: 0x0600BCEC RID: 48364 RVA: 0x00461238 File Offset: 0x0045F438
		public BaseTwitchCommand()
		{
			this.SetupCommandTextList();
			if (BaseTwitchCommand.allText == "")
			{
				BaseTwitchCommand.allText = Localization.Get("lblAll", false, null);
			}
		}

		// Token: 0x0600BCED RID: 48365 RVA: 0x00461274 File Offset: 0x0045F474
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

		// Token: 0x0600BCEE RID: 48366 RVA: 0x004612C8 File Offset: 0x0045F4C8
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

		// Token: 0x0600BCEF RID: 48367 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void Execute(ViewerEntry entry, TwitchIRCClient.TwitchChatMessage message)
		{
		}

		// Token: 0x0600BCF0 RID: 48368 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void ExecuteConsole(List<string> arguments)
		{
		}

		// Token: 0x04008E36 RID: 36406
		public List<string> CommandTextList = new List<string>();

		// Token: 0x04008E37 RID: 36407
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string allText = "";

		// Token: 0x04008E38 RID: 36408
		[PublicizedFrom(EAccessModifier.Protected)]
		public static Dictionary<string, BaseTwitchCommand.PermissionLevels> CommandPermissionOverrides = new Dictionary<string, BaseTwitchCommand.PermissionLevels>();

		// Token: 0x020017E7 RID: 6119
		public enum PermissionLevels
		{
			// Token: 0x04008E3A RID: 36410
			Everyone,
			// Token: 0x04008E3B RID: 36411
			VIP,
			// Token: 0x04008E3C RID: 36412
			Sub,
			// Token: 0x04008E3D RID: 36413
			Mod,
			// Token: 0x04008E3E RID: 36414
			Broadcaster
		}
	}
}
