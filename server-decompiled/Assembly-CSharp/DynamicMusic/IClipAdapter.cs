using System;
using System.Collections;
using System.Xml.Linq;
using MusicUtils.Enums;

namespace DynamicMusic
{
	// Token: 0x02001A98 RID: 6808
	public interface IClipAdapter
	{
		// Token: 0x0600CE2C RID: 52780
		float GetSample(int idx, params float[] _params);

		// Token: 0x170019BB RID: 6587
		// (get) Token: 0x0600CE2D RID: 52781
		bool IsLoaded { get; }

		// Token: 0x0600CE2E RID: 52782
		IEnumerator Load();

		// Token: 0x0600CE2F RID: 52783
		void LoadImmediate();

		// Token: 0x0600CE30 RID: 52784
		void Unload();

		// Token: 0x0600CE31 RID: 52785
		void SetPaths(int _num, PlacementType _placement, SectionType _section, LayerType _layer, string stress = "");

		// Token: 0x0600CE32 RID: 52786
		void ParseXml(XElement _xmlNode);
	}
}
