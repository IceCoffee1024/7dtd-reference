using System;
using System.Globalization;
using System.Net;

namespace Twitch
{
	// Token: 0x020017BA RID: 6074
	public class TwitchExtensionCommand
	{
		// Token: 0x0600BC18 RID: 48152 RVA: 0x0045CBAC File Offset: 0x0045ADAC
		public TwitchExtensionCommand(HttpListenerRequest _req)
		{
			this.userId = StringParsers.ParseSInt32(_req.QueryString.Get(0), 0, -1, NumberStyles.Integer);
			this.command = "#" + _req.QueryString.Get(1);
			this.id = StringParsers.ParseSInt32(_req.QueryString.Get(2), 0, -1, NumberStyles.Integer);
			this.isRerun = StringParsers.ParseBool(_req.QueryString.Get(3), 0, -1, true);
			Log.Out(string.Format("{0}: {1} : {2}", this.userId, this.command, this.id));
		}

		// Token: 0x04008CDD RID: 36061
		public int userId;

		// Token: 0x04008CDE RID: 36062
		public string command;

		// Token: 0x04008CDF RID: 36063
		public int id;

		// Token: 0x04008CE0 RID: 36064
		public bool isRerun;
	}
}
