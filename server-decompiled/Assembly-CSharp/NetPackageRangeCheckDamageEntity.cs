using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200084B RID: 2123
[Preserve]
public class NetPackageRangeCheckDamageEntity : NetPackage
{
	// Token: 0x17000699 RID: 1689
	// (get) Token: 0x06003DBE RID: 15806 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003DBF RID: 15807 RVA: 0x00189410 File Offset: 0x00187610
	public NetPackageRangeCheckDamageEntity Setup(int _targetEntityId, Vector3 _origin, float _maxRange, DamageSourceEntity _damageSource, int _strength, bool _isCritical, List<string> _buffActions, string _buffActionContext, ParticleEffect particleEffect)
	{
		this.entityId = _targetEntityId;
		this.origin = _origin;
		this.maxRangeSq = _maxRange * _maxRange;
		this.strength = _strength;
		this.damageStr = _damageSource.GetSource();
		this.damageTyp = _damageSource.GetDamageType();
		this.bCritical = _isCritical;
		this.attackerEntityId = _damageSource.getEntityId();
		this.dirX = _damageSource.getDirection().x;
		this.dirY = _damageSource.getDirection().y;
		this.dirZ = _damageSource.getDirection().z;
		this.hitTransformName = ((_damageSource.getHitTransformName() != null) ? _damageSource.getHitTransformName() : string.Empty);
		this.hitTransformPosition = _damageSource.getHitTransformPosition();
		this.uvHitx = _damageSource.getUVHit().x;
		this.uvHity = _damageSource.getUVHit().y;
		this.damageMultiplier = _damageSource.DamageMultiplier;
		this.bonusDamageType = (byte)_damageSource.BonusDamageType;
		this.bIgnoreConsecutiveDamages = _damageSource.IsIgnoreConsecutiveDamages();
		this.buffActions = _buffActions;
		this.buffActionContext = _buffActionContext;
		this.particleName = particleEffect.debugName;
		this.particlePos = particleEffect.pos;
		this.particleRot = particleEffect.rot.eulerAngles;
		this.particleLight = particleEffect.lightValue;
		this.particleColor = particleEffect.color;
		this.particleSound = particleEffect.soundName;
		return this;
	}

	// Token: 0x06003DC0 RID: 15808 RVA: 0x0018957C File Offset: 0x0018777C
	public override void read(PooledBinaryReader _reader)
	{
		this.entityId = _reader.ReadInt32();
		this.damageStr = (EnumDamageSource)_reader.ReadByte();
		this.damageTyp = (EnumDamageTypes)_reader.ReadByte();
		this.origin = new Vector3(_reader.ReadSingle(), _reader.ReadSingle(), _reader.ReadSingle());
		this.maxRangeSq = _reader.ReadSingle();
		this.strength = (int)_reader.ReadInt16();
		this.bCritical = _reader.ReadBoolean();
		this.attackerEntityId = _reader.ReadInt32();
		this.dirX = _reader.ReadSingle();
		this.dirY = _reader.ReadSingle();
		this.dirZ = _reader.ReadSingle();
		this.hitTransformName = _reader.ReadString();
		this.hitTransformPosition = new Vector3(_reader.ReadSingle(), _reader.ReadSingle(), _reader.ReadSingle());
		this.uvHitx = _reader.ReadSingle();
		this.uvHity = _reader.ReadSingle();
		this.damageMultiplier = _reader.ReadSingle();
		this.bIgnoreConsecutiveDamages = _reader.ReadBoolean();
		this.bIsDamageTransfer = _reader.ReadBoolean();
		this.bonusDamageType = _reader.ReadByte();
		this.particleName = _reader.ReadString();
		this.particlePos = new Vector3(_reader.ReadSingle(), _reader.ReadSingle(), _reader.ReadSingle());
		this.particleRot = new Vector3(_reader.ReadSingle(), _reader.ReadSingle(), _reader.ReadSingle());
		this.particleLight = _reader.ReadSingle();
		this.particleColor = new Color(_reader.ReadSingle(), _reader.ReadSingle(), _reader.ReadSingle(), _reader.ReadSingle());
		this.particleSound = _reader.ReadString();
		int num = (int)_reader.ReadByte();
		if (num > 0)
		{
			this.buffActions = new List<string>();
			for (int i = 0; i < num; i++)
			{
				this.buffActions.Add(_reader.ReadString());
			}
			return;
		}
		this.buffActions = null;
	}

