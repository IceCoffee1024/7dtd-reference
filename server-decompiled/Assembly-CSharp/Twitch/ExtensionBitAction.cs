using System;

namespace Twitch
{
	// Token: 0x020017CF RID: 6095
	[Serializable]
	public class ExtensionBitAction : ExtensionAction
	{
		// Token: 0x04008D8F RID: 36239
		public string txn_id;

		// Token: 0x04008D90 RID: 36240
		public long time_created;

		// Token: 0x04008D91 RID: 36241
		public int cost;
	}
}
