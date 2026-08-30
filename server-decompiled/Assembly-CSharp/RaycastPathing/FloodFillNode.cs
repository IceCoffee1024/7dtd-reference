using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace RaycastPathing
{
	// Token: 0x020018CE RID: 6350
	[Preserve]
	public class FloodFillNode : RaycastNode
	{
		// Token: 0x0600C3F4 RID: 50164 RVA: 0x004849A7 File Offset: 0x00482BA7
		public FloodFillNode(Vector3 pos, float scale = 1f, int depth = 0)
			: base(pos, scale, depth)
		{
			this.score = new FloodFillNodeScore();
		}

		// Token: 0x0600C3F5 RID: 50165 RVA: 0x004849BD File Offset: 0x00482BBD
		public FloodFillNode(Vector3 min, Vector3 max, float scale = 1f, int depth = 0)
			: base(min, max, scale, depth)
		{
			this.score = new FloodFillNodeScore();
		}

		// Token: 0x17001839 RID: 6201
		// (get) Token: 0x0600C3F6 RID: 50166 RVA: 0x004849D5 File Offset: 0x00482BD5
		// (set) Token: 0x0600C3F7 RID: 50167 RVA: 0x004849E2 File Offset: 0x00482BE2
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

		// Token: 0x1700183A RID: 6202
		// (get) Token: 0x0600C3F8 RID: 50168 RVA: 0x004849F0 File Offset: 0x00482BF0
		// (set) Token: 0x0600C3F9 RID: 50169 RVA: 0x004849FD File Offset: 0x00482BFD
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

		// Token: 0x1700183B RID: 6203
		// (get) Token: 0x0600C3FA RID: 50170 RVA: 0x00484A0B File Offset: 0x00482C0B
		public float F
		{
			get
			{
				return this.score.F;
			}
		}

		// Token: 0x0400943B RID: 37947
		[PublicizedFrom(EAccessModifier.Private)]
		public FloodFillNodeScore score;
	}
}
