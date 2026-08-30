using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace RaycastPathing
{
	// Token: 0x020018C9 RID: 6345
	[Preserve]
	public class RaycastNodeInfo
	{
		// Token: 0x0600C3E3 RID: 50147 RVA: 0x0048461C File Offset: 0x0048281C
		public RaycastNodeInfo(Vector3 pos, float scale = 1f, int depth = 0)
		{
			this.Position = pos;
			this.BlockPos = World.worldToBlockPos(pos);
			this.Scale = scale;
			this.Depth = depth;
			this.Min = pos - Vector3.one * scale * 0.5f;
			this.Max = pos + Vector3.one * scale * 0.5f;
			this.Center = (this.Min + this.Max) * 0.5f;
		}

		// Token: 0x0600C3E4 RID: 50148 RVA: 0x004846B4 File Offset: 0x004828B4
		public RaycastNodeInfo(Vector3 min, Vector3 max, float scale = 1f, int depth = 0)
		{
			this.Position = (min + max) * 0.5f;
			this.BlockPos = World.worldToBlockPos(this.Position);
			this.Scale = scale;
			this.Depth = depth;
			this.Min = min;
			this.Max = max;
			this.Center = this.Position;
		}

		// Token: 0x04009429 RID: 37929
		public readonly Vector3 Position;

		// Token: 0x0400942A RID: 37930
		public readonly Vector3i BlockPos;

		// Token: 0x0400942B RID: 37931
		public readonly float Scale;

		// Token: 0x0400942C RID: 37932
		public readonly int Depth;

		// Token: 0x0400942D RID: 37933
		public readonly Vector3 Min;

		// Token: 0x0400942E RID: 37934
		public readonly Vector3 Max;

		// Token: 0x0400942F RID: 37935
		public readonly Vector3 Center;
	}
}
