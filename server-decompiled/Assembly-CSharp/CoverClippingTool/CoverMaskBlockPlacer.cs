using System;
using UnityEngine;

namespace CoverClippingTool
{
	// Token: 0x020016A6 RID: 5798
	public class CoverMaskBlockPlacer : MonoBehaviour
	{
		// Token: 0x0600B541 RID: 46401 RVA: 0x0043C21E File Offset: 0x0043A41E
		public void UpdateSelectedMultiBlock(string value)
		{
			this.editMultiBlock = !value.Equals("All", StringComparison.OrdinalIgnoreCase);
			this.selectedMultiBlock = (this.editMultiBlock ? StringParsers.ParseVector3i(value, 0, -1, false) : Vector3i.zero);
		}

		// Token: 0x040087C3 RID: 34755
		public static readonly string PropCoverMask = "CoverMask";

		// Token: 0x040087C4 RID: 34756
		public static readonly string PropCoverMaskMulti = "CoverMaskMulti";

		// Token: 0x040087C5 RID: 34757
		public static readonly string PropCoverOffset = "CoverOffset";

		// Token: 0x040087C6 RID: 34758
		[SerializeField]
		public SelectedBlockInfo selectedModel = new SelectedBlockInfo(0);

		// Token: 0x040087C7 RID: 34759
		[SerializeField]
		public Vector3i selectedMultiBlock;

		// Token: 0x040087C8 RID: 34760
		[SerializeField]
		public bool editMultiBlock;

		// Token: 0x040087C9 RID: 34761
		[SerializeField]
		public string searchFilter;
	}
}
