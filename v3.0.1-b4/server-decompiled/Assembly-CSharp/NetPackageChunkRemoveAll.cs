using System;
using UnityEngine.Scripting;

// Token: 0x020007B1 RID: 1969
[Preserve]
public class NetPackageChunkRemoveAll : NetPackage
{
	// Token: 0x1700060C RID: 1548
	// (get) Token: 0x06003A69 RID: 14953 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int Channel
	{
		get
		{
			return 0;
		}
	}

	// Token: 0x06003A6A RID: 14954 RVA: 0x000027FC File Offset: 0x000009FC
	public override void read(PooledBinaryReader _reader)
	{
	}

	// Token: 0x06003A6B RID: 14955 RVA: 0x001799B3 File Offset: 0x00177BB3
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
	}

	// Token: 0x06003A6C RID: 14956 RVA: 0x0017A8E1 File Offset: 0x00178AE1
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		GameManager.Instance.World.m_ChunkManager.RemoveAllChunks();
	}

	// Token: 0x06003A6D RID: 14957 RVA: 0x0007FE9C File Offset: 0x0007E09C
	public override int GetLength()
	{
		return 4;
	}

	// Token: 0x1700060D RID: 1549
	// (get) Token: 0x06003A6E RID: 14958 RVA: 0x0002EEAC File Offset: 0x0002D0AC
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}
}
