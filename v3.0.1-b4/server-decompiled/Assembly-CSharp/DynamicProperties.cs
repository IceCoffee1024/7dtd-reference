using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using MemoryPack;
using MemoryPack.Formatters;
using MemoryPack.Internal;
using UnityEngine;

// Token: 0x020013D2 RID: 5074
[MemoryPackable(GenerateType.Object)]
public class DynamicProperties : IMemoryPackable<DynamicProperties>, IMemoryPackFormatterRegister
{
	// Token: 0x06009F00 RID: 40704 RVA: 0x003BD2B8 File Offset: 0x003BB4B8
	public Dictionary<string, string> ParseKeyData(string key)
	{
		try
		{
			string text;
			if (this.Data.TryGetValue(key, out text))
			{
				return DynamicProperties.ParseData(text);
			}
		}
		catch (Exception ex)
		{
			Log.Error("ParseKeyData error parsing key {0}, {1}", new object[] { key, ex });
		}
		return null;
	}

	// Token: 0x06009F01 RID: 40705 RVA: 0x003BD310 File Offset: 0x003BB510
	public static Dictionary<string, string> ParseData(string data)
	{
		Dictionary<string, string> dictionary = null;
		try
		{
			dictionary = new Dictionary<string, string>();
			if (data.IndexOf(';') < 0)
			{
				string[] array = data.Split(DynamicProperties.equalSeparator, StringSplitOptions.RemoveEmptyEntries);
				if (array.Length >= 2)
				{
					dictionary[array[0]] = array[1];
				}
			}
			else
			{
				string[] array2 = data.Split(DynamicProperties.semicolonSeparator, StringSplitOptions.RemoveEmptyEntries);
				for (int i = 0; i < array2.Length; i++)
				{
					string[] array3 = array2[i].Split(DynamicProperties.equalSeparator, StringSplitOptions.RemoveEmptyEntries);
					if (array3.Length >= 2)
					{
						dictionary[array3[0]] = array3[1];
					}
				}
			}
		}
		catch (Exception ex)
		{
			Log.Error("ParseData error parsing {0}, {1}", new object[] { data, ex });
		}
		return dictionary;
	}

	// Token: 0x06009F03 RID: 40707 RVA: 0x003BD419 File Offset: 0x003BB619
	[PublicizedFrom(EAccessModifier.Private)]
	public static void ValidateKey(string _key)
	{
		if (string.IsNullOrEmpty(_key) || _key.IndexOf('.') >= 0)
		{
			throw new Exception(string.Format("Property name '{0}' contains illegal character '{1}'", _key, '.'));
		}
	}

	// Token: 0x06009F04 RID: 40708 RVA: 0x003BD450 File Offset: 0x003BB650
	[PublicizedFrom(EAccessModifier.Private)]
	public DynamicProperties GetOrCreateClass(string _className)
	{
		DynamicProperties.ValidateKey(_className);
		DynamicProperties dynamicProperties;
		if (!this.Classes.TryGetValue(_className, out dynamicProperties))
		{
			dynamicProperties = new DynamicProperties();
			this.Classes[_className] = dynamicProperties;
		}
		return dynamicProperties;
	}

	// Token: 0x06009F05 RID: 40709 RVA: 0x003BD488 File Offset: 0x003BB688
	public DynamicProperties GetClass(string _className)
	{
		DynamicProperties dynamicProperties;
		if (this.Classes.TryGetValue(_className, out dynamicProperties))
		{
			return dynamicProperties;
		}
		return null;
	}

	// Token: 0x06009F06 RID: 40710 RVA: 0x003BD4A8 File Offset: 0x003BB6A8
	public void SetValue(string _propName, string _value)
	{
		DynamicProperties.ValidateKey(_propName);
		this.Values[_propName] = _value;
	}

	// Token: 0x06009F07 RID: 40711 RVA: 0x003BD4BD File Offset: 0x003BB6BD
	public void SetValue(string _className, string _propName, string _value)
	{
		this.GetOrCreateClass(_className).SetValue(_propName, _value);
	}

	// Token: 0x06009F08 RID: 40712 RVA: 0x003BD4D0 File Offset: 0x003BB6D0
	public string GetValue(string _propName)
	{
		string text;
		if (this.Values.TryGetValue(_propName, out text))
		{
			return text;
		}
		return null;
	}

	// Token: 0x06009F09 RID: 40713 RVA: 0x003BD4F0 File Offset: 0x003BB6F0
	public bool TryGetValue(string _propName, out string _value)
	{
		DynamicProperties.ValidateKey(_propName);
		return this.Values.TryGetValue(_propName, out _value);
	}

	// Token: 0x06009F0A RID: 40714 RVA: 0x003BD508 File Offset: 0x003BB708
	public bool TryGetValue(string _className, string _propName, out string _value)
	{
		DynamicProperties.ValidateKey(_className);
		DynamicProperties dynamicProperties;
		if (!this.Classes.TryGetValue(_className, out dynamicProperties))
		{
			_value = null;
			return false;
		}
		return dynamicProperties.TryGetValue(_propName, out _value);
	}

	// Token: 0x06009F0B RID: 40715 RVA: 0x003BD538 File Offset: 0x003BB738
	public bool Contains(string _propName)
	{
		DynamicProperties.ValidateKey(_propName);
		return this.Values.ContainsKey(_propName);
	}

	// Token: 0x06009F0C RID: 40716 RVA: 0x003BD54C File Offset: 0x003BB74C
	public bool Contains(string _className, string _propName)
	{
		string text;
		return this.TryGetValue(_className, _propName, out text);
	}

