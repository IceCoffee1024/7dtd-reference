using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02000BDF RID: 3039
public class VoxelMesh
{
	// Token: 0x06005B75 RID: 23413 RVA: 0x00235870 File Offset: 0x00233A70
	public VoxelMesh(int _meshIndex, int _minSize = 1024, VoxelMesh.CreateFlags _flags = VoxelMesh.CreateFlags.Default)
	{
		this.meshIndex = _meshIndex;
		this.m_Vertices = new ArrayListMP<Vector3>(MemoryPools.poolVector3, _minSize);
		this.m_Indices = new ArrayListMP<int>(MemoryPools.poolInt, _minSize);
		this.m_Uvs = new ArrayListMP<Vector2>(MemoryPools.poolVector2, _minSize);
		if ((_flags & VoxelMesh.CreateFlags.Cracks) > VoxelMesh.CreateFlags.None)
		{
			this.UvsCrack = new ArrayListMP<Vector2>(MemoryPools.poolVector2, _minSize);
		}
		this.m_Normals = new ArrayListMP<Vector3>(MemoryPools.poolVector3, _minSize);
		this.m_Tangents = new ArrayListMP<Vector4>(MemoryPools.poolVector4, _minSize);
		this.m_ColorVertices = new ArrayListMP<Color>(MemoryPools.poolColor, _minSize);
		if ((_flags & VoxelMesh.CreateFlags.Collider) > VoxelMesh.CreateFlags.None)
		{
			this.m_CollVertices = new ArrayListMP<Vector3>(MemoryPools.poolVector3, _minSize);
			this.m_CollIndices = new ArrayListMP<int>(MemoryPools.poolInt, _minSize);
		}
	}

	// Token: 0x06005B76 RID: 23414 RVA: 0x0023592F File Offset: 0x00233B2F
	public static VoxelMesh Create(int _meshIdx, VoxelMesh.EnumMeshType _meshType, int _minSize = 500)
	{
		if (_meshType == VoxelMesh.EnumMeshType.Terrain)
		{
			return new VoxelMeshTerrain(_meshIdx, _minSize);
		}
		if (_meshType == VoxelMesh.EnumMeshType.Decals)
		{
			return new VoxelMesh(_meshIdx, _minSize, VoxelMesh.CreateFlags.None);
		}
		return new VoxelMesh(_meshIdx, _minSize, VoxelMesh.CreateFlags.Default);
	}

	// Token: 0x06005B77 RID: 23415 RVA: 0x00235952 File Offset: 0x00233B52
	public void SetTemperature(float _temperature)
	{
		this.temperature = _temperature;
	}

	// Token: 0x06005B78 RID: 23416 RVA: 0x0023595C File Offset: 0x00233B5C
	public virtual void Write(BinaryWriter _bw)
	{
		_bw.Write((uint)this.m_Vertices.Count);
		for (int i = 0; i < this.m_Vertices.Count; i++)
		{
			_bw.Write(this.m_Vertices[i].x);
			_bw.Write(this.m_Vertices[i].y);
			_bw.Write(this.m_Vertices[i].z);
		}
		_bw.Write((uint)this.m_Normals.Count);
		for (int j = 0; j < this.m_Normals.Count; j++)
		{
			_bw.Write(this.m_Normals[j].x);
			_bw.Write(this.m_Normals[j].y);
			_bw.Write(this.m_Normals[j].z);
		}
		_bw.Write((uint)this.m_Uvs.Count);
		for (int k = 0; k < this.m_Uvs.Count; k++)
		{
			_bw.Write(this.m_Uvs[k].x);
			_bw.Write(this.m_Uvs[k].y);
		}
		ArrayListMP<Vector2> uvsCrack = this.UvsCrack;
		int num = ((uvsCrack != null) ? uvsCrack.Count : 0);
		_bw.Write((uint)num);
		for (int l = 0; l < num; l++)
		{
			_bw.Write(this.UvsCrack[l].x);
			_bw.Write(this.UvsCrack[l].y);
		}
		_bw.Write((uint)this.m_ColorVertices.Count);
		for (int m = 0; m < this.m_ColorVertices.Count; m++)
		{
			_bw.Write(this.m_ColorVertices[m].r);
			_bw.Write(this.m_ColorVertices[m].g);
			_bw.Write(this.m_ColorVertices[m].b);
			_bw.Write(this.m_ColorVertices[m].a);
		}
		_bw.Write((uint)this.m_Indices.Count);
		for (int n = 0; n < this.m_Indices.Count; n++)
		{
			_bw.Write(this.m_Indices[n]);
		}
	}

	// Token: 0x06005B79 RID: 23417 RVA: 0x00235BC8 File Offset: 0x00233DC8
	public virtual void Read(BinaryReader _br)
	{
		uint num = _br.ReadUInt32();
		this.m_Vertices.Clear();
		int num2 = this.m_Vertices.Alloc((int)num);
		int num3 = 0;
		while ((long)num3 < (long)((ulong)num))
		{
			this.m_Vertices[num2++] = new Vector3(_br.ReadSingle(), _br.ReadSingle(), _br.ReadSingle());
			num3++;
		}
		num = _br.ReadUInt32();
		this.m_Normals.Clear();
		num2 = this.m_Normals.Alloc((int)num);
		int num4 = 0;
		while ((long)num4 < (long)((ulong)num))
		{
			this.m_Normals[num2++] = new Vector3(_br.ReadSingle(), _br.ReadSingle(), _br.ReadSingle());
			num4++;
		}
		num = _br.ReadUInt32();
		this.m_Uvs.Clear();
		num2 = this.m_Uvs.Alloc((int)num);
		int num5 = 0;
		while ((long)num5 < (long)((ulong)num))
		{
			this.m_Uvs[num2++] = new Vector2(_br.ReadSingle(), _br.ReadSingle());
			num5++;
		}
		num = _br.ReadUInt32();
		ArrayListMP<Vector2> uvsCrack = this.UvsCrack;
		if (uvsCrack != null)
		{
			uvsCrack.Clear();
		}
		if (num > 0U)
		{
			num2 = this.UvsCrack.Alloc((int)num);
			int num6 = 0;
			while ((long)num6 < (long)((ulong)num))
			{
				this.UvsCrack[num2++] = new Vector2(_br.ReadSingle(), _br.ReadSingle());
				num6++;
			}
		}
		num = _br.ReadUInt32();
		this.m_ColorVertices.Clear();
		num2 = this.m_ColorVertices.Alloc((int)num);
		int num7 = 0;
		while ((long)num7 < (long)((ulong)num))
		{
			this.m_ColorVertices[num2++] = new Color(_br.ReadSingle(), _br.ReadSingle(), _br.ReadSingle(), _br.ReadSingle());
			num7++;
		}
		num = _br.ReadUInt32();
		this.m_Indices.Clear();
		num2 = this.m_Indices.Alloc((int)num);
		int num8 = 0;
		while ((long)num8 < (long)((ulong)num))
		{
			this.m_Indices[num2++] = _br.ReadInt32();
			num8++;
		}
		this.m_Tangents.Clear();
	}

	// Token: 0x06005B7A RID: 23418 RVA: 0x00235DDC File Offset: 0x00233FDC
	public static float BlockFaceToColor(BlockFace _blockFace)
	{
		switch (_blockFace)
		{
		case BlockFace.Top:
			return VoxelMesh.COLOR_TOP;
		case BlockFace.Bottom:
			return VoxelMesh.COLOR_BOTTOM;
		case BlockFace.North:
			return VoxelMesh.COLOR_NORTH;
		case BlockFace.West:
			return VoxelMesh.COLOR_WEST;
		case BlockFace.South:
			return VoxelMesh.COLOR_SOUTH;
		case BlockFace.East:
			return VoxelMesh.COLOR_EAST;
		default:
			return 1f;
		}
	}

	// Token: 0x06005B7B RID: 23419 RVA: 0x00235E32 File Offset: 0x00234032
	public static void CreateMeshFilter(int _meshIndex, int _yOffset, GameObject _gameObject, string _meshTag, bool _bAllowLOD, out MeshFilter _mf, out MeshRenderer _mr, out MeshCollider _mc)
	{
		VoxelMesh.CreateMeshFilter(_meshIndex, _yOffset, _gameObject, _meshTag, _bAllowLOD, out _mf, out _mr);
		VoxelMesh.CreateMeshCollider(_meshIndex, _gameObject, _meshTag, out _mc);
	}

	// Token: 0x06005B7C RID: 23420 RVA: 0x00235E50 File Offset: 0x00234050
	public static void CreateMeshFilter(int _meshIndex, int _yOffset, GameObject _gameObject, string _meshTag, bool _bAllowLOD, out MeshFilter _mf, out MeshRenderer _mr)
	{
		_mf = null;
		_mr = null;
		MeshDescription meshDescription = MeshDescription.meshes[_meshIndex];
		_gameObject.transform.localPosition = Vector3.zero;
		if (!string.IsNullOrEmpty(_meshTag))
		{
			_gameObject.tag = _meshTag;
		}
		if (meshDescription.materials != null)
		{
			_mf = _gameObject.GetComponent<MeshFilter>();
			if (_mf == null)
			{
				_mf = _gameObject.AddComponent<MeshFilter>();
			}
			_mr = _gameObject.GetComponent<MeshRenderer>();
			if (_mr == null)
			{
				_mr = _gameObject.AddComponent<MeshRenderer>();
			}
			if (_meshIndex != 5)
			{
				if (meshDescription.materials.Length > 1)
				{
					_mr.sharedMaterials = meshDescription.materials;
				}
				else
				{
					_mr.sharedMaterial = meshDescription.materials[0];
				}
			}
			_mr.receiveShadows = meshDescription.bReceiveShadows;
			_mr.shadowCastingMode = (meshDescription.bCastShadows ? ShadowCastingMode.On : ShadowCastingMode.Off);
		}
	}

	// Token: 0x06005B7D RID: 23421 RVA: 0x00235F24 File Offset: 0x00234124
	[PublicizedFrom(EAccessModifier.Private)]
	public static void CreateMeshCollider(int _meshIndex, GameObject _gameObject, string _meshTag, out MeshCollider _mc)
	{
		_mc = null;
		MeshDescription meshDescription = MeshDescription.meshes[_meshIndex];
		if (meshDescription.ColliderLayerName != null && meshDescription.ColliderLayerName.Length > 0)
		{
			GameObject gameObject = new GameObject();
			gameObject.name = _gameObject.name + "Collider";
			if (!string.IsNullOrEmpty(_meshTag))
			{
				gameObject.tag = _meshTag;
			}
			gameObject.transform.parent = _gameObject.transform.parent;
			_mc = gameObject.AddComponent<MeshCollider>();
		}
	}

