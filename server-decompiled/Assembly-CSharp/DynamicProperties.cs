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

// Token: 0x020013ED RID: 5101
[MemoryPackable(GenerateType.Object)]
public class DynamicProperties : IMemoryPackable<DynamicProperties>, IMemoryPackFormatterRegister
{
	// Token: 0x06009FFD RID: 40957 RVA: 0x003C25FC File Offset: 0x003C07FC
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

	// Token: 0x06009FFE RID: 40958 RVA: 0x003C2654 File Offset: 0x003C0854
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

	// Token: 0x0600A000 RID: 40960 RVA: 0x003C275D File Offset: 0x003C095D
	[PublicizedFrom(EAccessModifier.Private)]
	public static void ValidateKey(string _key)
	{
		if (string.IsNullOrEmpty(_key) || _key.IndexOf('.') >= 0)
		{
			throw new Exception(string.Format("Property name '{0}' contains illegal character '{1}'", _key, '.'));
		}
	}

	// Token: 0x0600A001 RID: 40961 RVA: 0x003C2794 File Offset: 0x003C0994
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

	// Token: 0x0600A002 RID: 40962 RVA: 0x003C27CC File Offset: 0x003C09CC
	public DynamicProperties GetClass(string _className)
	{
		DynamicProperties dynamicProperties;
		if (this.Classes.TryGetValue(_className, out dynamicProperties))
		{
			return dynamicProperties;
		}
		return null;
	}

	// Token: 0x0600A003 RID: 40963 RVA: 0x003C27EC File Offset: 0x003C09EC
	public void SetValue(string _propName, string _value)
	{
		DynamicProperties.ValidateKey(_propName);
		this.Values[_propName] = _value;
	}

	// Token: 0x0600A004 RID: 40964 RVA: 0x003C2801 File Offset: 0x003C0A01
	public void SetValue(string _className, string _propName, string _value)
	{
		this.GetOrCreateClass(_className).SetValue(_propName, _value);
	}

	// Token: 0x0600A005 RID: 40965 RVA: 0x003C2814 File Offset: 0x003C0A14
	public string GetValue(string _propName)
	{
		string text;
		if (this.Values.TryGetValue(_propName, out text))
		{
			return text;
		}
		return null;
	}

	// Token: 0x0600A006 RID: 40966 RVA: 0x003C2834 File Offset: 0x003C0A34
	public bool TryGetValue(string _propName, out string _value)
	{
		DynamicProperties.ValidateKey(_propName);
		return this.Values.TryGetValue(_propName, out _value);
	}

	// Token: 0x0600A007 RID: 40967 RVA: 0x003C284C File Offset: 0x003C0A4C
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

	// Token: 0x0600A008 RID: 40968 RVA: 0x003C287C File Offset: 0x003C0A7C
	public bool Contains(string _propName)
	{
		DynamicProperties.ValidateKey(_propName);
		return this.Values.ContainsKey(_propName);
	}

	// Token: 0x0600A009 RID: 40969 RVA: 0x003C2890 File Offset: 0x003C0A90
	public bool Contains(string _className, string _propName)
	{
		string text;
		return this.TryGetValue(_className, _propName, out text);
	}

	// Token: 0x0600A00A RID: 40970 RVA: 0x003C28A8 File Offset: 0x003C0AA8
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

	// Token: 0x0600A00B RID: 40971 RVA: 0x003C28FF File Offset: 0x003C0AFF
	public void SetParam1(string _className, string _propName, string _param1)
	{
		this.GetOrCreateClass(_className).SetParam1(_propName, _param1);
	}

	// Token: 0x0600A00C RID: 40972 RVA: 0x003C290F File Offset: 0x003C0B0F
	public bool TryGetParam1(string _propName, out string _param1)
	{
		DynamicProperties.ValidateKey(_propName);
		return this.Params1.TryGetValue(_propName, out _param1);
	}

	// Token: 0x0600A00D RID: 40973 RVA: 0x003C2924 File Offset: 0x003C0B24
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

	// Token: 0x0600A00E RID: 40974 RVA: 0x003C2954 File Offset: 0x003C0B54
	public string GetParam1(string _propName)
	{
		string text;
		if (this.Params1.TryGetValue(_propName, out text))
		{
			return text;
		}
		return null;
	}

	// Token: 0x0600A00F RID: 40975 RVA: 0x003C2974 File Offset: 0x003C0B74
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

	// Token: 0x0600A010 RID: 40976 RVA: 0x003C2998 File Offset: 0x003C0B98
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

	// Token: 0x0600A011 RID: 40977 RVA: 0x003C29C0 File Offset: 0x003C0BC0
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

