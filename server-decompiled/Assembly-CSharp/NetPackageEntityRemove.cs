using System;
using UnityEngine.Scripting;

// Token: 0x020007F2 RID: 2034
[Preserve]
public class NetPackageEntityRemove : NetPackageEntityTargeted
{
	// Token: 0x17000660 RID: 1632
	// (get) Token: 0x06003BF0 RID: 15344 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003BF1 RID: 15345 RVA: 0x0018162E File Offset: 0x0017F82E
	public NetPackageEntityRemove Setup(int _entityId, EnumRemoveEntityReason _reason)
	{
		base.Setup(_entityId);
		this.reason = (byte)_reason;
		return this;
	}

	// Token: 0x06003BF2 RID: 15346 RVA: 0x00181640 File Offset: 0x0017F840
	public override void read(PooledBinaryReader _reader)
	{
		base.read(_reader);
		this.reason = _reader.ReadByte();
	}

	// Token: 0x06003BF3 RID: 15347 RVA: 0x00181655 File Offset: 0x0017F855
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.reason);
	}

	// Token: 0x06003BF4 RID: 15348 RVA: 0x0018166C File Offset: 0x0017F86C
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

	// Token: 0x040030C6 RID: 12486
	[PublicizedFrom(EAccessModifier.Private)]
	public byte reason;
}
