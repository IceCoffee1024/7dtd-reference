using System;
using UnityEngine;

namespace CoverClippingTool
{
	// Token: 0x02001694 RID: 5780
	public class CoverMaskBlockPlacer : MonoBehaviour
	{
		// Token: 0x0600B4E9 RID: 46313 RVA: 0x00439E1A File Offset: 0x0043801A
		public void UpdateSelectedMultiBlock(string value)
		{
			this.editMultiBlock = !value.Equals("All", StringComparison.OrdinalIgnoreCase);
			this.selectedMultiBlock = (this.editMultiBlock ? StringParsers.ParseVector3i(value, 0, -1, false) : Vector3i.zero);
		}

		// Token: 0x04008782 RID: 34690
		public static readonly string PropCoverMask = "CoverMask";

		// Token: 0x04008783 RID: 34691
		public static readonly string PropCoverMaskMulti = "CoverMaskMulti";

		// Token: 0x04008784 RID: 34692
		public static readonly string PropCoverOffset = "CoverOffset";

		// Token: 0x04008785 RID: 34693
		[SerializeField]
		public SelectedBlockInfo selectedModel = new SelectedBlockInfo(0);

		// Token: 0x04008786 RID: 34694
		[SerializeField]
		public Vector3i selectedMultiBlock;

		// Token: 0x04008787 RID: 34695
		[SerializeField]
		public bool editMultiBlock;

		// Token: 0x04008788 RID: 34696
		[SerializeField]
		public string searchFilter;
	}
}