	// Token: 0x06009F0D RID: 40717 RVA: 0x003BD564 File Offset: 0x003BB764
	public void SetParam1(string _propName, string _param1)
	{
		DynamicProperties.ValidateKey(_propName);
		if (!this.Values.ContainsKey(_propName))
		{
			this.Values.Add(_propName, null);
		}
		if (this.Params1.ContainsKey(_propName))
		{
			this.Params1[_propName] = _param1;
			return;
		}
		this.Params1.Add(_propName, _param1);
	}

	// Token: 0x06009F0E RID: 40718 RVA: 0x003BD5BB File Offset: 0x003BB7BB
	public void SetParam1(string _className, string _propName, string _param1)
	{
		this.GetOrCreateClass(_className).SetParam1(_propName, _param1);
	}

	// Token: 0x06009F0F RID: 40719 RVA: 0x003BD5CB File Offset: 0x003BB7CB
	public bool TryGetParam1(string _propName, out string _param1)
	{
		DynamicProperties.ValidateKey(_propName);
		return this.Params1.TryGetValue(_propName, out _param1);
	}

	// Token: 0x06009F10 RID: 40720 RVA: 0x003BD5E0 File Offset: 0x003BB7E0
	public bool TryGetParam1(string _className, string _propName, out string _param1)
	{
		DynamicProperties.ValidateKey(_className);
		DynamicProperties dynamicProperties;
		if (!this.Classes.TryGetValue(_className, out dynamicProperties))
		{
			_param1 = null;
			return false;
		}
		return dynamicProperties.TryGetParam1(_propName, out _param1);
	}

	// Token: 0x06009F11 RID: 40721 RVA: 0x003BD610 File Offset: 0x003BB810
	public string GetParam1(string _propName)
	{
		string text;
		if (this.Params1.TryGetValue(_propName, out text))
		{
			return text;
		}
		return null;
	}

	// Token: 0x06009F12 RID: 40722 RVA: 0x003BD630 File Offset: 0x003BB830
	public bool GetBool(string _propName)
	{
		string text;
		if (!this.TryGetValue(_propName, out text))
		{
			return false;
		}
		bool flag;
		StringParsers.TryParseBool(text, out flag);
		return flag;
	}

	// Token: 0x06009F13 RID: 40723 RVA: 0x003BD654 File Offset: 0x003BB854
	public bool GetBool(string _className, string _propName)
	{
		string text;
		if (!this.TryGetValue(_className, _propName, out text))
		{
			return false;
		}
		bool flag;
		StringParsers.TryParseBool(text, out flag);
		return flag;
	}

	// Token: 0x06009F14 RID: 40724 RVA: 0x003BD67C File Offset: 0x003BB87C
	public float GetFloat(string _propName)
	{
		string text;
		if (!this.TryGetValue(_propName, out text))
		{
			return 0f;
		}
		float num;
		StringParsers.TryParseFloat(text, out num);
		return num;
	}

	// Token: 0x06009F15 RID: 40725 RVA: 0x003BD6A4 File Offset: 0x003BB8A4
	public float GetFloat(string _className, string _propName)
	{
		string text;
		if (!this.TryGetValue(_className, _propName, out text))
		{
			return 0f;
		}
		float num;
		StringParsers.TryParseFloat(text, out num);
		return num;
	}

	// Token: 0x06009F16 RID: 40726 RVA: 0x003BD6D0 File Offset: 0x003BB8D0
	public int GetInt(string _propName)
	{
		string text;
		if (!this.TryGetValue(_propName, out text))
		{
			return 0;
		}
		int num;
		int.TryParse(text, out num);
		return num;
	}

	// Token: 0x06009F17 RID: 40727 RVA: 0x003BD6F4 File Offset: 0x003BB8F4
	public int GetInt(string _className, string _propName)
	{
		string text;
		if (!this.TryGetValue(_className, _propName, out text))
		{
			return 0;
		}
		int num;
		int.TryParse(text, out num);
		return num;
	}

	// Token: 0x06009F18 RID: 40728 RVA: 0x003BD71C File Offset: 0x003BB91C
	public string GetString(string _propName)
	{
		string text;
		if (!this.TryGetValue(_propName, out text))
		{
			return string.Empty;
		}
		return text;
	}

	// Token: 0x06009F19 RID: 40729 RVA: 0x003BD73C File Offset: 0x003BB93C
	public string GetString(string _className, string _propName)
	{
		string text;
		if (!this.TryGetValue(_className, _propName, out text))
		{
			return string.Empty;
		}
		return text;
	}

	// Token: 0x06009F1A RID: 40730 RVA: 0x003BD75C File Offset: 0x003BB95C
	public bool Load(string _directory, string _name)
	{
		try
		{
			foreach (XElement xelement in new XmlFile(_directory, _name, false, false).XmlDoc.Root.Elements(XNames.property))
			{
				this.Add(xelement, false);
			}
			return true;
		}
		catch (Exception ex)
		{
			Log.Exception(ex);
		}
		return false;
	}

