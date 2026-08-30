using System;
using UnityEngine.Scripting;

// Token: 0x02000871 RID: 2161
[Preserve]
public class NetPackageWorldTime : NetPackage
{
	// Token: 0x06003EFE RID: 16126 RVA: 0x0018C23B File Offset: 0x0018A43B
	public NetPackageWorldTime Setup(ulong _worldTime)
	{
		this.worldTime = _worldTime;
		return this;
	}

	// Token: 0x06003EFF RID: 16127 RVA: 0x0018C245 File Offset: 0x0018A445
	public override void read(PooledBinaryReader _br)
	{
		this.worldTime = _br.ReadUInt64();
	}

	// Token: 0x06003F00 RID: 16128 RVA: 0x0018C253 File Offset: 0x0018A453
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.worldTime);
	}

	// Token: 0x06003F01 RID: 16129 RVA: 0x0018C268 File Offset: 0x0018A468
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		_world.gameManager.SetWorldTime(this.worldTime);
	}

	// Token: 0x06003F02 RID: 16130 RVA: 0x00080B69 File Offset: 0x0007ED69
	public override int GetLength()
	{
		return 8;
	}

	// Token: 0x1700068F RID: 1679
	// (get) Token: 0x06003F03 RID: 16131 RVA: 0x0002EEAC File Offset: 0x0002D0AC
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x040032BC RID: 12988
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong worldTime;
}
