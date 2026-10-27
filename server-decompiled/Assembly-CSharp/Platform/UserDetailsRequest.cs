using System;

namespace Platform
{
	// Token: 0x02001C1B RID: 7195
	public class UserDetailsRequest
	{
		// Token: 0x0600D60B RID: 54795 RVA: 0x004CFFA1 File Offset: 0x004CE1A1
		public UserDetailsRequest(PlatformUserIdentifierAbs id)
		{
			this.Id = id;
			this.NativePlatform = id.PlatformIdentifier;
		}

		// Token: 0x0600D60C RID: 54796 RVA: 0x004CFFBC File Offset: 0x004CE1BC
		public UserDetailsRequest(PlatformUserIdentifierAbs id, EPlatformIdentifier platform)
		{
			this.Id = id;
			this.NativePlatform = platform;
		}

		// Token: 0x0400A2D7 RID: 41687
		public readonly PlatformUserIdentifierAbs Id;

		// Token: 0x0400A2D8 RID: 41688
		public readonly EPlatformIdentifier NativePlatform;

		// Token: 0x0400A2D9 RID: 41689
		public PlatformUserDetails details;

		// Token: 0x0400A2DA RID: 41690
		public bool IsSuccess;
	}
}
