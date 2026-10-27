using System;
using System.Collections.Generic;

namespace GearVariants
{
	// Token: 0x02001745 RID: 5957
	[Serializable]
	public class GearFitEntry
	{
		// Token: 0x04008A8C RID: 35468
		public string wornKey = "";

		// Token: 0x04008A8D RID: 35469
		public string wornGuid = "";

		// Token: 0x04008A8E RID: 35470
		public List<BodyFit> fits = new List<BodyFit>();
	}
}
