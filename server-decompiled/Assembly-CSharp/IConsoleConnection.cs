using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020002CD RID: 717
public interface IConsoleConnection
{
	// Token: 0x06001499 RID: 5273
	void SendLines(List<string> _output);

	// Token: 0x0600149A RID: 5274
	void SendLine(string _text);

	// Token: 0x0600149B RID: 5275
	void SendLog(string _formattedMessage, string _plainMessage, string _trace, LogType _type, DateTime _timestamp, long _uptime);

	// Token: 0x0600149C RID: 5276
	void EnableLogLevel(LogType _type, bool _enable);

	// Token: 0x0600149D RID: 5277
	string GetDescription();
}
