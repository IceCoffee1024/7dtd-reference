using System;
using System.Globalization;
using System.Net;

namespace Twitch
{
	// Token: 0x0200183D RID: 6205
	public class TwitchExtensionCommand
	{
		// Token: 0x0600C009 RID: 49161 RVA: 0x0046A830 File Offset: 0x00468A30
		public TwitchExtensionCommand(HttpListenerRequest _req)
		{
			this.userId = StringParsers.ParseSInt32(_req.QueryString.Get(0), 0, -1, NumberStyles.Integer);
			this.command = "#" + _req.QueryString.Get(1);
			this.id = StringParsers.ParseSInt32(_req.QueryString.Get(2), 0, -1, NumberStyles.Integer);
			this.isRerun = StringParsers.ParseBool(_req.QueryString.Get(3), 0, -1, true);
			Log.Out(string.Format("{0}: {1} : {2}", this.userId, this.command, this.id));
		}

		// Token: 0x04008FA8 RID: 36776
		public int userId;

		// Token: 0x04008FA9 RID: 36777
		public string command;

		// Token: 0x04008FAA RID: 36778
		public int id;

		// Token: 0x04008FAB RID: 36779
		public bool isRerun;
	}
}
