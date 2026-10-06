using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace RaycastPathing
{
	// Token: 0x020018F5 RID: 6389
	[Preserve]
	public class RaycastNodeInfo
	{
		// Token: 0x0600C53B RID: 50491 RVA: 0x00488B64 File Offset: 0x00486D64
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

		// Token: 0x0600C53C RID: 50492 RVA: 0x00488BFC File Offset: 0x00486DFC
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

		// Token: 0x040094EF RID: 38127
		public readonly Vector3 Position;

		// Token: 0x040094F0 RID: 38128
		public readonly Vector3i BlockPos;

		// Token: 0x040094F1 RID: 38129
		public readonly float Scale;

		// Token: 0x040094F2 RID: 38130
		public readonly int Depth;

		// Token: 0x040094F3 RID: 38131
		public readonly Vector3 Min;

		// Token: 0x040094F4 RID: 38132
		public readonly Vector3 Max;

		// Token: 0x040094F5 RID: 38133
		public readonly Vector3 Center;
	}
}