	// Token: 0x06003DC1 RID: 15809 RVA: 0x0018974C File Offset: 0x0018794C
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.entityId);
		_writer.Write((byte)this.damageStr);
		_writer.Write((byte)this.damageTyp);
		_writer.Write(this.origin.x);
		_writer.Write(this.origin.y);
		_writer.Write(this.origin.z);
		_writer.Write(this.maxRangeSq);
		_writer.Write((short)this.strength);
		_writer.Write(this.bCritical);
		_writer.Write(this.attackerEntityId);
		_writer.Write(this.dirX);
		_writer.Write(this.dirY);
		_writer.Write(this.dirZ);
		_writer.Write(this.hitTransformName);
		_writer.Write(this.hitTransformPosition.x);
		_writer.Write(this.hitTransformPosition.y);
		_writer.Write(this.hitTransformPosition.z);
		_writer.Write(this.uvHitx);
		_writer.Write(this.uvHity);
		_writer.Write(this.damageMultiplier);
		_writer.Write(this.bIgnoreConsecutiveDamages);
		_writer.Write(this.bIsDamageTransfer);
		_writer.Write(this.bonusDamageType);
		_writer.Write(this.particleName);
		_writer.Write(this.particlePos.x);
		_writer.Write(this.particlePos.y);
		_writer.Write(this.particlePos.z);
		_writer.Write(this.particleRot.x);
		_writer.Write(this.particleRot.y);
		_writer.Write(this.particleRot.z);
		_writer.Write(this.particleLight);
		_writer.Write(this.particleColor.r);
		_writer.Write(this.particleColor.g);
		_writer.Write(this.particleColor.b);
		_writer.Write(this.particleColor.a);
		_writer.Write(this.particleSound);
		if (this.buffActions != null && this.buffActions.Count > 0)
		{
			_writer.Write((byte)this.buffActions.Count);
			for (int i = 0; i < this.buffActions.Count; i++)
			{
				_writer.Write(this.buffActions[i]);
			}
			return;
		}
		_writer.Write(0);
	}

	// Token: 0x06003DC2 RID: 15810 RVA: 0x001899C8 File Offset: 0x00187BC8
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		Entity entity = _world.GetEntity(this.entityId);
		if (entity != null)
		{
			Entity entity2 = _world.GetEntity(this.attackerEntityId);
			bool flag;
			if (entity2 == null)
			{
				flag = true;
			}
			else
			{
				Vector3 vector = entity.GetPosition() - entity2.GetPosition();
				float num = Vector3.Dot((entity2.transform.position - entity.transform.position).normalized, entity2.transform.forward);
				flag = vector.sqrMagnitude <= this.maxRangeSq && num < 0f;
			}
			if (flag)
			{
				DamageSource damageSource = new DamageSourceEntity(this.damageStr, this.damageTyp, this.attackerEntityId, new Vector3(this.dirX, this.dirY, this.dirZ), this.hitTransformName, this.hitTransformPosition, new Vector2(this.uvHitx, this.uvHity));
				damageSource.SetIgnoreConsecutiveDamages(this.bIgnoreConsecutiveDamages);
				damageSource.DamageMultiplier = this.damageMultiplier;
				damageSource.BonusDamageType = (EnumDamageBonusType)this.bonusDamageType;
				entity.DamageEntity(damageSource, this.strength, this.bCritical, 1f);
				if (this.buffActions != null)
				{
					ItemAction.ExecuteBuffActions(this.buffActions, this.attackerEntityId, entity as EntityAlive, this.bCritical, damageSource.GetEntityDamageBodyPart(entity), this.buffActionContext);
				}
				string.IsNullOrEmpty(this.particleName);
				_world.GetGameManager().SpawnParticleEffectServer(new ParticleEffect(this.particleName, this.particlePos, Quaternion.Euler(this.particleRot), this.particleLight, this.particleColor, this.particleSound, null, 1f, ""), _world.GetPrimaryPlayerId(), false, false);
			}
		}
	}

	// Token: 0x04003241 RID: 12865
	public int entityId;

	// Token: 0x04003242 RID: 12866
	public int attackerEntityId;

	// Token: 0x04003243 RID: 12867
	public EnumDamageSource damageStr;

	// Token: 0x04003244 RID: 12868
	public EnumDamageTypes damageTyp;

	// Token: 0x04003245 RID: 12869
	public int strength;

	// Token: 0x04003246 RID: 12870
	public Vector3 origin;

	// Token: 0x04003247 RID: 12871
	public float maxRangeSq;

	// Token: 0x04003248 RID: 12872
	public float dirX;

	// Token: 0x04003249 RID: 12873
	public float dirY;

	// Token: 0x0400324A RID: 12874
	public float dirZ;

	// Token: 0x0400324B RID: 12875
	public string hitTransformName;

	// Token: 0x0400324C RID: 12876
	public Vector3 hitTransformPosition;

	// Token: 0x0400324D RID: 12877
	public float uvHitx;

	// Token: 0x0400324E RID: 12878
	public float uvHity;

	// Token: 0x0400324F RID: 12879
	public float damageMultiplier;

	// Token: 0x04003250 RID: 12880
	public bool bIgnoreConsecutiveDamages;

	// Token: 0x04003251 RID: 12881
	public bool bCritical;

	// Token: 0x04003252 RID: 12882
	public bool bIsDamageTransfer;

	// Token: 0x04003253 RID: 12883
	public byte bonusDamageType;

	// Token: 0x04003254 RID: 12884
	public List<string> buffActions;

	// Token: 0x04003255 RID: 12885
	public string buffActionContext;

	// Token: 0x04003256 RID: 12886
	public string particleName;

	// Token: 0x04003257 RID: 12887
	public Vector3 particlePos;

	// Token: 0x04003258 RID: 12888
	public Vector3 particleRot;

	// Token: 0x04003259 RID: 12889
	public float particleLight;

	// Token: 0x0400325A RID: 12890
	public Color particleColor;

	// Token: 0x0400325B RID: 12891
	public string particleSound;
}
