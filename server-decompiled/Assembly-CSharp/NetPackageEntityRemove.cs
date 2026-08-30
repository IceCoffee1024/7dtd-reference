using System;
using UnityEngine.Scripting;

// Token: 0x020007E5 RID: 2021
[Preserve]
public class NetPackageEntityRemove : NetPackageEntityTargeted
{
	// Token: 0x17000635 RID: 1589
	// (get) Token: 0x06003BC0 RID: 15296 RVA: 0x0002F168 File Offset: 0x0002D368
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003BC1 RID: 15297 RVA: 0x0017FFEE File Offset: 0x0017E1EE
	public NetPackageEntityRemove Setup(int _entityId, EnumRemoveEntityReason _reason)
	{
		base.Setup(_entityId);
		this.reason = (byte)_reason;
		return this;
	}

	// Token: 0x06003BC2 RID: 15298 RVA: 0x00180000 File Offset: 0x0017E200
	public override void read(PooledBinaryReader _reader)
	{
		base.read(_reader);
		this.reason = _reader.ReadByte();
	}

	// Token: 0x06003BC3 RID: 15299 RVA: 0x00180015 File Offset: 0x0017E215
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.reason);
	}

	// Token: 0x06003BC4 RID: 15300 RVA: 0x0018002C File Offset: 0x0017E22C
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		if (_world.GetEntity(this.entityId) == null)
		{
			Log.Error(string.Format("NetPackageEntityRemove entity {0} missing", this.entityId));
		}
		_world.RemoveEntity(this.entityId, (EnumRemoveEntityReason)this.reason);
	}

	// Token: 0x06003BC5 RID: 15301 RVA: 0x00080550 File Offset: 0x0007E750
	public override int GetLength()
	{
		return 4;
	}

	// Token: 0x040030AD RID: 12461
	[PublicizedFrom(EAccessModifier.Private)]
	public byte reason;
}
