using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace RaycastPathing
{
	// Token: 0x0200189F RID: 6303
	[Preserve]
	public class RaycastPath
	{
		// Token: 0x170017AE RID: 6062
		// (get) Token: 0x0600C22F RID: 49711 RVA: 0x0047DF8E File Offset: 0x0047C18E
		// (set) Token: 0x0600C230 RID: 49712 RVA: 0x0047DF96 File Offset: 0x0047C196
		public RaycastPathInfo Info
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x170017AF RID: 6063
		// (get) Token: 0x0600C231 RID: 49713 RVA: 0x0047DF9F File Offset: 0x0047C19F
		public Vector3 Start
		{
			get
			{
				return this.Info.Start;
			}
		}

		// Token: 0x170017B0 RID: 6064
		// (get) Token: 0x0600C232 RID: 49714 RVA: 0x0047DFAC File Offset: 0x0047C1AC
		public Vector3 Target
		{
			get
			{
				return this.Info.Target;
			}
		}

		// Token: 0x170017B1 RID: 6065
		// (get) Token: 0x0600C233 RID: 49715 RVA: 0x0047DFB9 File Offset: 0x0047C1B9
		public Vector3i StartBlockPos
		{
			get
			{
				return this.Info.StartBlockPos;
			}
		}

		// Token: 0x170017B2 RID: 6066
		// (get) Token: 0x0600C234 RID: 49716 RVA: 0x0047DFC6 File Offset: 0x0047C1C6
		public Vector3i TargetBlockPos
		{
			get
			{
				return this.Info.TargetBlockPos;
			}
		}

		// Token: 0x170017B3 RID: 6067
		// (get) Token: 0x0600C235 RID: 49717 RVA: 0x0047DFD3 File Offset: 0x0047C1D3
		public bool PathStartsIndoors
		{
			get
			{
				return this.Info.PathStartsIndoors;
			}
		}

		// Token: 0x170017B4 RID: 6068
		// (get) Token: 0x0600C236 RID: 49718 RVA: 0x0047DFE0 File Offset: 0x0047C1E0
		public bool PathEndsIndoors
		{
			get
			{
				return this.Info.PathEndsIndoors;
			}
		}

		// Token: 0x0600C237 RID: 49719 RVA: 0x0047DFED File Offset: 0x0047C1ED
		public RaycastPath(Vector3 start, Vector3 target)
		{
			this.Info = new RaycastPathInfo(start, target);
			RaycastPathManager.Instance.Add(this);
		}

		// Token: 0x0600C238 RID: 49720 RVA: 0x0047E024 File Offset: 0x0047C224
		[PublicizedFrom(EAccessModifier.Protected)]
		public ~RaycastPath()
		{
			this.Destruct();
		}

		// Token: 0x0600C239 RID: 49721 RVA: 0x0047E050 File Offset: 0x0047C250
		public void Destruct()
		{
			RaycastPathManager.Instance.Remove(this);
		}

		// Token: 0x0600C23A RID: 49722 RVA: 0x0047E05D File Offset: 0x0047C25D
		public void AddNode(RaycastNode node)
		{
			if (!this.Nodes.Contains(node))
			{
				this.Nodes.Add(node);
			}
		}

		// Token: 0x0600C23B RID: 49723 RVA: 0x0047E079 File Offset: 0x0047C279
		public void AddProjectedPoint(Vector3 point)
		{
			if (!this.ProjectedPoints.Contains(point))
			{
				this.ProjectedPoints.Add(point);
			}
		}

		// Token: 0x0600C23C RID: 49724 RVA: 0x0047E098 File Offset: 0x0047C298
		public virtual void DebugDraw()
		{
			for (int i = 0; i < this.ProjectedPoints.Count - 1; i++)
			{
				Utils.DrawLine(World.blockToTransformPos(new Vector3i(this.ProjectedPoints[i] - Origin.position)), World.blockToTransformPos(new Vector3i(this.ProjectedPoints[i + 1] - Origin.position)), Color.white, Color.cyan, 2, 0f);
			}
			for (int j = 0; j < this.Nodes.Count; j++)
			{
				RaycastNode raycastNode = this.Nodes[j];
				for (int k = 0; k < raycastNode.Neighbors.Count; k++)
				{
					RaycastNode raycastNode2 = raycastNode.Neighbors[k];
					for (int l = 0; l < raycastNode2.ChildSolidBlocks.Count; l++)
					{
						RaycastPathUtils.DrawNode(raycastNode2.ChildSolidBlocks[l], Color.red, 0f);
					}
					for (int m = 0; m < raycastNode2.ChildAirBlocks.Count; m++)
					{
						RaycastPathUtils.DrawNode(raycastNode2.ChildAirBlocks[m], Color.cyan, 0f);
					}
				}
				if (raycastNode.Children.Count < 1)
				{
					cPathNodeType nodeType = raycastNode.nodeType;
					if (nodeType != cPathNodeType.Air)
					{
						if (nodeType == cPathNodeType.Door)
						{
							RaycastPathUtils.DrawNode(raycastNode, Color.green, 0f);
						}
					}
					else
					{
						RaycastPathUtils.DrawNode(raycastNode, Color.cyan, 0f);
					}
				}
				else
				{
					for (int n = 0; n < raycastNode.ChildSolidBlocks.Count; n++)
					{
						RaycastPathUtils.DrawNode(raycastNode.ChildSolidBlocks[n], Color.red, 0f);
					}
					for (int num = 0; num < raycastNode.ChildAirBlocks.Count; num++)
					{
						RaycastPathUtils.DrawNode(raycastNode.ChildAirBlocks[num], Color.cyan, 0f);
					}
				}
			}
			for (int num2 = 0; num2 < this.Nodes.Count - 1; num2++)
			{
				RaycastNode raycastNode3 = this.Nodes[num2];
				RaycastNode raycastNode4 = this.Nodes[num2 + 1];
				Utils.DrawLine(raycastNode3.Position - Origin.position, raycastNode4.Position - Origin.position, Color.white, Color.green, 2, 0f);
			}
		}

		// Token: 0x040092EE RID: 37614
		public List<RaycastNode> Nodes = new List<RaycastNode>();

		// Token: 0x040092EF RID: 37615
		public List<Vector3> ProjectedPoints = new List<Vector3>();
	}
}
