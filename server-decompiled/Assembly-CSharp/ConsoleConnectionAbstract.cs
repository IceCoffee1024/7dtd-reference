using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020002CD RID: 717
public abstract class ConsoleConnectionAbstract : IConsoleConnection
{
	// Token: 0x0600148D RID: 5261
	public abstract void SendLines(List<string> _output);

	// Token: 0x0600148E RID: 5262
	public abstract void SendLine(string _text);

	// Token: 0x0600148F RID: 5263
	public abstract void SendLog(string _formattedMessage, string _plainMessage, string _trace, LogType _type, DateTime _timestamp, long _uptime);

	// Token: 0x06001490 RID: 5264
	public abstract string GetDescription();

	// Token: 0x06001491 RID: 5265 RVA: 0x0007CAF2 File Offset: 0x0007ACF2
	public void EnableLogLevel(LogType _type, bool _enable)
	{
		if (_enable)
		{
			this.enabledLogLevels.Add(_type);
			return;
		}
		this.enabledLogLevels.Remove(_type);
	}

	// Token: 0x06001492 RID: 5266 RVA: 0x0007CB12 File Offset: 0x0007AD12
	public bool IsLogLevelEnabled(LogType _type)
	{
		return this.enabledLogLevels.Contains(_type);
	}

	// Token: 0x06001493 RID: 5267 RVA: 0x0007CB20 File Offset: 0x0007AD20
	[PublicizedFrom(EAccessModifier.Protected)]
	public ConsoleConnectionAbstract()
	{
	}

	// Token: 0x04000DAA RID: 3498
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
