using System;
using System.Collections.Generic;
using System.Xml.Linq;
using MusicUtils.Enums;

namespace DynamicMusic
{
	// Token: 0x02001A47 RID: 6727
	public interface IConfiguration
	{
		// Token: 0x170018A8 RID: 6312
		// (get) Token: 0x0600CB53 RID: 52051
		IList<SectionType> Sections { get; }

		// Token: 0x0600CB54 RID: 52052
		int CountFor(LayerType _layer);

		// Token: 0x0600CB55 RID: 52053
		void ParseFromXml(XElement _xmlNode);
	}
}
