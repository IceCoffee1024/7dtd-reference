using System;

// Token: 0x02000171 RID: 369
public interface IFeaturePhysicalCapabilities
{
	// Token: 0x06000A8F RID: 2703
	bool IsMovementBlocked(Vector3i _blockPos, BlockValue _blockValue, BlockFace _face);

	// Token: 0x06000A90 RID: 2704
	bool IsMovementBlocked(Vector3i _blockPos, BlockValue _blockValue, BlockFaceFlag _sides);

	// Token: 0x06000A91 RID: 2705
	bool IsSeeThrough(Vector3i _blockPos, BlockValue _blockValue);

	// Token: 0x06000A92 RID: 2706
	float GetStepHeight(Vector3i _blockPos, BlockValue _blockValue, BlockFace crossingFace);
}
