using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020002CF RID: 719
public interface IConsoleConnection
{
	// Token: 0x0600149D RID: 5277
	void SendLines(List<string> _output);

	// Token: 0x0600149E RID: 5278
	void SendLine(string _text);

	// Token: 0x0600149F RID: 5279
	void SendLog(string _formattedMessage, string _plainMessage, string _trace, LogType _type, DateTime _timestamp, long _uptime);

	// Token: 0x060014A0 RID: 5280
	void EnableLogLevel(LogType _type, bool _enable);

	// Token: 0x060014A1 RID: 5281
	string GetDescription();
}
