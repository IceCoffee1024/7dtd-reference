using System;
using UnityEngine.Scripting;

// Token: 0x020007E8 RID: 2024
[Preserve]
public class NetPackageEntityRemove : NetPackageEntityTargeted
{
	// Token: 0x1700065A RID: 1626
	// (get) Token: 0x06003BCD RID: 15309 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003BCE RID: 15310 RVA: 0x0018052A File Offset: 0x0017E72A
	public NetPackageEntityRemove Setup(int _entityId, EnumRemoveEntityReason _reason)
	{
		base.Setup(_entityId);
		this.reason = (byte)_reason;
		return this;
	}

	// Token: 0x06003BCF RID: 15311 RVA: 0x0018053C File Offset: 0x0017E73C
	public override void read(PooledBinaryReader _reader)
	{
		base.read(_reader);
		this.reason = _reader.ReadByte();
	}

	// Token: 0x06003BD0 RID: 15312 RVA: 0x00180551 File Offset: 0x0017E751
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.reason);
	}

	// Token: 0x06003BD1 RID: 15313 RVA: 0x00180568 File Offset: 0x0017E768
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

	// Token: 0x040030A6 RID: 12454
	[PublicizedFrom(EAccessModifier.Private)]
	public byte reason;
}
