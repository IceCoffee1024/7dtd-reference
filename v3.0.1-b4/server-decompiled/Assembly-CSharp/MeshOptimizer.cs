using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B64 RID: 2916
public class MeshOptimizer
{
	// Token: 0x06005845 RID: 22597 RVA: 0x0021B9AE File Offset: 0x00219BAE
	public void Optimize(List<Vector3> _vertices, List<int> _indices, float _maxEdgeCost, out Vector3[] optimizedVerts, out int[] optimizedIndices)
	{
		this.AllocateArrays(_vertices.Count, _indices.Count / 3);
		this.BuildEdgeList(_vertices, _indices);
		this.MarkBoundaryEdges();
		this.BuildSortedEdgeCollapseList();
		this.CollapseEdges(_maxEdgeCost);
		this.GenerateOutputMesh(_vertices, _indices, out optimizedVerts, out optimizedIndices);
	}

	// Token: 0x06005846 RID: 22598 RVA: 0x0021B9EC File Offset: 0x00219BEC
	[PublicizedFrom(EAccessModifier.Private)]
	public void BuildEdgeList(List<Vector3> _vertices, List<int> _indices)
	{
		for (int i = 0; i < _indices.Count; i += 3)
		{
			this.AddTri(_vertices, _indices, i);
		}
	}

	// Token: 0x06005847 RID: 22599 RVA: 0x0021BA14 File Offset: 0x00219C14
	[PublicizedFrom(EAccessModifier.Private)]
	public int CollapseEdges(float _maxEdgeCost)
	{
		int num = 0;
		while (this.NumOptimizedTris > 2 && this.NumSortedEdges > 0)
		{
			for (int i = 0; i < this.NumVerts; i++)
			{
				int num2 = this.SortedEdgeCost[i];
				if (!this.VertexList[num2].collapsed)
				{
					if (this.VertexList[num2].collapseCost > _maxEdgeCost)
					{
						this.Iterations++;
						this.FindBestVertexCollapse(5);
						return num;
					}
					int collapseEdge = this.VertexList[num2].collapseEdge;
					if (!this.EdgeList[collapseEdge].collapsed)
					{
						int num3 = ((this.EdgeList[collapseEdge].v0 == num2) ? this.EdgeList[collapseEdge].v1 : this.EdgeList[collapseEdge].v0);
						this.CollapseVertex(num2, num3, collapseEdge);
						this.NumSortedEdges--;
						num++;
						break;
					}
					this.Iterations++;
					if (this.FindBestVertexCollapse(num2))
					{
						this.ResortVert(num2);
						break;
					}
				}
			}
		}
		return num;
	}

	// Token: 0x06005848 RID: 22600 RVA: 0x0021BB44 File Offset: 0x00219D44
	[PublicizedFrom(EAccessModifier.Private)]
	public void SwapSortedVert(int v0, int v1)
	{
		int num = this.SortedEdgeCost[v0];
		this.SortedEdgeCost[v0] = this.SortedEdgeCost[v1];
		this.VertexList[this.SortedEdgeCost[v0]].sortedPosition = v0;
		this.SortedEdgeCost[v1] = num;
		this.VertexList[this.SortedEdgeCost[v1]].sortedPosition = v1;
	}

	// Token: 0x06005849 RID: 22601 RVA: 0x0021BBA8 File Offset: 0x00219DA8
	[PublicizedFrom(EAccessModifier.Private)]
	public int NextSortedVert(int v, int dir)
	{
		v += dir;
		while (v >= 0 && v < this.NumVerts)
		{
			int num = this.SortedEdgeCost[v];
			if (!this.VertexList[num].collapsed)
			{
				return v;
			}
			v += dir;
		}
		return -1;
	}

	// Token: 0x0600584A RID: 22602 RVA: 0x0021BBF0 File Offset: 0x00219DF0
	[PublicizedFrom(EAccessModifier.Private)]
	public void ValidateSort()
	{
		for (int i = 0; i < this.NumVerts; i++)
		{
			if (!this.VertexList[i].collapsed && i != this.SortedEdgeCost[this.VertexList[i].sortedPosition])
			{
				throw new Exception("Failed SANITY_CHECK in MeshOptimizer!");
			}
		}
		float num = float.MaxValue;
		int num2 = 0;
		for (int j = 0; j < this.NumVerts; j++)
		{
			if (!this.VertexList[this.SortedEdgeCost[j]].collapsed)
			{
				num = this.VertexList[this.SortedEdgeCost[j]].collapseCost;
				num2 = 0;
				break;
			}
		}
		for (int k = num2; k < this.NumVerts; k++)
		{
			if (!this.VertexList[this.SortedEdgeCost[k]].collapsed)
			{
				if (num > this.VertexList[this.SortedEdgeCost[k]].collapseCost)
				{
					throw new Exception("Failed SANITY_CHECK in MeshOptimizer!");
				}
				num = this.VertexList[this.SortedEdgeCost[k]].collapseCost;
			}
		}
	}

