using System;
using UnityEngine.Scripting;

// Token: 0x02000836 RID: 2102
[Preserve]
public class NetPackageRequestToSpawnEntity : NetPackage
{
	// Token: 0x1700066A RID: 1642
	// (get) Token: 0x06003D92 RID: 15762 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003D93 RID: 15763 RVA: 0x00186F49 File Offset: 0x00185149
	public NetPackageRequestToSpawnEntity Setup(EntityCreationData _es)
	{
		this.ecd = _es;
		return this;
	}

	// Token: 0x06003D94 RID: 15764 RVA: 0x00186F53 File Offset: 0x00185153
	public override void read(PooledBinaryReader _reader)
	{
		this.ecd = new EntityCreationData();
		this.ecd.read(_reader, true);
	}

	// Token: 0x06003D95 RID: 15765 RVA: 0x00186F6D File Offset: 0x0018516D
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		this.ecd.write(_writer, true);
	}

	// Token: 0x06003D96 RID: 15766 RVA: 0x00186F83 File Offset: 0x00185183
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		_world.GetGameManager().RequestToSpawnEntityServer(this.ecd);
	}

	// Token: 0x06003D97 RID: 15767 RVA: 0x001818A1 File Offset: 0x0017FAA1
	public override int GetLength()
	{
		return 32;
	}

	// Token: 0x040031D6 RID: 12758
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityCreationData ecd;
}
