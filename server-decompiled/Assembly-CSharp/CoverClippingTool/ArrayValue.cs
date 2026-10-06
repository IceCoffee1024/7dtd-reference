using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace CoverClippingTool
{
	// Token: 0x02001696 RID: 5782
	public class ArrayValue
	{
		// Token: 0x0600B4EE RID: 46318 RVA: 0x00439EA6 File Offset: 0x004380A6
		public ArrayValue(XElement element, List<Dictionary<string, PropertyValue>> items)
		{
			this.Element = element;
			this.Items = items;
		}

		// Token: 0x0600B4EF RID: 46319 RVA: 0x00439EBC File Offset: 0x004380BC
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

		// Token: 0x0400878B RID: 34699
		public readonly List<Dictionary<string, PropertyValue>> Items;

		// Token: 0x0400878C RID: 34700
		public readonly XElement Element;
	}
}