	// Token: 0x0600584B RID: 22603 RVA: 0x0021BD08 File Offset: 0x00219F08
	[PublicizedFrom(EAccessModifier.Private)]
	public void ResortVert(int v)
	{
		v = this.VertexList[v].sortedPosition;
		int num = 0;
		for (;;)
		{
			int num2 = v;
			int num3 = this.SortedEdgeCost[v];
			if (v > 0 && num != 1)
			{
				int num4 = this.NextSortedVert(v, -1);
				if (num4 != -1)
				{
					int num5 = this.SortedEdgeCost[num4];
					if (this.VertexList[num3].collapseCost < this.VertexList[num5].collapseCost)
					{
						v = num4;
						num = -1;
					}
				}
			}
			if (v < this.NumVerts - 1 && num != -1)
			{
				int num6 = this.NextSortedVert(v, 1);
				if (num6 != -1)
				{
					int num7 = this.SortedEdgeCost[num6];
					if (this.VertexList[num3].collapseCost > this.VertexList[num7].collapseCost)
					{
						v = num6;
						num = 1;
					}
				}
			}
			if (num2 == v)
			{
				break;
			}
			this.SwapSortedVert(num2, v);
		}
	}

	// Token: 0x0600584C RID: 22604 RVA: 0x0021BDE4 File Offset: 0x00219FE4
	[PublicizedFrom(EAccessModifier.Private)]
	public void MarkBoundaryEdge(int edge)
	{
		if (this.EdgeList[edge].collapsed)
		{
			return;
		}
		int firstTri = this.EdgeList[edge].firstTri;
		int numTris = this.EdgeList[edge].numTris;
		int num = 0;
		for (int i = 0; i < numTris; i++)
		{
			int num2 = this.ComponentList[firstTri + i];
			if (!this.TriList[num2].collapsed)
			{
				num++;
				if (num > 1)
				{
					break;
				}
			}
		}
		this.EdgeList[edge].onModelEdge = num < 2;
	}

	// Token: 0x0600584D RID: 22605 RVA: 0x0021BE74 File Offset: 0x0021A074
	[PublicizedFrom(EAccessModifier.Private)]
	public void CollapseEdgeList(int v)
	{
		int firstEdge = this.VertexList[v].firstEdge;
		int numEdges = this.VertexList[v].numEdges;
		for (int i = 0; i < numEdges; i++)
		{
			this.EdgeList[this.ComponentList[firstEdge + i]].collapsed = true;
		}
	}

	// Token: 0x0600584E RID: 22606 RVA: 0x0021BECC File Offset: 0x0021A0CC
	[PublicizedFrom(EAccessModifier.Private)]
	public void CollapseTri(int tri)
	{
		this.TriList[tri].collapsed = true;
		this.CollapseEdge(this.TriList[tri].e0);
		this.CollapseEdge(this.TriList[tri].e1);
		this.CollapseEdge(this.TriList[tri].e2);
		this.CollapseVert(this.TriList[tri].v0);
		this.CollapseVert(this.TriList[tri].v1);
		this.CollapseVert(this.TriList[tri].v2);
	}

	// Token: 0x0600584F RID: 22607 RVA: 0x0021BF78 File Offset: 0x0021A178
	[PublicizedFrom(EAccessModifier.Private)]
	public void CollapseEdge(int edge)
	{
		if (this.EdgeList[edge].collapsed)
		{
			return;
		}
		int firstTri = this.EdgeList[edge].firstTri;
		int numTris = this.EdgeList[edge].numTris;
		bool flag = true;
		for (int i = 0; i < numTris; i++)
		{
			int num = this.ComponentList[firstTri + i];
			if (!this.TriList[num].collapsed)
			{
				flag = false;
				break;
			}
		}
		this.EdgeList[edge].collapsed = flag;
	}

	// Token: 0x06005850 RID: 22608 RVA: 0x0021C004 File Offset: 0x0021A204
	[PublicizedFrom(EAccessModifier.Private)]
	public void CollapseVert(int v)
	{
		if (this.VertexList[v].collapsed)
		{
			return;
		}
		int firstEdge = this.VertexList[v].firstEdge;
		int numEdges = this.VertexList[v].numEdges;
		bool flag = true;
		for (int i = 0; i < numEdges; i++)
		{
			int num = this.ComponentList[firstEdge + i];
			if (!this.EdgeList[num].collapsed)
			{
				flag = false;
				break;
			}
		}
		this.VertexList[v].collapsed = flag;
	}

