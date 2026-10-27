using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000242 RID: 578
[Preserve]
public class ConsoleCmdLogLevel : ConsoleCmdAbstract
{
	// Token: 0x0600113C RID: 4412 RVA: 0x0006D9CB File Offset: 0x0006BBCB
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "loglevel" };
	}

	// Token: 0x170001A9 RID: 425
	// (get) Token: 0x0600113D RID: 4413 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x0600113E RID: 4414 RVA: 0x0006D9DB File Offset: 0x0006BBDB
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Telnet/Web only: Select which types of log messages are shown";
	}

	// Token: 0x0600113F RID: 4415 RVA: 0x0006D9E2 File Offset: 0x0006BBE2
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "Select which types of log messages are shown on the connection\nyou enter this command. By default all log messages are printed\non every connection.\nUsage: loglevel <loglevel name> <true/false>\nLog levels: INF, WRN, ERR, EXC or ALL\nExample: Disable display of WRN messages: loglevel WRN false";
	}

	// Token: 0x06001140 RID: 4416 RVA: 0x0006D9EC File Offset: 0x0006BBEC
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (_senderInfo.IsLocalGame || _senderInfo.RemoteClientInfo != null)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("This command can only be used through a management interface like Telnet or the Web Panel.");
			return;
		}
		if (_params.Count != 2)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Wrong number of arguments.");
			return;
		}
		LogType logType = LogType.Assert;
		bool flag = false;
		if (_params[0].EqualsCaseInsensitive("all"))
		{
			flag = true;
		}
		else if (_params[0].EqualsCaseInsensitive("inf"))
		{
			logType = LogType.Log;
		}
		else if (_params[0].EqualsCaseInsensitive("wrn"))
		{
			logType = LogType.Warning;
		}
		else if (_params[0].EqualsCaseInsensitive("err"))
		{
			logType = LogType.Error;
		}
		else
		{
			if (!_params[0].EqualsCaseInsensitive("exc"))
			{
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output("\"" + _params[0] + "\" is not a valid loglevel name.");
				return;
			}
			logType = LogType.Exception;
		}
		bool flag2;
		try
		{
			flag2 = ConsoleHelper.ParseParamBool(_params[1], false);
		}
		catch (ArgumentException)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("\"" + _params[1] + "\" is not a valid boolean value.");
			return;
		}
		if (flag)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output((flag2 ? "Enabling" : "Disabling") + " all loglevels on this connection.");
			_senderInfo.NetworkConnection.EnableLogLevel(LogType.Assert, flag2);
			_senderInfo.NetworkConnection.EnableLogLevel(LogType.Error, flag2);
			_senderInfo.NetworkConnection.EnableLogLevel(LogType.Exception, flag2);
			_senderInfo.NetworkConnection.EnableLogLevel(LogType.Log, flag2);
			_senderInfo.NetworkConnection.EnableLogLevel(LogType.Warning, flag2);
			return;
		}
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output((flag2 ? "Enabling" : "Disabling") + " loglevel \"" + logType.ToString() + "\" on this connection.");
		_senderInfo.NetworkConnection.EnableLogLevel(logType, flag2);
	}
}
