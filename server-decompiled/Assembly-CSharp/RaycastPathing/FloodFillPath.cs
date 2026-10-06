using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace RaycastPathing
{
	// Token: 0x020018F7 RID: 6391
	[Preserve]
	public class FloodFillPath : RaycastPath
	{
		// Token: 0x0600C543 RID: 50499 RVA: 0x00488D7D File Offset: 0x00486F7D
		public FloodFillPath(Vector3 start, Vector3 target)
			: base(start, target)
		{
		}

		// Token: 0x0600C544 RID: 50500 RVA: 0x00488DA0 File Offset: 0x00486FA0
		public bool IsPosOpen(Vector3 pos)
		{
			return this.open.Find((FloodFillNode n) => n.Position == pos) != null;
		}

		// Token: 0x0600C545 RID: 50501 RVA: 0x00488DD4 File Offset: 0x00486FD4
		public bool IsPosClosed(Vector3 pos)
		{
			return this.closed.Find((FloodFillNode n) => n.Position == pos) != null;
		}

		// Token: 0x0600C546 RID: 50502 RVA: 0x00488E08 File Offset: 0x00487008
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

		// Token: 0x0600C547 RID: 50503 RVA: 0x00488E70 File Offset: 0x00487070
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

		// Token: 0x040094FD RID: 38141
		public List<FloodFillNode> open = new List<FloodFillNode>();

		// Token: 0x040094FE RID: 38142
		public List<FloodFillNode> closed = new List<FloodFillNode>();
	}
}
