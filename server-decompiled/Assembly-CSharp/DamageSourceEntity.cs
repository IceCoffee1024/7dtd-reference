using System;
using UnityEngine;

// Token: 0x02000548 RID: 1352
public class DamageSourceEntity : DamageSource
{
	// Token: 0x06002C3A RID: 11322 RVA: 0x001181A2 File Offset: 0x001163A2
	public DamageSourceEntity(EnumDamageSource _damageSource, EnumDamageTypes _damageType, int _damageSourceEntityId)
		: base(_damageSource, _damageType)
	{
		this.ownerEntityId = _damageSourceEntityId;
	}

	// Token: 0x06002C3B RID: 11323 RVA: 0x001181B3 File Offset: 0x001163B3
	public DamageSourceEntity(EnumDamageSource _damageSource, EnumDamageTypes _damageType, int _damageSourceEntityId, Vector3 _direction)
		: base(_damageSource, _damageType, _direction)
	{
		this.ownerEntityId = _damageSourceEntityId;
	}

	// Token: 0x06002C3C RID: 11324 RVA: 0x001181C6 File Offset: 0x001163C6
	public DamageSourceEntity(EnumDamageSource _damageSource, EnumDamageTypes _damageType, int _damageSourceEntityId, Vector3 _direction, string _hitTransformName, Vector3 _hitTransformPosition, Vector2 _uvHit)
		: this(_damageSource, _damageType, _damageSourceEntityId, _direction)
	{
		this.hitTransformName = _hitTransformName;
		this.hitTransformPosition = _hitTransformPosition;
		this.uvHit = _uvHit;
	}

	// Token: 0x06002C3D RID: 11325 RVA: 0x001181EB File Offset: 0x001163EB
	public override Vector3 getHitTransformPosition()
	{
		return this.hitTransformPosition;
	}

	// Token: 0x06002C3E RID: 11326 RVA: 0x001181F3 File Offset: 0x001163F3
	public override string getHitTransformName()
	{
		return this.hitTransformName;
	}

	// Token: 0x06002C3F RID: 11327 RVA: 0x001181FB File Offset: 0x001163FB
	public override Vector2 getUVHit()
	{
		return this.uvHit;
	}

	// Token: 0x040021EE RID: 8686
	public Vector2 uvHit;

	// Token: 0x040021EF RID: 8687
	public string hitTransformName;

	// Token: 0x040021F0 RID: 8688
	public Vector3 hitTransformPosition;
}