	// Token: 0x06005B7E RID: 23422 RVA: 0x00235F9C File Offset: 0x0023419C
	public static Mesh GetPooledMesh()
	{
		int num = VoxelMesh.meshPool.Count;
		Mesh mesh;
		if (num > 0)
		{
			num--;
			mesh = VoxelMesh.meshPool[num];
			VoxelMesh.meshPool.RemoveAt(num);
		}
		else
		{
			mesh = new Mesh();
			mesh.name = "Pool";
		}
		return mesh;
	}

	// Token: 0x06005B7F RID: 23423 RVA: 0x00235FE7 File Offset: 0x002341E7
	public static void AddPooledMesh(Mesh mesh)
	{
		if (!mesh)
		{
			return;
		}
		if (VoxelMesh.meshPool.Count < 250)
		{
			mesh.Clear(false);
			VoxelMesh.meshPool.Add(mesh);
			return;
		}
		global::UnityEngine.Object.Destroy(mesh);
	}

	// Token: 0x06005B80 RID: 23424 RVA: 0x0023601C File Offset: 0x0023421C
	public virtual int CopyToMesh(MeshFilter _mf, MeshRenderer _mr, int _lodLevel, Action _onCopyComplete = null)
	{
		VoxelMesh.<>c__DisplayClass39_0 CS$<>8__locals1 = new VoxelMesh.<>c__DisplayClass39_0();
		CS$<>8__locals1._mf = _mf;
		CS$<>8__locals1._onCopyComplete = _onCopyComplete;
		CS$<>8__locals1.filterState = VoxelMesh.m_meshFilterStates.GetOrCreateValue(CS$<>8__locals1._mf);
		int count = this.m_Vertices.Count;
		int num;
		if (count == 0)
		{
			VoxelMesh.MeshFilterState filterState = CS$<>8__locals1.filterState;
			VoxelMesh.MeshFilterState filterState2 = CS$<>8__locals1.filterState;
			num = filterState2.NextGeneration;
			filterState2.NextGeneration = num + 1;
			filterState.AppliedGeneration = num;
			Mesh sharedMesh = CS$<>8__locals1._mf.sharedMesh;
			if (sharedMesh)
			{
				CS$<>8__locals1._mf.sharedMesh = null;
				VoxelMesh.AddPooledMesh(sharedMesh);
			}
			Action onCopyComplete = CS$<>8__locals1._onCopyComplete;
			if (onCopyComplete != null)
			{
				onCopyComplete();
			}
			return 0;
		}
		if (count != this.m_ColorVertices.Count)
		{
			Log.Error("ERROR: VM.mesh[{2}].Vertices.Count ({0}) != VM.mesh[{2}].ColorSides.Count ({1})", new object[]
			{
				count,
				this.m_ColorVertices.Count,
				this.meshIndex
			});
			Action onCopyComplete2 = CS$<>8__locals1._onCopyComplete;
			if (onCopyComplete2 != null)
			{
				onCopyComplete2();
			}
			return this.m_Triangles;
		}
		if (count != this.m_Uvs.Count)
		{
			Log.Error("ERROR: VM.mesh.chunkMesh[{2}].Vertices.Count ({0}) != VM.mesh[{2}].Uvs.Count ({1})", new object[]
			{
				count,
				this.m_Uvs.Count,
				this.meshIndex
			});
			Action onCopyComplete3 = CS$<>8__locals1._onCopyComplete;
			if (onCopyComplete3 != null)
			{
				onCopyComplete3();
			}
			return this.m_Triangles;
		}
		VoxelMesh.<>c__DisplayClass39_0 CS$<>8__locals2 = CS$<>8__locals1;
		VoxelMesh.MeshFilterState filterState3 = CS$<>8__locals1.filterState;
		num = filterState3.NextGeneration;
		filterState3.NextGeneration = num + 1;
		CS$<>8__locals2.generation = num;
		CS$<>8__locals1.newMesh = VoxelMesh.GetPooledMesh();
		this.copyToMesh(CS$<>8__locals1.newMesh, this.m_Vertices, this.m_Indices, this.m_Uvs, this.UvsCrack, this.m_Normals, this.m_Tangents, this.m_ColorVertices, delegate
		{
			if (!CS$<>8__locals1._mf || CS$<>8__locals1.generation <= CS$<>8__locals1.filterState.AppliedGeneration)
			{
				VoxelMesh.AddPooledMesh(CS$<>8__locals1.newMesh);
				Action onCopyComplete4 = CS$<>8__locals1._onCopyComplete;
				if (onCopyComplete4 == null)
				{
					return;
				}
				onCopyComplete4();
				return;
			}
			else
			{
				CS$<>8__locals1.filterState.AppliedGeneration = CS$<>8__locals1.generation;
				Mesh sharedMesh2 = CS$<>8__locals1._mf.sharedMesh;
				CS$<>8__locals1._mf.sharedMesh = CS$<>8__locals1.newMesh;
				VoxelMesh.AddPooledMesh(sharedMesh2);
				Action onCopyComplete5 = CS$<>8__locals1._onCopyComplete;
				if (onCopyComplete5 == null)
				{
					return;
				}
				onCopyComplete5();
				return;
			}
		});
		return this.m_Triangles;
	}

	// Token: 0x06005B81 RID: 23425 RVA: 0x002361EC File Offset: 0x002343EC
	[PublicizedFrom(EAccessModifier.Private)]
	public void copyToMesh(Mesh _mesh, ArrayListMP<Vector3> _vertices, ArrayListMP<int> _indices, ArrayListMP<Vector2> _uvs, ArrayListMP<Vector2> _uvCracks, ArrayListMP<Vector3> _normals, ArrayListMP<Vector4> _tangents, ArrayListMP<Color> _colorVertices, Action _onCopyComplete = null)
	{
		if (MeshDataManager.Enabled)
		{
			MeshDataManager.Instance.Add(_mesh, _vertices, _indices, _normals, _tangents, _colorVertices, _uvs, _uvCracks, PinnedBuffer.Ownership.Steal, true, true, false, false, _onCopyComplete);
			MeshDataManager.Instance.StartBatches();
			return;
		}
		MeshUnsafeCopyHelper.CopyVertices(_vertices, _mesh);
		if (_uvs.Count > 0)
		{
			MeshUnsafeCopyHelper.CopyUV(_uvs, _mesh);
			if (_uvCracks != null)
			{
				MeshUnsafeCopyHelper.CopyUV2(_uvCracks, _mesh);
			}
		}
		MeshUnsafeCopyHelper.CopyColors(_colorVertices, _mesh);
		MeshUnsafeCopyHelper.CopyTriangles(_indices, _mesh);
		if (_normals.Count == 0)
		{
			_mesh.RecalculateNormals();
		}
		else
		{
			if (_vertices.Count != _normals.Count)
			{
				Log.Error("ERROR: Vertices.Count ({0}) != Normals.Count ({1}), MeshIdx={2} TriIdx={3}", new object[] { _vertices.Count, _normals.Count, this.meshIndex, this.CurTriangleIndex });
			}
			MeshUnsafeCopyHelper.CopyNormals(_normals, _mesh);
		}
		if (_uvs.Count > 0)
		{
			if (_tangents.Count == 0)
			{
				Utils.CalculateMeshTangents(_vertices, _indices, _normals, _uvs, _tangents, _mesh, false);
			}
			if (_vertices.Count != _tangents.Count)
			{
				Log.Out("copyToMesh {0} verts #{1} != tangents #{2}, MeshIdx={3} TriIdx={4}", new object[] { _mesh.name, _vertices.Count, _tangents.Count, this.meshIndex, this.CurTriangleIndex });
			}
			else
			{
				MeshUnsafeCopyHelper.CopyTangents(_tangents, _mesh);
			}
		}
		_mesh.RecalculateUVDistributionMetrics(1E-09f);
		GameUtils.SetMeshVertexAttributes(_mesh, false);
		_mesh.UploadMeshData(false);
		if (_onCopyComplete != null)
		{
			_onCopyComplete();
		}
	}

	// Token: 0x06005B82 RID: 23426 RVA: 0x00236384 File Offset: 0x00234584
	public virtual int CopyToColliders(MeshCollider _meshCollider, out Mesh mesh)
	{
		if (this.m_CollIndices == null)
		{
			mesh = null;
			return 0;
		}
		if (this.m_CollIndices.Count == 0)
		{
			GameManager.Instance.World.m_ChunkManager.BakeDestroyCancel(_meshCollider);
			mesh = null;
			return 0;
		}
		mesh = this.ResetMesh(_meshCollider);
		MeshUnsafeCopyHelper.CopyVertices(this.m_CollVertices, mesh);
		MeshUnsafeCopyHelper.CopyTriangles(this.m_CollIndices, mesh);
		return this.m_CollIndices.Count / 3;
	}

	// Token: 0x06005B83 RID: 23427 RVA: 0x002363F8 File Offset: 0x002345F8
	public Mesh ResetMesh(MeshCollider _meshCollider)
	{
		Mesh mesh = GameManager.Instance.World.m_ChunkManager.BakeCancelAndGetMesh(_meshCollider);
		if (!mesh)
		{
			mesh = new Mesh();
		}
		else
		{
			mesh.Clear(false);
		}
		return mesh;
	}

	// Token: 0x06005B84 RID: 23428 RVA: 0x00236433 File Offset: 0x00234633
	public virtual void GetColorForTextureId(int _subMeshIdx, ref Transvoxel.BuildVertex _data)
	{
		_data.color = Color.black;
		_data.uv = Vector2.zero;
		_data.uv2 = Vector2.zero;
		_data.uv3 = Vector2.zero;
		_data.uv4 = Vector2.zero;
	}

	// Token: 0x06005B85 RID: 23429 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual int FindOrCreateSubMesh(int _t0, int _t1, int _t2)
	{
		return 0;
	}

	// Token: 0x06005B86 RID: 23430 RVA: 0x0023646C File Offset: 0x0023466C
	public virtual void AddIndices(int _i0, int _i1, int _i2, int _submesh)
	{
		this.m_Indices.Add(_i0);
		this.m_Indices.Add(_i1);
		this.m_Indices.Add(_i2);
	}

