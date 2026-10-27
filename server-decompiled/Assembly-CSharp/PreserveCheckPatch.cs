using System;
using System.Diagnostics;

// Token: 0x020014B5 RID: 5301
public static class PreserveCheckPatch
{
	// Token: 0x0600A5FF RID: 42495 RVA: 0x000027FC File Offset: 0x000009FC
	[Conditional("ENABLE_MONO")]
	public static void Enable()
	{
	}

	// Token: 0x0600A600 RID: 42496 RVA: 0x000027FC File Offset: 0x000009FC
	[Conditional("ENABLE_MONO")]
	public static void Disable()
	{
	}
}
