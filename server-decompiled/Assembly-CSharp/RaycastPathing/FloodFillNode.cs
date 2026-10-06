using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace RaycastPathing
{
	// Token: 0x020018FA RID: 6394
	[Preserve]
	public class FloodFillNode : RaycastNode
	{
		// Token: 0x0600C54C RID: 50508 RVA: 0x00488EEF File Offset: 0x004870EF
		public FloodFillNode(Vector3 pos, float scale = 1f, int depth = 0)
			: base(pos, scale, depth)
		{
			this.score = new FloodFillNodeScore();
		}

		// Token: 0x0600C54D RID: 50509 RVA: 0x00488F05 File Offset: 0x00487105
		public FloodFillNode(Vector3 min, Vector3 max, float scale = 1f, int depth = 0)
			: base(min, max, scale, depth)
		{
			this.score = new FloodFillNodeScore();
		}

		// Token: 0x170018DB RID: 6363
		// (get) Token: 0x0600C54E RID: 50510 RVA: 0x00488F1D File Offset: 0x0048711D
		// (set) Token: 0x0600C54F RID: 50511 RVA: 0x00488F2A File Offset: 0x0048712A
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

		// Token: 0x170018DC RID: 6364
		// (get) Token: 0x0600C550 RID: 50512 RVA: 0x00488F38 File Offset: 0x00487138
		// (set) Token: 0x0600C551 RID: 50513 RVA: 0x00488F45 File Offset: 0x00487145
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

		// Token: 0x170018DD RID: 6365
		// (get) Token: 0x0600C552 RID: 50514 RVA: 0x00488F53 File Offset: 0x00487153
		public float F
		{
			get
			{
				return this.score.F;
			}
		}

		// Token: 0x04009501 RID: 38145
		[PublicizedFrom(EAccessModifier.Private)]
		public FloodFillNodeScore score;
	}
}
