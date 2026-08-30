using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020007BD RID: 1981
[Preserve]
public class NetPackageDeleteChunkData : NetPackage
{
	// Token: 0x06003ABB RID: 15035 RVA: 0x0017BC4E File Offset: 0x00179E4E
	public NetPackageDeleteChunkData Setup(ICollection<long> _chunkKeys)
	{
		this.chunkKeys.Clear();
		this.chunkKeys.AddRange(_chunkKeys);
		this.length = 4 + 8 * this.chunkKeys.Count;
		return this;
	}

	// Token: 0x06003ABC RID: 15036 RVA: 0x0017BC80 File Offset: 0x00179E80
	public override void read(PooledBinaryReader _br)
	{
		int num = _br.ReadInt32();
		this.chunkKeys = new List<long>();
		for (int i = 0; i < num; i++)
		{
			this.chunkKeys.Add(_br.ReadInt64());
		}
	}

	// Token: 0x06003ABD RID: 15037 RVA: 0x0017BCBC File Offset: 0x00179EBC
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.chunkKeys.Count);
		for (int i = 0; i < this.chunkKeys.Count; i++)
		{
			_bw.Write(this.chunkKeys[i]);
		}
	}

	// Token: 0x06003ABE RID: 15038 RVA: 0x0017BD0C File Offset: 0x00179F0C
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

	// Token: 0x17000618 RID: 1560
	// (get) Token: 0x06003ABF RID: 15039 RVA: 0x0002EEAC File Offset: 0x0002D0AC
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003AC0 RID: 15040 RVA: 0x0017BD59 File Offset: 0x00179F59
	public override int GetLength()
	{
		return this.length;
	}

	// Token: 0x04002FC2 RID: 12226
	public List<long> chunkKeys = new List<long>();

	// Token: 0x04002FC3 RID: 12227
	[PublicizedFrom(EAccessModifier.Private)]
	public int length;
}
