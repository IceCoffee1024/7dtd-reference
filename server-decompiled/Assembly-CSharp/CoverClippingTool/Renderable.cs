using System;
using UnityEngine;

namespace CoverClippingTool
{
	// Token: 0x02001671 RID: 5745
	public struct Renderable
	{
		// Token: 0x04008721 RID: 34593
		public bool DestroyMesh;

		// Token: 0x04008722 RID: 34594
		public Mesh RenderMesh;

		// Token: 0x04008723 RID: 34595
		public Matrix4x4 LocalToWorldMatrix;
	}
}
