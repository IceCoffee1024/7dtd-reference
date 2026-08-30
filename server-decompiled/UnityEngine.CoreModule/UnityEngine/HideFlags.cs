using System;

namespace UnityEngine
{
	// Token: 0x02000267 RID: 615
	[Flags]
	public enum HideFlags
	{
		// Token: 0x040008F5 RID: 2293
		None = 0,
		// Token: 0x040008F6 RID: 2294
		HideInHierarchy = 1,
		// Token: 0x040008F7 RID: 2295
		HideInInspector = 2,
		// Token: 0x040008F8 RID: 2296
		DontSaveInEditor = 4,
		// Token: 0x040008F9 RID: 2297
		NotEditable = 8,
		// Token: 0x040008FA RID: 2298
		DontSaveInBuild = 16,
		// Token: 0x040008FB RID: 2299
		DontUnloadUnusedAsset = 32,
		// Token: 0x040008FC RID: 2300
		DontSave = 52,
		// Token: 0x040008FD RID: 2301
		HideAndDontSave = 61
	}
}
