using System;

namespace Audio
{
	// Token: 0x02001B46 RID: 6982
	public class NoiseData
	{
		// Token: 0x0600D18D RID: 53645 RVA: 0x004C0D8F File Offset: 0x004BEF8F
		public NoiseData()
		{
			this.volume = 0f;
			this.time = 1f;
			this.heatMapStrength = 0f;
			this.heatMapTime = 100UL;
			this.crouchMuffle = 1f;
		}

		// Token: 0x04009F63 RID: 40803
		public float volume;

		// Token: 0x04009F64 RID: 40804
		public float time;

		// Token: 0x04009F65 RID: 40805
		public float heatMapStrength;

		// Token: 0x04009F66 RID: 40806
		public ulong heatMapTime;

		// Token: 0x04009F67 RID: 40807
		public float crouchMuffle;
	}
}
