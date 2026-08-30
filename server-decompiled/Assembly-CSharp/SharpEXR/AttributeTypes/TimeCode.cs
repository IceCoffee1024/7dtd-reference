using System;

namespace SharpEXR.AttributeTypes
{
	// Token: 0x020016E8 RID: 5864
	public struct TimeCode
	{
		// Token: 0x0600B72D RID: 46893 RVA: 0x0043F11F File Offset: 0x0043D31F
		public TimeCode(uint timeAndFlags, uint userData)
		{
			this.TimeAndFlags = timeAndFlags;
			this.UserData = userData;
		}

		// Token: 0x040088E3 RID: 35043
		public readonly uint TimeAndFlags;

		// Token: 0x040088E4 RID: 35044
		public readonly uint UserData;
	}
}
