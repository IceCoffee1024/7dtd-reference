using System;

// Token: 0x0200046A RID: 1130
public struct DamageResponse
{
	// Token: 0x06002209 RID: 8713 RVA: 0x000CDA28 File Offset: 0x000CBC28
	public static DamageResponse New(bool _fatal)
	{
		return new DamageResponse
		{
			HitBodyPart = EnumBodyPartHit.Torso,
			Random = GameManager.Instance.World.GetGameRandom().RandomFloat,
			Fatal = _fatal,
			PainHit = !_fatal,
			ImpulseScale = 1f,
			ArmorSlot = EquipmentSlots.Count,
			ArmorDamage = 0
		};
	}

	// Token: 0x0600220A RID: 8714 RVA: 0x000CDA94 File Offset: 0x000CBC94
	public static DamageResponse New(DamageSource _source, bool _fatal)
	{
		return new DamageResponse
		{
			HitBodyPart = EnumBodyPartHit.Torso,
			Random = GameManager.Instance.World.GetGameRandom().RandomFloat,
			Fatal = _fatal,
			PainHit = !_fatal,
			Source = _source,
			ImpulseScale = 1f,
			ArmorSlot = EquipmentSlots.Count,
			ArmorDamage = 0
		};
	}

	// Token: 0x040017B7 RID: 6071
	public DamageSource Source;

	// Token: 0x040017B8 RID: 6072
	public int Strength;

	// Token: 0x040017B9 RID: 6073
	public int ModStrength;

	// Token: 0x040017BA RID: 6074
	public int MovementState;

	// Token: 0x040017BB RID: 6075
	public Utils.EnumHitDirection HitDirection;

	// Token: 0x040017BC RID: 6076
	public EnumBodyPartHit HitBodyPart;

	// Token: 0x040017BD RID: 6077
	public bool PainHit;

	// Token: 0x040017BE RID: 6078
	public bool Fatal;

	// Token: 0x040017BF RID: 6079
	public bool Critical;

	// Token: 0x040017C0 RID: 6080
	public bool Dismember;

	// Token: 0x040017C1 RID: 6081
	public bool CrippleLegs;

	// Token: 0x040017C2 RID: 6082
	public bool TurnIntoCrawler;

	// Token: 0x040017C3 RID: 6083
	public float Random;

	// Token: 0x040017C4 RID: 6084
	public float ImpulseScale;

	// Token: 0x040017C5 RID: 6085
	public EnumEntityStunType Stun;

	// Token: 0x040017C6 RID: 6086
	public float StunDuration;

	// Token: 0x040017C7 RID: 6087
	public EquipmentSlots ArmorSlot;

	// Token: 0x040017C8 RID: 6088
	public EquipmentSlotGroups ArmorSlotGroup;

	// Token: 0x040017C9 RID: 6089
	public int ArmorDamage;
}
