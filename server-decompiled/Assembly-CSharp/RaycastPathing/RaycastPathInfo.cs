using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace RaycastPathing
{
	// Token: 0x020018C6 RID: 6342
	[Preserve]
	public class RaycastPathInfo
	{
		// Token: 0x0600C3C4 RID: 50116 RVA: 0x00484428 File Offset: 0x00482628
		public RaycastPathInfo(Vector3 start, Vector3 target)
		{
			this.Start = start;
			this.Target = target;
			this.StartNode = new RaycastNode(this.Start, 1f, 0);
			this.TargetNode = new RaycastNode(this.Target, 1f, 0);
			this.PathStartsIndoors = RaycastPathWorldUtils.IsUnderground(start);
			this.PathEndsIndoors = RaycastPathWorldUtils.IsUnderground(target);
		}

		// Token: 0x17001827 RID: 6183
		// (get) Token: 0x0600C3C5 RID: 50117 RVA: 0x0048448F File Offset: 0x0048268F
		public Vector3i StartBlockPos
		{
			get
			{
				return this.StartNode.BlockPos;
			}
		}

		// Token: 0x17001828 RID: 6184
		// (get) Token: 0x0600C3C6 RID: 50118 RVA: 0x0048449C File Offset: 0x0048269C
		public Vector3i TargetBlockPos
		{
			get
			{
				return this.TargetNode.BlockPos;
			}
		}

		// Token: 0x17001829 RID: 6185
		// (get) Token: 0x0600C3C7 RID: 50119 RVA: 0x004844A9 File Offset: 0x004826A9
		// (set) Token: 0x0600C3C8 RID: 50120 RVA: 0x004844B1 File Offset: 0x004826B1
		public bool PathStartsIndoors
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x1700182A RID: 6186
		// (get) Token: 0x0600C3C9 RID: 50121 RVA: 0x004844BA File Offset: 0x004826BA
		// (set) Token: 0x0600C3CA RID: 50122 RVA: 0x004844C2 File Offset: 0x004826C2
		public bool PathEndsIndoors
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600C3CB RID: 50123 RVA: 0x0019C6D0 File Offset: 0x0019A8D0
		public static implicit operator bool(RaycastPathInfo exists)
		{
			return exists != null;
		}

		// Token: 0x0400941A RID: 37914
		public readonly Vector3 Start;

		// Token: 0x0400941B RID: 37915
		public readonly Vector3 Target;

		// Token: 0x0400941C RID: 37916
		[PublicizedFrom(EAccessModifier.Private)]
		public RaycastNode StartNode;

		// Token: 0x0400941D RID: 37917
		[PublicizedFrom(EAccessModifier.Private)]
		public RaycastNode TargetNode;
	}
}
