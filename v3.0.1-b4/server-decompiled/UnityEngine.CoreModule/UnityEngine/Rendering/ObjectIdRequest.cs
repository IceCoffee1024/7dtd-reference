using System;

namespace UnityEngine.Rendering
{
	// Token: 0x0200045E RID: 1118
	public class ObjectIdRequest
	{
		// Token: 0x170006D3 RID: 1747
		// (get) Token: 0x06002589 RID: 9609 RVA: 0x000400A9 File Offset: 0x0003E2A9
		// (set) Token: 0x0600258A RID: 9610 RVA: 0x000400B1 File Offset: 0x0003E2B1
		public RenderTexture destination { get; set; }

		// Token: 0x170006D4 RID: 1748
		// (get) Token: 0x0600258B RID: 9611 RVA: 0x000400BA File Offset: 0x0003E2BA
		// (set) Token: 0x0600258C RID: 9612 RVA: 0x000400C2 File Offset: 0x0003E2C2
		public int mipLevel { get; set; }

		// Token: 0x170006D5 RID: 1749
		// (get) Token: 0x0600258D RID: 9613 RVA: 0x000400CB File Offset: 0x0003E2CB
		// (set) Token: 0x0600258E RID: 9614 RVA: 0x000400D3 File Offset: 0x0003E2D3
		public CubemapFace face { get; set; }

		// Token: 0x170006D6 RID: 1750
		// (get) Token: 0x0600258F RID: 9615 RVA: 0x000400DC File Offset: 0x0003E2DC
		// (set) Token: 0x06002590 RID: 9616 RVA: 0x000400E4 File Offset: 0x0003E2E4
		public int slice { get; set; }

		// Token: 0x170006D7 RID: 1751
		// (get) Token: 0x06002591 RID: 9617 RVA: 0x000400ED File Offset: 0x0003E2ED
		// (set) Token: 0x06002592 RID: 9618 RVA: 0x000400F5 File Offset: 0x0003E2F5
		public ObjectIdResult result { get; internal set; }

		// Token: 0x06002593 RID: 9619 RVA: 0x000400FE File Offset: 0x0003E2FE
		public ObjectIdRequest(RenderTexture destination, int mipLevel = 0, CubemapFace face = CubemapFace.Unknown, int slice = 0)
		{
			this.destination = destination;
			this.mipLevel = mipLevel;
			this.face = face;
			this.slice = slice;
		}
	}
}
