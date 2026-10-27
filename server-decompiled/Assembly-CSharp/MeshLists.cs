using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003B8 RID: 952
public class MeshLists
{
	// Token: 0x06001C61 RID: 7265 RVA: 0x000A88F2 File Offset: 0x000A6AF2
	public static MeshLists GetList()
	{
		if (MeshLists.MeshListCache.Count == 0)
		{
			return new MeshLists();
		}
		return MeshLists.MeshListCache.Pop();
	}

	// Token: 0x06001C62 RID: 7266 RVA: 0x000A8910 File Offset: 0x000A6B10
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

	// Token: 0x06001C63 RID: 7267 RVA: 0x000A8970 File Offset: 0x000A6B70
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

	// Token: 0x04001268 RID: 4712
	public static Stack<MeshLists> MeshListCache = new Stack<MeshLists>();

	// Token: 0x04001269 RID: 4713
	public static int LastLargest = 0;

	// Token: 0x0400126A RID: 4714
	public List<Vector3> Vertices = new List<Vector3>();

	// Token: 0x0400126B RID: 4715
	public List<Vector2> Uvs = new List<Vector2>();

	// Token: 0x0400126C RID: 4716
	public List<Vector2> Uvs2 = new List<Vector2>();

	// Token: 0x0400126D RID: 4717
	public List<Vector2> Uvs3 = new List<Vector2>();

	// Token: 0x0400126E RID: 4718
	public List<Vector2> Uvs4 = new List<Vector2>();

	// Token: 0x0400126F RID: 4719
	public List<int> Triangles = new List<int>();

	// Token: 0x04001270 RID: 4720
	public List<List<int>> TerrainTriangles = new List<List<int>>();

	// Token: 0x04001271 RID: 4721
	public List<Color> Colours = new List<Color>();

	// Token: 0x04001272 RID: 4722
	public List<Vector3> Normals = new List<Vector3>();

	// Token: 0x04001273 RID: 4723
	public List<Vector4> Tangents = new List<Vector4>();
}
