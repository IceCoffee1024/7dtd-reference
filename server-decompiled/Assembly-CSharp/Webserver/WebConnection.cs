using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

namespace Webserver
{
	// Token: 0x02001AA0 RID: 6816
	public class WebConnection : ConsoleConnectionAbstract
	{
		// Token: 0x17001901 RID: 6401
		// (get) Token: 0x0600CCFA RID: 52474 RVA: 0x004ACFD7 File Offset: 0x004AB1D7
		public string SessionID { get; }

		// Token: 0x17001902 RID: 6402
		// (get) Token: 0x0600CCFB RID: 52475 RVA: 0x004ACFDF File Offset: 0x004AB1DF
		public IPAddress Endpoint { get; }

		// Token: 0x17001903 RID: 6403
		// (get) Token: 0x0600CCFC RID: 52476 RVA: 0x004ACFE7 File Offset: 0x004AB1E7
		public string Username { get; }

		// Token: 0x17001904 RID: 6404
		// (get) Token: 0x0600CCFD RID: 52477 RVA: 0x004ACFEF File Offset: 0x004AB1EF
		public PlatformUserIdentifierAbs UserId { get; }

		// Token: 0x17001905 RID: 6405
		// (get) Token: 0x0600CCFE RID: 52478 RVA: 0x004ACFF7 File Offset: 0x004AB1F7
		public PlatformUserIdentifierAbs CrossplatformUserId { get; }

		// Token: 0x17001906 RID: 6406
		// (get) Token: 0x0600CCFF RID: 52479 RVA: 0x004ACFFF File Offset: 0x004AB1FF
		public TimeSpan Age
		{
			get
			{
				return DateTime.Now - this.lastAction;
			}
		}

		// Token: 0x0600CD00 RID: 52480 RVA: 0x004AD014 File Offset: 0x004AB214
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

		// Token: 0x0600CD01 RID: 52481 RVA: 0x004AD079 File Offset: 0x004AB279
		public void UpdateUsage()
		{
			this.lastAction = DateTime.Now;
		}

		// Token: 0x0600CD02 RID: 52482 RVA: 0x004AD086 File Offset: 0x004AB286
		public override string GetDescription()
		{
			return this.conDescription;
		}

		// Token: 0x0600CD03 RID: 52483 RVA: 0x000027FC File Offset: 0x000009FC
		public override void SendLine(string _text)
		{
		}

		// Token: 0x0600CD04 RID: 52484 RVA: 0x000027FC File Offset: 0x000009FC
		public override void SendLines(List<string> _output)
		{
		}

		// Token: 0x0600CD05 RID: 52485 RVA: 0x000027FC File Offset: 0x000009FC
		public override void SendLog(string _formattedMsg, string _plainMsg, string _trace, LogType _type, DateTime _timestamp, long _uptime)
		{
		}

		// Token: 0x04009B50 RID: 39760
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly DateTime login;

		// Token: 0x04009B51 RID: 39761
		[PublicizedFrom(EAccessModifier.Private)]
		public DateTime lastAction;

		// Token: 0x04009B52 RID: 39762
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string conDescription;
	}
}
