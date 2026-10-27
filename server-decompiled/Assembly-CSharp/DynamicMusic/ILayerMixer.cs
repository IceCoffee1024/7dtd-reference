using System;
using System.Collections;
using MusicUtils.Enums;

namespace DynamicMusic
{
	// Token: 0x02001AD3 RID: 6867
	public interface ILayerMixer
	{
		// Token: 0x17001A08 RID: 6664
		// (get) Token: 0x0600CF52 RID: 53074
		// (set) Token: 0x0600CF53 RID: 53075
		SectionType Sect { get; set; }

		// Token: 0x17001A09 RID: 6665
		float this[int _idx] { get; }

		// Token: 0x0600CF55 RID: 53077
		IEnumerator Load();

		// Token: 0x0600CF56 RID: 53078
		void Unload();
	}
}
