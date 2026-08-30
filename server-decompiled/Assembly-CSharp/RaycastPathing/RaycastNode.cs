using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace RaycastPathing
{
	// Token: 0x020018C8 RID: 6344
	[Preserve]
	public class RaycastNode
	{
		// Token: 0x0600C3CC RID: 50124 RVA: 0x004844CB File Offset: 0x004826CB
		public RaycastNode(Vector3 pos, float scale = 1f, int depth = 0)
		{
			this.info = new RaycastNodeInfo(pos, scale, depth);
			this.hierarchy = new RaycastNodeHierarcy();
		}

		// Token: 0x0600C3CD RID: 50125 RVA: 0x004844EC File Offset: 0x004826EC
		public RaycastNode(Vector3 min, Vector3 max, float scale = 1f, int depth = 0)
		{
			this.info = new RaycastNodeInfo(min, max, scale, depth);
			this.hierarchy = new RaycastNodeHierarcy();
		}

		// Token: 0x1700182B RID: 6187
		// (get) Token: 0x0600C3CE RID: 50126 RVA: 0x0048450F File Offset: 0x0048270F
		public Vector3 Position
		{
			get
			{
				return this.info.Position;
			}
		}

		// Token: 0x1700182C RID: 6188
		// (get) Token: 0x0600C3CF RID: 50127 RVA: 0x0048451C File Offset: 0x0048271C
		public Vector3 Center
		{
			get
			{
				return this.info.Center;
			}
		}

		// Token: 0x1700182D RID: 6189
		// (get) Token: 0x0600C3D0 RID: 50128 RVA: 0x00484529 File Offset: 0x00482729
		public Vector3i BlockPos
		{
			get
			{
				return this.info.BlockPos;
			}
		}

		// Token: 0x1700182E RID: 6190
		// (get) Token: 0x0600C3D1 RID: 50129 RVA: 0x00484536 File Offset: 0x00482736
		public float Scale
		{
			get
			{
				return this.info.Scale;
			}
		}

		// Token: 0x1700182F RID: 6191
		// (get) Token: 0x0600C3D2 RID: 50130 RVA: 0x00484543 File Offset: 0x00482743
		public int Depth
		{
			get
			{
				return this.info.Depth;
			}
		}

		// Token: 0x17001830 RID: 6192
		// (get) Token: 0x0600C3D3 RID: 50131 RVA: 0x00484550 File Offset: 0x00482750
		public Vector3 Min
		{
			get
			{
				return this.info.Min;
			}
		}

		// Token: 0x17001831 RID: 6193
		// (get) Token: 0x0600C3D4 RID: 50132 RVA: 0x0048455D File Offset: 0x0048275D
		public Vector3 Max
		{
			get
			{
				return this.info.Max;
			}
		}

		// Token: 0x17001832 RID: 6194
		// (get) Token: 0x0600C3D5 RID: 50133 RVA: 0x0048456A File Offset: 0x0048276A
		public RaycastNode Parent
		{
			get
			{
				return this.hierarchy.parent;
			}
		}

		// Token: 0x17001833 RID: 6195
		// (get) Token: 0x0600C3D6 RID: 50134 RVA: 0x00484577 File Offset: 0x00482777
		public List<RaycastNode> Neighbors
		{
			get
			{
				return this.hierarchy.neighbors;
			}
		}

		// Token: 0x0600C3D7 RID: 50135 RVA: 0x00484584 File Offset: 0x00482784
		public void SetParent(RaycastNode node)
		{
			this.hierarchy.parent = node;
		}

		// Token: 0x0600C3D8 RID: 50136 RVA: 0x00484592 File Offset: 0x00482792
		public void AddNeighbor(RaycastNode node)
		{
			this.hierarchy.neighbors.Add(node);
		}

		// Token: 0x0600C3D9 RID: 50137 RVA: 0x004845A5 File Offset: 0x004827A5
		public RaycastNode GetNeighbor(Vector3 pos)
		{
			return this.hierarchy.GetNeighbor(pos);
		}

		// Token: 0x0600C3DA RID: 50138 RVA: 0x004845B3 File Offset: 0x004827B3
		public void AddChild(RaycastNode node)
		{
			this.hierarchy.AddChild(node);
		}

		// Token: 0x17001834 RID: 6196
		// (get) Token: 0x0600C3DB RID: 50139 RVA: 0x004845C1 File Offset: 0x004827C1
		public List<RaycastNode> Children
		{
			get
			{
				return this.hierarchy.children;
			}
		}

		// Token: 0x17001835 RID: 6197
		// (get) Token: 0x0600C3DC RID: 50140 RVA: 0x004845CE File Offset: 0x004827CE
		public List<RaycastNode> ChildAirBlocks
		{
			get
			{
				return this.hierarchy.childAirBlocks;
			}
		}

		// Token: 0x17001836 RID: 6198
		// (get) Token: 0x0600C3DD RID: 50141 RVA: 0x004845DB File Offset: 0x004827DB
		public List<RaycastNode> ChildSolidBlocks
		{
			get
			{
				return this.hierarchy.childSolidBlocks;
			}
		}

		// Token: 0x17001837 RID: 6199
		// (get) Token: 0x0600C3DE RID: 50142 RVA: 0x004845E8 File Offset: 0x004827E8
		public RaycastNode Waypoint
		{
			get
			{
				return this.hierarchy.waypoint;
			}
		}

		// Token: 0x0600C3DF RID: 50143 RVA: 0x004845F5 File Offset: 0x004827F5
		public void SetWaypoint(RaycastNode node)
		{
			this.hierarchy.SetWayPoint(node);
		}

		// Token: 0x17001838 RID: 6200
		// (get) Token: 0x0600C3E0 RID: 50144 RVA: 0x00484603 File Offset: 0x00482803
		public bool FlowToWaypoint
		{
			get
			{
				return this.hierarchy.flowToWaypoint;
			}
		}

		// Token: 0x0600C3E1 RID: 50145 RVA: 0x00484610 File Offset: 0x00482810
		public void SetType(cPathNodeType _nodeType)
		{
			this.nodeType = _nodeType;
		}

		// Token: 0x0600C3E2 RID: 50146 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void DebugDraw()
		{
		}

		// Token: 0x04009426 RID: 37926
		[PublicizedFrom(EAccessModifier.Private)]
		public RaycastNodeInfo info;

		// Token: 0x04009427 RID: 37927
		[PublicizedFrom(EAccessModifier.Private)]
		public RaycastNodeHierarcy hierarchy;

		// Token: 0x04009428 RID: 37928
		public cPathNodeType nodeType;
	}
}
