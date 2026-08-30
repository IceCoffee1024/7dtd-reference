using System;
using UnityEngine;

// Token: 0x02000CCC RID: 3276
public static class WaterDebugAssets
{
	// Token: 0x17000A79 RID: 2681
	// (get) Token: 0x06006465 RID: 25701 RVA: 0x002746C0 File Offset: 0x002728C0
	public static Mesh CubeMesh
	{
		get
		{
			return WaterDebugAssets.cubeMesh.Value;
		}
	}

	// Token: 0x06006466 RID: 25702 RVA: 0x002746CC File Offset: 0x002728CC
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

	// Token: 0x17000A7A RID: 2682
	// (get) Token: 0x06006467 RID: 25703 RVA: 0x002747E8 File Offset: 0x002729E8
	public static Material DebugMaterial
	{
		get
		{
			return WaterDebugAssets.sharedMaterial.Value;
		}
	}

	// Token: 0x06006468 RID: 25704 RVA: 0x002747F4 File Offset: 0x002729F4
	public static Material CreateDebugMaterial()
	{
		return new Material(Shader.Find("Debug/DebugInstancedProcedural"))
		{
			enableInstancing = true
		};
	}

	// Token: 0x04004DDF RID: 19935
	[PublicizedFrom(EAccessModifier.Private)]
	public static Lazy<Mesh> cubeMesh = new Lazy<Mesh>(new Func<Mesh>(WaterDebugAssets.GenerateCubeMesh));

	// Token: 0x04004DE0 RID: 19936
	[PublicizedFrom(EAccessModifier.Private)]
	public static Lazy<Material> sharedMaterial = new Lazy<Material>(new Func<Material>(WaterDebugAssets.CreateDebugMaterial));
}
