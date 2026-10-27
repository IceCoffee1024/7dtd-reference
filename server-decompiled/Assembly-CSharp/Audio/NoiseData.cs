using System;

namespace Audio
{
	// Token: 0x02001B77 RID: 7031
	public class NoiseData
	{
		// Token: 0x0600D2A1 RID: 53921 RVA: 0x004C48B7 File Offset: 0x004C2AB7
		public NoiseData()
		{
			this.volume = 0f;
			this.time = 1f;
			this.heatMapStrength = 0f;
			this.heatMapTime = 100UL;
			this.crouchMuffle = 1f;
		}

		// Token: 0x0400A04C RID: 41036
		public float volume;

		// Token: 0x0400A04D RID: 41037
		public float time;

		// Token: 0x0400A04E RID: 41038
		public float heatMapStrength;

		// Token: 0x0400A04F RID: 41039
		public ulong heatMapTime;

		// Token: 0x0400A050 RID: 41040
		public float crouchMuffle;
	}
}