	// Token: 0x06005B87 RID: 23431 RVA: 0x00236494 File Offset: 0x00234694
	public virtual void ClearMesh()
	{
		this.CurTriangleIndex = 0;
		this.m_Vertices.Clear();
		this.m_Indices.Clear();
		this.m_Uvs.Clear();
		if (this.UvsCrack != null)
		{
			this.UvsCrack.Clear();
		}
		if (this.m_Uvs3 != null)
		{
			this.m_Uvs3.Clear();
		}
		if (this.m_Uvs4 != null)
		{
			this.m_Uvs4.Clear();
		}
		this.m_ColorVertices.Clear();
		this.m_Normals.Clear();
		this.m_Tangents.Clear();
		if (this.m_CollVertices != null)
		{
			this.m_CollVertices.Clear();
			this.m_CollIndices.Clear();
		}
	}

	// Token: 0x06005B88 RID: 23432 RVA: 0x00236544 File Offset: 0x00234744
	public virtual void SizeToChunkDefaults(int _meshIndex)
	{
		int num;
		int num2;
		int num3;
		VoxelMesh.GetDefaultSizes(_meshIndex, out num, out num2, out num3);
		this.m_Vertices.Grow(num);
		this.m_Indices.Grow(num2);
		this.m_Uvs.Grow(num);
		if (this.UvsCrack != null)
		{
			this.UvsCrack.Grow(num);
		}
		if (this.m_Uvs3 != null)
		{
			this.m_Uvs3.Grow(num);
			this.m_Uvs4.Grow(num);
		}
		this.m_ColorVertices.Grow(num);
		this.m_Normals.Grow(num);
		this.m_Tangents.Grow(num);
		if (this.m_CollVertices != null)
		{
			this.m_CollVertices.Grow(num3);
			this.m_CollIndices.Grow(num3);
		}
	}

	// Token: 0x06005B89 RID: 23433 RVA: 0x002365FC File Offset: 0x002347FC
	[PublicizedFrom(EAccessModifier.Private)]
	public static void GetDefaultSizes(int _meshIndex, out int _vertMax, out int _indexMax, out int _colliderMax)
	{
		switch (_meshIndex)
		{
		case 0:
			_vertMax = 65536;
			_indexMax = 131072;
			_colliderMax = 32768;
			return;
		case 1:
			_vertMax = 16384;
			_indexMax = 32768;
			_colliderMax = 32768;
			return;
		case 2:
			_vertMax = 4096;
			_indexMax = 8192;
			_colliderMax = 4096;
			return;
		case 3:
			_vertMax = 16384;
			_indexMax = 32768;
			_colliderMax = 8192;
			return;
		case 4:
			_vertMax = 128;
			_indexMax = 128;
			_colliderMax = 0;
			return;
		case 5:
			_vertMax = 1024;
			_indexMax = 1024;
			_colliderMax = 1024;
			return;
		default:
			_vertMax = 512;
			_indexMax = 1024;
			_colliderMax = 512;
			return;
		}
	}

	// Token: 0x06005B8A RID: 23434 RVA: 0x002366C1 File Offset: 0x002348C1
	public virtual void Finished()
	{
		this.m_Triangles = this.m_Indices.Count / 3;
	}

	// Token: 0x06005B8B RID: 23435 RVA: 0x002366D8 File Offset: 0x002348D8
	public virtual Color[] UpdateColors(byte _suncolor, byte _blockcolor)
	{
		for (int i = 0; i < this.m_ColorVertices.Count; i++)
		{
			this.m_ColorVertices[i] = Lighting.ToColor((int)_suncolor, (int)_blockcolor, 1f);
		}
		return this.m_ColorVertices.ToArray();
	}

	// Token: 0x06005B8C RID: 23436 RVA: 0x00236720 File Offset: 0x00234920
	public void AddBlockSide(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4, BlockValue _blockValue, BlockFace blockFace, Lighting _lighting, int meshIndex)
	{
		Color color = _lighting.ToColor();
		this.AddQuadWithCracks(v1, color, v2, color, v3, color, v4, color, _blockValue.Block.getUVRectFromSideAndMetadata(meshIndex, blockFace, v1, _blockValue), WorldConstants.MapDamageToUVRect(_blockValue), false);
	}

	// Token: 0x06005B8D RID: 23437 RVA: 0x00236760 File Offset: 0x00234960
	public void AddBlockSide(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4, BlockValue _blockValue, float colorBlockFace, BlockFace blockFace, byte sunlight, byte blocklight, int meshIndex)
	{
		Color color = Lighting.ToColor((int)sunlight, (int)blocklight, colorBlockFace);
		this.AddQuadWithCracks(v1, color, v2, color, v3, color, v4, color, _blockValue.Block.getUVRectFromSideAndMetadata(meshIndex, blockFace, v1, _blockValue), WorldConstants.MapDamageToUVRect(_blockValue), false);
	}

	// Token: 0x06005B8E RID: 23438 RVA: 0x002367A4 File Offset: 0x002349A4
	public void AddBlockSideTri(Vector3 v1, Vector3 v2, Vector3 v3, int _meshIdx, BlockValue _blockValue, float colorBlockFace, BlockFace blockFace, byte sunlight, byte blocklight)
	{
		Color color = Lighting.ToColor((int)sunlight, (int)blocklight, colorBlockFace);
		this.AddTriWithCracks(v1, color, v2, color, v3, color, WorldConstants.MapBlockToUVRect(_meshIdx, _blockValue, blockFace), WorldConstants.MapDamageToUVRect(_blockValue), false);
	}

	// Token: 0x06005B8F RID: 23439 RVA: 0x002367DC File Offset: 0x002349DC
	public void AddBlockSideTri(Vector3 v1, Color c1, Vector3 v2, Color c2, Vector3 v3, Color c3, Rect uv1, Rect uv2)
	{
		this.AddTriWithCracks(v1, c1, v2, c2, v3, c3, uv1, uv2, false);
	}

	// Token: 0x06005B90 RID: 23440 RVA: 0x00236800 File Offset: 0x00234A00
	[PublicizedFrom(EAccessModifier.Protected)]
	public void AddQuad(Vector3 _v0, Vector2 _uv0, Vector3 _v1, Vector2 _uv1, Vector3 _v2, Vector2 _uv3, Vector3 _v3, Vector2 _uv4, byte sunlight, byte blocklight, float sideColor)
	{
		if (this.m_Vertices.Count > 786428)
		{
			return;
		}
		this.m_Vertices.Add(_v0);
		this.m_Vertices.Add(_v1);
		this.m_Vertices.Add(_v2);
		this.m_Vertices.Add(_v3);
		int count = this.m_CollVertices.Count;
		this.m_CollVertices.Add(_v0);
		this.m_CollVertices.Add(_v1);
		this.m_CollVertices.Add(_v2);
		this.m_CollVertices.Add(_v3);
		this.m_Normals.Add(Vector3.Cross(_v3 - _v0, _v1 - _v0).normalized);
		this.m_Normals.Add(Vector3.Cross(_v0 - _v1, _v2 - _v1).normalized);
		this.m_Normals.Add(Vector3.Cross(_v1 - _v2, _v3 - _v2).normalized);
		this.m_Normals.Add(Vector3.Cross(_v2 - _v3, _v0 - _v3).normalized);
		Color color = Lighting.ToColor((int)sunlight, (int)blocklight, sideColor);
		color.a = this.temperature;
		this.m_ColorVertices.Add(color);
		this.m_ColorVertices.Add(color);
		this.m_ColorVertices.Add(color);
		this.m_ColorVertices.Add(color);
		this.m_Indices.Add(this.CurTriangleIndex);
		this.m_Indices.Add(this.CurTriangleIndex + 2);
		this.m_Indices.Add(this.CurTriangleIndex + 1);
		this.m_Indices.Add(this.CurTriangleIndex + 3);
		this.m_Indices.Add(this.CurTriangleIndex + 2);
		this.m_Indices.Add(this.CurTriangleIndex);
		this.m_CollIndices.Add(count);
		this.m_CollIndices.Add(count + 2);
		this.m_CollIndices.Add(count + 1);
		this.m_CollIndices.Add(count + 3);
		this.m_CollIndices.Add(count + 2);
		this.m_CollIndices.Add(count);
		this.m_Uvs.Add(_uv0);
		this.m_Uvs.Add(_uv1);
		this.m_Uvs.Add(_uv3);
		this.m_Uvs.Add(_uv4);
		this.UvsCrack.Add(Vector2.zero);
		this.UvsCrack.Add(Vector2.zero);
		this.UvsCrack.Add(Vector2.zero);
		this.UvsCrack.Add(Vector2.zero);
		this.CurTriangleIndex += 4;
	}

	// Token: 0x06005B91 RID: 23441 RVA: 0x00236AB8 File Offset: 0x00234CB8
	public void CreateFromQuadList(Vector3[] _vertices, Color _color)
	{
		this.m_Vertices.Add(_vertices[0]);
		this.m_Vertices.Add(_vertices[0] + new Vector3(1f, 0f, 0f));
		this.m_Vertices.Add(_vertices[0] + new Vector3(1f, 0f, 1f));
		this.m_Vertices.Add(_vertices[0] + new Vector3(0f, 0f, 1f));
		this.m_Vertices.Add(_vertices[0] + new Vector3(0.5f, 0.15f, 0.5f));
		this.m_CollVertices.Add(_vertices[0]);
		this.m_CollVertices.Add(_vertices[0] + new Vector3(1f, 0f, 0f));
		this.m_CollVertices.Add(_vertices[0] + new Vector3(1f, 0f, 1f));
		this.m_CollVertices.Add(_vertices[0] + new Vector3(0f, 0f, 1f));
		this.m_CollVertices.Add(_vertices[0] + new Vector3(0.5f, 0.15f, 0.5f));
		this.m_ColorVertices.Add(_color);
		this.m_ColorVertices.Add(_color);
		this.m_ColorVertices.Add(_color);
		this.m_ColorVertices.Add(_color);
		this.m_ColorVertices.Add(_color);
		this.m_Normals.Add(new Vector3(0f, 1f, 0f));
		this.m_Normals.Add(new Vector3(0f, 1f, 0f));
		this.m_Normals.Add(new Vector3(0f, 1f, 0f));
		this.m_Normals.Add(new Vector3(0f, 1f, 0f));
		this.m_Normals.Add(new Vector3(0f, 1f, 0f));
		this.Uvs.Add(Vector2.zero);
		this.Uvs.Add(Vector2.zero);
		this.Uvs.Add(Vector2.zero);
		this.Uvs.Add(Vector2.zero);
		this.Uvs.Add(Vector2.zero);
		this.UvsCrack.Add(Vector2.zero);
		this.UvsCrack.Add(Vector2.zero);
		this.UvsCrack.Add(Vector2.zero);
		this.UvsCrack.Add(Vector2.zero);
		this.UvsCrack.Add(Vector2.zero);
		this.m_Indices.Add(this.CurTriangleIndex + 4);
		this.m_Indices.Add(this.CurTriangleIndex);
		this.m_Indices.Add(this.CurTriangleIndex + 3);
		this.m_CollIndices.Add(this.CurTriangleIndex + 4);
		this.m_CollIndices.Add(this.CurTriangleIndex);
		this.m_CollIndices.Add(this.CurTriangleIndex + 3);
		this.m_Indices.Add(this.CurTriangleIndex + 4);
		this.m_Indices.Add(this.CurTriangleIndex + 1);
		this.m_Indices.Add(this.CurTriangleIndex);
		this.m_CollIndices.Add(this.CurTriangleIndex + 4);
		this.m_CollIndices.Add(this.CurTriangleIndex + 1);
		this.m_CollIndices.Add(this.CurTriangleIndex);
		this.m_Indices.Add(this.CurTriangleIndex + 4);
		this.m_Indices.Add(this.CurTriangleIndex + 2);
		this.m_Indices.Add(this.CurTriangleIndex + 1);
		this.m_CollIndices.Add(this.CurTriangleIndex + 4);
		this.m_CollIndices.Add(this.CurTriangleIndex + 2);
		this.m_CollIndices.Add(this.CurTriangleIndex + 1);
		this.m_Indices.Add(this.CurTriangleIndex + 4);
		this.m_Indices.Add(this.CurTriangleIndex + 3);
		this.m_Indices.Add(this.CurTriangleIndex + 2);
		this.m_CollIndices.Add(this.CurTriangleIndex + 4);
		this.m_CollIndices.Add(this.CurTriangleIndex + 3);
		this.m_CollIndices.Add(this.CurTriangleIndex + 2);
		this.CurTriangleIndex += 5;
	}

