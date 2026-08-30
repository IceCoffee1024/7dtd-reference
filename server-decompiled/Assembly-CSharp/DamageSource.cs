using System;
using UnityEngine;

// Token: 0x0200053C RID: 1340
public class DamageSource
{
	// Token: 0x170004AE RID: 1198
	// (get) Token: 0x06002C03 RID: 11267 RVA: 0x00115BC4 File Offset: 0x00113DC4
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

	// Token: 0x06002C04 RID: 11268 RVA: 0x00115BDB File Offset: 0x00113DDB
	public DamageSource(EnumDamageSource _dsn, EnumDamageTypes _damageType)
	{
		this.damageSource = _dsn;
		this.damageType = _damageType;
		this.DamageTypeTag = FastTags<TagGroup.Global>.Parse(_damageType.ToStringCached<EnumDamageTypes>());
	}

	// Token: 0x06002C05 RID: 11269 RVA: 0x00115C1C File Offset: 0x00113E1C
	public DamageSource(EnumDamageSource _dsn, EnumDamageTypes _damageType, Vector3 _direction)
	{
		this.damageSource = _dsn;
		this.damageType = _damageType;
		this.direction = _direction;
		this.DamageTypeTag = FastTags<TagGroup.Global>.Parse(_damageType.ToStringCached<EnumDamageTypes>());
	}

	// Token: 0x06002C06 RID: 11270 RVA: 0x00115C6E File Offset: 0x00113E6E
	public bool AffectedByArmor()
	{
		return this.damageSource == EnumDamageSource.External;
	}

	// Token: 0x06002C07 RID: 11271 RVA: 0x00115C7C File Offset: 0x00113E7C
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

	// Token: 0x06002C08 RID: 11272 RVA: 0x00115D10 File Offset: 0x00113F10
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

	// Token: 0x06002C09 RID: 11273 RVA: 0x00115DA0 File Offset: 0x00113FA0
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

	// Token: 0x06002C0A RID: 11274 RVA: 0x00115DEC File Offset: 0x00113FEC
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

	// Token: 0x06002C0B RID: 11275 RVA: 0x00115F80 File Offset: 0x00114180
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

	// Token: 0x06002C0C RID: 11276 RVA: 0x001160B8 File Offset: 0x001142B8
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

	// Token: 0x06002C0D RID: 11277 RVA: 0x00116248 File Offset: 0x00114448
	public virtual Vector3 getDirection()
	{
		return this.direction;
	}

	// Token: 0x06002C0E RID: 11278 RVA: 0x00116250 File Offset: 0x00114450
	public virtual int getEntityId()
	{
		return this.ownerEntityId;
	}

	// Token: 0x06002C0F RID: 11279 RVA: 0x0001FF1E File Offset: 0x0001E11E
	public virtual string getHitTransformName()
	{
		return null;
	}

	// Token: 0x06002C10 RID: 11280 RVA: 0x0004E076 File Offset: 0x0004C276
	public virtual Vector3 getHitTransformPosition()
	{
		return Vector3.zero;
	}

	// Token: 0x06002C11 RID: 11281 RVA: 0x0004E0E2 File Offset: 0x0004C2E2
	public virtual Vector2 getUVHit()
	{
		return Vector2.zero;
	}

	// Token: 0x06002C12 RID: 11282 RVA: 0x00116258 File Offset: 0x00114458
	public virtual EnumDamageSource GetSource()
	{
		return this.damageSource;
	}

	// Token: 0x06002C13 RID: 11283 RVA: 0x00116260 File Offset: 0x00114460
	public virtual EnumDamageTypes GetDamageType()
	{
		return this.damageType;
	}

	// Token: 0x170004AF RID: 1199
	// (get) Token: 0x06002C14 RID: 11284 RVA: 0x00116268 File Offset: 0x00114468
	public bool CanStun
	{
		get
		{
			return this.damageType == EnumDamageTypes.Bashing || this.damageType == EnumDamageTypes.Heat || this.damageType == EnumDamageTypes.Piercing || this.damageType == EnumDamageTypes.Crushing || this.damageType == EnumDamageTypes.Falling;
		}
	}

