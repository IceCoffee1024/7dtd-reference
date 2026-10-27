using System;

// Token: 0x0200154F RID: 5455
public static class ChunkConditions
{
	// Token: 0x04007EEE RID: 32494
	public static readonly ChunkConditions.Delegate Decorated = (Chunk chunk) => !chunk.NeedsDecoration && !chunk.NeedsLightCalculation;

	// Token: 0x04007EEF RID: 32495
	public static readonly ChunkConditions.Delegate MeshesCopied = (Chunk chunk) => !chunk.InProgressDecorating && !chunk.InProgressLighting && !chunk.InProgressRegeneration && !chunk.InProgressCopying && !chunk.NeedsDecoration && !chunk.NeedsLightCalculation && !chunk.NeedsRegeneration && !chunk.NeedsCopying;

	// Token: 0x04007EF0 RID: 32496
	public static readonly ChunkConditions.Delegate Displayed = (Chunk chunk) => ChunkConditions.MeshesCopied(chunk) && chunk.displayState == Chunk.DisplayState.Done;

	// Token: 0x02001550 RID: 5456
	// (Invoke) Token: 0x0600AAF1 RID: 43761
	public delegate bool Delegate(Chunk chunk);
}
