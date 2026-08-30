using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x020002BD RID: 701
[Preserve]
public class ConsoleCmdWhitelist : ConsoleCmdAbstract
{
	// Token: 0x06001441 RID: 5185 RVA: 0x0007AD7A File Offset: 0x00078F7A
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "whitelist" };
	}

	// Token: 0x1700023B RID: 571
	// (get) Token: 0x06001442 RID: 5186 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700023C RID: 572
	// (get) Token: 0x06001443 RID: 5187 RVA: 0x0005FE4F File Offset: 0x0005E04F
	public override DeviceFlag AllowedDeviceTypes
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x06001444 RID: 5188 RVA: 0x0007AD8A File Offset: 0x00078F8A
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Manage whitelist entries";
	}

	// Token: 0x06001445 RID: 5189 RVA: 0x0007AD91 File Offset: 0x00078F91
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "Set/get whitelist entries. Note: If there is at least one entry on the list\nno user who is not on this list will be able to join the server!\nUsage:\n   whitelist add <name / entity id / platform + platform user id> [displayname]\n   whitelist remove <name / entity id / platform + platform user id>\n   whitelist addgroup <steam id> [displayname]\n   whitelist removegroup <steam id>\n   whitelist list\nTo use the add/remove sub commands with a name or entity ID the player has\nto be online, the variant with platform and platform ID can be used for currently offline\nusers too.\nDisplayname can be used to put a descriptive name to the ban list in addition\nto the user's platform ID. If used on an online user the player name is used by default.";
	}

	// Token: 0x06001446 RID: 5190 RVA: 0x0007AD98 File Offset: 0x00078F98
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (GameManager.Instance.adminTools == null)
		{
			return;
		}
		if (_params.Count < 1)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("No sub command given.");
			return;
		}
		if (_params[0].EqualsCaseInsensitive("add"))
		{
			this.ExecuteAdd(_params);
			return;
		}
		if (_params[0].EqualsCaseInsensitive("remove"))
		{
			this.ExecuteRemove(_params);
			return;
		}
		if (_params[0].EqualsCaseInsensitive("addgroup"))
		{
			this.ExecuteAddGroup(_params);
			return;
		}
		if (_params[0].EqualsCaseInsensitive("removegroup"))
		{
			this.ExecuteRemoveGroup(_params);
			return;
		}
		if (_params[0].EqualsCaseInsensitive("list"))
		{
			this.ExecuteList();
			return;
		}
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Invalid sub command \"" + _params[0] + "\".");
	}

	// Token: 0x06001447 RID: 5191 RVA: 0x0007AE74 File Offset: 0x00079074
	[PublicizedFrom(EAccessModifier.Private)]
	public void ExecuteAdd(List<string> _params)
	{
		if (_params.Count != 2 && _params.Count != 3)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Wrong number of arguments, expected 2 or 3, found " + _params.Count.ToString() + ".");
			return;
		}
		string text = null;
		if (_params.Count > 2)
		{
			text = _params[2];
		}
		ClientInfo clientInfo = ConsoleHelper.ParseParamIdOrName(_params[1], true, false);
		PlatformUserIdentifierAbs platformUserIdentifierAbs;
		if (clientInfo != null)
		{
			platformUserIdentifierAbs = clientInfo.InternalId;
			if (text == null)
			{
				text = clientInfo.playerName;
			}
		}
		else if ((platformUserIdentifierAbs = ConsoleHelper.ParseParamUserId(_params[1])) == null)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("\"" + _params[1] + "\" is not a valid entity id, player name or user id.");
			return;
		}
		bool flag = GameManager.Instance.adminTools.Whitelist.IsWhiteListEnabled();
		GameManager.Instance.adminTools.Whitelist.AddUser(text, platformUserIdentifierAbs);
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output(string.Format("{0} added to whitelist.", platformUserIdentifierAbs));
		this.WhitelistStateChangedMessage(flag);
	}

	// Token: 0x06001448 RID: 5192 RVA: 0x0007AF70 File Offset: 0x00079170
	[PublicizedFrom(EAccessModifier.Private)]
	public void ExecuteRemove(List<string> _params)
	{
		if (_params.Count != 2)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Wrong number of arguments, expected 2, found " + _params.Count.ToString() + ".");
			return;
		}
		ClientInfo clientInfo = ConsoleHelper.ParseParamIdOrName(_params[1], true, false);
		if (clientInfo != null)
		{
			bool flag = GameManager.Instance.adminTools.Whitelist.IsWhiteListEnabled();
			GameManager.Instance.adminTools.Whitelist.RemoveUser(clientInfo.PlatformId);
			if (clientInfo.CrossplatformId != null)
			{
				GameManager.Instance.adminTools.Whitelist.RemoveUser(clientInfo.CrossplatformId);
			}
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output(string.Format("{0} {1} the whitelist.", clientInfo.InternalId, this.WhitelistStateChangedMessage(flag) ? "removed from" : "was not on"));
			return;
		}
		PlatformUserIdentifierAbs platformUserIdentifierAbs;
		if ((platformUserIdentifierAbs = ConsoleHelper.ParseParamUserId(_params[1])) != null)
		{
			bool flag2 = GameManager.Instance.adminTools.Whitelist.IsWhiteListEnabled();
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output(string.Format("{0} {1} the whitelist.", platformUserIdentifierAbs, GameManager.Instance.adminTools.Whitelist.RemoveUser(platformUserIdentifierAbs) ? "removed from" : "was not on"));
			this.WhitelistStateChangedMessage(flag2);
			return;
		}
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("\"" + _params[1] + "\" is not a valid entity id, player name or user id.");
	}

	// Token: 0x06001449 RID: 5193 RVA: 0x0007B0D0 File Offset: 0x000792D0
	[PublicizedFrom(EAccessModifier.Private)]
	public void ExecuteAddGroup(List<string> _params)
	{
		if (_params.Count != 2 && _params.Count != 3)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Wrong number of arguments, expected 2 or 3, found " + _params.Count.ToString() + ".");
			return;
		}
		string text = null;
		if (_params.Count > 2)
		{
			text = _params[2];
		}
		if (ConsoleHelper.ParseParamSteamGroupIdValid(_params[1]))
		{
			string text2 = _params[1];
			bool flag = GameManager.Instance.adminTools.Whitelist.IsWhiteListEnabled();
			GameManager.Instance.adminTools.Whitelist.AddGroup(text, text2);
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Group " + text2 + " added to whitelist.");
			this.WhitelistStateChangedMessage(flag);
			return;
		}
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("\"" + _params[1] + "\" is not a valid steam group id.");
	}

	// Token: 0x0600144A RID: 5194 RVA: 0x0007B1B0 File Offset: 0x000793B0
	[PublicizedFrom(EAccessModifier.Private)]
	public void ExecuteRemoveGroup(List<string> _params)
	{
		if (_params.Count != 2)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Wrong number of arguments, expected 2, found " + _params.Count.ToString() + ".");
			return;
		}
		if (ConsoleHelper.ParseParamSteamGroupIdValid(_params[1]))
		{
			string text = _params[1];
			bool flag = GameManager.Instance.adminTools.Whitelist.IsWhiteListEnabled();
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output(string.Concat(new string[]
			{
				"Group ",
				text,
				" ",
				GameManager.Instance.adminTools.Whitelist.RemoveGroup(text) ? "removed from" : "was not on",
				" the whitelist."
			}));
			this.WhitelistStateChangedMessage(flag);
			return;
		}
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("\"" + _params[1] + "\" is not a valid steam group id.");
	}

	// Token: 0x0600144B RID: 5195 RVA: 0x0007B29C File Offset: 0x0007949C
	[PublicizedFrom(EAccessModifier.Private)]
	public void ExecuteList()
	{
		if (!GameManager.Instance.adminTools.Whitelist.IsWhiteListEnabled())
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("No users or groups on whitelist, whitelist only mode not enabled.");
			return;
		}
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Whitelisted users:");
		foreach (KeyValuePair<PlatformUserIdentifierAbs, AdminWhitelist.WhitelistUser> keyValuePair in GameManager.Instance.adminTools.Whitelist.GetUsers())
		{
			AdminWhitelist.WhitelistUser value = keyValuePair.Value;
			ClientInfo clientInfo = SingletonMonoBehaviour<ConnectionManager>.Instance.Clients.ForUserId(value.UserIdentifier);
			if (clientInfo != null)
			{
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output(string.Format("  {0} ({1}, stored name: {2})", value.UserIdentifier, clientInfo.playerName, value.Name ?? ""));
			}
			else
			{
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output(string.Format("  {0} (stored name: {1})", value.UserIdentifier, value.Name ?? ""));
			}
		}
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Whitelisted groups:");
		foreach (KeyValuePair<string, AdminWhitelist.WhitelistGroup> keyValuePair2 in GameManager.Instance.adminTools.Whitelist.GetGroups())
		{
			AdminWhitelist.WhitelistGroup value2 = keyValuePair2.Value;
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output(string.Concat(new string[] { "  ", value2.SteamIdGroup, " (stored name: ", value2.Name, ")" }));
		}
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Whitelist only mode active.");
	}

	// Token: 0x0600144C RID: 5196 RVA: 0x0007B460 File Offset: 0x00079660
	[PublicizedFrom(EAccessModifier.Private)]
	public bool WhitelistStateChangedMessage(bool _whitelistEnabledBefore)
	{
		bool flag = GameManager.Instance.adminTools.Whitelist.IsWhiteListEnabled();
		if (flag == _whitelistEnabledBefore)
		{
			return false;
		}
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Whitelist only mode has been " + (flag ? "ACTIVATED" : "DISABLED") + "!");
		return true;
	}
}
