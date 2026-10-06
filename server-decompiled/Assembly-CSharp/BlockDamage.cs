using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200011D RID: 285
[Preserve]
public class BlockDamage : Block
{
	// Token: 0x0600077E RID: 1918 RVA: 0x00035492 File Offset: 0x00033692
	public BlockDamage()
	{
		this.IsCheckCollideWithEntity = true;
	}

	// Token: 0x0600077F RID: 1919 RVA: 0x000354A8 File Offset: 0x000336A8
	public override void Init()
	{
		base.Init();
		if (base.Properties.Values.ContainsKey(Block.PropDamage))
		{
			int.TryParse(base.Properties.Values[Block.PropDamage], out this.damage);
		}
		else
		{
			Log.Error("Block " + base.GetBlockName() + " is a BlockDamage but does not specify a damage value");
			this.damage = 0;
		}
		if (base.Properties.Values.ContainsKey(BlockDamage.PropDamageReceived))
		{
			int.TryParse(base.Properties.Values[BlockDamage.PropDamageReceived], out this.damageReceived);
		}
		else
		{
			this.damageReceived = 0;
		}
		base.Properties.ParseEnum<EnumDamageTypes>(BlockDamage.PropDamageType, ref this.damageType);
	}

	// Token: 0x06000780 RID: 1920 RVA: 0x00035570 File Offset: 0x00033770
	public override void GetCollisionAABB(BlockValue _blockValue, int _x, int _y, int _z, float _distortedY, List<Bounds> _result)
	{
		base.GetCollisionAABB(_blockValue, _x, _y, _z, _distortedY, _result);
		Vector3 vector = new Vector3(0.05f, 0.05f, 0.05f);
		for (int i = 0; i < _result.Count; i++)
		{
			Bounds bounds = _result[i];
			bounds.SetMinMax(bounds.min - vector, bounds.max + vector);
			_result[i] = bounds;
		}
	}

	// Token: 0x06000781 RID: 1921 RVA: 0x000355E7 File Offset: 0x000337E7
	public override IList<Bounds> GetClipBoundsList(BlockValue _blockValue, Vector3 _blockPos)
	{
		Block.staticList_IntersectRayWithBlockList.Clear();
		this.GetCollisionAABB(_blockValue, (int)_blockPos.x, (int)_blockPos.y, (int)_blockPos.z, 0f, Block.staticList_IntersectRayWithBlockList);
		return Block.staticList_IntersectRayWithBlockList;
	}

	// Token: 0x06000782 RID: 1922 RVA: 0x00035620 File Offset: 0x00033820
	public override bool OnEntityCollidedWithBlock(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, Entity _targetEntity)
	{
		if (!(_targetEntity is EntityAlive))
		{
			return false;
		}
		EntityAlive entityAlive = (EntityAlive)_targetEntity;
		if (entityAlive.IsDead())
		{
			return false;
		}
		DamageSourceEntity damageSourceEntity = new DamageSourceEntity(EnumDamageSource.External, this.damageType, -1);
		damageSourceEntity.AttackingItem = _blockValue.ToItemValue();
		damageSourceEntity.BlockPosition = _blockPos;
		damageSourceEntity.SetIgnoreConsecutiveDamages(true);
		bool flag;
		if (entityAlive is EntityHuman)
		{
			damageSourceEntity.hitTransformName = entityAlive.emodel.GetHitTransform(BodyPrimaryHit.Torso).name;
			flag = _targetEntity.DamageEntity(damageSourceEntity, this.damage, false, 1f) > 0;
		}
		else
		{
			flag = _targetEntity.DamageEntity(damageSourceEntity, this.damage, false, 1f) > 0;
		}
		bool flag2 = false;
		int num = entityAlive.CalculateBlockDamage(this, this.damageReceived, out flag2);
		if (this.MovementFactor != 1f)
		{
			entityAlive.SetMotionMultiplier(EffectManager.GetValue(PassiveEffects.MovementFactorMultiplier, null, this.MovementFactor, entityAlive, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false));
		}
		if (flag && num > 0 && (World.SandboxUseTraderArea != TraderAreaStates.Default || !((World)_world).IsWithinTraderArea(_blockPos)))
		{
			this.DamageBlock(_world, _blockPos, _blockValue, num, (_targetEntity != null) ? _targetEntity.entityId : (-1), null, false, flag2);
		}
		return flag;
	}

	// Token: 0x040008E6 RID: 2278
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropDamageReceived = "Damage_received";

	// Token: 0x040008E7 RID: 2279
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropDamageType = "DamageType";

	// Token: 0x040008E8 RID: 2280
	[PublicizedFrom(EAccessModifier.Protected)]
	public int damage;

	// Token: 0x040008E9 RID: 2281
	[PublicizedFrom(EAccessModifier.Protected)]
	public int damageReceived;

	// Token: 0x040008EA RID: 2282
	[PublicizedFrom(EAccessModifier.Private)]
	public EnumDamageTypes damageType = EnumDamageTypes.Piercing;
}
