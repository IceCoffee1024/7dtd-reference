using System;

// Token: 0x02000472 RID: 1138
public struct DamageResponse
{
	// Token: 0x0600221A RID: 8730 RVA: 0x000CE624 File Offset: 0x000CC824
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

	// Token: 0x0600221B RID: 8731 RVA: 0x000CE690 File Offset: 0x000CC890
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

	// Token: 0x040017D4 RID: 6100
	public DamageSource Source;

	// Token: 0x040017D5 RID: 6101
	public int Strength;

	// Token: 0x040017D6 RID: 6102
	public int ModStrength;

	// Token: 0x040017D7 RID: 6103
	public int MovementState;

	// Token: 0x040017D8 RID: 6104
	public Utils.EnumHitDirection HitDirection;

	// Token: 0x040017D9 RID: 6105
	public EnumBodyPartHit HitBodyPart;

	// Token: 0x040017DA RID: 6106
	public bool PainHit;

	// Token: 0x040017DB RID: 6107
	public bool Fatal;

	// Token: 0x040017DC RID: 6108
	public bool Critical;

	// Token: 0x040017DD RID: 6109
	public bool Dismember;

	// Token: 0x040017DE RID: 6110
	public bool CrippleLegs;

	// Token: 0x040017DF RID: 6111
	public bool TurnIntoCrawler;

	// Token: 0x040017E0 RID: 6112
	public float Random;

	// Token: 0x040017E1 RID: 6113
	public float ImpulseScale;

	// Token: 0x040017E2 RID: 6114
	public EnumEntityStunType Stun;

	// Token: 0x040017E3 RID: 6115
	public float StunDuration;

	// Token: 0x040017E4 RID: 6116
	public EquipmentSlots ArmorSlot;

	// Token: 0x040017E5 RID: 6117
	public EquipmentSlotGroups ArmorSlotGroup;

	// Token: 0x040017E6 RID: 6118
	public int ArmorDamage;
}
