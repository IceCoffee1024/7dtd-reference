using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace RaycastPathing
{
	// Token: 0x020018F4 RID: 6388
	[Preserve]
	public class RaycastNode
	{
		// Token: 0x0600C524 RID: 50468 RVA: 0x00488A13 File Offset: 0x00486C13
		public RaycastNode(Vector3 pos, float scale = 1f, int depth = 0)
		{
			this.info = new RaycastNodeInfo(pos, scale, depth);
			this.hierarchy = new RaycastNodeHierarcy();
		}

		// Token: 0x0600C525 RID: 50469 RVA: 0x00488A34 File Offset: 0x00486C34
		public RaycastNode(Vector3 min, Vector3 max, float scale = 1f, int depth = 0)
		{
			this.info = new RaycastNodeInfo(min, max, scale, depth);
			this.hierarchy = new RaycastNodeHierarcy();
		}

		// Token: 0x170018CD RID: 6349
		// (get) Token: 0x0600C526 RID: 50470 RVA: 0x00488A57 File Offset: 0x00486C57
		public Vector3 Position
		{
			get
			{
				return this.info.Position;
			}
		}

		// Token: 0x170018CE RID: 6350
		// (get) Token: 0x0600C527 RID: 50471 RVA: 0x00488A64 File Offset: 0x00486C64
		public Vector3 Center
		{
			get
			{
				return this.info.Center;
			}
		}

		// Token: 0x170018CF RID: 6351
		// (get) Token: 0x0600C528 RID: 50472 RVA: 0x00488A71 File Offset: 0x00486C71
		public Vector3i BlockPos
		{
			get
			{
				return this.info.BlockPos;
			}
		}

		// Token: 0x170018D0 RID: 6352
		// (get) Token: 0x0600C529 RID: 50473 RVA: 0x00488A7E File Offset: 0x00486C7E
		public float Scale
		{
			get
			{
				return this.info.Scale;
			}
		}

		// Token: 0x170018D1 RID: 6353
		// (get) Token: 0x0600C52A RID: 50474 RVA: 0x00488A8B File Offset: 0x00486C8B
		public int Depth
		{
			get
			{
				return this.info.Depth;
			}
		}

		// Token: 0x170018D2 RID: 6354
		// (get) Token: 0x0600C52B RID: 50475 RVA: 0x00488A98 File Offset: 0x00486C98
		public Vector3 Min
		{
			get
			{
				return this.info.Min;
			}
		}

		// Token: 0x170018D3 RID: 6355
		// (get) Token: 0x0600C52C RID: 50476 RVA: 0x00488AA5 File Offset: 0x00486CA5
		public Vector3 Max
		{
			get
			{
				return this.info.Max;
			}
		}

		// Token: 0x170018D4 RID: 6356
		// (get) Token: 0x0600C52D RID: 50477 RVA: 0x00488AB2 File Offset: 0x00486CB2
		public RaycastNode Parent
		{
			get
			{
				return this.hierarchy.parent;
			}
		}

		// Token: 0x170018D5 RID: 6357
		// (get) Token: 0x0600C52E RID: 50478 RVA: 0x00488ABF File Offset: 0x00486CBF
		public List<RaycastNode> Neighbors
		{
			get
			{
				return this.hierarchy.neighbors;
			}
		}

		// Token: 0x0600C52F RID: 50479 RVA: 0x00488ACC File Offset: 0x00486CCC
		public void SetParent(RaycastNode node)
		{
			this.hierarchy.parent = node;
		}

		// Token: 0x0600C530 RID: 50480 RVA: 0x00488ADA File Offset: 0x00486CDA
		public void AddNeighbor(RaycastNode node)
		{
			this.hierarchy.neighbors.Add(node);
		}

		// Token: 0x0600C531 RID: 50481 RVA: 0x00488AED File Offset: 0x00486CED
		public RaycastNode GetNeighbor(Vector3 pos)
		{
			return this.hierarchy.GetNeighbor(pos);
		}

		// Token: 0x0600C532 RID: 50482 RVA: 0x00488AFB File Offset: 0x00486CFB
		public void AddChild(RaycastNode node)
		{
			this.hierarchy.AddChild(node);
		}

		// Token: 0x170018D6 RID: 6358
		// (get) Token: 0x0600C533 RID: 50483 RVA: 0x00488B09 File Offset: 0x00486D09
		public List<RaycastNode> Children
		{
			get
			{
				return this.hierarchy.children;
			}
		}

		// Token: 0x170018D7 RID: 6359
		// (get) Token: 0x0600C534 RID: 50484 RVA: 0x00488B16 File Offset: 0x00486D16
		public List<RaycastNode> ChildAirBlocks
		{
			get
			{
				return this.hierarchy.childAirBlocks;
			}
		}

		// Token: 0x170018D8 RID: 6360
		// (get) Token: 0x0600C535 RID: 50485 RVA: 0x00488B23 File Offset: 0x00486D23
		public List<RaycastNode> ChildSolidBlocks
		{
			get
			{
				return this.hierarchy.childSolidBlocks;
			}
		}

		// Token: 0x170018D9 RID: 6361
		// (get) Token: 0x0600C536 RID: 50486 RVA: 0x00488B30 File Offset: 0x00486D30
		public RaycastNode Waypoint
		{
			get
			{
				return this.hierarchy.waypoint;
			}
		}

		// Token: 0x0600C537 RID: 50487 RVA: 0x00488B3D File Offset: 0x00486D3D
		public void SetWaypoint(RaycastNode node)
		{
			this.hierarchy.SetWayPoint(node);
		}

		// Token: 0x170018DA RID: 6362
		// (get) Token: 0x0600C538 RID: 50488 RVA: 0x00488B4B File Offset: 0x00486D4B
		public bool FlowToWaypoint
		{
			get
			{
				return this.hierarchy.flowToWaypoint;
			}
		}

		// Token: 0x0600C539 RID: 50489 RVA: 0x00488B58 File Offset: 0x00486D58
		public void SetType(cPathNodeType _nodeType)
		{
			this.nodeType = _nodeType;
		}

		// Token: 0x0600C53A RID: 50490 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void DebugDraw()
		{
		}

		// Token: 0x040094EC RID: 38124
		[PublicizedFrom(EAccessModifier.Private)]
		public RaycastNodeInfo info;

		// Token: 0x040094ED RID: 38125
		[PublicizedFrom(EAccessModifier.Private)]
		public RaycastNodeHierarcy hierarchy;

		// Token: 0x040094EE RID: 38126
		public cPathNodeType nodeType;
	}
}
