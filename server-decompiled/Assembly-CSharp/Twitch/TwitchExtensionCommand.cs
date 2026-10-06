using System;
using System.Globalization;
using System.Net;

namespace Twitch
{
	// Token: 0x0200180C RID: 6156
	public class TwitchExtensionCommand
	{
		// Token: 0x0600BEF5 RID: 48885 RVA: 0x00466E84 File Offset: 0x00465084
		public TwitchExtensionCommand(HttpListenerRequest _req)
		{
			this.userId = StringParsers.ParseSInt32(_req.QueryString.Get(0), 0, -1, NumberStyles.Integer);
			this.command = "#" + _req.QueryString.Get(1);
			this.id = StringParsers.ParseSInt32(_req.QueryString.Get(2), 0, -1, NumberStyles.Integer);
			this.isRerun = StringParsers.ParseBool(_req.QueryString.Get(3), 0, -1, true);
			Log.Out(string.Format("{0}: {1} : {2}", this.userId, this.command, this.id));
		}

		// Token: 0x04008EBF RID: 36543
		public int userId;

		// Token: 0x04008EC0 RID: 36544
		public string command;

		// Token: 0x04008EC1 RID: 36545
		public int id;

		// Token: 0x04008EC2 RID: 36546
		public bool isRerun;
	}
}
