using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace RaycastPathing
{
	// Token: 0x020018F1 RID: 6385
	[Preserve]
	public class RaycastPath
	{
		// Token: 0x170018C2 RID: 6338
		// (get) Token: 0x0600C50E RID: 50446 RVA: 0x00488602 File Offset: 0x00486802
		// (set) Token: 0x0600C50F RID: 50447 RVA: 0x0048860A File Offset: 0x0048680A
		public RaycastPathInfo Info
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x170018C3 RID: 6339
		// (get) Token: 0x0600C510 RID: 50448 RVA: 0x00488613 File Offset: 0x00486813
		public Vector3 Start
		{
			get
			{
				return this.Info.Start;
			}
		}

		// Token: 0x170018C4 RID: 6340
		// (get) Token: 0x0600C511 RID: 50449 RVA: 0x00488620 File Offset: 0x00486820
		public Vector3 Target
		{
			get
			{
				return this.Info.Target;
			}
		}

		// Token: 0x170018C5 RID: 6341
		// (get) Token: 0x0600C512 RID: 50450 RVA: 0x0048862D File Offset: 0x0048682D
		public Vector3i StartBlockPos
		{
			get
			{
				return this.Info.StartBlockPos;
			}
		}

		// Token: 0x170018C6 RID: 6342
		// (get) Token: 0x0600C513 RID: 50451 RVA: 0x0048863A File Offset: 0x0048683A
		public Vector3i TargetBlockPos
		{
			get
			{
				return this.Info.TargetBlockPos;
			}
		}

		// Token: 0x170018C7 RID: 6343
		// (get) Token: 0x0600C514 RID: 50452 RVA: 0x00488647 File Offset: 0x00486847
		public bool PathStartsIndoors
		{
			get
			{
				return this.Info.PathStartsIndoors;
			}
		}

		// Token: 0x170018C8 RID: 6344
		// (get) Token: 0x0600C515 RID: 50453 RVA: 0x00488654 File Offset: 0x00486854
		public bool PathEndsIndoors
		{
			get
			{
				return this.Info.PathEndsIndoors;
			}
		}

		// Token: 0x0600C516 RID: 50454 RVA: 0x00488661 File Offset: 0x00486861
		public RaycastPath(Vector3 start, Vector3 target)
		{
			this.Info = new RaycastPathInfo(start, target);
			RaycastPathManager.Instance.Add(this);
		}

		// Token: 0x0600C517 RID: 50455 RVA: 0x00488698 File Offset: 0x00486898
		[PublicizedFrom(EAccessModifier.Protected)]
		public ~RaycastPath()
		{
			this.Destruct();
		}

		// Token: 0x0600C518 RID: 50456 RVA: 0x004886C4 File Offset: 0x004868C4
		public void Destruct()
		{
			RaycastPathManager.Instance.Remove(this);
		}

		// Token: 0x0600C519 RID: 50457 RVA: 0x004886D1 File Offset: 0x004868D1
		public void AddNode(RaycastNode node)
		{
			if (!this.Nodes.Contains(node))
			{
				this.Nodes.Add(node);
			}
		}

		// Token: 0x0600C51A RID: 50458 RVA: 0x004886ED File Offset: 0x004868ED
		public void AddProjectedPoint(Vector3 point)
		{
			if (!this.ProjectedPoints.Contains(point))
			{
				this.ProjectedPoints.Add(point);
			}
		}

		// Token: 0x0600C51B RID: 50459 RVA: 0x0048870C File Offset: 0x0048690C
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

		// Token: 0x040094DD RID: 38109
		public List<RaycastNode> Nodes = new List<RaycastNode>();

		// Token: 0x040094DE RID: 38110
		public List<Vector3> ProjectedPoints = new List<Vector3>();
	}
}
