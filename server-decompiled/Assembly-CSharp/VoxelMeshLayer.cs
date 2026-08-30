using System;
using System.Threading;
using UnityEngine;

// Token: 0x02000BE5 RID: 3045
public class VoxelMeshLayer : IMemoryPoolableObject
{
	// Token: 0x06005BCD RID: 23501 RVA: 0x0023A094 File Offset: 0x00238294
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

	// Token: 0x06005BCE RID: 23502 RVA: 0x0023A0F8 File Offset: 0x002382F8
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

	// Token: 0x06005BCF RID: 23503 RVA: 0x0023A13E File Offset: 0x0023833E
	public void Cleanup()
	{
		if (this.cleanedUp)
		{
			return;
		}
		Interlocked.Decrement(ref VoxelMeshLayer.InstanceCount);
		this.cleanedUp = true;
	}

	// Token: 0x06005BD0 RID: 23504 RVA: 0x0023A15B File Offset: 0x0023835B
	public static void StaticCleanup()
	{
		VoxelMeshLayer.InstanceCount = 0;
	}

	// Token: 0x06005BD1 RID: 23505 RVA: 0x0023A164 File Offset: 0x00238364
	[PublicizedFrom(EAccessModifier.Protected)]
	public ~VoxelMeshLayer()
	{
		if (!this.cleanedUp)
		{
			Log.Out(string.Format("VoxelMeshLayer {0} finalized without being cleaned up.", this.idx));
		}
	}

	// Token: 0x06005BD2 RID: 23506 RVA: 0x0023A1B0 File Offset: 0x002383B0
	public void SizeToChunkDefaults()
	{
		for (int i = 0; i < this.meshes.Length; i++)
		{
			this.meshes[i].SizeToChunkDefaults(i);
		}
	}

	// Token: 0x06005BD3 RID: 23507 RVA: 0x0023A1E0 File Offset: 0x002383E0
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

	// Token: 0x06005BD4 RID: 23508 RVA: 0x0023A218 File Offset: 0x00238418
	public int GetTris()
	{
		int num = 0;
		for (int i = 0; i < this.meshes.Length; i++)
		{
			num += this.meshes[i].Triangles;
		}
		return num;
	}

	// Token: 0x06005BD5 RID: 23509 RVA: 0x0023A24B File Offset: 0x0023844B
	public int GetTrisInMesh(int _idx)
	{
		return this.meshes[_idx].Triangles;
	}

	// Token: 0x06005BD6 RID: 23510 RVA: 0x0023A25A File Offset: 0x0023845A
	public int GetSizeOfMesh(int _idx)
	{
		return this.meshes[_idx].Size;
	}

	// Token: 0x06005BD7 RID: 23511 RVA: 0x0023A269 File Offset: 0x00238469
	[PublicizedFrom(EAccessModifier.Internal)]
	public int CopyToMesh(int _meshIdx, MeshFilter _mf, MeshRenderer _mr, int _lodLevel)
	{
		this.pendingCopyOperations++;
		return this.meshes[_meshIdx].CopyToMesh(_mf, _mr, _lodLevel, delegate
		{
			this.TryFree();
		});
	}

	// Token: 0x06005BD8 RID: 23512 RVA: 0x0023A296 File Offset: 0x00238496
	[PublicizedFrom(EAccessModifier.Internal)]
	public void StartCopyMeshes()
	{
		if (this.pendingCopyOperations != 0)
		{
			Log.Error("StartCopyMeshes called with pendingCopyOperations != 0");
		}
		this.pendingCopyOperations++;
	}

	// Token: 0x06005BD9 RID: 23513 RVA: 0x0023A2B8 File Offset: 0x002384B8
	[PublicizedFrom(EAccessModifier.Internal)]
	public void EndCopyMeshes()
	{
		this.TryFree();
	}

	// Token: 0x06005BDA RID: 23514 RVA: 0x0023A2C1 File Offset: 0x002384C1
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

	// Token: 0x04004768 RID: 18280
	public int idx;

	// Token: 0x04004769 RID: 18281
	public VoxelMesh[] meshes;

	// Token: 0x0400476A RID: 18282
	public static int InstanceCount;

	// Token: 0x0400476B RID: 18283
	public int pendingCopyOperations;

	// Token: 0x0400476C RID: 18284
	[PublicizedFrom(EAccessModifier.Private)]
	public bool cleanedUp;
}