	// Token: 0x06005B92 RID: 23442 RVA: 0x00236F88 File Offset: 0x00235188
	public void AddBasicQuad(Vector3[] _vertices, Color _color, Vector2 _UVdata, bool bForceNormalsUp = false, bool bAlternateWinding = false)
	{
		if (this.m_Vertices.Count > 786428)
		{
			return;
		}
		if (_vertices.Length != 4)
		{
			return;
		}
		Vector3 vector = _vertices[0];
		Vector3 vector2 = _vertices[1];
		Vector3 vector3 = _vertices[2];
		Vector3 vector4 = _vertices[3];
		int num = this.m_Vertices.Alloc(4);
		this.m_Vertices[num] = vector;
		this.m_Vertices[num + 1] = vector2;
		this.m_Vertices[num + 2] = vector3;
		this.m_Vertices[num + 3] = vector4;
		int num2 = this.m_CollVertices.Alloc(4);
		this.m_CollVertices[num] = vector;
		this.m_CollVertices[num + 1] = vector2;
		this.m_CollVertices[num + 2] = vector3;
		this.m_CollVertices[num + 3] = vector4;
		num = this.m_Normals.Alloc(4);
		if (bForceNormalsUp)
		{
			Vector3 up = Vector3.up;
			this.m_Normals[num] = up;
			this.m_Normals[num + 1] = up;
			this.m_Normals[num + 2] = up;
			this.m_Normals[num + 3] = up;
		}
		else
		{
			this.m_Normals[num] = Vector3.Cross(vector4 - vector, vector2 - vector).normalized;
			this.m_Normals[num + 1] = Vector3.Cross(vector - vector2, vector3 - vector2).normalized;
			this.m_Normals[num + 2] = Vector3.Cross(vector2 - vector3, vector4 - vector3).normalized;
			this.m_Normals[num + 3] = Vector3.Cross(vector3 - vector4, vector - vector4).normalized;
		}
		num = this.m_ColorVertices.Alloc(4);
		this.m_ColorVertices[num] = _color;
		this.m_ColorVertices[num + 1] = _color;
		this.m_ColorVertices[num + 2] = _color;
		this.m_ColorVertices[num + 3] = _color;
		num = this.m_Indices.Alloc(6);
		this.m_Indices[num] = (bAlternateWinding ? (this.CurTriangleIndex + 3) : this.CurTriangleIndex);
		this.m_Indices[num + 1] = this.CurTriangleIndex + 2;
		this.m_Indices[num + 2] = this.CurTriangleIndex + 1;
		this.m_Indices[num + 3] = this.CurTriangleIndex + 3;
		this.m_Indices[num + 4] = (bAlternateWinding ? (this.CurTriangleIndex + 1) : (this.CurTriangleIndex + 2));
		this.m_Indices[num + 5] = this.CurTriangleIndex;
		num = this.m_CollIndices.Alloc(6);
		this.m_CollIndices[num] = num2;
		this.m_CollIndices[num + 1] = num2 + 2;
		this.m_CollIndices[num + 2] = num2 + 1;
		this.m_CollIndices[num + 3] = num2 + 3;
		this.m_CollIndices[num + 4] = num2 + 2;
		this.m_CollIndices[num + 5] = num2;
		num = this.m_Uvs.Alloc(4);
		this.m_Uvs.Items[num].x = 0f;
		this.m_Uvs.Items[num].y = 1f;
		this.m_Uvs.Items[++num].x = 1f;
		this.m_Uvs.Items[num].y = 1f;
		this.m_Uvs.Items[++num].x = 1f;
		this.m_Uvs.Items[num].y = 0f;
		this.m_Uvs.Items[++num].x = 0f;
		this.m_Uvs.Items[num].y = 0f;
		num = this.UvsCrack.Alloc(4);
		this.UvsCrack[num] = _UVdata;
		this.UvsCrack[num + 1] = _UVdata;
		this.UvsCrack[num + 2] = _UVdata;
		this.UvsCrack[num + 3] = _UVdata;
		this.CurTriangleIndex += 4;
	}

	// Token: 0x06005B93 RID: 23443 RVA: 0x0023743C File Offset: 0x0023563C
	public void AddQuadNoCollision(Vector3 _v0, Vector3 _v1, Vector3 _v2, Vector3 _v3, Color _color, Rect _uvTex)
	{
		if (this.m_Vertices.Count > 786428)
		{
			return;
		}
		int num = this.m_Vertices.Alloc(4);
		this.m_Vertices[num] = _v0;
		this.m_Vertices[num + 1] = _v1;
		this.m_Vertices[num + 2] = _v2;
		this.m_Vertices[num + 3] = _v3;
		num = this.m_Normals.Alloc(4);
		this.m_Normals[num] = Vector3.Cross(_v3 - _v0, _v1 - _v0).normalized;
		this.m_Normals[num + 1] = Vector3.Cross(_v0 - _v1, _v2 - _v1).normalized;
		this.m_Normals[num + 2] = Vector3.Cross(_v1 - _v2, _v3 - _v2).normalized;
		this.m_Normals[num + 3] = Vector3.Cross(_v2 - _v3, _v0 - _v3).normalized;
		_color.a = this.temperature;
		num = this.m_ColorVertices.Alloc(4);
		this.m_ColorVertices[num] = _color;
		this.m_ColorVertices[num + 1] = _color;
		this.m_ColorVertices[num + 2] = _color;
		this.m_ColorVertices[num + 3] = _color;
		num = this.m_Indices.Alloc(6);
		this.m_Indices[num] = this.CurTriangleIndex;
		this.m_Indices[num + 1] = this.CurTriangleIndex + 2;
		this.m_Indices[num + 2] = this.CurTriangleIndex + 1;
		this.m_Indices[num + 3] = this.CurTriangleIndex + 3;
		this.m_Indices[num + 4] = this.CurTriangleIndex + 2;
		this.m_Indices[num + 5] = this.CurTriangleIndex;
		num = this.m_Uvs.Alloc(4);
		float x = _uvTex.x;
		float y = _uvTex.y;
		float width = _uvTex.width;
		float height = _uvTex.height;
		this.m_Uvs.Items[num].x = x;
		this.m_Uvs.Items[num].y = y;
		this.m_Uvs.Items[++num].x = x + width;
		this.m_Uvs.Items[num].y = y;
		this.m_Uvs.Items[++num].x = x + width;
		this.m_Uvs.Items[num].y = y + height;
		this.m_Uvs.Items[++num].x = x;
		this.m_Uvs.Items[num].y = y + height;
		this.CurTriangleIndex += 4;
	}

	// Token: 0x06005B94 RID: 23444 RVA: 0x00237748 File Offset: 0x00235948
	public void AddQuadWithCracks(Vector3 _v0, Color _c0, Vector3 _v1, Color _c1, Vector3 _v2, Color _c2, Vector3 _v3, Color _c3, Rect _uvTex, Rect _uvOverlay, bool bSwitchUvHorizontal)
	{
		if (this.m_Vertices.Count > 786428)
		{
			return;
		}
		int num = this.m_Vertices.Alloc(4);
		this.m_Vertices[num] = _v0;
		this.m_Vertices[num + 1] = _v1;
		this.m_Vertices[num + 2] = _v2;
		this.m_Vertices[num + 3] = _v3;
		int count = this.m_CollVertices.Count;
		this.m_CollVertices.Add(_v0);
		this.m_CollVertices.Add(_v1);
		this.m_CollVertices.Add(_v2);
		this.m_CollVertices.Add(_v3);
		num = this.m_Normals.Alloc(4);
		this.m_Normals[num] = Vector3.Cross(_v3 - _v0, _v1 - _v0).normalized;
		this.m_Normals[num + 1] = Vector3.Cross(_v0 - _v1, _v2 - _v1).normalized;
		this.m_Normals[num + 2] = Vector3.Cross(_v1 - _v2, _v3 - _v2).normalized;
		this.m_Normals[num + 3] = Vector3.Cross(_v2 - _v3, _v0 - _v3).normalized;
		float num2 = this.temperature;
		_c0.a = num2;
		_c1.a = num2;
		_c2.a = num2;
		_c3.a = num2;
		num = this.m_ColorVertices.Alloc(4);
		this.m_ColorVertices[num] = _c0;
		this.m_ColorVertices[num + 1] = _c1;
		this.m_ColorVertices[num + 2] = _c2;
		this.m_ColorVertices[num + 3] = _c3;
		num = this.m_Indices.Alloc(6);
		this.m_Indices[num] = this.CurTriangleIndex;
		this.m_Indices[num + 1] = this.CurTriangleIndex + 2;
		this.m_Indices[num + 2] = this.CurTriangleIndex + 1;
		this.m_Indices[num + 3] = this.CurTriangleIndex + 3;
		this.m_Indices[num + 4] = this.CurTriangleIndex + 2;
		this.m_Indices[num + 5] = this.CurTriangleIndex;
		this.m_CollIndices.Add(count);
		this.m_CollIndices.Add(count + 2);
		this.m_CollIndices.Add(count + 1);
		this.m_CollIndices.Add(count + 3);
		this.m_CollIndices.Add(count + 2);
		this.m_CollIndices.Add(count);
		num = this.m_Uvs.Alloc(4);
		float num3 = _uvTex.x;
		float num4 = _uvTex.y;
		float num5 = _uvTex.width;
		float num6 = _uvTex.height;
		if (!bSwitchUvHorizontal)
		{
			this.m_Uvs.Items[num].x = num3;
			this.m_Uvs.Items[num].y = num4;
			this.m_Uvs.Items[++num].x = num3 + num5;
			this.m_Uvs.Items[num].y = num4;
			this.m_Uvs.Items[++num].x = num3 + num5;
			this.m_Uvs.Items[num].y = num4 + num6;
			this.m_Uvs.Items[++num].x = num3;
			this.m_Uvs.Items[num].y = num4 + num6;
		}
		else
		{
			this.m_Uvs.Items[num].x = num3 + num5;
			this.m_Uvs.Items[num].y = num4;
			this.m_Uvs.Items[++num].x = num3;
			this.m_Uvs.Items[num].y = num4;
			this.m_Uvs.Items[++num].x = num3;
			this.m_Uvs.Items[num].y = num4 + num6;
			this.m_Uvs.Items[++num].x = num3 + num5;
			this.m_Uvs.Items[num].y = num4 + num6;
		}
		num = this.UvsCrack.Alloc(4);
		num3 = _uvOverlay.x;
		num4 = _uvOverlay.y;
		num5 = _uvOverlay.width;
		num6 = _uvOverlay.height;
		this.UvsCrack.Items[num].x = num3;
		this.UvsCrack.Items[num].y = num4;
		this.UvsCrack.Items[++num].x = num3;
		this.UvsCrack.Items[num].y = num4 + num6;
		this.UvsCrack.Items[++num].x = num3 + num5;
		this.UvsCrack.Items[num].y = num4 + num6;
		this.UvsCrack.Items[++num].x = num3 + num5;
		this.UvsCrack.Items[num].y = num4;
		this.CurTriangleIndex += 4;
	}

