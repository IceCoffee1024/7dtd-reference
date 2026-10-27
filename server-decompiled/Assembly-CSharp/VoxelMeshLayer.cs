using System;
using System.Threading;
using UnityEngine;

// Token: 0x02000C02 RID: 3074
public class VoxelMeshLayer : IMemoryPoolableObject
{
	// Token: 0x06005C0B RID: 23563 RVA: 0x0023E8DC File Offset: 0x0023CADC
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

	// Token: 0x06005C0C RID: 23564 RVA: 0x0023E940 File Offset: 0x0023CB40
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

	// Token: 0x06005C0D RID: 23565 RVA: 0x0023E986 File Offset: 0x0023CB86
	public void Cleanup()
	{
		if (this.cleanedUp)
		{
			return;
		}
		Interlocked.Decrement(ref VoxelMeshLayer.InstanceCount);
		this.cleanedUp = true;
	}

	// Token: 0x06005C0E RID: 23566 RVA: 0x0023E9A3 File Offset: 0x0023CBA3
	public static void StaticCleanup()
	{
		VoxelMeshLayer.InstanceCount = 0;
	}

	// Token: 0x06005C0F RID: 23567 RVA: 0x0023E9AC File Offset: 0x0023CBAC
	[PublicizedFrom(EAccessModifier.Protected)]
	public ~VoxelMeshLayer()
	{
		if (!this.cleanedUp)
		{
			Log.Out(string.Format("VoxelMeshLayer {0} finalized without being cleaned up.", this.idx));
		}
	}

	// Token: 0x06005C10 RID: 23568 RVA: 0x0023E9F8 File Offset: 0x0023CBF8
	public void SizeToChunkDefaults()
	{
		for (int i = 0; i < this.meshes.Length; i++)
		{
			this.meshes[i].SizeToChunkDefaults(i);
		}
	}

	// Token: 0x06005C11 RID: 23569 RVA: 0x0023EA28 File Offset: 0x0023CC28
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

	// Token: 0x06005C12 RID: 23570 RVA: 0x0023EA60 File Offset: 0x0023CC60
	public int GetTris()
	{
		int num = 0;
		for (int i = 0; i < this.meshes.Length; i++)
		{
			num += this.meshes[i].Triangles;
		}
		return num;
	}

	// Token: 0x06005C13 RID: 23571 RVA: 0x0023EA93 File Offset: 0x0023CC93
	public int GetTrisInMesh(int _idx)
	{
		return this.meshes[_idx].Triangles;
	}

	// Token: 0x06005C14 RID: 23572 RVA: 0x0023EAA2 File Offset: 0x0023CCA2
	public int GetSizeOfMesh(int _idx)
	{
		return this.meshes[_idx].Size;
	}

	// Token: 0x06005C15 RID: 23573 RVA: 0x0023EAB1 File Offset: 0x0023CCB1
	[PublicizedFrom(EAccessModifier.Internal)]
	public int CopyToMesh(int _meshIdx, MeshFilter _mf, MeshRenderer _mr, int _lodLevel)
	{
		this.pendingCopyOperations++;
		return this.meshes[_meshIdx].CopyToMesh(_mf, _mr, _lodLevel, delegate
		{
			this.TryFree();
		});
	}

	// Token: 0x06005C16 RID: 23574 RVA: 0x0023EADE File Offset: 0x0023CCDE
	[PublicizedFrom(EAccessModifier.Internal)]
	public void StartCopyMeshes()
	{
		if (this.pendingCopyOperations != 0)
		{
			Log.Error("StartCopyMeshes called with pendingCopyOperations != 0");
		}
		this.pendingCopyOperations++;
	}

	// Token: 0x06005C17 RID: 23575 RVA: 0x0023EB00 File Offset: 0x0023CD00
	[PublicizedFrom(EAccessModifier.Internal)]
	public void EndCopyMeshes()
	{
		this.TryFree();
	}

	// Token: 0x06005C18 RID: 23576 RVA: 0x0023EB09 File Offset: 0x0023CD09
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

	// Token: 0x040047CC RID: 18380
	public int idx;

	// Token: 0x040047CD RID: 18381
	public VoxelMesh[] meshes;

	// Token: 0x040047CE RID: 18382
	public static int InstanceCount;

	// Token: 0x040047CF RID: 18383
	public int pendingCopyOperations;

	// Token: 0x040047D0 RID: 18384
	[PublicizedFrom(EAccessModifier.Private)]
	public bool cleanedUp;
}
