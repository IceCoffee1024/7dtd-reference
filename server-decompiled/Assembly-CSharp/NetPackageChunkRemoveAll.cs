using System;
using UnityEngine.Scripting;

// Token: 0x020007C6 RID: 1990
[Preserve]
public class NetPackageChunkRemoveAll : NetPackage
{
	// Token: 0x1700063F RID: 1599
	// (get) Token: 0x06003B07 RID: 15111 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int Channel
	{
		get
		{
			return 0;
		}
	}

	// Token: 0x06003B08 RID: 15112 RVA: 0x000027FC File Offset: 0x000009FC
	public override void read(PooledBinaryReader _reader)
	{
	}

	// Token: 0x06003B09 RID: 15113 RVA: 0x0017D267 File Offset: 0x0017B467
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
	}

	// Token: 0x06003B0A RID: 15114 RVA: 0x0017E135 File Offset: 0x0017C335
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		GameManager.Instance.World.m_ChunkManager.RemoveAllChunks();
	}

	// Token: 0x17000640 RID: 1600
	// (get) Token: 0x06003B0B RID: 15115 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}
}
