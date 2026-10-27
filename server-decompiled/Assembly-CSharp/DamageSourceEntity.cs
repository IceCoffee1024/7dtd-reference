using System;
using UnityEngine;

// Token: 0x0200054C RID: 1356
public class DamageSourceEntity : DamageSource
{
	// Token: 0x06002C42 RID: 11330 RVA: 0x0011872E File Offset: 0x0011692E
	public DamageSourceEntity(EnumDamageSource _damageSource, EnumDamageTypes _damageType, int _damageSourceEntityId)
		: base(_damageSource, _damageType)
	{
		this.ownerEntityId = _damageSourceEntityId;
	}

	// Token: 0x06002C43 RID: 11331 RVA: 0x0011873F File Offset: 0x0011693F
	public DamageSourceEntity(EnumDamageSource _damageSource, EnumDamageTypes _damageType, int _damageSourceEntityId, Vector3 _direction)
		: base(_damageSource, _damageType, _direction)
	{
		this.ownerEntityId = _damageSourceEntityId;
	}

	// Token: 0x06002C44 RID: 11332 RVA: 0x00118752 File Offset: 0x00116952
	public DamageSourceEntity(EnumDamageSource _damageSource, EnumDamageTypes _damageType, int _damageSourceEntityId, Vector3 _direction, string _hitTransformName, Vector3 _hitTransformPosition, Vector2 _uvHit)
		: this(_damageSource, _damageType, _damageSourceEntityId, _direction)
	{
		this.hitTransformName = _hitTransformName;
		this.hitTransformPosition = _hitTransformPosition;
		this.uvHit = _uvHit;
	}

	// Token: 0x06002C45 RID: 11333 RVA: 0x00118777 File Offset: 0x00116977
	public override Vector3 getHitTransformPosition()
	{
		return this.hitTransformPosition;
	}

	// Token: 0x06002C46 RID: 11334 RVA: 0x0011877F File Offset: 0x0011697F
	public override string getHitTransformName()
	{
		return this.hitTransformName;
	}

	// Token: 0x06002C47 RID: 11335 RVA: 0x00118787 File Offset: 0x00116987
	public override Vector2 getUVHit()
	{
		return this.uvHit;
	}

	// Token: 0x04002205 RID: 8709
	public Vector2 uvHit;

	// Token: 0x04002206 RID: 8710
	public string hitTransformName;

	// Token: 0x04002207 RID: 8711
	public Vector3 hitTransformPosition;
}
