using System;
using UnityEngine;

// Token: 0x02000547 RID: 1351
public class DamageSource
{
	// Token: 0x170004A8 RID: 1192
	// (get) Token: 0x06002C23 RID: 11299 RVA: 0x00117A0C File Offset: 0x00115C0C
	public ItemClass ItemClass
	{
		get
		{
			if (this.AttackingItem != null)
			{
				return this.AttackingItem.ItemClass;
			}
			return null;
		}
	}

	// Token: 0x06002C24 RID: 11300 RVA: 0x00117A24 File Offset: 0x00115C24
	public DamageSource(EnumDamageSource _dsn, EnumDamageTypes _damageType)
	{
		this.damageSource = _dsn;
		this.damageType = _damageType;
		this.DamageTypeTag = FastTags<TagGroup.Global>.Parse(_damageType.ToStringCached<EnumDamageTypes>());
	}

	// Token: 0x06002C25 RID: 11301 RVA: 0x00117A7C File Offset: 0x00115C7C
	public DamageSource(EnumDamageSource _dsn, EnumDamageTypes _damageType, Vector3 _direction)
	{
		this.damageSource = _dsn;
		this.damageType = _damageType;
		this.direction = _direction;
		this.DamageTypeTag = FastTags<TagGroup.Global>.Parse(_damageType.ToStringCached<EnumDamageTypes>());
	}

	// Token: 0x06002C26 RID: 11302 RVA: 0x00117AD9 File Offset: 0x00115CD9
	public bool AffectedByArmor()
	{
		return this.damageSource == EnumDamageSource.External;
	}

	// Token: 0x06002C27 RID: 11303 RVA: 0x00117AE4 File Offset: 0x00115CE4
	public EquipmentSlots GetEntityDamageEquipmentSlot(Entity entity)
	{
		if (entity.emodel)
		{
			Transform hitTransform = entity.emodel.GetHitTransform(this);
			if (hitTransform)
			{
				string tag = hitTransform.tag;
				if ("E_BP_Head".Equals(tag))
				{
					return EquipmentSlots.Head;
				}
				if ("E_BP_Body".Equals(tag))
				{
					return EquipmentSlots.Chest;
				}
				if ("E_BP_LLeg".Equals(tag))
				{
					return EquipmentSlots.Chest;
				}
				if ("E_BP_RLeg".Equals(tag))
				{
					return EquipmentSlots.Chest;
				}
				if ("E_BP_LArm".Equals(tag))
				{
					return EquipmentSlots.Hands;
				}
				if ("E_BP_RArm".Equals(tag))
				{
					return EquipmentSlots.Hands;
				}
			}
		}
		return EquipmentSlots.Count;
	}

	// Token: 0x06002C28 RID: 11304 RVA: 0x00117B78 File Offset: 0x00115D78
	public EquipmentSlotGroups GetEntityDamageEquipmentSlotGroup(Entity entity)
	{
		if (entity.emodel)
		{
			Transform hitTransform = entity.emodel.GetHitTransform(this);
			if (hitTransform)
			{
				string tag = hitTransform.tag;
				if ("E_BP_Head".Equals(tag))
				{
					return EquipmentSlotGroups.Head;
				}
				if ("E_BP_Body".Equals(tag))
				{
					return EquipmentSlotGroups.UpperBody;
				}
				if ("E_BP_LLeg".Equals(tag))
				{
					return EquipmentSlotGroups.LowerBody;
				}
				if ("E_BP_RLeg".Equals(tag))
				{
					return EquipmentSlotGroups.LowerBody;
				}
				if ("E_BP_LArm".Equals(tag))
				{
					return EquipmentSlotGroups.UpperBody;
				}
				"E_BP_RArm".Equals(tag);
				return EquipmentSlotGroups.UpperBody;
			}
		}
		return EquipmentSlotGroups.UpperBody;
	}

	// Token: 0x06002C29 RID: 11305 RVA: 0x00117C08 File Offset: 0x00115E08
	public EnumBodyPartHit GetEntityDamageBodyPart(Entity entity)
	{
		if (this.bodyParts != EnumBodyPartHit.None)
		{
			return this.bodyParts;
		}
		if (entity.emodel)
		{
			Transform hitTransform = entity.emodel.GetHitTransform(this);
			if (hitTransform)
			{
				return DamageSource.TagToBodyPart(hitTransform.tag);
			}
		}
		return EnumBodyPartHit.None;
	}

