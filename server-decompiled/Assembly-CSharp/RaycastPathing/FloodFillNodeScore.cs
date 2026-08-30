using System;
using UnityEngine.Scripting;

namespace RaycastPathing
{
	// Token: 0x020018CF RID: 6351
	[Preserve]
	public class FloodFillNodeScore
	{
		// Token: 0x1700183C RID: 6204
		// (get) Token: 0x0600C3FB RID: 50171 RVA: 0x00484A18 File Offset: 0x00482C18
		public float F
		{
			get
			{
				return this.G + this.H;
			}
		}

		// Token: 0x0400943C RID: 37948
		public float G;

		// Token: 0x0400943D RID: 37949
		public float H;
	}
}
