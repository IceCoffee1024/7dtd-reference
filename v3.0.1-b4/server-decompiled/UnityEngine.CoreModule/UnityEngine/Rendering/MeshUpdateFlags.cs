using System;

namespace UnityEngine.Rendering
{
	// Token: 0x020003EB RID: 1003
	[Flags]
	public enum MeshUpdateFlags
	{
		// Token: 0x04000B25 RID: 2853
		Default = 0,
		// Token: 0x04000B26 RID: 2854
		DontValidateIndices = 1,
		// Token: 0x04000B27 RID: 2855
		DontResetBoneBounds = 2,
		// Token: 0x04000B28 RID: 2856
		DontNotifyMeshUsers = 4,
		// Token: 0x04000B29 RID: 2857
		DontRecalculateBounds = 8
	}
}
