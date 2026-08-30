using System;

namespace Platform
{
	// Token: 0x02001BBF RID: 7103
	public class MappedAccountRequest
	{
		// Token: 0x0600D395 RID: 54165 RVA: 0x004C79C2 File Offset: 0x004C5BC2
		public MappedAccountRequest(PlatformUserIdentifierAbs _id, EPlatformIdentifier _platform)
		{
			this.Id = _id;
			this.Platform = _platform;
		}

		// Token: 0x0400A122 RID: 41250
		public readonly PlatformUserIdentifierAbs Id;

		// Token: 0x0400A123 RID: 41251
		public readonly EPlatformIdentifier Platform;

		// Token: 0x0400A124 RID: 41252
		public string MappedAccountId;

		// Token: 0x0400A125 RID: 41253
		public string DisplayName;

		// Token: 0x0400A126 RID: 41254
		public MappedAccountQueryResult Result;
	}
}
