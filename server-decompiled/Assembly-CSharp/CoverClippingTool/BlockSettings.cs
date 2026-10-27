using System;
using UnityEngine;

namespace CoverClippingTool
{
	// Token: 0x020016AD RID: 5805
	[Serializable]
	public class BlockSettings
	{
		// Token: 0x0600B555 RID: 46421 RVA: 0x0043CED2 File Offset: 0x0043B0D2
		public BlockSettings()
		{
			this.ResetToDefault();
		}

		// Token: 0x0600B556 RID: 46422 RVA: 0x0043CEE0 File Offset: 0x0043B0E0
		public void ResetToDefault()
		{
			this.shapeModel = null;
			this.savedShapeInfo = null;
		}

		// Token: 0x040087EC RID: 34796
		public GameObject shapeModel;

		// Token: 0x040087ED RID: 34797
		public BlockShapeInfo savedShapeInfo;
	}
}
