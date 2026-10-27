using System;
using System.Collections.Generic;

// Token: 0x02000190 RID: 400
public class TileEntityFeatureData
{
	// Token: 0x06000C1E RID: 3102 RVA: 0x0004C46B File Offset: 0x0004A66B
	public TileEntityFeatureData(TileEntityCompositeData _parent, string _name, int _customOrder, Type _type, DynamicProperties _props)
	{
		this.Parent = _parent;
		this.Name = _name;
		this.NameHash = this.Name.GetStableHashCode();
		this.CustomOrder = _customOrder;
		this.Type = _type;
		this.Props = _props;
	}

	// Token: 0x06000C1F RID: 3103 RVA: 0x0004C4A9 File Offset: 0x0004A6A9
	public ITileEntityFeature InstantiateModule()
	{
		return ReflectionHelpers.Instantiate<ITileEntityFeature>(this.Type);
	}

	// Token: 0x04000AE4 RID: 2788
	public readonly TileEntityCompositeData Parent;

	// Token: 0x04000AE5 RID: 2789
	public readonly string Name;

	// Token: 0x04000AE6 RID: 2790
	public readonly int NameHash;

	// Token: 0x04000AE7 RID: 2791
	public readonly int CustomOrder;

	// Token: 0x04000AE8 RID: 2792
	public readonly Type Type;

	// Token: 0x04000AE9 RID: 2793
	public readonly DynamicProperties Props;

	// Token: 0x02000191 RID: 401
	public class FeatureDataSorterByName : IComparer<TileEntityFeatureData>
	{
		// Token: 0x06000C20 RID: 3104 RVA: 0x0000640C File Offset: 0x0000460C
		[PublicizedFrom(EAccessModifier.Private)]
		public FeatureDataSorterByName()
		{
		}

		// Token: 0x06000C21 RID: 3105 RVA: 0x0004C4B6 File Offset: 0x0004A6B6
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

		// Token: 0x04000AEA RID: 2794
		public static readonly TileEntityFeatureData.FeatureDataSorterByName Instance = new TileEntityFeatureData.FeatureDataSorterByName();
	}
}
