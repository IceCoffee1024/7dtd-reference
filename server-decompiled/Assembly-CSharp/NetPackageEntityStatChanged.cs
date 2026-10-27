using System;
using UnityEngine.Scripting;

// Token: 0x020001D3 RID: 467
[Preserve]
public class NetPackageEntityStatChanged : NetPackageEntityTargeted
{
	// Token: 0x06000E6B RID: 3691 RVA: 0x0005F590 File Offset: 0x0005D790
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

	// Token: 0x06000E6C RID: 3692 RVA: 0x0005F5E4 File Offset: 0x0005D7E4
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

	// Token: 0x06000E6D RID: 3693 RVA: 0x0005F644 File Offset: 0x0005D844
	public override void read(PooledBinaryReader _reader)
	{
		base.read(_reader);
		this.m_instigatorId = _reader.ReadInt32();
		this.m_enumStat = (NetPackageEntityStatChanged.EnumStat)_reader.ReadByte();
		this.m_value = _reader.ReadSingle();
		this.m_max = _reader.ReadSingle();
		this.m_maxModifier = _reader.ReadSingle();
	}

	// Token: 0x06000E6E RID: 3694 RVA: 0x0005F694 File Offset: 0x0005D894
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.m_instigatorId);
		_writer.Write((byte)this.m_enumStat);
		_writer.Write(this.m_value);
		_writer.Write(this.m_max);
		_writer.Write(this.m_maxModifier);
	}

	// Token: 0x06000E6F RID: 3695 RVA: 0x0005F6E8 File Offset: 0x0005D8E8
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

	// Token: 0x04000C42 RID: 3138
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_instigatorId;

	// Token: 0x04000C43 RID: 3139
	[PublicizedFrom(EAccessModifier.Private)]
	public NetPackageEntityStatChanged.EnumStat m_enumStat;

	// Token: 0x04000C44 RID: 3140
	[PublicizedFrom(EAccessModifier.Private)]
	public float m_value;

	// Token: 0x04000C45 RID: 3141
	[PublicizedFrom(EAccessModifier.Private)]
	public float m_max;

	// Token: 0x04000C46 RID: 3142
	[PublicizedFrom(EAccessModifier.Private)]
	public float m_maxModifier;

	// Token: 0x020001D4 RID: 468
	public enum EnumStat
	{
		// Token: 0x04000C48 RID: 3144
		Health,
		// Token: 0x04000C49 RID: 3145
		Stamina,
		// Token: 0x04000C4A RID: 3146
		Sickness,
		// Token: 0x04000C4B RID: 3147
		Gassiness,
		// Token: 0x04000C4C RID: 3148
		SpeedModifier,
		// Token: 0x04000C4D RID: 3149
		Wellness,
		// Token: 0x04000C4E RID: 3150
		CoreTempOLD,
		// Token: 0x04000C4F RID: 3151
		Food,
		// Token: 0x04000C50 RID: 3152
		Water
	}
}
