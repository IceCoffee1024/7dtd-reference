using System;
using UnityEngine.Scripting;

// Token: 0x02000156 RID: 342
[Preserve]
public class BlockStairs : Block
{
	// Token: 0x06000971 RID: 2417 RVA: 0x0004182A File Offset: 0x0003FA2A
	public override bool IsMovementBlocked(IBlockAccess _world, Vector3i _blockPos, BlockValue _blockValue, BlockFace _face)
	{
		return !_blockValue.ischild;
	}

	// Token: 0x06000972 RID: 2418 RVA: 0x0004182A File Offset: 0x0003FA2A
	public override bool IsMovementBlocked(IBlockAccess _world, Vector3i _blockPos, BlockValue _blockValue, BlockFaceFlag _sides)
	{
		return !_blockValue.ischild;
	}
}
