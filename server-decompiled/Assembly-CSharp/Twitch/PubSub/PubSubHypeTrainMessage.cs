using System;
using UnityEngine;

namespace Twitch.PubSub
{
	// Token: 0x020018E0 RID: 6368
	public class PubSubHypeTrainMessage : BasePubSubMessage
	{
		// Token: 0x0600C404 RID: 50180 RVA: 0x00481328 File Offset: 0x0047F528
		public static PubSubHypeTrainMessage Deserialize(string message)
		{
			Debug.LogWarning("HypeTrainMessage:\n" + message);
			return new PubSubHypeTrainMessage();
		}

		// Token: 0x0400942A RID: 37930
		public PubSubHypeTrainMessage.HypeTrainData data;

		// Token: 0x020018E1 RID: 6369
		public class HypeTrainData : EventArgs
		{
			// Token: 0x170018B0 RID: 6320
			// (get) Token: 0x0600C406 RID: 50182 RVA: 0x0048133F File Offset: 0x0047F53F
			// (set) Token: 0x0600C407 RID: 50183 RVA: 0x00481347 File Offset: 0x0047F547
			public string user_name { get; set; }
		}
	}
}
