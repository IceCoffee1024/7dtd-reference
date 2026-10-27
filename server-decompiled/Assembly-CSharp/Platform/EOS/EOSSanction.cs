using System;
using Epic.OnlineServices;

namespace Platform.EOS
{
	// Token: 0x02001D6F RID: 7535
	[PublicizedFrom(EAccessModifier.Internal)]
	public struct EOSSanction
	{
		// Token: 0x17001C6B RID: 7275
		// (get) Token: 0x0600DF07 RID: 57095 RVA: 0x004F5A2F File Offset: 0x004F3C2F
		public readonly string ReferenceId { get; }

		// Token: 0x0600DF08 RID: 57096 RVA: 0x004F5A37 File Offset: 0x004F3C37
		public EOSSanction(DateTime? expiryDate, Utf8String referenceId)
		{
			this.ReferenceId = referenceId;
			this.expiry = expiryDate.GetValueOrDefault();
		}

		// Token: 0x0400A867 RID: 43111
		public DateTime expiry;
	}
}
