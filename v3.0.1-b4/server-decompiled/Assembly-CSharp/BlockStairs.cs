using System;
using UnityEngine.Scripting;

// Token: 0x02000153 RID: 339
[Preserve]
public class BlockStairs : Block
{
	// Token: 0x06000961 RID: 2401 RVA: 0x000414EA File Offset: 0x0003F6EA
	public override bool IsMovementBlocked(IBlockAccess _world, Vector3i _blockPos, BlockValue _blockValue, BlockFace _face)
	{
		return !_blockValue.ischild;
	}

	// Token: 0x06000962 RID: 2402 RVA: 0x000414EA File Offset: 0x0003F6EA
	public override bool IsMovementBlocked(IBlockAccess _world, Vector3i _blockPos, BlockValue _blockValue, BlockFaceFlag _sides)
	{
		return !_blockValue.ischild;
	}
}
