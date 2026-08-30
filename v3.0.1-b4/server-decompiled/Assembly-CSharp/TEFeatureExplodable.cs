using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200017A RID: 378
[Preserve]
public class TEFeatureExplodable : TEFeatureAbs, IFeatureTriggerCapability
{
	// Token: 0x170000E0 RID: 224
	// (get) Token: 0x06000B16 RID: 2838 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public IFeatureTriggerCapability.ETriggerRole TriggerRole
	{
		get
		{
			return IFeatureTriggerCapability.ETriggerRole.TriggeredBy;
		}
	}

	// Token: 0x06000B17 RID: 2839 RVA: 0x00047670 File Offset: 0x00045870
	public override void Init(TileEntityComposite _parent, TileEntityFeatureData _featureData)
	{
		base.Init(_parent, _featureData);
		DynamicProperties props = _featureData.Props;
		this.explosionData = new ExplosionData(props, null);
		if (this.explosionData.ParticleIndex == 0)
		{
			Log.Error("Block with name " + base.Parent.TeData.Block.GetBlockName() + " has feature TEFeatureExplodable but no property ParticleIndex");
		}
	}

	// Token: 0x06000B18 RID: 2840 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void CopyFromInternal(TileEntityComposite _other)
	{
	}

	// Token: 0x06000B19 RID: 2841 RVA: 0x000476CF File Offset: 0x000458CF
	public void OnBlockTriggered(EntityPlayer _player, Vector3i _blockPos, BlockValue _blockValue, List<BlockChangeInfo> _blockChanges, BlockTrigger _triggeredBy)
	{
		this.Explode(_blockPos, _blockValue);
	}

	// Token: 0x06000B1A RID: 2842 RVA: 0x000476D9 File Offset: 0x000458D9
	public override void OnAdded(Vector3i _blockPos, BlockValue _blockValue)
	{
		base.OnAdded(_blockPos, _blockValue);
		_blockValue.rotation = (byte)(GameManager.Instance.World.GetGameRandom().RandomFloat * 4f);
	}

	// Token: 0x06000B1B RID: 2843 RVA: 0x00047705 File Offset: 0x00045905
	public override void OnBlockStartsToFall(Vector3i _blockPos, BlockValue _blockValue)
	{
		base.OnBlockStartsToFall(_blockPos, _blockValue);
		this.Explode(_blockPos, _blockValue);
	}

	// Token: 0x06000B1C RID: 2844 RVA: 0x00047717 File Offset: 0x00045917
	public override Block.DestroyedResult OnBlockDestroyedBy(Vector3i _blockPos, BlockValue _blockValue, int _entityId, bool _bUseHarvestTool)
	{
		base.OnBlockDestroyedBy(_blockPos, _blockValue, _entityId, _bUseHarvestTool);
		if (!_bUseHarvestTool)
		{
			this.Explode(_blockPos, _blockValue);
			return Block.DestroyedResult.Remove;
		}
		return Block.DestroyedResult.None;
	}

	// Token: 0x06000B1D RID: 2845 RVA: 0x00047734 File Offset: 0x00045934
	public override Block.DestroyedResult OnBlockDestroyedByExplosion(Vector3i _blockPos, BlockValue _blockValue, int _playerThatStartedExpl)
	{
		base.OnBlockDestroyedByExplosion(_blockPos, _blockValue, _playerThatStartedExpl);
		this.Explode(_blockPos, _blockValue);
		return Block.DestroyedResult.Remove;
	}

	// Token: 0x06000B1E RID: 2846 RVA: 0x00047749 File Offset: 0x00045949
	public override void Read(PooledBinaryReader _br, TileEntity.StreamModeRead _eStreamMode)
	{
		base.Read(_br, _eStreamMode);
		if (_eStreamMode == TileEntity.StreamModeRead.Persistency)
		{
			_br.ReadUInt16();
		}
	}

	// Token: 0x06000B1F RID: 2847 RVA: 0x0004775D File Offset: 0x0004595D
	public override void Write(PooledBinaryWriter _bw, TileEntity.StreamModeWrite _eStreamMode)
	{
		base.Write(_bw, _eStreamMode);
		if (_eStreamMode == TileEntity.StreamModeWrite.Persistency)
		{
			_bw.Write(1);
		}
	}

	// Token: 0x06000B20 RID: 2848 RVA: 0x00047774 File Offset: 0x00045974
	[PublicizedFrom(EAccessModifier.Private)]
	public void Explode(Vector3i _blockPos, BlockValue _blockValue)
	{
		if (_blockValue.Block == null)
		{
			return;
		}
		Vector3 vector = _blockPos.ToVector3();
		Quaternion quaternion = Quaternion.identity;
		BlockEntityData blockEntity = GameManager.Instance.World.ChunkCache.GetBlockEntity(_blockPos);
		if (blockEntity != null && blockEntity.transform)
		{
			vector = blockEntity.transform.position + Origin.position;
			quaternion = blockEntity.transform.rotation;
		}
		GameManager.Instance.ExplosionServer(vector, _blockPos, quaternion, this.explosionData, -1, 0.1f, false, null);
	}

	// Token: 0x04000A70 RID: 2672
	[PublicizedFrom(EAccessModifier.Private)]
	public const int Version = 1;

	// Token: 0x04000A71 RID: 2673
	[PublicizedFrom(EAccessModifier.Private)]
	public ExplosionData explosionData;
}
