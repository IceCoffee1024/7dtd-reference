using System;

// Token: 0x0200151B RID: 5403
public static class ChunkConditions
{
	// Token: 0x04007E2A RID: 32298
	public static readonly ChunkConditions.Delegate Decorated = (Chunk chunk) => !chunk.NeedsDecoration && !chunk.NeedsLightCalculation;

	// Token: 0x04007E2B RID: 32299
	public static readonly ChunkConditions.Delegate MeshesCopied = (Chunk chunk) => !chunk.InProgressDecorating && !chunk.InProgressLighting && !chunk.InProgressRegeneration && !chunk.InProgressCopying && !chunk.NeedsDecoration && !chunk.NeedsLightCalculation && !chunk.NeedsRegeneration && !chunk.NeedsCopying;

	// Token: 0x04007E2C RID: 32300
	public static readonly ChunkConditions.Delegate Displayed = (Chunk chunk) => ChunkConditions.MeshesCopied(chunk) && chunk.displayState == Chunk.DisplayState.Done;

	// Token: 0x0200151C RID: 5404
	// (Invoke) Token: 0x0600A9E6 RID: 43494
	public delegate bool Delegate(Chunk chunk);
}
