using System;
using System.Collections.Generic;
using System.Net;

namespace Webserver
{
	// Token: 0x02001AB8 RID: 6840
	public class ConnectionHandler
	{
		// Token: 0x0600CE49 RID: 52809 RVA: 0x004AF624 File Offset: 0x004AD824
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

		// Token: 0x0600CE4A RID: 52810 RVA: 0x004AF64A File Offset: 0x004AD84A
		public void LogOut(string _sessionId)
		{
			this.connections.Remove(_sessionId);
		}

		// Token: 0x0600CE4B RID: 52811 RVA: 0x004AF65C File Offset: 0x004AD85C
		public WebConnection LogIn(IPAddress _ip, string _username, PlatformUserIdentifierAbs _userId, PlatformUserIdentifierAbs _crossUserId = null)
		{
			string text = Guid.NewGuid().ToString();
			WebConnection webConnection = new WebConnection(text, _ip, _username, _userId, _crossUserId);
			this.connections.Add(text, webConnection);
			return webConnection;
		}

		// Token: 0x0600CE4C RID: 52812 RVA: 0x004AF698 File Offset: 0x004AD898
		public void SendLine(string _line)
		{
			foreach (KeyValuePair<string, WebConnection> keyValuePair in this.connections)
			{
				keyValuePair.Value.SendLine(_line);
			}
		}

		// Token: 0x04009C37 RID: 39991
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<string, WebConnection> connections = new Dictionary<string, WebConnection>();
	}
}
