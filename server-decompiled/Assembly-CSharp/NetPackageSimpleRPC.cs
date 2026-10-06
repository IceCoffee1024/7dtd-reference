using System;
using UnityEngine.Scripting;

// Token: 0x02000856 RID: 2134
[Preserve]
public class NetPackageSimpleRPC : NetPackage
{
	// Token: 0x06003E02 RID: 15874 RVA: 0x0018A184 File Offset: 0x00188384
	public NetPackageSimpleRPC Setup(int _entityId, SimpleRPCType _type)
	{
		this.entityId = _entityId;
		this.type = _type;
		return this;
	}

	// Token: 0x06003E03 RID: 15875 RVA: 0x0018A195 File Offset: 0x00188395
	public override void read(PooledBinaryReader _reader)
	{
		this.entityId = _reader.ReadInt32();
		this.type = (SimpleRPCType)_reader.ReadByte();
	}

	// Token: 0x06003E04 RID: 15876 RVA: 0x0018A1AF File Offset: 0x001883AF
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.entityId);
		_writer.Write((byte)this.type);
	}

	// Token: 0x06003E05 RID: 15877 RVA: 0x0018A1D0 File Offset: 0x001883D0
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (!base.ValidEntityIdForSender(this.entityId, false))
		{
			return;
		}
		_callbacks.SimpleRPC(this.entityId, this.type, true, _world.IsRemote());
	}

	// Token: 0x04003272 RID: 12914
	public int entityId;

	// Token: 0x04003273 RID: 12915
	public SimpleRPCType type;
}
