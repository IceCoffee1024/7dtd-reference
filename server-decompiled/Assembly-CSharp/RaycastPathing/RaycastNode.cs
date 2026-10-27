using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace RaycastPathing
{
	// Token: 0x02001925 RID: 6437
	[Preserve]
	public class RaycastNode
	{
		// Token: 0x0600C638 RID: 50744 RVA: 0x0048C3BF File Offset: 0x0048A5BF
		public RaycastNode(Vector3 pos, float scale = 1f, int depth = 0)
		{
			this.info = new RaycastNodeInfo(pos, scale, depth);
			this.hierarchy = new RaycastNodeHierarcy();
		}

		// Token: 0x0600C639 RID: 50745 RVA: 0x0048C3E0 File Offset: 0x0048A5E0
		public RaycastNode(Vector3 min, Vector3 max, float scale = 1f, int depth = 0)
		{
			this.info = new RaycastNodeInfo(min, max, scale, depth);
			this.hierarchy = new RaycastNodeHierarcy();
		}

		// Token: 0x17001915 RID: 6421
		// (get) Token: 0x0600C63A RID: 50746 RVA: 0x0048C403 File Offset: 0x0048A603
		public Vector3 Position
		{
			get
			{
				return this.info.Position;
			}
		}

		// Token: 0x17001916 RID: 6422
		// (get) Token: 0x0600C63B RID: 50747 RVA: 0x0048C410 File Offset: 0x0048A610
		public Vector3 Center
		{
			get
			{
				return this.info.Center;
			}
		}

		// Token: 0x17001917 RID: 6423
		// (get) Token: 0x0600C63C RID: 50748 RVA: 0x0048C41D File Offset: 0x0048A61D
		public Vector3i BlockPos
		{
			get
			{
				return this.info.BlockPos;
			}
		}

		// Token: 0x17001918 RID: 6424
		// (get) Token: 0x0600C63D RID: 50749 RVA: 0x0048C42A File Offset: 0x0048A62A
		public float Scale
		{
			get
			{
				return this.info.Scale;
			}
		}

		// Token: 0x17001919 RID: 6425
		// (get) Token: 0x0600C63E RID: 50750 RVA: 0x0048C437 File Offset: 0x0048A637
		public int Depth
		{
			get
			{
				return this.info.Depth;
			}
		}

		// Token: 0x1700191A RID: 6426
		// (get) Token: 0x0600C63F RID: 50751 RVA: 0x0048C444 File Offset: 0x0048A644
		public Vector3 Min
		{
			get
			{
				return this.info.Min;
			}
		}

		// Token: 0x1700191B RID: 6427
		// (get) Token: 0x0600C640 RID: 50752 RVA: 0x0048C451 File Offset: 0x0048A651
		public Vector3 Max
		{
			get
			{
				return this.info.Max;
			}
		}

		// Token: 0x1700191C RID: 6428
		// (get) Token: 0x0600C641 RID: 50753 RVA: 0x0048C45E File Offset: 0x0048A65E
		public RaycastNode Parent
		{
			get
			{
				return this.hierarchy.parent;
			}
		}

		// Token: 0x1700191D RID: 6429
		// (get) Token: 0x0600C642 RID: 50754 RVA: 0x0048C46B File Offset: 0x0048A66B
		public List<RaycastNode> Neighbors
		{
			get
			{
				return this.hierarchy.neighbors;
			}
		}

		// Token: 0x0600C643 RID: 50755 RVA: 0x0048C478 File Offset: 0x0048A678
		public void SetParent(RaycastNode node)
		{
			this.hierarchy.parent = node;
		}

		// Token: 0x0600C644 RID: 50756 RVA: 0x0048C486 File Offset: 0x0048A686
		public void AddNeighbor(RaycastNode node)
		{
			this.hierarchy.neighbors.Add(node);
		}

		// Token: 0x0600C645 RID: 50757 RVA: 0x0048C499 File Offset: 0x0048A699
		public RaycastNode GetNeighbor(Vector3 pos)
		{
			return this.hierarchy.GetNeighbor(pos);
		}

		// Token: 0x0600C646 RID: 50758 RVA: 0x0048C4A7 File Offset: 0x0048A6A7
		public void AddChild(RaycastNode node)
		{
			this.hierarchy.AddChild(node);
		}

		// Token: 0x1700191E RID: 6430
		// (get) Token: 0x0600C647 RID: 50759 RVA: 0x0048C4B5 File Offset: 0x0048A6B5
		public List<RaycastNode> Children
		{
			get
			{
				return this.hierarchy.children;
			}
		}

		// Token: 0x1700191F RID: 6431
		// (get) Token: 0x0600C648 RID: 50760 RVA: 0x0048C4C2 File Offset: 0x0048A6C2
		public List<RaycastNode> ChildAirBlocks
		{
			get
			{
				return this.hierarchy.childAirBlocks;
			}
		}

		// Token: 0x17001920 RID: 6432
		// (get) Token: 0x0600C649 RID: 50761 RVA: 0x0048C4CF File Offset: 0x0048A6CF
		public List<RaycastNode> ChildSolidBlocks
		{
			get
			{
				return this.hierarchy.childSolidBlocks;
			}
		}

		// Token: 0x17001921 RID: 6433
		// (get) Token: 0x0600C64A RID: 50762 RVA: 0x0048C4DC File Offset: 0x0048A6DC
		public RaycastNode Waypoint
		{
			get
			{
				return this.hierarchy.waypoint;
			}
		}

		// Token: 0x0600C64B RID: 50763 RVA: 0x0048C4E9 File Offset: 0x0048A6E9
		public void SetWaypoint(RaycastNode node)
		{
			this.hierarchy.SetWayPoint(node);
		}

		// Token: 0x17001922 RID: 6434
		// (get) Token: 0x0600C64C RID: 50764 RVA: 0x0048C4F7 File Offset: 0x0048A6F7
		public bool FlowToWaypoint
		{
			get
			{
				return this.hierarchy.flowToWaypoint;
			}
		}

		// Token: 0x0600C64D RID: 50765 RVA: 0x0048C504 File Offset: 0x0048A704
		public void SetType(cPathNodeType _nodeType)
		{
			this.nodeType = _nodeType;
		}

		// Token: 0x0600C64E RID: 50766 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void DebugDraw()
		{
		}

		// Token: 0x040095D5 RID: 38357
		[PublicizedFrom(EAccessModifier.Private)]
		public RaycastNodeInfo info;

		// Token: 0x040095D6 RID: 38358
		[PublicizedFrom(EAccessModifier.Private)]
		public RaycastNodeHierarcy hierarchy;

		// Token: 0x040095D7 RID: 38359
		public cPathNodeType nodeType;
	}
}
