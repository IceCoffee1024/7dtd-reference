using System;
using System.Collections.Generic;

namespace CoverClippingTool
{
	// Token: 0x02001697 RID: 5783
	public class ShapeData
	{
		// Token: 0x0600B4F0 RID: 46320 RVA: 0x00439F80 File Offset: 0x00438180
		public ShapeData(string name)
		{
			this.Name = name;
		}

		// Token: 0x0600B4F1 RID: 46321 RVA: 0x00439FB0 File Offset: 0x004381B0
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

		// Token: 0x0400878D RID: 34701
		public readonly string Name;

		// Token: 0x0400878E RID: 34702
		public ShapeData Extends;

		// Token: 0x0400878F RID: 34703
		public Dictionary<string, PropertyValue> Properties = new Dictionary<string, PropertyValue>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x04008790 RID: 34704
		public Dictionary<string, ArrayValue> Arrays = new Dictionary<string, ArrayValue>(StringComparer.OrdinalIgnoreCase);
	}
}
