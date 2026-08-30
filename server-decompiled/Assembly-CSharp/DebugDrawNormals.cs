using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200131A RID: 4890
[ExecuteInEditMode]
public class DebugDrawNormals : MonoBehaviour
{
	// Token: 0x06009A34 RID: 39476 RVA: 0x003A4350 File Offset: 0x003A2550
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

	// Token: 0x06009A35 RID: 39477 RVA: 0x003A43CC File Offset: 0x003A25CC
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

	// Token: 0x06009A36 RID: 39478 RVA: 0x003A4604 File Offset: 0x003A2804
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDisable()
	{
		if (!base.gameObject.activeInHierarchy)
		{
			this.SetDie();
		}
	}

	// Token: 0x06009A37 RID: 39479 RVA: 0x003A4619 File Offset: 0x003A2819
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetDie()
	{
		this.list = null;
		this.die = true;
	}

	// Token: 0x04007430 RID: 29744
	public float VertexNormalScale = 0.05f;

	// Token: 0x04007431 RID: 29745
	public float TriangleNormalScale = 0.05f;

	// Token: 0x04007432 RID: 29746
	public int MeshCount;

	// Token: 0x04007433 RID: 29747
	public int TriangleCount;

	// Token: 0x04007434 RID: 29748
	public int VertCount;

	// Token: 0x04007435 RID: 29749
	public bool Record;

	// Token: 0x04007436 RID: 29750
	public List<DebugDrawNormals.Data> list = new List<DebugDrawNormals.Data>();

	// Token: 0x04007437 RID: 29751
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool die;

	// Token: 0x0200131B RID: 4891
	[Serializable]
	public class Data
	{
		// Token: 0x04007438 RID: 29752
		public List<Vector3> verts = new List<Vector3>();

		// Token: 0x04007439 RID: 29753
		public List<Vector3> normals = new List<Vector3>();

		// Token: 0x0400743A RID: 29754
		public List<int> indices = new List<int>();
	}
}
