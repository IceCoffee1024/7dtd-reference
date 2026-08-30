using System;
using System.Xml.Linq;

namespace CoverClippingTool
{
	// Token: 0x02001676 RID: 5750
	public struct PropertyValue
	{
		// Token: 0x0600B45A RID: 46170 RVA: 0x004361A7 File Offset: 0x004343A7
		public PropertyValue(XElement element, string value)
		{
			this.Element = element;
			this.Value = value;
		}

		// Token: 0x0600B45B RID: 46171 RVA: 0x004361B7 File Offset: 0x004343B7
		public PropertyValue Clone()
		{
			return new PropertyValue(this.Element, this.Value);
		}

		// Token: 0x04008733 RID: 34611
		public readonly string Value;

		// Token: 0x04008734 RID: 34612
		public readonly XElement Element;
	}
}
