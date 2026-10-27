using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200015C RID: 348
[Preserve]
public class BlockTNT : Block
{
	// Token: 0x06000994 RID: 2452 RVA: 0x000420E3 File Offset: 0x000402E3
	public override void Init()
	{
		base.Init();
		this.explosion = new ExplosionData(base.Properties, null);
	}

	// Token: 0x06000995 RID: 2453 RVA: 0x00042100 File Offset: 0x00040300
	public override int OnBlockDamaged(WorldBase _world, BlockValueRef _bvRef, BlockValue _blockValue, int _damagePoints, int _entityId, ItemActionAttack.AttackHitInfo _attackHitInfo, bool _bUseHarvestTool, bool _bBypassMaxDamage, int _recDepth = 0)
	{
		if (_world.GetGameRandom().RandomFloat <= (float)_damagePoints / (float)_blockValue.Block.MaxDamage)
		{
			this.explode(_world, _bvRef, _entityId, 0.1f);
		}
		ChunkCluster chunkCache = _world.ChunkCache;
		if (chunkCache != null)
		{
			chunkCache.InvokeOnBlockDamagedDelegates(_bvRef, _blockValue, _damagePoints, _entityId);
		}
		return _blockValue.damage;
	}

	// Token: 0x06000996 RID: 2454 RVA: 0x00042156 File Offset: 0x00040356
	public override Block.DestroyedResult OnBlockDestroyedByExplosion(WorldBase _world, BlockValueRef _pos, BlockValue _blockValue, int _playerIdx)
	{
		base.OnBlockDestroyedByExplosion(_world, _pos, _blockValue, _playerIdx);
		this.explode(_world, _pos, _playerIdx, _world.GetGameRandom().RandomFloat * 0.5f + 0.3f);
		return Block.DestroyedResult.Remove;
	}

	// Token: 0x06000997 RID: 2455 RVA: 0x00042188 File Offset: 0x00040388
	[PublicizedFrom(EAccessModifier.Private)]
	public void explode(WorldBase _world, BlockValueRef _bvRef, int _entityId, float _delay)
	{
		Vector3 vector = _bvRef.ToVector3Center(_world);
		_world.GetGameManager().ExplosionServer(vector, _bvRef, Quaternion.identity, this.explosion, _entityId, _delay, true, null);
	}

	// Token: 0x040009D8 RID: 2520
	[PublicizedFrom(EAccessModifier.Private)]
	public ExplosionData explosion;
}
