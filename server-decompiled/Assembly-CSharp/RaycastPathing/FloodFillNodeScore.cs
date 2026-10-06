using System;
using UnityEngine.Scripting;

namespace RaycastPathing
{
	// Token: 0x020018FB RID: 6395
	[Preserve]
	public class FloodFillNodeScore
	{
		// Token: 0x170018DE RID: 6366
		// (get) Token: 0x0600C553 RID: 50515 RVA: 0x00488F60 File Offset: 0x00487160
		public float F
		{
			get
			{
				return this.G + this.H;
			}
		}

		// Token: 0x04009502 RID: 38146
		public float G;

		// Token: 0x04009503 RID: 38147
		public float H;
	}
}
