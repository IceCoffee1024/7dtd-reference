using System;
using UnityEngine;

namespace CoverClippingTool
{
	// Token: 0x02001661 RID: 5729
	[Serializable]
	public class BlockSettings
	{
		// Token: 0x0600B36E RID: 45934 RVA: 0x00431AB2 File Offset: 0x0042FCB2
		public BlockSettings()
		{
			this.ResetToDefault();
		}

		// Token: 0x0600B36F RID: 45935 RVA: 0x00431AC0 File Offset: 0x0042FCC0
		public void ResetToDefault()
		{
			this.shapeModel = null;
			this.savedShapeInfo = null;
		}

		// Token: 0x0400867C RID: 34428
		public GameObject shapeModel;

		// Token: 0x0400867D RID: 34429
		public BlockShapeInfo savedShapeInfo;
	}
}
