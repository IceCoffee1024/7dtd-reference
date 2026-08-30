using System;
using UnityEngine;

namespace CoverClippingTool
{
	// Token: 0x0200165A RID: 5722
	public class CoverMaskBlockPlacer : MonoBehaviour
	{
		// Token: 0x0600B35A RID: 45914 RVA: 0x00430DFE File Offset: 0x0042EFFE
		public void UpdateSelectedMultiBlock(string value)
		{
			this.editMultiBlock = !value.Equals("All", StringComparison.OrdinalIgnoreCase);
			this.selectedMultiBlock = (this.editMultiBlock ? StringParsers.ParseVector3i(value, 0, -1, false) : Vector3i.zero);
		}

		// Token: 0x04008653 RID: 34387
		public static readonly string PropCoverMask = "CoverMask";

		// Token: 0x04008654 RID: 34388
		public static readonly string PropCoverMaskMulti = "CoverMaskMulti";

		// Token: 0x04008655 RID: 34389
		public static readonly string PropCoverOffset = "CoverOffset";

		// Token: 0x04008656 RID: 34390
		[SerializeField]
		public SelectedBlockInfo selectedModel = new SelectedBlockInfo(0);

		// Token: 0x04008657 RID: 34391
		[SerializeField]
		public Vector3i selectedMultiBlock;

		// Token: 0x04008658 RID: 34392
		[SerializeField]
		public bool editMultiBlock;

		// Token: 0x04008659 RID: 34393
		[SerializeField]
		public string searchFilter;
	}
}
