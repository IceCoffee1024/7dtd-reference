using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020007B7 RID: 1975
[Preserve]
public class NetPackageDamageEntity : NetPackage
{
	// Token: 0x06003A96 RID: 14998 RVA: 0x0017AE20 File Offset: 0x00179020
	public NetPackageDamageEntity Setup(int _targetEntityId, DamageResponse _dmResponse)
	{
		this.entityId = _targetEntityId;
		DamageSource source = _dmResponse.Source;
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
		this.bPainHit = _dmResponse.PainHit;
		this.bFatal = _dmResponse.Fatal;
		this.bCritical = _dmResponse.Critical;
		this.attackerEntityId = source.getEntityId();
		this.dirV = source.getDirection();
		this.blockPos = source.BlockPosition;
		this.hitTransformName = source.getHitTransformName() ?? string.Empty;
		this.hitTransformPosition = source.getHitTransformPosition();
		this.uvHit = source.getUVHit();
		this.damageMultiplier = source.DamageMultiplier;
		this.bonusDamageType = (byte)source.BonusDamageType;
		this.random = _dmResponse.Random;
		this.bIgnoreConsecutiveDamages = source.IsIgnoreConsecutiveDamages();
		this.bDismember = _dmResponse.Dismember;
		this.bCrippleLegs = _dmResponse.CrippleLegs;
		this.bTurnIntoCrawler = _dmResponse.TurnIntoCrawler;
		this.StunType = (byte)_dmResponse.Stun;
		this.StunDuration = _dmResponse.StunDuration;
		this.ArmorSlot = _dmResponse.ArmorSlot;
		this.ArmorSlotGroup = _dmResponse.ArmorSlotGroup;
		this.ArmorDamage = _dmResponse.ArmorDamage;
		this.bFromBuff = source.BuffClass != null;
		return this;
	}

	// Token: 0x06003A97 RID: 14999 RVA: 0x0017AFB8 File Offset: 0x001791B8
	public override void read(PooledBinaryReader _reader)
	{
		this.entityId = _reader.ReadInt32();
		this.damageSrc = (EnumDamageSource)_reader.ReadByte();
		this.damageTyp = (EnumDamageTypes)_reader.ReadByte();
		this.strength = _reader.ReadUInt16();
		this.hitDirection = (int)_reader.ReadByte();
		this.hitBodyPart = _reader.ReadInt16();
		this.movementState = (int)_reader.ReadByte();
		this.bPainHit = _reader.ReadBoolean();
		this.bFatal = _reader.ReadBoolean();
		this.bCritical = _reader.ReadBoolean();
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
		this.damageMultiplier = _reader.ReadSingle();
		this.random = _reader.ReadSingle();
		this.bIgnoreConsecutiveDamages = _reader.ReadBoolean();
		this.bIsDamageTransfer = _reader.ReadBoolean();
		this.bDismember = _reader.ReadBoolean();
		this.bCrippleLegs = _reader.ReadBoolean();
		this.bTurnIntoCrawler = _reader.ReadBoolean();
		this.bonusDamageType = _reader.ReadByte();
		this.StunType = _reader.ReadByte();
		this.StunDuration = _reader.ReadSingle();
		this.bFromBuff = _reader.ReadBoolean();
		this.ArmorSlot = (EquipmentSlots)_reader.ReadByte();
		this.ArmorSlotGroup = (EquipmentSlotGroups)_reader.ReadByte();
		this.ArmorDamage = (int)_reader.ReadInt16();
		if (_reader.ReadBoolean())
		{
			this.attackingItem = new ItemValue();
			this.attackingItem.Read(_reader);
		}
	}

