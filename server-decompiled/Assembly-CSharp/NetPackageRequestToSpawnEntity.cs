using System;
using UnityEngine.Scripting;

// Token: 0x02000842 RID: 2114
[Preserve]
public class NetPackageRequestToSpawnEntity : NetPackage
{
	// Token: 0x17000676 RID: 1654
	// (get) Token: 0x06003DF7 RID: 15863 RVA: 0x00020021 File Offset: 0x0001E221
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003DF8 RID: 15864 RVA: 0x00188F69 File Offset: 0x00187169
	public NetPackageRequestToSpawnEntity Setup(EntityCreationData _es)
	{
		this.ecd = _es;
		return this;
	}

	// Token: 0x06003DF9 RID: 15865 RVA: 0x00188F73 File Offset: 0x00187173
	public override void read(PooledBinaryReader _reader)
	{
		this.ecd = new EntityCreationData();
		this.ecd.read(_reader, true);
	}

	// Token: 0x06003DFA RID: 15866 RVA: 0x00188F8D File Offset: 0x0018718D
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		this.ecd.write(_writer, true);
	}

	// Token: 0x06003DFB RID: 15867 RVA: 0x00188FA3 File Offset: 0x001871A3
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		_world.GetGameManager().RequestToSpawnEntityServer(this.ecd);
	}

	// Token: 0x06003DFC RID: 15868 RVA: 0x00183C79 File Offset: 0x00181E79
	public override int GetLength()
	{
		return 32;
	}

	// Token: 0x04003255 RID: 12885
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityCreationData ecd;
}
