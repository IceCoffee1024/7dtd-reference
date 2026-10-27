using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200080D RID: 2061
[Preserve]
public class NetPackageItemActionEffects : NetPackage
{
	// Token: 0x06003C7A RID: 15482 RVA: 0x00183863 File Offset: 0x00181A63
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

	// Token: 0x06003C7B RID: 15483 RVA: 0x001838A0 File Offset: 0x00181AA0
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

	// Token: 0x06003C7C RID: 15484 RVA: 0x00183924 File Offset: 0x00181B24
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

	// Token: 0x06003C7D RID: 15485 RVA: 0x001839C0 File Offset: 0x00181BC0
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

	// Token: 0x0400312D RID: 12589
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;

	// Token: 0x0400312E RID: 12590
	[PublicizedFrom(EAccessModifier.Private)]
	public sbyte slotIdx;

	// Token: 0x0400312F RID: 12591
	[PublicizedFrom(EAccessModifier.Private)]
	public byte actionIdx;

	// Token: 0x04003130 RID: 12592
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemActionFiringState firingState;

	// Token: 0x04003131 RID: 12593
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 startPos;

	// Token: 0x04003132 RID: 12594
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 direction;

	// Token: 0x04003133 RID: 12595
	[PublicizedFrom(EAccessModifier.Private)]
	public int userData;
}
