using System;

namespace Platform
{
	// Token: 0x02001C25 RID: 7205
	public class MappedAccountReverseRequest
	{
		// Token: 0x0600D62A RID: 54826 RVA: 0x004CFFE8 File Offset: 0x004CE1E8
		public MappedAccountReverseRequest(EPlatformIdentifier _platform, string _id)
		{
			this.Platform = _platform;
			this.Id = _id;
		}

		// Token: 0x0400A2E5 RID: 41701
		public readonly EPlatformIdentifier Platform;

		// Token: 0x0400A2E6 RID: 41702
		public readonly string Id;

		// Token: 0x0400A2E7 RID: 41703
		public MappedAccountQueryResult Result;

		// Token: 0x0400A2E8 RID: 41704
		public PlatformUserIdentifierAbs PlatformId;
	}
}
