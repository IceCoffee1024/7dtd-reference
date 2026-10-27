using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace XMLData
{
	// Token: 0x02001669 RID: 5737
	[Preserve]
	public class ColorMappingData : IXMLData
	{
		// Token: 0x17001550 RID: 5456
		// (get) Token: 0x0600B372 RID: 45938 RVA: 0x0042E6A7 File Offset: 0x0042C8A7
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

		// Token: 0x0600B373 RID: 45939 RVA: 0x0042E6BD File Offset: 0x0042C8BD
		[PublicizedFrom(EAccessModifier.Private)]
		public ColorMappingData()
		{
			this.IDFromName = new Dictionary<string, int>();
			this.NameFromID = new Dictionary<int, string>();
			this.ColorFromID = new Dictionary<int, Color>();
		}

		// Token: 0x040086B8 RID: 34488
		[PublicizedFrom(EAccessModifier.Private)]
		public static ColorMappingData instance;

		// Token: 0x040086B9 RID: 34489
		public Dictionary<string, int> IDFromName;

		// Token: 0x040086BA RID: 34490
		public Dictionary<int, string> NameFromID;

		// Token: 0x040086BB RID: 34491
		public Dictionary<int, Color> ColorFromID;
	}
}
