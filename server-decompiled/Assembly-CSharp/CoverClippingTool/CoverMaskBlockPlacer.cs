using System;
using UnityEngine;

namespace CoverClippingTool
{
	// Token: 0x02001675 RID: 5749
	public class CoverMaskBlockPlacer : MonoBehaviour
	{
		// Token: 0x0600B457 RID: 46167 RVA: 0x0043613E File Offset: 0x0043433E
		public void UpdateSelectedMultiBlock(string value)
		{
			this.editMultiBlock = !value.Equals("All", StringComparison.OrdinalIgnoreCase);
			this.selectedMultiBlock = (this.editMultiBlock ? StringParsers.ParseVector3i(value, 0, -1, false) : Vector3i.zero);
		}

		// Token: 0x0400872C RID: 34604
		public static readonly string PropCoverMask = "CoverMask";

		// Token: 0x0400872D RID: 34605
		public static readonly string PropCoverMaskMulti = "CoverMaskMulti";

		// Token: 0x0400872E RID: 34606
		public static readonly string PropCoverOffset = "CoverOffset";

		// Token: 0x0400872F RID: 34607
		[SerializeField]
		public SelectedBlockInfo selectedModel = new SelectedBlockInfo(0);

		// Token: 0x04008730 RID: 34608
		[SerializeField]
		public Vector3i selectedMultiBlock;

		// Token: 0x04008731 RID: 34609
		[SerializeField]
		public bool editMultiBlock;

		// Token: 0x04008732 RID: 34610
		[SerializeField]
		public string searchFilter;
	}
}
