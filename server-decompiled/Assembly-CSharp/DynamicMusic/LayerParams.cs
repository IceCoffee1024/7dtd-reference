using System;

namespace DynamicMusic
{
	// Token: 0x02001A7B RID: 6779
	public class LayerParams
	{
		// Token: 0x0600CD85 RID: 52613 RVA: 0x004AD13E File Offset: 0x004AB33E
		public LayerParams(float _volume, float _mix)
		{
			this.Volume = _volume;
			this.Mix = _mix;
		}

		// Token: 0x04009BE9 RID: 39913
		public float Volume;

		// Token: 0x04009BEA RID: 39914
		public float Mix;
	}
}
