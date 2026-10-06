using System;

namespace Twitch.PubSub
{
	// Token: 0x020018A4 RID: 6308
	public class BasePubSubMessage
	{
		// Token: 0x17001850 RID: 6224
		// (get) Token: 0x0600C2B2 RID: 49842 RVA: 0x0047D78F File Offset: 0x0047B98F
		// (set) Token: 0x0600C2B3 RID: 49843 RVA: 0x0047D797 File Offset: 0x0047B997
		public string type
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001851 RID: 6225
		// (get) Token: 0x0600C2B4 RID: 49844 RVA: 0x0047D7A0 File Offset: 0x0047B9A0
		// (set) Token: 0x0600C2B5 RID: 49845 RVA: 0x0047D7A8 File Offset: 0x0047B9A8
		public string nonce { get; set; } = Guid.NewGuid().ToString().Replace("-", "");

		// Token: 0x0600C2B6 RID: 49846 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void ReceiveData(string data)
		{
		}
	}
}
