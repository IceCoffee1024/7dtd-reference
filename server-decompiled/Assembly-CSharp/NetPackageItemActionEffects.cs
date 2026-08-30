using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020007F9 RID: 2041
[Preserve]
public class NetPackageItemActionEffects : NetPackage
{
	// Token: 0x06003C1C RID: 15388 RVA: 0x0018032E File Offset: 0x0017E52E
	public NetPackageItemActionEffects Setup(int _entityId, int _slotIdx, int _actionIdx, ItemActionFiringState _firingState, Vector3 _startPos, Vector3 _direction, int _userData)
	{
		this.entityId = _entityId;
		this.slotIdx = (byte)_slotIdx;
		this.actionIdx = (byte)_actionIdx;
		this.firingState = _firingState;
		this.startPos = _startPos;
		this.direction = _direction;
		this.userData = _userData;
		return this;
	}

	// Token: 0x06003C1D RID: 15389 RVA: 0x00180368 File Offset: 0x0017E568
	public override void read(PooledBinaryReader _reader)
	{
		this.entityId = _reader.ReadInt32();
		this.slotIdx = _reader.ReadByte();
		this.actionIdx = _reader.ReadByte();
		this.firingState = (ItemActionFiringState)_reader.ReadByte();
		if (_reader.ReadBoolean())
		{
			this.startPos = StreamUtils.ReadVector3(_reader);
			this.direction = StreamUtils.ReadVector3(_reader);
		}
		else
		{
			this.startPos = Vector3.zero;
			this.direction = Vector3.zero;
		}
		this.userData = _reader.ReadInt32();
	}

	// Token: 0x06003C1E RID: 15390 RVA: 0x001803EC File Offset: 0x0017E5EC
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.entityId);
		_writer.Write(this.slotIdx);
		_writer.Write(this.actionIdx);
		_writer.Write((byte)this.firingState);
		bool flag = !this.startPos.Equals(Vector3.zero) || !this.direction.Equals(Vector3.zero);
		_writer.Write(flag);
		if (flag)
		{
			StreamUtils.Write(_writer, this.startPos);
			StreamUtils.Write(_writer, this.direction);
		}
		_writer.Write(this.userData);
	}

	// Token: 0x06003C1F RID: 15391 RVA: 0x00180488 File Offset: 0x0017E688
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		if (!_world.IsRemote())
		{
			_world.GetGameManager().ItemActionEffectsServer(this.entityId, (int)this.slotIdx, (int)this.actionIdx, (int)this.firingState, this.startPos, this.direction, this.userData);
			return;
		}
		_world.GetGameManager().ItemActionEffectsClient(this.entityId, (int)this.slotIdx, (int)this.actionIdx, (int)this.firingState, this.startPos, this.direction, this.userData);
	}

	// Token: 0x06003C20 RID: 15392 RVA: 0x001757C8 File Offset: 0x001739C8
	public override int GetLength()
	{
		return 50;
	}

	// Token: 0x040030A0 RID: 12448
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;

	// Token: 0x040030A1 RID: 12449
	[PublicizedFrom(EAccessModifier.Private)]
	public byte slotIdx;

	// Token: 0x040030A2 RID: 12450
	[PublicizedFrom(EAccessModifier.Private)]
	public byte actionIdx;

	// Token: 0x040030A3 RID: 12451
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemActionFiringState firingState;

	// Token: 0x040030A4 RID: 12452
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 startPos;

	// Token: 0x040030A5 RID: 12453
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 direction;

	// Token: 0x040030A6 RID: 12454
	[PublicizedFrom(EAccessModifier.Private)]
	public int userData;
}
