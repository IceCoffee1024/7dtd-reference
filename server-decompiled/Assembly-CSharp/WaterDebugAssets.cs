using System;
using UnityEngine;

// Token: 0x02000CF7 RID: 3319
public static class WaterDebugAssets
{
	// Token: 0x17000AA8 RID: 2728
	// (get) Token: 0x06006508 RID: 25864 RVA: 0x0027BE88 File Offset: 0x0027A088
	public static Mesh CubeMesh
	{
		get
		{
			return WaterDebugAssets.cubeMesh.Value;
		}
	}

	// Token: 0x06006509 RID: 25865 RVA: 0x0027BE94 File Offset: 0x0027A094
	public static Mesh GenerateCubeMesh()
	{
		Mesh mesh = new Mesh();
		mesh.vertices = new Vector3[]
		{
			new Vector3(-0.5f, -0.5f, -0.5f),
			new Vector3(0.5f, -0.5f, -0.5f),
			new Vector3(0.5f, 0.5f, -0.5f),
			new Vector3(-0.5f, 0.5f, -0.5f),
			new Vector3(-0.5f, 0.5f, 0.5f),
			new Vector3(0.5f, 0.5f, 0.5f),
			new Vector3(0.5f, -0.5f, 0.5f),
			new Vector3(-0.5f, -0.5f, 0.5f)
		};
		mesh.triangles = new int[]
		{
			0, 2, 1, 0, 3, 2, 2, 3, 4, 2,
			4, 5, 1, 2, 5, 1, 5, 6, 0, 7,
			4, 0, 4, 3, 5, 4, 7, 5, 7, 6,
			0, 6, 7, 0, 1, 6
		};
		mesh.Optimize();
		mesh.RecalculateNormals();
		return mesh;
	}

	// Token: 0x17000AA9 RID: 2729
	// (get) Token: 0x0600650A RID: 25866 RVA: 0x0027BFB0 File Offset: 0x0027A1B0
	public static Material DebugMaterial
	{
		get
		{
			return WaterDebugAssets.sharedMaterial.Value;
		}
	}

	// Token: 0x0600650B RID: 25867 RVA: 0x0027BFBC File Offset: 0x0027A1BC
	public static Material CreateDebugMaterial()
	{
		return new Material(Shader.Find("Debug/DebugInstancedProcedural"))
		{
			enableInstancing = true
		};
	}

	// Token: 0x04004ED8 RID: 20184
	[PublicizedFrom(EAccessModifier.Private)]
	public static Lazy<Mesh> cubeMesh = new Lazy<Mesh>(new Func<Mesh>(WaterDebugAssets.GenerateCubeMesh));

	// Token: 0x04004ED9 RID: 20185
	[PublicizedFrom(EAccessModifier.Private)]
	public static Lazy<Material> sharedMaterial = new Lazy<Material>(new Func<Material>(WaterDebugAssets.CreateDebugMaterial));
}
