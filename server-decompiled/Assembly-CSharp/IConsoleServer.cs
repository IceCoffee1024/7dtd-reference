using System;
using UnityEngine;

// Token: 0x020002CE RID: 718
public interface IConsoleServer
{
	// Token: 0x0600149E RID: 5278
	void Disconnect();

	// Token: 0x0600149F RID: 5279
	void SendLine(string _line);

	// Token: 0x060014A0 RID: 5280
	void SendLog(string _formattedMessage, string _plainMessage, string _trace, LogType _type, DateTime _timestamp, long _uptime);
}
