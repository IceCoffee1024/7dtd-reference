using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace RaycastPathing
{
	// Token: 0x020018CB RID: 6347
	[Preserve]
	public class FloodFillPath : RaycastPath
	{
		// Token: 0x0600C3EB RID: 50155 RVA: 0x00484835 File Offset: 0x00482A35
		public FloodFillPath(Vector3 start, Vector3 target)
			: base(start, target)
		{
		}

		// Token: 0x0600C3EC RID: 50156 RVA: 0x00484858 File Offset: 0x00482A58
		public bool IsPosOpen(Vector3 pos)
		{
			return this.open.Find((FloodFillNode n) => n.Position == pos) != null;
		}

		// Token: 0x0600C3ED RID: 50157 RVA: 0x0048488C File Offset: 0x00482A8C
		public bool IsPosClosed(Vector3 pos)
		{
			return this.closed.Find((FloodFillNode n) => n.Position == pos) != null;
		}

		// Token: 0x0600C3EE RID: 50158 RVA: 0x004848C0 File Offset: 0x00482AC0
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

		// Token: 0x0600C3EF RID: 50159 RVA: 0x00484928 File Offset: 0x00482B28
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

		// Token: 0x04009437 RID: 37943
		public List<FloodFillNode> open = new List<FloodFillNode>();

		// Token: 0x04009438 RID: 37944
		public List<FloodFillNode> closed = new List<FloodFillNode>();
	}
}
