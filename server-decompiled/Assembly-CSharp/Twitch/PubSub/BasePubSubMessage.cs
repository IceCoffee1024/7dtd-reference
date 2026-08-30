using System;

namespace Twitch.PubSub
{
	// Token: 0x02001878 RID: 6264
	public class BasePubSubMessage
	{
		// Token: 0x170017AE RID: 6062
		// (get) Token: 0x0600C15A RID: 49498 RVA: 0x00479253 File Offset: 0x00477453
		// (set) Token: 0x0600C15B RID: 49499 RVA: 0x0047925B File Offset: 0x0047745B
		public string type
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x170017AF RID: 6063
		// (get) Token: 0x0600C15C RID: 49500 RVA: 0x00479264 File Offset: 0x00477464
		// (set) Token: 0x0600C15D RID: 49501 RVA: 0x0047926C File Offset: 0x0047746C
		public string nonce { get; set; } = Guid.NewGuid().ToString().Replace("-", "");

		// Token: 0x0600C15E RID: 49502 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void ReceiveData(string data)
		{
		}
	}
}
