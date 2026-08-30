using System;

namespace Platform
{
	// Token: 0x02001BC2 RID: 7106
	public class MappedAccountReverseRequest
	{
		// Token: 0x0600D39E RID: 54174 RVA: 0x004C79D8 File Offset: 0x004C5BD8
		public MappedAccountReverseRequest(EPlatformIdentifier _platform, string _id)
		{
			this.Platform = _platform;
			this.Id = _id;
		}

		// Token: 0x0400A127 RID: 41255
		public readonly EPlatformIdentifier Platform;

		// Token: 0x0400A128 RID: 41256
		public readonly string Id;

		// Token: 0x0400A129 RID: 41257
		public MappedAccountQueryResult Result;

		// Token: 0x0400A12A RID: 41258
		public PlatformUserIdentifierAbs PlatformId;
	}
}
