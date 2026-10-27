using System;
using System.Xml.Linq;

namespace CoverClippingTool
{
	// Token: 0x020016A7 RID: 5799
	public struct PropertyValue
	{
		// Token: 0x0600B544 RID: 46404 RVA: 0x0043C287 File Offset: 0x0043A487
		public PropertyValue(XElement element, string value)
		{
			this.Element = element;
			this.Value = value;
		}

		// Token: 0x0600B545 RID: 46405 RVA: 0x0043C297 File Offset: 0x0043A497
		public PropertyValue Clone()
		{
			return new PropertyValue(this.Element, this.Value);
		}

		// Token: 0x040087CA RID: 34762
		public readonly string Value;

		// Token: 0x040087CB RID: 34763
		public readonly XElement Element;
	}
}
