using System;

namespace Platform.XBL
{
	// Token: 0x02001C8C RID: 7308
	public class XuidResolveRequest
	{
		// Token: 0x0600D890 RID: 55440 RVA: 0x004D902C File Offset: 0x004D722C
		public XuidResolveRequest(PlatformUserIdentifierAbs id)
		{
			this.Id = id;
		}

		// Token: 0x0400A4C4 RID: 42180
		public readonly PlatformUserIdentifierAbs Id;

		// Token: 0x0400A4C5 RID: 42181
		public bool IsSuccess;

		// Token: 0x0400A4C6 RID: 42182
		public ulong Xuid;
	}
}
