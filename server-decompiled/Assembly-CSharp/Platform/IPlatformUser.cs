using System;

namespace Platform
{
	// Token: 0x02001BE7 RID: 7143
	public interface IPlatformUser
	{
		// Token: 0x17001A5C RID: 6748
		// (get) Token: 0x0600D48A RID: 54410
		PlatformUserIdentifierAbs PrimaryId { get; }

		// Token: 0x17001A5D RID: 6749
		// (get) Token: 0x0600D48B RID: 54411
		// (set) Token: 0x0600D48C RID: 54412
		PlatformUserIdentifierAbs NativeId { get; set; }
	}
}
