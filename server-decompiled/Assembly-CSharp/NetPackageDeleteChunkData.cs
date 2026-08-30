using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020007C8 RID: 1992
[Preserve]
public class NetPackageDeleteChunkData : NetPackage
{
	// Token: 0x06003B1A RID: 15130 RVA: 0x0017E026 File Offset: 0x0017C226
	public NetPackageDeleteChunkData Setup(ICollection<long> _chunkKeys)
	{
		this.chunkKeys.Clear();
		this.chunkKeys.AddRange(_chunkKeys);
		this.length = 4 + 8 * this.chunkKeys.Count;
		return this;
	}

	// Token: 0x06003B1B RID: 15131 RVA: 0x0017E058 File Offset: 0x0017C258
	public override void read(PooledBinaryReader _br)
	{
		int num = _br.ReadInt32();
		this.chunkKeys = new List<long>();
		for (int i = 0; i < num; i++)
		{
			this.chunkKeys.Add(_br.ReadInt64());
		}
	}

	// Token: 0x06003B1C RID: 15132 RVA: 0x0017E094 File Offset: 0x0017C294
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.chunkKeys.Count);
		for (int i = 0; i < this.chunkKeys.Count; i++)
		{
			_bw.Write(this.chunkKeys[i]);
		}
	}

	// Token: 0x06003B1D RID: 15133 RVA: 0x0017E0E4 File Offset: 0x0017C2E4
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

	// Token: 0x17000623 RID: 1571
	// (get) Token: 0x06003B1E RID: 15134 RVA: 0x0002F168 File Offset: 0x0002D368
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003B1F RID: 15135 RVA: 0x0017E131 File Offset: 0x0017C331
	public override int GetLength()
	{
		return this.length;
	}

	// Token: 0x04003041 RID: 12353
	public List<long> chunkKeys = new List<long>();

	// Token: 0x04003042 RID: 12354
	[PublicizedFrom(EAccessModifier.Private)]
	public int length;
}
