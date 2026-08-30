using System;

namespace Platform
{
	// Token: 0x02001B91 RID: 7057
	public class UserDetailsRequest
	{
		// Token: 0x0600D1F3 RID: 53747 RVA: 0x004C159D File Offset: 0x004BF79D
		public UserDetailsRequest(PlatformUserIdentifierAbs id)
		{
			this.Id = id;
			this.NativePlatform = id.PlatformIdentifier;
		}

		// Token: 0x0600D1F4 RID: 53748 RVA: 0x004C15B8 File Offset: 0x004BF7B8
		public UserDetailsRequest(PlatformUserIdentifierAbs id, EPlatformIdentifier platform)
		{
			this.Id = id;
			this.NativePlatform = platform;
		}

		// Token: 0x04009FE8 RID: 40936
		public readonly PlatformUserIdentifierAbs Id;

		// Token: 0x04009FE9 RID: 40937
		public readonly EPlatformIdentifier NativePlatform;

		// Token: 0x04009FEA RID: 40938
		public PlatformUserDetails details;

		// Token: 0x04009FEB RID: 40939
		public bool IsSuccess;
	}
}
