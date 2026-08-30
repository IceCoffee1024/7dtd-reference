using System;
using UnityEngine;

namespace CoverClippingTool
{
	// Token: 0x0200167C RID: 5756
	[Serializable]
	public class BlockSettings
	{
		// Token: 0x0600B46B RID: 46187 RVA: 0x00436DF2 File Offset: 0x00434FF2
		public BlockSettings()
		{
			this.ResetToDefault();
		}

		// Token: 0x0600B46C RID: 46188 RVA: 0x00436E00 File Offset: 0x00435000
		public void ResetToDefault()
		{
			this.shapeModel = null;
			this.savedShapeInfo = null;
		}

		// Token: 0x04008755 RID: 34645
		public GameObject shapeModel;

		// Token: 0x04008756 RID: 34646
		public BlockShapeInfo savedShapeInfo;
	}
}
