using System;
using UnityEngine.Scripting;

// Token: 0x02000137 RID: 311
[Preserve]
public class BlockPlant : Block
{
	// Token: 0x06000866 RID: 2150 RVA: 0x0003BAA5 File Offset: 0x00039CA5
	public BlockPlant()
	{
		this.IsRandomlyTick = true;
	}

	// Token: 0x06000867 RID: 2151 RVA: 0x0003BABB File Offset: 0x00039CBB
	public override bool CanPlaceBlockAt(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, bool _bOmitCollideCheck = false)
	{
		return base.CanPlaceBlockAt(_world, _blockPos, _blockValue, _bOmitCollideCheck) && this.CanGrowOn(_world, _blockPos - Vector3i.up, _blockValue);
	}

	// Token: 0x06000868 RID: 2152 RVA: 0x0003BAE0 File Offset: 0x00039CE0
	public virtual bool CanGrowOn(WorldBase _world, Vector3i _blockPos, BlockValue _blockValueOfPlant)
	{
		return GameManager.Instance.IsEditMode() || this.fertileLevel == 0 || _world.GetBlock(_blockPos).Block.blockMaterial.FertileLevel >= this.fertileLevel;
	}

	// Token: 0x06000869 RID: 2153 RVA: 0x0003BB29 File Offset: 0x00039D29
	public override void OnNeighborBlockChange(WorldBase _world, Vector3i _myBlockPos, BlockValue _myBlockValue, Vector3i _blockPosThatChanged, BlockValue _newNeighborBlockValue, BlockValue _oldNeighborBlockValue)
	{
		base.OnNeighborBlockChange(_world, _myBlockPos, _myBlockValue, _blockPosThatChanged, _newNeighborBlockValue, _oldNeighborBlockValue);
		if (!_myBlockValue.ischild)
		{
			this.CheckPlantAlive(_world, _myBlockPos, _myBlockValue);
		}
	}

	// Token: 0x0600086A RID: 2154 RVA: 0x0003BB4D File Offset: 0x00039D4D
	public override bool UpdateTick(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, bool _bRandomTick, ulong _ticksIfLoaded, GameRandom _rnd)
	{
		this.CheckPlantAlive(_world, _blockPos, _blockValue);
		return false;
	}

	// Token: 0x0600086B RID: 2155 RVA: 0x0003BB5A File Offset: 0x00039D5A
	public virtual bool CheckPlantAlive(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue)
	{
		if (GameManager.Instance.IsEditMode())
		{
			return true;
		}
		if (!this.CanPlantStay(_world, _blockPos, _blockValue))
		{
			_world.SetBlockRPC(_blockPos, BlockValue.Air);
			return false;
		}
		return true;
	}

	// Token: 0x0600086C RID: 2156 RVA: 0x0003BB8C File Offset: 0x00039D8C
	public override bool CanPlantStay(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue)
	{
		return GameManager.Instance.IsEditMode() || ((this.lightLevelStay == 0 || _world.GetBlockLightValue(_blockPos) >= this.lightLevelStay || _world.GetBlockLightValue(_blockPos + Vector3i.up) >= this.lightLevelStay || _world.IsOpenSkyAbove(_blockPos.x, _blockPos.y, _blockPos.z)) && this.CanGrowOn(_world, _blockPos - Vector3i.up, _blockValue));
	}

	// Token: 0x0600086D RID: 2157 RVA: 0x0003BC06 File Offset: 0x00039E06
	public override BlockValue OnBlockPlaced(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, GameRandom _rnd)
	{
		_blockValue.rotation = (byte)_rnd.RandomRange(4);
		return _blockValue;
	}

	// Token: 0x0400095F RID: 2399
	[PublicizedFrom(EAccessModifier.Protected)]
	public int lightLevelStay;

	// Token: 0x04000960 RID: 2400
	[PublicizedFrom(EAccessModifier.Protected)]
	public int fertileLevel = 1;
}
