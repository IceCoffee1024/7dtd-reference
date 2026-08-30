using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020002C4 RID: 708
public abstract class ConsoleConnectionAbstract : IConsoleConnection
{
	// Token: 0x0600147A RID: 5242
	public abstract void SendLines(List<string> _output);

	// Token: 0x0600147B RID: 5243
	public abstract void SendLine(string _text);

	// Token: 0x0600147C RID: 5244
	public abstract void SendLog(string _formattedMessage, string _plainMessage, string _trace, LogType _type, DateTime _timestamp, long _uptime);

	// Token: 0x0600147D RID: 5245
	public abstract string GetDescription();

	// Token: 0x0600147E RID: 5246 RVA: 0x0007C016 File Offset: 0x0007A216
	public void EnableLogLevel(LogType _type, bool _enable)
	{
		if (_enable)
		{
			this.enabledLogLevels.Add(_type);
			return;
		}
		this.enabledLogLevels.Remove(_type);
	}

	// Token: 0x0600147F RID: 5247 RVA: 0x0007C036 File Offset: 0x0007A236
	public bool IsLogLevelEnabled(LogType _type)
	{
		return this.enabledLogLevels.Contains(_type);
	}

	// Token: 0x06001480 RID: 5248 RVA: 0x0007C044 File Offset: 0x0007A244
	[PublicizedFrom(EAccessModifier.Protected)]
	public ConsoleConnectionAbstract()
	{
	}

	// Token: 0x04000D9F RID: 3487
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
