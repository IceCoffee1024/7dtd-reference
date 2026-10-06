using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02000BEA RID: 3050
public class UnityDistantTerrainWaterPlane
{
	// Token: 0x06005B68 RID: 23400 RVA: 0x0023616C File Offset: 0x0023436C
	public UnityDistantTerrainWaterPlane(UnityDistantTerrain.Config _terrainConfig, Material _waterMaterial)
	{
		this.terrainConfig = _terrainConfig;
		this.waterMaterial = _waterMaterial;
	}

	// Token: 0x06005B69 RID: 23401 RVA: 0x00236184 File Offset: 0x00234384
	public void createDynamicWaterPlane_Step1(int _xStart, int _zStart, int _tileSize, int _waterChunks16x16Width, byte[] _waterChunks16x16)
	{
		int num = this.terrainConfig.ChunkWorldSize / 16 + 1;
		int num2 = num * num;
		if (this.dynamiceWaterPlaneChunkArr == null || this.dynamiceWaterPlaneChunkArr.Length != num2 * num2)
		{
			this.dynamiceWaterPlaneChunkArr = new int[num2];
		}
		else
		{
			Array.Clear(this.dynamiceWaterPlaneChunkArr, 0, this.dynamiceWaterPlaneChunkArr.Length);
		}
		int num3 = _tileSize / 16;
		int num4 = _tileSize / 16;
		int num5 = _xStart / 16 + _zStart / 16 * _waterChunks16x16Width;
		for (int i = 0; i < num3; i++)
		{
			for (int j = 0; j < num4; j++)
			{
				int num6 = (int)_waterChunks16x16[num5 + i + j * _waterChunks16x16Width];
				if (num6 != 0)
				{
					this.dynamiceWaterPlaneChunkArr[i + j * num] = 16842752 | num6;
				}
			}
		}
		if (this.dynamicWaterPlane == null)
		{
			this.dynamicWaterPlane = new UnityDistantTerrainWaterPlane.DynamicMeshData(num * num);
		}
		else
		{
			this.dynamicWaterPlane.Clear();
		}
		for (int k = 0; k < this.dynamiceWaterPlaneChunkArr.Length; k++)
		{
			int num7 = this.dynamiceWaterPlaneChunkArr[k] >> 16;
			if (num7 != 0)
			{
				int num8 = k % num;
				int num9 = k / num;
				int num10 = num7 >> 8;
				int num11 = num7 & 255;
				int num12 = this.dynamiceWaterPlaneChunkArr[k] & 65535;
				this.dynamicWaterPlane.AddQuad(num8, num12, num9, num10, num11, 16, num);
			}
		}
	}

	// Token: 0x06005B6A RID: 23402 RVA: 0x002362D0 File Offset: 0x002344D0
	public GameObject createDynamicWaterPlane_Step2(GameObject _existingWaterPlane, Transform _parent, string _tag = null)
	{
		if (_existingWaterPlane == null)
		{
			_existingWaterPlane = new GameObject("WaterHi");
			_existingWaterPlane.transform.parent = _parent;
			_existingWaterPlane.transform.localPosition = new Vector3(0f, 0f, 0f);
			_existingWaterPlane.layer = 14;
		}
		if (this.dynamicWaterPlane == null || this.dynamicWaterPlane.vertices.Count == 0)
		{
			return _existingWaterPlane;
		}
		MeshFilter meshFilter = _existingWaterPlane.GetComponent<MeshFilter>();
		if (meshFilter == null)
		{
			meshFilter = _existingWaterPlane.AddComponent<MeshFilter>();
		}
		else
		{
			meshFilter.mesh.Clear();
		}
		Mesh mesh = meshFilter.mesh;
		mesh.SetVertices(this.dynamicWaterPlane.vertices);
		mesh.SetNormals(this.dynamicWaterPlane.normals);
		mesh.SetTangents(this.dynamicWaterPlane.tangents);
		mesh.SetTriangles(this.dynamicWaterPlane.triangles, 0);
		MeshRenderer meshRenderer = _existingWaterPlane.GetComponent<MeshRenderer>();
		if (meshRenderer == null)
		{
			meshRenderer = _existingWaterPlane.AddComponent<MeshRenderer>();
		}
		meshRenderer.material = this.waterMaterial;
		meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
		if (_tag != null)
		{
			_existingWaterPlane.tag = _tag;
		}
		this.dynamicWaterPlane.Clear();
		_existingWaterPlane.SetActive(true);
		return _existingWaterPlane;
	}

	// Token: 0x06005B6B RID: 23403 RVA: 0x002363F6 File Offset: 0x002345F6
	public void Cleanup()
	{
		this.dynamicWaterPlane = null;
		this.fixedWaterPlaneHiRes = null;
		this.fixedWaterPlaneLoRes = null;
	}

	// Token: 0x04004750 RID: 18256
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cWaterPlaneChunkSize = 16;

	// Token: 0x04004751 RID: 18257
	[PublicizedFrom(EAccessModifier.Private)]
	public UnityDistantTerrainWaterPlane.FixedMeshData fixedWaterPlaneHiRes;

	// Token: 0x04004752 RID: 18258
	[PublicizedFrom(EAccessModifier.Private)]
	public UnityDistantTerrainWaterPlane.FixedMeshData fixedWaterPlaneLoRes;

	// Token: 0x04004753 RID: 18259
	[PublicizedFrom(EAccessModifier.Private)]
	public UnityDistantTerrainWaterPlane.DynamicMeshData dynamicWaterPlane;

