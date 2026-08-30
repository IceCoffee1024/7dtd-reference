using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace CoverClippingTool
{
	// Token: 0x02001679 RID: 5753
	public class ShapeDataSet
	{
		// Token: 0x0600B460 RID: 46176 RVA: 0x004363A4 File Offset: 0x004345A4
		public ShapeDataSet(DataSource source)
		{
			this.Source = source;
		}

		// Token: 0x0400873B RID: 34619
		public readonly DataSource Source;

		// Token: 0x0400873C RID: 34620
		public readonly Dictionary<string, XElement> Data = new Dictionary<string, XElement>(StringComparer.OrdinalIgnoreCase);
	}
}
