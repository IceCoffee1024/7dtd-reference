using System;
using System.Collections.Generic;

namespace CoverClippingTool
{
	// Token: 0x020016A9 RID: 5801
	public class ShapeData
	{
		// Token: 0x0600B548 RID: 46408 RVA: 0x0043C384 File Offset: 0x0043A584
		public ShapeData(string name)
		{
			this.Name = name;
		}

		// Token: 0x0600B549 RID: 46409 RVA: 0x0043C3B4 File Offset: 0x0043A5B4
		public ShapeData Clone(string name)
		{
			ShapeData shapeData = new ShapeData(name);
			foreach (KeyValuePair<string, PropertyValue> keyValuePair in this.Properties)
			{
				shapeData.Properties[keyValuePair.Key] = keyValuePair.Value.Clone();
			}
			foreach (KeyValuePair<string, ArrayValue> keyValuePair2 in this.Arrays)
			{
				shapeData.Arrays[keyValuePair2.Key] = keyValuePair2.Value.Clone();
			}
			return shapeData;
		}

		// Token: 0x040087CE RID: 34766
		public readonly string Name;

		// Token: 0x040087CF RID: 34767
		public ShapeData Extends;

		// Token: 0x040087D0 RID: 34768
		public Dictionary<string, PropertyValue> Properties = new Dictionary<string, PropertyValue>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x040087D1 RID: 34769
		public Dictionary<string, ArrayValue> Arrays = new Dictionary<string, ArrayValue>(StringComparer.OrdinalIgnoreCase);
	}
}
