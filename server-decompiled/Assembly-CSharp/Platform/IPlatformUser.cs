using System;

namespace Platform
{
	// Token: 0x02001C4A RID: 7242
	public interface IPlatformUser
	{
		// Token: 0x17001B4A RID: 6986
		// (get) Token: 0x0600D716 RID: 55062
		PlatformUserIdentifierAbs PrimaryId { get; }

		// Token: 0x17001B4B RID: 6987
		// (get) Token: 0x0600D717 RID: 55063
		// (set) Token: 0x0600D718 RID: 55064
		PlatformUserIdentifierAbs NativeId { get; set; }
	}
}
