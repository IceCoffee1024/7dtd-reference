using System;

namespace UnityEngine
{
	// Token: 0x02000271 RID: 625
	public class WaitForSecondsRealtime : CustomYieldInstruction
	{
		// Token: 0x170004E4 RID: 1252
		// (get) Token: 0x06001A22 RID: 6690 RVA: 0x0002C191 File Offset: 0x0002A391
		// (set) Token: 0x06001A23 RID: 6691 RVA: 0x0002C199 File Offset: 0x0002A399
		public float waitTime { get; set; }

		// Token: 0x170004E5 RID: 1253
		// (get) Token: 0x06001A24 RID: 6692 RVA: 0x0002C1A4 File Offset: 0x0002A3A4
		public override bool keepWaiting
		{
			get
			{
				bool flag = this.m_WaitUntilTime < 0f;
				if (flag)
				{
					this.m_WaitUntilTime = Time.realtimeSinceStartup + this.waitTime;
				}
				bool flag2 = Time.realtimeSinceStartup < this.m_WaitUntilTime;
				bool flag3 = !flag2;
				if (flag3)
				{
					this.Reset();
				}
				return flag2;
			}
		}

		// Token: 0x06001A25 RID: 6693 RVA: 0x0002C1FB File Offset: 0x0002A3FB
		public WaitForSecondsRealtime(float time)
		{
			this.waitTime = time;
		}

		// Token: 0x06001A26 RID: 6694 RVA: 0x0002C218 File Offset: 0x0002A418
		public override void Reset()
		{
			this.m_WaitUntilTime = -1f;
		}

		// Token: 0x04000915 RID: 2325
		private float m_WaitUntilTime = -1f;
	}
}
