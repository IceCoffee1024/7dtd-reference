using System;

namespace UnityEngine.Rendering
{
	// Token: 0x020003E5 RID: 997
	[Obsolete("GPUFence has been deprecated. Use GraphicsFence instead (UnityUpgradable) -> GraphicsFence", false)]
	public struct GPUFence
	{
		// Token: 0x17000657 RID: 1623
		// (get) Token: 0x060021A6 RID: 8614 RVA: 0x00037FB0 File Offset: 0x000361B0
		public bool passed
		{
			get
			{
				return true;
			}
		}
	}
}
