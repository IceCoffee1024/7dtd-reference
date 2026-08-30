using System;
using System.Collections;
using MusicUtils.Enums;

namespace DynamicMusic
{
	// Token: 0x02001A76 RID: 6774
	public interface ILayerMixer
	{
		// Token: 0x1700191E RID: 6430
		// (get) Token: 0x0600CCE6 RID: 52454
		// (set) Token: 0x0600CCE7 RID: 52455
		SectionType Sect { get; set; }

		// Token: 0x1700191F RID: 6431
		float this[int _idx] { get; }

		// Token: 0x0600CCE9 RID: 52457
		IEnumerator Load();

		// Token: 0x0600CCEA RID: 52458
		void Unload();
	}
}
