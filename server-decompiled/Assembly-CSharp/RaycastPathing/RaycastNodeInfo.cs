using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace RaycastPathing
{
	// Token: 0x020018A3 RID: 6307
	[Preserve]
	public class RaycastNodeInfo
	{
		// Token: 0x0600C25C RID: 49756 RVA: 0x0047E4F0 File Offset: 0x0047C6F0
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

		// Token: 0x0600C25D RID: 49757 RVA: 0x0047E588 File Offset: 0x0047C788
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

		// Token: 0x04009300 RID: 37632
		public readonly Vector3 Position;

		// Token: 0x04009301 RID: 37633
		public readonly Vector3i BlockPos;

		// Token: 0x04009302 RID: 37634
		public readonly float Scale;

		// Token: 0x04009303 RID: 37635
		public readonly int Depth;

		// Token: 0x04009304 RID: 37636
		public readonly Vector3 Min;

		// Token: 0x04009305 RID: 37637
		public readonly Vector3 Max;

		// Token: 0x04009306 RID: 37638
		public readonly Vector3 Center;
	}
}
