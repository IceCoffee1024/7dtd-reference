using System;
using UnityEngine.Scripting;

// Token: 0x020007EE RID: 2030
[Preserve]
public class NetPackageEntitySpeeds : NetPackageEntityTargeted
{
	// Token: 0x1700065D RID: 1629
	// (get) Token: 0x06003BEB RID: 15339 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool ReliableDelivery
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06003BEC RID: 15340 RVA: 0x00180BFC File Offset: 0x0017EDFC
	public NetPackageEntitySpeeds Setup(Entity _entity)
	{
		this.entityId = _entity.entityId;
		this.movementState = _entity.MovementState;
		this.speedForward = _entity.speedForward;
		this.speedStrafe = _entity.speedStrafe;
		return this;
	}

	// Token: 0x06003BED RID: 15341 RVA: 0x00180C2F File Offset: 0x0017EE2F
	public override void read(PooledBinaryReader _reader)
	{
		base.read(_reader);
		this.movementState = (int)_reader.ReadByte();
		this.speedForward = _reader.ReadSingle();
		this.speedStrafe = _reader.ReadSingle();
	}

	// Token: 0x06003BEE RID: 15342 RVA: 0x00180C5C File Offset: 0x0017EE5C
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write((byte)this.movementState);
		_writer.Write(this.speedForward);
		_writer.Write(this.speedStrafe);
	}

	// Token: 0x06003BEF RID: 15343 RVA: 0x00180C8C File Offset: 0x0017EE8C
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

	// Token: 0x040030B1 RID: 12465
	[PublicizedFrom(EAccessModifier.Private)]
	public int movementState;

	// Token: 0x040030B2 RID: 12466
	[PublicizedFrom(EAccessModifier.Private)]
	public float speedForward;

	// Token: 0x040030B3 RID: 12467
	[PublicizedFrom(EAccessModifier.Private)]
	public float speedStrafe;
}
