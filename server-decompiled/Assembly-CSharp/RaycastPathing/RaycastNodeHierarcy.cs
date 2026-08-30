using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace RaycastPathing
{
	// Token: 0x020018CA RID: 6346
	[Preserve]
	public class RaycastNodeHierarcy
	{
		// Token: 0x0600C3E5 RID: 50149 RVA: 0x00484718 File Offset: 0x00482918
		public void SetWayPoint(RaycastNode node)
		{
			this.flowToWaypoint = true;
			this.waypoint = node;
		}

		// Token: 0x0600C3E6 RID: 50150 RVA: 0x00484728 File Offset: 0x00482928
		public void SetParent(RaycastNode node)
		{
			this.parent = node;
		}

		// Token: 0x0600C3E7 RID: 50151 RVA: 0x00484731 File Offset: 0x00482931
		public void AddNeighbor(RaycastNode node)
		{
			if (!this.neighbors.Contains(node))
			{
				this.neighbors.Add(node);
			}
		}

		// Token: 0x0600C3E8 RID: 50152 RVA: 0x00484750 File Offset: 0x00482950
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

		// Token: 0x0600C3E9 RID: 50153 RVA: 0x0048479C File Offset: 0x0048299C
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

		// Token: 0x04009430 RID: 37936
		public RaycastNode parent;

		// Token: 0x04009431 RID: 37937
		public List<RaycastNode> neighbors = new List<RaycastNode>();

		// Token: 0x04009432 RID: 37938
		public List<RaycastNode> children = new List<RaycastNode>();

		// Token: 0x04009433 RID: 37939
		public List<RaycastNode> childAirBlocks = new List<RaycastNode>();

		// Token: 0x04009434 RID: 37940
		public List<RaycastNode> childSolidBlocks = new List<RaycastNode>();

		// Token: 0x04009435 RID: 37941
		public bool flowToWaypoint;

		// Token: 0x04009436 RID: 37942
		public RaycastNode waypoint;
	}
}
