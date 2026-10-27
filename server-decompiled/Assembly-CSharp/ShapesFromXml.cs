using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;

// Token: 0x02000D47 RID: 3399
public static class ShapesFromXml
{
	// Token: 0x17000AED RID: 2797
	// (get) Token: 0x060066D9 RID: 26329 RVA: 0x0028C318 File Offset: 0x0028A518
	public static ShapesFromXml.EDebugLevel DebugLevel
	{
		get
		{
			return ShapesFromXml.debug;
		}
	}

	// Token: 0x060066DA RID: 26330 RVA: 0x0028C320 File Offset: 0x0028A520
	[PublicizedFrom(EAccessModifier.Private)]
	static ShapesFromXml()
	{
		string launchArgument = GameUtils.GetLaunchArgument("debugshapes");
		if (launchArgument != null)
		{
			if (launchArgument == "verbose")
			{
				ShapesFromXml.debug = ShapesFromXml.EDebugLevel.Verbose;
			}
			else
			{
				ShapesFromXml.debug = ShapesFromXml.EDebugLevel.Normal;
			}
		}
		ShapesFromXml.TextureLabelsByChannel[0] = new ShapesFromXml.TextureLabels(string.Empty);
		for (int i = 1; i < 1; i++)
		{
			ShapesFromXml.TextureLabelsByChannel[i] = new ShapesFromXml.TextureLabels((i + 1).ToString());
		}
	}

