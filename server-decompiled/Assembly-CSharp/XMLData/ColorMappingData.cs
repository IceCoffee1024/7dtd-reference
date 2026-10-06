using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace XMLData
{
	// Token: 0x02001657 RID: 5719
	[Preserve]
	public class ColorMappingData : IXMLData
	{
		// Token: 0x17001544 RID: 5444
		// (get) Token: 0x0600B31A RID: 45850 RVA: 0x0042C2A3 File Offset: 0x0042A4A3
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

		// Token: 0x0600B31B RID: 45851 RVA: 0x0042C2B9 File Offset: 0x0042A4B9
		[PublicizedFrom(EAccessModifier.Private)]
		public ColorMappingData()
		{
			this.IDFromName = new Dictionary<string, int>();
			this.NameFromID = new Dictionary<int, string>();
			this.ColorFromID = new Dictionary<int, Color>();
		}

		// Token: 0x04008677 RID: 34423
		[PublicizedFrom(EAccessModifier.Private)]
		public static ColorMappingData instance;

		// Token: 0x04008678 RID: 34424
		public Dictionary<string, int> IDFromName;

		// Token: 0x04008679 RID: 34425
		public Dictionary<int, string> NameFromID;

		// Token: 0x0400867A RID: 34426
		public Dictionary<int, Color> ColorFromID;
	}
}
