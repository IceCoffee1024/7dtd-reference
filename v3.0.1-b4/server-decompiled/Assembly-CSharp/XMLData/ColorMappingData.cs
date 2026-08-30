using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace XMLData
{
	// Token: 0x0200161D RID: 5661
	[Preserve]
	public class ColorMappingData : IXMLData
	{
		// Token: 0x170014C8 RID: 5320
		// (get) Token: 0x0600B18B RID: 45451 RVA: 0x00423287 File Offset: 0x00421487
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

		// Token: 0x0600B18C RID: 45452 RVA: 0x0042329D File Offset: 0x0042149D
		[PublicizedFrom(EAccessModifier.Private)]
		public ColorMappingData()
		{
			this.IDFromName = new Dictionary<string, int>();
			this.NameFromID = new Dictionary<int, string>();
			this.ColorFromID = new Dictionary<int, Color>();
		}

		// Token: 0x04008548 RID: 34120
		[PublicizedFrom(EAccessModifier.Private)]
		public static ColorMappingData instance;

		// Token: 0x04008549 RID: 34121
		public Dictionary<string, int> IDFromName;

		// Token: 0x0400854A RID: 34122
		public Dictionary<int, string> NameFromID;

		// Token: 0x0400854B RID: 34123
		public Dictionary<int, Color> ColorFromID;
	}
}
