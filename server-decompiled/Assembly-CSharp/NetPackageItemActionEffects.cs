using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000803 RID: 2051
[Preserve]
public class NetPackageItemActionEffects : NetPackage
{
	// Token: 0x06003C57 RID: 15447 RVA: 0x0018275F File Offset: 0x0018095F
	public NetPackageItemActionEffects Setup(int _entityId, int _slotIdx, int _actionIdx, ItemActionFiringState _firingState, Vector3 _startPos, Vector3 _direction, int _userData)
	{
		this.entityId = _entityId;
		this.slotIdx = (sbyte)_slotIdx;
		this.actionIdx = (byte)_actionIdx;
		this.firingState = _firingState;
		this.startPos = _startPos;
		this.direction = _direction;
		this.userData = _userData;
		return this;
	}

	// Token: 0x06003C58 RID: 15448 RVA: 0x0018279C File Offset: 0x0018099C
	public override void read(PooledBinaryReader _reader)
	{
		this.entityId = _reader.ReadInt32();
		this.slotIdx = _reader.ReadSByte();
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

	// Token: 0x06003C59 RID: 15449 RVA: 0x00182820 File Offset: 0x00180A20
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

	// Token: 0x06003C5A RID: 15450 RVA: 0x001828BC File Offset: 0x00180ABC
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

	// Token: 0x0400310D RID: 12557
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;

	// Token: 0x0400310E RID: 12558
	[PublicizedFrom(EAccessModifier.Private)]
	public sbyte slotIdx;

	// Token: 0x0400310F RID: 12559
	[PublicizedFrom(EAccessModifier.Private)]
	public byte actionIdx;

	// Token: 0x04003110 RID: 12560
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemActionFiringState firingState;

	// Token: 0x04003111 RID: 12561
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 startPos;

	// Token: 0x04003112 RID: 12562
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 direction;

	// Token: 0x04003113 RID: 12563
	[PublicizedFrom(EAccessModifier.Private)]
	public int userData;
}
