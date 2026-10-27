using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace CoverClippingTool
{
	// Token: 0x020016AA RID: 5802
	public class ShapeDataSet
	{
		// Token: 0x0600B54A RID: 46410 RVA: 0x0043C484 File Offset: 0x0043A684
		public ShapeDataSet(DataSource source)
		{
			this.Source = source;
		}

		// Token: 0x040087D2 RID: 34770
		public readonly DataSource Source;

		// Token: 0x040087D3 RID: 34771
		public readonly Dictionary<string, XElement> Data = new Dictionary<string, XElement>(StringComparer.OrdinalIgnoreCase);
	}
}
