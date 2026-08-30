using System;

namespace Audio
{
	// Token: 0x02001AF3 RID: 6899
	public class NoiseData
	{
		// Token: 0x0600CEAC RID: 52908 RVA: 0x004B64CF File Offset: 0x004B46CF
		public NoiseData()
		{
			this.volume = 0f;
			this.time = 1f;
			this.heatMapStrength = 0f;
			this.heatMapTime = 100UL;
			this.crouchMuffle = 1f;
		}

		// Token: 0x04009D6C RID: 40300
		public float volume;

		// Token: 0x04009D6D RID: 40301
		public float time;

		// Token: 0x04009D6E RID: 40302
		public float heatMapStrength;

		// Token: 0x04009D6F RID: 40303
		public ulong heatMapTime;

		// Token: 0x04009D70 RID: 40304
		public float crouchMuffle;
	}
}
