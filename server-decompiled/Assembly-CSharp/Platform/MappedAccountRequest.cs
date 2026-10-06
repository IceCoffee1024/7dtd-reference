using System;

namespace Platform
{
	// Token: 0x02001BF1 RID: 7153
	public class MappedAccountRequest
	{
		// Token: 0x0600D50D RID: 54541 RVA: 0x004CC4AA File Offset: 0x004CA6AA
		public MappedAccountRequest(PlatformUserIdentifierAbs _id, EPlatformIdentifier _platform)
		{
			this.Id = _id;
			this.Platform = _platform;
		}

		// Token: 0x0400A1F7 RID: 41463
		public readonly PlatformUserIdentifierAbs Id;

		// Token: 0x0400A1F8 RID: 41464
		public readonly EPlatformIdentifier Platform;

		// Token: 0x0400A1F9 RID: 41465
		public string MappedAccountId;

		// Token: 0x0400A1FA RID: 41466
		public string DisplayName;

		// Token: 0x0400A1FB RID: 41467
		public MappedAccountQueryResult Result;
	}
}
