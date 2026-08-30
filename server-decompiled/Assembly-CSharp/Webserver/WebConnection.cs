using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

namespace Webserver
{
	// Token: 0x02001AC7 RID: 6855
	public class WebConnection : ConsoleConnectionAbstract
	{
		// Token: 0x17001973 RID: 6515
		// (get) Token: 0x0600CE83 RID: 52867 RVA: 0x004B33CB File Offset: 0x004B15CB
		public string SessionID { get; }

		// Token: 0x17001974 RID: 6516
		// (get) Token: 0x0600CE84 RID: 52868 RVA: 0x004B33D3 File Offset: 0x004B15D3
		public IPAddress Endpoint { get; }

		// Token: 0x17001975 RID: 6517
		// (get) Token: 0x0600CE85 RID: 52869 RVA: 0x004B33DB File Offset: 0x004B15DB
		public string Username { get; }

		// Token: 0x17001976 RID: 6518
		// (get) Token: 0x0600CE86 RID: 52870 RVA: 0x004B33E3 File Offset: 0x004B15E3
		public PlatformUserIdentifierAbs UserId { get; }

		// Token: 0x17001977 RID: 6519
		// (get) Token: 0x0600CE87 RID: 52871 RVA: 0x004B33EB File Offset: 0x004B15EB
		public PlatformUserIdentifierAbs CrossplatformUserId { get; }

		// Token: 0x17001978 RID: 6520
		// (get) Token: 0x0600CE88 RID: 52872 RVA: 0x004B33F3 File Offset: 0x004B15F3
		public TimeSpan Age
		{
			get
			{
				return DateTime.Now - this.lastAction;
			}
		}

		// Token: 0x0600CE89 RID: 52873 RVA: 0x004B3408 File Offset: 0x004B1608
		public WebConnection(string _sessionId, IPAddress _endpoint, string _username, PlatformUserIdentifierAbs _userId, PlatformUserIdentifierAbs _crossUserId = null)
		{
			this.SessionID = _sessionId;
			this.Endpoint = _endpoint;
			this.Username = _username;
			this.UserId = _userId;
			this.CrossplatformUserId = _crossUserId;
			this.login = DateTime.Now;
			this.lastAction = this.login;
			this.conDescription = string.Format("WebPanel from {0}", this.Endpoint);
		}

		// Token: 0x0600CE8A RID: 52874 RVA: 0x004B346D File Offset: 0x004B166D
		public void UpdateUsage()
		{
			this.lastAction = DateTime.Now;
		}

		// Token: 0x0600CE8B RID: 52875 RVA: 0x004B347A File Offset: 0x004B167A
		public override string GetDescription()
		{
			return this.conDescription;
		}

		// Token: 0x0600CE8C RID: 52876 RVA: 0x000027FC File Offset: 0x000009FC
		public override void SendLine(string _text)
		{
		}

		// Token: 0x0600CE8D RID: 52877 RVA: 0x000027FC File Offset: 0x000009FC
		public override void SendLines(List<string> _output)
		{
		}

		// Token: 0x0600CE8E RID: 52878 RVA: 0x000027FC File Offset: 0x000009FC
		public override void SendLog(string _formattedMsg, string _plainMsg, string _trace, LogType _type, DateTime _timestamp, long _uptime)
		{
		}

		// Token: 0x04009C81 RID: 40065
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly DateTime login;

		// Token: 0x04009C82 RID: 40066
		[PublicizedFrom(EAccessModifier.Private)]
		public DateTime lastAction;

		// Token: 0x04009C83 RID: 40067
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string conDescription;
	}
}
