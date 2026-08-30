using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace CoverClippingTool
{
	// Token: 0x02001677 RID: 5751
	public class ArrayValue
	{
		// Token: 0x0600B45C RID: 46172 RVA: 0x004361CA File Offset: 0x004343CA
		public ArrayValue(XElement element, List<Dictionary<string, PropertyValue>> items)
		{
			this.Element = element;
			this.Items = items;
		}

		// Token: 0x0600B45D RID: 46173 RVA: 0x004361E0 File Offset: 0x004343E0
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

		// Token: 0x04008735 RID: 34613
		public readonly List<Dictionary<string, PropertyValue>> Items;

		// Token: 0x04008736 RID: 34614
		public readonly XElement Element;
	}
}