	// Token: 0x06005B95 RID: 23445 RVA: 0x00237CEC File Offset: 0x00235EEC
	public void AddRectangle(Vector3 _v0, Vector2 _uv0, Color _c0, Vector3 _v1, Vector2 _uv1, Color _c1, Vector3 _v2, Vector2 _uv2, Color _c2, Vector3 _v3, Vector2 _uv3, Color _c3, Rect _uvTex, Rect _uvOverlay)
	{
		if (this.m_Vertices.Count > 786428)
		{
			return;
		}
		this.m_Vertices.Add(_v0);
		this.m_Vertices.Add(_v1);
		this.m_Vertices.Add(_v2);
		this.m_Vertices.Add(_v3);
		int count = this.m_CollVertices.Count;
		this.m_CollVertices.Add(_v0);
		this.m_CollVertices.Add(_v1);
		this.m_CollVertices.Add(_v2);
		this.m_CollVertices.Add(_v3);
		this.m_Normals.Add(Vector3.Cross(_v3 - _v0, _v1 - _v0).normalized);
		this.m_Normals.Add(Vector3.Cross(_v0 - _v1, _v2 - _v1).normalized);
		this.m_Normals.Add(Vector3.Cross(_v1 - _v2, _v3 - _v2).normalized);
		this.m_Normals.Add(Vector3.Cross(_v2 - _v3, _v0 - _v3).normalized);
		float num = this.temperature;
		_c0.a = num;
		_c1.a = num;
		_c2.a = num;
		_c3.a = num;
		this.m_ColorVertices.Add(_c0);
		this.m_ColorVertices.Add(_c1);
		this.m_ColorVertices.Add(_c2);
		this.m_ColorVertices.Add(_c3);
		this.m_Indices.Add(this.CurTriangleIndex);
		this.m_Indices.Add(this.CurTriangleIndex + 2);
		this.m_Indices.Add(this.CurTriangleIndex + 1);
		this.m_Indices.Add(this.CurTriangleIndex + 3);
		this.m_Indices.Add(this.CurTriangleIndex + 2);
		this.m_Indices.Add(this.CurTriangleIndex);
		this.m_CollIndices.Add(count);
		this.m_CollIndices.Add(count + 2);
		this.m_CollIndices.Add(count + 1);
		this.m_CollIndices.Add(count + 3);
		this.m_CollIndices.Add(count + 2);
		this.m_CollIndices.Add(count);
		this.m_Uvs.Add(new Vector2(_uvTex.x + _uv0.x * _uvTex.width, _uvTex.y + _uv0.y * _uvTex.height));
		this.m_Uvs.Add(new Vector2(_uvTex.x + _uv1.x * _uvTex.width, _uvTex.y + _uv1.y * _uvTex.height));
		this.m_Uvs.Add(new Vector2(_uvTex.x + _uv2.x * _uvTex.width, _uvTex.y + _uv2.y * _uvTex.height));
		this.m_Uvs.Add(new Vector2(_uvTex.x + _uv3.x * _uvTex.width, _uvTex.y + _uv3.y * _uvTex.height));
		this.UvsCrack.Add(new Vector2(_uvOverlay.x + _uv0.x * _uvOverlay.width, _uvOverlay.y + _uv0.y * _uvOverlay.height));
		this.UvsCrack.Add(new Vector2(_uvOverlay.x + _uv1.x * _uvOverlay.width, _uvOverlay.y + _uv1.y * _uvOverlay.height));
		this.UvsCrack.Add(new Vector2(_uvOverlay.x + _uv2.x * _uvOverlay.width, _uvOverlay.y + _uv2.y * _uvOverlay.height));
		this.UvsCrack.Add(new Vector2(_uvOverlay.x + _uv3.x * _uvOverlay.width, _uvOverlay.y + _uv3.y * _uvOverlay.height));
		this.CurTriangleIndex += 4;
	}

	// Token: 0x06005B96 RID: 23446 RVA: 0x00238134 File Offset: 0x00236334
	public void AddRectangle(Vector3 _v0, Vector2 _uv0, Vector3 _v1, Vector2 _uv1, Vector3 _v2, Vector2 _uv2, Vector3 _v3, Vector2 _uv3, Vector3 _normal, Vector3 _normalTop, Vector4 _tangent, Rect _uvTex, byte _sunlight, byte _blocklight)
	{
		if (this.m_Vertices.Count > 786428)
		{
			return;
		}
		int num = this.m_Vertices.Alloc(4);
		this.m_Vertices[num] = _v0;
		this.m_Vertices[num + 1] = _v1;
		this.m_Vertices[num + 2] = _v2;
		this.m_Vertices[num + 3] = _v3;
		num = this.m_Normals.Alloc(4);
		this.m_Normals[num] = _normal;
		this.m_Normals[num + 1] = _normal;
		this.m_Normals[num + 2] = _normalTop;
		this.m_Normals[num + 3] = _normalTop;
		num = this.m_Tangents.Alloc(4);
		this.m_Tangents[num] = _tangent;
		this.m_Tangents[num + 1] = _tangent;
		this.m_Tangents[num + 2] = _tangent;
		this.m_Tangents[num + 3] = _tangent;
		Color color = Lighting.ToColor((int)_sunlight, (int)_blocklight);
		color.a = this.temperature;
		num = this.m_ColorVertices.Alloc(4);
		this.m_ColorVertices[num] = color;
		this.m_ColorVertices[num + 1] = color;
		this.m_ColorVertices[num + 2] = color;
		this.m_ColorVertices[num + 3] = color;
		num = this.m_Indices.Alloc(6);
		this.m_Indices[num] = this.CurTriangleIndex;
		this.m_Indices[num + 1] = this.CurTriangleIndex + 2;
		this.m_Indices[num + 2] = this.CurTriangleIndex + 1;
		this.m_Indices[num + 3] = this.CurTriangleIndex + 3;
		this.m_Indices[num + 4] = this.CurTriangleIndex + 2;
		this.m_Indices[num + 5] = this.CurTriangleIndex;
		num = this.m_Uvs.Alloc(4);
		float x = _uvTex.x;
		float y = _uvTex.y;
		float width = _uvTex.width;
		float height = _uvTex.height;
		this.m_Uvs.Items[num].x = x + _uv0.x * width;
		this.m_Uvs.Items[num].y = y + _uv0.y * height;
		this.m_Uvs.Items[++num].x = x + _uv1.x * width;
		this.m_Uvs.Items[num].y = y + _uv1.y * height;
		this.m_Uvs.Items[++num].x = x + _uv2.x * width;
		this.m_Uvs.Items[num].y = y + _uv2.y * height;
		this.m_Uvs.Items[++num].x = x + _uv3.x * width;
		this.m_Uvs.Items[num].y = y + _uv3.y * height;
		num = this.UvsCrack.Alloc(4);
		this.UvsCrack[num] = Vector2.zero;
		this.UvsCrack[num + 1] = Vector2.zero;
		this.UvsCrack[num + 2] = Vector2.zero;
		this.UvsCrack[num + 3] = Vector2.zero;
		this.CurTriangleIndex += 4;
	}

	// Token: 0x06005B97 RID: 23447 RVA: 0x002384CC File Offset: 0x002366CC
	public void AddRectangleColliderPair(Vector3 _v0, Vector3 _v1, Vector3 _v2, Vector3 _v3)
	{
		if (this.m_CollVertices.Count > 786428)
		{
			return;
		}
		int num = this.m_CollVertices.Alloc(4);
		this.m_CollVertices[num] = _v0;
		this.m_CollVertices[num + 1] = _v1;
		this.m_CollVertices[num + 2] = _v2;
		this.m_CollVertices[num + 3] = _v3;
		int num2 = this.m_CollIndices.Alloc(12);
		this.m_CollIndices[num2] = num;
		this.m_CollIndices[num2 + 1] = num + 1;
		this.m_CollIndices[num2 + 2] = num + 2;
		this.m_CollIndices[num2 + 3] = num;
		this.m_CollIndices[num2 + 4] = num + 2;
		this.m_CollIndices[num2 + 5] = num + 3;
		this.m_CollIndices[num2 + 6] = num;
		this.m_CollIndices[num2 + 7] = num + 2;
		this.m_CollIndices[num2 + 8] = num + 1;
		this.m_CollIndices[num2 + 9] = num + 3;
		this.m_CollIndices[num2 + 10] = num + 2;
		this.m_CollIndices[num2 + 11] = num;
	}

