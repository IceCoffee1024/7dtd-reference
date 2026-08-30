using System;
using UnityEngine.Scripting;

// Token: 0x020001CE RID: 462
[Preserve]
public class NetPackageEntityStatChanged : NetPackageEntityTargeted
{
	// Token: 0x06000E65 RID: 3685 RVA: 0x0005ED84 File Offset: 0x0005CF84
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

	// Token: 0x06000E66 RID: 3686 RVA: 0x0005EDD8 File Offset: 0x0005CFD8
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

	// Token: 0x06000E67 RID: 3687 RVA: 0x0005EE38 File Offset: 0x0005D038
	public override void read(PooledBinaryReader _reader)
	{
		base.read(_reader);
		this.m_instigatorId = _reader.ReadInt32();
		this.m_enumStat = (NetPackageEntityStatChanged.EnumStat)_reader.ReadByte();
		this.m_value = _reader.ReadSingle();
		this.m_max = _reader.ReadSingle();
		this.m_maxModifier = _reader.ReadSingle();
	}

	// Token: 0x06000E68 RID: 3688 RVA: 0x0005EE88 File Offset: 0x0005D088
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.m_instigatorId);
		_writer.Write((byte)this.m_enumStat);
		_writer.Write(this.m_value);
		_writer.Write(this.m_max);
		_writer.Write(this.m_maxModifier);
	}

	// Token: 0x06000E69 RID: 3689 RVA: 0x0005EEDC File Offset: 0x0005D0DC
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

	// Token: 0x06000E6A RID: 3690 RVA: 0x0005EFE1 File Offset: 0x0005D1E1
	public override int GetLength()
	{
		return 21;
	}

	// Token: 0x04000C3A RID: 3130
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_instigatorId;

	// Token: 0x04000C3B RID: 3131
	[PublicizedFrom(EAccessModifier.Private)]
	public NetPackageEntityStatChanged.EnumStat m_enumStat;

	// Token: 0x04000C3C RID: 3132
	[PublicizedFrom(EAccessModifier.Private)]
	public float m_value;

	// Token: 0x04000C3D RID: 3133
	[PublicizedFrom(EAccessModifier.Private)]
	public float m_max;

	// Token: 0x04000C3E RID: 3134
	[PublicizedFrom(EAccessModifier.Private)]
	public float m_maxModifier;

	// Token: 0x020001CF RID: 463
	public enum EnumStat
	{
		// Token: 0x04000C40 RID: 3136
		Health,
		// Token: 0x04000C41 RID: 3137
		Stamina,
		// Token: 0x04000C42 RID: 3138
		Sickness,
		// Token: 0x04000C43 RID: 3139
		Gassiness,
		// Token: 0x04000C44 RID: 3140
		SpeedModifier,
		// Token: 0x04000C45 RID: 3141
		Wellness,
		// Token: 0x04000C46 RID: 3142
		CoreTempOLD,
		// Token: 0x04000C47 RID: 3143
		Food,
		// Token: 0x04000C48 RID: 3144
		Water
	}
}