	// Token: 0x06002C2A RID: 11306 RVA: 0x00117C54 File Offset: 0x00115E54
	public static EnumBodyPartHit TagToBodyPart(string _name)
	{
		uint num = <PrivateImplementationDetails>.ComputeStringHash(_name);
		if (num <= 1181961453U)
		{
			if (num <= 719580451U)
			{
				if (num != 265341418U)
				{
					if (num == 719580451U)
					{
						if (_name == "E_BP_Special")
						{
							return EnumBodyPartHit.Special;
						}
					}
				}
				else if (_name == "E_BP_Head")
				{
					return EnumBodyPartHit.Head;
				}
			}
			else if (num != 769391494U)
			{
				if (num != 1103658916U)
				{
					if (num == 1181961453U)
					{
						if (_name == "E_BP_RLowerLeg")
						{
							return EnumBodyPartHit.RightLowerLeg;
						}
					}
				}
				else if (_name == "E_BP_Body")
				{
					return EnumBodyPartHit.Torso;
				}
			}
			else if (_name == "E_BP_LArm")
			{
				return EnumBodyPartHit.LeftUpperArm;
			}
		}
		else if (num <= 2493191411U)
		{
			if (num != 1478707584U)
			{
				if (num != 2129509723U)
				{
					if (num == 2493191411U)
					{
						if (_name == "E_BP_RLowerArm")
						{
							return EnumBodyPartHit.RightLowerArm;
						}
					}
				}
				else if (_name == "E_BP_LLowerLeg")
				{
					return EnumBodyPartHit.LeftLowerLeg;
				}
			}
			else if (_name == "E_BP_RArm")
			{
				return EnumBodyPartHit.RightUpperArm;
			}
		}
		else if (num != 2661128377U)
		{
			if (num != 2886638712U)
			{
				if (num == 3661196970U)
				{
					if (_name == "E_BP_RLeg")
					{
						return EnumBodyPartHit.RightUpperLeg;
					}
				}
			}
			else if (_name == "E_BP_LLeg")
			{
				return EnumBodyPartHit.LeftUpperLeg;
			}
		}
		else if (_name == "E_BP_LLowerArm")
		{
			return EnumBodyPartHit.LeftLowerArm;
		}
		return EnumBodyPartHit.None;
	}

	// Token: 0x06002C2B RID: 11307 RVA: 0x00117DE8 File Offset: 0x00115FE8
	public void GetEntityDamageBodyPartAndEquipmentSlot(Entity entity, out EnumBodyPartHit bodyPartHit, out EquipmentSlots damageSlot)
	{
		damageSlot = EquipmentSlots.Count;
		bodyPartHit = EnumBodyPartHit.None;
		if (entity.emodel)
		{
			Transform hitTransform = entity.emodel.GetHitTransform(this);
			if (hitTransform)
			{
				string tag = hitTransform.tag;
				if ("E_BP_Head".Equals(tag))
				{
					damageSlot = EquipmentSlots.Head;
					bodyPartHit = EnumBodyPartHit.Head;
					return;
				}
				if ("E_BP_Body".Equals(tag))
				{
					damageSlot = EquipmentSlots.Chest;
					bodyPartHit = EnumBodyPartHit.Torso;
					return;
				}
				if ("E_BP_LLeg".Equals(tag))
				{
					damageSlot = EquipmentSlots.Chest;
					bodyPartHit = EnumBodyPartHit.LeftUpperLeg;
					return;
				}
				if ("E_BP_LLowerLeg".Equals(tag))
				{
					damageSlot = EquipmentSlots.Feet;
					bodyPartHit = EnumBodyPartHit.LeftLowerLeg;
					return;
				}
				if ("E_BP_RLeg".Equals(tag))
				{
					damageSlot = EquipmentSlots.Chest;
					bodyPartHit = EnumBodyPartHit.RightUpperLeg;
					return;
				}
				if ("E_BP_RLowerLeg".Equals(tag))
				{
					damageSlot = EquipmentSlots.Feet;
					bodyPartHit = EnumBodyPartHit.RightLowerLeg;
					return;
				}
				if ("E_BP_LArm".Equals(tag))
				{
					damageSlot = EquipmentSlots.Hands;
					bodyPartHit = EnumBodyPartHit.LeftUpperArm;
					return;
				}
				if ("E_BP_LLowerArm".Equals(tag))
				{
					damageSlot = EquipmentSlots.Hands;
					bodyPartHit = EnumBodyPartHit.LeftLowerArm;
					return;
				}
				if ("E_BP_RArm".Equals(tag))
				{
					damageSlot = EquipmentSlots.Hands;
					bodyPartHit = EnumBodyPartHit.RightUpperArm;
					return;
				}
				if ("E_BP_RLowerArm".Equals(tag))
				{
					damageSlot = EquipmentSlots.Hands;
					bodyPartHit = EnumBodyPartHit.RightLowerArm;
					return;
				}
			}
		}
		else
		{
			if (this.damageType == EnumDamageTypes.Falling)
			{
				bodyPartHit = EnumBodyPartHit.RightLowerLeg;
				damageSlot = EquipmentSlots.Feet;
				return;
			}
			bodyPartHit = EnumBodyPartHit.Torso;
			damageSlot = EquipmentSlots.Chest;
		}
	}

