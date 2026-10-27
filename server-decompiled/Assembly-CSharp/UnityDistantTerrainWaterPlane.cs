using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02000BF5 RID: 3061
public class UnityDistantTerrainWaterPlane
{
	// Token: 0x06005B8D RID: 23437 RVA: 0x002376B8 File Offset: 0x002358B8
	public UnityDistantTerrainWaterPlane(UnityDistantTerrain.Config _terrainConfig, Material _waterMaterial)
	{
		this.terrainConfig = _terrainConfig;
		this.waterMaterial = _waterMaterial;
	}

	// Token: 0x06005B8E RID: 23438 RVA: 0x002376D0 File Offset: 0x002358D0
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

	// Token: 0x06005B8F RID: 23439 RVA: 0x0023781C File Offset: 0x00235A1C
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

	// Token: 0x06005B90 RID: 23440 RVA: 0x00237942 File Offset: 0x00235B42
	public void Cleanup()
	{
		this.dynamicWaterPlane = null;
		this.fixedWaterPlaneHiRes = null;
		this.fixedWaterPlaneLoRes = null;
	}

	// Token: 0x04004775 RID: 18293
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cWaterPlaneChunkSize = 16;

	// Token: 0x04004776 RID: 18294
	[PublicizedFrom(EAccessModifier.Private)]
	public UnityDistantTerrainWaterPlane.FixedMeshData fixedWaterPlaneHiRes;

	// Token: 0x04004777 RID: 18295
	[PublicizedFrom(EAccessModifier.Private)]
	public UnityDistantTerrainWaterPlane.FixedMeshData fixedWaterPlaneLoRes;

	// Token: 0x04004778 RID: 18296
	[PublicizedFrom(EAccessModifier.Private)]
	public UnityDistantTerrainWaterPlane.DynamicMeshData dynamicWaterPlane;

	// Token: 0x04004779 RID: 18297
	[PublicizedFrom(EAccessModifier.Private)]
	public int[] dynamiceWaterPlaneChunkArr;

	// Token: 0x0400477A RID: 18298
	[PublicizedFrom(EAccessModifier.Private)]
	public UnityDistantTerrain.Config terrainConfig;

	// Token: 0x0400477B RID: 18299
	[PublicizedFrom(EAccessModifier.Private)]
	public Material waterMaterial;

	// Token: 0x02000BF6 RID: 3062
	[PublicizedFrom(EAccessModifier.Private)]
	public class FixedMeshData
	{
		// Token: 0x06005B91 RID: 23441 RVA: 0x0023795C File Offset: 0x00235B5C
		public FixedMeshData(int _vertCount, int _triCount)
		{
			this.count = _vertCount;
			this.vertices = new Vector3[this.count];
			this.normals = new Vector3[this.count];
			this.tangents = new Vector4[this.count];
			this.uvs = new Vector2[this.count];
			this.triangles = new int[_triCount];
		}

		// Token: 0x0400477C RID: 18300
		public int count;

		// Token: 0x0400477D RID: 18301
		public Vector3[] vertices;

		// Token: 0x0400477E RID: 18302
		public Vector3[] normals;

		// Token: 0x0400477F RID: 18303
		public Vector4[] tangents;

		// Token: 0x04004780 RID: 18304
		public Vector2[] uvs;

		// Token: 0x04004781 RID: 18305
		public int[] triangles;
	}

	// Token: 0x02000BF7 RID: 3063
	[PublicizedFrom(EAccessModifier.Private)]
	public class DynamicMeshData
	{
		// Token: 0x06005B92 RID: 23442 RVA: 0x002379C8 File Offset: 0x00235BC8
		public DynamicMeshData(int _triCapacity)
		{
			this.vertices = new List<Vector3>(_triCapacity / 2);
			this.normals = new List<Vector3>(_triCapacity / 2);
			this.tangents = new List<Vector4>(_triCapacity / 2);
			this.uvs = new List<Vector2>(_triCapacity / 2);
			this.triangles = new List<int>(_triCapacity);
		}

		// Token: 0x06005B93 RID: 23443 RVA: 0x00237A1F File Offset: 0x00235C1F
		public void Clear()
		{
			this.vertices.Clear();
			this.normals.Clear();
			this.tangents.Clear();
			this.uvs.Clear();
			this.triangles.Clear();
		}

		// Token: 0x06005B94 RID: 23444 RVA: 0x00237A58 File Offset: 0x00235C58
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

		// Token: 0x04004782 RID: 18306
		public List<Vector3> vertices;

		// Token: 0x04004783 RID: 18307
		public List<Vector3> normals;

		// Token: 0x04004784 RID: 18308
		public List<Vector4> tangents;

		// Token: 0x04004785 RID: 18309
		public List<Vector2> uvs;

		// Token: 0x04004786 RID: 18310
		public List<int> triangles;

		// Token: 0x04004787 RID: 18311
		[PublicizedFrom(EAccessModifier.Private)]
		public static Vector4 defaultWaterPlaneTangent = new Vector4(1f, 0f, 0f, 1f);
	}
}
