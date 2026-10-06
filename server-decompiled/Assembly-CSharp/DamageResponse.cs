using System;

// Token: 0x02000470 RID: 1136
public struct DamageResponse
{
	// Token: 0x06002215 RID: 8725 RVA: 0x000CE278 File Offset: 0x000CC478
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

	// Token: 0x06002216 RID: 8726 RVA: 0x000CE2E4 File Offset: 0x000CC4E4
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

	// Token: 0x040017C4 RID: 6084
	public DamageSource Source;

	// Token: 0x040017C5 RID: 6085
	public int Strength;

	// Token: 0x040017C6 RID: 6086
	public int ModStrength;

	// Token: 0x040017C7 RID: 6087
	public int MovementState;

	// Token: 0x040017C8 RID: 6088
	public Utils.EnumHitDirection HitDirection;

	// Token: 0x040017C9 RID: 6089
	public EnumBodyPartHit HitBodyPart;

	// Token: 0x040017CA RID: 6090
	public bool PainHit;

	// Token: 0x040017CB RID: 6091
	public bool Fatal;

	// Token: 0x040017CC RID: 6092
	public bool Critical;

	// Token: 0x040017CD RID: 6093
	public bool Dismember;

	// Token: 0x040017CE RID: 6094
	public bool CrippleLegs;

	// Token: 0x040017CF RID: 6095
	public bool TurnIntoCrawler;

	// Token: 0x040017D0 RID: 6096
	public float Random;

	// Token: 0x040017D1 RID: 6097
	public float ImpulseScale;

	// Token: 0x040017D2 RID: 6098
	public EnumEntityStunType Stun;

	// Token: 0x040017D3 RID: 6099
	public float StunDuration;

	// Token: 0x040017D4 RID: 6100
	public EquipmentSlots ArmorSlot;

	// Token: 0x040017D5 RID: 6101
	public EquipmentSlotGroups ArmorSlotGroup;

	// Token: 0x040017D6 RID: 6102
	public int ArmorDamage;
}
