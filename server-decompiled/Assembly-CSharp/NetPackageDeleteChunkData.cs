using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020007D3 RID: 2003
[Preserve]
public class NetPackageDeleteChunkData : NetPackage
{
	// Token: 0x06003B54 RID: 15188 RVA: 0x0017F59C File Offset: 0x0017D79C
	public NetPackageDeleteChunkData Setup(ICollection<long> _chunkKeys)
	{
		this.chunkKeys.Clear();
		this.chunkKeys.AddRange(_chunkKeys);
		this.length = 4 + 8 * this.chunkKeys.Count;
		return this;
	}

	// Token: 0x06003B55 RID: 15189 RVA: 0x0017F5CC File Offset: 0x0017D7CC
	public override void read(PooledBinaryReader _br)
	{
		int num = _br.ReadInt32();
		this.chunkKeys = new List<long>();
		for (int i = 0; i < num; i++)
		{
			this.chunkKeys.Add(_br.ReadInt64());
		}
	}

	// Token: 0x06003B56 RID: 15190 RVA: 0x0017F608 File Offset: 0x0017D808
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.chunkKeys.Count);
		for (int i = 0; i < this.chunkKeys.Count; i++)
		{
			_bw.Write(this.chunkKeys[i]);
		}
	}

	// Token: 0x06003B57 RID: 15191 RVA: 0x0017F658 File Offset: 0x0017D858
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

	// Token: 0x1700064C RID: 1612
	// (get) Token: 0x06003B58 RID: 15192 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x04003057 RID: 12375
	public List<long> chunkKeys = new List<long>();

	// Token: 0x04003058 RID: 12376
	[PublicizedFrom(EAccessModifier.Private)]
	public int length;
}
