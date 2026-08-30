using System;

namespace JetBrains.Annotations
{
	// Token: 0x020000D9 RID: 217
	[Flags]
	public enum CollectionAccessType
	{
		// Token: 0x0400026C RID: 620
		None = 0,
		// Token: 0x0400026D RID: 621
		Read = 1,
		// Token: 0x0400026E RID: 622
		ModifyExistingContent = 2,
		// Token: 0x0400026F RID: 623
		UpdatedContent = 6
	}
}
