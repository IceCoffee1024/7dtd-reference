using System;
using Epic.OnlineServices;

namespace Platform.EOS
{
	// Token: 0x02001D3E RID: 7486
	[PublicizedFrom(EAccessModifier.Internal)]
	public struct EOSSanction
	{
		// Token: 0x17001C23 RID: 7203
		// (get) Token: 0x0600DDF3 RID: 56819 RVA: 0x004F1E8F File Offset: 0x004F008F
		public readonly string ReferenceId { get; }

		// Token: 0x0600DDF4 RID: 56820 RVA: 0x004F1E97 File Offset: 0x004F0097
		public EOSSanction(DateTime? expiryDate, Utf8String referenceId)
		{
			this.ReferenceId = referenceId;
			this.expiry = expiryDate.GetValueOrDefault();
		}

		// Token: 0x0400A77E RID: 42878
		public DateTime expiry;
	}
}