	// Token: 0x06003A98 RID: 15000 RVA: 0x0017B1B0 File Offset: 0x001793B0
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.entityId);
		_writer.Write((byte)this.damageSrc);
		_writer.Write((byte)this.damageTyp);
		_writer.Write(this.strength);
		_writer.Write((byte)this.hitDirection);
		_writer.Write(this.hitBodyPart);
		_writer.Write((byte)this.movementState);
		_writer.Write(this.bPainHit);
		_writer.Write(this.bFatal);
		_writer.Write(this.bCritical);
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
		_writer.Write(this.damageMultiplier);
		_writer.Write(this.random);
		_writer.Write(this.bIgnoreConsecutiveDamages);
		_writer.Write(this.bIsDamageTransfer);
		_writer.Write(this.bDismember);
		_writer.Write(this.bCrippleLegs);
		_writer.Write(this.bTurnIntoCrawler);
		_writer.Write(this.bonusDamageType);
		_writer.Write(this.StunType);
		_writer.Write(this.StunDuration);
		_writer.Write(this.bFromBuff);
		_writer.Write((byte)this.ArmorSlot);
		_writer.Write((byte)this.ArmorSlotGroup);
		_writer.Write((ushort)this.ArmorDamage);
		_writer.Write(this.attackingItem != null);
		if (this.attackingItem != null)
		{
			this.attackingItem.Write(_writer);
		}
	}

	// Token: 0x06003A99 RID: 15001 RVA: 0x0017B3BC File Offset: 0x001795BC
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		if (_world.GetPrimaryPlayer() != null && _world.GetPrimaryPlayer().entityId == this.entityId)
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
		if (entity != null)
		{
			DamageSource damageSource = new DamageSourceEntity(this.damageSrc, this.damageTyp, this.attackerEntityId, this.dirV, this.hitTransformName, this.hitTransformPosition, this.uvHit);
			damageSource.SetIgnoreConsecutiveDamages(this.bIgnoreConsecutiveDamages);
			damageSource.DamageMultiplier = this.damageMultiplier;
			damageSource.BonusDamageType = (EnumDamageBonusType)this.bonusDamageType;
			damageSource.AttackingItem = this.attackingItem;
			damageSource.BlockPosition = this.blockPos;
			DamageResponse damageResponse = default(DamageResponse);
			damageResponse.Strength = (int)this.strength;
			damageResponse.ModStrength = 0;
			damageResponse.MovementState = this.movementState;
			damageResponse.HitDirection = (Utils.EnumHitDirection)this.hitDirection;
			damageResponse.HitBodyPart = (EnumBodyPartHit)this.hitBodyPart;
			damageResponse.PainHit = this.bPainHit;
			damageResponse.Fatal = this.bFatal;
			damageResponse.Critical = this.bCritical;
			damageResponse.Random = this.random;
			damageResponse.Source = damageSource;
			damageResponse.CrippleLegs = this.bCrippleLegs;
			damageResponse.Dismember = this.bDismember;
			damageResponse.TurnIntoCrawler = this.bTurnIntoCrawler;
			damageResponse.Stun = (EnumEntityStunType)this.StunType;
			damageResponse.StunDuration = this.StunDuration;
			damageResponse.ArmorSlot = this.ArmorSlot;
			damageResponse.ArmorSlotGroup = this.ArmorSlotGroup;
			damageResponse.ArmorDamage = this.ArmorDamage;
			if (this.bFromBuff)
			{
				damageResponse.Source.BuffClass = new BuffClass("");
			}
			entity.FireAttackedEvents(damageResponse);
			entity.ProcessDamageResponse(damageResponse);
		}
	}

	// Token: 0x06003A9A RID: 15002 RVA: 0x001757C8 File Offset: 0x001739C8
	public override int GetLength()
	{
		return 50;
	}

	// Token: 0x04002F94 RID: 12180
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;

	// Token: 0x04002F95 RID: 12181
	[PublicizedFrom(EAccessModifier.Private)]
	public int attackerEntityId;

	// Token: 0x04002F96 RID: 12182
	[PublicizedFrom(EAccessModifier.Private)]
	public EnumDamageSource damageSrc;

	// Token: 0x04002F97 RID: 12183
	[PublicizedFrom(EAccessModifier.Private)]
	public EnumDamageTypes damageTyp;

	// Token: 0x04002F98 RID: 12184
	[PublicizedFrom(EAccessModifier.Private)]
	public ushort strength;

	// Token: 0x04002F99 RID: 12185
	[PublicizedFrom(EAccessModifier.Private)]
	public int hitDirection;

	// Token: 0x04002F9A RID: 12186
	[PublicizedFrom(EAccessModifier.Private)]
	public short hitBodyPart;

	// Token: 0x04002F9B RID: 12187
	[PublicizedFrom(EAccessModifier.Private)]
	public int movementState;

	// Token: 0x04002F9C RID: 12188
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 dirV;

	// Token: 0x04002F9D RID: 12189
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i blockPos;

	// Token: 0x04002F9E RID: 12190
	[PublicizedFrom(EAccessModifier.Private)]
	public string hitTransformName;

	// Token: 0x04002F9F RID: 12191
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 hitTransformPosition;

	// Token: 0x04002FA0 RID: 12192
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2 uvHit;

	// Token: 0x04002FA1 RID: 12193
	[PublicizedFrom(EAccessModifier.Private)]
	public float damageMultiplier;

	// Token: 0x04002FA2 RID: 12194
	[PublicizedFrom(EAccessModifier.Private)]
	public float random;

	// Token: 0x04002FA3 RID: 12195
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bIgnoreConsecutiveDamages;

	// Token: 0x04002FA4 RID: 12196
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bPainHit;

	// Token: 0x04002FA5 RID: 12197
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bFatal;

	// Token: 0x04002FA6 RID: 12198
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bCritical;

	// Token: 0x04002FA7 RID: 12199
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bIsDamageTransfer;

	// Token: 0x04002FA8 RID: 12200
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bDismember;

	// Token: 0x04002FA9 RID: 12201
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bCrippleLegs;

	// Token: 0x04002FAA RID: 12202
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bTurnIntoCrawler;

	// Token: 0x04002FAB RID: 12203
	[PublicizedFrom(EAccessModifier.Private)]
	public byte bonusDamageType;

	// Token: 0x04002FAC RID: 12204
	[PublicizedFrom(EAccessModifier.Private)]
	public byte StunType;

	// Token: 0x04002FAD RID: 12205
	[PublicizedFrom(EAccessModifier.Private)]
	public float StunDuration;

	// Token: 0x04002FAE RID: 12206
	[PublicizedFrom(EAccessModifier.Private)]
	public EquipmentSlots ArmorSlot;

	// Token: 0x04002FAF RID: 12207
	[PublicizedFrom(EAccessModifier.Private)]
	public EquipmentSlotGroups ArmorSlotGroup;

	// Token: 0x04002FB0 RID: 12208
	[PublicizedFrom(EAccessModifier.Private)]
	public int ArmorDamage;

	// Token: 0x04002FB1 RID: 12209
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue attackingItem;

	// Token: 0x04002FB2 RID: 12210
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bFromBuff;
}