	// Token: 0x06002C15 RID: 11285 RVA: 0x0011629A File Offset: 0x0011449A
	public void SetIgnoreConsecutiveDamages(bool _b)
	{
		this.bIgnoreConsecutiveDamages = _b;
	}

	// Token: 0x06002C16 RID: 11286 RVA: 0x001162A3 File Offset: 0x001144A3
	public virtual bool IsIgnoreConsecutiveDamages()
	{
		return this.bIgnoreConsecutiveDamages;
	}

	// Token: 0x170004B0 RID: 1200
	// (get) Token: 0x06002C17 RID: 11287 RVA: 0x001162AB File Offset: 0x001144AB
	// (set) Token: 0x06002C18 RID: 11288 RVA: 0x001162B3 File Offset: 0x001144B3
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

	// Token: 0x0400219C RID: 8604
	public static readonly DamageSource eat = new DamageSource(EnumDamageSource.External, EnumDamageTypes.Slashing);

	// Token: 0x0400219D RID: 8605
	public static readonly DamageSource fallingBlock = new DamageSource(EnumDamageSource.External, EnumDamageTypes.Crushing);

	// Token: 0x0400219E RID: 8606
	public static readonly DamageSource radiation = new DamageSource(EnumDamageSource.External, EnumDamageTypes.Radiation);

	// Token: 0x0400219F RID: 8607
	public static readonly DamageSource fall = new DamageSource(EnumDamageSource.Internal, EnumDamageTypes.Falling);

	// Token: 0x040021A0 RID: 8608
	public static readonly DamageSource starve = new DamageSource(EnumDamageSource.Internal, EnumDamageTypes.Starvation);

	// Token: 0x040021A1 RID: 8609
	public static readonly DamageSource dehydrate = new DamageSource(EnumDamageSource.Internal, EnumDamageTypes.Dehydration);

	// Token: 0x040021A2 RID: 8610
	public static readonly DamageSource radiationSickness = new DamageSource(EnumDamageSource.Internal, EnumDamageTypes.Radiation);

	// Token: 0x040021A3 RID: 8611
	public static readonly DamageSource disease = new DamageSource(EnumDamageSource.Internal, EnumDamageTypes.Disease);

	// Token: 0x040021A4 RID: 8612
	public static readonly DamageSource suffocating = new DamageSource(EnumDamageSource.Internal, EnumDamageTypes.Suffocation);

	// Token: 0x040021A5 RID: 8613
	public BuffClass BuffClass;

	// Token: 0x040021A6 RID: 8614
	public ItemValue AttackingItem;

	// Token: 0x040021A7 RID: 8615
	public EnumDamageSource damageSource;

	// Token: 0x040021A8 RID: 8616
	public readonly EnumDamageTypes damageType;

	// Token: 0x040021A9 RID: 8617
	public EnumBodyPartHit bodyParts;

	// Token: 0x040021AA RID: 8618
	public float DismemberChance;

	// Token: 0x040021AB RID: 8619
	public EnumDamageBonusType BonusDamageType;

	// Token: 0x040021AC RID: 8620
	public bool canHitSpecialBodyParts;

	// Token: 0x040021AD RID: 8621
	public FastTags<TagGroup.Global> DamageTypeTag;

	// Token: 0x040021AE RID: 8622
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bIgnoreConsecutiveDamages;

	// Token: 0x040021AF RID: 8623
	[PublicizedFrom(EAccessModifier.Private)]
	public float damageMultiplier = 1f;

	// Token: 0x040021B0 RID: 8624
	[PublicizedFrom(EAccessModifier.Protected)]
	public int ownerEntityId = -1;

	// Token: 0x040021B1 RID: 8625
	public int CreatorEntityId = -1;

	// Token: 0x040021B2 RID: 8626
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 direction;

	// Token: 0x040021B3 RID: 8627
	public Vector3i BlockPosition;
}
