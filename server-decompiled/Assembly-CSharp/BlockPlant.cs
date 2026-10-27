using System;
using UnityEngine.Scripting;

// Token: 0x0200013A RID: 314
[Preserve]
public class BlockPlant : Block
{
	// Token: 0x06000876 RID: 2166 RVA: 0x0003BDB1 File Offset: 0x00039FB1
	public BlockPlant()
	{
		this.IsRandomlyTick = true;
	}

	// Token: 0x06000877 RID: 2167 RVA: 0x0003BDC7 File Offset: 0x00039FC7
	public override bool CanPlaceBlockAt(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, bool _bOmitCollideCheck = false)
	{
		return base.CanPlaceBlockAt(_world, _blockPos, _blockValue, _bOmitCollideCheck) && this.CanGrowOn(_world, _blockPos - Vector3i.up, _blockValue);
	}

	// Token: 0x06000878 RID: 2168 RVA: 0x0003BDEC File Offset: 0x00039FEC
	public virtual bool CanGrowOn(WorldBase _world, Vector3i _blockPos, BlockValue _blockValueOfPlant)
	{
		return GameManager.Instance.IsEditMode() || this.fertileLevel == 0 || _world.GetBlock(_blockPos).Block.blockMaterial.FertileLevel >= this.fertileLevel;
	}

	// Token: 0x06000879 RID: 2169 RVA: 0x0003BE35 File Offset: 0x0003A035
	public override void OnNeighborBlockChange(WorldBase _world, Vector3i _myBlockPos, BlockValue _myBlockValue, Vector3i _blockPosThatChanged, BlockValue _newNeighborBlockValue, BlockValue _oldNeighborBlockValue)
	{
		base.OnNeighborBlockChange(_world, _myBlockPos, _myBlockValue, _blockPosThatChanged, _newNeighborBlockValue, _oldNeighborBlockValue);
		if (!_myBlockValue.ischild)
		{
			this.CheckPlantAlive(_world, _myBlockPos, _myBlockValue);
		}
	}

	// Token: 0x0600087A RID: 2170 RVA: 0x0003BE59 File Offset: 0x0003A059
	public override bool UpdateTick(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, bool _bRandomTick, ulong _ticksIfLoaded, GameRandom _rnd)
	{
		this.CheckPlantAlive(_world, _blockPos, _blockValue);
		return false;
	}

	// Token: 0x0600087B RID: 2171 RVA: 0x0003BE66 File Offset: 0x0003A066
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

	// Token: 0x0600087C RID: 2172 RVA: 0x0003BE98 File Offset: 0x0003A098
	public override bool CanPlantStay(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue)
	{
		return GameManager.Instance.IsEditMode() || ((this.lightLevelStay == 0 || _world.GetBlockLightValue(_blockPos) >= this.lightLevelStay || _world.GetBlockLightValue(_blockPos + Vector3i.up) >= this.lightLevelStay || _world.IsOpenSkyAbove(_blockPos.x, _blockPos.y, _blockPos.z)) && this.CanGrowOn(_world, _blockPos - Vector3i.up, _blockValue));
	}

	// Token: 0x0600087D RID: 2173 RVA: 0x0003BF12 File Offset: 0x0003A112
	public override BlockValue OnBlockPlaced(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, GameRandom _rnd)
	{
		_blockValue.rotation = (byte)_rnd.RandomRange(4);
		return _blockValue;
	}

	// Token: 0x04000964 RID: 2404
	[PublicizedFrom(EAccessModifier.Protected)]
	public int lightLevelStay;

	// Token: 0x04000965 RID: 2405
	[PublicizedFrom(EAccessModifier.Protected)]
	public int fertileLevel = 1;
}
