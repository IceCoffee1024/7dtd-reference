using System;

namespace Platform
{
	// Token: 0x02001BF4 RID: 7156
	public class MappedAccountReverseRequest
	{
		// Token: 0x0600D516 RID: 54550 RVA: 0x004CC4C0 File Offset: 0x004CA6C0
		public MappedAccountReverseRequest(EPlatformIdentifier _platform, string _id)
		{
			this.Platform = _platform;
			this.Id = _id;
		}

		// Token: 0x0400A1FC RID: 41468
		public readonly EPlatformIdentifier Platform;

		// Token: 0x0400A1FD RID: 41469
		public readonly string Id;

		// Token: 0x0400A1FE RID: 41470
		public MappedAccountQueryResult Result;

		// Token: 0x0400A1FF RID: 41471
		public PlatformUserIdentifierAbs PlatformId;
	}
}
