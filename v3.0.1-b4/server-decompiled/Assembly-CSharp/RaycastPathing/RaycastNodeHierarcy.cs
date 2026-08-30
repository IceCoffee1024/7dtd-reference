using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace RaycastPathing
{
	// Token: 0x020018A4 RID: 6308
	[Preserve]
	public class RaycastNodeHierarcy
	{
		// Token: 0x0600C25E RID: 49758 RVA: 0x0047E5EC File Offset: 0x0047C7EC
		public void SetWayPoint(RaycastNode node)
		{
			this.flowToWaypoint = true;
			this.waypoint = node;
		}

		// Token: 0x0600C25F RID: 49759 RVA: 0x0047E5FC File Offset: 0x0047C7FC
		public void SetParent(RaycastNode node)
		{
			this.parent = node;
		}

		// Token: 0x0600C260 RID: 49760 RVA: 0x0047E605 File Offset: 0x0047C805
		public void AddNeighbor(RaycastNode node)
		{
			if (!this.neighbors.Contains(node))
			{
				this.neighbors.Add(node);
			}
		}

		// Token: 0x0600C261 RID: 49761 RVA: 0x0047E624 File Offset: 0x0047C824
		public RaycastNode GetNeighbor(Vector3 pos)
		{
			for (int i = 0; i < this.neighbors.Count; i++)
			{
				if (this.neighbors[i].Center == pos)
				{
					return this.neighbors[i];
				}
			}
			return null;
		}

		// Token: 0x0600C262 RID: 49762 RVA: 0x0047E670 File Offset: 0x0047C870
		public void AddChild(RaycastNode node)
		{
			if (!this.children.Contains(node))
			{
				this.children.Add(node);
				if (node.nodeType == cPathNodeType.Air)
				{
					if (!this.childAirBlocks.Contains(node))
					{
						this.childAirBlocks.Add(node);
						return;
					}
				}
				else if (!this.childSolidBlocks.Contains(node))
				{
					this.childSolidBlocks.Add(node);
				}
			}
		}

		// Token: 0x04009307 RID: 37639
		public RaycastNode parent;

		// Token: 0x04009308 RID: 37640
		public List<RaycastNode> neighbors = new List<RaycastNode>();

		// Token: 0x04009309 RID: 37641
		public List<RaycastNode> children = new List<RaycastNode>();

		// Token: 0x0400930A RID: 37642
		public List<RaycastNode> childAirBlocks = new List<RaycastNode>();

		// Token: 0x0400930B RID: 37643
		public List<RaycastNode> childSolidBlocks = new List<RaycastNode>();

		// Token: 0x0400930C RID: 37644
		public bool flowToWaypoint;

		// Token: 0x0400930D RID: 37645
		public RaycastNode waypoint;
	}
}
