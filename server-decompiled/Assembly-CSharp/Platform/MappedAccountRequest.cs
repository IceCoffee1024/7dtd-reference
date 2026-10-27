using System;

namespace Platform
{
	// Token: 0x02001C22 RID: 7202
	public class MappedAccountRequest
	{
		// Token: 0x0600D621 RID: 54817 RVA: 0x004CFFD2 File Offset: 0x004CE1D2
		public MappedAccountRequest(PlatformUserIdentifierAbs _id, EPlatformIdentifier _platform)
		{
			this.Id = _id;
			this.Platform = _platform;
		}

		// Token: 0x0400A2E0 RID: 41696
		public readonly PlatformUserIdentifierAbs Id;

		// Token: 0x0400A2E1 RID: 41697
		public readonly EPlatformIdentifier Platform;

		// Token: 0x0400A2E2 RID: 41698
		public string MappedAccountId;

		// Token: 0x0400A2E3 RID: 41699
		public string DisplayName;

		// Token: 0x0400A2E4 RID: 41700
		public MappedAccountQueryResult Result;
	}
}
