using System;
using UnityEngine;

// Token: 0x020002C7 RID: 711
public interface IConsoleServer
{
	// Token: 0x0600148F RID: 5263
	void Disconnect();

	// Token: 0x06001490 RID: 5264
	void SendLine(string _line);

	// Token: 0x06001491 RID: 5265
	void SendLog(string _formattedMessage, string _plainMessage, string _trace, LogType _type, DateTime _timestamp, long _uptime);
}
