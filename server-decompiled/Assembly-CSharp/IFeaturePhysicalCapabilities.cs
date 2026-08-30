using System;

// Token: 0x0200016F RID: 367
public interface IFeaturePhysicalCapabilities
{
	// Token: 0x06000A89 RID: 2697
	bool IsMovementBlocked(Vector3i _blockPos, BlockValue _blockValue, BlockFace _face);

	// Token: 0x06000A8A RID: 2698
	bool IsMovementBlocked(Vector3i _blockPos, BlockValue _blockValue, BlockFaceFlag _sides);

	// Token: 0x06000A8B RID: 2699
	bool IsSeeThrough(Vector3i _blockPos, BlockValue _blockValue);

	// Token: 0x06000A8C RID: 2700
	float GetStepHeight(Vector3i _blockPos, BlockValue _blockValue, BlockFace crossingFace);
}
