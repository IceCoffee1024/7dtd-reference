using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000804 RID: 2052
[Preserve]
public class NetPackageItemActionEffects : NetPackage
{
	// Token: 0x06003C7B RID: 15483 RVA: 0x00182706 File Offset: 0x00180906
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

	// Token: 0x06003C7C RID: 15484 RVA: 0x00182740 File Offset: 0x00180940
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

	// Token: 0x06003C7D RID: 15485 RVA: 0x001827C4 File Offset: 0x001809C4
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

	// Token: 0x06003C7E RID: 15486 RVA: 0x00182860 File Offset: 0x00180A60
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

	// Token: 0x06003C7F RID: 15487 RVA: 0x00177A48 File Offset: 0x00175C48
	public override int GetLength()
	{
		return 50;
	}

	// Token: 0x0400311F RID: 12575
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;

	// Token: 0x04003120 RID: 12576
	[PublicizedFrom(EAccessModifier.Private)]
	public byte slotIdx;

	// Token: 0x04003121 RID: 12577
	[PublicizedFrom(EAccessModifier.Private)]
	public byte actionIdx;

	// Token: 0x04003122 RID: 12578
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemActionFiringState firingState;

	// Token: 0x04003123 RID: 12579
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 startPos;

	// Token: 0x04003124 RID: 12580
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 direction;

	// Token: 0x04003125 RID: 12581
	[PublicizedFrom(EAccessModifier.Private)]
	public int userData;
}
