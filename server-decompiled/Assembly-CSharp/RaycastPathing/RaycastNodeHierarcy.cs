using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace RaycastPathing
{
	// Token: 0x02001927 RID: 6439
	[Preserve]
	public class RaycastNodeHierarcy
	{
		// Token: 0x0600C651 RID: 50769 RVA: 0x0048C60C File Offset: 0x0048A80C
		public void SetWayPoint(RaycastNode node)
		{
			this.flowToWaypoint = true;
			this.waypoint = node;
		}

		// Token: 0x0600C652 RID: 50770 RVA: 0x0048C61C File Offset: 0x0048A81C
		public void SetParent(RaycastNode node)
		{
			this.parent = node;
		}

		// Token: 0x0600C653 RID: 50771 RVA: 0x0048C625 File Offset: 0x0048A825
		public void AddNeighbor(RaycastNode node)
		{
			if (!this.neighbors.Contains(node))
			{
				this.neighbors.Add(node);
			}
		}

		// Token: 0x0600C654 RID: 50772 RVA: 0x0048C644 File Offset: 0x0048A844
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

		// Token: 0x0600C655 RID: 50773 RVA: 0x0048C690 File Offset: 0x0048A890
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

		// Token: 0x040095DF RID: 38367
		public RaycastNode parent;

		// Token: 0x040095E0 RID: 38368
		public List<RaycastNode> neighbors = new List<RaycastNode>();

		// Token: 0x040095E1 RID: 38369
		public List<RaycastNode> children = new List<RaycastNode>();

		// Token: 0x040095E2 RID: 38370
		public List<RaycastNode> childAirBlocks = new List<RaycastNode>();

		// Token: 0x040095E3 RID: 38371
		public List<RaycastNode> childSolidBlocks = new List<RaycastNode>();

		// Token: 0x040095E4 RID: 38372
		public bool flowToWaypoint;

		// Token: 0x040095E5 RID: 38373
		public RaycastNode waypoint;
	}
}
