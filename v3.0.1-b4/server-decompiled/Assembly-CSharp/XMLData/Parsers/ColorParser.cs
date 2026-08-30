using System;
using System.Text.RegularExpressions;
using UnityEngine;
using XMLData.Exceptions;

namespace XMLData.Parsers
{
	// Token: 0x02001635 RID: 5685
	public static class ColorParser
	{
		// Token: 0x0600B2D5 RID: 45781 RVA: 0x0042EAD4 File Offset: 0x0042CCD4
		public static Color Parse(string _value)
		{
			Match match = ColorParser.decimalMatcher.Match(_value);
			if (!match.Success)
			{
				return StringParsers.ParseHexColor(_value);
			}
			float num;
			if (!StringParsers.TryParseFloat(match.Groups[1].Value, out num))
			{
				throw new InvalidValueException("Expected float value as first part of Color field, found \"" + match.Groups[1].Value + "\"", -1);
			}
			if (num < 0f || num > 1f)
			{
				throw new InvalidValueException("Expected float between 0 and 1 as first part of Color field, found " + num.ToCultureInvariantString(), -1);
			}
			float num2;
			if (!StringParsers.TryParseFloat(match.Groups[2].Value, out num2))
			{
				throw new InvalidValueException("Expected float value as second part of Color field, found \"" + match.Groups[2].Value + "\"", -1);
			}
			if (num2 < 0f || num2 > 1f)
			{
				throw new InvalidValueException("Expected float between 0 and 1 as second part of Color field, found " + num2.ToCultureInvariantString(), -1);
			}
			float num3;
			if (!StringParsers.TryParseFloat(match.Groups[3].Value, out num3))
			{
				throw new InvalidValueException("Expected float value as third part of Color field, found \"" + match.Groups[3].Value + "\"", -1);
			}
			if (num3 < 0f || num3 > 1f)
			{
				throw new InvalidValueException("Expected float between 0 and 1 as third part of Color field, found " + num3.ToCultureInvariantString(), -1);
			}
			return new Color(num, num2, num3, 1f);
		}

		// Token: 0x0600B2D6 RID: 45782 RVA: 0x0042EC44 File Offset: 0x0042CE44
		public static string Unparse(Color _value)
		{
			return string.Format("{0},{1},{2}", _value.r.ToCultureInvariantString(), _value.g.ToCultureInvariantString(), _value.b.ToCultureInvariantString());
		}

		// Token: 0x04008612 RID: 34322
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly Regex decimalMatcher = new Regex("^\\s*(\\d*\\.\\d+)\\s*,\\s*(\\d*\\.\\d+)\\s*,\\s*(\\d*\\.\\d+)\\s*$");
	}
}
