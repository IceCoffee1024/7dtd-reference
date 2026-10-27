using System;

namespace DynamicMusic
{
	// Token: 0x02001AAC RID: 6828
	public class LayerParams
	{
		// Token: 0x0600CE99 RID: 52889 RVA: 0x004B0C3A File Offset: 0x004AEE3A
		public LayerParams(float _volume, float _mix)
		{
			this.Volume = _volume;
			this.Mix = _mix;
		}

		// Token: 0x04009CD2 RID: 40146
		public float Volume;

		// Token: 0x04009CD3 RID: 40147
		public float Mix;
	}
}
