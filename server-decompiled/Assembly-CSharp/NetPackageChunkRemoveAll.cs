using System;
using UnityEngine.Scripting;

// Token: 0x020007BE RID: 1982
[Preserve]
public class NetPackageChunkRemoveAll : NetPackage
{
	// Token: 0x1700063B RID: 1595
	// (get) Token: 0x06003AF0 RID: 15088 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int Channel
	{
		get
		{
			return 0;
		}
	}

	// Token: 0x06003AF1 RID: 15089 RVA: 0x000027FC File Offset: 0x000009FC
	public override void read(PooledBinaryReader _reader)
	{
	}

	// Token: 0x06003AF2 RID: 15090 RVA: 0x0017C2CF File Offset: 0x0017A4CF
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
	}

	// Token: 0x06003AF3 RID: 15091 RVA: 0x0017D19D File Offset: 0x0017B39D
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		GameManager.Instance.World.m_ChunkManager.RemoveAllChunks();
	}

	// Token: 0x1700063C RID: 1596
	// (get) Token: 0x06003AF4 RID: 15092 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}
}