	// Token: 0x06005B98 RID: 23448 RVA: 0x00238608 File Offset: 0x00236808
	public void AddColoredRectangle(Vector3 _v0, Color _c0, Vector3 _v1, Color _c1, Vector3 _v2, Color _c2, Vector3 _v3, Color _c3)
	{
		if (this.m_Vertices.Count > 786428)
		{
			return;
		}
		this.m_Vertices.Add(_v0);
		this.m_Vertices.Add(_v1);
		this.m_Vertices.Add(_v2);
		this.m_Vertices.Add(_v3);
		int count = this.m_CollVertices.Count;
		this.m_CollVertices.Add(_v0);
		this.m_CollVertices.Add(_v1);
		this.m_CollVertices.Add(_v2);
		this.m_CollVertices.Add(_v3);
		this.m_Normals.Add(Vector3.Cross(_v3 - _v0, _v1 - _v0).normalized);
		this.m_Normals.Add(Vector3.Cross(_v0 - _v1, _v2 - _v1).normalized);
		this.m_Normals.Add(Vector3.Cross(_v1 - _v2, _v3 - _v2).normalized);
		this.m_Normals.Add(Vector3.Cross(_v2 - _v3, _v0 - _v3).normalized);
		float num = this.temperature;
		_c0.a = num;
		_c1.a = num;
		_c2.a = num;
		_c3.a = num;
		this.m_ColorVertices.Add(_c0);
		this.m_ColorVertices.Add(_c1);
		this.m_ColorVertices.Add(_c2);
		this.m_ColorVertices.Add(_c3);
		this.m_Indices.Add(this.CurTriangleIndex);
		this.m_Indices.Add(this.CurTriangleIndex + 2);
		this.m_Indices.Add(this.CurTriangleIndex + 1);
		this.m_Indices.Add(this.CurTriangleIndex + 3);
		this.m_Indices.Add(this.CurTriangleIndex + 2);
		this.m_Indices.Add(this.CurTriangleIndex);
		this.m_CollIndices.Add(count);
		this.m_CollIndices.Add(count + 2);
		this.m_CollIndices.Add(count + 1);
		this.m_CollIndices.Add(count + 3);
		this.m_CollIndices.Add(count + 2);
		this.m_CollIndices.Add(count);
		this.CurTriangleIndex += 4;
	}

	// Token: 0x06005B99 RID: 23449 RVA: 0x00238860 File Offset: 0x00236A60
	public void AddTriWithCracks(Vector3 _v0, Color _c0, Vector3 _v1, Color _c1, Vector3 _v2, Color _c2, Rect uvTex, Rect uvOverlay, bool bSwitchUvHorizontal)
	{
		if (this.m_Vertices.Count > 786429)
		{
			return;
		}
		this.m_Vertices.Add(_v0);
		this.m_Vertices.Add(_v1);
		this.m_Vertices.Add(_v2);
		int count = this.m_CollVertices.Count;
		this.m_CollVertices.Add(_v0);
		this.m_CollVertices.Add(_v1);
		this.m_CollVertices.Add(_v2);
		this.m_Normals.Add(Vector3.Cross(_v2 - _v0, _v1 - _v0).normalized);
		this.m_Normals.Add(Vector3.Cross(_v0 - _v1, _v2 - _v1).normalized);
		this.m_Normals.Add(Vector3.Cross(_v1 - _v2, _v0 - _v2).normalized);
		float num = this.temperature;
		_c0.a = num;
		_c1.a = num;
		_c2.a = num;
		this.ColorVertices.Add(_c0);
		this.ColorVertices.Add(_c1);
		this.ColorVertices.Add(_c2);
		this.m_Indices.Add(this.CurTriangleIndex);
		this.m_Indices.Add(this.CurTriangleIndex + 2);
		this.m_Indices.Add(this.CurTriangleIndex + 1);
		this.m_CollIndices.Add(count);
		this.m_CollIndices.Add(count + 2);
		this.m_CollIndices.Add(count + 1);
		this.Uvs.Add(new Vector2(uvTex.x + 0f, uvTex.y + 0f));
		if (!bSwitchUvHorizontal)
		{
			this.Uvs.Add(new Vector2(uvTex.x + 0f, uvTex.y + uvTex.height - 0f));
		}
		else
		{
			this.Uvs.Add(new Vector2(uvTex.x + uvTex.width - 0f, uvTex.y + 0f));
		}
		this.Uvs.Add(new Vector2(uvTex.x + uvTex.width - 0f, uvTex.y + uvTex.height - 0f));
		this.UvsCrack.Add(new Vector2(uvOverlay.x + 0f, uvOverlay.y + 0f));
		if (!bSwitchUvHorizontal)
		{
			this.UvsCrack.Add(new Vector2(uvOverlay.x + 0f, uvOverlay.y + uvOverlay.height - 0f));
		}
		else
		{
			this.UvsCrack.Add(new Vector2(uvOverlay.x + uvOverlay.width - 0f, uvOverlay.y + 0f));
		}
		this.UvsCrack.Add(new Vector2(uvOverlay.x + uvOverlay.width - 0f, uvOverlay.y + uvOverlay.height - 0f));
		this.CurTriangleIndex += 3;
	}

	// Token: 0x06005B9A RID: 23450 RVA: 0x00238B9C File Offset: 0x00236D9C
	public void AddTriangle(Vector3 _v0, Vector2 _uv0, Color _c0, Vector3 _v1, Vector2 _uv1, Color _c1, Vector3 _v2, Vector2 _uv2, Color _c2, Rect uvTex, Rect uvOverlay)
	{
		if (this.m_Vertices.Count > 786429)
		{
			return;
		}
		this.m_Vertices.Add(_v0);
		this.m_Vertices.Add(_v1);
		this.m_Vertices.Add(_v2);
		int count = this.m_CollVertices.Count;
		this.m_CollVertices.Add(_v0);
		this.m_CollVertices.Add(_v1);
		this.m_CollVertices.Add(_v2);
		this.m_Normals.Add(Vector3.Cross(_v2 - _v0, _v1 - _v0).normalized);
		this.m_Normals.Add(Vector3.Cross(_v0 - _v1, _v2 - _v1).normalized);
		this.m_Normals.Add(Vector3.Cross(_v1 - _v2, _v0 - _v2).normalized);
		float num = this.temperature;
		_c0.a = num;
		_c1.a = num;
		_c2.a = num;
		this.ColorVertices.Add(_c0);
		this.ColorVertices.Add(_c1);
		this.ColorVertices.Add(_c2);
		this.m_Indices.Add(this.CurTriangleIndex);
		this.m_Indices.Add(this.CurTriangleIndex + 2);
		this.m_Indices.Add(this.CurTriangleIndex + 1);
		this.m_CollIndices.Add(count);
		this.m_CollIndices.Add(count + 2);
		this.m_CollIndices.Add(count + 1);
		this.Uvs.Add(new Vector2(uvTex.x + _uv0.x * uvTex.width, uvTex.y + _uv0.y * uvTex.height));
		this.Uvs.Add(new Vector2(uvTex.x + _uv1.x * uvTex.width, uvTex.y + _uv1.y * uvTex.height));
		this.Uvs.Add(new Vector2(uvTex.x + _uv2.x * uvTex.width, uvTex.y + _uv2.y * uvTex.height));
		this.UvsCrack.Add(new Vector2(uvOverlay.x + _uv0.x * uvOverlay.width, uvOverlay.y + _uv0.y * uvOverlay.height));
		this.UvsCrack.Add(new Vector2(uvOverlay.x + _uv1.x * uvOverlay.width, uvOverlay.y + _uv1.y * uvOverlay.height));
		this.UvsCrack.Add(new Vector2(uvOverlay.x + _uv2.x * uvOverlay.width, uvOverlay.y + _uv2.y * uvOverlay.height));
		this.CurTriangleIndex += 3;
	}

	// Token: 0x06005B9B RID: 23451 RVA: 0x00238EB0 File Offset: 0x002370B0
	public virtual void AddMesh(Vector3 _drawPos, int _count, Vector3[] _vertices, Vector3[] _normals, ArrayListMP<int> _indices, ArrayListMP<Vector2> _uvs, byte _sunlight, byte _blocklight, VoxelMesh _specialColliders, int damage)
	{
		if (_count + this.m_Vertices.Count > 786432)
		{
			return;
		}
		int curTriangleIndex = this.CurTriangleIndex;
		this.CurTriangleIndex += _count;
		this.m_Vertices.AddRange(_vertices, 0, _count);
		this.m_Normals.AddRange(_normals, 0, _count);
		Color color = new Color((float)_sunlight / 15f, 0f, 0f, this.temperature);
		int num = this.m_ColorVertices.Alloc(_count);
		for (int i = 0; i < _count; i++)
		{
			this.m_ColorVertices[num + i] = color;
		}
		num = this.m_Indices.Alloc(_indices.Count);
		for (int j = 0; j < _indices.Count; j++)
		{
			this.m_Indices[num + j] = _indices[j] + curTriangleIndex;
		}
		this.m_Uvs.AddRange(_uvs.Items, 0, _uvs.Count);
		Vector2 vector = new Vector2((float)damage, 0f);
		num = this.UvsCrack.Alloc(_uvs.Count);
		for (int k = 0; k < _uvs.Count; k++)
		{
			this.UvsCrack[num + k] = vector;
		}
	}

	// Token: 0x1700098C RID: 2444
	// (get) Token: 0x06005B9C RID: 23452 RVA: 0x00238FF6 File Offset: 0x002371F6
	// (set) Token: 0x06005B9D RID: 23453 RVA: 0x00238FFE File Offset: 0x002371FE
	public ArrayListMP<int> Indices
	{
		get
		{
			return this.m_Indices;
		}
		set
		{
			this.m_Indices = value;
		}
	}

	// Token: 0x1700098D RID: 2445
	// (get) Token: 0x06005B9E RID: 23454 RVA: 0x00239007 File Offset: 0x00237207
	// (set) Token: 0x06005B9F RID: 23455 RVA: 0x0023900F File Offset: 0x0023720F
	public ArrayListMP<Vector2> Uvs
	{
		get
		{
			return this.m_Uvs;
		}
		set
		{
			this.m_Uvs = value;
		}
	}

	// Token: 0x1700098E RID: 2446
	// (get) Token: 0x06005BA0 RID: 23456 RVA: 0x00239018 File Offset: 0x00237218
	// (set) Token: 0x06005BA1 RID: 23457 RVA: 0x00239020 File Offset: 0x00237220
	public ArrayListMP<Vector2> Uvs3
	{
		get
		{
			return this.m_Uvs3;
		}
		set
		{
			this.m_Uvs3 = value;
		}
	}

