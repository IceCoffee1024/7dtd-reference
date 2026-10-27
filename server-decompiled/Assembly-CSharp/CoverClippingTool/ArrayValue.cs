using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace CoverClippingTool
{
	// Token: 0x020016A8 RID: 5800
	public class ArrayValue
	{
		// Token: 0x0600B546 RID: 46406 RVA: 0x0043C2AA File Offset: 0x0043A4AA
		public ArrayValue(XElement element, List<Dictionary<string, PropertyValue>> items)
		{
			this.Element = element;
			this.Items = items;
		}

		// Token: 0x0600B547 RID: 46407 RVA: 0x0043C2C0 File Offset: 0x0043A4C0
		public ArrayValue Clone()
		{
			ArrayValue arrayValue = new ArrayValue(this.Element, new List<Dictionary<string, PropertyValue>>());
			foreach (Dictionary<string, PropertyValue> dictionary in this.Items)
			{
				Dictionary<string, PropertyValue> dictionary2 = new Dictionary<string, PropertyValue>();
				foreach (KeyValuePair<string, PropertyValue> keyValuePair in dictionary)
				{
					dictionary2[keyValuePair.Key] = keyValuePair.Value.Clone();
				}
				arrayValue.Items.Add(dictionary2);
			}
			return arrayValue;
		}

		// Token: 0x040087CC RID: 34764
		public readonly List<Dictionary<string, PropertyValue>> Items;

		// Token: 0x040087CD RID: 34765
		public readonly XElement Element;
	}
}
