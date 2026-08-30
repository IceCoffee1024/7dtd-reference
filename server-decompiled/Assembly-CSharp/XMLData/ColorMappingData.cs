using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace XMLData
{
	// Token: 0x02001638 RID: 5688
	[Preserve]
	public class ColorMappingData : IXMLData
	{
		// Token: 0x17001501 RID: 5377
		// (get) Token: 0x0600B288 RID: 45704 RVA: 0x004285C7 File Offset: 0x004267C7
		public static ColorMappingData Instance
		{
			get
			{
				ColorMappingData colorMappingData;
				if ((colorMappingData = ColorMappingData.instance) == null)
				{
					colorMappingData = (ColorMappingData.instance = new ColorMappingData());
				}
				return colorMappingData;
			}
		}

		// Token: 0x0600B289 RID: 45705 RVA: 0x004285DD File Offset: 0x004267DD
		[PublicizedFrom(EAccessModifier.Private)]
		public ColorMappingData()
		{
			this.IDFromName = new Dictionary<string, int>();
			this.NameFromID = new Dictionary<int, string>();
			this.ColorFromID = new Dictionary<int, Color>();
		}

		// Token: 0x04008621 RID: 34337
		[PublicizedFrom(EAccessModifier.Private)]
		public static ColorMappingData instance;

		// Token: 0x04008622 RID: 34338
		public Dictionary<string, int> IDFromName;

		// Token: 0x04008623 RID: 34339
		public Dictionary<int, string> NameFromID;

		// Token: 0x04008624 RID: 34340
		public Dictionary<int, Color> ColorFromID;
	}
}
