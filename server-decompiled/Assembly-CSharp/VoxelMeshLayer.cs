using System;
using System.Threading;
using UnityEngine;

// Token: 0x02000BF7 RID: 3063
public class VoxelMeshLayer : IMemoryPoolableObject
{
	// Token: 0x06005BE6 RID: 23526 RVA: 0x0023D390 File Offset: 0x0023B590
	public VoxelMeshLayer()
	{
		this.idx = 0;
		this.meshes = new VoxelMesh[MeshDescription.meshes.Length];
		for (int i = 0; i < MeshDescription.meshes.Length; i++)
		{
			this.meshes[i] = VoxelMesh.Create(i, MeshDescription.meshes[i].meshType, 0);
		}
		Interlocked.Increment(ref VoxelMeshLayer.InstanceCount);
	}

	// Token: 0x06005BE7 RID: 23527 RVA: 0x0023D3F4 File Offset: 0x0023B5F4
	public void Reset()
	{
		for (int i = 0; i < this.meshes.Length; i++)
		{
			this.meshes[i].ClearMesh();
		}
		if (this.pendingCopyOperations != 0)
		{
			Log.Error("Resetting VML with pendingCopyOperations");
		}
		this.pendingCopyOperations = 0;
	}

	// Token: 0x06005BE8 RID: 23528 RVA: 0x0023D43A File Offset: 0x0023B63A
	public void Cleanup()
	{
		if (this.cleanedUp)
		{
			return;
		}
		Interlocked.Decrement(ref VoxelMeshLayer.InstanceCount);
		this.cleanedUp = true;
	}

	// Token: 0x06005BE9 RID: 23529 RVA: 0x0023D457 File Offset: 0x0023B657
	public static void StaticCleanup()
	{
		VoxelMeshLayer.InstanceCount = 0;
	}

	// Token: 0x06005BEA RID: 23530 RVA: 0x0023D460 File Offset: 0x0023B660
	[PublicizedFrom(EAccessModifier.Protected)]
	public ~VoxelMeshLayer()
	{
		if (!this.cleanedUp)
		{
			Log.Out(string.Format("VoxelMeshLayer {0} finalized without being cleaned up.", this.idx));
		}
	}

	// Token: 0x06005BEB RID: 23531 RVA: 0x0023D4AC File Offset: 0x0023B6AC
	public void SizeToChunkDefaults()
	{
		for (int i = 0; i < this.meshes.Length; i++)
		{
			this.meshes[i].SizeToChunkDefaults(i);
		}
	}

	// Token: 0x06005BEC RID: 23532 RVA: 0x0023D4DC File Offset: 0x0023B6DC
	public bool HasContent()
	{
		for (int i = 0; i < this.meshes.Length; i++)
		{
			if (this.meshes[i].Vertices.Count > 0)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06005BED RID: 23533 RVA: 0x0023D514 File Offset: 0x0023B714
	public int GetTris()
	{
		int num = 0;
		for (int i = 0; i < this.meshes.Length; i++)
		{
			num += this.meshes[i].Triangles;
		}
		return num;
	}

	// Token: 0x06005BEE RID: 23534 RVA: 0x0023D547 File Offset: 0x0023B747
	public int GetTrisInMesh(int _idx)
	{
		return this.meshes[_idx].Triangles;
	}

	// Token: 0x06005BEF RID: 23535 RVA: 0x0023D556 File Offset: 0x0023B756
	public int GetSizeOfMesh(int _idx)
	{
		return this.meshes[_idx].Size;
	}

	// Token: 0x06005BF0 RID: 23536 RVA: 0x0023D565 File Offset: 0x0023B765
	[PublicizedFrom(EAccessModifier.Internal)]
	public int CopyToMesh(int _meshIdx, MeshFilter _mf, MeshRenderer _mr, int _lodLevel)
	{
		this.pendingCopyOperations++;
		return this.meshes[_meshIdx].CopyToMesh(_mf, _mr, _lodLevel, delegate
		{
			this.TryFree();
		});
	}

	// Token: 0x06005BF1 RID: 23537 RVA: 0x0023D592 File Offset: 0x0023B792
	[PublicizedFrom(EAccessModifier.Internal)]
	public void StartCopyMeshes()
	{
		if (this.pendingCopyOperations != 0)
		{
			Log.Error("StartCopyMeshes called with pendingCopyOperations != 0");
		}
		this.pendingCopyOperations++;
	}

	// Token: 0x06005BF2 RID: 23538 RVA: 0x0023D5B4 File Offset: 0x0023B7B4
	[PublicizedFrom(EAccessModifier.Internal)]
	public void EndCopyMeshes()
	{
		this.TryFree();
	}

	// Token: 0x06005BF3 RID: 23539 RVA: 0x0023D5BD File Offset: 0x0023B7BD
	public bool TryFree()
	{
		this.pendingCopyOperations--;
		if (this.pendingCopyOperations > 0)
		{
			return false;
		}
		MemoryPools.poolVML.FreeSync(this);
		return true;
	}

	// Token: 0x040047A7 RID: 18343
	public int idx;

	// Token: 0x040047A8 RID: 18344
	public VoxelMesh[] meshes;

	// Token: 0x040047A9 RID: 18345
	public static int InstanceCount;

	// Token: 0x040047AA RID: 18346
	public int pendingCopyOperations;

	// Token: 0x040047AB RID: 18347
	[PublicizedFrom(EAccessModifier.Private)]
	public bool cleanedUp;
}
