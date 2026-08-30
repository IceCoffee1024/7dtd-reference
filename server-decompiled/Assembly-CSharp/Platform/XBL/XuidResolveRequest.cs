using System;

namespace Platform.XBL
{
	// Token: 0x02001C29 RID: 7209
	public class XuidResolveRequest
	{
		// Token: 0x0600D604 RID: 54788 RVA: 0x004D0A1C File Offset: 0x004CEC1C
		public XuidResolveRequest(PlatformUserIdentifierAbs id)
		{
			this.Id = id;
		}

		// Token: 0x0400A306 RID: 41734
		public readonly PlatformUserIdentifierAbs Id;

		// Token: 0x0400A307 RID: 41735
		public bool IsSuccess;

		// Token: 0x0400A308 RID: 41736
		public ulong Xuid;
	}
}
