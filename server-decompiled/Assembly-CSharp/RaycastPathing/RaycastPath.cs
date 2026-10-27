using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace RaycastPathing
{
	// Token: 0x02001922 RID: 6434
	[Preserve]
	public class RaycastPath
	{
		// Token: 0x1700190A RID: 6410
		// (get) Token: 0x0600C622 RID: 50722 RVA: 0x0048BFAE File Offset: 0x0048A1AE
		// (set) Token: 0x0600C623 RID: 50723 RVA: 0x0048BFB6 File Offset: 0x0048A1B6
		public RaycastPathInfo Info
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x1700190B RID: 6411
		// (get) Token: 0x0600C624 RID: 50724 RVA: 0x0048BFBF File Offset: 0x0048A1BF
		public Vector3 Start
		{
			get
			{
				return this.Info.Start;
			}
		}

		// Token: 0x1700190C RID: 6412
		// (get) Token: 0x0600C625 RID: 50725 RVA: 0x0048BFCC File Offset: 0x0048A1CC
		public Vector3 Target
		{
			get
			{
				return this.Info.Target;
			}
		}

		// Token: 0x1700190D RID: 6413
		// (get) Token: 0x0600C626 RID: 50726 RVA: 0x0048BFD9 File Offset: 0x0048A1D9
		public Vector3i StartBlockPos
		{
			get
			{
				return this.Info.StartBlockPos;
			}
		}

		// Token: 0x1700190E RID: 6414
		// (get) Token: 0x0600C627 RID: 50727 RVA: 0x0048BFE6 File Offset: 0x0048A1E6
		public Vector3i TargetBlockPos
		{
			get
			{
				return this.Info.TargetBlockPos;
			}
		}

		// Token: 0x1700190F RID: 6415
		// (get) Token: 0x0600C628 RID: 50728 RVA: 0x0048BFF3 File Offset: 0x0048A1F3
		public bool PathStartsIndoors
		{
			get
			{
				return this.Info.PathStartsIndoors;
			}
		}

		// Token: 0x17001910 RID: 6416
		// (get) Token: 0x0600C629 RID: 50729 RVA: 0x0048C000 File Offset: 0x0048A200
		public bool PathEndsIndoors
		{
			get
			{
				return this.Info.PathEndsIndoors;
			}
		}

		// Token: 0x0600C62A RID: 50730 RVA: 0x0048C00D File Offset: 0x0048A20D
		public RaycastPath(Vector3 start, Vector3 target)
		{
			this.Info = new RaycastPathInfo(start, target);
			RaycastPathManager.Instance.Add(this);
		}

		// Token: 0x0600C62B RID: 50731 RVA: 0x0048C044 File Offset: 0x0048A244
		[PublicizedFrom(EAccessModifier.Protected)]
		public ~RaycastPath()
		{
			this.Destruct();
		}

		// Token: 0x0600C62C RID: 50732 RVA: 0x0048C070 File Offset: 0x0048A270
		public void Destruct()
		{
			RaycastPathManager.Instance.Remove(this);
		}

		// Token: 0x0600C62D RID: 50733 RVA: 0x0048C07D File Offset: 0x0048A27D
		public void AddNode(RaycastNode node)
		{
			if (!this.Nodes.Contains(node))
			{
				this.Nodes.Add(node);
			}
		}

		// Token: 0x0600C62E RID: 50734 RVA: 0x0048C099 File Offset: 0x0048A299
		public void AddProjectedPoint(Vector3 point)
		{
			if (!this.ProjectedPoints.Contains(point))
			{
				this.ProjectedPoints.Add(point);
			}
		}

		// Token: 0x0600C62F RID: 50735 RVA: 0x0048C0B8 File Offset: 0x0048A2B8
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

		// Token: 0x040095C6 RID: 38342
		public List<RaycastNode> Nodes = new List<RaycastNode>();

		// Token: 0x040095C7 RID: 38343
		public List<Vector3> ProjectedPoints = new List<Vector3>();
	}
}
