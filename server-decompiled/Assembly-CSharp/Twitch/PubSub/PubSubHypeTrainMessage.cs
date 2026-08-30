using System;
using UnityEngine;

namespace Twitch.PubSub
{
	// Token: 0x02001883 RID: 6275
	public class PubSubHypeTrainMessage : BasePubSubMessage
	{
		// Token: 0x0600C198 RID: 49560 RVA: 0x00479440 File Offset: 0x00477640
		public static PubSubHypeTrainMessage Deserialize(string message)
		{
			Debug.LogWarning("HypeTrainMessage:\n" + message);
			return new PubSubHypeTrainMessage();
		}

		// Token: 0x0400927B RID: 37499
		public PubSubHypeTrainMessage.HypeTrainData data;

		// Token: 0x02001884 RID: 6276
		public class HypeTrainData : EventArgs
		{
			// Token: 0x170017C6 RID: 6086
			// (get) Token: 0x0600C19A RID: 49562 RVA: 0x00479457 File Offset: 0x00477657
			// (set) Token: 0x0600C19B RID: 49563 RVA: 0x0047945F File Offset: 0x0047765F
			public string user_name { get; set; }
		}
	}
}
