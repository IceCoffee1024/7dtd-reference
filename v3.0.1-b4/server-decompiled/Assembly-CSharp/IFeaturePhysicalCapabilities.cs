using System;

// Token: 0x0200016E RID: 366
public interface IFeaturePhysicalCapabilities
{
	// Token: 0x06000A7F RID: 2687
	bool IsMovementBlocked(Vector3i _blockPos, BlockValue _blockValue, BlockFace _face);

	// Token: 0x06000A80 RID: 2688
	bool IsMovementBlocked(Vector3i _blockPos, BlockValue _blockValue, BlockFaceFlag _sides);

	// Token: 0x06000A81 RID: 2689
	bool IsSeeThrough(Vector3i _blockPos, BlockValue _blockValue);

	// Token: 0x06000A82 RID: 2690
	float GetStepHeight(Vector3i _blockPos, BlockValue _blockValue, BlockFace crossingFace);
}
