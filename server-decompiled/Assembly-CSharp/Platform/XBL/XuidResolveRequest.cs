using System;

namespace Platform.XBL
{
	// Token: 0x02001C02 RID: 7170
	public class XuidResolveRequest
	{
		// Token: 0x0600D476 RID: 54390 RVA: 0x004CA628 File Offset: 0x004C8828
		public XuidResolveRequest(PlatformUserIdentifierAbs id)
		{
			this.Id = id;
		}

		// Token: 0x0400A1D5 RID: 41429
		public readonly PlatformUserIdentifierAbs Id;

		// Token: 0x0400A1D6 RID: 41430
		public bool IsSuccess;

		// Token: 0x0400A1D7 RID: 41431
		public ulong Xuid;
	}
}
