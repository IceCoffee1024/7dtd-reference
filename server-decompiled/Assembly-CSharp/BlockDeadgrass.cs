using System;
using UnityEngine.Scripting;

// Token: 0x0200011B RID: 283
[Preserve]
public class BlockDeadgrass : Block
{
	// Token: 0x06000775 RID: 1909 RVA: 0x0003538F File Offset: 0x0003358F
	public override void Init()
	{
		base.Init();
		this.IsDecoration = true;
	}

	// Token: 0x06000776 RID: 1910 RVA: 0x000353A0 File Offset: 0x000335A0
	public override void OnNeighborBlockChange(WorldBase world, Vector3i _myBlockPos, BlockValue _myBlockValue, Vector3i _blockPosThatChanged, BlockValue _newNeighborBlockValue, BlockValue _oldNeighborBlockValue)
	{
		if (_blockPosThatChanged.x == _myBlockPos.x && _blockPosThatChanged.z == _myBlockPos.z && _blockPosThatChanged.y == _myBlockPos.y - 1 && !_newNeighborBlockValue.Block.shape.IsSolidCube)
		{
			world.SetBlockRPC(_myBlockPos, BlockValue.Air);
		}
	}

	// Token: 0x06000777 RID: 1911 RVA: 0x00035400 File Offset: 0x00033600
	public override BlockValue OnBlockPlaced(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, GameRandom _rnd)
	{
		_blockValue.meta = (byte)_rnd.RandomRange(16);
		return _blockValue;
	}

	// Token: 0x06000778 RID: 1912 RVA: 0x00035414 File Offset: 0x00033614
	public override void OnBlockPlaceBefore(WorldBase _world, ref BlockPlacement.Result _bpResult, EntityAlive _ea, GameRandom _rnd)
	{
		_bpResult.blockValue.meta = (byte)_rnd.RandomRange(16);
	}
}
