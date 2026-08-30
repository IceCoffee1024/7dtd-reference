using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003B3 RID: 947
public class MeshLists
{
	// Token: 0x06001C64 RID: 7268 RVA: 0x000A84A2 File Offset: 0x000A66A2
	public static MeshLists GetList()
	{
		if (MeshLists.MeshListCache.Count == 0)
		{
			return new MeshLists();
		}
		return MeshLists.MeshListCache.Pop();
	}

	// Token: 0x06001C65 RID: 7269 RVA: 0x000A84C0 File Offset: 0x000A66C0
	public static void ReturnList(MeshLists list)
	{
		list.Reset();
		MeshLists.MeshListCache.Push(list);
		if (MeshLists.MeshListCache.Count > MeshLists.LastLargest)
		{
			MeshLists.LastLargest = MeshLists.MeshListCache.Count;
			Log.Out("Meshlist count is now " + MeshLists.MeshListCache.Count.ToString());
		}
	}

	// Token: 0x06001C66 RID: 7270 RVA: 0x000A8520 File Offset: 0x000A6720
	public void Reset()
	{
		this.Vertices.Clear();
		this.Uvs.Clear();
		this.Uvs2.Clear();
		this.Uvs3.Clear();
		this.Uvs4.Clear();
		this.Triangles.Clear();
		foreach (List<int> list in this.TerrainTriangles)
		{
			list.Clear();
		}
		this.TerrainTriangles.Clear();
		this.Colours.Clear();
		this.Normals.Clear();
		this.Tangents.Clear();
	}

	// Token: 0x0400125F RID: 4703
	public static Stack<MeshLists> MeshListCache = new Stack<MeshLists>();

	// Token: 0x04001260 RID: 4704
	public static int LastLargest = 0;

	// Token: 0x04001261 RID: 4705
	public List<Vector3> Vertices = new List<Vector3>();

	// Token: 0x04001262 RID: 4706
	public List<Vector2> Uvs = new List<Vector2>();

	// Token: 0x04001263 RID: 4707
	public List<Vector2> Uvs2 = new List<Vector2>();

	// Token: 0x04001264 RID: 4708
	public List<Vector2> Uvs3 = new List<Vector2>();

	// Token: 0x04001265 RID: 4709
	public List<Vector2> Uvs4 = new List<Vector2>();

	// Token: 0x04001266 RID: 4710
	public List<int> Triangles = new List<int>();

	// Token: 0x04001267 RID: 4711
	public List<List<int>> TerrainTriangles = new List<List<int>>();

	// Token: 0x04001268 RID: 4712
	public List<Color> Colours = new List<Color>();

	// Token: 0x04001269 RID: 4713
	public List<Vector3> Normals = new List<Vector3>();

	// Token: 0x0400126A RID: 4714
	public List<Vector4> Tangents = new List<Vector4>();
}
