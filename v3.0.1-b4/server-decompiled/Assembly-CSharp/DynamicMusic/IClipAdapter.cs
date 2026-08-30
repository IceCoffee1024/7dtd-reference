using System;
using System.Collections;
using System.Xml.Linq;
using MusicUtils.Enums;

namespace DynamicMusic
{
	// Token: 0x02001A45 RID: 6725
	public interface IClipAdapter
	{
		// Token: 0x0600CB4B RID: 52043
		float GetSample(int idx, params float[] _params);

		// Token: 0x170018A7 RID: 6311
		// (get) Token: 0x0600CB4C RID: 52044
		bool IsLoaded { get; }

		// Token: 0x0600CB4D RID: 52045
		IEnumerator Load();

		// Token: 0x0600CB4E RID: 52046
		void LoadImmediate();

		// Token: 0x0600CB4F RID: 52047
		void Unload();

		// Token: 0x0600CB50 RID: 52048
		void SetPaths(int _num, PlacementType _placement, SectionType _section, LayerType _layer, string stress = "");

		// Token: 0x0600CB51 RID: 52049
		void ParseXml(XElement _xmlNode);
	}
}
