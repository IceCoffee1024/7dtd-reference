using System;
using UnityEngine.Scripting;

// Token: 0x0200084D RID: 2125
[Preserve]
public class NetPackageRequestToSpawnEntity : NetPackage
{
	// Token: 0x1700069B RID: 1691
	// (get) Token: 0x06003DC9 RID: 15817 RVA: 0x00020119 File Offset: 0x0001E319
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003DCA RID: 15818 RVA: 0x00189BA5 File Offset: 0x00187DA5
	public NetPackageRequestToSpawnEntity Setup(EntityCreationData _es)
	{
		this.ecd = _es;
		return this;
	}

	// Token: 0x06003DCB RID: 15819 RVA: 0x00189BAF File Offset: 0x00187DAF
	public override void read(PooledBinaryReader _reader)
	{
		this.ecd = new EntityCreationData();
		this.ecd.read(_reader, StreamModeRead.FromClient);
	}

	// Token: 0x06003DCC RID: 15820 RVA: 0x00189BC9 File Offset: 0x00187DC9
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		this.ecd.write(_writer, StreamModeWrite.ToServer);
	}

	// Token: 0x06003DCD RID: 15821 RVA: 0x00189BDF File Offset: 0x00187DDF
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		_world.GetGameManager().RequestToSpawnEntityServer(this.ecd);
	}

	// Token: 0x0400325C RID: 12892
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityCreationData ecd;
}
