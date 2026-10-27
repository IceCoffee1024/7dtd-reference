using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200017D RID: 381
[Preserve]
public class TEFeatureExplodable : TEFeatureAbs, IFeatureTriggerCapability
{
	// Token: 0x170000DF RID: 223
	// (get) Token: 0x06000B2A RID: 2858 RVA: 0x00020119 File Offset: 0x0001E319
	public IFeatureTriggerCapability.ETriggerRole TriggerRole
	{
		get
		{
			return IFeatureTriggerCapability.ETriggerRole.TriggeredBy;
		}
	}

	// Token: 0x06000B2B RID: 2859 RVA: 0x00047C24 File Offset: 0x00045E24
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

	// Token: 0x06000B2C RID: 2860 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void CopyFromInternal(TileEntityComposite _other)
	{
	}

	// Token: 0x06000B2D RID: 2861 RVA: 0x00047C83 File Offset: 0x00045E83
	public void OnBlockTriggered(EntityPlayer _player, Vector3i _blockPos, BlockValue _blockValue, List<BlockChangeInfo> _blockChanges, BlockTrigger _triggeredBy)
	{
		this.Explode(_blockPos, _blockValue);
	}

	// Token: 0x06000B2E RID: 2862 RVA: 0x00047C8D File Offset: 0x00045E8D
	public override void OnAdded(Vector3i _blockPos, BlockValue _blockValue)
	{
		base.OnAdded(_blockPos, _blockValue);
		_blockValue.rotation = (byte)(GameManager.Instance.World.GetGameRandom().RandomFloat * 4f);
	}

	// Token: 0x06000B2F RID: 2863 RVA: 0x00047CB9 File Offset: 0x00045EB9
	public override void OnBlockStartsToFall(Vector3i _blockPos, BlockValue _blockValue)
	{
		base.OnBlockStartsToFall(_blockPos, _blockValue);
		this.Explode(_blockPos, _blockValue);
	}

	// Token: 0x06000B30 RID: 2864 RVA: 0x00047CCB File Offset: 0x00045ECB
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

	// Token: 0x06000B31 RID: 2865 RVA: 0x00047CE8 File Offset: 0x00045EE8
	public override Block.DestroyedResult OnBlockDestroyedByExplosion(Vector3i _blockPos, BlockValue _blockValue, int _playerThatStartedExpl)
	{
		base.OnBlockDestroyedByExplosion(_blockPos, _blockValue, _playerThatStartedExpl);
		this.Explode(_blockPos, _blockValue);
		return Block.DestroyedResult.Remove;
	}

	// Token: 0x06000B32 RID: 2866 RVA: 0x00047CFD File Offset: 0x00045EFD
	public override void Read(PooledBinaryReader _br, StreamModeRead _eStreamMode)
	{
		base.Read(_br, _eStreamMode);
		if (_eStreamMode == StreamModeRead.Persistency)
		{
			_br.ReadUInt16();
		}
	}

	// Token: 0x06000B33 RID: 2867 RVA: 0x00047D11 File Offset: 0x00045F11
	public override void Write(PooledBinaryWriter _bw, StreamModeWrite _eStreamMode)
	{
		base.Write(_bw, _eStreamMode);
		if (_eStreamMode == StreamModeWrite.Persistency)
		{
			_bw.Write(1);
		}
	}

	// Token: 0x06000B34 RID: 2868 RVA: 0x00047D28 File Offset: 0x00045F28
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

	// Token: 0x04000A7C RID: 2684
	[PublicizedFrom(EAccessModifier.Private)]
	public const int Version = 1;

	// Token: 0x04000A7D RID: 2685
	[PublicizedFrom(EAccessModifier.Private)]
	public ExplosionData explosionData;
}
