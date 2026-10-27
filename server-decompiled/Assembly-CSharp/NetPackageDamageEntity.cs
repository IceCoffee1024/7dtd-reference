using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020007CD RID: 1997
[Preserve]
public class NetPackageDamageEntity : NetPackage
{
	// Token: 0x06003B34 RID: 15156 RVA: 0x0017E708 File Offset: 0x0017C908
	public NetPackageDamageEntity Setup(int _targetEntityId, DamageResponse _dmResponse)
	{
		this.entityId = _targetEntityId;
		DamageSource source = _dmResponse.Source;
		this.flags = 0U;
		this.flags |= (source.canHitSpecialBodyParts ? 1U : 0U);
		this.flags |= (_dmResponse.CrippleLegs ? 2U : 0U);
		this.flags |= (_dmResponse.Critical ? 4U : 0U);
		this.flags |= (_dmResponse.Dismember ? 8U : 0U);
		this.flags |= (_dmResponse.Fatal ? 16U : 0U);
		this.flags |= ((source.BuffClass != null) ? 32U : 0U);
		this.flags |= (source.IsIgnoreConsecutiveDamages() ? 64U : 0U);
		this.flags |= (source.bIgnorePartyShare ? 128U : 0U);
		this.flags |= (_dmResponse.PainHit ? 256U : 0U);
		this.flags |= (_dmResponse.TurnIntoCrawler ? 512U : 0U);
		this.flags |= (source.bTrapKillXP ? 1024U : 0U);
		this.damageSrc = source.GetSource();
		this.damageTyp = source.GetDamageType();
		this.attackingItem = source.AttackingItem;
		int num = _dmResponse.Strength;
		if (num > 65535)
		{
			num = 65535;
		}
		this.strength = (ushort)num;
		this.hitDirection = (int)_dmResponse.HitDirection;
		this.hitBodyPart = (short)_dmResponse.HitBodyPart;
		this.movementState = _dmResponse.MovementState;
		this.attackerEntityId = source.getEntityId();
		this.dirV = source.getDirection();
		this.blockPos = source.BlockPosition;
		this.hitTransformName = source.getHitTransformName() ?? string.Empty;
		this.hitTransformPosition = source.getHitTransformPosition();
		this.uvHit = source.getUVHit();
		this.KillXPScale = source.KillXPScale;
		this.damageMultiplier = source.DamageMultiplier;
		this.bonusDamageType = (byte)source.BonusDamageType;
		this.random = _dmResponse.Random;
		this.StunType = (byte)_dmResponse.Stun;
		this.StunDuration = _dmResponse.StunDuration;
		this.ArmorSlot = _dmResponse.ArmorSlot;
		this.ArmorSlotGroup = _dmResponse.ArmorSlotGroup;
		this.ArmorDamage = _dmResponse.ArmorDamage;
		return this;
	}

	// Token: 0x06003B35 RID: 15157 RVA: 0x0017E978 File Offset: 0x0017CB78
	public override void read(PooledBinaryReader _reader)
	{
		this.entityId = _reader.ReadInt32();
		this.flags = _reader.ReadUInt32();
		this.damageSrc = (EnumDamageSource)_reader.ReadByte();
		this.damageTyp = (EnumDamageTypes)_reader.ReadByte();
		this.strength = _reader.ReadUInt16();
		this.hitDirection = (int)_reader.ReadByte();
		this.hitBodyPart = _reader.ReadInt16();
		this.movementState = (int)_reader.ReadByte();
		this.attackerEntityId = _reader.ReadInt32();
		this.dirV.x = _reader.ReadSingle();
		this.dirV.y = _reader.ReadSingle();
		this.dirV.z = _reader.ReadSingle();
		this.blockPos = StreamUtils.ReadVector3i(_reader);
		this.hitTransformName = _reader.ReadString();
		this.hitTransformPosition.x = _reader.ReadSingle();
		this.hitTransformPosition.y = _reader.ReadSingle();
		this.hitTransformPosition.z = _reader.ReadSingle();
		this.uvHit.x = _reader.ReadSingle();
		this.uvHit.y = _reader.ReadSingle();
		this.KillXPScale = _reader.ReadSingle();
		this.damageMultiplier = _reader.ReadSingle();
		this.random = _reader.ReadSingle();
		this.bonusDamageType = _reader.ReadByte();
		this.StunType = _reader.ReadByte();
		this.StunDuration = _reader.ReadSingle();
		this.ArmorSlot = (EquipmentSlots)_reader.ReadByte();
		this.ArmorSlotGroup = (EquipmentSlotGroups)_reader.ReadByte();
		this.ArmorDamage = (int)_reader.ReadInt16();
		if (_reader.ReadBoolean())
		{
			this.attackingItem = new ItemValue();
			this.attackingItem.Read(_reader);
		}
	}

