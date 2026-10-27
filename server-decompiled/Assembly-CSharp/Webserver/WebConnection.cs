using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

namespace Webserver
{
	// Token: 0x02001B24 RID: 6948
	public class WebConnection : ConsoleConnectionAbstract
	{
		// Token: 0x17001A5D RID: 6749
		// (get) Token: 0x0600D0EF RID: 53487 RVA: 0x004BB393 File Offset: 0x004B9593
		public string SessionID { get; }

		// Token: 0x17001A5E RID: 6750
		// (get) Token: 0x0600D0F0 RID: 53488 RVA: 0x004BB39B File Offset: 0x004B959B
		public IPAddress Endpoint { get; }

		// Token: 0x17001A5F RID: 6751
		// (get) Token: 0x0600D0F1 RID: 53489 RVA: 0x004BB3A3 File Offset: 0x004B95A3
		public string Username { get; }

		// Token: 0x17001A60 RID: 6752
		// (get) Token: 0x0600D0F2 RID: 53490 RVA: 0x004BB3AB File Offset: 0x004B95AB
		public PlatformUserIdentifierAbs UserId { get; }

		// Token: 0x17001A61 RID: 6753
		// (get) Token: 0x0600D0F3 RID: 53491 RVA: 0x004BB3B3 File Offset: 0x004B95B3
		public PlatformUserIdentifierAbs CrossplatformUserId { get; }

		// Token: 0x17001A62 RID: 6754
		// (get) Token: 0x0600D0F4 RID: 53492 RVA: 0x004BB3BB File Offset: 0x004B95BB
		public TimeSpan Age
		{
			get
			{
				return DateTime.Now - this.lastAction;
			}
		}

		// Token: 0x0600D0F5 RID: 53493 RVA: 0x004BB3D0 File Offset: 0x004B95D0
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

		// Token: 0x0600D0F6 RID: 53494 RVA: 0x004BB435 File Offset: 0x004B9635
		public void UpdateUsage()
		{
			this.lastAction = DateTime.Now;
		}

		// Token: 0x0600D0F7 RID: 53495 RVA: 0x004BB442 File Offset: 0x004B9642
		public override string GetDescription()
		{
			return this.conDescription;
		}

		// Token: 0x0600D0F8 RID: 53496 RVA: 0x000027FC File Offset: 0x000009FC
		public override void SendLine(string _text)
		{
		}

		// Token: 0x0600D0F9 RID: 53497 RVA: 0x000027FC File Offset: 0x000009FC
		public override void SendLines(List<string> _output)
		{
		}

		// Token: 0x0600D0FA RID: 53498 RVA: 0x000027FC File Offset: 0x000009FC
		public override void SendLog(string _formattedMsg, string _plainMsg, string _trace, LogType _type, DateTime _timestamp, long _uptime)
		{
		}

		// Token: 0x04009E30 RID: 40496
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly DateTime login;

		// Token: 0x04009E31 RID: 40497
		[PublicizedFrom(EAccessModifier.Private)]
		public DateTime lastAction;

		// Token: 0x04009E32 RID: 40498
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string conDescription;
	}
}
