using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

// Token: 0x020002CF RID: 719
public class SdtdConsole : SingletonMonoBehaviour<SdtdConsole>
{
	// Token: 0x17000242 RID: 578
	// (get) Token: 0x060014A1 RID: 5281 RVA: 0x0007CCB8 File Offset: 0x0007AEB8
	public int HideCommandExecutionLog
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			if (this.hideCommandExecutionLog < 0)
			{
				this.hideCommandExecutionLog = GamePrefs.GetInt(EnumGamePrefs.HideCommandExecutionLog);
			}
			return this.hideCommandExecutionLog;
		}
	}

	// Token: 0x060014A2 RID: 5282 RVA: 0x0007CCD9 File Offset: 0x0007AED9
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void singletonAwake()
	{
		Log.LogCallbacksExtended += this.LogCallback;
		this.m_CommandsReadOnly = new ReadOnlyCollection<IConsoleCommand>(this.m_Commands);
		PreserveCheckPatch.Enable();
	}

	// Token: 0x060014A3 RID: 5283 RVA: 0x0007CD04 File Offset: 0x0007AF04
	[PublicizedFrom(EAccessModifier.Private)]
	public void LogCallback(string _formattedMessage, string _plainMessage, string _trace, LogType _type, DateTime _timestamp, long _uptime)
	{
		if (GameManager.IsDedicatedServer)
		{
			for (int i = 0; i < this.m_Servers.Count; i++)
			{
				IConsoleServer consoleServer = this.m_Servers[i];
				try
				{
					consoleServer.SendLog(_formattedMessage, _plainMessage, _trace, _type, _timestamp, _uptime);
				}
				catch (Exception ex)
				{
					Log.Error("Error sending to console server:");
					Log.Exception(ex);
				}
			}
		}
	}

	// Token: 0x060014A4 RID: 5284 RVA: 0x0007CD70 File Offset: 0x0007AF70
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Update()
	{
		if (this.m_commandsToExecuteAsync.Count > 0)
		{
			List<SdtdConsole.CommandInstance> commandsToExecuteAsync = this.m_commandsToExecuteAsync;
			lock (commandsToExecuteAsync)
			{
				try
				{
					CommandSenderInfo commandSenderInfo = new CommandSenderInfo
					{
						IsLocalGame = false,
						RemoteClientInfo = null,
						NetworkConnection = this.m_commandsToExecuteAsync[0].sender
					};
					List<string> list = this.executeCommand(this.m_commandsToExecuteAsync[0].command, commandSenderInfo);
					this.m_commandsToExecuteAsync[0].sender.SendLines(list);
				}
				catch (Exception ex)
				{
					Log.Exception(ex);
				}
				this.m_commandsToExecuteAsync.RemoveAt(0);
			}
		}
	}

	// Token: 0x060014A5 RID: 5285 RVA: 0x0007CE40 File Offset: 0x0007B040
	public void Output(string _line)
	{
		List<string> currentCommandOutputList = this.m_currentCommandOutputList;
		if (currentCommandOutputList == null)
		{
			return;
		}
		currentCommandOutputList.Add(_line);
	}

	// Token: 0x060014A6 RID: 5286 RVA: 0x0007CE53 File Offset: 0x0007B053
	public void Output(string _format, params object[] _args)
	{
		this.Output(string.Format(_format, _args));
	}

	// Token: 0x060014A7 RID: 5287 RVA: 0x0007CE64 File Offset: 0x0007B064
	[PublicizedFrom(EAccessModifier.Private)]
	public void RegisterCommand(SortedList<string, IConsoleCommand> _commandsList, string _className, IConsoleCommand _command)
	{
		string[] commands = _command.GetCommands();
		string text = commands[0];
		if (_commandsList.ContainsKey(text))
		{
			Log.Warning("Command with name \"" + text + "\" already loaded, not loading from class " + _className);
			return;
		}
		_commandsList.Add(text, _command);
		if (GameManager.Instance.adminTools == null)
		{
			return;
		}
		if (!GameManager.Instance.adminTools.Commands.IsPermissionDefined(commands) && _command.DefaultPermissionLevel != 0)
		{
			Log.Out("Command \"{0}\" has no explicit permission level, but a default permission of {1}, adding to permission list", new object[] { text, _command.DefaultPermissionLevel });
			GameManager.Instance.adminTools.Commands.AddCommand(text, _command.DefaultPermissionLevel, false);
		}
	}

	// Token: 0x060014A8 RID: 5288 RVA: 0x0007CF10 File Offset: 0x0007B110
	public void RegisterCommands()
	{
		SortedList<string, IConsoleCommand> commandsList = new SortedList<string, IConsoleCommand>();
		ReflectionHelpers.FindTypesImplementingBase(typeof(IConsoleCommand), delegate(Type _type)
		{
			IConsoleCommand consoleCommand3 = ReflectionHelpers.Instantiate<IConsoleCommand>(_type);
			if (consoleCommand3 != null)
			{
				this.RegisterCommand(commandsList, _type.Name, consoleCommand3);
			}
		}, false);
		try
		{
			foreach (IConsoleCommand consoleCommand in commandsList.Values)
			{
				this.m_Commands.Add(consoleCommand);
				for (int i = 0; i < consoleCommand.GetCommands().Length; i++)
				{
					string text = consoleCommand.GetCommands()[i];
					if (!string.IsNullOrEmpty(text))
					{
						IConsoleCommand consoleCommand2;
						if (this.m_CommandsAllVariants.TryGetValue(text, out consoleCommand2))
						{
							Log.Warning(string.Concat(new string[]
							{
								"Command with alias \"",
								text,
								"\" already registered from ",
								consoleCommand2.GetType().Name,
								", not registering for class ",
								consoleCommand.GetType().Name
							}));
						}
						else
						{
							this.m_CommandsAllVariants.Add(text, consoleCommand);
						}
					}
				}
			}
			ConsoleCmdHelp.ValidateNoCommandOverlap();
		}
		catch (Exception ex)
		{
			Log.Error("Error registering commands");
			Log.Exception(ex);
		}
		AdminTools adminTools = GameManager.Instance.adminTools;
		if (adminTools == null)
		{
			return;
		}
		adminTools.Save();
	}

	// Token: 0x060014A9 RID: 5289 RVA: 0x0007D070 File Offset: 0x0007B270
	public void RegisterServer(IConsoleServer _server)
	{
		if (_server != null)
		{
			this.m_Servers.Add(_server);
		}
	}

	// Token: 0x060014AA RID: 5290 RVA: 0x0007D084 File Offset: 0x0007B284
	public void Cleanup()
	{
		for (int i = 0; i < this.m_Servers.Count; i++)
		{
			IConsoleServer consoleServer = this.m_Servers[i];
			try
			{
				consoleServer.Disconnect();
			}
			catch (Exception ex)
			{
				Log.Error("Error sending to console server:");
				Log.Exception(ex);
			}
		}
		this.m_Servers.Clear();
	}

	// Token: 0x060014AB RID: 5291 RVA: 0x0007D0E8 File Offset: 0x0007B2E8
	public IConsoleCommand GetCommand(string _command, bool _alreadyTokenized = false)
	{
		if (!_alreadyTokenized)
		{
			int num = _command.IndexOf(' ');
			if (num >= 0)
			{
				_command = _command.Substring(0, num);
			}
		}
		IConsoleCommand consoleCommand;
		if (this.m_CommandsAllVariants.TryGetValue(_command, out consoleCommand))
		{
			return consoleCommand;
		}
		return null;
	}

	// Token: 0x060014AC RID: 5292 RVA: 0x0007D122 File Offset: 0x0007B322
	public IList<IConsoleCommand> GetCommands()
	{
		return this.m_CommandsReadOnly;
	}

	// Token: 0x060014AD RID: 5293 RVA: 0x0007D12C File Offset: 0x0007B32C
	public void ExecuteAsync(string _command, IConsoleConnection _sender)
	{
		SdtdConsole.CommandInstance commandInstance = new SdtdConsole.CommandInstance(_command, _sender);
		List<SdtdConsole.CommandInstance> commandsToExecuteAsync = this.m_commandsToExecuteAsync;
		lock (commandsToExecuteAsync)
		{
			this.m_commandsToExecuteAsync.Add(commandInstance);
		}
	}

	// Token: 0x060014AE RID: 5294 RVA: 0x0007D17C File Offset: 0x0007B37C
	public List<string> ExecuteSync(string _command, ClientInfo _cInfo)
	{
		CommandSenderInfo commandSenderInfo = new CommandSenderInfo
		{
			IsLocalGame = (_cInfo == null),
			RemoteClientInfo = _cInfo,
			NetworkConnection = null
		};
		return this.executeCommand(_command, commandSenderInfo);
	}

	// Token: 0x060014AF RID: 5295 RVA: 0x0007D1B8 File Offset: 0x0007B3B8
	[PublicizedFrom(EAccessModifier.Private)]
	public List<string> executeCommand(string _command, CommandSenderInfo _senderInfo)
	{
		if (string.IsNullOrEmpty(_command))
		{
			return null;
		}
		this.m_currentCommandOutputList.Clear();
		List<string> list = this.tokenizeCommand(_command);
		if (list != null)
		{
			if (list[0] == string.Empty)
			{
				return null;
			}
			IConsoleCommand command = this.GetCommand(list[0], true);
			if (command != null)
			{
				if (!command.CanExecuteForDevice)
				{
					this.m_currentCommandOutputList.Add("*** ERROR: Command '" + list[0] + "' can not be executed on this device type.");
					return this.m_currentCommandOutputList;
				}
				if (GameManager.Instance.World == null && !command.AllowedInMainMenu)
				{
					this.m_currentCommandOutputList.Add("*** ERROR: Command '" + list[0] + "' can only be executed when a game is started.");
					return this.m_currentCommandOutputList;
				}
				if (_senderInfo.IsLocalGame)
				{
					if (this.HideCommandExecutionLog < 3)
					{
						Log.Out("Executing command '" + _command + "'");
					}
				}
				else if (_senderInfo.RemoteClientInfo != null)
				{
					if (this.HideCommandExecutionLog < 2)
					{
						string text = "Executing command '";
						string text2 = "' from client ";
						ClientInfo remoteClientInfo = _senderInfo.RemoteClientInfo;
						Log.Out(text + _command + text2 + ((remoteClientInfo != null) ? remoteClientInfo.ToString() : null));
					}
				}
				else if (this.HideCommandExecutionLog < 1)
				{
					Log.Out("Executing command '" + _command + "' by " + _senderInfo.NetworkConnection.GetDescription());
				}
				try
				{
					command.Execute(list.GetRange(1, list.Count - 1), _senderInfo);
					goto IL_01AE;
				}
				catch (Exception ex)
				{
					this.m_currentCommandOutputList.Add("*** ERROR: Executing command '" + list[0] + "' failed: " + ex.Message);
					Log.Exception(ex);
					goto IL_01AE;
				}
			}
			this.m_currentCommandOutputList.Add("*** ERROR: unknown command '" + list[0] + "'");
		}
		IL_01AE:
		return this.m_currentCommandOutputList;
	}

	// Token: 0x060014B0 RID: 5296 RVA: 0x0007D38C File Offset: 0x0007B58C
	[PublicizedFrom(EAccessModifier.Private)]
	public List<string> tokenizeCommand(string _command)
	{
		List<string> list = this.tokenizedCommandList;
		list.Clear();
		bool flag = false;
		int num = 0;
		for (int i = 0; i < _command.Length; i++)
		{
			if (!flag)
			{
				if (_command[i] == '"')
				{
					if (i - num > 0)
					{
						list.Add(_command.Substring(num, i - num));
					}
					num = i + 1;
					flag = true;
				}
				else if (_command[i] == ' ' || _command[i] == '\t')
				{
					if (i - num > 0)
					{
						list.Add(_command.Substring(num, i - num));
					}
					num = i + 1;
				}
			}
			else if (_command[i] == '"')
			{
				if (i + 1 < _command.Length && _command[i + 1] == '"')
				{
					i++;
				}
				else
				{
					string text = _command.Substring(num, i - num);
					text = text.Replace("\"\"", "\"");
					list.Add(text);
					num = i + 1;
					flag = false;
				}
			}
		}
		if (flag)
		{
			this.m_currentCommandOutputList.Add("*** ERROR: Quotation started at position " + num.ToString() + " was not closed");
			return null;
		}
		if (num < _command.Length)
		{
			list.Add(_command.Substring(num, _command.Length - num));
		}
		return list;
	}

	// Token: 0x04000DAA RID: 3498
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly List<IConsoleCommand> m_Commands = new List<IConsoleCommand>();

	// Token: 0x04000DAB RID: 3499
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly Dictionary<string, IConsoleCommand> m_CommandsAllVariants = new CaseInsensitiveStringDictionary<IConsoleCommand>();

	// Token: 0x04000DAC RID: 3500
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public ReadOnlyCollection<IConsoleCommand> m_CommandsReadOnly;

	// Token: 0x04000DAD RID: 3501
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly List<IConsoleServer> m_Servers = new List<IConsoleServer>();

	// Token: 0x04000DAE RID: 3502
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly List<SdtdConsole.CommandInstance> m_commandsToExecuteAsync = new List<SdtdConsole.CommandInstance>();

	// Token: 0x04000DAF RID: 3503
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly List<string> m_currentCommandOutputList = new List<string>();

	// Token: 0x04000DB0 RID: 3504
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int hideCommandExecutionLog = -1;

	// Token: 0x04000DB1 RID: 3505
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly List<string> tokenizedCommandList = new List<string>(16);

	// Token: 0x020002D0 RID: 720
	[PublicizedFrom(EAccessModifier.Private)]
	public class CommandInstance
	{
		// Token: 0x060014B2 RID: 5298 RVA: 0x0007D51E File Offset: 0x0007B71E
		public CommandInstance(string _command, IConsoleConnection _sender)
		{
			this.command = _command;
			this.sender = _sender;
		}

		// Token: 0x04000DB2 RID: 3506
		public readonly string command;

		// Token: 0x04000DB3 RID: 3507
		public readonly IConsoleConnection sender;
	}
}