	// Token: 0x04004754 RID: 18260
	[PublicizedFrom(EAccessModifier.Private)]
	public int[] dynamiceWaterPlaneChunkArr;

	// Token: 0x04004755 RID: 18261
	[PublicizedFrom(EAccessModifier.Private)]
	public UnityDistantTerrain.Config terrainConfig;

	// Token: 0x04004756 RID: 18262
	[PublicizedFrom(EAccessModifier.Private)]
	public Material waterMaterial;

	// Token: 0x02000BEB RID: 3051
	[PublicizedFrom(EAccessModifier.Private)]
	public class FixedMeshData
	{
		// Token: 0x06005B6C RID: 23404 RVA: 0x00236410 File Offset: 0x00234610
		public FixedMeshData(int _vertCount, int _triCount)
		{
			this.count = _vertCount;
			this.vertices = new Vector3[this.count];
			this.normals = new Vector3[this.count];
			this.tangents = new Vector4[this.count];
			this.uvs = new Vector2[this.count];
			this.triangles = new int[_triCount];
		}

		// Token: 0x04004757 RID: 18263
		public int count;

		// Token: 0x04004758 RID: 18264
		public Vector3[] vertices;

		// Token: 0x04004759 RID: 18265
		public Vector3[] normals;

		// Token: 0x0400475A RID: 18266
		public Vector4[] tangents;

		// Token: 0x0400475B RID: 18267
		public Vector2[] uvs;

		// Token: 0x0400475C RID: 18268
		public int[] triangles;
	}

	// Token: 0x02000BEC RID: 3052
	[PublicizedFrom(EAccessModifier.Private)]
	public class DynamicMeshData
	{
		// Token: 0x06005B6D RID: 23405 RVA: 0x0023647C File Offset: 0x0023467C
		public DynamicMeshData(int _triCapacity)
		{
			this.vertices = new List<Vector3>(_triCapacity / 2);
			this.normals = new List<Vector3>(_triCapacity / 2);
			this.tangents = new List<Vector4>(_triCapacity / 2);
			this.uvs = new List<Vector2>(_triCapacity / 2);
			this.triangles = new List<int>(_triCapacity);
		}

		// Token: 0x06005B6E RID: 23406 RVA: 0x002364D3 File Offset: 0x002346D3
		public void Clear()
		{
			this.vertices.Clear();
			this.normals.Clear();
			this.tangents.Clear();
			this.uvs.Clear();
			this.triangles.Clear();
		}

		// Token: 0x06005B6F RID: 23407 RVA: 0x0023650C File Offset: 0x0023470C
		public void AddQuad(int _x, int _y, int _z, int _width, int _height, int _scale, int _totalCountP1)
		{
			int num = this.vertices.Count;
			_x *= _scale;
			_z *= _scale;
			_width *= _scale;
			_height *= _scale;
			this.vertices.Add(new Vector3((float)_x, (float)_y + 0.77f, (float)_z));
			this.vertices.Add(new Vector3((float)(_x + _width), (float)_y + 0.77f, (float)(_z + _height)));
			this.vertices.Add(new Vector3((float)(_x + _width), (float)_y + 0.77f, (float)_z));
			this.normals.Add(Vector3.up);
			this.normals.Add(Vector3.up);
			this.normals.Add(Vector3.up);
			this.tangents.Add(UnityDistantTerrainWaterPlane.DynamicMeshData.defaultWaterPlaneTangent);
			this.tangents.Add(UnityDistantTerrainWaterPlane.DynamicMeshData.defaultWaterPlaneTangent);
			this.tangents.Add(UnityDistantTerrainWaterPlane.DynamicMeshData.defaultWaterPlaneTangent);
			this.triangles.Add(num);
			this.triangles.Add(1 + num);
			this.triangles.Add(2 + num);
			num = this.vertices.Count;
			this.vertices.Add(new Vector3((float)_x, (float)_y + 0.77f, (float)_z));
			this.vertices.Add(new Vector3((float)_x, (float)_y + 0.77f, (float)(_z + _height)));
			this.vertices.Add(new Vector3((float)(_x + _width), (float)_y + 0.77f, (float)(_z + _height)));
			this.normals.Add(Vector3.up);
			this.normals.Add(Vector3.up);
			this.normals.Add(Vector3.up);
			this.tangents.Add(UnityDistantTerrainWaterPlane.DynamicMeshData.defaultWaterPlaneTangent);
			this.tangents.Add(UnityDistantTerrainWaterPlane.DynamicMeshData.defaultWaterPlaneTangent);
			this.tangents.Add(UnityDistantTerrainWaterPlane.DynamicMeshData.defaultWaterPlaneTangent);
			this.triangles.Add(num);
			this.triangles.Add(1 + num);
			this.triangles.Add(2 + num);
		}

		// Token: 0x0400475D RID: 18269
		public List<Vector3> vertices;

		// Token: 0x0400475E RID: 18270
		public List<Vector3> normals;

		// Token: 0x0400475F RID: 18271
		public List<Vector4> tangents;

		// Token: 0x04004760 RID: 18272
		public List<Vector2> uvs;

		// Token: 0x04004761 RID: 18273
		public List<int> triangles;

		// Token: 0x04004762 RID: 18274
		[PublicizedFrom(EAccessModifier.Private)]
		public static Vector4 defaultWaterPlaneTangent = new Vector4(1f, 0f, 0f, 1f);
	}
}
