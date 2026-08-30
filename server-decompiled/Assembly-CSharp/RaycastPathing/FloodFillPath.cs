using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace RaycastPathing
{
	// Token: 0x020018A5 RID: 6309
	[Preserve]
	public class FloodFillPath : RaycastPath
	{
		// Token: 0x0600C264 RID: 49764 RVA: 0x0047E709 File Offset: 0x0047C909
		public FloodFillPath(Vector3 start, Vector3 target)
			: base(start, target)
		{
		}

		// Token: 0x0600C265 RID: 49765 RVA: 0x0047E72C File Offset: 0x0047C92C
		public bool IsPosOpen(Vector3 pos)
		{
			return this.open.Find((FloodFillNode n) => n.Position == pos) != null;
		}

		// Token: 0x0600C266 RID: 49766 RVA: 0x0047E760 File Offset: 0x0047C960
		public bool IsPosClosed(Vector3 pos)
		{
			return this.closed.Find((FloodFillNode n) => n.Position == pos) != null;
		}

		// Token: 0x0600C267 RID: 49767 RVA: 0x0047E794 File Offset: 0x0047C994
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

		// Token: 0x0600C268 RID: 49768 RVA: 0x0047E7FC File Offset: 0x0047C9FC
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

		// Token: 0x0400930E RID: 37646
		public List<FloodFillNode> open = new List<FloodFillNode>();

		// Token: 0x0400930F RID: 37647
		public List<FloodFillNode> closed = new List<FloodFillNode>();
	}
}
