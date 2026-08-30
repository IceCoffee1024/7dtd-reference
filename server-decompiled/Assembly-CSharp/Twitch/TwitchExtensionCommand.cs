using System;
using System.Globalization;
using System.Net;

namespace Twitch
{
	// Token: 0x020017E0 RID: 6112
	public class TwitchExtensionCommand
	{
		// Token: 0x0600BD9D RID: 48541 RVA: 0x00462948 File Offset: 0x00460B48
		public TwitchExtensionCommand(HttpListenerRequest _req)
		{
			this.userId = StringParsers.ParseSInt32(_req.QueryString.Get(0), 0, -1, NumberStyles.Integer);
			this.command = "#" + _req.QueryString.Get(1);
			this.id = StringParsers.ParseSInt32(_req.QueryString.Get(2), 0, -1, NumberStyles.Integer);
			this.isRerun = StringParsers.ParseBool(_req.QueryString.Get(3), 0, -1, true);
			Log.Out(string.Format("{0}: {1} : {2}", this.userId, this.command, this.id));
		}

		// Token: 0x04008DF9 RID: 36345
		public int userId;

		// Token: 0x04008DFA RID: 36346
		public string command;

		// Token: 0x04008DFB RID: 36347
		public int id;

		// Token: 0x04008DFC RID: 36348
		public bool isRerun;
	}
}
