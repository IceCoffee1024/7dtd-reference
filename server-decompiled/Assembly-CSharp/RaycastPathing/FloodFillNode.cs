using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace RaycastPathing
{
	// Token: 0x020018A8 RID: 6312
	[Preserve]
	public class FloodFillNode : RaycastNode
	{
		// Token: 0x0600C26D RID: 49773 RVA: 0x0047E87B File Offset: 0x0047CA7B
		public FloodFillNode(Vector3 pos, float scale = 1f, int depth = 0)
			: base(pos, scale, depth)
		{
			this.score = new FloodFillNodeScore();
		}

		// Token: 0x0600C26E RID: 49774 RVA: 0x0047E891 File Offset: 0x0047CA91
		public FloodFillNode(Vector3 min, Vector3 max, float scale = 1f, int depth = 0)
			: base(min, max, scale, depth)
		{
			this.score = new FloodFillNodeScore();
		}

		// Token: 0x170017C7 RID: 6087
		// (get) Token: 0x0600C26F RID: 49775 RVA: 0x0047E8A9 File Offset: 0x0047CAA9
		// (set) Token: 0x0600C270 RID: 49776 RVA: 0x0047E8B6 File Offset: 0x0047CAB6
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

		// Token: 0x170017C8 RID: 6088
		// (get) Token: 0x0600C271 RID: 49777 RVA: 0x0047E8C4 File Offset: 0x0047CAC4
		// (set) Token: 0x0600C272 RID: 49778 RVA: 0x0047E8D1 File Offset: 0x0047CAD1
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

		// Token: 0x170017C9 RID: 6089
		// (get) Token: 0x0600C273 RID: 49779 RVA: 0x0047E8DF File Offset: 0x0047CADF
		public float F
		{
			get
			{
				return this.score.F;
			}
		}

		// Token: 0x04009312 RID: 37650
		[PublicizedFrom(EAccessModifier.Private)]
		public FloodFillNodeScore score;
	}
}
