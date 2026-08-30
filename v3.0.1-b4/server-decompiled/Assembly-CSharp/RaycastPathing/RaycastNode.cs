using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace RaycastPathing
{
	// Token: 0x020018A2 RID: 6306
	[Preserve]
	public class RaycastNode
	{
		// Token: 0x0600C245 RID: 49733 RVA: 0x0047E39F File Offset: 0x0047C59F
		public RaycastNode(Vector3 pos, float scale = 1f, int depth = 0)
		{
			this.info = new RaycastNodeInfo(pos, scale, depth);
			this.hierarchy = new RaycastNodeHierarcy();
		}

		// Token: 0x0600C246 RID: 49734 RVA: 0x0047E3C0 File Offset: 0x0047C5C0
		public RaycastNode(Vector3 min, Vector3 max, float scale = 1f, int depth = 0)
		{
			this.info = new RaycastNodeInfo(min, max, scale, depth);
			this.hierarchy = new RaycastNodeHierarcy();
		}

		// Token: 0x170017B9 RID: 6073
		// (get) Token: 0x0600C247 RID: 49735 RVA: 0x0047E3E3 File Offset: 0x0047C5E3
		public Vector3 Position
		{
			get
			{
				return this.info.Position;
			}
		}

		// Token: 0x170017BA RID: 6074
		// (get) Token: 0x0600C248 RID: 49736 RVA: 0x0047E3F0 File Offset: 0x0047C5F0
		public Vector3 Center
		{
			get
			{
				return this.info.Center;
			}
		}

		// Token: 0x170017BB RID: 6075
		// (get) Token: 0x0600C249 RID: 49737 RVA: 0x0047E3FD File Offset: 0x0047C5FD
		public Vector3i BlockPos
		{
			get
			{
				return this.info.BlockPos;
			}
		}

		// Token: 0x170017BC RID: 6076
		// (get) Token: 0x0600C24A RID: 49738 RVA: 0x0047E40A File Offset: 0x0047C60A
		public float Scale
		{
			get
			{
				return this.info.Scale;
			}
		}

		// Token: 0x170017BD RID: 6077
		// (get) Token: 0x0600C24B RID: 49739 RVA: 0x0047E417 File Offset: 0x0047C617
		public int Depth
		{
			get
			{
				return this.info.Depth;
			}
		}

		// Token: 0x170017BE RID: 6078
		// (get) Token: 0x0600C24C RID: 49740 RVA: 0x0047E424 File Offset: 0x0047C624
		public Vector3 Min
		{
			get
			{
				return this.info.Min;
			}
		}

		// Token: 0x170017BF RID: 6079
		// (get) Token: 0x0600C24D RID: 49741 RVA: 0x0047E431 File Offset: 0x0047C631
		public Vector3 Max
		{
			get
			{
				return this.info.Max;
			}
		}

		// Token: 0x170017C0 RID: 6080
		// (get) Token: 0x0600C24E RID: 49742 RVA: 0x0047E43E File Offset: 0x0047C63E
		public RaycastNode Parent
		{
			get
			{
				return this.hierarchy.parent;
			}
		}

		// Token: 0x170017C1 RID: 6081
		// (get) Token: 0x0600C24F RID: 49743 RVA: 0x0047E44B File Offset: 0x0047C64B
		public List<RaycastNode> Neighbors
		{
			get
			{
				return this.hierarchy.neighbors;
			}
		}

		// Token: 0x0600C250 RID: 49744 RVA: 0x0047E458 File Offset: 0x0047C658
		public void SetParent(RaycastNode node)
		{
			this.hierarchy.parent = node;
		}

		// Token: 0x0600C251 RID: 49745 RVA: 0x0047E466 File Offset: 0x0047C666
		public void AddNeighbor(RaycastNode node)
		{
			this.hierarchy.neighbors.Add(node);
		}

		// Token: 0x0600C252 RID: 49746 RVA: 0x0047E479 File Offset: 0x0047C679
		public RaycastNode GetNeighbor(Vector3 pos)
		{
			return this.hierarchy.GetNeighbor(pos);
		}

		// Token: 0x0600C253 RID: 49747 RVA: 0x0047E487 File Offset: 0x0047C687
		public void AddChild(RaycastNode node)
		{
			this.hierarchy.AddChild(node);
		}

		// Token: 0x170017C2 RID: 6082
		// (get) Token: 0x0600C254 RID: 49748 RVA: 0x0047E495 File Offset: 0x0047C695
		public List<RaycastNode> Children
		{
			get
			{
				return this.hierarchy.children;
			}
		}

		// Token: 0x170017C3 RID: 6083
		// (get) Token: 0x0600C255 RID: 49749 RVA: 0x0047E4A2 File Offset: 0x0047C6A2
		public List<RaycastNode> ChildAirBlocks
		{
			get
			{
				return this.hierarchy.childAirBlocks;
			}
		}

		// Token: 0x170017C4 RID: 6084
		// (get) Token: 0x0600C256 RID: 49750 RVA: 0x0047E4AF File Offset: 0x0047C6AF
		public List<RaycastNode> ChildSolidBlocks
		{
			get
			{
				return this.hierarchy.childSolidBlocks;
			}
		}

		// Token: 0x170017C5 RID: 6085
		// (get) Token: 0x0600C257 RID: 49751 RVA: 0x0047E4BC File Offset: 0x0047C6BC
		public RaycastNode Waypoint
		{
			get
			{
				return this.hierarchy.waypoint;
			}
		}

		// Token: 0x0600C258 RID: 49752 RVA: 0x0047E4C9 File Offset: 0x0047C6C9
		public void SetWaypoint(RaycastNode node)
		{
			this.hierarchy.SetWayPoint(node);
		}

		// Token: 0x170017C6 RID: 6086
		// (get) Token: 0x0600C259 RID: 49753 RVA: 0x0047E4D7 File Offset: 0x0047C6D7
		public bool FlowToWaypoint
		{
			get
			{
				return this.hierarchy.flowToWaypoint;
			}
		}

		// Token: 0x0600C25A RID: 49754 RVA: 0x0047E4E4 File Offset: 0x0047C6E4
		public void SetType(cPathNodeType _nodeType)
		{
			this.nodeType = _nodeType;
		}

		// Token: 0x0600C25B RID: 49755 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void DebugDraw()
		{
		}

		// Token: 0x040092FD RID: 37629
		[PublicizedFrom(EAccessModifier.Private)]
		public RaycastNodeInfo info;

		// Token: 0x040092FE RID: 37630
		[PublicizedFrom(EAccessModifier.Private)]
		public RaycastNodeHierarcy hierarchy;

		// Token: 0x040092FF RID: 37631
		public cPathNodeType nodeType;
	}
}
