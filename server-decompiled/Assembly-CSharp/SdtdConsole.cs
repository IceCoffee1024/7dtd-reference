using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

// Token: 0x020002C8 RID: 712
public class SdtdConsole : SingletonMonoBehaviour<SdtdConsole>
{
	// Token: 0x17000249 RID: 585
	// (get) Token: 0x06001492 RID: 5266 RVA: 0x0007C3AC File Offset: 0x0007A5AC
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

	// Token: 0x06001493 RID: 5267 RVA: 0x0007C3CD File Offset: 0x0007A5CD
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void singletonAwake()
	{
		Log.LogCallbacksExtended += this.LogCallback;
		this.m_CommandsReadOnly = new ReadOnlyCollection<IConsoleCommand>(this.m_Commands);
		PreserveCheckPatch.Enable();
	}

	// Token: 0x06001494 RID: 5268 RVA: 0x0007C3F8 File Offset: 0x0007A5F8
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

	// Token: 0x06001495 RID: 5269 RVA: 0x0007C464 File Offset: 0x0007A664
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

	// Token: 0x06001496 RID: 5270 RVA: 0x0007C534 File Offset: 0x0007A734
	public void Output(string _line)
	{
		List<string> currentCommandOutputList = this.m_currentCommandOutputList;
		if (currentCommandOutputList == null)
		{
			return;
		}
		currentCommandOutputList.Add(_line);
	}

	// Token: 0x06001497 RID: 5271 RVA: 0x0007C547 File Offset: 0x0007A747
	public void Output(string _format, params object[] _args)
	{
		this.Output(string.Format(_format, _args));
	}

	// Token: 0x06001498 RID: 5272 RVA: 0x0007C558 File Offset: 0x0007A758
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

	// Token: 0x06001499 RID: 5273 RVA: 0x0007C604 File Offset: 0x0007A804
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

	// Token: 0x0600149A RID: 5274 RVA: 0x0007C764 File Offset: 0x0007A964
	public void RegisterServer(IConsoleServer _server)
	{
		if (_server != null)
		{
			this.m_Servers.Add(_server);
		}
	}

	// Token: 0x0600149B RID: 5275 RVA: 0x0007C778 File Offset: 0x0007A978
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

	// Token: 0x0600149C RID: 5276 RVA: 0x0007C7DC File Offset: 0x0007A9DC
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

	// Token: 0x0600149D RID: 5277 RVA: 0x0007C816 File Offset: 0x0007AA16
	public IList<IConsoleCommand> GetCommands()
	{
		return this.m_CommandsReadOnly;
	}

	// Token: 0x0600149E RID: 5278 RVA: 0x0007C820 File Offset: 0x0007AA20
	public void ExecuteAsync(string _command, IConsoleConnection _sender)
	{
		SdtdConsole.CommandInstance commandInstance = new SdtdConsole.CommandInstance(_command, _sender);
		List<SdtdConsole.CommandInstance> commandsToExecuteAsync = this.m_commandsToExecuteAsync;
		lock (commandsToExecuteAsync)
		{
			this.m_commandsToExecuteAsync.Add(commandInstance);
		}
	}

	// Token: 0x0600149F RID: 5279 RVA: 0x0007C870 File Offset: 0x0007AA70
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

	// Token: 0x060014A0 RID: 5280 RVA: 0x0007C8AC File Offset: 0x0007AAAC
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

	// Token: 0x060014A1 RID: 5281 RVA: 0x0007CA80 File Offset: 0x0007AC80
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

	// Token: 0x04000DA0 RID: 3488
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly List<IConsoleCommand> m_Commands = new List<IConsoleCommand>();

	// Token: 0x04000DA1 RID: 3489
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly Dictionary<string, IConsoleCommand> m_CommandsAllVariants = new CaseInsensitiveStringDictionary<IConsoleCommand>();

	// Token: 0x04000DA2 RID: 3490
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public ReadOnlyCollection<IConsoleCommand> m_CommandsReadOnly;

	// Token: 0x04000DA3 RID: 3491
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly List<IConsoleServer> m_Servers = new List<IConsoleServer>();

	// Token: 0x04000DA4 RID: 3492
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly List<SdtdConsole.CommandInstance> m_commandsToExecuteAsync = new List<SdtdConsole.CommandInstance>();

	// Token: 0x04000DA5 RID: 3493
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly List<string> m_currentCommandOutputList = new List<string>();

	// Token: 0x04000DA6 RID: 3494
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int hideCommandExecutionLog = -1;

	// Token: 0x04000DA7 RID: 3495
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly List<string> tokenizedCommandList = new List<string>(16);

	// Token: 0x020002C9 RID: 713
	[PublicizedFrom(EAccessModifier.Private)]
	public class CommandInstance
	{
		// Token: 0x060014A3 RID: 5283 RVA: 0x0007CC12 File Offset: 0x0007AE12
		public CommandInstance(string _command, IConsoleConnection _sender)
		{
			this.command = _command;
			this.sender = _sender;
		}

		// Token: 0x04000DA8 RID: 3496
		public readonly string command;

		// Token: 0x04000DA9 RID: 3497
		public readonly IConsoleConnection sender;
	}
}
