using System;
using UnityEngine.Scripting;

// Token: 0x0200012A RID: 298
[Preserve]
public class BlockLiquidSource : Block
{
	// Token: 0x060007EF RID: 2031 RVA: 0x00038568 File Offset: 0x00036768
	public BlockLiquidSource()
	{
		Vector3i[,] array = new Vector3i[8, 4];
		array[0, 0] = new Vector3i(-1, 0, 0);
		array[0, 1] = new Vector3i(0, 0, -1);
		array[0, 2] = new Vector3i(1, 0, 0);
		array[0, 3] = new Vector3i(0, 0, 1);
		array[1, 0] = new Vector3i(1, 0, 0);
		array[1, 1] = new Vector3i(0, 0, -1);
		array[1, 2] = new Vector3i(0, 0, 1);
		array[1, 3] = new Vector3i(-1, 0, 0);
		array[2, 0] = new Vector3i(0, 0, 1);
		array[2, 1] = new Vector3i(-1, 0, 0);
		array[2, 2] = new Vector3i(0, 0, -1);
		array[2, 3] = new Vector3i(1, 0, 0);
		array[3, 0] = new Vector3i(0, 0, -1);
		array[3, 1] = new Vector3i(0, 0, 1);
		array[3, 2] = new Vector3i(1, 0, 0);
		array[3, 3] = new Vector3i(-1, 0, 0);
		array[4, 0] = new Vector3i(-1, 0, 0);
		array[4, 1] = new Vector3i(1, 0, 0);
		array[4, 2] = new Vector3i(0, 0, -1);
		array[4, 3] = new Vector3i(0, 0, 1);
		array[5, 0] = new Vector3i(1, 0, 0);
		array[5, 1] = new Vector3i(-1, 0, 0);
		array[5, 2] = new Vector3i(0, 0, 1);
		array[5, 3] = new Vector3i(0, 0, -1);
		array[6, 0] = new Vector3i(0, 0, 1);
		array[6, 1] = new Vector3i(-1, 0, 0);
		array[6, 2] = new Vector3i(1, 0, 0);
		array[6, 3] = new Vector3i(0, 0, -1);
		array[7, 0] = new Vector3i(1, 0, 0);
		array[7, 1] = new Vector3i(-1, 0, 0);
		array[7, 2] = new Vector3i(0, 0, 1);
		array[7, 3] = new Vector3i(0, 0, -1);
		this.fallDirsSet = array;
		base..ctor();
		this.IsRandomlyTick = false;
	}

	// Token: 0x060007F0 RID: 2032 RVA: 0x00038790 File Offset: 0x00036990
	public override void LateInit()
	{
		base.LateInit();
		ItemValue item = ItemClass.GetItem("water", false);
		if (item != null)
		{
			this.waterBlock = item.ToBlockValue(false);
			return;
		}
		this.waterBlock = BlockValue.Air;
	}

	// Token: 0x060007F1 RID: 2033 RVA: 0x000387CC File Offset: 0x000369CC
	public override void OnNeighborBlockChange(WorldBase world, Vector3i _myBlockPos, BlockValue _myBlockValue, Vector3i _blockPosThatChanged, BlockValue _newNeighborBlockValue, BlockValue _oldNeighborBlockValue)
	{
		base.OnNeighborBlockChange(world, _myBlockPos, _myBlockValue, _blockPosThatChanged, _newNeighborBlockValue, _oldNeighborBlockValue);
		if (_newNeighborBlockValue.isair)
		{
			_myBlockValue.meta = 1 - _myBlockValue.meta;
			world.SetBlockRPC(_myBlockPos, _myBlockValue);
			world.GetWBT().AddScheduledBlockUpdate(_myBlockPos, this.blockID, 1UL);
		}
	}

	// Token: 0x060007F2 RID: 2034 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool IsMovementBlocked(IBlockAccess world, Vector3i _blockPos, BlockValue _blockValue, BlockFace crossingFace)
	{
		return false;
	}

	// Token: 0x060007F3 RID: 2035 RVA: 0x00038823 File Offset: 0x00036A23
	public override ulong GetTickRate()
	{
		return 20UL;
	}

	// Token: 0x060007F4 RID: 2036 RVA: 0x00038828 File Offset: 0x00036A28
	public override bool UpdateTick(WorldBase world, Vector3i _blockPos, BlockValue _blockValue, bool _bRandomTick, ulong _ticksIfLoaded, GameRandom _rnd)
	{
		BlockValue blockValue;
		for (int i = -1; i <= 1; i++)
		{
			for (int j = -1; j <= 1; j++)
			{
				if (i != 0 || j != 0)
				{
					this.emitionPos = _blockPos;
					this.emitionPos.x = this.emitionPos.x + j;
					this.emitionPos.z = this.emitionPos.z + i;
					BlockValue block = world.GetBlock(this.emitionPos.x, this.emitionPos.y, this.emitionPos.z);
					if (block.isair || block.Block.blockMaterial.IsPlant)
					{
						blockValue = new BlockValue((uint)this.waterBlock.type);
						blockValue.meta = 14;
						blockValue.meta2 = 8;
						world.SetBlockRPC(this.emitionPos, blockValue);
						world.GetWBT().AddScheduledBlockUpdate(this.emitionPos, BlockValue.Air.type, 1UL);
					}
				}
			}
		}
		blockValue = new BlockValue((uint)this.waterBlock.type);
		blockValue.meta = 14;
		blockValue.meta2 = 0;
		world.SetBlockRPC(_blockPos, blockValue);
		world.GetWBT().AddScheduledBlockUpdate(_blockPos, this.blockID, 1UL);
		return true;
	}

	// Token: 0x060007F5 RID: 2037 RVA: 0x00038964 File Offset: 0x00036B64
	public override void OnBlockAdded(WorldBase world, Chunk _chunk, Vector3i _blockPos, BlockValue _blockValue, PlatformUserIdentifierAbs _addedByPlayer)
	{
		base.OnBlockAdded(world, _chunk, _blockPos, _blockValue, _addedByPlayer);
		if (!world.IsRemote())
		{
			_blockValue.damage = this.Count;
			world.SetBlockRPC(_blockPos, _blockValue);
			world.GetWBT().AddScheduledBlockUpdate(_blockPos, this.blockID, 1UL);
		}
	}

	// Token: 0x04000929 RID: 2345
	[PublicizedFrom(EAccessModifier.Private)]
	public BlockValue waterBlock;

	// Token: 0x0400092A RID: 2346
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i[,] fallDirsSet;

	// Token: 0x0400092B RID: 2347
	[PublicizedFrom(EAccessModifier.Private)]
	public static int fallSet;

	// Token: 0x0400092C RID: 2348
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i emitionPos;
}
