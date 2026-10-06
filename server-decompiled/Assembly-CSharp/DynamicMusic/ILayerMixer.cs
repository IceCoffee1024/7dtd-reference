using System;
using System.Collections;
using MusicUtils.Enums;

namespace DynamicMusic
{
	// Token: 0x02001AA2 RID: 6818
	public interface ILayerMixer
	{
		// Token: 0x170019C0 RID: 6592
		// (get) Token: 0x0600CE3E RID: 52798
		// (set) Token: 0x0600CE3F RID: 52799
		SectionType Sect { get; set; }

		// Token: 0x170019C1 RID: 6593
		float this[int _idx] { get; }

		// Token: 0x0600CE41 RID: 52801
		IEnumerator Load();

		// Token: 0x0600CE42 RID: 52802
		void Unload();
	}
}
