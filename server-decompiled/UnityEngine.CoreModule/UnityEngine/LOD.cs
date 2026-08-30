using System;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x020001C3 RID: 451
	[UsedByNativeCode]
	public struct LOD
	{
		// Token: 0x06001041 RID: 4161 RVA: 0x00015F09 File Offset: 0x00014109
		public LOD(float screenRelativeTransitionHeight, Renderer[] renderers)
		{
			this.screenRelativeTransitionHeight = screenRelativeTransitionHeight;
			this.fadeTransitionWidth = 0f;
			this.renderers = renderers;
		}

		// Token: 0x0400063A RID: 1594
		public float screenRelativeTransitionHeight;

		// Token: 0x0400063B RID: 1595
		public float fadeTransitionWidth;

		// Token: 0x0400063C RID: 1596
		public Renderer[] renderers;
	}
}
