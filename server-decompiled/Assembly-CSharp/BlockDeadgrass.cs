using System;
using UnityEngine.Scripting;

// Token: 0x0200011E RID: 286
[Preserve]
public class BlockDeadgrass : Block
{
	// Token: 0x06000785 RID: 1925 RVA: 0x00035773 File Offset: 0x00033973
	public override void Init()
	{
		base.Init();
		this.IsDecoration = true;
	}

	// Token: 0x06000786 RID: 1926 RVA: 0x00035784 File Offset: 0x00033984
	public override void OnNeighborBlockChange(WorldBase world, Vector3i _myBlockPos, BlockValue _myBlockValue, Vector3i _blockPosThatChanged, BlockValue _newNeighborBlockValue, BlockValue _oldNeighborBlockValue)
	{
		if (_blockPosThatChanged.x == _myBlockPos.x && _blockPosThatChanged.z == _myBlockPos.z && _blockPosThatChanged.y == _myBlockPos.y - 1 && !_newNeighborBlockValue.Block.shape.IsSolidCube)
		{
			world.SetBlockRPC(_myBlockPos, BlockValue.Air);
		}
	}

	// Token: 0x06000787 RID: 1927 RVA: 0x000357E4 File Offset: 0x000339E4
	public override BlockValue OnBlockPlaced(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, GameRandom _rnd)
	{
		_blockValue.meta = (byte)_rnd.RandomRange(16);
		return _blockValue;
	}

	// Token: 0x06000788 RID: 1928 RVA: 0x000357F8 File Offset: 0x000339F8
	public override void OnBlockPlaceBefore(WorldBase _world, ref BlockPlacement.Result _bpResult, EntityAlive _ea, GameRandom _rnd)
	{
		_bpResult.blockValue.meta = (byte)_rnd.RandomRange(16);
	}
}
