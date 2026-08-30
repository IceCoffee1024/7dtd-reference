using System;
using UnityEngine;

// Token: 0x020002CB RID: 715
public interface IConsoleServer
{
	// Token: 0x060014A1 RID: 5281
	void Disconnect();

	// Token: 0x060014A2 RID: 5282
	void SendLine(string _line);

	// Token: 0x060014A3 RID: 5283
	void SendLog(string _formattedMessage, string _plainMessage, string _trace, LogType _type, DateTime _timestamp, long _uptime);
}
