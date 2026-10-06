using System;

namespace Platform
{
	// Token: 0x02001C19 RID: 7193
	public interface IPlatformUser
	{
		// Token: 0x17001B02 RID: 6914
		// (get) Token: 0x0600D602 RID: 54786
		PlatformUserIdentifierAbs PrimaryId { get; }

		// Token: 0x17001B03 RID: 6915
		// (get) Token: 0x0600D603 RID: 54787
		// (set) Token: 0x0600D604 RID: 54788
		PlatformUserIdentifierAbs NativeId { get; set; }
	}
}
