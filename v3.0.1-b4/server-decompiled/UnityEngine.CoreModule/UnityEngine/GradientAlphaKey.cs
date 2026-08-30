using System;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x020001EC RID: 492
	[UsedByNativeCode]
	public struct GradientAlphaKey
	{
		// Token: 0x06001527 RID: 5415 RVA: 0x0001FB3A File Offset: 0x0001DD3A
		public GradientAlphaKey(float alpha, float time)
		{
			this.alpha = alpha;
			this.time = time;
		}

		// Token: 0x040007E6 RID: 2022
		public float alpha;

		// Token: 0x040007E7 RID: 2023
		public float time;
	}
}
