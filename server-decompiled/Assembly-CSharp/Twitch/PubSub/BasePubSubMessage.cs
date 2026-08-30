using System;

namespace Twitch.PubSub
{
	// Token: 0x02001852 RID: 6226
	public class BasePubSubMessage
	{
		// Token: 0x1700173C RID: 5948
		// (get) Token: 0x0600BFD5 RID: 49109 RVA: 0x004734B7 File Offset: 0x004716B7
		// (set) Token: 0x0600BFD6 RID: 49110 RVA: 0x004734BF File Offset: 0x004716BF
		public string type
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x1700173D RID: 5949
		// (get) Token: 0x0600BFD7 RID: 49111 RVA: 0x004734C8 File Offset: 0x004716C8
		// (set) Token: 0x0600BFD8 RID: 49112 RVA: 0x004734D0 File Offset: 0x004716D0
		public string nonce { get; set; } = Guid.NewGuid().ToString().Replace("-", "");

		// Token: 0x0600BFD9 RID: 49113 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void ReceiveData(string data)
		{
		}
	}
}
