using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace CoverClippingTool
{
	// Token: 0x02001698 RID: 5784
	public class ShapeDataSet
	{
		// Token: 0x0600B4F2 RID: 46322 RVA: 0x0043A080 File Offset: 0x00438280
		public ShapeDataSet(DataSource source)
		{
			this.Source = source;
		}

		// Token: 0x04008791 RID: 34705
		public readonly DataSource Source;

		// Token: 0x04008792 RID: 34706
		public readonly Dictionary<string, XElement> Data = new Dictionary<string, XElement>(StringComparer.OrdinalIgnoreCase);
	}
}
