using System;
using System.Collections.Generic;
using System.Text;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000225 RID: 549
[Preserve]
public class ConsoleCmdHelp : ConsoleCmdAbstract
{
	// Token: 0x060010AB RID: 4267 RVA: 0x00069C4F File Offset: 0x00067E4F
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "help" };
	}

	// Token: 0x1700019E RID: 414
	// (get) Token: 0x060010AC RID: 4268 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700019F RID: 415
	// (get) Token: 0x060010AD RID: 4269 RVA: 0x0006110A File Offset: 0x0005F30A
	public override int DefaultPermissionLevel
	{
		get
		{
			return 1000;
		}
	}

	// Token: 0x170001A0 RID: 416
	// (get) Token: 0x060010AE RID: 4270 RVA: 0x0005FE4F File Offset: 0x0005E04F
	public override DeviceFlag AllowedDeviceTypes
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x060010AF RID: 4271 RVA: 0x00069C5F File Offset: 0x00067E5F
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Help on console and specific commands";
	}

	// Token: 0x060010B0 RID: 4272 RVA: 0x00069C66 File Offset: 0x00067E66
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "\r\n\t\t\t|Usage:\r\n\t\t\t|  1. help\r\n\t\t\t|  2. help * <searchstring>\r\n\t\t\t|  3. help <command name>\r\n\t\t\t|  4. help output\r\n\t\t\t|  5. help outputdetailed\r\n\t\t\t|1. Show general help and list all available commands\r\n\t\t\t|2. List commands where either the name or the description contains the given text\r\n\t\t\t|3. Show help for the given command\r\n\t\t\t|4. Write command list to log file\r\n\t\t\t|5. Write command list with help texts to log file\r\n\t\t\t".Unindent(true);
	}

	// Token: 0x060010B1 RID: 4273 RVA: 0x00069C74 File Offset: 0x00067E74
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		ConsoleCmdHelp.sb.Clear();
		if (_params.Count == 0)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("*** Generic Console Help ***");
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("To get further help on a specific topic or command type (without the brackets)");
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("    help <topic / command>");
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("");
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Generic notation of command parameters:");
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("   <param name>              Required parameter");
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("   <entityId / player name>  Possible types of parameter values");
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("   [param name]              Optional parameter");
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("");
			ConsoleCmdHelp.sb.Clear();
			ConsoleCmdHelp.BuildStringCommandDescriptions();
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output(ConsoleCmdHelp.sb.ToString());
			ConsoleCmdHelp.sb.Clear();
			return;
		}
		Action<List<string>> action = null;
		if (ConsoleCmdHelp.helpTopics.ContainsKey(_params[0]))
		{
			action = ConsoleCmdHelp.helpTopics[_params[0]].Action;
		}
		ConsoleCmdHelp.sb.Clear();
		ConsoleCmdHelp.BuildStringHelpText(_params[0]);
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output(ConsoleCmdHelp.sb.ToString());
		ConsoleCmdHelp.sb.Clear();
		if (action != null)
		{
			action(_params);
		}
	}

	// Token: 0x060010B2 RID: 4274 RVA: 0x00069DBC File Offset: 0x00067FBC
	public static void ValidateNoCommandOverlap()
	{
		IList<IConsoleCommand> commands = SingletonMonoBehaviour<SdtdConsole>.Instance.GetCommands();
		for (int i = 0; i < commands.Count; i++)
		{
			foreach (string text in commands[i].GetCommands())
			{
				if (ConsoleCmdHelp.helpTopics.ContainsKey(text))
				{
					Log.Warning("Command with alias \"" + text + "\" conflicts with help topic command");
				}
			}
		}
	}

	// Token: 0x060010B3 RID: 4275 RVA: 0x00069E2C File Offset: 0x0006802C
	[PublicizedFrom(EAccessModifier.Private)]
	public static void BuildStringCommandDescriptions()
	{
		ConsoleCmdHelp.sb.AppendLine("*** List of Help Topics ***");
		foreach (KeyValuePair<string, ConsoleCmdHelp.HelpTopic> keyValuePair in ConsoleCmdHelp.helpTopics)
		{
			ConsoleCmdHelp.sb.Append(keyValuePair.Key);
			ConsoleCmdHelp.sb.Append(" => ");
			ConsoleCmdHelp.sb.AppendLine(keyValuePair.Value.Description);
		}
		ConsoleCmdHelp.sb.AppendLine("");
		ConsoleCmdHelp.sb.AppendLine("*** List of Commands ***");
		IList<IConsoleCommand> commands = SingletonMonoBehaviour<SdtdConsole>.Instance.GetCommands();
		for (int i = 0; i < commands.Count; i++)
		{
			IConsoleCommand consoleCommand = commands[i];
			if (consoleCommand.CanExecuteForDevice && consoleCommand != null)
			{
				foreach (string text in consoleCommand.GetCommands())
				{
					ConsoleCmdHelp.sb.Append(" ");
					ConsoleCmdHelp.sb.Append(text);
				}
				ConsoleCmdHelp.sb.Append(" => ");
				ConsoleCmdHelp.sb.AppendLine(consoleCommand.GetDescription());
			}
		}
	}

	// Token: 0x060010B4 RID: 4276 RVA: 0x00069F78 File Offset: 0x00068178
	[PublicizedFrom(EAccessModifier.Private)]
	public static void BuildStringHelpText(string key)
	{
		string text = null;
		string text2 = null;
		if (ConsoleCmdHelp.helpTopics.ContainsKey(key))
		{
			text = "Topic: " + key;
			text2 = ConsoleCmdHelp.helpTopics[key].ActionCompleteText;
		}
		else
		{
			IConsoleCommand command = SingletonMonoBehaviour<SdtdConsole>.Instance.GetCommand(key, false);
			if (command != null && command.CanExecuteForDevice)
			{
				text = "Command(s): " + string.Join(", ", command.GetCommands());
				text2 = command.GetHelp();
				if (string.IsNullOrEmpty(text2))
				{
					text2 = "No detailed help available.\nDescription: " + command.GetDescription();
				}
			}
		}
		if (text != null)
		{
			ConsoleCmdHelp.sb.AppendLine("*** " + text + " ***");
			foreach (string text3 in text2.Split('\n', StringSplitOptions.None))
			{
				ConsoleCmdHelp.sb.AppendLine(text3);
			}
			return;
		}
		ConsoleCmdHelp.sb.AppendLine("No command or topic found by \"" + key + "\"");
	}

	// Token: 0x060010B5 RID: 4277 RVA: 0x0006A071 File Offset: 0x00068271
	[PublicizedFrom(EAccessModifier.Private)]
	public static void OutputHelp(List<string> _params)
	{
		ConsoleCmdHelp.sb.Clear();
		ConsoleCmdHelp.BuildStringCommandDescriptions();
		Log.Out(ConsoleCmdHelp.sb.ToString());
		ConsoleCmdHelp.sb.Clear();
	}

	// Token: 0x060010B6 RID: 4278 RVA: 0x0006A0A0 File Offset: 0x000682A0
	[PublicizedFrom(EAccessModifier.Private)]
	public static void OutputDetailedHelp(List<string> _params)
	{
		ConsoleCmdHelp.sb.Clear();
		ConsoleCmdHelp.sb.AppendLine("*** List of Help Topics ***");
		foreach (KeyValuePair<string, ConsoleCmdHelp.HelpTopic> keyValuePair in ConsoleCmdHelp.helpTopics)
		{
			ConsoleCmdHelp.BuildStringHelpText(keyValuePair.Key);
			ConsoleCmdHelp.sb.AppendLine();
		}
		ConsoleCmdHelp.sb.AppendLine("*** List of Commands ***");
		foreach (IConsoleCommand consoleCommand in SingletonMonoBehaviour<SdtdConsole>.Instance.GetCommands())
		{
			ConsoleCmdHelp.BuildStringHelpText(consoleCommand.GetCommands()[0]);
			ConsoleCmdHelp.sb.AppendLine();
		}
		Log.Out(ConsoleCmdHelp.sb.ToString());
		ConsoleCmdHelp.sb.Clear();
	}

	// Token: 0x060010B7 RID: 4279 RVA: 0x0006A198 File Offset: 0x00068398
	[PublicizedFrom(EAccessModifier.Private)]
	public static void SearchHelp(List<string> _params)
	{
		ConsoleCmdHelp.sb.Clear();
		if (_params.Count < 2)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Argument for search mask missing");
			return;
		}
		string text = _params[1];
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("*** List of Commands for \"" + text + "\" ***");
		IList<IConsoleCommand> commands = SingletonMonoBehaviour<SdtdConsole>.Instance.GetCommands();
		for (int i = 0; i < commands.Count; i++)
		{
			IConsoleCommand consoleCommand = commands[i];
			string description = consoleCommand.GetDescription();
			bool flag = text == null;
			if (!flag)
			{
				flag = description.ContainsCaseInsensitive(text);
				foreach (string text2 in consoleCommand.GetCommands())
				{
					flag |= text2.ContainsCaseInsensitive(text);
				}
			}
			if (flag)
			{
				foreach (string text3 in consoleCommand.GetCommands())
				{
					ConsoleCmdHelp.sb.Append(" ");
					ConsoleCmdHelp.sb.Append(text3);
				}
				ConsoleCmdHelp.sb.Append(" => ");
				ConsoleCmdHelp.sb.Append(description);
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output(ConsoleCmdHelp.sb.ToString());
				ConsoleCmdHelp.sb.Length = 0;
			}
		}
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output(ConsoleCmdHelp.sb.ToString());
		ConsoleCmdHelp.sb.Clear();
	}

	// Token: 0x04000CB4 RID: 3252
	[PublicizedFrom(EAccessModifier.Private)]
	public static StringBuilder sb = new StringBuilder();

	// Token: 0x04000CB5 RID: 3253
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<string, ConsoleCmdHelp.HelpTopic> helpTopics = new Dictionary<string, ConsoleCmdHelp.HelpTopic>
	{
		{
			"output",
			new ConsoleCmdHelp.HelpTopic("Prints commands to log file", new Action<List<string>>(ConsoleCmdHelp.OutputHelp), "Printed commands to log file")
		},
		{
			"outputdetailed",
			new ConsoleCmdHelp.HelpTopic("Prints commands with details to log file", new Action<List<string>>(ConsoleCmdHelp.OutputDetailedHelp), "Printed commands with details to log file")
		},
		{
			"search",
			new ConsoleCmdHelp.HelpTopic("Search for all commands matching a string", new Action<List<string>>(ConsoleCmdHelp.SearchHelp), "<first argument will be the string to match>")
		},
		{
			"*",
			new ConsoleCmdHelp.HelpTopic("Search for all commands matching a string", new Action<List<string>>(ConsoleCmdHelp.SearchHelp), "<first argument will be the string to match>")
		}
	};

	// Token: 0x02000226 RID: 550
	[PublicizedFrom(EAccessModifier.Private)]
	public struct HelpTopic
	{
		// Token: 0x060010BA RID: 4282 RVA: 0x0006A3B9 File Offset: 0x000685B9
		public HelpTopic(string _desc, Action<List<string>> _action, string _actionCompleteText)
		{
			this.Description = _desc;
			this.Action = _action;
			this.ActionCompleteText = _actionCompleteText;
		}

		// Token: 0x04000CB6 RID: 3254
		public string Description;

		// Token: 0x04000CB7 RID: 3255
		public Action<List<string>> Action;

		// Token: 0x04000CB8 RID: 3256
		public string ActionCompleteText;
	}
}
