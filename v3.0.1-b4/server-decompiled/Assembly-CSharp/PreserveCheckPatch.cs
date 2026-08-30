using System;
using System.Diagnostics;

// Token: 0x02001468 RID: 5224
public static class PreserveCheckPatch
{
	// Token: 0x0600A3FC RID: 41980 RVA: 0x000027FC File Offset: 0x000009FC
	[Conditional("ENABLE_MONO")]
	public static void Enable()
	{
	}

	// Token: 0x0600A3FD RID: 41981 RVA: 0x000027FC File Offset: 0x000009FC
	[Conditional("ENABLE_MONO")]
	public static void Disable()
	{
	}
}
