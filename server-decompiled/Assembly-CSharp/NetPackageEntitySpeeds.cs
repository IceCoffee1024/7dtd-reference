using System;
using UnityEngine.Scripting;

// Token: 0x020007EB RID: 2027
[Preserve]
public class NetPackageEntitySpeeds : NetPackageEntityTargeted
{
	// Token: 0x17000638 RID: 1592
	// (get) Token: 0x06003BE3 RID: 15331 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool ReliableDelivery
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06003BE4 RID: 15332 RVA: 0x001806C4 File Offset: 0x0017E8C4
	public NetPackageEntitySpeeds Setup(Entity _entity)
	{
		this.entityId = _entity.entityId;
		this.movementState = _entity.MovementState;
		this.speedForward = _entity.speedForward;
		this.speedStrafe = _entity.speedStrafe;
		return this;
	}

	// Token: 0x06003BE5 RID: 15333 RVA: 0x001806F7 File Offset: 0x0017E8F7
	public override void read(PooledBinaryReader _reader)
	{
		base.read(_reader);
		this.movementState = (int)_reader.ReadByte();
		this.speedForward = _reader.ReadSingle();
		this.speedStrafe = _reader.ReadSingle();
	}

	// Token: 0x06003BE6 RID: 15334 RVA: 0x00180724 File Offset: 0x0017E924
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write((byte)this.movementState);
		_writer.Write(this.speedForward);
		_writer.Write(this.speedStrafe);
	}

	// Token: 0x06003BE7 RID: 15335 RVA: 0x00180754 File Offset: 0x0017E954
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		Entity entity = _world.GetEntity(this.entityId);
		if (entity != null)
		{
			entity.MovementState = this.movementState;
			entity.speedForward = this.speedForward;
			entity.speedStrafe = this.speedStrafe;
			if (!_world.IsRemote())
			{
				_world.entityDistributer.SendPacketToTrackedPlayers(this.entityId, this.entityId, this, true);
			}
		}
	}

	// Token: 0x06003BE8 RID: 15336 RVA: 0x000B93F5 File Offset: 0x000B75F5
	public override int GetLength()
	{
		return 20;
	}

	// Token: 0x040030B8 RID: 12472
	[PublicizedFrom(EAccessModifier.Private)]
	public int movementState;

	// Token: 0x040030B9 RID: 12473
	[PublicizedFrom(EAccessModifier.Private)]
	public float speedForward;

	// Token: 0x040030BA RID: 12474
	[PublicizedFrom(EAccessModifier.Private)]
	public float speedStrafe;
}
