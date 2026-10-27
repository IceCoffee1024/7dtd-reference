using System;
using UnityEngine.Scripting;

// Token: 0x020007F8 RID: 2040
[Preserve]
public class NetPackageEntitySpeeds : NetPackageEntityTargeted
{
	// Token: 0x17000663 RID: 1635
	// (get) Token: 0x06003C0E RID: 15374 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool ReliableDelivery
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06003C0F RID: 15375 RVA: 0x00181D00 File Offset: 0x0017FF00
	public NetPackageEntitySpeeds Setup(Entity _entity)
	{
		this.entityId = _entity.entityId;
		this.movementState = _entity.MovementState;
		this.speedForward = _entity.speedForward;
		this.speedStrafe = _entity.speedStrafe;
		return this;
	}

	// Token: 0x06003C10 RID: 15376 RVA: 0x00181D33 File Offset: 0x0017FF33
	public override void read(PooledBinaryReader _reader)
	{
		base.read(_reader);
		this.movementState = (int)_reader.ReadByte();
		this.speedForward = _reader.ReadSingle();
		this.speedStrafe = _reader.ReadSingle();
	}

	// Token: 0x06003C11 RID: 15377 RVA: 0x00181D60 File Offset: 0x0017FF60
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write((byte)this.movementState);
		_writer.Write(this.speedForward);
		_writer.Write(this.speedStrafe);
	}

	// Token: 0x06003C12 RID: 15378 RVA: 0x00181D90 File Offset: 0x0017FF90
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

	// Token: 0x040030D1 RID: 12497
	[PublicizedFrom(EAccessModifier.Private)]
	public int movementState;

	// Token: 0x040030D2 RID: 12498
	[PublicizedFrom(EAccessModifier.Private)]
	public float speedForward;

	// Token: 0x040030D3 RID: 12499
	[PublicizedFrom(EAccessModifier.Private)]
	public float speedStrafe;
}
