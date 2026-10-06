using System;
using System.Collections.Generic;
using System.Xml.Linq;
using MusicUtils.Enums;

namespace DynamicMusic
{
	// Token: 0x02001A9A RID: 6810
	public interface IConfiguration
	{
		// Token: 0x170019BC RID: 6588
		// (get) Token: 0x0600CE34 RID: 52788
		IList<SectionType> Sections { get; }

		// Token: 0x0600CE35 RID: 52789
		int CountFor(LayerType _layer);

		// Token: 0x0600CE36 RID: 52790
		void ParseFromXml(XElement _xmlNode);
	}
}