	// Token: 0x06005851 RID: 22609 RVA: 0x0021C090 File Offset: 0x0021A290
	[PublicizedFrom(EAccessModifier.Private)]
	public void CollapseVertex(int v0, int v1, int edge)
	{
		int firstTri = this.EdgeList[edge].firstTri;
		int numTris = this.EdgeList[edge].numTris;
		for (int i = 0; i < numTris; i++)
		{
			int num = this.ComponentList[firstTri + i];
			if (!this.TriList[num].collapsed)
			{
				this.CollapseTri(num);
				this.NumOptimizedTris--;
			}
		}
		this.VertexList[v1].collapsed = false;
		this.MergeEdgeList(v0, v1);
		this.MarkBoundaryEdges(v1);
		this.VertexList[v0].collapsed = true;
		this.NumOptimizedVerts--;
		this.Iterations++;
		int firstTri2 = this.VertexList[v1].firstTri;
		int numTris2 = this.VertexList[v1].numTris;
		for (int j = 0; j < numTris2; j++)
		{
			int num2 = this.ComponentList[firstTri2 + j];
			if (!this.TriList[num2].collapsed)
			{
				if (this.FindBestVertexCollapse(this.TriList[num2].v0))
				{
					this.ResortVert(this.TriList[num2].v0);
				}
				if (this.FindBestVertexCollapse(this.TriList[num2].v1))
				{
					this.ResortVert(this.TriList[num2].v1);
				}
				if (this.FindBestVertexCollapse(this.TriList[num2].v2))
				{
					this.ResortVert(this.TriList[num2].v2);
				}
			}
		}
		if (this.VertexList[v1].iterations != this.Iterations && this.FindBestVertexCollapse(v1))
		{
			this.ResortVert(v1);
		}
	}

	// Token: 0x06005852 RID: 22610 RVA: 0x0021C274 File Offset: 0x0021A474
	[PublicizedFrom(EAccessModifier.Private)]
	public void MarkBoundaryEdges(int v)
	{
		int firstEdge = this.VertexList[v].firstEdge;
		int numEdges = this.VertexList[v].numEdges;
		for (int i = 0; i < numEdges; i++)
		{
			int num = this.ComponentList[firstEdge + i];
			this.MarkBoundaryEdge(num);
		}
		this.MarkBoundaryVert(v);
		for (int j = 0; j < numEdges; j++)
		{
			int num2 = this.ComponentList[firstEdge + j];
			int v2 = this.EdgeList[num2].v0;
			int v3 = this.EdgeList[num2].v1;
			if (v2 == v)
			{
				this.MarkBoundaryVert(v3);
			}
			else
			{
				this.MarkBoundaryVert(v2);
			}
		}
	}

	// Token: 0x06005853 RID: 22611 RVA: 0x0021C328 File Offset: 0x0021A528
	[PublicizedFrom(EAccessModifier.Private)]
	public void MarkBoundaryVert(int v)
	{
		int firstEdge = this.VertexList[v].firstEdge;
		int numEdges = this.VertexList[v].numEdges;
		this.VertexList[v].onModelEdge = false;
		for (int i = 0; i < numEdges; i++)
		{
			int num = this.ComponentList[firstEdge + i];
			if (!this.EdgeList[num].collapsed && this.EdgeList[num].onModelEdge)
			{
				this.VertexList[v].onModelEdge = true;
				return;
			}
		}
	}

