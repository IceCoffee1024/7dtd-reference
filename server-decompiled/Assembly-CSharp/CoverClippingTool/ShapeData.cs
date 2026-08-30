using System;
using System.Collections.Generic;

namespace CoverClippingTool
{
	// Token: 0x02001678 RID: 5752
	public class ShapeData
	{
		// Token: 0x0600B45E RID: 46174 RVA: 0x004362A4 File Offset: 0x004344A4
		public ShapeData(string name)
		{
			this.Name = name;
		}

		// Token: 0x0600B45F RID: 46175 RVA: 0x004362D4 File Offset: 0x004344D4
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

		// Token: 0x04008737 RID: 34615
		public readonly string Name;

		// Token: 0x04008738 RID: 34616
		public ShapeData Extends;

		// Token: 0x04008739 RID: 34617
		public Dictionary<string, PropertyValue> Properties = new Dictionary<string, PropertyValue>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x0400873A RID: 34618
		public Dictionary<string, ArrayValue> Arrays = new Dictionary<string, ArrayValue>(StringComparer.OrdinalIgnoreCase);
	}
}