	// Token: 0x1700098F RID: 2447
	// (get) Token: 0x06005BA2 RID: 23458 RVA: 0x00239029 File Offset: 0x00237229
	// (set) Token: 0x06005BA3 RID: 23459 RVA: 0x00239031 File Offset: 0x00237231
	public ArrayListMP<Vector2> Uvs4
	{
		get
		{
			return this.m_Uvs4;
		}
		set
		{
			this.m_Uvs4 = value;
		}
	}

	// Token: 0x17000990 RID: 2448
	// (get) Token: 0x06005BA4 RID: 23460 RVA: 0x0023903A File Offset: 0x0023723A
	// (set) Token: 0x06005BA5 RID: 23461 RVA: 0x00239042 File Offset: 0x00237242
	public ArrayListMP<Vector3> Vertices
	{
		get
		{
			return this.m_Vertices;
		}
		set
		{
			this.m_Vertices = value;
		}
	}

	// Token: 0x17000991 RID: 2449
	// (get) Token: 0x06005BA6 RID: 23462 RVA: 0x0023904B File Offset: 0x0023724B
	// (set) Token: 0x06005BA7 RID: 23463 RVA: 0x00239053 File Offset: 0x00237253
	public ArrayListMP<Vector3> Normals
	{
		get
		{
			return this.m_Normals;
		}
		set
		{
			this.m_Normals = value;
		}
	}

	// Token: 0x17000992 RID: 2450
	// (get) Token: 0x06005BA8 RID: 23464 RVA: 0x0023905C File Offset: 0x0023725C
	// (set) Token: 0x06005BA9 RID: 23465 RVA: 0x00239064 File Offset: 0x00237264
	public ArrayListMP<Vector4> Tangents
	{
		get
		{
			return this.m_Tangents;
		}
		set
		{
			this.m_Tangents = value;
		}
	}

	// Token: 0x17000993 RID: 2451
	// (get) Token: 0x06005BAA RID: 23466 RVA: 0x0023906D File Offset: 0x0023726D
	// (set) Token: 0x06005BAB RID: 23467 RVA: 0x00239075 File Offset: 0x00237275
	public ArrayListMP<Color> ColorVertices
	{
		get
		{
			return this.m_ColorVertices;
		}
		set
		{
			this.m_ColorVertices = value;
		}
	}

	// Token: 0x17000994 RID: 2452
	// (get) Token: 0x06005BAC RID: 23468 RVA: 0x0023907E File Offset: 0x0023727E
	public ArrayListMP<int> CollIndices
	{
		get
		{
			return this.m_CollIndices;
		}
	}

	// Token: 0x17000995 RID: 2453
	// (get) Token: 0x06005BAD RID: 23469 RVA: 0x00239086 File Offset: 0x00237286
	public ArrayListMP<Vector3> CollVertices
	{
		get
		{
			return this.m_CollVertices;
		}
	}

	// Token: 0x17000996 RID: 2454
	// (get) Token: 0x06005BAE RID: 23470 RVA: 0x00239090 File Offset: 0x00237290
	public int Size
	{
		get
		{
			int num = this.m_Indices.Count * 4 + this.m_Uvs.Count * 8 + this.m_Vertices.Count * 12 + this.m_ColorVertices.Count * 16 + this.m_Normals.Count * 12 + this.m_Tangents.Count * 16;
			if (this.UvsCrack != null)
			{
				num += this.UvsCrack.Count * 8;
			}
			return num;
		}
	}

	// Token: 0x17000997 RID: 2455
	// (get) Token: 0x06005BAF RID: 23471 RVA: 0x0023910E File Offset: 0x0023730E
	public int Triangles
	{
		get
		{
			return this.m_Triangles;
		}
	}

	// Token: 0x06005BB0 RID: 23472 RVA: 0x00239118 File Offset: 0x00237318
	[PublicizedFrom(EAccessModifier.Protected)]
	public static void calculateMeshTangentsDummy(Mesh mesh)
	{
		int num = mesh.vertices.Length;
		Vector4[] array = new Vector4[num];
		for (int i = 0; i < num; i++)
		{
			array[i] = Vector4.one;
		}
		mesh.tangents = array;
	}

	// Token: 0x06005BB1 RID: 23473 RVA: 0x00239154 File Offset: 0x00237354
	[PublicizedFrom(EAccessModifier.Protected)]
	public static void calculateMeshNormalsDummy(Mesh mesh)
	{
		int num = mesh.vertices.Length;
		Vector3[] array = new Vector3[num];
		for (int i = 0; i < num; i++)
		{
			array[i] = Vector3.one;
		}
		mesh.normals = array;
	}

	// Token: 0x06005BB2 RID: 23474 RVA: 0x000027FC File Offset: 0x000009FC
	public void CheckVertexLimit(int _count)
	{
	}

	// Token: 0x06005BB3 RID: 23475 RVA: 0x00239190 File Offset: 0x00237390
	public void AddRectXYFacingNorth(float _x, float _y, float _z, int _xAdd, int _yAdd)
	{
		int count = this.m_Vertices.Count;
		this.m_Vertices.Add(new Vector3(_x, _y, _z));
		this.m_Vertices.Add(new Vector3(_x + (float)_xAdd, _y, _z));
		this.m_Vertices.Add(new Vector3(_x + (float)_xAdd, _y + (float)_yAdd, _z));
		this.m_Vertices.Add(new Vector3(_x, _y + (float)_yAdd, _z));
		this.m_Indices.Add(count);
		this.m_Indices.Add(count + 1);
		this.m_Indices.Add(count + 2);
		this.m_Indices.Add(count + 2);
		this.m_Indices.Add(count + 3);
		this.m_Indices.Add(count);
	}

	// Token: 0x06005BB4 RID: 23476 RVA: 0x00239258 File Offset: 0x00237458
	public void AddRectXYFacingNorth(float _x, float _y, float _z, int _xAdd, int _yAdd, Color _c)
	{
		int count = this.m_Vertices.Count;
		this.m_Vertices.Add(new Vector3(_x, _y, _z));
		this.m_Vertices.Add(new Vector3(_x + (float)_xAdd, _y, _z));
		this.m_Vertices.Add(new Vector3(_x + (float)_xAdd, _y + (float)_yAdd, _z));
		this.m_Vertices.Add(new Vector3(_x, _y + (float)_yAdd, _z));
		_c.a = this.temperature;
		this.m_ColorVertices.Add(_c);
		this.m_ColorVertices.Add(_c);
		this.m_ColorVertices.Add(_c);
		this.m_ColorVertices.Add(_c);
		this.m_Indices.Add(count);
		this.m_Indices.Add(count + 1);
		this.m_Indices.Add(count + 2);
		this.m_Indices.Add(count + 2);
		this.m_Indices.Add(count + 3);
		this.m_Indices.Add(count);
	}

	// Token: 0x06005BB5 RID: 23477 RVA: 0x00239360 File Offset: 0x00237560
	public void AddRectXYFacingSouth(float _x, float _y, float _z, int _xAdd, int _zAdd)
	{
		int count = this.m_Vertices.Count;
		this.m_Vertices.Add(new Vector3(_x, _y, _z));
		this.m_Vertices.Add(new Vector3(_x + (float)_xAdd, _y, _z));
		this.m_Vertices.Add(new Vector3(_x + (float)_xAdd, _y + (float)_zAdd, _z));
		this.m_Vertices.Add(new Vector3(_x, _y + (float)_zAdd, _z));
		this.m_Indices.Add(count);
		this.m_Indices.Add(count + 3);
		this.m_Indices.Add(count + 2);
		this.m_Indices.Add(count + 2);
		this.m_Indices.Add(count + 1);
		this.m_Indices.Add(count);
	}

	// Token: 0x06005BB6 RID: 23478 RVA: 0x00239428 File Offset: 0x00237628
	public void AddRectXYFacingSouth(float _x, float _y, float _z, int _xAdd, int _zAdd, Color _c)
	{
		int count = this.m_Vertices.Count;
		this.m_Vertices.Add(new Vector3(_x, _y, _z));
		this.m_Vertices.Add(new Vector3(_x + (float)_xAdd, _y, _z));
		this.m_Vertices.Add(new Vector3(_x + (float)_xAdd, _y + (float)_zAdd, _z));
		this.m_Vertices.Add(new Vector3(_x, _y + (float)_zAdd, _z));
		_c.a = this.temperature;
		this.m_ColorVertices.Add(_c);
		this.m_ColorVertices.Add(_c);
		this.m_ColorVertices.Add(_c);
		this.m_ColorVertices.Add(_c);
		this.m_Indices.Add(count);
		this.m_Indices.Add(count + 3);
		this.m_Indices.Add(count + 2);
		this.m_Indices.Add(count + 2);
		this.m_Indices.Add(count + 1);
		this.m_Indices.Add(count);
	}

	// Token: 0x06005BB7 RID: 23479 RVA: 0x00239530 File Offset: 0x00237730
	public void AddRectYZFacingWest(float _x, float _y, float _z, int _yAdd, int _zAdd)
	{
		int count = this.m_Vertices.Count;
		this.m_Vertices.Add(new Vector3(_x, _y, _z));
		this.m_Vertices.Add(new Vector3(_x, _y, _z + (float)_zAdd));
		this.m_Vertices.Add(new Vector3(_x, _y + (float)_yAdd, _z + (float)_zAdd));
		this.m_Vertices.Add(new Vector3(_x, _y + (float)_yAdd, _z));
		this.m_Indices.Add(count);
		this.m_Indices.Add(count + 1);
		this.m_Indices.Add(count + 2);
		this.m_Indices.Add(count + 2);
		this.m_Indices.Add(count + 3);
		this.m_Indices.Add(count);
	}

	// Token: 0x06005BB8 RID: 23480 RVA: 0x002395F8 File Offset: 0x002377F8
	public void AddRectYZFacingWest(float _x, float _y, float _z, int _yAdd, int _zAdd, Color _c)
	{
		int count = this.m_Vertices.Count;
		this.m_Vertices.Add(new Vector3(_x, _y, _z));
		this.m_Vertices.Add(new Vector3(_x, _y, _z + (float)_zAdd));
		this.m_Vertices.Add(new Vector3(_x, _y + (float)_yAdd, _z + (float)_zAdd));
		this.m_Vertices.Add(new Vector3(_x, _y + (float)_yAdd, _z));
		_c.a = this.temperature;
		this.m_ColorVertices.Add(_c);
		this.m_ColorVertices.Add(_c);
		this.m_ColorVertices.Add(_c);
		this.m_ColorVertices.Add(_c);
		this.m_Indices.Add(count);
		this.m_Indices.Add(count + 1);
		this.m_Indices.Add(count + 2);
		this.m_Indices.Add(count + 2);
		this.m_Indices.Add(count + 3);
		this.m_Indices.Add(count);
	}

