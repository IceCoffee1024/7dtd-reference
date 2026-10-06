using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

namespace Webserver
{
	// Token: 0x02001AF3 RID: 6899
	public class WebConnection : ConsoleConnectionAbstract
	{
		// Token: 0x17001A15 RID: 6677
		// (get) Token: 0x0600CFDB RID: 53211 RVA: 0x004B7897 File Offset: 0x004B5A97
		public string SessionID { get; }

		// Token: 0x17001A16 RID: 6678
		// (get) Token: 0x0600CFDC RID: 53212 RVA: 0x004B789F File Offset: 0x004B5A9F
		public IPAddress Endpoint { get; }

		// Token: 0x17001A17 RID: 6679
		// (get) Token: 0x0600CFDD RID: 53213 RVA: 0x004B78A7 File Offset: 0x004B5AA7
		public string Username { get; }

		// Token: 0x17001A18 RID: 6680
		// (get) Token: 0x0600CFDE RID: 53214 RVA: 0x004B78AF File Offset: 0x004B5AAF
		public PlatformUserIdentifierAbs UserId { get; }

		// Token: 0x17001A19 RID: 6681
		// (get) Token: 0x0600CFDF RID: 53215 RVA: 0x004B78B7 File Offset: 0x004B5AB7
		public PlatformUserIdentifierAbs CrossplatformUserId { get; }

		// Token: 0x17001A1A RID: 6682
		// (get) Token: 0x0600CFE0 RID: 53216 RVA: 0x004B78BF File Offset: 0x004B5ABF
		public TimeSpan Age
		{
			get
			{
				return DateTime.Now - this.lastAction;
			}
		}

		// Token: 0x0600CFE1 RID: 53217 RVA: 0x004B78D4 File Offset: 0x004B5AD4
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

		// Token: 0x0600CFE2 RID: 53218 RVA: 0x004B7939 File Offset: 0x004B5B39
		public void UpdateUsage()
		{
			this.lastAction = DateTime.Now;
		}

		// Token: 0x0600CFE3 RID: 53219 RVA: 0x004B7946 File Offset: 0x004B5B46
		public override string GetDescription()
		{
			return this.conDescription;
		}

		// Token: 0x0600CFE4 RID: 53220 RVA: 0x000027FC File Offset: 0x000009FC
		public override void SendLine(string _text)
		{
		}

		// Token: 0x0600CFE5 RID: 53221 RVA: 0x000027FC File Offset: 0x000009FC
		public override void SendLines(List<string> _output)
		{
		}

		// Token: 0x0600CFE6 RID: 53222 RVA: 0x000027FC File Offset: 0x000009FC
		public override void SendLog(string _formattedMsg, string _plainMsg, string _trace, LogType _type, DateTime _timestamp, long _uptime)
		{
		}

		// Token: 0x04009D47 RID: 40263
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly DateTime login;

		// Token: 0x04009D48 RID: 40264
		[PublicizedFrom(EAccessModifier.Private)]
		public DateTime lastAction;

		// Token: 0x04009D49 RID: 40265
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string conDescription;
	}
}
