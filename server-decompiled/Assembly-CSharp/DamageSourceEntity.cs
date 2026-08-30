using System;
using UnityEngine;

// Token: 0x0200053D RID: 1341
public class DamageSourceEntity : DamageSource
{
	// Token: 0x06002C1A RID: 11290 RVA: 0x0011633A File Offset: 0x0011453A
	public DamageSourceEntity(EnumDamageSource _damageSource, EnumDamageTypes _damageType, int _damageSourceEntityId)
		: base(_damageSource, _damageType)
	{
		this.ownerEntityId = _damageSourceEntityId;
	}

	// Token: 0x06002C1B RID: 11291 RVA: 0x0011634B File Offset: 0x0011454B
	public DamageSourceEntity(EnumDamageSource _damageSource, EnumDamageTypes _damageType, int _damageSourceEntityId, Vector3 _direction)
		: base(_damageSource, _damageType, _direction)
	{
		this.ownerEntityId = _damageSourceEntityId;
	}

	// Token: 0x06002C1C RID: 11292 RVA: 0x0011635E File Offset: 0x0011455E
	public DamageSourceEntity(EnumDamageSource _damageSource, EnumDamageTypes _damageType, int _damageSourceEntityId, Vector3 _direction, string _hitTransformName, Vector3 _hitTransformPosition, Vector2 _uvHit)
		: this(_damageSource, _damageType, _damageSourceEntityId, _direction)
	{
		this.hitTransformName = _hitTransformName;
		this.hitTransformPosition = _hitTransformPosition;
		this.uvHit = _uvHit;
	}

	// Token: 0x06002C1D RID: 11293 RVA: 0x00116383 File Offset: 0x00114583
	public override Vector3 getHitTransformPosition()
	{
		return this.hitTransformPosition;
	}

	// Token: 0x06002C1E RID: 11294 RVA: 0x0011638B File Offset: 0x0011458B
	public override string getHitTransformName()
	{
		return this.hitTransformName;
	}

	// Token: 0x06002C1F RID: 11295 RVA: 0x00116393 File Offset: 0x00114593
	public override Vector2 getUVHit()
	{
		return this.uvHit;
	}

	// Token: 0x040021B4 RID: 8628
	public Vector2 uvHit;

	// Token: 0x040021B5 RID: 8629
	public string hitTransformName;

	// Token: 0x040021B6 RID: 8630
	public Vector3 hitTransformPosition;
}
