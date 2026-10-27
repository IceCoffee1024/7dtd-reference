using System;
using System.Collections;
using System.Xml.Linq;
using MusicUtils.Enums;

namespace DynamicMusic
{
	// Token: 0x02001AC9 RID: 6857
	public interface IClipAdapter
	{
		// Token: 0x0600CF40 RID: 53056
		float GetSample(int idx, params float[] _params);

		// Token: 0x17001A03 RID: 6659
		// (get) Token: 0x0600CF41 RID: 53057
		bool IsLoaded { get; }

		// Token: 0x0600CF42 RID: 53058
		IEnumerator Load();

		// Token: 0x0600CF43 RID: 53059
		void LoadImmediate();

		// Token: 0x0600CF44 RID: 53060
		void Unload();

		// Token: 0x0600CF45 RID: 53061
		void SetPaths(int _num, PlacementType _placement, SectionType _section, LayerType _layer, string stress = "");

		// Token: 0x0600CF46 RID: 53062
		void ParseXml(XElement _xmlNode);
	}
}
