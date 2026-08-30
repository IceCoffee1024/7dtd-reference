using System;
using System.Collections.Generic;

// Token: 0x0200018A RID: 394
public class TileEntityCompositeData
{
	// Token: 0x06000C08 RID: 3080 RVA: 0x0004BA63 File Offset: 0x00049C63
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

	// Token: 0x06000C09 RID: 3081 RVA: 0x0004BAA2 File Offset: 0x00049CA2
	public static void Cleanup()
	{
		TileEntityCompositeData.FeaturesByBlock.Clear();
	}

	// Token: 0x06000C0A RID: 3082 RVA: 0x0004BAB0 File Offset: 0x00049CB0
	public static TileEntityCompositeData ParseBlock(BlockCompositeTileEntity _block)
	{
		DynamicProperties dynamicProperties;
		if (!_block.Properties.Classes.TryGetValue("CompositeFeatures", out dynamicProperties))
		{
			throw new ArgumentException("Block " + _block.GetBlockName() + " uses class BlockCompositeTileEntity but has no CompositeFeatures property");
		}
		return new TileEntityCompositeData(_block, dynamicProperties);
	}

	// Token: 0x06000C0B RID: 3083 RVA: 0x0004BAF8 File Offset: 0x00049CF8
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

	// Token: 0x06000C0C RID: 3084 RVA: 0x0004BD1C File Offset: 0x00049F1C
	public int GetFeatureIndex(ReadOnlyMemory<char> _featureName)
	{
		return this.featureIndexByName.GetValueOrDefault(_featureName, -1);
	}

	// Token: 0x06000C0D RID: 3085 RVA: 0x0004BD2B File Offset: 0x00049F2B
	public int GetFeatureIndex(int _featureNameHash)
	{
		return this.featureIndexByHash.GetValueOrDefault(_featureNameHash, -1);
	}

	// Token: 0x06000C0E RID: 3086 RVA: 0x0004BD3C File Offset: 0x00049F3C
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

	// Token: 0x06000C0F RID: 3087 RVA: 0x0004BDB4 File Offset: 0x00049FB4
	public bool HasFeature(ReadOnlyMemory<char> _featureName)
	{
		return this.GetFeatureIndex(_featureName) >= 0;
	}

	// Token: 0x06000C10 RID: 3088 RVA: 0x0004BDC3 File Offset: 0x00049FC3
	public bool HasFeature(int _featureNameHash)
	{
		return this.GetFeatureIndex(_featureNameHash) >= 0;
	}

	// Token: 0x06000C11 RID: 3089 RVA: 0x0004BDD2 File Offset: 0x00049FD2
	public bool HasFeature<T>() where T : class
	{
		return this.GetFeatureIndex<T>() >= 0;
	}

	// Token: 0x06000C12 RID: 3090 RVA: 0x0004BDE0 File Offset: 0x00049FE0
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

	// Token: 0x06000C13 RID: 3091 RVA: 0x0004BE0C File Offset: 0x0004A00C
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

	// Token: 0x04000AD3 RID: 2771
	public static readonly Dictionary<BlockCompositeTileEntity, TileEntityCompositeData> FeaturesByBlock = new Dictionary<BlockCompositeTileEntity, TileEntityCompositeData>();

	// Token: 0x04000AD4 RID: 2772
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Dictionary<string, Type> knownFeatures = new Dictionary<string, Type>();

	// Token: 0x04000AD5 RID: 2773
	public readonly BlockCompositeTileEntity Block;

	// Token: 0x04000AD6 RID: 2774
	public readonly DynamicProperties CompositeProps;

	// Token: 0x04000AD7 RID: 2775
	public readonly List<TileEntityFeatureData> Features = new List<TileEntityFeatureData>();

	// Token: 0x04000AD8 RID: 2776
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<Type, int> featureIndexByType = new Dictionary<Type, int>();

	// Token: 0x04000AD9 RID: 2777
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<ReadOnlyMemory<char>, int> featureIndexByName = new Dictionary<ReadOnlyMemory<char>, int>(TileEntityCompositeData.MemStringEqualityComparer.Instance);

	// Token: 0x04000ADA RID: 2778
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<int, int> featureIndexByHash = new Dictionary<int, int>();

	// Token: 0x0200018B RID: 395
	public class MemStringEqualityComparer : IEqualityComparer<ReadOnlyMemory<char>>
	{
		// Token: 0x06000C15 RID: 3093 RVA: 0x0000640C File Offset: 0x0000460C
		[PublicizedFrom(EAccessModifier.Private)]
		public MemStringEqualityComparer()
		{
		}

		// Token: 0x06000C16 RID: 3094 RVA: 0x0004BFBA File Offset: 0x0004A1BA
		public int GetHashCode(ReadOnlyMemory<char> _obj)
		{
			return _obj.Span.GetStableHashCode();
		}

		// Token: 0x06000C17 RID: 3095 RVA: 0x0004BFC8 File Offset: 0x0004A1C8
		public bool Equals(ReadOnlyMemory<char> _x, ReadOnlyMemory<char> _y)
		{
			return _x.Span.Equals(_y.Span, StringComparison.Ordinal);
		}

		// Token: 0x04000ADB RID: 2779
		public static readonly TileEntityCompositeData.MemStringEqualityComparer Instance = new TileEntityCompositeData.MemStringEqualityComparer();
	}
}