	// Token: 0x0600A012 RID: 40978 RVA: 0x003C29E8 File Offset: 0x003C0BE8
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

	// Token: 0x0600A013 RID: 40979 RVA: 0x003C2A14 File Offset: 0x003C0C14
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

	// Token: 0x0600A014 RID: 40980 RVA: 0x003C2A38 File Offset: 0x003C0C38
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

	// Token: 0x0600A015 RID: 40981 RVA: 0x003C2A60 File Offset: 0x003C0C60
	public string GetString(string _propName)
	{
		string text;
		if (!this.TryGetValue(_propName, out text))
		{
			return string.Empty;
		}
		return text;
	}

	// Token: 0x0600A016 RID: 40982 RVA: 0x003C2A80 File Offset: 0x003C0C80
	public string GetString(string _className, string _propName)
	{
		string text;
		if (!this.TryGetValue(_className, _propName, out text))
		{
			return string.Empty;
		}
		return text;
	}

	// Token: 0x0600A017 RID: 40983 RVA: 0x003C2AA0 File Offset: 0x003C0CA0
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

	// Token: 0x0600A018 RID: 40984 RVA: 0x003C2B20 File Offset: 0x003C0D20
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

	// Token: 0x0600A019 RID: 40985 RVA: 0x003C2B90 File Offset: 0x003C0D90
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

	// Token: 0x0600A01A RID: 40986 RVA: 0x003C2BFC File Offset: 0x003C0DFC
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

	// Token: 0x0600A01B RID: 40987 RVA: 0x003C2E04 File Offset: 0x003C1004
	public void Add(XElement _propertyNode, bool _doValueReplace = false)
	{
		this.Parse(_propertyNode, _doValueReplace);
	}

	// Token: 0x0600A01C RID: 40988 RVA: 0x003C2E10 File Offset: 0x003C1010
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

	// Token: 0x0600A01D RID: 40989 RVA: 0x003C2F00 File Offset: 0x003C1100
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

	// Token: 0x0600A01E RID: 40990 RVA: 0x003C3080 File Offset: 0x003C1280
	public void Clear()
	{
		this.Values.Clear();
		this.Params1.Clear();
		this.Params2.Clear();
		this.Data.Clear();
		this.Classes.Clear();
	}

	// Token: 0x0600A01F RID: 40991 RVA: 0x003C30BC File Offset: 0x003C12BC
	public void ParseString(string _propName, ref string optionalValue)
	{
		string text;
		if (this.TryGetValue(_propName, out text))
		{
			optionalValue = text;
		}
	}

	// Token: 0x0600A020 RID: 40992 RVA: 0x003C30D8 File Offset: 0x003C12D8
	public void ParseString(string _className, string _propName, ref string optionalValue)
	{
		string text;
		if (this.TryGetValue(_className, _propName, out text))
		{
			optionalValue = text;
		}
	}

	// Token: 0x0600A021 RID: 40993 RVA: 0x003C30F4 File Offset: 0x003C12F4
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

	// Token: 0x0600A022 RID: 40994 RVA: 0x003C3198 File Offset: 0x003C1398
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

	// Token: 0x0600A023 RID: 40995 RVA: 0x003C31E4 File Offset: 0x003C13E4
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

	// Token: 0x0600A024 RID: 40996 RVA: 0x003C3230 File Offset: 0x003C1430
	public string GetLocalizedString(string _propName)
	{
		string text;
		if (this.TryGetValue(_propName, out text))
		{
			return text;
		}
		return string.Empty;
	}

	// Token: 0x0600A025 RID: 40997 RVA: 0x003C3250 File Offset: 0x003C1450
	public void ParseLocalizedString(string _propName, ref string optionalValue)
	{
		string text;
		if (this.TryGetValue(_propName, out text))
		{
			optionalValue = Localization.Get(text, false, null);
		}
	}

	// Token: 0x0600A026 RID: 40998 RVA: 0x003C3274 File Offset: 0x003C1474
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

	// Token: 0x0600A027 RID: 40999 RVA: 0x003C32B4 File Offset: 0x003C14B4
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

	// Token: 0x0600A028 RID: 41000 RVA: 0x003C32F4 File Offset: 0x003C14F4
	public void ParseColor(string _propName, ref Color optionalValue)
	{
		string text;
		if (this.TryGetValue(_propName, out text))
		{
			optionalValue = StringParsers.ParseColor32(text);
		}
	}

	// Token: 0x0600A029 RID: 41001 RVA: 0x003C3318 File Offset: 0x003C1518
	public void ParseColorHex(string _propName, ref Color optionalValue)
	{
		string text;
		if (this.TryGetValue(_propName, out text))
		{
			optionalValue = StringParsers.ParseHexColor(text);
		}
	}

