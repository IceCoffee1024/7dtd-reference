using System;
using UnityEngine;

// Token: 0x0200054B RID: 1355
public class DamageSource
{
	// Token: 0x170004A8 RID: 1192
	// (get) Token: 0x06002C2B RID: 11307 RVA: 0x00117F98 File Offset: 0x00116198
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

	// Token: 0x06002C2C RID: 11308 RVA: 0x00117FB0 File Offset: 0x001161B0
	public DamageSource(EnumDamageSource _dsn, EnumDamageTypes _damageType)
	{
		this.damageSource = _dsn;
		this.damageType = _damageType;
		this.DamageTypeTag = FastTags<TagGroup.Global>.Parse(_damageType.ToStringCached<EnumDamageTypes>());
	}

	// Token: 0x06002C2D RID: 11309 RVA: 0x00118008 File Offset: 0x00116208
	public DamageSource(EnumDamageSource _dsn, EnumDamageTypes _damageType, Vector3 _direction)
	{
		this.damageSource = _dsn;
		this.damageType = _damageType;
		this.direction = _direction;
		this.DamageTypeTag = FastTags<TagGroup.Global>.Parse(_damageType.ToStringCached<EnumDamageTypes>());
	}

	// Token: 0x06002C2E RID: 11310 RVA: 0x00118065 File Offset: 0x00116265
	public bool AffectedByArmor()
	{
		return this.damageSource == EnumDamageSource.External;
	}

	// Token: 0x06002C2F RID: 11311 RVA: 0x00118070 File Offset: 0x00116270
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

	// Token: 0x06002C30 RID: 11312 RVA: 0x00118104 File Offset: 0x00116304
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

	// Token: 0x06002C31 RID: 11313 RVA: 0x00118194 File Offset: 0x00116394
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

	// Token: 0x06002C32 RID: 11314 RVA: 0x001181E0 File Offset: 0x001163E0
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

	// Token: 0x06002C33 RID: 11315 RVA: 0x00118374 File Offset: 0x00116574
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

	// Token: 0x06002C34 RID: 11316 RVA: 0x001184AC File Offset: 0x001166AC
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

	// Token: 0x06002C35 RID: 11317 RVA: 0x0011863C File Offset: 0x0011683C
	public virtual Vector3 getDirection()
	{
		return this.direction;
	}

	// Token: 0x06002C36 RID: 11318 RVA: 0x00118644 File Offset: 0x00116844
	public virtual int getEntityId()
	{
		return this.ownerEntityId;
	}

	// Token: 0x06002C37 RID: 11319 RVA: 0x000200DA File Offset: 0x0001E2DA
	public virtual string getHitTransformName()
	{
		return null;
	}

	// Token: 0x06002C38 RID: 11320 RVA: 0x0004E452 File Offset: 0x0004C652
	public virtual Vector3 getHitTransformPosition()
	{
		return Vector3.zero;
	}

	// Token: 0x06002C39 RID: 11321 RVA: 0x0004E4BE File Offset: 0x0004C6BE
	public virtual Vector2 getUVHit()
	{
		return Vector2.zero;
	}

	// Token: 0x06002C3A RID: 11322 RVA: 0x0011864C File Offset: 0x0011684C
	public virtual EnumDamageSource GetSource()
	{
		return this.damageSource;
	}

	// Token: 0x06002C3B RID: 11323 RVA: 0x00118654 File Offset: 0x00116854
	public virtual EnumDamageTypes GetDamageType()
	{
		return this.damageType;
	}

	// Token: 0x170004A9 RID: 1193
	// (get) Token: 0x06002C3C RID: 11324 RVA: 0x0011865C File Offset: 0x0011685C
	public bool CanStun
	{
		get
		{
			return this.damageType == EnumDamageTypes.Bashing || this.damageType == EnumDamageTypes.Heat || this.damageType == EnumDamageTypes.Piercing || this.damageType == EnumDamageTypes.Crushing || this.damageType == EnumDamageTypes.Falling;
		}
	}

