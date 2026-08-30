using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace CoverClippingTool
{
	// Token: 0x0200165C RID: 5724
	public class ArrayValue
	{
		// Token: 0x0600B35F RID: 45919 RVA: 0x00430E8A File Offset: 0x0042F08A
		public ArrayValue(XElement element, List<Dictionary<string, PropertyValue>> items)
		{
			this.Element = element;
			this.Items = items;
		}

		// Token: 0x0600B360 RID: 45920 RVA: 0x00430EA0 File Offset: 0x0042F0A0
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

		// Token: 0x0400865C RID: 34396
		public readonly List<Dictionary<string, PropertyValue>> Items;

		// Token: 0x0400865D RID: 34397
		public readonly XElement Element;
	}
}
