using System;

namespace Platform
{
	// Token: 0x02001BEA RID: 7146
	public class UserDetailsRequest
	{
		// Token: 0x0600D4F7 RID: 54519 RVA: 0x004CC479 File Offset: 0x004CA679
		public UserDetailsRequest(PlatformUserIdentifierAbs id)
		{
			this.Id = id;
			this.NativePlatform = id.PlatformIdentifier;
		}

		// Token: 0x0600D4F8 RID: 54520 RVA: 0x004CC494 File Offset: 0x004CA694
		public UserDetailsRequest(PlatformUserIdentifierAbs id, EPlatformIdentifier platform)
		{
			this.Id = id;
			this.NativePlatform = platform;
		}

		// Token: 0x0400A1EE RID: 41454
		public readonly PlatformUserIdentifierAbs Id;

		// Token: 0x0400A1EF RID: 41455
		public readonly EPlatformIdentifier NativePlatform;

		// Token: 0x0400A1F0 RID: 41456
		public PlatformUserDetails details;

		// Token: 0x0400A1F1 RID: 41457
		public bool IsSuccess;
	}
}