	// Token: 0x06005854 RID: 22612 RVA: 0x0021C3BC File Offset: 0x0021A5BC
	[PublicizedFrom(EAccessModifier.Private)]
	public bool EdgeWillMerge(int v0, int v1, int edge)
	{
		if (this.EdgeList[edge].collapsed)
		{
			return true;
		}
		int num = ((this.EdgeList[edge].v0 == v0) ? this.EdgeList[edge].v1 : this.EdgeList[edge].v0);
		int firstEdge = this.VertexList[v1].firstEdge;
		int numEdges = this.VertexList[v1].numEdges;
		for (int i = 0; i < numEdges; i++)
		{
			int num2 = this.ComponentList[firstEdge + i];
			if (!this.EdgeList[num2].collapsed && ((this.EdgeList[num2].v0 == v1 && this.EdgeList[num2].v1 == num) || (this.EdgeList[num2].v0 == num && this.EdgeList[num2].v1 == v1)))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06005855 RID: 22613 RVA: 0x0021C4C0 File Offset: 0x0021A6C0
	[PublicizedFrom(EAccessModifier.Private)]
	public void MergeEdgeList(int v0, int v1)
	{
		this.CollapseEdgeList(v0);
		int firstTri = this.VertexList[v0].firstTri;
		int numTris = this.VertexList[v0].numTris;
		int num = 0;
		for (int i = 0; i < numTris; i++)
		{
			int num2 = this.ComponentList[firstTri + i];
			if (!this.TriList[num2].collapsed)
			{
				if (this.TriList[num2].v0 == v0)
				{
					this.TriList[num2].e0 = this.AddEdge(v1, this.TriList[num2].v1, num2);
					this.TriList[num2].e2 = this.AddEdge(this.TriList[num2].v2, v1, num2);
					this.TriList[num2].v0 = v1;
				}
				else if (this.TriList[num2].v1 == v0)
				{
					this.TriList[num2].e0 = this.AddEdge(this.TriList[num2].v0, v1, num2);
					this.TriList[num2].e1 = this.AddEdge(v1, this.TriList[num2].v2, num2);
					this.TriList[num2].v1 = v1;
				}
				else
				{
					if (this.TriList[num2].v2 != v0)
					{
						goto IL_0305;
					}
					this.TriList[num2].e1 = this.AddEdge(this.TriList[num2].v1, v1, num2);
					this.TriList[num2].e2 = this.AddEdge(v1, this.TriList[num2].v0, num2);
					this.TriList[num2].v2 = v1;
				}
				if (this.TriList[num2].collapsed)
				{
					this.NumOptimizedTris--;
				}
				else
				{
					this.VertexList[v1].collapsed = false;
					this.AddVertex(v1, num2, null, true);
					int e = this.TriList[num2].e0;
					int e2 = this.TriList[num2].e1;
					Vector3 vector = this.EdgeList[e].normal;
					Vector3 vector2 = this.EdgeList[e2].normal;
					if (this.EdgeList[e].v0 != this.TriList[num2].v0)
					{
						vector = -vector;
					}
					if (this.EdgeList[e2].v0 != this.TriList[num2].v1)
					{
						vector2 = -vector2;
					}
					this.TriList[num2].normal = Vector3.Cross(vector, vector2).normalized;
					num++;
				}
			}
			IL_0305:;
		}
	}

	// Token: 0x06005856 RID: 22614 RVA: 0x0021C7E0 File Offset: 0x0021A9E0
	[PublicizedFrom(EAccessModifier.Private)]
	public void GenerateOutputMesh(List<Vector3> _vertices, List<int> _indices, out Vector3[] optimizedVerts, out int[] optimizedIndices)
	{
		optimizedVerts = new Vector3[this.NumOptimizedVerts];
		optimizedIndices = new int[this.NumOptimizedTris * 3];
		int num = 0;
		for (int i = 0; i < this.NumVerts; i++)
		{
			if (!this.VertexList[i].collapsed)
			{
				this.VertexList[i].emitIndex = num;
				optimizedVerts[num] = this.VertexList[i].pos;
				num++;
			}
		}
		int num2 = 0;
		for (int j = 0; j < this.NumTris; j++)
		{
			if (!this.TriList[j].collapsed)
			{
				optimizedIndices[num2] = this.VertexList[this.TriList[j].v0].emitIndex;
				optimizedIndices[num2 + 1] = this.VertexList[this.TriList[j].v1].emitIndex;
				optimizedIndices[num2 + 2] = this.VertexList[this.TriList[j].v2].emitIndex;
				num2 += 3;
			}
		}
	}

	// Token: 0x06005857 RID: 22615 RVA: 0x0021C908 File Offset: 0x0021AB08
	[PublicizedFrom(EAccessModifier.Private)]
	public void BuildSortedEdgeCollapseList()
	{
		this.Iterations++;
		this.SortedEdgeCost = new int[this.NumVerts];
		for (int i = 0; i < this.NumVerts; i++)
		{
			this.FindBestVertexCollapse(i);
			this.SortedEdgeCost[i] = i;
		}
		this.NumSortedEdges = this.NumVerts;
		Array.Sort<int>(this.SortedEdgeCost, (int a, int b) => this.VertexList[a].collapseCost.CompareTo(this.VertexList[b].collapseCost));
		for (int j = 0; j < this.NumVerts; j++)
		{
			this.VertexList[this.SortedEdgeCost[j]].sortedPosition = j;
		}
	}

	// Token: 0x06005858 RID: 22616 RVA: 0x0021C9A4 File Offset: 0x0021ABA4
	[PublicizedFrom(EAccessModifier.Private)]
	public void MarkBoundaryEdges()
	{
		for (int i = 0; i < this.NumEdges; i++)
		{
			if (this.EdgeList[i].numTris < 2)
			{
				this.EdgeList[i].onModelEdge = true;
				this.VertexList[this.EdgeList[i].v0].onModelEdge = true;
				this.VertexList[this.EdgeList[i].v1].onModelEdge = true;
			}
		}
	}

	// Token: 0x06005859 RID: 22617 RVA: 0x0021CA2C File Offset: 0x0021AC2C
	[PublicizedFrom(EAccessModifier.Private)]
	public bool FindBestVertexCollapse(int v)
	{
		if (this.VertexList[v].iterations >= this.Iterations)
		{
			return false;
		}
		this.VertexList[v].iterations = this.Iterations;
		this.VertexList[v].collapseEdge = -1;
		if (this.VertexList[v].collapsed)
		{
			this.VertexList[v].collapseCost = float.MaxValue;
			return true;
		}
		bool onModelEdge = this.VertexList[v].onModelEdge;
		float num = float.MaxValue;
		int firstEdge = this.VertexList[v].firstEdge;
		int numEdges = this.VertexList[v].numEdges;
		for (int i = 0; i < numEdges; i++)
		{
			int num2 = this.ComponentList[firstEdge + i];
			MeshOptimizer.Edge edge = this.EdgeList[num2];
			if (!edge.collapsed && (!onModelEdge || edge.onModelEdge))
			{
				int num3 = edge.v0;
				int num4 = edge.v1;
				if (num4 == v)
				{
					num4 = num3;
					num3 = v;
				}
				float num5 = this.CalculateEdgeCost(num3, num4, num2);
				if (num5 < num)
				{
					num = num5;
					this.VertexList[v].collapseEdge = num2;
				}
			}
		}
		this.VertexList[v].collapseCost = num;
		return true;
	}

	// Token: 0x0600585A RID: 22618 RVA: 0x0021CB84 File Offset: 0x0021AD84
	[PublicizedFrom(EAccessModifier.Private)]
	public float CalculateBoundaryEdgeCost(int v0, int v1, int edge)
	{
		float num = 0f;
		int firstEdge = this.VertexList[v0].firstEdge;
		int numEdges = this.VertexList[v0].numEdges;
		Vector3 normal = this.EdgeList[edge].normal;
		float len = this.EdgeList[edge].len;
		for (int i = 0; i < numEdges; i++)
		{
			int num2 = this.ComponentList[firstEdge + i];
			if (num2 != edge && !this.EdgeList[num2].collapsed && this.EdgeList[num2].onModelEdge)
			{
				float num3 = Mathf.Abs(Vector3.Dot(normal, this.EdgeList[num2].normal));
				num = Mathf.Max(num, (1f - num3) * (len + this.EdgeList[num2].len));
			}
		}
		return num;
	}

	// Token: 0x0600585B RID: 22619 RVA: 0x0021CC78 File Offset: 0x0021AE78
	[PublicizedFrom(EAccessModifier.Private)]
	public bool DetectDegenerateCollapse(int v0, int v1, int edge)
	{
		int firstTri = this.VertexList[v0].firstTri;
		int numTris = this.VertexList[v0].numTris;
		for (int i = 0; i < numTris; i++)
		{
			int num = this.ComponentList[firstTri + i];
			if (!this.TriList[num].collapsed && this.TriList[num].e0 != edge && this.TriList[num].e1 != edge && this.TriList[num].e2 != edge)
			{
				if (this.TriList[num].v0 == v0)
				{
					Vector3 vector = this.VertexList[this.TriList[num].v1].pos - this.VertexList[v1].pos;
					if (vector.sqrMagnitude < 0.0001f)
					{
						return true;
					}
					vector.Normalize();
					if (Mathf.Abs(Vector3.Dot(vector, this.EdgeList[this.TriList[num].e1].normal)) >= this.DegenerateEpsilon)
					{
						return true;
					}
					if (this.EdgeList[this.TriList[num].e1].v0 != this.TriList[num].v1)
					{
						vector = -vector;
					}
					if (Vector3.Dot(Vector3.Cross(vector, this.EdgeList[this.TriList[num].e1].normal), this.TriList[num].normal) < 0f)
					{
						return true;
					}
				}
				else if (this.TriList[num].v1 == v0)
				{
					Vector3 vector2 = this.VertexList[this.TriList[num].v2].pos - this.VertexList[v1].pos;
					if (vector2.sqrMagnitude < 0.0001f)
					{
						return true;
					}
					vector2.Normalize();
					if (Mathf.Abs(Vector3.Dot(vector2, this.EdgeList[this.TriList[num].e2].normal)) >= this.DegenerateEpsilon)
					{
						return true;
					}
					if (this.EdgeList[this.TriList[num].e2].v0 != this.TriList[num].v2)
					{
						vector2 = -vector2;
					}
					if (Vector3.Dot(Vector3.Cross(vector2, this.EdgeList[this.TriList[num].e2].normal), this.TriList[num].normal) < 0f)
					{
						return true;
					}
				}
				else if (this.TriList[num].v2 == v0)
				{
					Vector3 vector3 = this.VertexList[this.TriList[num].v0].pos - this.VertexList[v1].pos;
					if (vector3.sqrMagnitude < 0.0001f)
					{
						return true;
					}
					vector3.Normalize();
					if (Mathf.Abs(Vector3.Dot(vector3, this.EdgeList[this.TriList[num].e0].normal)) >= this.DegenerateEpsilon)
					{
						return true;
					}
					if (this.EdgeList[this.TriList[num].e0].v0 != this.TriList[num].v0)
					{
						vector3 = -vector3;
					}
					if (Vector3.Dot(Vector3.Cross(vector3, this.EdgeList[this.TriList[num].e0].normal), this.TriList[num].normal) < 0f)
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	// Token: 0x0600585C RID: 22620 RVA: 0x0021D090 File Offset: 0x0021B290
	[PublicizedFrom(EAccessModifier.Private)]
	public float CalculateEdgeCost(int v0, int v1, int edge)
	{
		if (this.EdgeList[edge].collapsed || this.DetectDegenerateCollapse(v0, v1, edge))
		{
			return float.MaxValue;
		}
		if (this.EdgeList[edge].onModelEdge)
		{
			return this.CalculateBoundaryEdgeCost(v0, v1, edge);
		}
		float num = 0f;
		int firstTri = this.EdgeList[edge].firstTri;
		int numTris = this.EdgeList[edge].numTris;
		int firstTri2 = this.VertexList[v0].firstTri;
		int numTris2 = this.VertexList[v0].numTris;
		for (int i = 0; i < numTris2; i++)
		{
			int num2 = this.ComponentList[firstTri2 + i];
			if (!this.TriList[num2].collapsed && this.TriList[num2].e0 != edge && this.TriList[num2].e1 != edge && this.TriList[num2].e2 != edge)
			{
				float num3 = 1f;
				for (int j = 0; j < numTris; j++)
				{
					int num4 = this.ComponentList[firstTri + j];
					if (num4 != num2 && !this.TriList[num4].collapsed)
					{
						float num5 = Vector3.Dot(this.TriList[num4].normal, this.TriList[num2].normal);
						num3 = Mathf.Min(num3, (1f - num5) / 2f);
					}
				}
				num = Mathf.Max(num, num3);
			}
		}
		return this.EdgeList[edge].len * num;
	}

	// Token: 0x0600585D RID: 22621 RVA: 0x0021D24C File Offset: 0x0021B44C
	[PublicizedFrom(EAccessModifier.Private)]
	public void AddTri(List<Vector3> _vertices, List<int> _indices, int index)
	{
		int num = index / 3;
		this.TriList[num].collapsed = false;
		int num2 = this.AddVertex(_indices[index], num, _vertices, true);
		int num3 = this.AddVertex(_indices[index + 1], num, _vertices, true);
		int num4 = this.AddVertex(_indices[index + 2], num, _vertices, true);
		this.TriList[num].v0 = num2;
		this.TriList[num].v1 = num3;
		this.TriList[num].v2 = num4;
		this.TriList[num].e0 = -1;
		this.TriList[num].e1 = -1;
		this.TriList[num].e2 = -1;
		int num5 = this.AddEdge(num2, num3, num);
		this.TriList[num].e0 = num5;
		int num6 = this.AddEdge(num3, num4, num);
		this.TriList[num].e1 = num6;
		int num7 = this.AddEdge(num4, num2, num);
		this.TriList[num].e2 = num7;
		Vector3 vector = this.EdgeList[num5].normal;
		Vector3 vector2 = this.EdgeList[num6].normal;
		if (this.EdgeList[num5].v0 != num2)
		{
			vector = -vector;
		}
		if (this.EdgeList[num6].v0 != num3)
		{
			vector2 = -vector2;
		}
		this.TriList[num].normal = Vector3.Cross(vector, vector2).normalized;
		this.NumTris++;
		this.NumOptimizedTris++;
	}

	// Token: 0x0600585E RID: 22622 RVA: 0x0021D410 File Offset: 0x0021B610
	[PublicizedFrom(EAccessModifier.Private)]
	public void AddEdgeToVertex(int v, int edge)
	{
		bool flag = false;
		int firstEdge = this.VertexList[v].firstEdge;
		int numEdges = this.VertexList[v].numEdges;
		for (int i = 0; i < numEdges; i++)
		{
			int num = this.ComponentList[firstEdge + i];
			if (this.EdgeList[num].collapsed)
			{
				this.ComponentList[firstEdge + i] = edge;
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			if (this.VertexList[v].numEdges == 256)
			{
				this.VertexList[v].collapsed = true;
				return;
			}
			this.ComponentList[this.VertexList[v].firstEdge + this.VertexList[v].numEdges] = edge;
			MeshOptimizer.Vertex[] vertexList = this.VertexList;
			vertexList[v].numEdges = vertexList[v].numEdges + 1;
		}
	}

	// Token: 0x0600585F RID: 22623 RVA: 0x0021D4F0 File Offset: 0x0021B6F0
	[PublicizedFrom(EAccessModifier.Private)]
	public int AddEdge(int v0, int v1, int tri)
	{
		int num = this.FindTriEdge(v0, v1);
		if (num == -1)
		{
			int numEdges = this.NumEdges;
			this.NumEdges = numEdges + 1;
			num = numEdges;
			this.EdgeList[num].collapsed = false;
			this.EdgeList[num].onModelEdge = false;
			this.EdgeList[num].v0 = v0;
			this.EdgeList[num].v1 = v1;
			this.EdgeList[num].vec = this.VertexList[v1].pos - this.VertexList[v0].pos;
			this.EdgeList[num].normal = this.EdgeList[num].vec.normalized;
			this.EdgeList[num].firstTri = this.AllocateComponents(8);
			this.EdgeList[num].numTris = 0;
			this.EdgeList[num].len = this.EdgeList[num].vec.magnitude;
			this.AddEdgeToVertex(v0, num);
			this.AddEdgeToVertex(v1, num);
		}
		bool flag = false;
		int firstTri = this.EdgeList[num].firstTri;
		int numTris = this.EdgeList[num].numTris;
		for (int i = 0; i < numTris; i++)
		{
			int num2 = this.ComponentList[firstTri + i];
			if (this.TriList[num2].collapsed)
			{
				this.ComponentList[firstTri + i] = tri;
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			if (this.EdgeList[num].numTris == 8)
			{
				this.EdgeList[num].collapsed = true;
				return num;
			}
			this.ComponentList[this.EdgeList[num].firstTri + this.EdgeList[num].numTris] = tri;
			MeshOptimizer.Edge[] edgeList = this.EdgeList;
			int num3 = num;
			edgeList[num3].numTris = edgeList[num3].numTris + 1;
		}
		return num;
	}

	// Token: 0x06005860 RID: 22624 RVA: 0x0021D700 File Offset: 0x0021B900
	[PublicizedFrom(EAccessModifier.Private)]
	public int FindTriEdge(int v0, int v1)
	{
		int firstTri = this.VertexList[v0].firstTri;
		for (int i = 0; i < this.VertexList[v0].numTris; i++)
		{
			int num = this.ComponentList[firstTri + i];
			if (!this.TriList[num].collapsed)
			{
				int v2 = this.TriList[num].v0;
				int v3 = this.TriList[num].v1;
				int v4 = this.TriList[num].v2;
				if (((v2 == v0 && v3 == v1) || (v2 == v1 && v3 == v0)) && this.TriList[num].e0 != -1)
				{
					return this.TriList[num].e0;
				}
				if (((v3 == v0 && v4 == v1) || (v3 == v1 && v4 == v0)) && this.TriList[num].e1 != -1)
				{
					return this.TriList[num].e1;
				}
				if (((v4 == v0 && v2 == v1) || (v4 == v1 && v2 == v0)) && this.TriList[num].e2 != -1)
				{
					return this.TriList[num].e2;
				}
			}
		}
		return -1;
	}

	// Token: 0x06005861 RID: 22625 RVA: 0x0021D848 File Offset: 0x0021BA48
	[PublicizedFrom(EAccessModifier.Private)]
	public int AddVertex(int v, int tri, List<Vector3> _vertices, bool addTriToVerts)
	{
		if (this.VertexList[v].collapsed)
		{
			if (_vertices == null)
			{
				throw new Exception("Error: model vertex not found!");
			}
			this.NumVerts = ((v + 1 > this.NumVerts) ? (v + 1) : this.NumVerts);
			this.NumOptimizedVerts = this.NumVerts;
			this.VertexList[v].collapsed = false;
			this.VertexList[v].onModelEdge = false;
			this.VertexList[v].firstTri = this.AllocateComponents(256);
			this.VertexList[v].numTris = 0;
			this.VertexList[v].firstEdge = this.AllocateComponents(256);
			this.VertexList[v].numEdges = 0;
			this.VertexList[v].iterations = 0;
			this.VertexList[v].sortedPosition = -1;
			this.VertexList[v].pos = _vertices[v];
		}
		if (addTriToVerts)
		{
			bool flag = false;
			int firstTri = this.VertexList[v].firstTri;
			int numTris = this.VertexList[v].numTris;
			for (int i = 0; i < numTris; i++)
			{
				int num = this.ComponentList[firstTri + i];
				if (this.TriList[num].collapsed)
				{
					this.ComponentList[firstTri + i] = tri;
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				if (this.VertexList[v].numTris == 256)
				{
					this.VertexList[v].collapsed = true;
					return v;
				}
				this.ComponentList[this.VertexList[v].firstTri + this.VertexList[v].numTris] = tri;
				MeshOptimizer.Vertex[] vertexList = this.VertexList;
				vertexList[v].numTris = vertexList[v].numTris + 1;
			}
		}
		return v;
	}

	// Token: 0x06005862 RID: 22626 RVA: 0x0021DA36 File Offset: 0x0021BC36
	[PublicizedFrom(EAccessModifier.Private)]
	public int AllocateComponents(int num)
	{
		int numComponents = this.NumComponents;
		this.NumComponents += num;
		return numComponents;
	}

	// Token: 0x06005863 RID: 22627 RVA: 0x0021DA4C File Offset: 0x0021BC4C
	[PublicizedFrom(EAccessModifier.Private)]
	public void AllocateArrays(int _numVerts, int _numTriangles)
	{
		this.NumVerts = 0;
		this.NumTris = 0;
		this.NumEdges = 0;
		this.NumComponents = 0;
		this.NumOptimizedTris = 0;
		this.NumOptimizedVerts = 0;
		int num = _numTriangles * 8 * 2;
		if (this.VertexList == null || this.VertexList.Length < _numVerts)
		{
			this.VertexList = new MeshOptimizer.Vertex[_numVerts];
		}
		if (this.EdgeList == null || this.EdgeList.Length < num)
		{
			this.EdgeList = new MeshOptimizer.Edge[num];
		}
		if (this.TriList == null || this.TriList.Length < _numTriangles)
		{
			this.TriList = new MeshOptimizer.Tri[_numTriangles];
		}
		int num2 = _numVerts * 256 * 2 + num * 8;
		if (this.ComponentList == null || this.ComponentList.Length < num2)
		{
			this.ComponentList = new int[num2];
		}
		for (int i = 0; i < _numVerts; i++)
		{
			this.VertexList[i].collapsed = true;
		}
		for (int j = 0; j < num; j++)
		{
			this.EdgeList[j].collapsed = true;
		}
		for (int k = 0; k < _numTriangles; k++)
		{
			this.TriList[k].collapsed = true;
		}
	}

	// Token: 0x0400444E RID: 17486
	[PublicizedFrom(EAccessModifier.Private)]
	public float DegenerateEpsilon = 0.9999f;

	// Token: 0x0400444F RID: 17487
	[PublicizedFrom(EAccessModifier.Private)]
	public int Iterations;

	// Token: 0x04004450 RID: 17488
	[PublicizedFrom(EAccessModifier.Private)]
	public int NumEdges;

	// Token: 0x04004451 RID: 17489
	[PublicizedFrom(EAccessModifier.Private)]
	public int NumTris;

	// Token: 0x04004452 RID: 17490
	[PublicizedFrom(EAccessModifier.Private)]
	public int NumVerts;

	// Token: 0x04004453 RID: 17491
	[PublicizedFrom(EAccessModifier.Private)]
	public int NumComponents;

	// Token: 0x04004454 RID: 17492
	[PublicizedFrom(EAccessModifier.Private)]
	public int NumOptimizedVerts;

	// Token: 0x04004455 RID: 17493
	[PublicizedFrom(EAccessModifier.Private)]
	public int NumOptimizedTris;

	// Token: 0x04004456 RID: 17494
	[PublicizedFrom(EAccessModifier.Private)]
	public MeshOptimizer.Edge[] EdgeList;

	// Token: 0x04004457 RID: 17495
	[PublicizedFrom(EAccessModifier.Private)]
	public MeshOptimizer.Tri[] TriList;

	// Token: 0x04004458 RID: 17496
	[PublicizedFrom(EAccessModifier.Private)]
	public MeshOptimizer.Vertex[] VertexList;

	// Token: 0x04004459 RID: 17497
	[PublicizedFrom(EAccessModifier.Private)]
	public int[] ComponentList;

	// Token: 0x0400445A RID: 17498
	[PublicizedFrom(EAccessModifier.Private)]
	public int NumSortedEdges;

	// Token: 0x0400445B RID: 17499
	[PublicizedFrom(EAccessModifier.Private)]
	public int[] SortedEdgeCost;

	// Token: 0x02000B65 RID: 2917
	[PublicizedFrom(EAccessModifier.Private)]
	public struct Edge
	{
		// Token: 0x0400445C RID: 17500
		public const int MaxEdgeTris = 8;

		// Token: 0x0400445D RID: 17501
		public int firstTri;

		// Token: 0x0400445E RID: 17502
		public int numTris;

		// Token: 0x0400445F RID: 17503
		public int v0;

		// Token: 0x04004460 RID: 17504
		public int v1;

		// Token: 0x04004461 RID: 17505
		public Vector3 vec;

		// Token: 0x04004462 RID: 17506
		public Vector3 normal;

		// Token: 0x04004463 RID: 17507
		public float len;

		// Token: 0x04004464 RID: 17508
		public bool collapsed;

		// Token: 0x04004465 RID: 17509
		public bool onModelEdge;
	}

	// Token: 0x02000B66 RID: 2918
	[PublicizedFrom(EAccessModifier.Private)]
	public struct Tri
	{
		// Token: 0x04004466 RID: 17510
		public int e0;

		// Token: 0x04004467 RID: 17511
		public int e1;

		// Token: 0x04004468 RID: 17512
		public int e2;

		// Token: 0x04004469 RID: 17513
		public int v0;

		// Token: 0x0400446A RID: 17514
		public int v1;

		// Token: 0x0400446B RID: 17515
		public int v2;

		// Token: 0x0400446C RID: 17516
		public Vector3 normal;

		// Token: 0x0400446D RID: 17517
		public bool collapsed;
	}

	// Token: 0x02000B67 RID: 2919
	[PublicizedFrom(EAccessModifier.Private)]
	public struct Vertex
	{
		// Token: 0x0400446E RID: 17518
		public const int MaxVertexTris = 256;

		// Token: 0x0400446F RID: 17519
		public Vector3 pos;

		// Token: 0x04004470 RID: 17520
		public int firstTri;

		// Token: 0x04004471 RID: 17521
		public int numTris;

		// Token: 0x04004472 RID: 17522
		public int firstEdge;

		// Token: 0x04004473 RID: 17523
		public int numEdges;

		// Token: 0x04004474 RID: 17524
		public bool collapsed;

		// Token: 0x04004475 RID: 17525
		public bool onModelEdge;

		// Token: 0x04004476 RID: 17526
		public float collapseCost;

		// Token: 0x04004477 RID: 17527
		public int collapseEdge;

		// Token: 0x04004478 RID: 17528
		public int emitIndex;

		// Token: 0x04004479 RID: 17529
		public int iterations;

		// Token: 0x0400447A RID: 17530
		public int sortedPosition;
	}
}