	// Token: 0x06005BB9 RID: 23481 RVA: 0x00239700 File Offset: 0x00237900
	public void AddRectYZFacingEast(float _x, float _y, float _z, int _yAdd, int _zAdd)
	{
		int count = this.m_Vertices.Count;
		this.m_Vertices.Add(new Vector3(_x, _y, _z));
		this.m_Vertices.Add(new Vector3(_x, _y, _z + (float)_zAdd));
		this.m_Vertices.Add(new Vector3(_x, _y + (float)_yAdd, _z + (float)_zAdd));
		this.m_Vertices.Add(new Vector3(_x, _y + (float)_yAdd, _z));
		this.m_Indices.Add(count);
		this.m_Indices.Add(count + 3);
		this.m_Indices.Add(count + 2);
		this.m_Indices.Add(count + 2);
		this.m_Indices.Add(count + 1);
		this.m_Indices.Add(count);
	}

	// Token: 0x06005BBA RID: 23482 RVA: 0x002397C8 File Offset: 0x002379C8
	public void AddRectYZFacingEast(float _x, float _y, float _z, int _yAdd, int _zAdd, Color _c)
	{
		int count = this.m_Vertices.Count;
		this.m_Vertices.Add(new Vector3(_x, _y, _z));
		this.m_Vertices.Add(new Vector3(_x, _y, _z + (float)_zAdd));
		this.m_Vertices.Add(new Vector3(_x, _y + (float)_yAdd, _z + (float)_zAdd));
		this.m_Vertices.Add(new Vector3(_x, _y + (float)_yAdd, _z));
		_c.a = this.temperature;
		this.m_ColorVertices.Add(_c);
		this.m_ColorVertices.Add(_c);
		this.m_ColorVertices.Add(_c);
		this.m_ColorVertices.Add(_c);
		this.m_Indices.Add(count);
		this.m_Indices.Add(count + 3);
		this.m_Indices.Add(count + 2);
		this.m_Indices.Add(count + 2);
		this.m_Indices.Add(count + 1);
		this.m_Indices.Add(count);
	}

	// Token: 0x06005BBB RID: 23483 RVA: 0x002398D0 File Offset: 0x00237AD0
	public void AddRectXZFacingUp(float _x, float _y, float _z, int _xAdd, int _zAdd)
	{
		int count = this.m_Vertices.Count;
		this.m_Vertices.Add(new Vector3(_x, _y, _z));
		this.m_Vertices.Add(new Vector3(_x + (float)_xAdd, _y, _z));
		this.m_Vertices.Add(new Vector3(_x + (float)_xAdd, _y, _z + (float)_zAdd));
		this.m_Vertices.Add(new Vector3(_x, _y, _z + (float)_zAdd));
		this.m_Indices.Add(count);
		this.m_Indices.Add(count + 3);
		this.m_Indices.Add(count + 2);
		this.m_Indices.Add(count + 2);
		this.m_Indices.Add(count + 1);
		this.m_Indices.Add(count);
	}

	// Token: 0x06005BBC RID: 23484 RVA: 0x00239998 File Offset: 0x00237B98
	public void AddRectXZFacingUp(float _x, float _y, float _z, int _xAdd, int _zAdd, Color _c)
	{
		int count = this.m_Vertices.Count;
		this.m_Vertices.Add(new Vector3(_x, _y, _z));
		this.m_Vertices.Add(new Vector3(_x + (float)_xAdd, _y, _z));
		this.m_Vertices.Add(new Vector3(_x + (float)_xAdd, _y, _z + (float)_zAdd));
		this.m_Vertices.Add(new Vector3(_x, _y, _z + (float)_zAdd));
		_c.a = this.temperature;
		this.m_ColorVertices.Add(_c);
		this.m_ColorVertices.Add(_c);
		this.m_ColorVertices.Add(_c);
		this.m_ColorVertices.Add(_c);
		this.m_Indices.Add(count);
		this.m_Indices.Add(count + 3);
		this.m_Indices.Add(count + 2);
		this.m_Indices.Add(count + 2);
		this.m_Indices.Add(count + 1);
		this.m_Indices.Add(count);
	}

	// Token: 0x06005BBD RID: 23485 RVA: 0x00239AA0 File Offset: 0x00237CA0
	public void AddRectXZFacingDown(float _x, float _y, float _z, int _xAdd, int _zAdd)
	{
		int count = this.m_Vertices.Count;
		this.m_Vertices.Add(new Vector3(_x, _y, _z));
		this.m_Vertices.Add(new Vector3(_x + (float)_xAdd, _y, _z));
		this.m_Vertices.Add(new Vector3(_x + (float)_xAdd, _y, _z + (float)_zAdd));
		this.m_Vertices.Add(new Vector3(_x, _y, _z + (float)_zAdd));
		this.m_Indices.Add(count);
		this.m_Indices.Add(count + 1);
		this.m_Indices.Add(count + 2);
		this.m_Indices.Add(count + 2);
		this.m_Indices.Add(count + 3);
		this.m_Indices.Add(count);
	}

	// Token: 0x06005BBE RID: 23486 RVA: 0x00239B68 File Offset: 0x00237D68
	public void AddRectXZFacingDown(float _x, float _y, float _z, int _xAdd, int _zAdd, Color _c)
	{
		int count = this.m_Vertices.Count;
		this.m_Vertices.Add(new Vector3(_x, _y, _z));
		this.m_Vertices.Add(new Vector3(_x + (float)_xAdd, _y, _z));
		this.m_Vertices.Add(new Vector3(_x + (float)_xAdd, _y, _z + (float)_zAdd));
		this.m_Vertices.Add(new Vector3(_x, _y, _z + (float)_zAdd));
		_c.a = this.temperature;
		this.m_ColorVertices.Add(_c);
		this.m_ColorVertices.Add(_c);
		this.m_ColorVertices.Add(_c);
		this.m_ColorVertices.Add(_c);
		this.m_Indices.Add(count);
		this.m_Indices.Add(count + 1);
		this.m_Indices.Add(count + 2);
		this.m_Indices.Add(count + 2);
		this.m_Indices.Add(count + 3);
		this.m_Indices.Add(count);
	}

	// Token: 0x06005BBF RID: 23487 RVA: 0x00239C6E File Offset: 0x00237E6E
	public static float GetTemperature(BiomeDefinition bd)
	{
		if (bd == null)
		{
			return -10f;
		}
		return 0f;
	}

	// Token: 0x06005BC0 RID: 23488 RVA: 0x00239C80 File Offset: 0x00237E80
	public void ClearTemperatureValues()
	{
		for (int i = 0; i < this.m_ColorVertices.Count; i++)
		{
			this.m_ColorVertices[i] = new Color(this.m_ColorVertices[i].r, this.m_ColorVertices[i].g, this.m_ColorVertices[i].b, 100f);
		}
	}

	// Token: 0x04004739 RID: 18233
	public static float COLOR_SOUTH = 0.9f;

	// Token: 0x0400473A RID: 18234
	public static float COLOR_WEST = 0.8f;

	// Token: 0x0400473B RID: 18235
	public static float COLOR_NORTH = 0.7f;

	// Token: 0x0400473C RID: 18236
	public static float COLOR_EAST = 0.85f;

	// Token: 0x0400473D RID: 18237
	public static float COLOR_TOP = 1f;

	// Token: 0x0400473E RID: 18238
	public static float COLOR_BOTTOM = 0.65f;

	// Token: 0x0400473F RID: 18239
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ConditionalWeakTable<MeshFilter, VoxelMesh.MeshFilterState> m_meshFilterStates = new ConditionalWeakTable<MeshFilter, VoxelMesh.MeshFilterState>();

	// Token: 0x04004740 RID: 18240
	public ArrayListMP<Vector3> m_Vertices;

	// Token: 0x04004741 RID: 18241
	public ArrayListMP<int> m_Indices;

	// Token: 0x04004742 RID: 18242
	public ArrayListMP<Vector2> m_Uvs;

	// Token: 0x04004743 RID: 18243
	public ArrayListMP<Vector2> UvsCrack;

	// Token: 0x04004744 RID: 18244
	public ArrayListMP<Vector2> m_Uvs3;

	// Token: 0x04004745 RID: 18245
	public ArrayListMP<Vector2> m_Uvs4;

	// Token: 0x04004746 RID: 18246
	public ArrayListMP<Vector3> m_Normals;

	// Token: 0x04004747 RID: 18247
	public ArrayListMP<Vector4> m_Tangents;

	// Token: 0x04004748 RID: 18248
	public ArrayListMP<Color> m_ColorVertices;

	// Token: 0x04004749 RID: 18249
	public ArrayListMP<Vector3> m_CollVertices;

	// Token: 0x0400474A RID: 18250
	public ArrayListMP<int> m_CollIndices;

	// Token: 0x0400474B RID: 18251
	public int CurTriangleIndex;

	// Token: 0x0400474C RID: 18252
	[PublicizedFrom(EAccessModifier.Protected)]
	public int m_Triangles;

	// Token: 0x0400474D RID: 18253
	public int meshIndex;

	// Token: 0x0400474E RID: 18254
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cTemperatureDefault = -10f;

	// Token: 0x0400474F RID: 18255
	[PublicizedFrom(EAccessModifier.Private)]
	public float temperature;

	// Token: 0x04004750 RID: 18256
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cPoolMeshMax = 250;

	// Token: 0x04004751 RID: 18257
	public static List<Mesh> meshPool = new List<Mesh>();

	// Token: 0x02000BE0 RID: 3040
	public enum EnumMeshType
	{
		// Token: 0x04004753 RID: 18259
		Blocks,
		// Token: 0x04004754 RID: 18260
		Models,
		// Token: 0x04004755 RID: 18261
		Terrain,
		// Token: 0x04004756 RID: 18262
		Decals
	}

	// Token: 0x02000BE1 RID: 3041
	[PublicizedFrom(EAccessModifier.Private)]
	public class MeshFilterState
	{
		// Token: 0x04004757 RID: 18263
		public int NextGeneration;

		// Token: 0x04004758 RID: 18264
		public int AppliedGeneration = -1;
	}

	// Token: 0x02000BE2 RID: 3042
	public enum CreateFlags
	{
		// Token: 0x0400475A RID: 18266
		None,
		// Token: 0x0400475B RID: 18267
		Collider,
		// Token: 0x0400475C RID: 18268
		Cracks,
		// Token: 0x0400475D RID: 18269
		Default
	}
}
