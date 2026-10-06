using System;

namespace Platform.XBL
{
	// Token: 0x02001C5B RID: 7259
	public class XuidResolveRequest
	{
		// Token: 0x0600D77C RID: 55164 RVA: 0x004D5504 File Offset: 0x004D3704
		public XuidResolveRequest(PlatformUserIdentifierAbs id)
		{
			this.Id = id;
		}

		// Token: 0x0400A3DB RID: 41947
		public readonly PlatformUserIdentifierAbs Id;

		// Token: 0x0400A3DC RID: 41948
		public bool IsSuccess;

		// Token: 0x0400A3DD RID: 41949
		public ulong Xuid;
	}
}
