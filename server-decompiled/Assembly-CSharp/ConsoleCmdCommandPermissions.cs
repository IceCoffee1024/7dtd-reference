using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x020001FA RID: 506
[Preserve]
public class ConsoleCmdCommandPermissions : ConsoleCmdAbstract
{
	// Token: 0x06000F87 RID: 3975 RVA: 0x00064D14 File Offset: 0x00062F14
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "commandpermission", "cp" };
	}

	// Token: 0x17000156 RID: 342
	// (get) Token: 0x06000F88 RID: 3976 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000157 RID: 343
	// (get) Token: 0x06000F89 RID: 3977 RVA: 0x0005FE4F File Offset: 0x0005E04F
	public override DeviceFlag AllowedDeviceTypes
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x06000F8A RID: 3978 RVA: 0x00064D2C File Offset: 0x00062F2C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Manage command permission levels";
	}

	// Token: 0x06000F8B RID: 3979 RVA: 0x00064D33 File Offset: 0x00062F33
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "Set/get permission levels required to execute a given command. Default\nlevel required for commands that are not explicitly specified is 0.\nUsage:\n   cp add <command> <level>\n   cp remove <command>\n   cp list";
	}

	// Token: 0x06000F8C RID: 3980 RVA: 0x00064D3C File Offset: 0x00062F3C
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
		if (_params[0].EqualsCaseInsensitive("list"))
		{
			this.ExecuteList();
			return;
		}
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Invalid sub command \"" + _params[0] + "\".");
	}

	// Token: 0x06000F8D RID: 3981 RVA: 0x00064DE0 File Offset: 0x00062FE0
	[PublicizedFrom(EAccessModifier.Private)]
	public void ExecuteAdd(List<string> _params)
	{
		if (_params.Count != 3)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Wrong number of arguments, expected 3, found " + _params.Count.ToString() + ".");
			return;
		}
		if (SingletonMonoBehaviour<SdtdConsole>.Instance.GetCommand(_params[1], false) == null)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("\"" + _params[1] + "\" is not a valid command.");
			return;
		}
		int num;
		if (!int.TryParse(_params[2], out num))
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("\"" + _params[2] + "\" is not a valid integer.");
			return;
		}
		GameManager.Instance.adminTools.Commands.AddCommand(_params[1], num, true);
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output(string.Format("{0} added with permission level of {1}.", _params[1], num));
	}

	// Token: 0x06000F8E RID: 3982 RVA: 0x00064EC4 File Offset: 0x000630C4
	[PublicizedFrom(EAccessModifier.Private)]
	public void ExecuteRemove(List<string> _params)
	{
		if (_params.Count != 2)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Wrong number of arguments, expected 2, found " + _params.Count.ToString() + ".");
			return;
		}
		IConsoleCommand command = SingletonMonoBehaviour<SdtdConsole>.Instance.GetCommand(_params[1], false);
		if (command == null)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("\"" + _params[1] + "\" is not a valid command.");
			return;
		}
		GameManager.Instance.adminTools.Commands.RemoveCommand(command.GetCommands());
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output(string.Format("{0} removed from permissions list.", _params[1]));
	}

	// Token: 0x06000F8F RID: 3983 RVA: 0x00064F70 File Offset: 0x00063170
	[PublicizedFrom(EAccessModifier.Private)]
	public void ExecuteList()
	{
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Defined Command Permissions:");
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("  Level: Command");
		foreach (KeyValuePair<string, AdminCommands.CommandPermission> keyValuePair in GameManager.Instance.adminTools.Commands.GetCommands())
		{
			AdminCommands.CommandPermission value = keyValuePair.Value;
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output(string.Format("  {0,5}: {1}", value.PermissionLevel, value.Command));
		}
	}
}
