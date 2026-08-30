using System;

namespace Platform
{
	// Token: 0x02001B9B RID: 7067
	public class MappedAccountReverseRequest
	{
		// Token: 0x0600D212 RID: 53778 RVA: 0x004C15E4 File Offset: 0x004BF7E4
		public MappedAccountReverseRequest(EPlatformIdentifier _platform, string _id)
		{
			this.Platform = _platform;
			this.Id = _id;
		}

		// Token: 0x04009FF6 RID: 40950
		public readonly EPlatformIdentifier Platform;

		// Token: 0x04009FF7 RID: 40951
		public readonly string Id;

		// Token: 0x04009FF8 RID: 40952
		public MappedAccountQueryResult Result;

		// Token: 0x04009FF9 RID: 40953
		public PlatformUserIdentifierAbs PlatformId;
	}
}
