using System;
using UnityEngine;

// Token: 0x02000542 RID: 1346
public class DamageSourceEntity : DamageSource
{
	// Token: 0x06002C44 RID: 11332 RVA: 0x001173D2 File Offset: 0x001155D2
	public DamageSourceEntity(EnumDamageSource _damageSource, EnumDamageTypes _damageType, int _damageSourceEntityId)
		: base(_damageSource, _damageType)
	{
		this.ownerEntityId = _damageSourceEntityId;
	}

	// Token: 0x06002C45 RID: 11333 RVA: 0x001173E3 File Offset: 0x001155E3
	public DamageSourceEntity(EnumDamageSource _damageSource, EnumDamageTypes _damageType, int _damageSourceEntityId, Vector3 _direction)
		: base(_damageSource, _damageType, _direction)
	{
		this.ownerEntityId = _damageSourceEntityId;
	}

	// Token: 0x06002C46 RID: 11334 RVA: 0x001173F6 File Offset: 0x001155F6
	public DamageSourceEntity(EnumDamageSource _damageSource, EnumDamageTypes _damageType, int _damageSourceEntityId, Vector3 _direction, string _hitTransformName, Vector3 _hitTransformPosition, Vector2 _uvHit)
		: this(_damageSource, _damageType, _damageSourceEntityId, _direction)
	{
		this.hitTransformName = _hitTransformName;
		this.hitTransformPosition = _hitTransformPosition;
		this.uvHit = _uvHit;
	}

	// Token: 0x06002C47 RID: 11335 RVA: 0x0011741B File Offset: 0x0011561B
	public override Vector3 getHitTransformPosition()
	{
		return this.hitTransformPosition;
	}

	// Token: 0x06002C48 RID: 11336 RVA: 0x00117423 File Offset: 0x00115623
	public override string getHitTransformName()
	{
		return this.hitTransformName;
	}

	// Token: 0x06002C49 RID: 11337 RVA: 0x0011742B File Offset: 0x0011562B
	public override Vector2 getUVHit()
	{
		return this.uvHit;
	}

	// Token: 0x040021E6 RID: 8678
	public Vector2 uvHit;

	// Token: 0x040021E7 RID: 8679
	public string hitTransformName;

	// Token: 0x040021E8 RID: 8680
	public Vector3 hitTransformPosition;
}
