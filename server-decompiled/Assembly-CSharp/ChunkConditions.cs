using System;

// Token: 0x02001500 RID: 5376
public static class ChunkConditions
{
	// Token: 0x04007D51 RID: 32081
	public static readonly ChunkConditions.Delegate Decorated = (Chunk chunk) => !chunk.NeedsDecoration && !chunk.NeedsLightCalculation;

	// Token: 0x04007D52 RID: 32082
	public static readonly ChunkConditions.Delegate MeshesCopied = (Chunk chunk) => !chunk.InProgressDecorating && !chunk.InProgressLighting && !chunk.InProgressRegeneration && !chunk.InProgressCopying && !chunk.NeedsDecoration && !chunk.NeedsLightCalculation && !chunk.NeedsRegeneration && !chunk.NeedsCopying;

	// Token: 0x04007D53 RID: 32083
	public static readonly ChunkConditions.Delegate Displayed = (Chunk chunk) => ChunkConditions.MeshesCopied(chunk) && chunk.displayState == Chunk.DisplayState.Done;

	// Token: 0x02001501 RID: 5377
	// (Invoke) Token: 0x0600A8E9 RID: 43241
	public delegate bool Delegate(Chunk chunk);
}
