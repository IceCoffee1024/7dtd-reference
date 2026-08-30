using System;
using System.Collections.Generic;

namespace CoverClippingTool
{
	// Token: 0x0200165D RID: 5725
	public class ShapeData
	{
		// Token: 0x0600B361 RID: 45921 RVA: 0x00430F64 File Offset: 0x0042F164
		public ShapeData(string name)
		{
			this.Name = name;
		}

		// Token: 0x0600B362 RID: 45922 RVA: 0x00430F94 File Offset: 0x0042F194
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

		// Token: 0x0400865E RID: 34398
		public readonly string Name;

		// Token: 0x0400865F RID: 34399
		public ShapeData Extends;

		// Token: 0x04008660 RID: 34400
		public Dictionary<string, PropertyValue> Properties = new Dictionary<string, PropertyValue>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x04008661 RID: 34401
		public Dictionary<string, ArrayValue> Arrays = new Dictionary<string, ArrayValue>(StringComparer.OrdinalIgnoreCase);
	}
}
