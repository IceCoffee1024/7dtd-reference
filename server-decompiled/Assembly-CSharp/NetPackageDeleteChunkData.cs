using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020007CB RID: 1995
[Preserve]
public class NetPackageDeleteChunkData : NetPackage
{
	// Token: 0x06003B3D RID: 15165 RVA: 0x0017E604 File Offset: 0x0017C804
	public NetPackageDeleteChunkData Setup(ICollection<long> _chunkKeys)
	{
		this.chunkKeys.Clear();
		this.chunkKeys.AddRange(_chunkKeys);
		this.length = 4 + 8 * this.chunkKeys.Count;
		return this;
	}

	// Token: 0x06003B3E RID: 15166 RVA: 0x0017E634 File Offset: 0x0017C834
	public override void read(PooledBinaryReader _br)
	{
		int num = _br.ReadInt32();
		this.chunkKeys = new List<long>();
		for (int i = 0; i < num; i++)
		{
			this.chunkKeys.Add(_br.ReadInt64());
		}
	}

	// Token: 0x06003B3F RID: 15167 RVA: 0x0017E670 File Offset: 0x0017C870
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.chunkKeys.Count);
		for (int i = 0; i < this.chunkKeys.Count; i++)
		{
			_bw.Write(this.chunkKeys[i]);
		}
	}

	// Token: 0x06003B40 RID: 15168 RVA: 0x0017E6C0 File Offset: 0x0017C8C0
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		DynamicMeshUnity.DeleteDynamicMeshData(this.chunkKeys);
		WaterSimulationNative instance = WaterSimulationNative.Instance;
		if (instance != null)
		{
			WaterSimulationApplyChanges changeApplier = instance.changeApplier;
			if (changeApplier != null)
			{
				changeApplier.DiscardChangesForChunks(this.chunkKeys);
			}
		}
		MultiBlockManager.Instance.CullChunklessDataOnClient(this.chunkKeys);
	}

	// Token: 0x17000648 RID: 1608
	// (get) Token: 0x06003B41 RID: 15169 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x0400303B RID: 12347
	public List<long> chunkKeys = new List<long>();

	// Token: 0x0400303C RID: 12348
	[PublicizedFrom(EAccessModifier.Private)]
	public int length;
}
