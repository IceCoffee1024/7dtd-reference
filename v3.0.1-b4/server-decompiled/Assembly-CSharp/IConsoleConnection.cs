using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020002C6 RID: 710
public interface IConsoleConnection
{
	// Token: 0x0600148A RID: 5258
	void SendLines(List<string> _output);

	// Token: 0x0600148B RID: 5259
	void SendLine(string _text);

	// Token: 0x0600148C RID: 5260
	void SendLog(string _formattedMessage, string _plainMessage, string _trace, LogType _type, DateTime _timestamp, long _uptime);

	// Token: 0x0600148D RID: 5261
	void EnableLogLevel(LogType _type, bool _enable);

	// Token: 0x0600148E RID: 5262
	string GetDescription();
}