	// Token: 0x0600A02A RID: 41002 RVA: 0x003C333C File Offset: 0x003C153C
	public void ParseEnum<T>(string _propName, ref T optionalValue) where T : struct, IConvertible
	{
		string text;
		T t;
		if (this.TryGetValue(_propName, out text) && EnumUtils.TryParse<T>(text, out t, true))
		{
			optionalValue = t;
		}
	}

	// Token: 0x0600A02B RID: 41003 RVA: 0x003C3368 File Offset: 0x003C1568
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

	// Token: 0x0600A02C RID: 41004 RVA: 0x003C33A8 File Offset: 0x003C15A8
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

	// Token: 0x0600A02D RID: 41005 RVA: 0x003C33E4 File Offset: 0x003C15E4
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

	// Token: 0x0600A02E RID: 41006 RVA: 0x003C3430 File Offset: 0x003C1630
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

	// Token: 0x0600A02F RID: 41007 RVA: 0x003C3470 File Offset: 0x003C1670
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

	// Token: 0x0600A030 RID: 41008 RVA: 0x003C34B4 File Offset: 0x003C16B4
	public void ParseVec(string _propName, ref Vector2 optionalValue)
	{
		string text;
		if (this.TryGetValue(_propName, out text))
		{
			optionalValue = StringParsers.ParseVector2(text);
		}
	}

	// Token: 0x0600A031 RID: 41009 RVA: 0x003C34D8 File Offset: 0x003C16D8
	public void ParseVec(string _className, string _propName, ref Vector2 optionalValue)
	{
		string text;
		if (this.TryGetValue(_className, _propName, out text))
		{
			optionalValue = StringParsers.ParseVector2(text);
		}
	}

	// Token: 0x0600A032 RID: 41010 RVA: 0x003C3500 File Offset: 0x003C1700
	public void ParseVec(string _propName, ref Vector2 optionalValue, float _defaultValue)
	{
		string text;
		if (this.TryGetValue(_propName, out text))
		{
			optionalValue = StringParsers.ParseVector2(text, _defaultValue);
		}
	}

	// Token: 0x0600A033 RID: 41011 RVA: 0x003C3528 File Offset: 0x003C1728
	public void ParseVec(string _className, string _propName, ref Vector2 optionalValue, float _defaultValue)
	{
		string text;
		if (this.TryGetValue(_className, _propName, out text))
		{
			optionalValue = StringParsers.ParseVector2(text, _defaultValue);
		}
	}

	// Token: 0x0600A034 RID: 41012 RVA: 0x003C3550 File Offset: 0x003C1750
	public void ParseVec(string _propName, ref Vector3 optionalValue)
	{
		string text;
		if (this.TryGetValue(_propName, out text))
		{
			optionalValue = StringParsers.ParseVector3(text, 0, -1);
		}
	}

	// Token: 0x0600A035 RID: 41013 RVA: 0x003C3578 File Offset: 0x003C1778
	public void ParseVec(string _className, string _propName, ref Vector3 optionalValue)
	{
		string text;
		if (this.TryGetValue(_className, _propName, out text))
		{
			optionalValue = StringParsers.ParseVector3(text, 0, -1);
		}
	}

	// Token: 0x0600A036 RID: 41014 RVA: 0x003C35A0 File Offset: 0x003C17A0
	public void ParseVec(string _propName, ref Vector3 optionalValue, float _defaultValue)
	{
		string text;
		if (this.TryGetValue(_propName, out text))
		{
			optionalValue = StringParsers.ParseVector3(text, _defaultValue);
		}
	}

	// Token: 0x0600A037 RID: 41015 RVA: 0x003C35C8 File Offset: 0x003C17C8
	public void ParseVec(string _className, string _propName, ref Vector3 optionalValue, float _defaultValue)
	{
		string text;
		if (this.TryGetValue(_className, _propName, out text))
		{
			optionalValue = StringParsers.ParseVector3(text, _defaultValue);
		}
	}

	// Token: 0x0600A038 RID: 41016 RVA: 0x003C35F0 File Offset: 0x003C17F0
	public void ParseVec(string _propName, ref Vector3i optionalValue)
	{
		string text;
		if (this.TryGetValue(_propName, out text))
		{
			optionalValue = StringParsers.ParseVector3i(text, 0, -1, false);
		}
	}

	// Token: 0x0600A039 RID: 41017 RVA: 0x003C3618 File Offset: 0x003C1818
	public void ParseVec(string _className, string _propName, ref Vector3i optionalValue)
	{
		string text;
		if (this.TryGetValue(_className, _propName, out text))
		{
			optionalValue = StringParsers.ParseVector3i(text, 0, -1, false);
		}
	}