	// Token: 0x060066DB RID: 26331 RVA: 0x0028C3B7 File Offset: 0x0028A5B7
	public static IEnumerator LoadShapes(XmlFile _xmlFile)
	{
		ShapesFromXml.shapes = new CaseInsensitiveStringDictionary<XElement>();
		ShapesFromXml.shapeCategories.Clear();
		BlockShapeNew.Cleanup();
		XElement root = _xmlFile.XmlDoc.Root;
		if (root == null || !root.HasElements)
		{
			throw new Exception("No element <shapes> found!");
		}
		int num = 1;
		foreach (XElement xelement in root.Elements("shape"))
		{
			string attribute = xelement.GetAttribute(XNames.name);
			ShapesFromXml.SetProperty(xelement, Block.PropCreativeSort2, XNames.value, num++.ToString("0000"));
			ShapesFromXml.shapes.Add(attribute, xelement);
		}
		using (IEnumerator<XElement> enumerator = root.Elements("categories").GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				XElement xelement2 = enumerator.Current;
				ShapesFromXml.ParseCategories(xelement2);
			}
			yield break;
		}
		yield break;
	}

	// Token: 0x060066DC RID: 26332 RVA: 0x0028C3C6 File Offset: 0x0028A5C6
	public static void Cleanup()
	{
		ShapesFromXml.shapes.Clear();
		ShapesFromXml.shapes = null;
	}

	// Token: 0x060066DD RID: 26333 RVA: 0x0028C3D8 File Offset: 0x0028A5D8
	[PublicizedFrom(EAccessModifier.Private)]
	public static void ParseCategories(XElement _parentElement)
	{
		foreach (XElement xelement in _parentElement.Elements(XNames.category))
		{
			string attribute = xelement.GetAttribute(XNames.name);
			string attribute2 = xelement.GetAttribute(XNames.icon);
			int num = int.Parse(xelement.GetAttribute(XNames.order));
			ShapesFromXml.shapeCategories.Add(attribute, new ShapesFromXml.ShapeCategory(attribute, attribute2, num));
		}
	}

	// Token: 0x060066DE RID: 26334 RVA: 0x0028C460 File Offset: 0x0028A660
	public static IEnumerator CreateShapeVariants(bool _bEditMode, XElement _elementBlock)
	{
		string blockBaseName = _elementBlock.GetAttribute(XNames.name);
		string allowedShapes = _elementBlock.GetAttribute(XNames.shapes);
		XAttribute xattribute = _elementBlock.Attribute(XNames.hideHelperInCreative);
		bool hideHelperInCreative;
		StringParsers.TryParseBool(((xattribute != null) ? xattribute.Value : null) ?? "false", out hideHelperInCreative);
		if (ShapesFromXml.debug != ShapesFromXml.EDebugLevel.Off)
		{
			Log.Out("Creating block+shape combinations for base block " + blockBaseName);
			Console.WriteLine();
			Console.WriteLine();
			Console.WriteLine();
			Console.WriteLine();
		}
		List<string> shapeNames = new List<string>();
		bool isAll = allowedShapes.EqualsCaseInsensitive("All");
		MicroStopwatch msw = new MicroStopwatch(true);
		foreach (KeyValuePair<string, XElement> keyValuePair in ShapesFromXml.shapes)
		{
			if (isAll || keyValuePair.Value.GetAttribute(XNames.tag).EqualsCaseInsensitive(allowedShapes))
			{
				ShapesFromXml.CreateShapeMaterialCombination(_bEditMode, _elementBlock, blockBaseName, keyValuePair);
				shapeNames.Add(keyValuePair.Key);
				if (msw.ElapsedMilliseconds > (long)Constants.cMaxLoadTimePerFrameMillis)
				{
					yield return null;
					msw.ResetAndRestart();
				}
			}
		}
		Dictionary<string, XElement>.Enumerator enumerator = default(Dictionary<string, XElement>.Enumerator);
		if (shapeNames.Count > 0)
		{
			ShapesFromXml.CreateMaterialHelper(_bEditMode, blockBaseName, shapeNames, hideHelperInCreative);
		}
		if (ShapesFromXml.debug != ShapesFromXml.EDebugLevel.Off)
		{
			Console.WriteLine();
			Console.WriteLine();
			Console.WriteLine();
			Console.WriteLine();
		}
		yield break;
		yield break;
	}

	// Token: 0x060066DF RID: 26335 RVA: 0x0028C478 File Offset: 0x0028A678
	[PublicizedFrom(EAccessModifier.Private)]
	public static void CreateMaterialHelper(bool _bEditMode, string _blockBaseName, List<string> _shapeNames, bool _hideHelperInCreative)
	{
		string text = _blockBaseName + ":" + ShapesFromXml.VariantHelperName;
		string text2 = _blockBaseName + ":" + _shapeNames[0];
		DynamicProperties dynamicProperties = BlocksFromXml.CreateProperties(text2, null);
		dynamicProperties.SetValue("Extends", text2);
		dynamicProperties.SetValue(Block.PropCreativeMode, (_hideHelperInCreative ? EnumCreativeMode.None : EnumCreativeMode.All).ToStringCached<EnumCreativeMode>());
		dynamicProperties.SetValue(Block.PropCreativeSort2, "0000");
		dynamicProperties.SetValue(Block.PropDescriptionKey, "blockVariantHelperGroupDesc");
		dynamicProperties.SetValue(Block.PropItemTypeIcon, "all_blocks");
		dynamicProperties.SetValue("SelectAlternates", "true");
		string text3 = _blockBaseName + ":" + string.Join("," + _blockBaseName + ":", _shapeNames);
		dynamicProperties.SetValue(Block.PropPlaceAltBlockValue, text3);
		dynamicProperties.SetValue(Block.PropAutoShape, EAutoShapeType.Helper.ToStringCached<EAutoShapeType>());
		if (ShapesFromXml.debug != ShapesFromXml.EDebugLevel.Off)
		{
			Console.WriteLine("{0}: {1}", text, dynamicProperties.PrettyPrint());
		}
		Block block = BlocksFromXml.CreateBlock(_bEditMode, text, dynamicProperties);
		BlocksFromXml.LoadExtendedItemDrops(block);
		BlocksFromXml.InitBlock(block);
	}

	// Token: 0x060066E0 RID: 26336 RVA: 0x0028C580 File Offset: 0x0028A780
	[PublicizedFrom(EAccessModifier.Private)]
	public static void CreateShapeMaterialCombination(bool _bEditMode, XElement _elementBlock, string _blockBaseName, KeyValuePair<string, XElement> _shapeKvp)
	{
		string key = _shapeKvp.Key;
		string text = _blockBaseName + ":" + key;
		string text2;
		string text3;
		BlocksFromXml.ParseExtendedBlock(_elementBlock, out text2, out text3);
		if (text2 == null)
		{
			BlocksFromXml.ParseExtendedBlock(_shapeKvp.Value, out text2, out text3);
		}
		DynamicProperties dynamicProperties = BlocksFromXml.CreateProperties(text2, text3);
		BlocksFromXml.LoadProperties(dynamicProperties, _elementBlock);
		BlocksFromXml.LoadProperties(dynamicProperties, _shapeKvp.Value);
		string text4;
		if (dynamicProperties.TryGetValue(Block.PropDowngradeBlock, out text4))
		{
			text4 = ShapesFromXml.AppendShapeName(text4, _blockBaseName, key);
			dynamicProperties.SetValue(Block.PropDowngradeBlock, text4);
		}
		string text5;
		if (dynamicProperties.TryGetValue(Block.PropUpgradeBlockClass, Block.PropUpgradeBlockToBlock, out text5))
		{
			text5 = ShapesFromXml.AppendShapeName(text5, _blockBaseName, key);
			dynamicProperties.SetValue("UpgradeBlock", "ToBlock", text5);
		}
		string text6;
		if (dynamicProperties.TryGetValue(Block.PropSiblingBlock, out text6))
		{
			text6 = ShapesFromXml.PrependBlockBaseName(text6, _blockBaseName);
			dynamicProperties.SetValue(Block.PropSiblingBlock, text6);
		}
		string text7;
		if (dynamicProperties.TryGetValue("MirrorSibling", out text7))
		{
			text7 = ShapesFromXml.PrependBlockBaseName(text7, _blockBaseName);
			dynamicProperties.SetValue("MirrorSibling", text7);
		}
		dynamicProperties.SetValue(Block.PropCreativeMode, EnumCreativeMode.Dev.ToStringCached<EnumCreativeMode>());
		dynamicProperties.SetParam1(Block.PropCanPickup, _blockBaseName + ":" + ShapesFromXml.VariantHelperName);
		ShapesFromXml.FixCustomIcon(dynamicProperties, _blockBaseName, _shapeKvp.Key);
		ShapesFromXml.FixImposterExchangeId(dynamicProperties, _blockBaseName, _shapeKvp.Key, 0);
		for (int i = 0; i < 1; i++)
		{
			ShapesFromXml.FixTextureId(dynamicProperties, _blockBaseName, _shapeKvp.Key, i);
		}
		ShapesFromXml.SetMaxDamage(dynamicProperties, _blockBaseName, _shapeKvp.Key);
		dynamicProperties.SetValue("AutoShape", EAutoShapeType.Shape.ToStringCached<EAutoShapeType>());
		if (ShapesFromXml.debug != ShapesFromXml.EDebugLevel.Off)
		{
			Console.WriteLine("{0}: {1}", text, dynamicProperties.PrettyPrint());
		}
		Block block = BlocksFromXml.CreateBlock(_bEditMode, text, dynamicProperties);
		bool flag;
		BlocksFromXml.ParseItemDrops(block, _shapeKvp.Value, out flag);
		if (block.itemsToDrop.Count == 0)
		{
			bool flag2;
			BlocksFromXml.ParseItemDrops(block, _elementBlock, out flag2);
			if (!flag2 && !flag)
			{
				BlocksFromXml.LoadExtendedItemDrops(block);
			}
		}
		BlocksFromXml.InitBlock(block);
	}

	// Token: 0x060066E1 RID: 26337 RVA: 0x0028C77C File Offset: 0x0028A97C
	[PublicizedFrom(EAccessModifier.Private)]
	public static void SetMaxDamage(DynamicProperties _properties, string _blockBaseName, string _shapeName)
	{
		if (!_properties.Contains("MaterialHitpointMultiplier"))
		{
			return;
		}
		if (!_properties.Contains("Material"))
		{
			Log.Warning(string.Concat(new string[] { "Blocks: Shape ", _shapeName, " defines a 'MaterialHitpointMultiplier' but block template ", _blockBaseName, " does not define a 'Material'!" }));
			return;
		}
		float num = StringParsers.ParseFloat(_properties.GetString("MaterialHitpointMultiplier"), 0, -1, NumberStyles.Any);
		if (num == 1f)
		{
			return;
		}
		string @string = _properties.GetString("Material");
		MaterialBlock materialBlock = MaterialBlock.fromString(@string);
		if (materialBlock == null)
		{
			Log.Error(string.Concat(new string[] { "Blocks: Block template ", _blockBaseName, " refers to an unknown Material '", @string, "'!" }));
			return;
		}
		int num2 = Mathf.RoundToInt(num * (float)materialBlock.MaxDamage);
		num2 = Utils.FastClamp(num2, 1, 65535);
		_properties.SetValue(Block.PropMaxDamage, num2.ToString());
		ShapesFromXml.ScaleProperty(_properties, Block.PropUpgradeBlockClass, Block.PropUpgradeBlockItemCount, num);
		_properties.SetValue(Block.PropResourceScale, num.ToCultureInvariantString());
	}

	// Token: 0x060066E2 RID: 26338 RVA: 0x0028C890 File Offset: 0x0028AA90
	[PublicizedFrom(EAccessModifier.Private)]
	public static void FixCustomIcon(DynamicProperties _properties, string _blockBaseName, string _shapeName)
	{
		if (_properties.Contains(Block.PropCustomIcon))
		{
			return;
		}
		if (!_properties.Contains("Model"))
		{
			Log.Warning(string.Concat(new string[] { "Blocks: Neither shape ", _shapeName, " nor the block template ", _blockBaseName, " define a 'CustomIcon' or 'Model'!" }));
			return;
		}
		string @string = _properties.GetString("Model");
		_properties.SetValue(Block.PropCustomIcon, "shape" + @string);
	}

	// Token: 0x060066E3 RID: 26339 RVA: 0x0028C90C File Offset: 0x0028AB0C
	public static void SetProperty(XElement _element, string _propertyName, XName _attribName, string _value)
	{
		XElement xelement = (from e in _element.Elements(XNames.property)
			where e.GetAttribute(XNames.name) == _propertyName
			select e).FirstOrDefault<XElement>();
		if (xelement == null)
		{
			xelement = new XElement(XNames.property, new XAttribute(XNames.name, _propertyName));
			_element.Add(xelement);
		}
		xelement.SetAttributeValue(_attribName, _value);
	}

	// Token: 0x060066E4 RID: 26340 RVA: 0x0028C978 File Offset: 0x0028AB78
	[PublicizedFrom(EAccessModifier.Private)]
	public static void ScaleProperty(DynamicProperties _properties, string _className, string _propertyName, float _scale)
	{
		if (_properties.Contains(_className, _propertyName))
		{
			int num = int.Parse(_properties.GetString(_className, _propertyName));
			if (num > 0)
			{
				num = (int)((float)num * _scale);
				if (num < 1)
				{
					num = 1;
				}
				_properties.SetValue(_className, _propertyName, num.ToString());
			}
		}
	}

	// Token: 0x060066E5 RID: 26341 RVA: 0x0028C9BC File Offset: 0x0028ABBC
	[PublicizedFrom(EAccessModifier.Private)]
	public static void FixImposterExchangeId(DynamicProperties _properties, string _blockBaseName, string _shapeName, int channel)
	{
		string texture = ShapesFromXml.TextureLabelsByChannel[channel].Texture;
		string imposterExchange = ShapesFromXml.TextureLabelsByChannel[channel].ImposterExchange;
		if (!_properties.Contains(imposterExchange))
		{
			return;
		}
		if (!_properties.Contains(texture))
		{
			Log.Warning(string.Concat(new string[] { "Blocks: Shape ", _shapeName, " defines ", imposterExchange, " but block template ", _blockBaseName, " does not have a '", texture, "' property!" }));
			return;
		}
		int id = BlockTextureData.GetDataByTextureID(int.Parse(_properties.GetString(texture))).ID;
		_properties.SetParam1(imposterExchange, id.ToString());
	}

	// Token: 0x060066E6 RID: 26342 RVA: 0x0028CA70 File Offset: 0x0028AC70
	[PublicizedFrom(EAccessModifier.Private)]
	public static void FixTextureId(DynamicProperties _properties, string _blockBaseName, string _shapeName, int channel)
	{
		string texture = ShapesFromXml.TextureLabelsByChannel[channel].Texture;
		string shapeAltTexture = ShapesFromXml.TextureLabelsByChannel[channel].ShapeAltTexture;
		if (!_properties.Contains(shapeAltTexture))
		{
			return;
		}
		if (!_properties.Contains(texture))
		{
			Log.Warning(string.Concat(new string[] { "Blocks: Shape ", _shapeName, " defines ", shapeAltTexture, " but block template ", _blockBaseName, " does not have a '", texture, "' property!" }));
			return;
		}
		string[] array = _properties.GetString(shapeAltTexture).Split(',', StringSplitOptions.None);
		string @string = _properties.GetString(texture);
		for (int i = 0; i < array.Length; i++)
		{
			int num;
			if (!int.TryParse(array[i], out num))
			{
				array[i] = @string;
			}
		}
		string text = string.Join(",", array);
		_properties.SetValue(texture, text);
	}

	// Token: 0x060066E7 RID: 26343 RVA: 0x0028CB4E File Offset: 0x0028AD4E
	[PublicizedFrom(EAccessModifier.Private)]
	public static string AppendShapeName(string _innerText, string _blockBaseName, string _shapeName)
	{
		if (_innerText[0] == ':')
		{
			return _blockBaseName + _innerText;
		}
		if (!_innerText.Contains(":"))
		{
			return _innerText + ":" + _shapeName;
		}
		return _innerText;
	}

	// Token: 0x060066E8 RID: 26344 RVA: 0x0028CB7E File Offset: 0x0028AD7E
	[PublicizedFrom(EAccessModifier.Private)]
	public static string PrependBlockBaseName(string _innerText, string _blockBaseName)
	{
		return _blockBaseName + ":" + _innerText;
	}

	// Token: 0x04004FB6 RID: 20406
	[PublicizedFrom(EAccessModifier.Private)]
	public static ShapesFromXml.EDebugLevel debug = ShapesFromXml.EDebugLevel.Off;

	// Token: 0x04004FB7 RID: 20407
	public static readonly ShapesFromXml.TextureLabels[] TextureLabelsByChannel = new ShapesFromXml.TextureLabels[1];

	// Token: 0x04004FB8 RID: 20408
	public static readonly string VariantHelperName = "VariantHelper";

	// Token: 0x04004FB9 RID: 20409
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<string, XElement> shapes;

	// Token: 0x04004FBA RID: 20410
	public static readonly Dictionary<string, ShapesFromXml.ShapeCategory> shapeCategories = new CaseInsensitiveStringDictionary<ShapesFromXml.ShapeCategory>();

	// Token: 0x02000D48 RID: 3400
	public class ShapeCategory : IComparable<ShapesFromXml.ShapeCategory>, IEquatable<ShapesFromXml.ShapeCategory>, IComparable
	{
		// Token: 0x17000AEE RID: 2798
		// (get) Token: 0x060066E9 RID: 26345 RVA: 0x0028CB8C File Offset: 0x0028AD8C
		public string LocalizedName
		{
			get
			{
				return Localization.Get(this.localizationName, false, null);
			}
		}

		// Token: 0x060066EA RID: 26346 RVA: 0x0028CB9B File Offset: 0x0028AD9B
		public ShapeCategory(string _name, string _icon, int _order)
		{
			this.Name = _name;
			this.Icon = _icon;
			this.Order = _order;
			this.localizationName = "shapeCategory" + this.Name;
		}

		// Token: 0x060066EB RID: 26347 RVA: 0x0028CBD0 File Offset: 0x0028ADD0
		public bool Equals(ShapesFromXml.ShapeCategory _other)
		{
			return _other != null && (this == _other || (this.Name == _other.Name && this.Icon == _other.Icon && this.Order == _other.Order));
		}

		// Token: 0x060066EC RID: 26348 RVA: 0x0028CC1E File Offset: 0x0028AE1E
		public override bool Equals(object _obj)
		{
			return _obj != null && (this == _obj || (!(_obj.GetType() != base.GetType()) && this.Equals((ShapesFromXml.ShapeCategory)_obj)));
		}

		// Token: 0x060066ED RID: 26349 RVA: 0x0028CC4C File Offset: 0x0028AE4C
		public override int GetHashCode()
		{
			return (((((this.Name != null) ? this.Name.GetHashCode() : 0) * 397) ^ ((this.Icon != null) ? this.Icon.GetHashCode() : 0)) * 397) ^ this.Order;
		}

		// Token: 0x060066EE RID: 26350 RVA: 0x0028CC9C File Offset: 0x0028AE9C
		public int CompareTo(ShapesFromXml.ShapeCategory _other)
		{
			if (this == _other)
			{
				return 0;
			}
			if (_other == null)
			{
				return 1;
			}
			return this.Order.CompareTo(_other.Order);
		}

		// Token: 0x060066EF RID: 26351 RVA: 0x0028CCC8 File Offset: 0x0028AEC8
		public int CompareTo(object _obj)
		{
			if (_obj == null)
			{
				return 1;
			}
			if (this == _obj)
			{
				return 0;
			}
			ShapesFromXml.ShapeCategory shapeCategory = _obj as ShapesFromXml.ShapeCategory;
			if (shapeCategory == null)
			{
				throw new ArgumentException("Object must be of type ShapeCategory");
			}
			return this.CompareTo(shapeCategory);
		}

		// Token: 0x04004FBB RID: 20411
		public readonly string Name;

		// Token: 0x04004FBC RID: 20412
		public readonly string Icon;

		// Token: 0x04004FBD RID: 20413
		public readonly int Order;

		// Token: 0x04004FBE RID: 20414
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string localizationName;
	}

	// Token: 0x02000D49 RID: 3401
	public enum EDebugLevel
	{
		// Token: 0x04004FC0 RID: 20416
		Off,
		// Token: 0x04004FC1 RID: 20417
		Normal,
		// Token: 0x04004FC2 RID: 20418
		Verbose
	}

	// Token: 0x02000D4A RID: 3402
	public struct TextureLabels
	{
		// Token: 0x060066F0 RID: 26352 RVA: 0x0028CCFC File Offset: 0x0028AEFC
		public TextureLabels(string suffix)
		{
			this.Texture = "Texture" + suffix;
			this.ImposterExchange = "ImposterExchange" + suffix;
			this.ShapeAltTexture = "ShapeAltTexture" + suffix;
			this.UseGlobalUV = "UseGlobalUV" + suffix;
		}

		// Token: 0x04004FC3 RID: 20419
		public readonly string Texture;

		// Token: 0x04004FC4 RID: 20420
		public readonly string ImposterExchange;

		// Token: 0x04004FC5 RID: 20421
		public readonly string ShapeAltTexture;

		// Token: 0x04004FC6 RID: 20422
		public readonly string UseGlobalUV;
	}
}
