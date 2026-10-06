using System;
using UnityEngine;

namespace Twitch.PubSub
{
	// Token: 0x020018AF RID: 6319
	public class PubSubHypeTrainMessage : BasePubSubMessage
	{
		// Token: 0x0600C2F0 RID: 49904 RVA: 0x0047D97C File Offset: 0x0047BB7C
		public static PubSubHypeTrainMessage Deserialize(string message)
		{
			Debug.LogWarning("HypeTrainMessage:\n" + message);
			return new PubSubHypeTrainMessage();
		}

		// Token: 0x04009341 RID: 37697
		public PubSubHypeTrainMessage.HypeTrainData data;

		// Token: 0x020018B0 RID: 6320
		public class HypeTrainData : EventArgs
		{
			// Token: 0x17001868 RID: 6248
			// (get) Token: 0x0600C2F2 RID: 49906 RVA: 0x0047D993 File Offset: 0x0047BB93
			// (set) Token: 0x0600C2F3 RID: 49907 RVA: 0x0047D99B File Offset: 0x0047BB9B
			public string user_name { get; set; }
		}
	}
}
