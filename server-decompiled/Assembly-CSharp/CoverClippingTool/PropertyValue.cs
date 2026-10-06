using System;
using System.Xml.Linq;

namespace CoverClippingTool
{
	// Token: 0x02001695 RID: 5781
	public struct PropertyValue
	{
		// Token: 0x0600B4EC RID: 46316 RVA: 0x00439E83 File Offset: 0x00438083
		public PropertyValue(XElement element, string value)
		{
			this.Element = element;
			this.Value = value;
		}

		// Token: 0x0600B4ED RID: 46317 RVA: 0x00439E93 File Offset: 0x00438093
		public PropertyValue Clone()
		{
			return new PropertyValue(this.Element, this.Value);
		}

		// Token: 0x04008789 RID: 34697
		public readonly string Value;

		// Token: 0x0400878A RID: 34698
		public readonly XElement Element;
	}
}
