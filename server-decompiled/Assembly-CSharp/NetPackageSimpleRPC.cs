using System;
using UnityEngine.Scripting;

// Token: 0x02000853 RID: 2131
[Preserve]
public class NetPackageSimpleRPC : NetPackage
{
	// Token: 0x06003E56 RID: 15958 RVA: 0x0018A2DB File Offset: 0x001884DB
	public NetPackageSimpleRPC Setup(int _entityId, SimpleRPCType _type)
	{
		this.entityId = _entityId;
		this.type = _type;
		return this;
	}

	// Token: 0x06003E57 RID: 15959 RVA: 0x0018A2EC File Offset: 0x001884EC
	public override void read(PooledBinaryReader _reader)
	{
		this.entityId = _reader.ReadInt32();
		this.type = (SimpleRPCType)_reader.ReadByte();
	}

	// Token: 0x06003E58 RID: 15960 RVA: 0x0018A306 File Offset: 0x00188506
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.entityId);
		_writer.Write((byte)this.type);
	}

	// Token: 0x06003E59 RID: 15961 RVA: 0x0018A327 File Offset: 0x00188527
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (!base.ValidEntityIdForSender(this.entityId, false))
		{
			return;
		}
		_callbacks.SimpleRPC(this.entityId, this.type, true, _world.IsRemote());
	}

	// Token: 0x06003E5A RID: 15962 RVA: 0x000811EE File Offset: 0x0007F3EE
	public override int GetLength()
	{
		return 10;
	}

	// Token: 0x04003287 RID: 12935
	public int entityId;

	// Token: 0x04003288 RID: 12936
	public SimpleRPCType type;
}
