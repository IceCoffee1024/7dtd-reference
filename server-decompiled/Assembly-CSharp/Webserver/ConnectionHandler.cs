using System;
using System.Collections.Generic;
using System.Net;

namespace Webserver
{
	// Token: 0x02001A91 RID: 6801
	public class ConnectionHandler
	{
		// Token: 0x0600CCC0 RID: 52416 RVA: 0x004A9230 File Offset: 0x004A7430
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

		// Token: 0x0600CCC1 RID: 52417 RVA: 0x004A9256 File Offset: 0x004A7456
		public void LogOut(string _sessionId)
		{
			this.connections.Remove(_sessionId);
		}

		// Token: 0x0600CCC2 RID: 52418 RVA: 0x004A9268 File Offset: 0x004A7468
		public WebConnection LogIn(IPAddress _ip, string _username, PlatformUserIdentifierAbs _userId, PlatformUserIdentifierAbs _crossUserId = null)
		{
			string text = Guid.NewGuid().ToString();
			WebConnection webConnection = new WebConnection(text, _ip, _username, _userId, _crossUserId);
			this.connections.Add(text, webConnection);
			return webConnection;
		}

		// Token: 0x0600CCC3 RID: 52419 RVA: 0x004A92A4 File Offset: 0x004A74A4
		public void SendLine(string _line)
		{
			foreach (KeyValuePair<string, WebConnection> keyValuePair in this.connections)
			{
				keyValuePair.Value.SendLine(_line);
			}
		}

		// Token: 0x04009B06 RID: 39686
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<string, WebConnection> connections = new Dictionary<string, WebConnection>();
	}
}
