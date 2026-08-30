using System;

namespace DynamicMusic
{
	// Token: 0x02001A28 RID: 6696
	public class LayerParams
	{
		// Token: 0x0600CAA4 RID: 51876 RVA: 0x004A287E File Offset: 0x004A0A7E
		public LayerParams(float _volume, float _mix)
		{
			this.Volume = _volume;
			this.Mix = _mix;
		}

		// Token: 0x040099F2 RID: 39410
		public float Volume;

		// Token: 0x040099F3 RID: 39411
		public float Mix;
	}
}
