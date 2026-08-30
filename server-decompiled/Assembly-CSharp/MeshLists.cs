using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003AF RID: 943
public class MeshLists
{
	// Token: 0x06001C47 RID: 7239 RVA: 0x000A7D62 File Offset: 0x000A5F62
	public static MeshLists GetList()
	{
		if (MeshLists.MeshListCache.Count == 0)
		{
			return new MeshLists();
		}
		return MeshLists.MeshListCache.Pop();
	}

	// Token: 0x06001C48 RID: 7240 RVA: 0x000A7D80 File Offset: 0x000A5F80
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

	// Token: 0x06001C49 RID: 7241 RVA: 0x000A7DE0 File Offset: 0x000A5FE0
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

	// Token: 0x0400124A RID: 4682
	public static Stack<MeshLists> MeshListCache = new Stack<MeshLists>();

	// Token: 0x0400124B RID: 4683
	public static int LastLargest = 0;

	// Token: 0x0400124C RID: 4684
	public List<Vector3> Vertices = new List<Vector3>();

	// Token: 0x0400124D RID: 4685
	public List<Vector2> Uvs = new List<Vector2>();

	// Token: 0x0400124E RID: 4686
	public List<Vector2> Uvs2 = new List<Vector2>();

	// Token: 0x0400124F RID: 4687
	public List<Vector2> Uvs3 = new List<Vector2>();

	// Token: 0x04001250 RID: 4688
	public List<Vector2> Uvs4 = new List<Vector2>();

	// Token: 0x04001251 RID: 4689
	public List<int> Triangles = new List<int>();

	// Token: 0x04001252 RID: 4690
	public List<List<int>> TerrainTriangles = new List<List<int>>();

	// Token: 0x04001253 RID: 4691
	public List<Color> Colours = new List<Color>();

	// Token: 0x04001254 RID: 4692
	public List<Vector3> Normals = new List<Vector3>();

	// Token: 0x04001255 RID: 4693
	public List<Vector4> Tangents = new List<Vector4>();
}
