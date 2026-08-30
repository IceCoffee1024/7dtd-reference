using System;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x020001EB RID: 491
	[UsedByNativeCode]
	public struct GradientColorKey
	{
		// Token: 0x06001526 RID: 5414 RVA: 0x0001FB29 File Offset: 0x0001DD29
		public GradientColorKey(Color col, float time)
		{
			this.color = col;
			this.time = time;
		}

		// Token: 0x040007E4 RID: 2020
		public Color color;

		// Token: 0x040007E5 RID: 2021
		public float time;
	}
}
