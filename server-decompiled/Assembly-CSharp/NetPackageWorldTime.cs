using System;
using UnityEngine.Scripting;

// Token: 0x02000880 RID: 2176
[Preserve]
public class NetPackageWorldTime : NetPackage
{
	// Token: 0x06003EF2 RID: 16114 RVA: 0x0018E0FD File Offset: 0x0018C2FD
	public NetPackageWorldTime Setup(ulong _worldTime)
	{
		this.worldTime = _worldTime;
		return this;
	}

	// Token: 0x06003EF3 RID: 16115 RVA: 0x0018E107 File Offset: 0x0018C307
	public override void read(PooledBinaryReader _br)
	{
		this.worldTime = _br.ReadUInt64();
	}

	// Token: 0x06003EF4 RID: 16116 RVA: 0x0018E115 File Offset: 0x0018C315
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.worldTime);
	}

	// Token: 0x06003EF5 RID: 16117 RVA: 0x0018E12A File Offset: 0x0018C32A
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		_world.gameManager.SetWorldTime(this.worldTime);
	}

	// Token: 0x170006BA RID: 1722
	// (get) Token: 0x06003EF6 RID: 16118 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x04003328 RID: 13096
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong worldTime;
}
