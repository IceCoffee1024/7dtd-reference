using System;

namespace DynamicMusic
{
	// Token: 0x02001A4F RID: 6735
	public class LayerParams
	{
		// Token: 0x0600CC2D RID: 52269 RVA: 0x004A8C72 File Offset: 0x004A6E72
		public LayerParams(float _volume, float _mix)
		{
			this.Volume = _volume;
			this.Mix = _mix;
		}

		// Token: 0x04009B23 RID: 39715
		public float Volume;

		// Token: 0x04009B24 RID: 39716
		public float Mix;
	}
}