	// Token: 0x06009F1B RID: 40731 RVA: 0x003BD7DC File Offset: 0x003BB9DC
	public bool Save(string _rootNodeName, Stream stream)
	{
		try
		{
			XmlDocument xmlDocument = new XmlDocument();
			XmlDeclaration xmlDeclaration = xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);
			xmlDocument.InsertBefore(xmlDeclaration, xmlDocument.DocumentElement);
			XmlNode xmlNode = xmlDocument.AppendChild(xmlDocument.CreateElement(_rootNodeName));
			this.toXml(xmlDocument, xmlNode);
			xmlDocument.Save(stream);
			return true;
		}
		catch (Exception ex)
		{
			Log.Exception(ex);
		}
		return false;
	}

	// Token: 0x06009F1C RID: 40732 RVA: 0x003BD84C File Offset: 0x003BBA4C
	public bool Save(string _rootNodeName, string _path, string _name)
	{
		try
		{
			using (Stream stream = SdFile.Create(Path.Join(_path, _name + ".xml")))
			{
				return this.Save(_rootNodeName, stream);
			}
		}
		catch (Exception ex)
		{
			Log.Exception(ex);
		}
		return false;
	}

	// Token: 0x06009F1D RID: 40733 RVA: 0x003BD8B8 File Offset: 0x003BBAB8
	[PublicizedFrom(EAccessModifier.Private)]
	public void toXml(XmlDocument _doc, XmlNode _parent)
	{
		foreach (KeyValuePair<string, string> keyValuePair in this.Values)
		{
			XmlElement xmlElement = _doc.CreateElement("property");
			XmlAttribute xmlAttribute = _doc.CreateAttribute("name");
			xmlAttribute.Value = keyValuePair.Key;
			xmlElement.Attributes.Append(xmlAttribute);
			XmlAttribute xmlAttribute2 = _doc.CreateAttribute("value");
			xmlAttribute2.Value = this.Values[keyValuePair.Key];
			xmlElement.Attributes.Append(xmlAttribute2);
			if (this.Params1.ContainsKey(keyValuePair.Key))
			{
				XmlAttribute xmlAttribute3 = _doc.CreateAttribute("param1");
				xmlAttribute3.Value = this.Params1[keyValuePair.Key];
				xmlElement.Attributes.Append(xmlAttribute3);
			}
			if (this.Data.ContainsKey(keyValuePair.Key))
			{
				XmlAttribute xmlAttribute4 = _doc.CreateAttribute("fields");
				xmlAttribute4.Value = this.Data[keyValuePair.Key];
				xmlElement.Attributes.Append(xmlAttribute4);
			}
			_parent.AppendChild(xmlElement);
		}
		if (this.Classes.Count > 0)
		{
			foreach (KeyValuePair<string, DynamicProperties> keyValuePair2 in this.Classes)
			{
				XmlElement xmlElement2 = _doc.CreateElement("property");
				XmlAttribute xmlAttribute5 = _doc.CreateAttribute("class");
				xmlAttribute5.Value = keyValuePair2.Key;
				xmlElement2.Attributes.Append(xmlAttribute5);
				keyValuePair2.Value.toXml(_doc, xmlElement2);
				_parent.AppendChild(xmlElement2);
			}
		}
	}

	// Token: 0x06009F1E RID: 40734 RVA: 0x003BDAC0 File Offset: 0x003BBCC0
	public void Add(XElement _propertyNode, bool _doValueReplace = false)
	{
		this.Parse(_propertyNode, _doValueReplace);
	}

	// Token: 0x06009F1F RID: 40735 RVA: 0x003BDACC File Offset: 0x003BBCCC
	public void AddArray(XElement _arrayNode)
	{
		string attribute = _arrayNode.GetAttribute(XNames.name);
		if (!string.IsNullOrEmpty(attribute))
		{
			List<Dictionary<string, string>> list;
			if (!this.Array.TryGetValue(attribute, out list) || list == null)
			{
				list = new List<Dictionary<string, string>>();
				this.Array[attribute] = list;
			}
			list.Clear();
			foreach (XElement xelement in _arrayNode.Elements(XNames.item))
			{
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				list.Add(dictionary);
				foreach (XAttribute xattribute in xelement.Attributes())
				{
					dictionary[xattribute.Name.ToString()] = xattribute.Value;
				}
			}
		}
	}

	// Token: 0x06009F20 RID: 40736 RVA: 0x003BDBBC File Offset: 0x003BBDBC
	public void Parse(XElement elementProperty, bool _doValueReplace = false)
	{
		if (elementProperty.HasAttribute(XNames.class_))
		{
			string attribute = elementProperty.GetAttribute(XNames.class_);
			DynamicProperties orCreateClass = this.GetOrCreateClass(attribute);
			using (IEnumerator<XElement> enumerator = elementProperty.Elements(XNames.property).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					XElement xelement = enumerator.Current;
					orCreateClass.Parse(xelement, _doValueReplace);
				}
				return;
			}
		}
		string attribute2 = elementProperty.GetAttribute(XNames.name);
		if (attribute2.Length == 0)
		{
			throw new Exception("Attribute 'name' missing on property");
		}
		DynamicProperties.ValidateKey(attribute2);
		string text = attribute2;
		string text2 = elementProperty.GetAttribute(XNames.value);
		if (_doValueReplace)
		{
			text2 = EntityClassesFromXml.ReplaceProperty(text2);
		}
		string text3 = null;
		if (elementProperty.HasAttribute(XNames.param1))
		{
			text3 = elementProperty.GetAttribute(XNames.param1);
		}
		string text4 = null;
		if (elementProperty.HasAttribute(XNames.param2))
		{
			text4 = elementProperty.GetAttribute(XNames.param2);
		}
		if (text3 != null)
		{
			this.Params1[text] = text3;
		}
		if (text4 != null)
		{
			this.Params2[text] = text4;
		}
		if (text2 != null)
		{
			string attribute3 = elementProperty.GetAttribute(XNames.data);
			if (attribute3.Length > 0)
			{
				this.Data[text] = attribute3;
			}
		}
		if (this.Classes.ContainsKey(text))
		{
			throw new Exception("Cannot create property '" + text + "': a class with the same name already exists. Property and class names must be unique.");
		}
		this.Values[text] = text2;
	}

	// Token: 0x06009F21 RID: 40737 RVA: 0x003BDD3C File Offset: 0x003BBF3C
	public void Clear()
	{
		this.Values.Clear();
		this.Params1.Clear();
		this.Params2.Clear();
		this.Data.Clear();
		this.Classes.Clear();
	}

	// Token: 0x06009F22 RID: 40738 RVA: 0x003BDD78 File Offset: 0x003BBF78
	public void ParseString(string _propName, ref string optionalValue)
	{
		string text;
		if (this.TryGetValue(_propName, out text))
		{
			optionalValue = text;
		}
	}

	// Token: 0x06009F23 RID: 40739 RVA: 0x003BDD94 File Offset: 0x003BBF94
	public void ParseString(string _className, string _propName, ref string optionalValue)
	{
		string text;
		if (this.TryGetValue(_className, _propName, out text))
		{
			optionalValue = text;
		}
	}

	// Token: 0x06009F24 RID: 40740 RVA: 0x003BDDB0 File Offset: 0x003BBFB0
	public void ParseStringFloatDictWithSubStringKey(string _propName, char _propSeparator, Dictionary<string, string> _dict, ref Dictionary<string, float> optionalValue)
	{
		foreach (KeyValuePair<string, string> keyValuePair in _dict)
		{
			if (keyValuePair.Key.StartsWith(_propName))
			{
				string text = keyValuePair.Key.Substring(keyValuePair.Key.IndexOf(_propSeparator) + 1);
				if (text.Length != keyValuePair.Key.Length)
				{
					optionalValue.Add(text, StringParsers.ParseFloat(keyValuePair.Value, 0, -1, NumberStyles.Any));
				}
			}
		}
	}

	// Token: 0x06009F25 RID: 40741 RVA: 0x003BDE54 File Offset: 0x003BC054
	public IntRange TryParseRange(string _propName, IntRange defaultValue = default(IntRange))
	{
		IntRange intRange = defaultValue;
		string text;
		if (this.TryGetValue(_propName, out text))
		{
			try
			{
				if (!StringParsers.TryParseRange(text, out intRange, null, '-'))
				{
					intRange = defaultValue;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
		}
		return intRange;
	}

	// Token: 0x06009F26 RID: 40742 RVA: 0x003BDEA0 File Offset: 0x003BC0A0
	public FloatRange TryParseRange(string _propName, FloatRange defaultValue = default(FloatRange))
	{
		FloatRange floatRange = defaultValue;
		string text;
		if (this.TryGetValue(_propName, out text))
		{
			try
			{
				if (!StringParsers.TryParseRange(text, out floatRange, null, '-'))
				{
					floatRange = defaultValue;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
		}
		return floatRange;
	}

	// Token: 0x06009F27 RID: 40743 RVA: 0x003BDEEC File Offset: 0x003BC0EC
	public string GetLocalizedString(string _propName)
	{
		string text;
		if (this.TryGetValue(_propName, out text))
		{
			return text;
		}
		return string.Empty;
	}

	// Token: 0x06009F28 RID: 40744 RVA: 0x003BDF0C File Offset: 0x003BC10C
	public void ParseLocalizedString(string _propName, ref string optionalValue)
	{
		string text;
		if (this.TryGetValue(_propName, out text))
		{
			optionalValue = Localization.Get(text, false, null);
		}
	}

	// Token: 0x06009F29 RID: 40745 RVA: 0x003BDF30 File Offset: 0x003BC130
	public void ParseBool(string _propName, ref bool optionalValue)
	{
		string text;
		if (this.TryGetValue(_propName, out text))
		{
			bool flag;
			if (StringParsers.TryParseBool(text, out flag))
			{
				optionalValue = flag;
				return;
			}
			Log.Warning("Can't parse bool {0} '{1}'", new object[] { _propName, text });
		}
	}

	// Token: 0x06009F2A RID: 40746 RVA: 0x003BDF70 File Offset: 0x003BC170
	public void ParseByte(string _propName, ref byte optionalValue)
	{
		string text;
		if (this.TryGetValue(_propName, out text))
		{
			byte b;
			if (StringParsers.TryParseUInt8(text, out b))
			{
				optionalValue = b;
				return;
			}
			Log.Warning("Can't parse byte {0} '{1}'", new object[] { _propName, text });
		}
	}

	// Token: 0x06009F2B RID: 40747 RVA: 0x003BDFB0 File Offset: 0x003BC1B0
	public void ParseColor(string _propName, ref Color optionalValue)
	{
		string text;
		if (this.TryGetValue(_propName, out text))
		{
			optionalValue = StringParsers.ParseColor32(text);
		}
	}

	// Token: 0x06009F2C RID: 40748 RVA: 0x003BDFD4 File Offset: 0x003BC1D4
	public void ParseColorHex(string _propName, ref Color optionalValue)
	{
		string text;
		if (this.TryGetValue(_propName, out text))
		{
			optionalValue = StringParsers.ParseHexColor(text);
		}
	}

	// Token: 0x06009F2D RID: 40749 RVA: 0x003BDFF8 File Offset: 0x003BC1F8
	public void ParseEnum<T>(string _propName, ref T optionalValue) where T : struct, IConvertible
	{
		string text;
		T t;
		if (this.TryGetValue(_propName, out text) && EnumUtils.TryParse<T>(text, out t, true))
		{
			optionalValue = t;
		}
	}

	// Token: 0x06009F2E RID: 40750 RVA: 0x003BE024 File Offset: 0x003BC224
	public void ParseFloat(string _propName, ref float optionalValue)
	{
		string text;
		if (this.TryGetValue(_propName, out text))
		{
			float num;
			if (StringParsers.TryParseFloat(text, out num))
			{
				optionalValue = num;
				return;
			}
			Log.Warning("Can't parse float {0} '{1}'", new object[] { _propName, text });
		}
	}

	// Token: 0x06009F2F RID: 40751 RVA: 0x003BE064 File Offset: 0x003BC264
	public void ParseFloat(string _className, string _propName, ref float optionalValue)
	{
		string text;
		if (this.TryGetValue(_className, _propName, out text))
		{
			float num;
			if (StringParsers.TryParseFloat(text, out num))
			{
				optionalValue = num;
				return;
			}
			Log.Warning("Can't parse float '{0}'", new object[] { text });
		}
	}

	// Token: 0x06009F30 RID: 40752 RVA: 0x003BE0A0 File Offset: 0x003BC2A0
	public static void ParseFloat(XElement _e, string _propName, ref float optionalValue)
	{
		XAttribute xattribute = _e.Attribute(_propName);
		if (xattribute != null)
		{
			string value = xattribute.Value;
			float num;
			if (StringParsers.TryParseFloat(value, out num))
			{
				optionalValue = num;
				return;
			}
			Log.Warning("Can't parse float {0} '{1}'", new object[] { _propName, value });
		}
	}

	// Token: 0x06009F31 RID: 40753 RVA: 0x003BE0EC File Offset: 0x003BC2EC
	public void ParseInt(string _propName, ref int optionalValue)
	{
		string text;
		if (this.TryGetValue(_propName, out text))
		{
			int num;
			if (StringParsers.TryParseSInt32(text, out num))
			{
				optionalValue = num;
				return;
			}
			Log.Warning("Can't parse int {0} '{1}'", new object[] { _propName, text });
		}
	}

	// Token: 0x06009F32 RID: 40754 RVA: 0x003BE12C File Offset: 0x003BC32C
	public void ParseInt(string _className, string _propName, ref int optionalValue)
	{
		string text;
		if (this.TryGetValue(_className, _propName, out text))
		{
			int num;
			if (StringParsers.TryParseSInt32(text, out num))
			{
				optionalValue = num;
				return;
			}
			Log.Warning("Can't parse int {0}.{1} '{2}'", new object[] { _className, _propName, text });
		}
	}

	// Token: 0x06009F33 RID: 40755 RVA: 0x003BE170 File Offset: 0x003BC370
	public void ParseVec(string _propName, ref Vector2 optionalValue)
	{
		string text;
		if (this.TryGetValue(_propName, out text))
		{
			optionalValue = StringParsers.ParseVector2(text);
		}
	}

	// Token: 0x06009F34 RID: 40756 RVA: 0x003BE194 File Offset: 0x003BC394
	public void ParseVec(string _className, string _propName, ref Vector2 optionalValue)
	{
		string text;
		if (this.TryGetValue(_className, _propName, out text))
		{
			optionalValue = StringParsers.ParseVector2(text);
		}
	}

	// Token: 0x06009F35 RID: 40757 RVA: 0x003BE1BC File Offset: 0x003BC3BC
	public void ParseVec(string _propName, ref Vector2 optionalValue, float _defaultValue)
	{
		string text;
		if (this.TryGetValue(_propName, out text))
		{
			optionalValue = StringParsers.ParseVector2(text, _defaultValue);
		}
	}

	// Token: 0x06009F36 RID: 40758 RVA: 0x003BE1E4 File Offset: 0x003BC3E4
	public void ParseVec(string _className, string _propName, ref Vector2 optionalValue, float _defaultValue)
	{
		string text;
		if (this.TryGetValue(_className, _propName, out text))
		{
			optionalValue = StringParsers.ParseVector2(text, _defaultValue);
		}
	}

	// Token: 0x06009F37 RID: 40759 RVA: 0x003BE20C File Offset: 0x003BC40C
	public void ParseVec(string _propName, ref Vector3 optionalValue)
	{
		string text;
		if (this.TryGetValue(_propName, out text))
		{
			optionalValue = StringParsers.ParseVector3(text, 0, -1);
		}
	}

	// Token: 0x06009F38 RID: 40760 RVA: 0x003BE234 File Offset: 0x003BC434
	public void ParseVec(string _className, string _propName, ref Vector3 optionalValue)
	{
		string text;
		if (this.TryGetValue(_className, _propName, out text))
		{
			optionalValue = StringParsers.ParseVector3(text, 0, -1);
		}
	}

	// Token: 0x06009F39 RID: 40761 RVA: 0x003BE25C File Offset: 0x003BC45C
	public void ParseVec(string _propName, ref Vector3 optionalValue, float _defaultValue)
	{
		string text;
		if (this.TryGetValue(_propName, out text))
		{
			optionalValue = StringParsers.ParseVector3(text, _defaultValue);
		}
	}

	// Token: 0x06009F3A RID: 40762 RVA: 0x003BE284 File Offset: 0x003BC484
	public void ParseVec(string _className, string _propName, ref Vector3 optionalValue, float _defaultValue)
	{
		string text;
		if (this.TryGetValue(_className, _propName, out text))
		{
			optionalValue = StringParsers.ParseVector3(text, _defaultValue);
		}
	}

	// Token: 0x06009F3B RID: 40763 RVA: 0x003BE2AC File Offset: 0x003BC4AC
	public void ParseVec(string _propName, ref Vector3i optionalValue)
	{
		string text;
		if (this.TryGetValue(_propName, out text))
		{
			optionalValue = StringParsers.ParseVector3i(text, 0, -1, false);
		}
	}

	// Token: 0x06009F3C RID: 40764 RVA: 0x003BE2D4 File Offset: 0x003BC4D4
	public void ParseVec(string _className, string _propName, ref Vector3i optionalValue)
	{
		string text;
		if (this.TryGetValue(_className, _propName, out text))
		{
			optionalValue = StringParsers.ParseVector3i(text, 0, -1, false);
		}
	}

	// Token: 0x06009F3D RID: 40765 RVA: 0x003BE2FC File Offset: 0x003BC4FC
	public void ParseVec(string _propName, ref float optionalValue1, ref float optionalValue2)
	{
		string text;
		if (this.TryGetValue(_propName, out text))
		{
			Vector2 vector = StringParsers.ParseVector2(text);
			optionalValue1 = vector.x;
			optionalValue2 = vector.y;
		}
	}

	// Token: 0x06009F3E RID: 40766 RVA: 0x003BE32C File Offset: 0x003BC52C
	public void ParseVec(string _className, string _propName, ref float optionalValue1, ref float optionalValue2)
	{
		string text;
		if (this.TryGetValue(_className, _propName, out text))
		{
			Vector2 vector = StringParsers.ParseVector2(text);
			optionalValue1 = vector.x;
			optionalValue2 = vector.y;
		}
	}

	// Token: 0x06009F3F RID: 40767 RVA: 0x003BE360 File Offset: 0x003BC560
	public void ParseVec(string _propName, ref float optionalValue1, ref float optionalValue2, ref float optionalValue3)
	{
		string text;
		if (this.TryGetValue(_propName, out text))
		{
			Vector3 vector = StringParsers.ParseVector3(text, 0, -1);
			optionalValue1 = vector.x;
			optionalValue2 = vector.y;
			optionalValue3 = vector.z;
		}
	}

	// Token: 0x06009F40 RID: 40768 RVA: 0x003BE39C File Offset: 0x003BC59C
	public void ParseVec(string _className, string _propName, ref float optionalValue1, ref float optionalValue2, ref float optionalValue3)
	{
		string text;
		if (this.TryGetValue(_className, _propName, out text))
		{
			Vector3 vector = StringParsers.ParseVector3(text, 0, -1);
			optionalValue1 = vector.x;
			optionalValue2 = vector.y;
			optionalValue3 = vector.z;
		}
	}

	// Token: 0x06009F41 RID: 40769 RVA: 0x003BE3D8 File Offset: 0x003BC5D8
	public void ParseVec(string _propName, ref float optionalValue1, ref float optionalValue2, ref float optionalValue3, ref float optionalValue4)
	{
		string text;
		if (this.TryGetValue(_propName, out text))
		{
			Vector4 vector = StringParsers.ParseVector4(text);
			optionalValue1 = vector.x;
			optionalValue2 = vector.y;
			optionalValue3 = vector.z;
			optionalValue4 = vector.w;
		}
	}

	// Token: 0x06009F42 RID: 40770 RVA: 0x003BE41C File Offset: 0x003BC61C
	public void ParseVec(string _className, string _propName, ref float optionalValue1, ref float optionalValue2, ref float optionalValue3, ref float optionalValue4)
	{
		string text;
		if (this.TryGetValue(_className, _propName, out text))
		{
			Vector4 vector = StringParsers.ParseVector4(text);
			optionalValue1 = vector.x;
			optionalValue2 = vector.y;
			optionalValue3 = vector.z;
			optionalValue4 = vector.w;
		}
	}

	// Token: 0x06009F43 RID: 40771 RVA: 0x003BE460 File Offset: 0x003BC660
	public void CopyFrom(DynamicProperties _other, HashSet<string> _exclude = null)
	{
		DynamicProperties.copyDict(_other.Values, this.Values, _exclude);
		DynamicProperties.copyDict(_other.Params1, this.Params1, _exclude);
		DynamicProperties.copyDict(_other.Params2, this.Params2, _exclude);
		DynamicProperties.copyDict(_other.Data, this.Data, _exclude);
		foreach (KeyValuePair<string, DynamicProperties> keyValuePair in _other.Classes)
		{
			if (_exclude == null || !_exclude.Contains(keyValuePair.Key))
			{
				DynamicProperties dynamicProperties = (this.Classes.ContainsKey(keyValuePair.Key) ? this.Classes[keyValuePair.Key] : new DynamicProperties());
				this.Classes[keyValuePair.Key] = dynamicProperties;
				HashSet<string> nestedExclusions = DynamicProperties.GetNestedExclusions(keyValuePair.Key, _exclude);
				dynamicProperties.CopyFrom(keyValuePair.Value, nestedExclusions);
			}
		}
	}

	// Token: 0x06009F44 RID: 40772 RVA: 0x003BE564 File Offset: 0x003BC764
	[PublicizedFrom(EAccessModifier.Private)]
	public static HashSet<string> GetNestedExclusions(string _className, HashSet<string> _exclude)
	{
		if (_exclude == null)
		{
			return null;
		}
		HashSet<string> hashSet = null;
		string text = _className + ".";
		foreach (string text2 in _exclude)
		{
			if (text2.StartsWith(text))
			{
				if (hashSet == null)
				{
					hashSet = new HashSet<string>();
				}
				hashSet.Add(text2.Substring(text.Length));
			}
		}
		return hashSet;
	}

	// Token: 0x06009F45 RID: 40773 RVA: 0x003BE5E4 File Offset: 0x003BC7E4
	[PublicizedFrom(EAccessModifier.Private)]
	public static void copyDict(Dictionary<string, string> _source, Dictionary<string, string> _dest, HashSet<string> _exclude)
	{
		foreach (KeyValuePair<string, string> keyValuePair in _source)
		{
			if (DynamicProperties.copyKey(keyValuePair.Key, _exclude))
			{
				_dest[keyValuePair.Key] = keyValuePair.Value;
			}
		}
	}

	// Token: 0x06009F46 RID: 40774 RVA: 0x003BE650 File Offset: 0x003BC850
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool copyKey(string _key, HashSet<string> _exclude)
	{
		if (_exclude == null)
		{
			return true;
		}
		if (_exclude.Contains(_key))
		{
			return false;
		}
		if (_key.IndexOf('.') > 0)
		{
			foreach (string text in _exclude)
			{
				if (_key.StartsWith(text + "."))
				{
					return false;
				}
			}
			return true;
		}
		return true;
	}

	// Token: 0x06009F47 RID: 40775 RVA: 0x003BE6CC File Offset: 0x003BC8CC
	public string PrettyPrint()
	{
		StringBuilder stringBuilder = new StringBuilder();
		this.PrettyPrint(stringBuilder, "");
		return stringBuilder.ToString();
	}

	// Token: 0x06009F48 RID: 40776 RVA: 0x003BE6F4 File Offset: 0x003BC8F4
	[PublicizedFrom(EAccessModifier.Private)]
	public void PrettyPrint(StringBuilder sb, string indent)
	{
		sb.AppendFormat("{0}Properties:\n", indent);
		foreach (KeyValuePair<string, string> keyValuePair in this.Values.OrderBy<KeyValuePair<string, string>, string>((KeyValuePair<string, string> kvp) => kvp.Key))
		{
			sb.AppendFormat("{2}    name={0}, value={1}", keyValuePair.Key, this.Values[keyValuePair.Key], indent);
			if (this.Params1.ContainsKey(keyValuePair.Key))
			{
				sb.AppendFormat(", param1={0}", this.Params1[keyValuePair.Key]);
			}
			if (this.Params2.ContainsKey(keyValuePair.Key))
			{
				sb.AppendFormat(", param2={0}", this.Params2[keyValuePair.Key]);
			}
			if (this.Data.ContainsKey(keyValuePair.Key))
			{
				sb.AppendFormat(", fields={0}", this.Data[keyValuePair.Key]);
			}
			sb.AppendLine();
		}
		if (this.Classes.Count > 0)
		{
			sb.AppendFormat("{0}Classes:\n", indent);
			foreach (KeyValuePair<string, DynamicProperties> keyValuePair2 in this.Classes)
			{
				sb.AppendFormat("{1}    class={0}\n", keyValuePair2.Key, indent);
				keyValuePair2.Value.PrettyPrint(sb, string.Format("{0}    ", indent));
			}
		}
	}

	// Token: 0x06009F49 RID: 40777 RVA: 0x003BE8B8 File Offset: 0x003BCAB8
	[PublicizedFrom(EAccessModifier.Private)]
	static DynamicProperties()
	{
		DynamicProperties.RegisterFormatter();
	}

	// Token: 0x06009F4A RID: 40778 RVA: 0x003BE8E0 File Offset: 0x003BCAE0
	[Preserve]
	public static void RegisterFormatter()
	{
		if (!MemoryPackFormatterProvider.IsRegistered<DynamicProperties>())
		{
			MemoryPackFormatterProvider.Register<DynamicProperties>(new DynamicProperties.DynamicPropertiesFormatter());
		}
		if (!MemoryPackFormatterProvider.IsRegistered<DynamicProperties[]>())
		{
			MemoryPackFormatterProvider.Register<DynamicProperties[]>(new ArrayFormatter<DynamicProperties>());
		}
		if (!MemoryPackFormatterProvider.IsRegistered<Dictionary<string, string>>())
		{
			MemoryPackFormatterProvider.Register<Dictionary<string, string>>(new DictionaryFormatter<string, string>());
		}
		if (!MemoryPackFormatterProvider.IsRegistered<Dictionary<string, DynamicProperties>>())
		{
			MemoryPackFormatterProvider.Register<Dictionary<string, DynamicProperties>>(new DictionaryFormatter<string, DynamicProperties>());
		}
		if (!MemoryPackFormatterProvider.IsRegistered<Dictionary<string, List<Dictionary<string, string>>>>())
		{
			MemoryPackFormatterProvider.Register<Dictionary<string, List<Dictionary<string, string>>>>(new DictionaryFormatter<string, List<Dictionary<string, string>>>());
		}
		if (!MemoryPackFormatterProvider.IsRegistered<List<Dictionary<string, string>>>())
		{
			MemoryPackFormatterProvider.Register<List<Dictionary<string, string>>>(new ListFormatter<Dictionary<string, string>>());
		}
	}

	// Token: 0x06009F4B RID: 40779 RVA: 0x003BE954 File Offset: 0x003BCB54
	[NullableContext(2)]
	[Preserve]
	public static void Serialize(ref MemoryPackWriter writer, ref DynamicProperties value)
	{
		if (value == null)
		{
			writer.WriteNullObjectHeader();
			return;
		}
		writer.WriteObjectHeader(6);
		writer.WriteValue<Dictionary<string, string>>(in value.Values);
		writer.WriteValue<Dictionary<string, string>>(in value.Params1);
		writer.WriteValue<Dictionary<string, string>>(in value.Params2);
		writer.WriteValue<Dictionary<string, string>>(in value.Data);
		writer.WriteValue<Dictionary<string, DynamicProperties>>(in value.Classes);
		writer.WriteValue<Dictionary<string, List<Dictionary<string, string>>>>(in value.Array);
	}

	// Token: 0x06009F4C RID: 40780 RVA: 0x003BE9C4 File Offset: 0x003BCBC4
	[NullableContext(2)]
	[Preserve]
	public static void Deserialize(ref MemoryPackReader reader, ref DynamicProperties value)
	{
		byte b;
		if (!reader.TryReadObjectHeader(out b))
		{
			value = null;
			return;
		}
		Dictionary<string, string> dictionary;
		Dictionary<string, string> dictionary2;
		Dictionary<string, string> dictionary3;
		Dictionary<string, string> dictionary4;
		Dictionary<string, DynamicProperties> dictionary5;
		Dictionary<string, List<Dictionary<string, string>>> dictionary6;
		if (b == 6)
		{
			if (value == null)
			{
				dictionary = reader.ReadValue<Dictionary<string, string>>();
				dictionary2 = reader.ReadValue<Dictionary<string, string>>();
				dictionary3 = reader.ReadValue<Dictionary<string, string>>();
				dictionary4 = reader.ReadValue<Dictionary<string, string>>();
				dictionary5 = reader.ReadValue<Dictionary<string, DynamicProperties>>();
				dictionary6 = reader.ReadValue<Dictionary<string, List<Dictionary<string, string>>>>();
				goto IL_0194;
			}
			dictionary = value.Values;
			dictionary2 = value.Params1;
			dictionary3 = value.Params2;
			dictionary4 = value.Data;
			dictionary5 = value.Classes;
			dictionary6 = value.Array;
			reader.ReadValue<Dictionary<string, string>>(ref dictionary);
			reader.ReadValue<Dictionary<string, string>>(ref dictionary2);
			reader.ReadValue<Dictionary<string, string>>(ref dictionary3);
			reader.ReadValue<Dictionary<string, string>>(ref dictionary4);
			reader.ReadValue<Dictionary<string, DynamicProperties>>(ref dictionary5);
			reader.ReadValue<Dictionary<string, List<Dictionary<string, string>>>>(ref dictionary6);
		}
		else
		{
			if (b > 6)
			{
				MemoryPackSerializationException.ThrowInvalidPropertyCount(typeof(DynamicProperties), 6, b);
				return;
			}
			if (value == null)
			{
				dictionary = null;
				dictionary2 = null;
				dictionary3 = null;
				dictionary4 = null;
				dictionary5 = null;
				dictionary6 = null;
			}
			else
			{
				dictionary = value.Values;
				dictionary2 = value.Params1;
				dictionary3 = value.Params2;
				dictionary4 = value.Data;
				dictionary5 = value.Classes;
				dictionary6 = value.Array;
			}
			if (b != 0)
			{
				reader.ReadValue<Dictionary<string, string>>(ref dictionary);
				if (b != 1)
				{
					reader.ReadValue<Dictionary<string, string>>(ref dictionary2);
					if (b != 2)
					{
						reader.ReadValue<Dictionary<string, string>>(ref dictionary3);
						if (b != 3)
						{
							reader.ReadValue<Dictionary<string, string>>(ref dictionary4);
							if (b != 4)
							{
								reader.ReadValue<Dictionary<string, DynamicProperties>>(ref dictionary5);
								if (b != 5)
								{
									reader.ReadValue<Dictionary<string, List<Dictionary<string, string>>>>(ref dictionary6);
								}
							}
						}
					}
				}
			}
			if (value == null)
			{
				goto IL_0194;
			}
		}
		value.Values = dictionary;
		value.Params1 = dictionary2;
		value.Params2 = dictionary3;
		value.Data = dictionary4;
		value.Classes = dictionary5;
		value.Array = dictionary6;
		return;
		IL_0194:
		value = new DynamicProperties
		{
			Values = dictionary,
			Params1 = dictionary2,
			Params2 = dictionary3,
			Data = dictionary4,
			Classes = dictionary5,
			Array = dictionary6
		};
	}

	// Token: 0x04007821 RID: 30753
	[MemoryPackInclude]
	public Dictionary<string, string> Values = new Dictionary<string, string>();

	// Token: 0x04007822 RID: 30754
	[MemoryPackInclude]
	public Dictionary<string, string> Params1 = new Dictionary<string, string>();

	// Token: 0x04007823 RID: 30755
	[MemoryPackInclude]
	public Dictionary<string, string> Params2 = new Dictionary<string, string>();

	// Token: 0x04007824 RID: 30756
	[MemoryPackInclude]
	public Dictionary<string, string> Data = new Dictionary<string, string>();

	// Token: 0x04007825 RID: 30757
	[MemoryPackInclude]
	public Dictionary<string, DynamicProperties> Classes = new Dictionary<string, DynamicProperties>();

	// Token: 0x04007826 RID: 30758
	[MemoryPackInclude]
	public Dictionary<string, List<Dictionary<string, string>>> Array = new Dictionary<string, List<Dictionary<string, string>>>();

	// Token: 0x04007827 RID: 30759
	[PublicizedFrom(EAccessModifier.Private)]
	public const char PathAccessSeparator = '.';

	// Token: 0x04007828 RID: 30760
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly char[] semicolonSeparator = new char[] { ';' };

	// Token: 0x04007829 RID: 30761
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly char[] equalSeparator = new char[] { '=' };

	// Token: 0x020013D3 RID: 5075
	[NullableContext(1)]
	[Nullable(new byte[] { 0, 1 })]
	[Preserve]
	[PublicizedFrom(EAccessModifier.Private)]
	public sealed class DynamicPropertiesFormatter : MemoryPackFormatter<DynamicProperties>
	{
		// Token: 0x06009F4D RID: 40781 RVA: 0x003BEB99 File Offset: 0x003BCD99
		[Preserve]
		public override void Serialize(ref MemoryPackWriter writer, ref DynamicProperties value)
		{
			DynamicProperties.Serialize(ref writer, ref value);
		}

		// Token: 0x06009F4E RID: 40782 RVA: 0x003BEBA2 File Offset: 0x003BCDA2
		[Preserve]
		public override void Deserialize(ref MemoryPackReader reader, ref DynamicProperties value)
		{
			DynamicProperties.Deserialize(ref reader, ref value);
		}
	}
}
