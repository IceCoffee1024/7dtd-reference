using System;
using System.Xml.Linq;

namespace CoverClippingTool
{
	// Token: 0x0200165B RID: 5723
	public struct PropertyValue
	{
		// Token: 0x0600B35D RID: 45917 RVA: 0x00430E67 File Offset: 0x0042F067
		public PropertyValue(XElement element, string value)
		{
			this.Element = element;
			this.Value = value;
		}

		// Token: 0x0600B35E RID: 45918 RVA: 0x00430E77 File Offset: 0x0042F077
		public PropertyValue Clone()
		{
			return new PropertyValue(this.Element, this.Value);
		}

		// Token: 0x0400865A RID: 34394
		public readonly string Value;

		// Token: 0x0400865B RID: 34395
		public readonly XElement Element;
	}
}
