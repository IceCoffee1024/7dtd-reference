using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000159 RID: 345
[Preserve]
public class BlockTNT : Block
{
	// Token: 0x06000984 RID: 2436 RVA: 0x00041DE7 File Offset: 0x0003FFE7
	public override void Init()
	{
		base.Init();
		this.explosion = new ExplosionData(base.Properties, null);
	}

	// Token: 0x06000985 RID: 2437 RVA: 0x00041E04 File Offset: 0x00040004
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

	// Token: 0x06000986 RID: 2438 RVA: 0x00041E5A File Offset: 0x0004005A
	public override Block.DestroyedResult OnBlockDestroyedByExplosion(WorldBase _world, BlockValueRef _pos, BlockValue _blockValue, int _playerIdx)
	{
		base.OnBlockDestroyedByExplosion(_world, _pos, _blockValue, _playerIdx);
		this.explode(_world, _pos, _playerIdx, _world.GetGameRandom().RandomFloat * 0.5f + 0.3f);
		return Block.DestroyedResult.Remove;
	}

	// Token: 0x06000987 RID: 2439 RVA: 0x00041E8C File Offset: 0x0004008C
	[PublicizedFrom(EAccessModifier.Private)]
	public void explode(WorldBase _world, BlockValueRef _bvRef, int _entityId, float _delay)
	{
		Vector3 vector = _bvRef.ToVector3Center(_world);
		_world.GetGameManager().ExplosionServer(vector, _bvRef, Quaternion.identity, this.explosion, _entityId, _delay, true, null);
	}

	// Token: 0x040009D3 RID: 2515
	[PublicizedFrom(EAccessModifier.Private)]
	public ExplosionData explosion;
}
