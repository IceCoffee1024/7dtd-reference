using System;
using System.Diagnostics;

// Token: 0x02001483 RID: 5251
public static class PreserveCheckPatch
{
	// Token: 0x0600A4F9 RID: 42233 RVA: 0x000027FC File Offset: 0x000009FC
	[Conditional("ENABLE_MONO")]
	public static void Enable()
	{
	}

	// Token: 0x0600A4FA RID: 42234 RVA: 0x000027FC File Offset: 0x000009FC
	[Conditional("ENABLE_MONO")]
	public static void Disable()
	{
	}
}
