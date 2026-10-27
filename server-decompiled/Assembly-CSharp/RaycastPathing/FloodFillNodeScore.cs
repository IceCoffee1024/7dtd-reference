using System;
using UnityEngine.Scripting;

namespace RaycastPathing
{
	// Token: 0x0200192C RID: 6444
	[Preserve]
	public class FloodFillNodeScore
	{
		// Token: 0x17001926 RID: 6438
		// (get) Token: 0x0600C667 RID: 50791 RVA: 0x0048C90C File Offset: 0x0048AB0C
		public float F
		{
			get
			{
				return this.G + this.H;
			}
		}

		// Token: 0x040095EB RID: 38379
		public float G;

		// Token: 0x040095EC RID: 38380
		public float H;
	}
}
