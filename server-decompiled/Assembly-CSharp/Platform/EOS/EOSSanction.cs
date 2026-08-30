using System;
using Epic.OnlineServices;

namespace Platform.EOS
{
	// Token: 0x02001D0C RID: 7436
	[PublicizedFrom(EAccessModifier.Internal)]
	public struct EOSSanction
	{
		// Token: 0x17001B7D RID: 7037
		// (get) Token: 0x0600DC78 RID: 56440 RVA: 0x004ED247 File Offset: 0x004EB447
		public readonly string ReferenceId { get; }

		// Token: 0x0600DC79 RID: 56441 RVA: 0x004ED24F File Offset: 0x004EB44F
		public EOSSanction(DateTime? expiryDate, Utf8String referenceId)
		{
			this.ReferenceId = referenceId;
			this.expiry = expiryDate.GetValueOrDefault();
		}

		// Token: 0x0400A6A7 RID: 42663
		public DateTime expiry;
	}
}
