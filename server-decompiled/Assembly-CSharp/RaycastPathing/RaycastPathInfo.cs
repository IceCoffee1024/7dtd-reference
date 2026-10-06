using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace RaycastPathing
{
	// Token: 0x020018F2 RID: 6386
	[Preserve]
	public class RaycastPathInfo
	{
		// Token: 0x0600C51C RID: 50460 RVA: 0x00488970 File Offset: 0x00486B70
		public RaycastPathInfo(Vector3 start, Vector3 target)
		{
			this.Start = start;
			this.Target = target;
			this.StartNode = new RaycastNode(this.Start, 1f, 0);
			this.TargetNode = new RaycastNode(this.Target, 1f, 0);
			this.PathStartsIndoors = RaycastPathWorldUtils.IsUnderground(start);
			this.PathEndsIndoors = RaycastPathWorldUtils.IsUnderground(target);
		}

		// Token: 0x170018C9 RID: 6345
		// (get) Token: 0x0600C51D RID: 50461 RVA: 0x004889D7 File Offset: 0x00486BD7
		public Vector3i StartBlockPos
		{
			get
			{
				return this.StartNode.BlockPos;
			}
		}

		// Token: 0x170018CA RID: 6346
		// (get) Token: 0x0600C51E RID: 50462 RVA: 0x004889E4 File Offset: 0x00486BE4
		public Vector3i TargetBlockPos
		{
			get
			{
				return this.TargetNode.BlockPos;
			}
		}

		// Token: 0x170018CB RID: 6347
		// (get) Token: 0x0600C51F RID: 50463 RVA: 0x004889F1 File Offset: 0x00486BF1
		// (set) Token: 0x0600C520 RID: 50464 RVA: 0x004889F9 File Offset: 0x00486BF9
		public bool PathStartsIndoors
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x170018CC RID: 6348
		// (get) Token: 0x0600C521 RID: 50465 RVA: 0x00488A02 File Offset: 0x00486C02
		// (set) Token: 0x0600C522 RID: 50466 RVA: 0x00488A0A File Offset: 0x00486C0A
		public bool PathEndsIndoors
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600C523 RID: 50467 RVA: 0x0019C7E4 File Offset: 0x0019A9E4
		public static implicit operator bool(RaycastPathInfo exists)
		{
			return exists != null;
		}

		// Token: 0x040094E0 RID: 38112
		public readonly Vector3 Start;

		// Token: 0x040094E1 RID: 38113
		public readonly Vector3 Target;

		// Token: 0x040094E2 RID: 38114
		[PublicizedFrom(EAccessModifier.Private)]
		public RaycastNode StartNode;

		// Token: 0x040094E3 RID: 38115
		[PublicizedFrom(EAccessModifier.Private)]
		public RaycastNode TargetNode;
	}
}
