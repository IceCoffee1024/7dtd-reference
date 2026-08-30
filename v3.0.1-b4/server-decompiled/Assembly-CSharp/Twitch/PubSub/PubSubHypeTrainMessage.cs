using System;
using UnityEngine;

namespace Twitch.PubSub
{
	// Token: 0x0200185D RID: 6237
	public class PubSubHypeTrainMessage : BasePubSubMessage
	{
		// Token: 0x0600C013 RID: 49171 RVA: 0x004736A4 File Offset: 0x004718A4
		public static PubSubHypeTrainMessage Deserialize(string message)
		{
			Debug.LogWarning("HypeTrainMessage:\n" + message);
			return new PubSubHypeTrainMessage();
		}

		// Token: 0x0400915F RID: 37215
		public PubSubHypeTrainMessage.HypeTrainData data;

		// Token: 0x0200185E RID: 6238
		public class HypeTrainData : EventArgs
		{
			// Token: 0x17001754 RID: 5972
			// (get) Token: 0x0600C015 RID: 49173 RVA: 0x004736BB File Offset: 0x004718BB
			// (set) Token: 0x0600C016 RID: 49174 RVA: 0x004736C3 File Offset: 0x004718C3
			public string user_name { get; set; }
		}
	}
}
