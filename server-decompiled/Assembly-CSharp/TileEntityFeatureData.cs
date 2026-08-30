using System;
using System.Collections.Generic;

// Token: 0x0200018F RID: 399
public class TileEntityFeatureData
{
	// Token: 0x06000C27 RID: 3111 RVA: 0x0004C4A7 File Offset: 0x0004A6A7
	public TileEntityFeatureData(TileEntityCompositeData _parent, string _name, int _customOrder, Type _type, DynamicProperties _props)
	{
		this.Parent = _parent;
		this.Name = _name;
		this.NameHash = this.Name.GetStableHashCode();
		this.CustomOrder = _customOrder;
		this.Type = _type;
		this.Props = _props;
	}

	// Token: 0x06000C28 RID: 3112 RVA: 0x0004C4E5 File Offset: 0x0004A6E5
	public ITileEntityFeature InstantiateModule()
	{
		return ReflectionHelpers.Instantiate<ITileEntityFeature>(this.Type);
	}

	// Token: 0x04000AEB RID: 2795
	public readonly TileEntityCompositeData Parent;

	// Token: 0x04000AEC RID: 2796
	public readonly string Name;

	// Token: 0x04000AED RID: 2797
	public readonly int NameHash;

	// Token: 0x04000AEE RID: 2798
	public readonly int CustomOrder;

	// Token: 0x04000AEF RID: 2799
	public readonly Type Type;

	// Token: 0x04000AF0 RID: 2800
	public readonly DynamicProperties Props;

	// Token: 0x02000190 RID: 400
	public class FeatureDataSorterByName : IComparer<TileEntityFeatureData>
	{
		// Token: 0x06000C29 RID: 3113 RVA: 0x0000640C File Offset: 0x0000460C
		[PublicizedFrom(EAccessModifier.Private)]
		public FeatureDataSorterByName()
		{
		}

		// Token: 0x06000C2A RID: 3114 RVA: 0x0004C4F2 File Offset: 0x0004A6F2
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

		// Token: 0x04000AF1 RID: 2801
		public static readonly TileEntityFeatureData.FeatureDataSorterByName Instance = new TileEntityFeatureData.FeatureDataSorterByName();
	}
}
