using System;
using UnityEngine.Scripting;

namespace RaycastPathing
{
	// Token: 0x020018A9 RID: 6313
	[Preserve]
	public class FloodFillNodeScore
	{
		// Token: 0x170017CA RID: 6090
		// (get) Token: 0x0600C274 RID: 49780 RVA: 0x0047E8EC File Offset: 0x0047CAEC
		public float F
		{
			get
			{
				return this.G + this.H;
			}
		}

		// Token: 0x04009313 RID: 37651
		public float G;

		// Token: 0x04009314 RID: 37652
		public float H;
	}
}