	// Token: 0x06002C2C RID: 11308 RVA: 0x00117F20 File Offset: 0x00116120
	public EquipmentSlots GetDamagedEquipmentSlot(Entity entity)
	{
		if (entity.emodel)
		{
			Transform hitTransform = entity.emodel.GetHitTransform(this);
			if (hitTransform)
			{
				string tag = hitTransform.tag;
				uint num = <PrivateImplementationDetails>.ComputeStringHash(tag);
				if (num <= 1478707584U)
				{
					if (num <= 769391494U)
					{
						if (num != 265341418U)
						{
							if (num == 769391494U)
							{
								if (tag == "E_BP_LArm")
								{
									return EquipmentSlots.Chest;
								}
							}
						}
						else if (tag == "E_BP_Head")
						{
							return EquipmentSlots.Head;
						}
					}
					else if (num != 1103658916U)
					{
						if (num != 1181961453U)
						{
							if (num == 1478707584U)
							{
								if (tag == "E_BP_RArm")
								{
									return EquipmentSlots.Chest;
								}
							}
						}
						else if (tag == "E_BP_RLowerLeg")
						{
							return EquipmentSlots.Feet;
						}
					}
					else if (tag == "E_BP_Body")
					{
						return EquipmentSlots.Chest;
					}
				}
				else if (num <= 2493191411U)
				{
					if (num != 2129509723U)
					{
						if (num == 2493191411U)
						{
							if (tag == "E_BP_RLowerArm")
							{
								return EquipmentSlots.Hands;
							}
						}
					}
					else if (tag == "E_BP_LLowerLeg")
					{
						return EquipmentSlots.Feet;
					}
				}
				else if (num != 2661128377U)
				{
					if (num != 2886638712U)
					{
						if (num == 3661196970U)
						{
							if (tag == "E_BP_RLeg")
							{
								return EquipmentSlots.Chest;
							}
						}
					}
					else if (tag == "E_BP_LLeg")
					{
						return EquipmentSlots.Chest;
					}
				}
				else if (tag == "E_BP_LLowerArm")
				{
					return EquipmentSlots.Hands;
				}
				return EquipmentSlots.Chest;
			}
		}
		else if (this.damageType == EnumDamageTypes.Falling)
		{
			return EquipmentSlots.Feet;
		}
		return EquipmentSlots.Chest;
	}

	// Token: 0x06002C2D RID: 11309 RVA: 0x001180B0 File Offset: 0x001162B0
	public virtual Vector3 getDirection()
	{
		return this.direction;
	}

	// Token: 0x06002C2E RID: 11310 RVA: 0x001180B8 File Offset: 0x001162B8
	public virtual int getEntityId()
	{
		return this.ownerEntityId;
	}

	// Token: 0x06002C2F RID: 11311 RVA: 0x000200DA File Offset: 0x0001E2DA
	public virtual string getHitTransformName()
	{
		return null;
	}

	// Token: 0x06002C30 RID: 11312 RVA: 0x0004E492 File Offset: 0x0004C692
	public virtual Vector3 getHitTransformPosition()
	{
		return Vector3.zero;
	}

	// Token: 0x06002C31 RID: 11313 RVA: 0x0004E4FE File Offset: 0x0004C6FE
	public virtual Vector2 getUVHit()
	{
		return Vector2.zero;
	}

	// Token: 0x06002C32 RID: 11314 RVA: 0x001180C0 File Offset: 0x001162C0
	public virtual EnumDamageSource GetSource()
	{
		return this.damageSource;
	}

	// Token: 0x06002C33 RID: 11315 RVA: 0x001180C8 File Offset: 0x001162C8
	public virtual EnumDamageTypes GetDamageType()
	{
		return this.damageType;
	}

