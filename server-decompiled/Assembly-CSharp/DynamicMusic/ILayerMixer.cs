using System;
using System.Collections;
using MusicUtils.Enums;

namespace DynamicMusic
{
	// Token: 0x02001A4F RID: 6735
	public interface ILayerMixer
	{
		// Token: 0x170018AC RID: 6316
		// (get) Token: 0x0600CB5D RID: 52061
		// (set) Token: 0x0600CB5E RID: 52062
		SectionType Sect { get; set; }

		// Token: 0x170018AD RID: 6317
		float this[int _idx] { get; }

		// Token: 0x0600CB60 RID: 52064
		IEnumerator Load();

		// Token: 0x0600CB61 RID: 52065
		void Unload();
	}
}
