using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace RaycastPathing
{
	// Token: 0x02001923 RID: 6435
	[Preserve]
	public class RaycastPathInfo
	{
		// Token: 0x0600C630 RID: 50736 RVA: 0x0048C31C File Offset: 0x0048A51C
		public RaycastPathInfo(Vector3 start, Vector3 target)
		{
			this.Start = start;
			this.Target = target;
			this.StartNode = new RaycastNode(this.Start, 1f, 0);
			this.TargetNode = new RaycastNode(this.Target, 1f, 0);
			this.PathStartsIndoors = RaycastPathWorldUtils.IsUnderground(start);
			this.PathEndsIndoors = RaycastPathWorldUtils.IsUnderground(target);
		}

		// Token: 0x17001911 RID: 6417
		// (get) Token: 0x0600C631 RID: 50737 RVA: 0x0048C383 File Offset: 0x0048A583
		public Vector3i StartBlockPos
		{
			get
			{
				return this.StartNode.BlockPos;
			}
		}

		// Token: 0x17001912 RID: 6418
		// (get) Token: 0x0600C632 RID: 50738 RVA: 0x0048C390 File Offset: 0x0048A590
		public Vector3i TargetBlockPos
		{
			get
			{
				return this.TargetNode.BlockPos;
			}
		}

		// Token: 0x17001913 RID: 6419
		// (get) Token: 0x0600C633 RID: 50739 RVA: 0x0048C39D File Offset: 0x0048A59D
		// (set) Token: 0x0600C634 RID: 50740 RVA: 0x0048C3A5 File Offset: 0x0048A5A5
		public bool PathStartsIndoors
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001914 RID: 6420
		// (get) Token: 0x0600C635 RID: 50741 RVA: 0x0048C3AE File Offset: 0x0048A5AE
		// (set) Token: 0x0600C636 RID: 50742 RVA: 0x0048C3B6 File Offset: 0x0048A5B6
		public bool PathEndsIndoors
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600C637 RID: 50743 RVA: 0x0019D88C File Offset: 0x0019BA8C
		public static implicit operator bool(RaycastPathInfo exists)
		{
			return exists != null;
		}

		// Token: 0x040095C9 RID: 38345
		public readonly Vector3 Start;

		// Token: 0x040095CA RID: 38346
		public readonly Vector3 Target;

		// Token: 0x040095CB RID: 38347
		[PublicizedFrom(EAccessModifier.Private)]
		public RaycastNode StartNode;

		// Token: 0x040095CC RID: 38348
		[PublicizedFrom(EAccessModifier.Private)]
		public RaycastNode TargetNode;
	}
}
