using System;
using System.Collections.Generic;

// Token: 0x0200018D RID: 397
public class TileEntityFeatureData
{
	// Token: 0x06000C1C RID: 3100 RVA: 0x0004C08F File Offset: 0x0004A28F
	public TileEntityFeatureData(TileEntityCompositeData _parent, string _name, int _customOrder, Type _type, DynamicProperties _props)
	{
		this.Parent = _parent;
		this.Name = _name;
		this.NameHash = this.Name.GetStableHashCode();
		this.CustomOrder = _customOrder;
		this.Type = _type;
		this.Props = _props;
	}

	// Token: 0x06000C1D RID: 3101 RVA: 0x0004C0CD File Offset: 0x0004A2CD
	public ITileEntityFeature InstantiateModule()
	{
		return ReflectionHelpers.Instantiate<ITileEntityFeature>(this.Type);
	}

	// Token: 0x04000ADE RID: 2782
	public readonly TileEntityCompositeData Parent;

	// Token: 0x04000ADF RID: 2783
	public readonly string Name;

	// Token: 0x04000AE0 RID: 2784
	public readonly int NameHash;

	// Token: 0x04000AE1 RID: 2785
	public readonly int CustomOrder;

	// Token: 0x04000AE2 RID: 2786
	public readonly Type Type;

	// Token: 0x04000AE3 RID: 2787
	public readonly DynamicProperties Props;

	// Token: 0x0200018E RID: 398
	public class FeatureDataSorterByName : IComparer<TileEntityFeatureData>
	{
		// Token: 0x06000C1E RID: 3102 RVA: 0x0000640C File Offset: 0x0000460C
		[PublicizedFrom(EAccessModifier.Private)]
		public FeatureDataSorterByName()
		{
		}

		// Token: 0x06000C1F RID: 3103 RVA: 0x0004C0DA File Offset: 0x0004A2DA
		public int Compare(TileEntityFeatureData _x, TileEntityFeatureData _y)
		{
			if (_x == _y)
			{
				return 0;
			}
			if (_y == null)
			{
				return 1;
			}
			if (_x == null)
			{
				return -1;
			}
			return string.Compare(_x.Name, _y.Name, StringComparison.Ordinal);
		}

		// Token: 0x04000AE4 RID: 2788
		public static readonly TileEntityFeatureData.FeatureDataSorterByName Instance = new TileEntityFeatureData.FeatureDataSorterByName();
	}
}
