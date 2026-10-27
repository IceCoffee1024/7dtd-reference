using System;
using System.Globalization;
using System.Xml;
using System.Xml.Linq;
using ICSharpCode.WpfDesign.XamlDom;
using XMLData.Exceptions;

namespace XMLData
{
	// Token: 0x02001665 RID: 5733
	public static class ParserUtils
	{
		// Token: 0x0600B362 RID: 45922 RVA: 0x0042E1F4 File Offset: 0x0042C3F4
		[PublicizedFrom(EAccessModifier.Private)]
		static ParserUtils()
		{
			ParserUtils.ci.NumberFormat.CurrencyDecimalSeparator = ".";
			ParserUtils.ci.NumberFormat.NumberDecimalSeparator = ".";
			ParserUtils.ci.NumberFormat.NumberGroupSeparator = ",";
		}

		// Token: 0x0600B363 RID: 45923 RVA: 0x0042E251 File Offset: 0x0042C451
		public static bool TryParseFloat(string _s, out float _result)
		{
			return float.TryParse(_s, NumberStyles.Any, ParserUtils.ci, out _result);
		}

		// Token: 0x0600B364 RID: 45924 RVA: 0x0042E264 File Offset: 0x0042C464
		public static bool TryParseDouble(string _s, out double _result)
		{
			return double.TryParse(_s, NumberStyles.Any, ParserUtils.ci, out _result);
		}

		// Token: 0x0600B365 RID: 45925 RVA: 0x0042E278 File Offset: 0x0042C478
		public static bool ParseBoolAttribute(PositionXmlElement _elem, string _attrName, bool _mandatory, bool _defaultValue)
		{
			if (_elem.HasAttribute(_attrName))
			{
				string attribute = _elem.GetAttribute(_attrName);
				if (attribute == "true")
				{
					return true;
				}
				if (attribute == "false")
				{
					return false;
				}
				throw new InvalidValueException(string.Concat(new string[] { "Element has invalid value \"", attribute, "\" for bool attribute \"", _attrName, "\"" }), _elem.LineNumber);
			}
			else
			{
				if (_mandatory)
				{
					throw new MissingAttributeException("Element \"\" + _elem.Name + \"\" is missing required attribute \"" + _attrName + "\"", _elem.LineNumber);
				}
				return _defaultValue;
			}
		}

		// Token: 0x0600B366 RID: 45926 RVA: 0x0042E30C File Offset: 0x0042C50C
		public static string ParseStringAttribute(PositionXmlElement _elem, string _attrName, bool _mandatory, string _defaultValue = null)
		{
			if (_elem.HasAttribute(_attrName))
			{
				return _elem.GetAttribute(_attrName);
			}
			if (_mandatory)
			{
				throw new MissingAttributeException(string.Concat(new string[] { "Element \"", _elem.Name, "\" is missing required attribute \"", _attrName, "\"" }), _elem.LineNumber);
			}
			return _defaultValue;
		}

		// Token: 0x0600B367 RID: 45927 RVA: 0x0042E36C File Offset: 0x0042C56C
		public static string ParseStringAttribute(XElement _elem, string _attrName, bool _mandatory, string _defaultValue = null)
		{
			if (_elem.HasAttribute(_attrName))
			{
				return _elem.GetAttribute(_attrName);
			}
			if (_mandatory)
			{
				string[] array = new string[5];
				array[0] = "Element \"";
				int num = 1;
				XName name = _elem.Name;
				array[num] = ((name != null) ? name.ToString() : null);
				array[2] = "\" is missing required attribute \"";
				array[3] = _attrName;
				array[4] = "\"";
				throw new MissingAttributeException(string.Concat(array), ((IXmlLineInfo)_elem).LineNumber);
			}
			return _defaultValue;
		}

		// Token: 0x0600B368 RID: 45928 RVA: 0x0042E3E0 File Offset: 0x0042C5E0
		public static int ParseIntAttribute(PositionXmlElement _elem, string _attrName, bool _mandatory, int _defaultValue = 0)
		{
			if (_elem.HasAttribute(_attrName))
			{
				int num;
				if (int.TryParse(_elem.GetAttribute(_attrName), out num))
				{
					return num;
				}
				throw new InvalidValueException(string.Concat(new string[]
				{
					"Element has invalid value \"",
					num.ToString(),
					"\" for int attribute \"",
					_attrName,
					"\""
				}), _elem.LineNumber);
			}
			else
			{
				if (_mandatory)
				{
					throw new MissingAttributeException("Element \"\" + _elem.Name + \"\" is missing required attribute \"" + _attrName + "\"", _elem.LineNumber);
				}
				return _defaultValue;
			}
		}

		// Token: 0x0600B369 RID: 45929 RVA: 0x0042E468 File Offset: 0x0042C668
		public static void ParseRangeAttribute<T>(PositionXmlElement _elem, string _attrName, bool _mandatory, ref Range<T> _defaultAndOutput, bool _allowFixedValue, ParserUtils.ConverterDelegate<T> _converter)
		{
			if (!_elem.HasAttribute(_attrName))
			{
				if (_mandatory)
				{
					throw new MissingAttributeException("Element \"\" + _elem.Name + \"\" is missing required attribute \"" + _attrName + "\"", _elem.LineNumber);
				}
				return;
			}
			else
			{
				string attribute = _elem.GetAttribute(_attrName);
				string[] array = attribute.Split('-', StringSplitOptions.None);
				Range<T> range = new Range<T>();
				if (array.Length > 2)
				{
					throw new InvalidValueException(string.Concat(new string[] { "Element has invalid value \"", attribute, "\" for range attribute \"", _attrName, "\" with more than one separator \"-\"" }), _elem.LineNumber);
				}
				if (!_allowFixedValue && array.Length < 2)
				{
					throw new InvalidValueException(string.Concat(new string[] { "Element has invalid value \"", attribute, "\" for range attribute \"", _attrName, "\" with less than one separator \"-\"" }), _elem.LineNumber);
				}
				if (array[0] != "*")
				{
					T t;
					if (!_converter(array[0], out t))
					{
						throw new InvalidValueException(string.Concat(new string[]
						{
							"Element has invalid min range value \"",
							array[0],
							"\" for attribute \"",
							_attrName,
							"\""
						}), _elem.LineNumber);
					}
					range.min = t;
					range.hasMin = true;
				}
				if (array.Length > 1)
				{
					if (array[1] != "*")
					{
						T t2;
						if (!_converter(array[1], out t2))
						{
							throw new InvalidValueException(string.Concat(new string[]
							{
								"Element has invalid max range value \"",
								array[1],
								"\" for attribute \"",
								_attrName,
								"\""
							}), _elem.LineNumber);
						}
						range.max = t2;
						range.hasMax = true;
					}
				}
				else
				{
					range.hasMax = range.hasMin;
					range.max = range.min;
				}
				_defaultAndOutput = range;
				return;
			}
		}

		// Token: 0x040086B3 RID: 34483
		public static readonly CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();

		// Token: 0x02001666 RID: 5734
		// (Invoke) Token: 0x0600B36B RID: 45931
		public delegate bool ConverterDelegate<T>(string _in, out T _out);
	}
}
