using System;
using System.Collections.Generic;
using System.Xml.Linq;
using MusicUtils.Enums;

namespace DynamicMusic
{
	// Token: 0x02001A6E RID: 6766
	public interface IConfiguration
	{
		// Token: 0x1700191A RID: 6426
		// (get) Token: 0x0600CCDC RID: 52444
		IList<SectionType> Sections { get; }

		// Token: 0x0600CCDD RID: 52445
		int CountFor(LayerType _layer);

		// Token: 0x0600CCDE RID: 52446
		void ParseFromXml(XElement _xmlNode);
	}
}
