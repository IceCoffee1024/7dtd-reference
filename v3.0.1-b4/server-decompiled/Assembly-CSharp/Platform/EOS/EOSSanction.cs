using System;
using Epic.OnlineServices;

namespace Platform.EOS
{
	// Token: 0x02001CE5 RID: 7397
	[PublicizedFrom(EAccessModifier.Internal)]
	public struct EOSSanction
	{
		// Token: 0x17001B0B RID: 6923
		// (get) Token: 0x0600DADC RID: 56028 RVA: 0x004E6C07 File Offset: 0x004E4E07
		public readonly string ReferenceId { get; }

		// Token: 0x0600DADD RID: 56029 RVA: 0x004E6C0F File Offset: 0x004E4E0F
		public EOSSanction(DateTime? expiryDate, Utf8String referenceId)
		{
			this.ReferenceId = referenceId;
			this.expiry = expiryDate.GetValueOrDefault();
		}

		// Token: 0x0400A574 RID: 42356
		public DateTime expiry;
	}
}
