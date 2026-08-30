using System;
using System.Collections.Generic;

// Token: 0x0200018C RID: 396
public class TileEntityCompositeData
{
	// Token: 0x06000C13 RID: 3091 RVA: 0x0004BE7B File Offset: 0x0004A07B
	[PublicizedFrom(EAccessModifier.Private)]
	public static void Init()
	{
		if (TileEntityCompositeData.knownFeatures.Count > 0)
		{
			return;
		}
		ReflectionHelpers.FindTypesImplementingBase(typeof(ITileEntityFeature), delegate(Type _type)
		{
			string name = _type.Name;
			Type type;
			if (TileEntityCompositeData.knownFeatures.TryGetValue(name, out type))
			{
				Log.Warning(string.Concat(new string[] { "Redeclaration of CompositeTileEntity feature ", name, ": ", type.FullName, " vs ", _type.FullName }));
				return;
			}
			if (_type.GetConstructor(Type.EmptyTypes) == null)
			{
				Log.Warning("CompositeTileEntity feature " + name + " has no parameterless constructor!");
				return;
			}
			TileEntityCompositeData.knownFeatures[name] = _type;
		}, false);
	}

	// Token: 0x06000C14 RID: 3092 RVA: 0x0004BEBA File Offset: 0x0004A0BA
	public static void Cleanup()
	{
		TileEntityCompositeData.FeaturesByBlock.Clear();
	}

	// Token: 0x06000C15 RID: 3093 RVA: 0x0004BEC8 File Offset: 0x0004A0C8
	public static TileEntityCompositeData ParseBlock(BlockCompositeTileEntity _block)
	{
		DynamicProperties dynamicProperties;
		if (!_block.Properties.Classes.TryGetValue("CompositeFeatures", out dynamicProperties))
		{
			throw new ArgumentException("Block " + _block.GetBlockName() + " uses class BlockCompositeTileEntity but has no CompositeFeatures property");
		}
		return new TileEntityCompositeData(_block, dynamicProperties);
	}

	// Token: 0x06000C16 RID: 3094 RVA: 0x0004BF10 File Offset: 0x0004A110
	[PublicizedFrom(EAccessModifier.Private)]
	public TileEntityCompositeData(BlockCompositeTileEntity _block, DynamicProperties _compositeProps)
	{
		TileEntityCompositeData.Init();
		this.Block = _block;
		this.CompositeProps = _compositeProps;
		int num = 0;
		foreach (KeyValuePair<string, DynamicProperties> keyValuePair in _compositeProps.Classes)
		{
			string text;
			DynamicProperties dynamicProperties;
			keyValuePair.Deconstruct(out text, out dynamicProperties);
			string text2 = text;
			DynamicProperties dynamicProperties2 = dynamicProperties;
			Type type;
			if (!TileEntityCompositeData.knownFeatures.TryGetValue(text2, out type))
			{
				throw new ArgumentException(string.Concat(new string[]
				{
					"Block \"",
					_block.GetBlockName(),
					"\": CompositeFeature class \"",
					text2,
					"\" not found!"
				}));
			}
			TileEntityFeatureData tileEntityFeatureData = new TileEntityFeatureData(this, text2, num, type, dynamicProperties2);
			this.Features.Add(tileEntityFeatureData);
			num++;
		}
		if (this.Features.Count == 0)
		{
			throw new ArgumentException("Block \"" + _block.GetBlockName() + "\": No CompositeFeatures specified!");
		}
		this.Features.Sort(TileEntityFeatureData.FeatureDataSorterByName.Instance);
		for (int i = 0; i < this.Features.Count; i++)
		{
			TileEntityFeatureData tileEntityFeatureData2 = this.Features[i];
			this.featureIndexByName[tileEntityFeatureData2.Name.AsMemory()] = i;
			int stableHashCode = tileEntityFeatureData2.Name.GetStableHashCode();
			int num2;
			if (this.featureIndexByHash.TryGetValue(stableHashCode, out num2))
			{
				TileEntityFeatureData tileEntityFeatureData3 = this.Features[num2];
				Log.Error(string.Format("Composite block \"{0}\": Feature hash collision 0x{1:X8} for \"{2}\" vs \"{3}\". ", new object[]
				{
					_block.GetBlockName(),
					stableHashCode,
					tileEntityFeatureData3.Name,
					tileEntityFeatureData2.Name
				}) + "This hash will be ambiguous during load.");
			}
			else
			{
				this.featureIndexByHash.Add(stableHashCode, i);
			}
		}
		TileEntityCompositeData.FeaturesByBlock[_block] = this;
	}

	// Token: 0x06000C17 RID: 3095 RVA: 0x0004C134 File Offset: 0x0004A334
	public int GetFeatureIndex(ReadOnlyMemory<char> _featureName)
	{
		return this.featureIndexByName.GetValueOrDefault(_featureName, -1);
	}

	// Token: 0x06000C18 RID: 3096 RVA: 0x0004C143 File Offset: 0x0004A343
	public int GetFeatureIndex(int _featureNameHash)
	{
		return this.featureIndexByHash.GetValueOrDefault(_featureNameHash, -1);
	}

