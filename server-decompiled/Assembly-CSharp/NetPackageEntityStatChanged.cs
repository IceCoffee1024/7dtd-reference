using System;
using UnityEngine.Scripting;

// Token: 0x020001D0 RID: 464
[Preserve]
public class NetPackageEntityStatChanged : NetPackageEntityTargeted
{
	// Token: 0x06000E70 RID: 3696 RVA: 0x0005F2CC File Offset: 0x0005D4CC
	public NetPackageEntityStatChanged Setup(EntityAlive entity, int instigatorId, NetPackageEntityStatChanged.EnumStat Estat)
	{
		base.Setup(entity.entityId);
		this.m_instigatorId = instigatorId;
		this.m_enumStat = Estat;
		Stat stat = NetPackageEntityStatChanged.GetStat(entity, Estat);
		this.m_value = stat.Value;
		this.m_max = stat.BaseMax;
		this.m_maxModifier = stat.MaxModifier;
		return this;
	}

	// Token: 0x06000E71 RID: 3697 RVA: 0x0005F320 File Offset: 0x0005D520
	[PublicizedFrom(EAccessModifier.Private)]
	public static Stat GetStat(EntityAlive entity, NetPackageEntityStatChanged.EnumStat stat)
	{
		if (stat <= NetPackageEntityStatChanged.EnumStat.Stamina)
		{
			if (stat == NetPackageEntityStatChanged.EnumStat.Health)
			{
				return entity.Stats.Health;
			}
			if (stat == NetPackageEntityStatChanged.EnumStat.Stamina)
			{
				return entity.Stats.Stamina;
			}
		}
		else
		{
			if (stat == NetPackageEntityStatChanged.EnumStat.Food)
			{
				return entity.Stats.Food;
			}
			if (stat == NetPackageEntityStatChanged.EnumStat.Water)
			{
				return entity.Stats.Water;
			}
		}
		return entity.Stats.Health;
	}

	// Token: 0x06000E72 RID: 3698 RVA: 0x0005F380 File Offset: 0x0005D580
	public override void read(PooledBinaryReader _reader)
	{
		base.read(_reader);
		this.m_instigatorId = _reader.ReadInt32();
		this.m_enumStat = (NetPackageEntityStatChanged.EnumStat)_reader.ReadByte();
		this.m_value = _reader.ReadSingle();
		this.m_max = _reader.ReadSingle();
		this.m_maxModifier = _reader.ReadSingle();
	}

	// Token: 0x06000E73 RID: 3699 RVA: 0x0005F3D0 File Offset: 0x0005D5D0
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.m_instigatorId);
		_writer.Write((byte)this.m_enumStat);
		_writer.Write(this.m_value);
		_writer.Write(this.m_max);
		_writer.Write(this.m_maxModifier);
	}

	// Token: 0x06000E74 RID: 3700 RVA: 0x0005F424 File Offset: 0x0005D624
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		if (this.entityId == _world.GetPrimaryPlayerId() && this.entityId == this.m_instigatorId)
		{
			return;
		}
		if (!base.ValidEntityIdForSender(this.m_instigatorId, false))
		{
			return;
		}
		EntityAlive entityAlive = _world.GetEntity(this.entityId) as EntityAlive;
		if (entityAlive)
		{
			Stat stat = NetPackageEntityStatChanged.GetStat(entityAlive, this.m_enumStat);
			stat.BaseMax = this.m_max;
			stat.MaxModifier = this.m_maxModifier;
			stat.Value = this.m_value;
			stat.Changed = false;
			if (!entityAlive.isEntityRemote && this.m_enumStat == NetPackageEntityStatChanged.EnumStat.Health)
			{
				entityAlive.MinEventContext.Other = _world.GetEntity(this.m_instigatorId) as EntityAlive;
				entityAlive.FireEvent(MinEventTypes.onOtherHealedSelf, true);
			}
			if (!_world.IsRemote())
			{
				_world.entityDistributer.SendPacketToTrackedPlayersAndTrackedEntity(entityAlive.entityId, this.m_instigatorId, NetPackageManager.GetPackage<NetPackageEntityStatChanged>().Setup(entityAlive, this.m_instigatorId, this.m_enumStat), this.m_enumStat > NetPackageEntityStatChanged.EnumStat.Health);
			}
		}
	}

	// Token: 0x06000E75 RID: 3701 RVA: 0x0005F529 File Offset: 0x0005D729
	public override int GetLength()
	{
		return 21;
	}

	// Token: 0x04000C47 RID: 3143
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_instigatorId;

	// Token: 0x04000C48 RID: 3144
	[PublicizedFrom(EAccessModifier.Private)]
	public NetPackageEntityStatChanged.EnumStat m_enumStat;

	// Token: 0x04000C49 RID: 3145
	[PublicizedFrom(EAccessModifier.Private)]
	public float m_value;

	// Token: 0x04000C4A RID: 3146
	[PublicizedFrom(EAccessModifier.Private)]
	public float m_max;

	// Token: 0x04000C4B RID: 3147
	[PublicizedFrom(EAccessModifier.Private)]
	public float m_maxModifier;

	// Token: 0x020001D1 RID: 465
	public enum EnumStat
	{
		// Token: 0x04000C4D RID: 3149
		Health,
		// Token: 0x04000C4E RID: 3150
		Stamina,
		// Token: 0x04000C4F RID: 3151
		Sickness,
		// Token: 0x04000C50 RID: 3152
		Gassiness,
		// Token: 0x04000C51 RID: 3153
		SpeedModifier,
		// Token: 0x04000C52 RID: 3154
		Wellness,
		// Token: 0x04000C53 RID: 3155
		CoreTempOLD,
		// Token: 0x04000C54 RID: 3156
		Food,
		// Token: 0x04000C55 RID: 3157
		Water
	}
}
