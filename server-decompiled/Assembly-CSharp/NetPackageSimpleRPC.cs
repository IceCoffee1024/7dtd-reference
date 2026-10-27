using System;
using UnityEngine.Scripting;

// Token: 0x02000860 RID: 2144
[Preserve]
public class NetPackageSimpleRPC : NetPackage
{
	// Token: 0x06003E25 RID: 15909 RVA: 0x0018B22C File Offset: 0x0018942C
	public NetPackageSimpleRPC Setup(int _entityId, SimpleRPCType _type)
	{
		this.entityId = _entityId;
		this.type = _type;
		return this;
	}

	// Token: 0x06003E26 RID: 15910 RVA: 0x0018B23D File Offset: 0x0018943D
	public override void read(PooledBinaryReader _reader)
	{
		this.entityId = _reader.ReadInt32();
		this.type = (SimpleRPCType)_reader.ReadByte();
	}

	// Token: 0x06003E27 RID: 15911 RVA: 0x0018B257 File Offset: 0x00189457
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.entityId);
		_writer.Write((byte)this.type);
	}

	// Token: 0x06003E28 RID: 15912 RVA: 0x0018B278 File Offset: 0x00189478
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (!base.ValidEntityIdForSender(this.entityId, false))
		{
			return;
		}
		_callbacks.SimpleRPC(this.entityId, this.type, true, _world.IsRemote());
	}

	// Token: 0x04003292 RID: 12946
	public int entityId;

	// Token: 0x04003293 RID: 12947
	public SimpleRPCType type;
}
