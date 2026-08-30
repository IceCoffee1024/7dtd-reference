using System;
using System.IO;
using UnityEngine;

// Token: 0x02000BE4 RID: 3044
public class VoxelMeshExt3dModel : VoxelMesh
{
	// Token: 0x06005BC5 RID: 23493 RVA: 0x00239DE4 File Offset: 0x00237FE4
	public VoxelMeshExt3dModel(int _meshIndex, int _minSize = 500)
		: base(_meshIndex, _minSize, VoxelMesh.CreateFlags.Default)
	{
	}

	// Token: 0x06005BC6 RID: 23494 RVA: 0x00239E44 File Offset: 0x00238044
	public override void Write(BinaryWriter _bw)
	{
		base.Write(_bw);
		_bw.Write((uint)this.aabb.Length);
		foreach (Bounds bounds in this.aabb)
		{
			BoundsUtils.WriteBounds(_bw, bounds);
		}
		this.boundingBoxMesh.Write(_bw);
		this.lod1Mesh.Write(_bw);
	}

	// Token: 0x06005BC7 RID: 23495 RVA: 0x00239EA4 File Offset: 0x002380A4
	public override void Read(BinaryReader _br)
	{
		base.Read(_br);
		uint num = _br.ReadUInt32();
		this.aabb = new Bounds[num];
		int num2 = 0;
		while ((long)num2 < (long)((ulong)num))
		{
			this.aabb[num2] = BoundsUtils.ReadBounds(_br);
			num2++;
		}
		this.boundingBoxMesh.ClearMesh();
		this.boundingBoxMesh.Read(_br);
		this.lod1Mesh.ClearMesh();
		this.lod1Mesh.Read(_br);
		this.lod1Mesh.ClearMesh();
	}

	// Token: 0x06005BC8 RID: 23496 RVA: 0x00239F24 File Offset: 0x00238124
	[PublicizedFrom(EAccessModifier.Private)]
	public void addColliders(Vector3 _drawPos, VoxelMesh _colliderMesh)
	{
		if (_colliderMesh.Vertices.Count == 0)
		{
			return;
		}
		int count = this.boundsVertices.Count;
		for (int i = 0; i < _colliderMesh.Vertices.Count; i++)
		{
			this.boundsVertices.Add(_drawPos + _colliderMesh.Vertices[i]);
		}
		for (int j = 0; j < _colliderMesh.Indices.Count; j++)
		{
			this.boundsTriangles.Add(count + _colliderMesh.Indices[j]);
		}
	}

	// Token: 0x06005BC9 RID: 23497 RVA: 0x00239FB0 File Offset: 0x002381B0
	public override void AddMesh(Vector3 _drawPos, int _count, Vector3[] _vertices, Vector3[] _normals, ArrayListMP<int> _indices, ArrayListMP<Vector2> _uvs, byte _sunlight, byte _blocklight, VoxelMesh _specialColliders, int damage)
	{
		if (_specialColliders != null)
		{
			this.addColliders(_drawPos, _specialColliders);
		}
		base.AddMesh(_drawPos, _count, _vertices, _normals, _indices, _uvs, _sunlight, _blocklight, null, damage);
	}

	// Token: 0x06005BCA RID: 23498 RVA: 0x00239FE0 File Offset: 0x002381E0
	public override int CopyToColliders(MeshCollider _meshCollider, out Mesh mesh)
	{
		if (this.boundsVertices == null || this.boundsVertices.Count == 0)
		{
			GameManager.Instance.World.m_ChunkManager.BakeDestroyCancel(_meshCollider);
			mesh = null;
			return 0;
		}
		mesh = base.ResetMesh(_meshCollider);
		MeshUnsafeCopyHelper.CopyVertices(this.boundsVertices, mesh);
		MeshUnsafeCopyHelper.CopyTriangles(this.boundsTriangles, mesh);
		_meshCollider.tag = "T_Mesh_B";
		return this.boundsTriangles.Count / 3;
	}

	// Token: 0x06005BCB RID: 23499 RVA: 0x0023A057 File Offset: 0x00238257
	public override void ClearMesh()
	{
		base.ClearMesh();
		this.lod1Mesh.ClearMesh();
		this.boundsVertices.Clear();
		this.boundsTriangles.Clear();
	}

	// Token: 0x06005BCC RID: 23500 RVA: 0x0023A080 File Offset: 0x00238280
	public override void Finished()
	{
		base.Finished();
		this.lod1Mesh.Finished();
	}

	// Token: 0x04004763 RID: 18275
	public VoxelMesh boundingBoxMesh = new VoxelMesh(-1, 0, VoxelMesh.CreateFlags.Default);

	// Token: 0x04004764 RID: 18276
	public Bounds[] aabb = new Bounds[0];

	// Token: 0x04004765 RID: 18277
	[PublicizedFrom(EAccessModifier.Private)]
	public ArrayListMP<Vector3> boundsVertices = new ArrayListMP<Vector3>(MemoryPools.poolVector3, 0);

	// Token: 0x04004766 RID: 18278
	[PublicizedFrom(EAccessModifier.Private)]
	public ArrayListMP<int> boundsTriangles = new ArrayListMP<int>(MemoryPools.poolInt, 0);

	// Token: 0x04004767 RID: 18279
	public VoxelMesh lod1Mesh = new VoxelMesh(-1, 0, VoxelMesh.CreateFlags.Default);
}
