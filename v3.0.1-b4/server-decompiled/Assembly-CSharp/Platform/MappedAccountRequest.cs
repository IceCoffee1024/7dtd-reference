using System;

namespace Platform
{
	// Token: 0x02001B98 RID: 7064
	public class MappedAccountRequest
	{
		// Token: 0x0600D209 RID: 53769 RVA: 0x004C15CE File Offset: 0x004BF7CE
		public MappedAccountRequest(PlatformUserIdentifierAbs _id, EPlatformIdentifier _platform)
		{
			this.Id = _id;
			this.Platform = _platform;
		}

		// Token: 0x04009FF1 RID: 40945
		public readonly PlatformUserIdentifierAbs Id;

		// Token: 0x04009FF2 RID: 40946
		public readonly EPlatformIdentifier Platform;

		// Token: 0x04009FF3 RID: 40947
		public string MappedAccountId;

		// Token: 0x04009FF4 RID: 40948
		public string DisplayName;

		// Token: 0x04009FF5 RID: 40949
		public MappedAccountQueryResult Result;
	}
}
