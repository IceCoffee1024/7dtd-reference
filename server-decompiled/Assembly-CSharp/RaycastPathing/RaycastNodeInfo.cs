using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace RaycastPathing
{
	// Token: 0x02001926 RID: 6438
	[Preserve]
	public class RaycastNodeInfo
	{
		// Token: 0x0600C64F RID: 50767 RVA: 0x0048C510 File Offset: 0x0048A710
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

		// Token: 0x0600C650 RID: 50768 RVA: 0x0048C5A8 File Offset: 0x0048A7A8
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

		// Token: 0x040095D8 RID: 38360
		public readonly Vector3 Position;

		// Token: 0x040095D9 RID: 38361
		public readonly Vector3i BlockPos;

		// Token: 0x040095DA RID: 38362
		public readonly float Scale;

		// Token: 0x040095DB RID: 38363
		public readonly int Depth;

		// Token: 0x040095DC RID: 38364
		public readonly Vector3 Min;

		// Token: 0x040095DD RID: 38365
		public readonly Vector3 Max;

		// Token: 0x040095DE RID: 38366
		public readonly Vector3 Center;
	}
}
