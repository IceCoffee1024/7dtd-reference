using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200017C RID: 380
[Preserve]
public class TEFeatureExplodable : TEFeatureAbs, IFeatureTriggerCapability
{
	// Token: 0x170000E0 RID: 224
	// (get) Token: 0x06000B21 RID: 2849 RVA: 0x00020021 File Offset: 0x0001E221
	public IFeatureTriggerCapability.ETriggerRole TriggerRole
	{
		get
		{
			return IFeatureTriggerCapability.ETriggerRole.TriggeredBy;
		}
	}

	// Token: 0x06000B22 RID: 2850 RVA: 0x00047A88 File Offset: 0x00045C88
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

	// Token: 0x06000B23 RID: 2851 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void CopyFromInternal(TileEntityComposite _other)
	{
	}

	// Token: 0x06000B24 RID: 2852 RVA: 0x00047AE7 File Offset: 0x00045CE7
	public void OnBlockTriggered(EntityPlayer _player, Vector3i _blockPos, BlockValue _blockValue, List<BlockChangeInfo> _blockChanges, BlockTrigger _triggeredBy)
	{
		this.Explode(_blockPos, _blockValue);
	}

	// Token: 0x06000B25 RID: 2853 RVA: 0x00047AF1 File Offset: 0x00045CF1
	public override void OnAdded(Vector3i _blockPos, BlockValue _blockValue)
	{
		base.OnAdded(_blockPos, _blockValue);
		_blockValue.rotation = (byte)(GameManager.Instance.World.GetGameRandom().RandomFloat * 4f);
	}

	// Token: 0x06000B26 RID: 2854 RVA: 0x00047B1D File Offset: 0x00045D1D
	public override void OnBlockStartsToFall(Vector3i _blockPos, BlockValue _blockValue)
	{
		base.OnBlockStartsToFall(_blockPos, _blockValue);
		this.Explode(_blockPos, _blockValue);
	}

	// Token: 0x06000B27 RID: 2855 RVA: 0x00047B2F File Offset: 0x00045D2F
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

	// Token: 0x06000B28 RID: 2856 RVA: 0x00047B4C File Offset: 0x00045D4C
	public override Block.DestroyedResult OnBlockDestroyedByExplosion(Vector3i _blockPos, BlockValue _blockValue, int _playerThatStartedExpl)
	{
		base.OnBlockDestroyedByExplosion(_blockPos, _blockValue, _playerThatStartedExpl);
		this.Explode(_blockPos, _blockValue);
		return Block.DestroyedResult.Remove;
	}

	// Token: 0x06000B29 RID: 2857 RVA: 0x00047B61 File Offset: 0x00045D61
	public override void Read(PooledBinaryReader _br, TileEntity.StreamModeRead _eStreamMode)
	{
		base.Read(_br, _eStreamMode);
		if (_eStreamMode == TileEntity.StreamModeRead.Persistency)
		{
			_br.ReadUInt16();
		}
	}

	// Token: 0x06000B2A RID: 2858 RVA: 0x00047B75 File Offset: 0x00045D75
	public override void Write(PooledBinaryWriter _bw, TileEntity.StreamModeWrite _eStreamMode)
	{
		base.Write(_bw, _eStreamMode);
		if (_eStreamMode == TileEntity.StreamModeWrite.Persistency)
		{
			_bw.Write(1);
		}
	}

	// Token: 0x06000B2B RID: 2859 RVA: 0x00047B8C File Offset: 0x00045D8C
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

	// Token: 0x04000A7D RID: 2685
	[PublicizedFrom(EAccessModifier.Private)]
	public const int Version = 1;

	// Token: 0x04000A7E RID: 2686
	[PublicizedFrom(EAccessModifier.Private)]
	public ExplosionData explosionData;
}
