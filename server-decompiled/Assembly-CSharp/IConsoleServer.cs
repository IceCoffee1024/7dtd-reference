using System;
using UnityEngine;

// Token: 0x020002D0 RID: 720
public interface IConsoleServer
{
	// Token: 0x060014A2 RID: 5282
	void Disconnect();

	// Token: 0x060014A3 RID: 5283
	void SendLine(string _line);

	// Token: 0x060014A4 RID: 5284
	void SendLog(string _formattedMessage, string _plainMessage, string _trace, LogType _type, DateTime _timestamp, long _uptime);
}
