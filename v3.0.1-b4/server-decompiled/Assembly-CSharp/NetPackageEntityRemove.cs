using System;
using UnityEngine.Scripting;

// Token: 0x020007DA RID: 2010
[Preserve]
public class NetPackageEntityRemove : NetPackageEntityTargeted
{
	// Token: 0x1700062A RID: 1578
	// (get) Token: 0x06003B61 RID: 15201 RVA: 0x0002EEAC File Offset: 0x0002D0AC
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003B62 RID: 15202 RVA: 0x0017DC16 File Offset: 0x0017BE16
	public NetPackageEntityRemove Setup(int _entityId, EnumRemoveEntityReason _reason)
	{
		base.Setup(_entityId);
		this.reason = (byte)_reason;
		return this;
	}

	// Token: 0x06003B63 RID: 15203 RVA: 0x0017DC28 File Offset: 0x0017BE28
	public override void read(PooledBinaryReader _reader)
	{
		base.read(_reader);
		this.reason = _reader.ReadByte();
	}

	// Token: 0x06003B64 RID: 15204 RVA: 0x0017DC3D File Offset: 0x0017BE3D
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.reason);
	}

	// Token: 0x06003B65 RID: 15205 RVA: 0x0017DC54 File Offset: 0x0017BE54
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

	// Token: 0x06003B66 RID: 15206 RVA: 0x0007FE9C File Offset: 0x0007E09C
	public override int GetLength()
	{
		return 4;
	}

	// Token: 0x0400302E RID: 12334
	[PublicizedFrom(EAccessModifier.Private)]
	public byte reason;
}
