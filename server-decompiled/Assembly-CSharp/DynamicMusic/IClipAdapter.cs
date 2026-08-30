using System;
using System.Collections;
using System.Xml.Linq;
using MusicUtils.Enums;

namespace DynamicMusic
{
	// Token: 0x02001A6C RID: 6764
	public interface IClipAdapter
	{
		// Token: 0x0600CCD4 RID: 52436
		float GetSample(int idx, params float[] _params);

		// Token: 0x17001919 RID: 6425
		// (get) Token: 0x0600CCD5 RID: 52437
		bool IsLoaded { get; }

		// Token: 0x0600CCD6 RID: 52438
		IEnumerator Load();

		// Token: 0x0600CCD7 RID: 52439
		void LoadImmediate();

		// Token: 0x0600CCD8 RID: 52440
		void Unload();

		// Token: 0x0600CCD9 RID: 52441
		void SetPaths(int _num, PlacementType _placement, SectionType _section, LayerType _layer, string stress = "");

		// Token: 0x0600CCDA RID: 52442
		void ParseXml(XElement _xmlNode);
	}
}
