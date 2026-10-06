using System;
using UnityEngine.Scripting;

// Token: 0x02000843 RID: 2115
[Preserve]
public class NetPackageRequestToSpawnEntity : NetPackage
{
	// Token: 0x17000695 RID: 1685
	// (get) Token: 0x06003DA6 RID: 15782 RVA: 0x00020119 File Offset: 0x0001E319
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003DA7 RID: 15783 RVA: 0x00188AA1 File Offset: 0x00186CA1
	public NetPackageRequestToSpawnEntity Setup(EntityCreationData _es)
	{
		this.ecd = _es;
		return this;
	}

	// Token: 0x06003DA8 RID: 15784 RVA: 0x00188AAB File Offset: 0x00186CAB
	public override void read(PooledBinaryReader _reader)
	{
		this.ecd = new EntityCreationData();
		this.ecd.read(_reader, StreamModeRead.FromClient);
	}

	// Token: 0x06003DA9 RID: 15785 RVA: 0x00188AC5 File Offset: 0x00186CC5
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		this.ecd.write(_writer, StreamModeWrite.ToServer);
	}

	// Token: 0x06003DAA RID: 15786 RVA: 0x00188ADB File Offset: 0x00186CDB
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		_world.GetGameManager().RequestToSpawnEntityServer(this.ecd);
	}

	// Token: 0x0400323C RID: 12860
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityCreationData ecd;
}