	// Token: 0x0600A03A RID: 41018 RVA: 0x003C3640 File Offset: 0x003C1840
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

	// Token: 0x0600A03B RID: 41019 RVA: 0x003C3670 File Offset: 0x003C1870
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

	// Token: 0x0600A03C RID: 41020 RVA: 0x003C36A4 File Offset: 0x003C18A4
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

	// Token: 0x0600A03D RID: 41021 RVA: 0x003C36E0 File Offset: 0x003C18E0
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

	// Token: 0x0600A03E RID: 41022 RVA: 0x003C371C File Offset: 0x003C191C
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

	// Token: 0x0600A03F RID: 41023 RVA: 0x003C3760 File Offset: 0x003C1960
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

	// Token: 0x0600A040 RID: 41024 RVA: 0x003C37A4 File Offset: 0x003C19A4
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

	// Token: 0x0600A041 RID: 41025 RVA: 0x003C38A8 File Offset: 0x003C1AA8
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

	// Token: 0x0600A042 RID: 41026 RVA: 0x003C3928 File Offset: 0x003C1B28
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

	// Token: 0x0600A043 RID: 41027 RVA: 0x003C3994 File Offset: 0x003C1B94
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

	// Token: 0x0600A044 RID: 41028 RVA: 0x003C3A10 File Offset: 0x003C1C10
	public string PrettyPrint()
	{
		StringBuilder stringBuilder = new StringBuilder();
		this.PrettyPrint(stringBuilder, "");
		return stringBuilder.ToString();
	}

	// Token: 0x0600A045 RID: 41029 RVA: 0x003C3A38 File Offset: 0x003C1C38
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

	// Token: 0x0600A046 RID: 41030 RVA: 0x003C3BFC File Offset: 0x003C1DFC
	[PublicizedFrom(EAccessModifier.Private)]
	static DynamicProperties()
	{
		DynamicProperties.RegisterFormatter();
	}

	// Token: 0x0600A047 RID: 41031 RVA: 0x003C3C24 File Offset: 0x003C1E24
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

	// Token: 0x0600A048 RID: 41032 RVA: 0x003C3C98 File Offset: 0x003C1E98
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

	// Token: 0x0600A049 RID: 41033 RVA: 0x003C3D08 File Offset: 0x003C1F08
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

	// Token: 0x040078FA RID: 30970
	[MemoryPackInclude]
	public Dictionary<string, string> Values = new Dictionary<string, string>();

	// Token: 0x040078FB RID: 30971
	[MemoryPackInclude]
	public Dictionary<string, string> Params1 = new Dictionary<string, string>();

	// Token: 0x040078FC RID: 30972
	[MemoryPackInclude]
	public Dictionary<string, string> Params2 = new Dictionary<string, string>();

	// Token: 0x040078FD RID: 30973
	[MemoryPackInclude]
	public Dictionary<string, string> Data = new Dictionary<string, string>();

	// Token: 0x040078FE RID: 30974
	[MemoryPackInclude]
	public Dictionary<string, DynamicProperties> Classes = new Dictionary<string, DynamicProperties>();

	// Token: 0x040078FF RID: 30975
	[MemoryPackInclude]
	public Dictionary<string, List<Dictionary<string, string>>> Array = new Dictionary<string, List<Dictionary<string, string>>>();

	// Token: 0x04007900 RID: 30976
	[PublicizedFrom(EAccessModifier.Private)]
	public const char PathAccessSeparator = '.';

	// Token: 0x04007901 RID: 30977
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly char[] semicolonSeparator = new char[] { ';' };

	// Token: 0x04007902 RID: 30978
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly char[] equalSeparator = new char[] { '=' };

	// Token: 0x020013EE RID: 5102
	[NullableContext(1)]
	[Nullable(new byte[] { 0, 1 })]
	[Preserve]
	[PublicizedFrom(EAccessModifier.Private)]
	public sealed class DynamicPropertiesFormatter : MemoryPackFormatter<DynamicProperties>
	{
		// Token: 0x0600A04A RID: 41034 RVA: 0x003C3EDD File Offset: 0x003C20DD
		[Preserve]
		public override void Serialize(ref MemoryPackWriter writer, ref DynamicProperties value)
		{
			DynamicProperties.Serialize(ref writer, ref value);
		}

		// Token: 0x0600A04B RID: 41035 RVA: 0x003C3EE6 File Offset: 0x003C20E6
		[Preserve]
		public override void Deserialize(ref MemoryPackReader reader, ref DynamicProperties value)
		{
			DynamicProperties.Deserialize(ref reader, ref value);
		}
	}
}
