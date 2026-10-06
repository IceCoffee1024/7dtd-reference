using System;
using System.Diagnostics;

// Token: 0x020014A3 RID: 5283
public static class PreserveCheckPatch
{
	// Token: 0x0600A5A7 RID: 42407 RVA: 0x000027FC File Offset: 0x000009FC
	[Conditional("ENABLE_MONO")]
	public static void Enable()
	{
	}

	// Token: 0x0600A5A8 RID: 42408 RVA: 0x000027FC File Offset: 0x000009FC
	[Conditional("ENABLE_MONO")]
	public static void Disable()
	{
	}
}