	// Token: 0x06003B36 RID: 15158 RVA: 0x0017EB1C File Offset: 0x0017CD1C
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.entityId);
		_writer.Write(this.flags);
		_writer.Write((byte)this.damageSrc);
		_writer.Write((byte)this.damageTyp);
		_writer.Write(this.strength);
		_writer.Write((byte)this.hitDirection);
		_writer.Write(this.hitBodyPart);
		_writer.Write((byte)this.movementState);
		_writer.Write(this.attackerEntityId);
		_writer.Write(this.dirV.x);
		_writer.Write(this.dirV.y);
		_writer.Write(this.dirV.z);
		StreamUtils.Write(_writer, this.blockPos);
		_writer.Write(this.hitTransformName);
		_writer.Write(this.hitTransformPosition.x);
		_writer.Write(this.hitTransformPosition.y);
		_writer.Write(this.hitTransformPosition.z);
		_writer.Write(this.uvHit.x);
		_writer.Write(this.uvHit.y);
		_writer.Write(this.KillXPScale);
		_writer.Write(this.damageMultiplier);
		_writer.Write(this.random);
		_writer.Write(this.bonusDamageType);
		_writer.Write(this.StunType);
		_writer.Write(this.StunDuration);
		_writer.Write((byte)this.ArmorSlot);
		_writer.Write((byte)this.ArmorSlotGroup);
		_writer.Write((ushort)this.ArmorDamage);
		_writer.Write(this.attackingItem != null);
		if (this.attackingItem != null)
		{
			this.attackingItem.Write(_writer);
		}
	}

	// Token: 0x06003B37 RID: 15159 RVA: 0x0017ECD4 File Offset: 0x0017CED4
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		EntityPlayerLocal primaryPlayer = _world.GetPrimaryPlayer();
		if (primaryPlayer && primaryPlayer.entityId == this.entityId)
		{
			if (this.damageTyp == EnumDamageTypes.Falling)
			{
				return;
			}
			if (this.damageSrc == EnumDamageSource.External && (this.damageTyp == EnumDamageTypes.Piercing || this.damageTyp == EnumDamageTypes.BarbedWire) && this.attackerEntityId == -1)
			{
				return;
			}
		}
		Entity entity = _world.GetEntity(this.entityId);
		if (entity)
		{
			DamageSourceEntity damageSourceEntity = new DamageSourceEntity(this.damageSrc, this.damageTyp, this.attackerEntityId, this.dirV, this.hitTransformName, this.hitTransformPosition, this.uvHit);
			damageSourceEntity.canHitSpecialBodyParts = (this.flags & 1U) > 0U;
			damageSourceEntity.bIgnorePartyShare = (this.flags & 128U) > 0U;
			damageSourceEntity.bTrapKillXP = (this.flags & 1024U) > 0U;
			damageSourceEntity.KillXPScale = this.KillXPScale;
			damageSourceEntity.SetIgnoreConsecutiveDamages((this.flags & 64U) > 0U);
			damageSourceEntity.DamageMultiplier = this.damageMultiplier;
			damageSourceEntity.BonusDamageType = (EnumDamageBonusType)this.bonusDamageType;
			damageSourceEntity.AttackingItem = this.attackingItem;
			damageSourceEntity.BlockPosition = this.blockPos;
			DamageResponse damageResponse = default(DamageResponse);
			damageResponse.Strength = (int)this.strength;
			damageResponse.ModStrength = 0;
			damageResponse.MovementState = this.movementState;
			damageResponse.HitDirection = (Utils.EnumHitDirection)this.hitDirection;
			damageResponse.HitBodyPart = (EnumBodyPartHit)this.hitBodyPart;
			damageResponse.PainHit = (this.flags & 256U) > 0U;
			damageResponse.Fatal = (this.flags & 16U) > 0U;
			damageResponse.Critical = (this.flags & 4U) > 0U;
			damageResponse.Random = this.random;
			damageResponse.Source = damageSourceEntity;
			damageResponse.CrippleLegs = (this.flags & 2U) > 0U;
			damageResponse.Dismember = (this.flags & 8U) > 0U;
			damageResponse.TurnIntoCrawler = (this.flags & 512U) > 0U;
			damageResponse.Stun = (EnumEntityStunType)this.StunType;
			damageResponse.StunDuration = this.StunDuration;
			damageResponse.ArmorSlot = this.ArmorSlot;
			damageResponse.ArmorSlotGroup = this.ArmorSlotGroup;
			damageResponse.ArmorDamage = this.ArmorDamage;
			if ((this.flags & 32U) > 0U)
			{
				damageResponse.Source.BuffClass = new BuffClass("");
			}
			entity.FireAttackedEvents(damageResponse);
			entity.ProcessDamageResponse(damageResponse);
		}
	}

	// Token: 0x04003025 RID: 12325
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;

	// Token: 0x04003026 RID: 12326
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cFlagsCanHitSpecialBodyParts = 1;

	// Token: 0x04003027 RID: 12327
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cFlagsCrippleLegs = 2;

	// Token: 0x04003028 RID: 12328
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cFlagsCritical = 4;

	// Token: 0x04003029 RID: 12329
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cFlagsDismember = 8;

	// Token: 0x0400302A RID: 12330
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cFlagsFatal = 16;

	// Token: 0x0400302B RID: 12331
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cFlagsFromBuff = 32;

	// Token: 0x0400302C RID: 12332
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cFlagsIgnoreConsecutiveDamages = 64;

	// Token: 0x0400302D RID: 12333
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cFlagsIgnorePartyShare = 128;

	// Token: 0x0400302E RID: 12334
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cFlagsPainHit = 256;

	// Token: 0x0400302F RID: 12335
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cFlagsTurnIntoCrawler = 512;

	// Token: 0x04003030 RID: 12336
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cFlagsTrapKillXP = 1024;

	// Token: 0x04003031 RID: 12337
	[PublicizedFrom(EAccessModifier.Private)]
	public uint flags;

	// Token: 0x04003032 RID: 12338
	[PublicizedFrom(EAccessModifier.Private)]
	public int attackerEntityId;

	// Token: 0x04003033 RID: 12339
	[PublicizedFrom(EAccessModifier.Private)]
	public EnumDamageSource damageSrc;

	// Token: 0x04003034 RID: 12340
	[PublicizedFrom(EAccessModifier.Private)]
	public EnumDamageTypes damageTyp;

	// Token: 0x04003035 RID: 12341
	[PublicizedFrom(EAccessModifier.Private)]
	public ushort strength;

	// Token: 0x04003036 RID: 12342
	[PublicizedFrom(EAccessModifier.Private)]
	public int hitDirection;

	// Token: 0x04003037 RID: 12343
	[PublicizedFrom(EAccessModifier.Private)]
	public short hitBodyPart;

	// Token: 0x04003038 RID: 12344
	[PublicizedFrom(EAccessModifier.Private)]
	public int movementState;

	// Token: 0x04003039 RID: 12345
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 dirV;

	// Token: 0x0400303A RID: 12346
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i blockPos;

	// Token: 0x0400303B RID: 12347
	[PublicizedFrom(EAccessModifier.Private)]
	public string hitTransformName;

	// Token: 0x0400303C RID: 12348
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 hitTransformPosition;

	// Token: 0x0400303D RID: 12349
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2 uvHit;

	// Token: 0x0400303E RID: 12350
	[PublicizedFrom(EAccessModifier.Private)]
	public float damageMultiplier;

	// Token: 0x0400303F RID: 12351
	[PublicizedFrom(EAccessModifier.Private)]
	public float random;

	// Token: 0x04003040 RID: 12352
	[PublicizedFrom(EAccessModifier.Private)]
	public byte bonusDamageType;

	// Token: 0x04003041 RID: 12353
	[PublicizedFrom(EAccessModifier.Private)]
	public byte StunType;

	// Token: 0x04003042 RID: 12354
	[PublicizedFrom(EAccessModifier.Private)]
	public float StunDuration;

	// Token: 0x04003043 RID: 12355
	[PublicizedFrom(EAccessModifier.Private)]
	public EquipmentSlots ArmorSlot;

	// Token: 0x04003044 RID: 12356
	[PublicizedFrom(EAccessModifier.Private)]
	public EquipmentSlotGroups ArmorSlotGroup;

	// Token: 0x04003045 RID: 12357
	[PublicizedFrom(EAccessModifier.Private)]
	public int ArmorDamage;

	// Token: 0x04003046 RID: 12358
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue attackingItem;

	// Token: 0x04003047 RID: 12359
	[PublicizedFrom(EAccessModifier.Private)]
	public float KillXPScale;
}
