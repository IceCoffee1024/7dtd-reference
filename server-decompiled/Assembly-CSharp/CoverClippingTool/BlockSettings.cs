using System;
using UnityEngine;

namespace CoverClippingTool
{
	// Token: 0x0200169B RID: 5787
	[Serializable]
	public class BlockSettings
	{
		// Token: 0x0600B4FD RID: 46333 RVA: 0x0043AACE File Offset: 0x00438CCE
		public BlockSettings()
		{
			this.ResetToDefault();
		}

		// Token: 0x0600B4FE RID: 46334 RVA: 0x0043AADC File Offset: 0x00438CDC
		public void ResetToDefault()
		{
			this.shapeModel = null;
			this.savedShapeInfo = null;
		}

		// Token: 0x040087AB RID: 34731
		public GameObject shapeModel;

		// Token: 0x040087AC RID: 34732
		public BlockShapeInfo savedShapeInfo;
	}
}
