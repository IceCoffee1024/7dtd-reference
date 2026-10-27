using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001366 RID: 4966
[ExecuteInEditMode]
public class DebugDrawNormals : MonoBehaviour
{
	// Token: 0x06009C33 RID: 39987 RVA: 0x003B0568 File Offset: 0x003AE768
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
		if (this.die)
		{
			global::UnityEngine.Object.DestroyImmediate(this);
			return;
		}
		this.TriangleCount = 0;
		this.VertCount = 0;
		MeshFilter component = base.GetComponent<MeshFilter>();
		if (component)
		{
			this.MeshCount = 1;
			this.Draw(component);
			return;
		}
		MeshFilter[] componentsInChildren = base.GetComponentsInChildren<MeshFilter>();
		this.MeshCount = componentsInChildren.Length;
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			this.Draw(componentsInChildren[i]);
		}
		if (componentsInChildren.Length == 0)
		{
			this.SetDie();
		}
	}

	// Token: 0x06009C34 RID: 39988 RVA: 0x003B05E4 File Offset: 0x003AE7E4
	[PublicizedFrom(EAccessModifier.Private)]
	public void Draw(MeshFilter mf)
	{
		Mesh sharedMesh = mf.sharedMesh;
		if (!sharedMesh)
		{
			return;
		}
		DebugDrawNormals.Data data;
		if (this.list.Count > 0 && !this.Record)
		{
			data = this.list[0];
		}
		else
		{
			data = new DebugDrawNormals.Data();
			this.list.Add(data);
			this.Record = false;
		}
		sharedMesh.GetVertices(data.verts);
		sharedMesh.GetNormals(data.normals);
		sharedMesh.GetTriangles(data.indices, 0);
		this.VertCount += data.verts.Count;
		this.TriangleCount += data.indices.Count / 3;
		Matrix4x4 localToWorldMatrix = mf.transform.localToWorldMatrix;
		for (int i = 0; i < this.list.Count; i++)
		{
			DebugDrawNormals.Data data2 = this.list[i];
			for (int j = 0; j < data2.normals.Count; j++)
			{
				Utils.DrawRay(localToWorldMatrix.MultiplyPoint(data2.verts[j]), localToWorldMatrix.MultiplyVector(data2.normals[j]) * this.VertexNormalScale, Color.white, Color.blue, 3, 0f);
			}
			for (int k = 0; k < data2.indices.Count - 2; k += 3)
			{
				Vector3 vector = data2.verts[data2.indices[k]];
				Vector3 vector2 = data2.verts[data2.indices[k + 1]];
				Vector3 vector3 = data2.verts[data2.indices[k + 2]];
				Vector3 vector4 = (vector + vector2 + vector3) * 0.33333334f;
				Vector3 normalized = Vector3.Cross(vector2 - vector, vector3 - vector).normalized;
				Utils.DrawRay(localToWorldMatrix.MultiplyPoint(vector4), localToWorldMatrix.MultiplyVector(normalized) * this.TriangleNormalScale, Color.yellow, Color.red, 3, 0f);
			}
		}
	}

	// Token: 0x06009C35 RID: 39989 RVA: 0x003B081C File Offset: 0x003AEA1C
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDisable()
	{
		if (!base.gameObject.activeInHierarchy)
		{
			this.SetDie();
		}
	}

	// Token: 0x06009C36 RID: 39990 RVA: 0x003B0831 File Offset: 0x003AEA31
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetDie()
	{
		this.list = null;
		this.die = true;
	}

	// Token: 0x040075C4 RID: 30148
	public float VertexNormalScale = 0.05f;

	// Token: 0x040075C5 RID: 30149
	public float TriangleNormalScale = 0.05f;

	// Token: 0x040075C6 RID: 30150
	public int MeshCount;

	// Token: 0x040075C7 RID: 30151
	public int TriangleCount;

	// Token: 0x040075C8 RID: 30152
	public int VertCount;

	// Token: 0x040075C9 RID: 30153
	public bool Record;

	// Token: 0x040075CA RID: 30154
	public List<DebugDrawNormals.Data> list = new List<DebugDrawNormals.Data>();

	// Token: 0x040075CB RID: 30155
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool die;

	// Token: 0x02001367 RID: 4967
	[Serializable]
	public class Data
	{
		// Token: 0x040075CC RID: 30156
		public List<Vector3> verts = new List<Vector3>();

		// Token: 0x040075CD RID: 30157
		public List<Vector3> normals = new List<Vector3>();

		// Token: 0x040075CE RID: 30158
		public List<int> indices = new List<int>();
	}
}
