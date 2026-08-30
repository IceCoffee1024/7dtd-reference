using System;

namespace UnityEngine.Experimental.Rendering
{
	// Token: 0x020004DC RID: 1244
	[Flags]
	public enum TextureCreationFlags
	{
		// Token: 0x04001037 RID: 4151
		None = 0,
		// Token: 0x04001038 RID: 4152
		MipChain = 1,
		// Token: 0x04001039 RID: 4153
		DontInitializePixels = 4,
		// Token: 0x0400103A RID: 4154
		Crunch = 64,
		// Token: 0x0400103B RID: 4155
		DontUploadUponCreate = 1024,
		// Token: 0x0400103C RID: 4156
		IgnoreMipmapLimit = 2048
	}
}