	// Token: 0x06000C19 RID: 3097 RVA: 0x0004C154 File Offset: 0x0004A354
	public int GetFeatureIndex<T>() where T : class
	{
		Type typeFromHandle = typeof(T);
		int num;
		if (this.featureIndexByType.TryGetValue(typeFromHandle, out num))
		{
			return num;
		}
		for (int i = 0; i < this.Features.Count; i++)
		{
			TileEntityFeatureData tileEntityFeatureData = this.Features[i];
			if (typeFromHandle.IsAssignableFrom(tileEntityFeatureData.Type))
			{
				this.featureIndexByType[typeFromHandle] = i;
				return i;
			}
		}
		this.featureIndexByType[typeFromHandle] = -1;
		return -1;
	}

	// Token: 0x06000C1A RID: 3098 RVA: 0x0004C1CC File Offset: 0x0004A3CC
	public bool HasFeature(ReadOnlyMemory<char> _featureName)
	{
		return this.GetFeatureIndex(_featureName) >= 0;
	}

	// Token: 0x06000C1B RID: 3099 RVA: 0x0004C1DB File Offset: 0x0004A3DB
	public bool HasFeature(int _featureNameHash)
	{
		return this.GetFeatureIndex(_featureNameHash) >= 0;
	}

	// Token: 0x06000C1C RID: 3100 RVA: 0x0004C1EA File Offset: 0x0004A3EA
	public bool HasFeature<T>() where T : class
	{
		return this.GetFeatureIndex<T>() >= 0;
	}

	// Token: 0x06000C1D RID: 3101 RVA: 0x0004C1F8 File Offset: 0x0004A3F8
	public bool TryGetFeatureData<T>(out TileEntityFeatureData featureData) where T : class, ITileEntityFeature
	{
		int featureIndex = this.GetFeatureIndex<T>();
		if (featureIndex >= 0)
		{
			featureData = this.Features[featureIndex];
			return true;
		}
		featureData = null;
		return false;
	}

	// Token: 0x06000C1E RID: 3102 RVA: 0x0004C224 File Offset: 0x0004A424
	public void PrintConfig()
	{
		Log.Out("Composite block: " + this.Block.GetBlockName() + ":");
		foreach (KeyValuePair<string, string> keyValuePair in this.CompositeProps.Values)
		{
			string text;
			string text2;
			keyValuePair.Deconstruct(out text, out text2);
			string text3 = text;
			string text4 = text2;
			Log.Out("    " + text3 + "=" + text4);
		}
		foreach (TileEntityFeatureData tileEntityFeatureData in this.Features)
		{
			Log.Out(string.Format("  Feature: {0} (class {1} in assembly ({2})), manual order = {3}:", new object[]
			{
				tileEntityFeatureData.Name,
				tileEntityFeatureData.Type.FullName,
				tileEntityFeatureData.Type.Assembly.FullName,
				tileEntityFeatureData.CustomOrder
			}));
			foreach (KeyValuePair<string, string> keyValuePair in tileEntityFeatureData.Props.Values)
			{
				string text;
				string text2;
				keyValuePair.Deconstruct(out text2, out text);
				string text5 = text2;
				string text6 = text;
				Log.Out("    " + text5 + "=" + text6);
			}
		}
	}

	// Token: 0x04000AE0 RID: 2784
	public static readonly Dictionary<BlockCompositeTileEntity, TileEntityCompositeData> FeaturesByBlock = new Dictionary<BlockCompositeTileEntity, TileEntityCompositeData>();

	// Token: 0x04000AE1 RID: 2785
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Dictionary<string, Type> knownFeatures = new Dictionary<string, Type>();

	// Token: 0x04000AE2 RID: 2786
	public readonly BlockCompositeTileEntity Block;

	// Token: 0x04000AE3 RID: 2787
	public readonly DynamicProperties CompositeProps;

	// Token: 0x04000AE4 RID: 2788
	public readonly List<TileEntityFeatureData> Features = new List<TileEntityFeatureData>();

	// Token: 0x04000AE5 RID: 2789
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<Type, int> featureIndexByType = new Dictionary<Type, int>();

	// Token: 0x04000AE6 RID: 2790
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<ReadOnlyMemory<char>, int> featureIndexByName = new Dictionary<ReadOnlyMemory<char>, int>(TileEntityCompositeData.MemStringEqualityComparer.Instance);

	// Token: 0x04000AE7 RID: 2791
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<int, int> featureIndexByHash = new Dictionary<int, int>();

	// Token: 0x0200018D RID: 397
	public class MemStringEqualityComparer : IEqualityComparer<ReadOnlyMemory<char>>
	{
		// Token: 0x06000C20 RID: 3104 RVA: 0x0000640C File Offset: 0x0000460C
		[PublicizedFrom(EAccessModifier.Private)]
		public MemStringEqualityComparer()
		{
		}

		// Token: 0x06000C21 RID: 3105 RVA: 0x0004C3D2 File Offset: 0x0004A5D2
		public int GetHashCode(ReadOnlyMemory<char> _obj)
		{
			return _obj.Span.GetStableHashCode();
		}

		// Token: 0x06000C22 RID: 3106 RVA: 0x0004C3E0 File Offset: 0x0004A5E0
		public bool Equals(ReadOnlyMemory<char> _x, ReadOnlyMemory<char> _y)
		{
			return _x.Span.Equals(_y.Span, StringComparison.Ordinal);
		}

		// Token: 0x04000AE8 RID: 2792
		public static readonly TileEntityCompositeData.MemStringEqualityComparer Instance = new TileEntityCompositeData.MemStringEqualityComparer();
	}
}
