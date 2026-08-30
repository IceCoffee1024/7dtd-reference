using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace RaycastPathing
{
	// Token: 0x020018A0 RID: 6304
	[Preserve]
	public class RaycastPathInfo
	{
		// Token: 0x0600C23D RID: 49725 RVA: 0x0047E2FC File Offset: 0x0047C4FC
		public RaycastPathInfo(Vector3 start, Vector3 target)
		{
			this.Start = start;
			this.Target = target;
			this.StartNode = new RaycastNode(this.Start, 1f, 0);
			this.TargetNode = new RaycastNode(this.Target, 1f, 0);
			this.PathStartsIndoors = RaycastPathWorldUtils.IsUnderground(start);
			this.PathEndsIndoors = RaycastPathWorldUtils.IsUnderground(target);
		}

		// Token: 0x170017B5 RID: 6069
		// (get) Token: 0x0600C23E RID: 49726 RVA: 0x0047E363 File Offset: 0x0047C563
		public Vector3i StartBlockPos
		{
			get
			{
				return this.StartNode.BlockPos;
			}
		}

		// Token: 0x170017B6 RID: 6070
		// (get) Token: 0x0600C23F RID: 49727 RVA: 0x0047E370 File Offset: 0x0047C570
		public Vector3i TargetBlockPos
		{
			get
			{
				return this.TargetNode.BlockPos;
			}
		}

		// Token: 0x170017B7 RID: 6071
		// (get) Token: 0x0600C240 RID: 49728 RVA: 0x0047E37D File Offset: 0x0047C57D
		// (set) Token: 0x0600C241 RID: 49729 RVA: 0x0047E385 File Offset: 0x0047C585
		public bool PathStartsIndoors
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x170017B8 RID: 6072
		// (get) Token: 0x0600C242 RID: 49730 RVA: 0x0047E38E File Offset: 0x0047C58E
		// (set) Token: 0x0600C243 RID: 49731 RVA: 0x0047E396 File Offset: 0x0047C596
		public bool PathEndsIndoors
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600C244 RID: 49732 RVA: 0x0019A5F8 File Offset: 0x001987F8
		public static implicit operator bool(RaycastPathInfo exists)
		{
			return exists != null;
		}

		// Token: 0x040092F1 RID: 37617
		public readonly Vector3 Start;

		// Token: 0x040092F2 RID: 37618
		public readonly Vector3 Target;

		// Token: 0x040092F3 RID: 37619
		[PublicizedFrom(EAccessModifier.Private)]
		public RaycastNode StartNode;

		// Token: 0x040092F4 RID: 37620
		[PublicizedFrom(EAccessModifier.Private)]
		public RaycastNode TargetNode;
	}
}
