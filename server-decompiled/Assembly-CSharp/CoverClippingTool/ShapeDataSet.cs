using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace CoverClippingTool
{
	// Token: 0x0200165E RID: 5726
	public class ShapeDataSet
	{
		// Token: 0x0600B363 RID: 45923 RVA: 0x00431064 File Offset: 0x0042F264
		public ShapeDataSet(DataSource source)
		{
			this.Source = source;
		}

		// Token: 0x04008662 RID: 34402
		public readonly DataSource Source;

		// Token: 0x04008663 RID: 34403
		public readonly Dictionary<string, XElement> Data = new Dictionary<string, XElement>(StringComparer.OrdinalIgnoreCase);
	}
}
