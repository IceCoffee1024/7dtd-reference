using System;

namespace Audio
{
	// Token: 0x02001B1A RID: 6938
	public class NoiseData
	{
		// Token: 0x0600D035 RID: 53301 RVA: 0x004BC8C3 File Offset: 0x004BAAC3
		public NoiseData()
		{
			this.volume = 0f;
			this.time = 1f;
			this.heatMapStrength = 0f;
			this.heatMapTime = 100UL;
			this.crouchMuffle = 1f;
		}

		// Token: 0x04009E9D RID: 40605
		public float volume;

		// Token: 0x04009E9E RID: 40606
		public float time;

		// Token: 0x04009E9F RID: 40607
		public float heatMapStrength;

		// Token: 0x04009EA0 RID: 40608
		public ulong heatMapTime;

		// Token: 0x04009EA1 RID: 40609
		public float crouchMuffle;
	}
}
