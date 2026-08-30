using System;

namespace Platform
{
	// Token: 0x02001BC0 RID: 7104
	public interface IPlatformUser
	{
		// Token: 0x170019EA RID: 6634
		// (get) Token: 0x0600D2FC RID: 54012
		PlatformUserIdentifierAbs PrimaryId { get; }

		// Token: 0x170019EB RID: 6635
		// (get) Token: 0x0600D2FD RID: 54013
		// (set) Token: 0x0600D2FE RID: 54014
		PlatformUserIdentifierAbs NativeId { get; set; }
	}
}
