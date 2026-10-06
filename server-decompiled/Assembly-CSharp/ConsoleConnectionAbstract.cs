using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020002CB RID: 715
public abstract class ConsoleConnectionAbstract : IConsoleConnection
{
	// Token: 0x06001489 RID: 5257
	public abstract void SendLines(List<string> _output);

	// Token: 0x0600148A RID: 5258
	public abstract void SendLine(string _text);

	// Token: 0x0600148B RID: 5259
	public abstract void SendLog(string _formattedMessage, string _plainMessage, string _trace, LogType _type, DateTime _timestamp, long _uptime);

	// Token: 0x0600148C RID: 5260
	public abstract string GetDescription();

	// Token: 0x0600148D RID: 5261 RVA: 0x0007C922 File Offset: 0x0007AB22
	public void EnableLogLevel(LogType _type, bool _enable)
	{
		if (_enable)
		{
			this.enabledLogLevels.Add(_type);
			return;
		}
		this.enabledLogLevels.Remove(_type);
	}

	// Token: 0x0600148E RID: 5262 RVA: 0x0007C942 File Offset: 0x0007AB42
	public bool IsLogLevelEnabled(LogType _type)
	{
		return this.enabledLogLevels.Contains(_type);
	}

	// Token: 0x0600148F RID: 5263 RVA: 0x0007C950 File Offset: 0x0007AB50
	[PublicizedFrom(EAccessModifier.Protected)]
	public ConsoleConnectionAbstract()
	{
	}

	// Token: 0x04000DA9 RID: 3497
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSet<LogType> enabledLogLevels = new HashSet<LogType>
	{
		LogType.Log,
		LogType.Warning,
		LogType.Error,
		LogType.Exception,
		LogType.Assert
	};
}
