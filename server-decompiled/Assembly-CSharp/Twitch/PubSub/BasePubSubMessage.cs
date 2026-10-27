using System;

namespace Twitch.PubSub
{
	// Token: 0x020018D5 RID: 6357
	public class BasePubSubMessage
	{
		// Token: 0x17001898 RID: 6296
		// (get) Token: 0x0600C3C6 RID: 50118 RVA: 0x0048113B File Offset: 0x0047F33B
		// (set) Token: 0x0600C3C7 RID: 50119 RVA: 0x00481143 File Offset: 0x0047F343
		public string type
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001899 RID: 6297
		// (get) Token: 0x0600C3C8 RID: 50120 RVA: 0x0048114C File Offset: 0x0047F34C
		// (set) Token: 0x0600C3C9 RID: 50121 RVA: 0x00481154 File Offset: 0x0047F354
		public string nonce { get; set; } = Guid.NewGuid().ToString().Replace("-", "");

		// Token: 0x0600C3CA RID: 50122 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void ReceiveData(string data)
		{
		}
	}
}
