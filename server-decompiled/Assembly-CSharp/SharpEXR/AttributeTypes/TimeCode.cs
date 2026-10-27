using System;

namespace SharpEXR.AttributeTypes
{
	// Token: 0x02001733 RID: 5939
	public struct TimeCode
	{
		// Token: 0x0600B958 RID: 47448 RVA: 0x004460E3 File Offset: 0x004442E3
		public TimeCode(uint timeAndFlags, uint userData)
		{
			this.TimeAndFlags = timeAndFlags;
			this.UserData = userData;
		}

		// Token: 0x04008A5B RID: 35419
		public readonly uint TimeAndFlags;

		// Token: 0x04008A5C RID: 35420
		public readonly uint UserData;
	}
}
