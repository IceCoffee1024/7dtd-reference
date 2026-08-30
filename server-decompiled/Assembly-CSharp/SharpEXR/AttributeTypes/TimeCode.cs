using System;

namespace SharpEXR.AttributeTypes
{
	// Token: 0x020016C2 RID: 5826
	public struct TimeCode
	{
		// Token: 0x0600B5AA RID: 46506 RVA: 0x004393A7 File Offset: 0x004375A7
		public TimeCode(uint timeAndFlags, uint userData)
		{
			this.TimeAndFlags = timeAndFlags;
			this.UserData = userData;
		}

		// Token: 0x040087C7 RID: 34759
		public readonly uint TimeAndFlags;

		// Token: 0x040087C8 RID: 34760
		public readonly uint UserData;
	}
}
