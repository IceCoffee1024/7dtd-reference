using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace RaycastPathing
{
	// Token: 0x02001928 RID: 6440
	[Preserve]
	public class FloodFillPath : RaycastPath
	{
		// Token: 0x0600C657 RID: 50775 RVA: 0x0048C729 File Offset: 0x0048A929
		public FloodFillPath(Vector3 start, Vector3 target)
			: base(start, target)
		{
		}

		// Token: 0x0600C658 RID: 50776 RVA: 0x0048C74C File Offset: 0x0048A94C
		public bool IsPosOpen(Vector3 pos)
		{
			return this.open.Find((FloodFillNode n) => n.Position == pos) != null;
		}

		// Token: 0x0600C659 RID: 50777 RVA: 0x0048C780 File Offset: 0x0048A980
		public bool IsPosClosed(Vector3 pos)
		{
			return this.closed.Find((FloodFillNode n) => n.Position == pos) != null;
		}

		// Token: 0x0600C65A RID: 50778 RVA: 0x0048C7B4 File Offset: 0x0048A9B4
		public FloodFillNode getLowestScore()
		{
			FloodFillNode floodFillNode = null;
			float num = float.MaxValue;
			float num2 = float.MaxValue;
			for (int i = 0; i < this.open.Count; i++)
			{
				FloodFillNode floodFillNode2 = this.open[i];
				if (floodFillNode2.F <= num && floodFillNode2.Heuristic < num2)
				{
					floodFillNode = floodFillNode2;
					num = floodFillNode2.F;
					num2 = floodFillNode2.Heuristic;
				}
			}
			return floodFillNode;
		}

		// Token: 0x0600C65B RID: 50779 RVA: 0x0048C81C File Offset: 0x0048AA1C
		public override void DebugDraw()
		{
			for (int i = 0; i < this.closed.Count; i++)
			{
				FloodFillNode floodFillNode = this.closed[i];
				if (floodFillNode.nodeType == cPathNodeType.Air)
				{
					RaycastPathUtils.DrawBounds(floodFillNode.BlockPos, Color.yellow, 0f, 1f);
				}
			}
			base.DebugDraw();
		}

		// Token: 0x040095E6 RID: 38374
		public List<FloodFillNode> open = new List<FloodFillNode>();

		// Token: 0x040095E7 RID: 38375
		public List<FloodFillNode> closed = new List<FloodFillNode>();
	}
}