	// Token: 0x06002C3D RID: 11325 RVA: 0x0011868E File Offset: 0x0011688E
	public void SetIgnoreConsecutiveDamages(bool _b)
	{
		this.bIgnoreConsecutiveDamages = _b;
	}

	// Token: 0x06002C3E RID: 11326 RVA: 0x00118697 File Offset: 0x00116897
	public virtual bool IsIgnoreConsecutiveDamages()
	{
		return this.bIgnoreConsecutiveDamages;
	}

	// Token: 0x170004AA RID: 1194
	// (get) Token: 0x06002C3F RID: 11327 RVA: 0x0011869F File Offset: 0x0011689F
	// (set) Token: 0x06002C40 RID: 11328 RVA: 0x001186A7 File Offset: 0x001168A7
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

	// Token: 0x040021EA RID: 8682
	public static readonly DamageSource eat = new DamageSource(EnumDamageSource.External, EnumDamageTypes.Slashing);

	// Token: 0x040021EB RID: 8683
	public static readonly DamageSource fallingBlock = new DamageSource(EnumDamageSource.External, EnumDamageTypes.Crushing);

	// Token: 0x040021EC RID: 8684
	public static readonly DamageSource radiation = new DamageSource(EnumDamageSource.External, EnumDamageTypes.Radiation);

	// Token: 0x040021ED RID: 8685
	public static readonly DamageSource fall = new DamageSource(EnumDamageSource.Internal, EnumDamageTypes.Falling);

	// Token: 0x040021EE RID: 8686
	public static readonly DamageSource starve = new DamageSource(EnumDamageSource.Internal, EnumDamageTypes.Starvation);

	// Token: 0x040021EF RID: 8687
	public static readonly DamageSource dehydrate = new DamageSource(EnumDamageSource.Internal, EnumDamageTypes.Dehydration);

	// Token: 0x040021F0 RID: 8688
	public static readonly DamageSource radiationSickness = new DamageSource(EnumDamageSource.Internal, EnumDamageTypes.Radiation);

	// Token: 0x040021F1 RID: 8689
	public static readonly DamageSource disease = new DamageSource(EnumDamageSource.Internal, EnumDamageTypes.Disease);

	// Token: 0x040021F2 RID: 8690
	public static readonly DamageSource suffocating = new DamageSource(EnumDamageSource.Internal, EnumDamageTypes.Suffocation);

	// Token: 0x040021F3 RID: 8691
	public BuffClass BuffClass;

	// Token: 0x040021F4 RID: 8692
	public ItemValue AttackingItem;

	// Token: 0x040021F5 RID: 8693
	public EnumDamageSource damageSource;

	// Token: 0x040021F6 RID: 8694
	public readonly EnumDamageTypes damageType;

	// Token: 0x040021F7 RID: 8695
	public EnumBodyPartHit bodyParts;

	// Token: 0x040021F8 RID: 8696
	public float DismemberChance;

	// Token: 0x040021F9 RID: 8697
	public EnumDamageBonusType BonusDamageType;

	// Token: 0x040021FA RID: 8698
	public bool canHitSpecialBodyParts;

	// Token: 0x040021FB RID: 8699
	public bool bIgnorePartyShare;

	// Token: 0x040021FC RID: 8700
	public bool bTrapKillXP;

	// Token: 0x040021FD RID: 8701
	public float KillXPScale = 1f;

	// Token: 0x040021FE RID: 8702
	public FastTags<TagGroup.Global> DamageTypeTag;

	// Token: 0x040021FF RID: 8703
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bIgnoreConsecutiveDamages;

	// Token: 0x04002200 RID: 8704
	[PublicizedFrom(EAccessModifier.Private)]
	public float damageMultiplier = 1f;

	// Token: 0x04002201 RID: 8705
	[PublicizedFrom(EAccessModifier.Protected)]
	public int ownerEntityId = -1;

	// Token: 0x04002202 RID: 8706
	public int CreatorEntityId = -1;

	// Token: 0x04002203 RID: 8707
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 direction;

	// Token: 0x04002204 RID: 8708
	public Vector3i BlockPosition;
}
