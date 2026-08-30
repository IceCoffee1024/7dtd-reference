using System;
using UnityEngine.Scripting;

// Token: 0x0200087D RID: 2173
[Preserve]
public class NetPackageWorldTime : NetPackage
{
	// Token: 0x06003F63 RID: 16227 RVA: 0x0018E30B File Offset: 0x0018C50B
	public NetPackageWorldTime Setup(ulong _worldTime)
	{
		this.worldTime = _worldTime;
		return this;
	}

	// Token: 0x06003F64 RID: 16228 RVA: 0x0018E315 File Offset: 0x0018C515
	public override void read(PooledBinaryReader _br)
	{
		this.worldTime = _br.ReadUInt64();
	}

	// Token: 0x06003F65 RID: 16229 RVA: 0x0018E323 File Offset: 0x0018C523
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.worldTime);
	}

	// Token: 0x06003F66 RID: 16230 RVA: 0x0018E338 File Offset: 0x0018C538
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		_world.gameManager.SetWorldTime(this.worldTime);
	}

	// Token: 0x06003F67 RID: 16231 RVA: 0x0008121D File Offset: 0x0007F41D
	public override int GetLength()
	{
		return 8;
	}

	// Token: 0x1700069B RID: 1691
	// (get) Token: 0x06003F68 RID: 16232 RVA: 0x0002F168 File Offset: 0x0002D368
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x0400333D RID: 13117
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong worldTime;
}
