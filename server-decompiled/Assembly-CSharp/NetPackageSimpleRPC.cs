using System;
using UnityEngine.Scripting;

// Token: 0x02000847 RID: 2119
[Preserve]
public class NetPackageSimpleRPC : NetPackage
{
	// Token: 0x06003DF1 RID: 15857 RVA: 0x001882A7 File Offset: 0x001864A7
	public NetPackageSimpleRPC Setup(int _entityId, SimpleRPCType _type)
	{
		this.entityId = _entityId;
		this.type = _type;
		return this;
	}

	// Token: 0x06003DF2 RID: 15858 RVA: 0x001882B8 File Offset: 0x001864B8
	public override void read(PooledBinaryReader _reader)
	{
		this.entityId = _reader.ReadInt32();
		this.type = (SimpleRPCType)_reader.ReadByte();
	}

	// Token: 0x06003DF3 RID: 15859 RVA: 0x001882D2 File Offset: 0x001864D2
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.entityId);
		_writer.Write((byte)this.type);
	}

	// Token: 0x06003DF4 RID: 15860 RVA: 0x001882F3 File Offset: 0x001864F3
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (!base.ValidEntityIdForSender(this.entityId, false))
		{
			return;
		}
		_callbacks.SimpleRPC(this.entityId, this.type, true, _world.IsRemote());
	}

	// Token: 0x06003DF5 RID: 15861 RVA: 0x00080B3A File Offset: 0x0007ED3A
	public override int GetLength()
	{
		return 10;
	}

	// Token: 0x04003208 RID: 12808
	public int entityId;

	// Token: 0x04003209 RID: 12809
	public SimpleRPCType type;
}