	// Token: 0x170004A9 RID: 1193
	// (get) Token: 0x06002C34 RID: 11316 RVA: 0x001180D0 File Offset: 0x001162D0
	public bool CanStun
	{
		get
		{
			return this.damageType == EnumDamageTypes.Bashing || this.damageType == EnumDamageTypes.Heat || this.damageType == EnumDamageTypes.Piercing || this.damageType == EnumDamageTypes.Crushing || this.damageType == EnumDamageTypes.Falling;
		}
	}

	// Token: 0x06002C35 RID: 11317 RVA: 0x00118102 File Offset: 0x00116302
	public void SetIgnoreConsecutiveDamages(bool _b)
	{
		this.bIgnoreConsecutiveDamages = _b;
	}

	// Token: 0x06002C36 RID: 11318 RVA: 0x0011810B File Offset: 0x0011630B
	public virtual bool IsIgnoreConsecutiveDamages()
	{
		return this.bIgnoreConsecutiveDamages;
	}

	// Token: 0x170004AA RID: 1194
	// (get) Token: 0x06002C37 RID: 11319 RVA: 0x00118113 File Offset: 0x00116313
	// (set) Token: 0x06002C38 RID: 11320 RVA: 0x0011811B File Offset: 0x0011631B
	public float DamageMultiplier
	{
		get
		{
			return this.damageMultiplier;
		}
		set
		{
			this.damageMultiplier = value;
		}
	}

	// Token: 0x040021D3 RID: 8659
	public static readonly DamageSource eat = new DamageSource(EnumDamageSource.External, EnumDamageTypes.Slashing);

	// Token: 0x040021D4 RID: 8660
	public static readonly DamageSource fallingBlock = new DamageSource(EnumDamageSource.External, EnumDamageTypes.Crushing);

	// Token: 0x040021D5 RID: 8661
	public static readonly DamageSource radiation = new DamageSource(EnumDamageSource.External, EnumDamageTypes.Radiation);

	// Token: 0x040021D6 RID: 8662
	public static readonly DamageSource fall = new DamageSource(EnumDamageSource.Internal, EnumDamageTypes.Falling);

	// Token: 0x040021D7 RID: 8663
	public static readonly DamageSource starve = new DamageSource(EnumDamageSource.Internal, EnumDamageTypes.Starvation);

	// Token: 0x040021D8 RID: 8664
	public static readonly DamageSource dehydrate = new DamageSource(EnumDamageSource.Internal, EnumDamageTypes.Dehydration);

	// Token: 0x040021D9 RID: 8665
	public static readonly DamageSource radiationSickness = new DamageSource(EnumDamageSource.Internal, EnumDamageTypes.Radiation);

	// Token: 0x040021DA RID: 8666
	public static readonly DamageSource disease = new DamageSource(EnumDamageSource.Internal, EnumDamageTypes.Disease);

	// Token: 0x040021DB RID: 8667
	public static readonly DamageSource suffocating = new DamageSource(EnumDamageSource.Internal, EnumDamageTypes.Suffocation);

	// Token: 0x040021DC RID: 8668
	public BuffClass BuffClass;

	// Token: 0x040021DD RID: 8669
	public ItemValue AttackingItem;

	// Token: 0x040021DE RID: 8670
	public EnumDamageSource damageSource;

	// Token: 0x040021DF RID: 8671
	public readonly EnumDamageTypes damageType;

	// Token: 0x040021E0 RID: 8672
	public EnumBodyPartHit bodyParts;

	// Token: 0x040021E1 RID: 8673
	public float DismemberChance;

	// Token: 0x040021E2 RID: 8674
	public EnumDamageBonusType BonusDamageType;

	// Token: 0x040021E3 RID: 8675
	public bool canHitSpecialBodyParts;

	// Token: 0x040021E4 RID: 8676
	public bool bIgnorePartyShare;

	// Token: 0x040021E5 RID: 8677
	public bool bTrapKillXP;

	// Token: 0x040021E6 RID: 8678
	public float KillXPScale = 1f;

	// Token: 0x040021E7 RID: 8679
	public FastTags<TagGroup.Global> DamageTypeTag;

	// Token: 0x040021E8 RID: 8680
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bIgnoreConsecutiveDamages;

	// Token: 0x040021E9 RID: 8681
	[PublicizedFrom(EAccessModifier.Private)]
	public float damageMultiplier = 1f;

	// Token: 0x040021EA RID: 8682
	[PublicizedFrom(EAccessModifier.Protected)]
	public int ownerEntityId = -1;

	// Token: 0x040021EB RID: 8683
	public int CreatorEntityId = -1;

	// Token: 0x040021EC RID: 8684
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 direction;

	// Token: 0x040021ED RID: 8685
	public Vector3i BlockPosition;
}
