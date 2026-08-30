using System;
using UnityEngine.Scripting;

// Token: 0x020007BB RID: 1979
[Preserve]
public class NetPackageChunkRemoveAll : NetPackage
{
	// Token: 0x17000616 RID: 1558
	// (get) Token: 0x06003AC1 RID: 15041 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int Channel
	{
		get
		{
			return 0;
		}
	}

	// Token: 0x06003AC2 RID: 15042 RVA: 0x000027FC File Offset: 0x000009FC
	public override void read(PooledBinaryReader _reader)
	{
	}

	// Token: 0x06003AC3 RID: 15043 RVA: 0x0017BC33 File Offset: 0x00179E33
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
	}

	// Token: 0x06003AC4 RID: 15044 RVA: 0x0017CB61 File Offset: 0x0017AD61
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		GameManager.Instance.World.m_ChunkManager.RemoveAllChunks();
	}

	// Token: 0x06003AC5 RID: 15045 RVA: 0x00080550 File Offset: 0x0007E750
	public override int GetLength()
	{
		return 4;
	}

	// Token: 0x17000617 RID: 1559
	// (get) Token: 0x06003AC6 RID: 15046 RVA: 0x0002F168 File Offset: 0x0002D368
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}
}
