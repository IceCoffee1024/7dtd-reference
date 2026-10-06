using System;

// Token: 0x0200153D RID: 5437
public static class ChunkConditions
{
	// Token: 0x04007EAD RID: 32429
	public static readonly ChunkConditions.Delegate Decorated = (Chunk chunk) => !chunk.NeedsDecoration && !chunk.NeedsLightCalculation;

	// Token: 0x04007EAE RID: 32430
	public static readonly ChunkConditions.Delegate MeshesCopied = (Chunk chunk) => !chunk.InProgressDecorating && !chunk.InProgressLighting && !chunk.InProgressRegeneration && !chunk.InProgressCopying && !chunk.NeedsDecoration && !chunk.NeedsLightCalculation && !chunk.NeedsRegeneration && !chunk.NeedsCopying;

	// Token: 0x04007EAF RID: 32431
	public static readonly ChunkConditions.Delegate Displayed = (Chunk chunk) => ChunkConditions.MeshesCopied(chunk) && chunk.displayState == Chunk.DisplayState.Done;

	// Token: 0x0200153E RID: 5438
	// (Invoke) Token: 0x0600AA99 RID: 43673
	public delegate bool Delegate(Chunk chunk);
}
