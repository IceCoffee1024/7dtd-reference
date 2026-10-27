using System;
using System.Collections.Generic;
using System.Net;

namespace Webserver
{
	// Token: 0x02001B15 RID: 6933
	public class ConnectionHandler
	{
		// Token: 0x0600D0B5 RID: 53429 RVA: 0x004B75EC File Offset: 0x004B57EC
		public WebConnection IsLoggedIn(string _sessionId, IPAddress _ip)
		{
			WebConnection webConnection;
			if (!this.connections.TryGetValue(_sessionId, out webConnection))
			{
				return null;
			}
			webConnection.UpdateUsage();
			return webConnection;
		}

		// Token: 0x0600D0B6 RID: 53430 RVA: 0x004B7612 File Offset: 0x004B5812
		public void LogOut(string _sessionId)
		{
			this.connections.Remove(_sessionId);
		}

		// Token: 0x0600D0B7 RID: 53431 RVA: 0x004B7624 File Offset: 0x004B5824
		public WebConnection LogIn(IPAddress _ip, string _username, PlatformUserIdentifierAbs _userId, PlatformUserIdentifierAbs _crossUserId = null)
		{
			string text = Guid.NewGuid().ToString();
			WebConnection webConnection = new WebConnection(text, _ip, _username, _userId, _crossUserId);
			this.connections.Add(text, webConnection);
			return webConnection;
		}

		// Token: 0x0600D0B8 RID: 53432 RVA: 0x004B7660 File Offset: 0x004B5860
		public void SendLine(string _line)
		{
			foreach (KeyValuePair<string, WebConnection> keyValuePair in this.connections)
			{
				keyValuePair.Value.SendLine(_line);
			}
		}

		// Token: 0x04009DE6 RID: 40422
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<string, WebConnection> connections = new Dictionary<string, WebConnection>();
	}
}
