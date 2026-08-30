using System;
using UnityEngine.Scripting;

// Token: 0x020007E0 RID: 2016
[Preserve]
public class NetPackageEntitySpeeds : NetPackageEntityTargeted
{
	// Token: 0x1700062D RID: 1581
	// (get) Token: 0x06003B84 RID: 15236 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool ReliableDelivery
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06003B85 RID: 15237 RVA: 0x0017E2EC File Offset: 0x0017C4EC
	public NetPackageEntitySpeeds Setup(Entity _entity)
	{
		this.entityId = _entity.entityId;
		this.movementState = _entity.MovementState;
		this.speedForward = _entity.speedForward;
		this.speedStrafe = _entity.speedStrafe;
		return this;
	}

	// Token: 0x06003B86 RID: 15238 RVA: 0x0017E31F File Offset: 0x0017C51F
	public override void read(PooledBinaryReader _reader)
	{
		base.read(_reader);
		this.movementState = (int)_reader.ReadByte();
		this.speedForward = _reader.ReadSingle();
		this.speedStrafe = _reader.ReadSingle();
	}

	// Token: 0x06003B87 RID: 15239 RVA: 0x0017E34C File Offset: 0x0017C54C
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write((byte)this.movementState);
		_writer.Write(this.speedForward);
		_writer.Write(this.speedStrafe);
	}

	// Token: 0x06003B88 RID: 15240 RVA: 0x0017E37C File Offset: 0x0017C57C
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

	// Token: 0x06003B89 RID: 15241 RVA: 0x000B8CB1 File Offset: 0x000B6EB1
	public override int GetLength()
	{
		return 20;
	}

	// Token: 0x04003039 RID: 12345
	[PublicizedFrom(EAccessModifier.Private)]
	public int movementState;

	// Token: 0x0400303A RID: 12346
	[PublicizedFrom(EAccessModifier.Private)]
	public float speedForward;

	// Token: 0x0400303B RID: 12347
	[PublicizedFrom(EAccessModifier.Private)]
	public float speedStrafe;
}
