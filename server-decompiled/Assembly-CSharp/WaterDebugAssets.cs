using System;
using UnityEngine;

// Token: 0x02000CDE RID: 3294
public static class WaterDebugAssets
{
	// Token: 0x17000A8C RID: 2700
	// (get) Token: 0x060064F1 RID: 25841 RVA: 0x002775BC File Offset: 0x002757BC
	public static Mesh CubeMesh
	{
		get
		{
			return WaterDebugAssets.cubeMesh.Value;
		}
	}

	// Token: 0x060064F2 RID: 25842 RVA: 0x002775C8 File Offset: 0x002757C8
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

	// Token: 0x17000A8D RID: 2701
	// (get) Token: 0x060064F3 RID: 25843 RVA: 0x002776E4 File Offset: 0x002758E4
	public static Material DebugMaterial
	{
		get
		{
			return WaterDebugAssets.sharedMaterial.Value;
		}
	}

	// Token: 0x060064F4 RID: 25844 RVA: 0x002776F0 File Offset: 0x002758F0
	public static Material CreateDebugMaterial()
	{
		return new Material(Shader.Find("Debug/DebugInstancedProcedural"))
		{
			enableInstancing = true
		};
	}

	// Token: 0x04004E7D RID: 20093
	[PublicizedFrom(EAccessModifier.Private)]
	public static Lazy<Mesh> cubeMesh = new Lazy<Mesh>(new Func<Mesh>(WaterDebugAssets.GenerateCubeMesh));

	// Token: 0x04004E7E RID: 20094
	[PublicizedFrom(EAccessModifier.Private)]
	public static Lazy<Material> sharedMaterial = new Lazy<Material>(new Func<Material>(WaterDebugAssets.CreateDebugMaterial));
}
