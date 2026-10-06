using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020007C5 RID: 1989
[Preserve]
public class NetPackageDamageEntity : NetPackage
{
	// Token: 0x06003B1D RID: 15133 RVA: 0x0017D770 File Offset: 0x0017B970
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

	// Token: 0x06003B1E RID: 15134 RVA: 0x0017D9E0 File Offset: 0x0017BBE0
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

	// Token: 0x06003B1F RID: 15135 RVA: 0x0017DB84 File Offset: 0x0017BD84
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

	// Token: 0x06003B20 RID: 15136 RVA: 0x0017DD3C File Offset: 0x0017BF3C
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

	// Token: 0x04003009 RID: 12297
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;

	// Token: 0x0400300A RID: 12298
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cFlagsCanHitSpecialBodyParts = 1;

	// Token: 0x0400300B RID: 12299
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cFlagsCrippleLegs = 2;

	// Token: 0x0400300C RID: 12300
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cFlagsCritical = 4;

	// Token: 0x0400300D RID: 12301
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cFlagsDismember = 8;

	// Token: 0x0400300E RID: 12302
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cFlagsFatal = 16;

	// Token: 0x0400300F RID: 12303
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cFlagsFromBuff = 32;

	// Token: 0x04003010 RID: 12304
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cFlagsIgnoreConsecutiveDamages = 64;

	// Token: 0x04003011 RID: 12305
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cFlagsIgnorePartyShare = 128;

	// Token: 0x04003012 RID: 12306
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cFlagsPainHit = 256;

	// Token: 0x04003013 RID: 12307
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cFlagsTurnIntoCrawler = 512;

	// Token: 0x04003014 RID: 12308
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cFlagsTrapKillXP = 1024;

	// Token: 0x04003015 RID: 12309
	[PublicizedFrom(EAccessModifier.Private)]
	public uint flags;

	// Token: 0x04003016 RID: 12310
	[PublicizedFrom(EAccessModifier.Private)]
	public int attackerEntityId;

	// Token: 0x04003017 RID: 12311
	[PublicizedFrom(EAccessModifier.Private)]
	public EnumDamageSource damageSrc;

	// Token: 0x04003018 RID: 12312
	[PublicizedFrom(EAccessModifier.Private)]
	public EnumDamageTypes damageTyp;

	// Token: 0x04003019 RID: 12313
	[PublicizedFrom(EAccessModifier.Private)]
	public ushort strength;

	// Token: 0x0400301A RID: 12314
	[PublicizedFrom(EAccessModifier.Private)]
	public int hitDirection;

	// Token: 0x0400301B RID: 12315
	[PublicizedFrom(EAccessModifier.Private)]
	public short hitBodyPart;

	// Token: 0x0400301C RID: 12316
	[PublicizedFrom(EAccessModifier.Private)]
	public int movementState;

	// Token: 0x0400301D RID: 12317
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 dirV;

	// Token: 0x0400301E RID: 12318
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i blockPos;

	// Token: 0x0400301F RID: 12319
	[PublicizedFrom(EAccessModifier.Private)]
	public string hitTransformName;

	// Token: 0x04003020 RID: 12320
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 hitTransformPosition;

	// Token: 0x04003021 RID: 12321
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2 uvHit;

	// Token: 0x04003022 RID: 12322
	[PublicizedFrom(EAccessModifier.Private)]
	public float damageMultiplier;

	// Token: 0x04003023 RID: 12323
	[PublicizedFrom(EAccessModifier.Private)]
	public float random;

	// Token: 0x04003024 RID: 12324
	[PublicizedFrom(EAccessModifier.Private)]
	public byte bonusDamageType;

	// Token: 0x04003025 RID: 12325
	[PublicizedFrom(EAccessModifier.Private)]
	public byte StunType;

	// Token: 0x04003026 RID: 12326
	[PublicizedFrom(EAccessModifier.Private)]
	public float StunDuration;

	// Token: 0x04003027 RID: 12327
	[PublicizedFrom(EAccessModifier.Private)]
	public EquipmentSlots ArmorSlot;

	// Token: 0x04003028 RID: 12328
	[PublicizedFrom(EAccessModifier.Private)]
	public EquipmentSlotGroups ArmorSlotGroup;

	// Token: 0x04003029 RID: 12329
	[PublicizedFrom(EAccessModifier.Private)]
	public int ArmorDamage;

	// Token: 0x0400302A RID: 12330
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue attackingItem;

	// Token: 0x0400302B RID: 12331
	[PublicizedFrom(EAccessModifier.Private)]
	public float KillXPScale;
}
