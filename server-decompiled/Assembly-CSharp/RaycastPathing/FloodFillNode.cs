using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace RaycastPathing
{
	// Token: 0x0200192B RID: 6443
	[Preserve]
	public class FloodFillNode : RaycastNode
	{
		// Token: 0x0600C660 RID: 50784 RVA: 0x0048C89B File Offset: 0x0048AA9B
		public FloodFillNode(Vector3 pos, float scale = 1f, int depth = 0)
			: base(pos, scale, depth)
		{
			this.score = new FloodFillNodeScore();
		}

		// Token: 0x0600C661 RID: 50785 RVA: 0x0048C8B1 File Offset: 0x0048AAB1
		public FloodFillNode(Vector3 min, Vector3 max, float scale = 1f, int depth = 0)
			: base(min, max, scale, depth)
		{
			this.score = new FloodFillNodeScore();
		}

		// Token: 0x17001923 RID: 6435
		// (get) Token: 0x0600C662 RID: 50786 RVA: 0x0048C8C9 File Offset: 0x0048AAC9
		// (set) Token: 0x0600C663 RID: 50787 RVA: 0x0048C8D6 File Offset: 0x0048AAD6
		public float G
		{
			get
			{
				return this.score.G;
			}
			set
			{
				this.score.G = value;
			}
		}

		// Token: 0x17001924 RID: 6436
		// (get) Token: 0x0600C664 RID: 50788 RVA: 0x0048C8E4 File Offset: 0x0048AAE4
		// (set) Token: 0x0600C665 RID: 50789 RVA: 0x0048C8F1 File Offset: 0x0048AAF1
		public float Heuristic
		{
			get
			{
				return this.score.H;
			}
			set
			{
				this.score.H = value;
			}
		}

		// Token: 0x17001925 RID: 6437
		// (get) Token: 0x0600C666 RID: 50790 RVA: 0x0048C8FF File Offset: 0x0048AAFF
		public float F
		{
			get
			{
				return this.score.F;
			}
		}

		// Token: 0x040095EA RID: 38378
		[PublicizedFrom(EAccessModifier.Private)]
		public FloodFillNodeScore score;
	}
}
