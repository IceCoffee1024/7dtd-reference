using System;

namespace SharpEXR.AttributeTypes
{
	// Token: 0x02001714 RID: 5908
	public struct TimeCode
	{
		// Token: 0x0600B884 RID: 47236 RVA: 0x0044363F File Offset: 0x0044183F
		public TimeCode(uint timeAndFlags, uint userData)
		{
			this.TimeAndFlags = timeAndFlags;
			this.UserData = userData;
		}

		// Token: 0x040089A8 RID: 35240
		public readonly uint TimeAndFlags;

		// Token: 0x040089A9 RID: 35241
		public readonly uint UserData;
	}
}
