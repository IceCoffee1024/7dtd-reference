using System;
using UnityEngine.Scripting;

// Token: 0x0200088A RID: 2186
[Preserve]
public class NetPackageWorldTime : NetPackage
{
	// Token: 0x06003F15 RID: 16149 RVA: 0x0018F1A5 File Offset: 0x0018D3A5
	public NetPackageWorldTime Setup(ulong _worldTime)
	{
		this.worldTime = _worldTime;
		return this;
	}

	// Token: 0x06003F16 RID: 16150 RVA: 0x0018F1AF File Offset: 0x0018D3AF
	public override void read(PooledBinaryReader _br)
	{
		this.worldTime = _br.ReadUInt64();
	}

	// Token: 0x06003F17 RID: 16151 RVA: 0x0018F1BD File Offset: 0x0018D3BD
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.worldTime);
	}

	// Token: 0x06003F18 RID: 16152 RVA: 0x0018F1D2 File Offset: 0x0018D3D2
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		_world.gameManager.SetWorldTime(this.worldTime);
	}

	// Token: 0x170006C0 RID: 1728
	// (get) Token: 0x06003F19 RID: 16153 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x04003348 RID: 13128
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong worldTime;
}
