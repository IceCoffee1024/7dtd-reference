using System;
using System.Globalization;
using System.Xml;
using System.Xml.Linq;
using ICSharpCode.WpfDesign.XamlDom;
using XMLData.Exceptions;

namespace XMLData
{
	// Token: 0x02001653 RID: 5715
	public static class ParserUtils
	{
		// Token: 0x0600B30A RID: 45834 RVA: 0x0042BDF0 File Offset: 0x00429FF0
		[PublicizedFrom(EAccessModifier.Private)]
		static ParserUtils()
		{
			ParserUtils.ci.NumberFormat.CurrencyDecimalSeparator = ".";
			ParserUtils.ci.NumberFormat.NumberDecimalSeparator = ".";
			ParserUtils.ci.NumberFormat.NumberGroupSeparator = ",";
		}

		// Token: 0x0600B30B RID: 45835 RVA: 0x0042BE4D File Offset: 0x0042A04D
		public static bool TryParseFloat(string _s, out float _result)
		{
			return float.TryParse(_s, NumberStyles.Any, ParserUtils.ci, out _result);
		}

		// Token: 0x0600B30C RID: 45836 RVA: 0x0042BE60 File Offset: 0x0042A060
		public static bool TryParseDouble(string _s, out double _result)
		{
			return double.TryParse(_s, NumberStyles.Any, ParserUtils.ci, out _result);
		}

		// Token: 0x0600B30D RID: 45837 RVA: 0x0042BE74 File Offset: 0x0042A074
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

		// Token: 0x0600B30E RID: 45838 RVA: 0x0042BF08 File Offset: 0x0042A108
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

		// Token: 0x0600B30F RID: 45839 RVA: 0x0042BF68 File Offset: 0x0042A168
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

		// Token: 0x0600B310 RID: 45840 RVA: 0x0042BFDC File Offset: 0x0042A1DC
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

		// Token: 0x0600B311 RID: 45841 RVA: 0x0042C064 File Offset: 0x0042A264
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

		// Token: 0x04008672 RID: 34418
		public static readonly CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();

		// Token: 0x02001654 RID: 5716
		// (Invoke) Token: 0x0600B313 RID: 45843
		public delegate bool ConverterDelegate<T>(string _in, out T _out);
	}
}
