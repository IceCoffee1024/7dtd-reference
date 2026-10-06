using System;
using System.Collections.Generic;
using System.Net;

namespace Webserver
{
	// Token: 0x02001AE4 RID: 6884
	public class ConnectionHandler
	{
		// Token: 0x0600CFA1 RID: 53153 RVA: 0x004B3AF0 File Offset: 0x004B1CF0
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

		// Token: 0x0600CFA2 RID: 53154 RVA: 0x004B3B16 File Offset: 0x004B1D16
		public void LogOut(string _sessionId)
		{
			this.connections.Remove(_sessionId);
		}

		// Token: 0x0600CFA3 RID: 53155 RVA: 0x004B3B28 File Offset: 0x004B1D28
		public WebConnection LogIn(IPAddress _ip, string _username, PlatformUserIdentifierAbs _userId, PlatformUserIdentifierAbs _crossUserId = null)
		{
			string text = Guid.NewGuid().ToString();
			WebConnection webConnection = new WebConnection(text, _ip, _username, _userId, _crossUserId);
			this.connections.Add(text, webConnection);
			return webConnection;
		}

		// Token: 0x0600CFA4 RID: 53156 RVA: 0x004B3B64 File Offset: 0x004B1D64
		public void SendLine(string _line)
		{
			foreach (KeyValuePair<string, WebConnection> keyValuePair in this.connections)
			{
				keyValuePair.Value.SendLine(_line);
			}
		}

		// Token: 0x04009CFD RID: 40189
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<string, WebConnection> connections = new Dictionary<string, WebConnection>();
	}
}
