using System;

namespace Platform
{
	// Token: 0x02001BB8 RID: 7096
	public class UserDetailsRequest
	{
		// Token: 0x0600D37F RID: 54143 RVA: 0x004C7991 File Offset: 0x004C5B91
		public UserDetailsRequest(PlatformUserIdentifierAbs id)
		{
			this.Id = id;
			this.NativePlatform = id.PlatformIdentifier;
		}

		// Token: 0x0600D380 RID: 54144 RVA: 0x004C79AC File Offset: 0x004C5BAC
		public UserDetailsRequest(PlatformUserIdentifierAbs id, EPlatformIdentifier platform)
		{
			this.Id = id;
			this.NativePlatform = platform;
		}

		// Token: 0x0400A119 RID: 41241
		public readonly PlatformUserIdentifierAbs Id;

		// Token: 0x0400A11A RID: 41242
		public readonly EPlatformIdentifier NativePlatform;

		// Token: 0x0400A11B RID: 41243
		public PlatformUserDetails details;

		// Token: 0x0400A11C RID: 41244
		public bool IsSuccess;
	}
}
