using System;
using UnityEngine;

// Token: 0x02000CEC RID: 3308
public static class WaterDebugAssets
{
	// Token: 0x17000AA2 RID: 2722
	// (get) Token: 0x060064E3 RID: 25827 RVA: 0x0027A960 File Offset: 0x00278B60
	public static Mesh CubeMesh
	{
		get
		{
			return WaterDebugAssets.cubeMesh.Value;
		}
	}

	// Token: 0x060064E4 RID: 25828 RVA: 0x0027A96C File Offset: 0x00278B6C
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

	// Token: 0x17000AA3 RID: 2723
	// (get) Token: 0x060064E5 RID: 25829 RVA: 0x0027AA88 File Offset: 0x00278C88
	public static Material DebugMaterial
	{
		get
		{
			return WaterDebugAssets.sharedMaterial.Value;
		}
	}

	// Token: 0x060064E6 RID: 25830 RVA: 0x0027AA94 File Offset: 0x00278C94
	public static Material CreateDebugMaterial()
	{
		return new Material(Shader.Find("Debug/DebugInstancedProcedural"))
		{
			enableInstancing = true
		};
	}

	// Token: 0x04004EB3 RID: 20147
	[PublicizedFrom(EAccessModifier.Private)]
	public static Lazy<Mesh> cubeMesh = new Lazy<Mesh>(new Func<Mesh>(WaterDebugAssets.GenerateCubeMesh));

	// Token: 0x04004EB4 RID: 20148
	[PublicizedFrom(EAccessModifier.Private)]
	public static Lazy<Material> sharedMaterial = new Lazy<Material>(new Func<Material>(WaterDebugAssets.CreateDebugMaterial));
}
