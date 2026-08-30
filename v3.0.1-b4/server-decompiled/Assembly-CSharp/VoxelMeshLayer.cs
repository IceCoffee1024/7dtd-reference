using System;
using System.Threading;
using UnityEngine;

// Token: 0x02000BD3 RID: 3027
public class VoxelMeshLayer : IMemoryPoolableObject
{
	// Token: 0x06005B44 RID: 23364 RVA: 0x00237378 File Offset: 0x00235578
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

	// Token: 0x06005B45 RID: 23365 RVA: 0x002373DC File Offset: 0x002355DC
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

	// Token: 0x06005B46 RID: 23366 RVA: 0x00237422 File Offset: 0x00235622
	public void Cleanup()
	{
		if (this.cleanedUp)
		{
			return;
		}
		Interlocked.Decrement(ref VoxelMeshLayer.InstanceCount);
		this.cleanedUp = true;
	}

	// Token: 0x06005B47 RID: 23367 RVA: 0x0023743F File Offset: 0x0023563F
	public static void StaticCleanup()
	{
		VoxelMeshLayer.InstanceCount = 0;
	}

	// Token: 0x06005B48 RID: 23368 RVA: 0x00237448 File Offset: 0x00235648
	[PublicizedFrom(EAccessModifier.Protected)]
	public ~VoxelMeshLayer()
	{
		if (!this.cleanedUp)
		{
			Log.Out(string.Format("VoxelMeshLayer {0} finalized without being cleaned up.", this.idx));
		}
	}

	// Token: 0x06005B49 RID: 23369 RVA: 0x00237494 File Offset: 0x00235694
	public void SizeToChunkDefaults()
	{
		for (int i = 0; i < this.meshes.Length; i++)
		{
			this.meshes[i].SizeToChunkDefaults(i);
		}
	}

	// Token: 0x06005B4A RID: 23370 RVA: 0x002374C4 File Offset: 0x002356C4
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

	// Token: 0x06005B4B RID: 23371 RVA: 0x002374FC File Offset: 0x002356FC
	public int GetTris()
	{
		int num = 0;
		for (int i = 0; i < this.meshes.Length; i++)
		{
			num += this.meshes[i].Triangles;
		}
		return num;
	}

	// Token: 0x06005B4C RID: 23372 RVA: 0x0023752F File Offset: 0x0023572F
	public int GetTrisInMesh(int _idx)
	{
		return this.meshes[_idx].Triangles;
	}

	// Token: 0x06005B4D RID: 23373 RVA: 0x0023753E File Offset: 0x0023573E
	public int GetSizeOfMesh(int _idx)
	{
		return this.meshes[_idx].Size;
	}

	// Token: 0x06005B4E RID: 23374 RVA: 0x0023754D File Offset: 0x0023574D
	[PublicizedFrom(EAccessModifier.Internal)]
	public int CopyToMesh(int _meshIdx, MeshFilter _mf, MeshRenderer _mr, int _lodLevel)
	{
		this.pendingCopyOperations++;
		return this.meshes[_meshIdx].CopyToMesh(_mf, _mr, _lodLevel, delegate
		{
			this.TryFree();
		});
	}

	// Token: 0x06005B4F RID: 23375 RVA: 0x0023757A File Offset: 0x0023577A
	[PublicizedFrom(EAccessModifier.Internal)]
	public void StartCopyMeshes()
	{
		if (this.pendingCopyOperations != 0)
		{
			Log.Error("StartCopyMeshes called with pendingCopyOperations != 0");
		}
		this.pendingCopyOperations++;
	}

	// Token: 0x06005B50 RID: 23376 RVA: 0x0023759C File Offset: 0x0023579C
	[PublicizedFrom(EAccessModifier.Internal)]
	public void EndCopyMeshes()
	{
		this.TryFree();
	}

	// Token: 0x06005B51 RID: 23377 RVA: 0x002375A5 File Offset: 0x002357A5
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

	// Token: 0x040046CC RID: 18124
	public int idx;

	// Token: 0x040046CD RID: 18125
	public VoxelMesh[] meshes;

	// Token: 0x040046CE RID: 18126
	public static int InstanceCount;

	// Token: 0x040046CF RID: 18127
	public int pendingCopyOperations;

	// Token: 0x040046D0 RID: 18128
	[PublicizedFrom(EAccessModifier.Private)]
	public bool cleanedUp;
}
