using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace RaycastPathing
{
	// Token: 0x020018F6 RID: 6390
	[Preserve]
	public class RaycastNodeHierarcy
	{
		// Token: 0x0600C53D RID: 50493 RVA: 0x00488C60 File Offset: 0x00486E60
		public void SetWayPoint(RaycastNode node)
		{
			this.flowToWaypoint = true;
			this.waypoint = node;
		}

		// Token: 0x0600C53E RID: 50494 RVA: 0x00488C70 File Offset: 0x00486E70
		public void SetParent(RaycastNode node)
		{
			this.parent = node;
		}

		// Token: 0x0600C53F RID: 50495 RVA: 0x00488C79 File Offset: 0x00486E79
		public void AddNeighbor(RaycastNode node)
		{
			if (!this.neighbors.Contains(node))
			{
				this.neighbors.Add(node);
			}
		}

		// Token: 0x0600C540 RID: 50496 RVA: 0x00488C98 File Offset: 0x00486E98
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

		// Token: 0x0600C541 RID: 50497 RVA: 0x00488CE4 File Offset: 0x00486EE4
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

		// Token: 0x040094F6 RID: 38134
		public RaycastNode parent;

		// Token: 0x040094F7 RID: 38135
		public List<RaycastNode> neighbors = new List<RaycastNode>();

		// Token: 0x040094F8 RID: 38136
		public List<RaycastNode> children = new List<RaycastNode>();

		// Token: 0x040094F9 RID: 38137
		public List<RaycastNode> childAirBlocks = new List<RaycastNode>();

		// Token: 0x040094FA RID: 38138
		public List<RaycastNode> childSolidBlocks = new List<RaycastNode>();

		// Token: 0x040094FB RID: 38139
		public bool flowToWaypoint;

		// Token: 0x040094FC RID: 38140
		public RaycastNode waypoint;
	}
}
