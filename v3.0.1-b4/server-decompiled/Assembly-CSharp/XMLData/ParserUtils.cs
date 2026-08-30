using System;
using System.Globalization;
using System.Xml;
using System.Xml.Linq;
using ICSharpCode.WpfDesign.XamlDom;
using XMLData.Exceptions;

namespace XMLData
{
	// Token: 0x02001619 RID: 5657
	public static class ParserUtils
	{
		// Token: 0x0600B17B RID: 45435 RVA: 0x00422DD4 File Offset: 0x00420FD4
		[PublicizedFrom(EAccessModifier.Private)]
		static ParserUtils()
		{
			ParserUtils.ci.NumberFormat.CurrencyDecimalSeparator = ".";
			ParserUtils.ci.NumberFormat.NumberDecimalSeparator = ".";
			ParserUtils.ci.NumberFormat.NumberGroupSeparator = ",";
		}

		// Token: 0x0600B17C RID: 45436 RVA: 0x00422E31 File Offset: 0x00421031
		public static bool TryParseFloat(string _s, out float _result)
		{
			return float.TryParse(_s, NumberStyles.Any, ParserUtils.ci, out _result);
		}

		// Token: 0x0600B17D RID: 45437 RVA: 0x00422E44 File Offset: 0x00421044
		public static bool TryParseDouble(string _s, out double _result)
		{
			return double.TryParse(_s, NumberStyles.Any, ParserUtils.ci, out _result);
		}

		// Token: 0x0600B17E RID: 45438 RVA: 0x00422E58 File Offset: 0x00421058
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

		// Token: 0x0600B17F RID: 45439 RVA: 0x00422EEC File Offset: 0x004210EC
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

		// Token: 0x0600B180 RID: 45440 RVA: 0x00422F4C File Offset: 0x0042114C
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

		// Token: 0x0600B181 RID: 45441 RVA: 0x00422FC0 File Offset: 0x004211C0
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

		// Token: 0x0600B182 RID: 45442 RVA: 0x00423048 File Offset: 0x00421248
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

		// Token: 0x04008543 RID: 34115
		public static readonly CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();

		// Token: 0x0200161A RID: 5658
		// (Invoke) Token: 0x0600B184 RID: 45444
		public delegate bool ConverterDelegate<T>(string _in, out T _out);
	}
}
