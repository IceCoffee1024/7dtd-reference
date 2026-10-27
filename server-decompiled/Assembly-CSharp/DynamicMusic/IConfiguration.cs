using System;
using System.Collections.Generic;
using System.Xml.Linq;
using MusicUtils.Enums;

namespace DynamicMusic
{
	// Token: 0x02001ACB RID: 6859
	public interface IConfiguration
	{
		// Token: 0x17001A04 RID: 6660
		// (get) Token: 0x0600CF48 RID: 53064
		IList<SectionType> Sections { get; }

		// Token: 0x0600CF49 RID: 53065
		int CountFor(LayerType _layer);

		// Token: 0x0600CF4A RID: 53066
		void ParseFromXml(XElement _xmlNode);
	}
}
