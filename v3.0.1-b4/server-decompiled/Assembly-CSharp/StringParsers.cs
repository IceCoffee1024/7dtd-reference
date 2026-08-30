using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001495 RID: 5269
[XuiXmlAttributeConvertersClass]
[Preserve]
public static class StringParsers
{
	// Token: 0x0600A52B RID: 42283 RVA: 0x003D775C File Offset: 0x003D595C
	public static sbyte ParseSInt8(string _input, int _startIndex = 0, int _endIndex = -1, NumberStyles _style = NumberStyles.Integer)
	{
		long num;
		ulong num2;
		Exception ex;
		if (!StringParsers.internalParseInt64(_input, _style, false, out num, out num2, out ex, true, _startIndex, _endIndex))
		{
			throw ex;
		}
		if (num < -128L || num > 127L)
		{
			throw new OverflowException("Value too large or too small (input='" + _input + "')");
		}
		return (sbyte)num;
	}

	// Token: 0x0600A52C RID: 42284 RVA: 0x003D77A4 File Offset: 0x003D59A4
	public static byte ParseUInt8(string _input, int _startIndex = 0, int _endIndex = -1, NumberStyles _style = NumberStyles.Integer)
	{
		long num;
		ulong num2;
		Exception ex;
		if (!StringParsers.internalParseInt64(_input, _style, false, out num, out num2, out ex, false, _startIndex, _endIndex))
		{
			throw ex;
		}
		if (num2 > 255UL)
		{
			throw new OverflowException("Value too large or too small (input='" + _input + "')");
		}
		return (byte)num2;
	}

	// Token: 0x0600A52D RID: 42285 RVA: 0x003D77E8 File Offset: 0x003D59E8
	public static bool TryParseSInt8(string _input, out sbyte _result, int _startIndex, int _endIndex = -1, NumberStyles _style = NumberStyles.Integer)
	{
		long num;
		ulong num2;
		Exception ex;
		if (!StringParsers.internalParseInt64(_input, _style, true, out num, out num2, out ex, true, _startIndex, _endIndex))
		{
			_result = 0;
			return false;
		}
		if (num < -128L || num > 127L)
		{
			_result = 0;
			return false;
		}
		_result = (sbyte)num;
		return true;
	}

	// Token: 0x0600A52E RID: 42286 RVA: 0x003D7824 File Offset: 0x003D5A24
	public static bool TryParseUInt8(string _input, out byte _result, int _startIndex, int _endIndex = -1, NumberStyles _style = NumberStyles.Integer)
	{
		long num;
		ulong num2;
		Exception ex;
		if (!StringParsers.internalParseInt64(_input, _style, true, out num, out num2, out ex, false, _startIndex, _endIndex))
		{
			_result = 0;
			return false;
		}
		if (num2 > 255UL)
		{
			_result = 0;
			return false;
		}
		_result = (byte)num2;
		return true;
	}

	// Token: 0x0600A52F RID: 42287 RVA: 0x003D785D File Offset: 0x003D5A5D
	[XuiXmlAttributeConverter]
	public static bool TryParseSInt8(string _input, out sbyte _result)
	{
		return StringParsers.TryParseSInt8(_input, out _result, 0, -1, NumberStyles.Integer);
	}

	// Token: 0x0600A530 RID: 42288 RVA: 0x003D7869 File Offset: 0x003D5A69
	[XuiXmlAttributeConverter]
	public static bool TryParseUInt8(string _input, out byte _result)
	{
		return StringParsers.TryParseUInt8(_input, out _result, 0, -1, NumberStyles.Integer);
	}

	// Token: 0x0600A531 RID: 42289 RVA: 0x003D7878 File Offset: 0x003D5A78
	public static short ParseSInt16(string _input, int _startIndex = 0, int _endIndex = -1, NumberStyles _style = NumberStyles.Integer)
	{
		long num;
		ulong num2;
		Exception ex;
		if (!StringParsers.internalParseInt64(_input, _style, false, out num, out num2, out ex, true, _startIndex, _endIndex))
		{
			throw ex;
		}
		if (num < -32768L || num > 32767L)
		{
			throw new OverflowException("Value too large or too small (input='" + _input + "')");
		}
		return (short)num;
	}

	// Token: 0x0600A532 RID: 42290 RVA: 0x003D78C4 File Offset: 0x003D5AC4
	public static ushort ParseUInt16(string _input, int _startIndex = 0, int _endIndex = -1, NumberStyles _style = NumberStyles.Integer)
	{
		long num;
		ulong num2;
		Exception ex;
		if (!StringParsers.internalParseInt64(_input, _style, false, out num, out num2, out ex, false, _startIndex, _endIndex))
		{
			throw ex;
		}
		if (num2 > 65535UL)
		{
			throw new OverflowException("Value too large or too small (input='" + _input + "')");
		}
		return (ushort)num2;
	}

	// Token: 0x0600A533 RID: 42291 RVA: 0x003D7908 File Offset: 0x003D5B08
	public static bool TryParseSInt16(string _input, out short _result, int _startIndex, int _endIndex = -1, NumberStyles _style = NumberStyles.Integer)
	{
		long num;
		ulong num2;
		Exception ex;
		if (!StringParsers.internalParseInt64(_input, _style, true, out num, out num2, out ex, true, _startIndex, _endIndex))
		{
			_result = 0;
			return false;
		}
		if (num < -32768L || num > 32767L)
		{
			_result = 0;
			return false;
		}
		_result = (short)num;
		return true;
	}

	// Token: 0x0600A534 RID: 42292 RVA: 0x003D794C File Offset: 0x003D5B4C
	public static bool TryParseUInt16(string _input, out ushort _result, int _startIndex, int _endIndex = -1, NumberStyles _style = NumberStyles.Integer)
	{
		long num;
		ulong num2;
		Exception ex;
		if (!StringParsers.internalParseInt64(_input, _style, true, out num, out num2, out ex, false, _startIndex, _endIndex))
		{
			_result = 0;
			return false;
		}
		if (num2 > 65535UL)
		{
			_result = 0;
			return false;
		}
		_result = (ushort)num2;
		return true;
	}

	// Token: 0x0600A535 RID: 42293 RVA: 0x003D7985 File Offset: 0x003D5B85
	[XuiXmlAttributeConverter]
	public static bool TryParseSInt16(string _input, out short _result)
	{
		return StringParsers.TryParseSInt16(_input, out _result, 0, -1, NumberStyles.Integer);
	}

	// Token: 0x0600A536 RID: 42294 RVA: 0x003D7991 File Offset: 0x003D5B91
	[XuiXmlAttributeConverter]
	public static bool TryParseUInt16(string _input, out ushort _result)
	{
		return StringParsers.TryParseUInt16(_input, out _result, 0, -1, NumberStyles.Integer);
	}

	// Token: 0x0600A537 RID: 42295 RVA: 0x003D79A0 File Offset: 0x003D5BA0
	public static int ParseSInt32(string _input, int _startIndex = 0, int _endIndex = -1, NumberStyles _style = NumberStyles.Integer)
	{
		long num;
		ulong num2;
		Exception ex;
		if (!StringParsers.internalParseInt64(_input, _style, false, out num, out num2, out ex, true, _startIndex, _endIndex))
		{
			throw ex;
		}
		if (num < -2147483648L || num > 2147483647L)
		{
			throw new OverflowException("Value too large or too small (input='" + _input + "')");
		}
		return (int)num;
	}

	// Token: 0x0600A538 RID: 42296 RVA: 0x003D79EC File Offset: 0x003D5BEC
	public static uint ParseUInt32(string _input, int _startIndex = 0, int _endIndex = -1, NumberStyles _style = NumberStyles.Integer)
	{
		long num;
		ulong num2;
		Exception ex;
		if (!StringParsers.internalParseInt64(_input, _style, false, out num, out num2, out ex, false, _startIndex, _endIndex))
		{
			throw ex;
		}
		if (num2 > (ulong)(-1))
		{
			throw new OverflowException("Value too large or too small (input='" + _input + "')");
		}
		return (uint)num2;
	}

	// Token: 0x0600A539 RID: 42297 RVA: 0x003D7A2C File Offset: 0x003D5C2C
	public static bool TryParseSInt32(string _input, out int _result, int _startIndex, int _endIndex = -1, NumberStyles _style = NumberStyles.Integer)
	{
		long num;
		ulong num2;
		Exception ex;
		if (!StringParsers.internalParseInt64(_input, _style, true, out num, out num2, out ex, true, _startIndex, _endIndex))
		{
			_result = 0;
			return false;
		}
		if (num < -2147483648L || num > 2147483647L)
		{
			_result = 0;
			return false;
		}
		_result = (int)num;
		return true;
	}

	// Token: 0x0600A53A RID: 42298 RVA: 0x003D7A70 File Offset: 0x003D5C70
	public static bool TryParseUInt32(string _input, out uint _result, int _startIndex, int _endIndex = -1, NumberStyles _style = NumberStyles.Integer)
	{
		long num;
		ulong num2;
		Exception ex;
		if (!StringParsers.internalParseInt64(_input, _style, true, out num, out num2, out ex, false, _startIndex, _endIndex))
		{
			_result = 0U;
			return false;
		}
		if (num2 > (ulong)(-1))
		{
			_result = 0U;
			return false;
		}
		_result = (uint)num2;
		return true;
	}

	// Token: 0x0600A53B RID: 42299 RVA: 0x003D7AA5 File Offset: 0x003D5CA5
	[XuiXmlAttributeConverter]
	public static bool TryParseSInt32(string _input, out int _result)
	{
		return StringParsers.TryParseSInt32(_input, out _result, 0, -1, NumberStyles.Integer);
	}

	// Token: 0x0600A53C RID: 42300 RVA: 0x003D7AB1 File Offset: 0x003D5CB1
	[XuiXmlAttributeConverter]
	public static bool TryParseUInt32(string _input, out uint _result)
	{
		return StringParsers.TryParseUInt32(_input, out _result, 0, -1, NumberStyles.Integer);
	}

	// Token: 0x0600A53D RID: 42301 RVA: 0x003D7AC0 File Offset: 0x003D5CC0
	public static long ParseSInt64(string _input, int _startIndex = 0, int _endIndex = -1, NumberStyles _style = NumberStyles.Integer)
	{
		long num;
		ulong num2;
		Exception ex;
		if (!StringParsers.internalParseInt64(_input, _style, false, out num, out num2, out ex, true, _startIndex, _endIndex))
		{
			throw ex;
		}
		return num;
	}

	// Token: 0x0600A53E RID: 42302 RVA: 0x003D7AE4 File Offset: 0x003D5CE4
	public static ulong ParseUInt64(string _input, int _startIndex = 0, int _endIndex = -1, NumberStyles _style = NumberStyles.Integer)
	{
		long num;
		ulong num2;
		Exception ex;
		if (!StringParsers.internalParseInt64(_input, _style, false, out num, out num2, out ex, false, _startIndex, _endIndex))
		{
			throw ex;
		}
		return num2;
	}

	// Token: 0x0600A53F RID: 42303 RVA: 0x003D7B08 File Offset: 0x003D5D08
	public static bool TryParseSInt64(string _input, out long _result, int _startIndex, int _endIndex = -1, NumberStyles _style = NumberStyles.Integer)
	{
		long num;
		ulong num2;
		Exception ex;
		if (!StringParsers.internalParseInt64(_input, _style, true, out num, out num2, out ex, true, _startIndex, _endIndex))
		{
			_result = 0L;
			return false;
		}
		_result = num;
		return true;
	}

	// Token: 0x0600A540 RID: 42304 RVA: 0x003D7B34 File Offset: 0x003D5D34
	public static bool TryParseUInt64(string _input, out ulong _result, int _startIndex, int _endIndex = -1, NumberStyles _style = NumberStyles.Integer)
	{
		long num;
		ulong num2;
		Exception ex;
		if (!StringParsers.internalParseInt64(_input, _style, true, out num, out num2, out ex, false, _startIndex, _endIndex))
		{
			_result = 0UL;
			return false;
		}
		_result = num2;
		return true;
	}

	// Token: 0x0600A541 RID: 42305 RVA: 0x003D7B5F File Offset: 0x003D5D5F
	[XuiXmlAttributeConverter]
	public static bool TryParseSInt64(string _input, out long _result)
	{
		return StringParsers.TryParseSInt64(_input, out _result, 0, -1, NumberStyles.Integer);
	}

	// Token: 0x0600A542 RID: 42306 RVA: 0x003D7B6B File Offset: 0x003D5D6B
	[XuiXmlAttributeConverter]
	public static bool TryParseUInt64(string _input, out ulong _result)
	{
		return StringParsers.TryParseUInt64(_input, out _result, 0, -1, NumberStyles.Integer);
	}

	// Token: 0x0600A543 RID: 42307 RVA: 0x003D7B78 File Offset: 0x003D5D78
	public static float ParseFloat(string _input, int _startIndex = 0, int _endIndex = -1, NumberStyles _style = NumberStyles.Any)
	{
		double num = StringParsers.ParseDouble(_input, _startIndex, _endIndex, _style);
		if (num - 3.4028234663852886E+38 > 3.6147112457961776E+29 && !double.IsPositiveInfinity(num))
		{
			throw new OverflowException();
		}
		return (float)num;
	}

	// Token: 0x0600A544 RID: 42308 RVA: 0x003D7BB8 File Offset: 0x003D5DB8
	public static bool TryParseFloat(string _input, out float _result, int _startIndex, int _endIndex = -1, NumberStyles _style = NumberStyles.Any)
	{
		double num;
		Exception ex;
		if (!StringParsers.internalParseDouble(_input, _style, true, out num, out ex, _startIndex, _endIndex))
		{
			_result = 0f;
			return false;
		}
		if (num - 3.4028234663852886E+38 > 3.6147112457961776E+29 && !double.IsPositiveInfinity(num))
		{
			_result = 0f;
			return false;
		}
		_result = (float)num;
		return true;
	}

	// Token: 0x0600A545 RID: 42309 RVA: 0x003D7C0B File Offset: 0x003D5E0B
	[XuiXmlAttributeConverter]
	public static bool TryParseFloat(string _input, out float _result)
	{
		return StringParsers.TryParseFloat(_input, out _result, 0, -1, NumberStyles.Any);
	}

	// Token: 0x0600A546 RID: 42310 RVA: 0x003D7C1C File Offset: 0x003D5E1C
	public static double ParseDouble(string _input, int _startIndex = 0, int _endIndex = -1, NumberStyles _style = NumberStyles.Any)
	{
		double num;
		Exception ex;
		if (!StringParsers.internalParseDouble(_input, _style, false, out num, out ex, _startIndex, _endIndex))
		{
			throw ex;
		}
		return num;
	}

	// Token: 0x0600A547 RID: 42311 RVA: 0x003D7C3C File Offset: 0x003D5E3C
	public static bool TryParseDouble(string _input, out double _result, int _startIndex, int _endIndex = -1, NumberStyles _style = NumberStyles.Any)
	{
		Exception ex;
		if (!StringParsers.internalParseDouble(_input, _style, true, out _result, out ex, _startIndex, _endIndex))
		{
			_result = 0.0;
			return false;
		}
		return true;
	}

	// Token: 0x0600A548 RID: 42312 RVA: 0x003D7C67 File Offset: 0x003D5E67
	[XuiXmlAttributeConverter]
	public static bool TryParseDouble(string _input, out double _result)
	{
		return StringParsers.TryParseDouble(_input, out _result, 0, -1, NumberStyles.Any);
	}

	// Token: 0x0600A549 RID: 42313 RVA: 0x003D7C77 File Offset: 0x003D5E77
	public static DateTime ParseDateTime(string _s)
	{
		return DateTime.Parse(_s, Utils.StandardCulture);
	}

	// Token: 0x0600A54A RID: 42314 RVA: 0x003D7C84 File Offset: 0x003D5E84
	[XuiXmlAttributeConverter]
	public static bool TryParseDateTime(string _s, out DateTime _result)
	{
		return DateTime.TryParse(_s, Utils.StandardCulture, DateTimeStyles.None, out _result);
	}

	// Token: 0x0600A54B RID: 42315 RVA: 0x003D7C94 File Offset: 0x003D5E94
	public static bool ParseBool(string _input, int _startIndex = 0, int _endIndex = -1, bool _ignoreCase = true)
	{
		bool flag;
		Exception ex;
		if (!StringParsers.internalParseBool(_input, false, out flag, out ex, _ignoreCase, _startIndex, _endIndex))
		{
			throw ex;
		}
		return flag;
	}

	// Token: 0x0600A54C RID: 42316 RVA: 0x003D7CB4 File Offset: 0x003D5EB4
	public static bool TryParseBool(string _input, out bool _result, int _startIndex, int _endIndex = -1, bool _ignoreCase = true)
	{
		Exception ex;
		if (!StringParsers.internalParseBool(_input, true, out _result, out ex, _ignoreCase, _startIndex, _endIndex))
		{
			_result = false;
			return false;
		}
		return true;
	}

	// Token: 0x0600A54D RID: 42317 RVA: 0x003D7CD7 File Offset: 0x003D5ED7
	[XuiXmlAttributeConverter]
	public static bool TryParseBool(string _input, out bool _result)
	{
		return StringParsers.TryParseBool(_input, out _result, 0, -1, true);
	}

	// Token: 0x0600A54E RID: 42318 RVA: 0x003D7CE4 File Offset: 0x003D5EE4
	public static bool TryParseRange(string _input, out FloatRange _result, float? _defaultMax = null, char separators = ',')
	{
		_result = default(FloatRange);
		StringParsers.SeparatorPositions separatorPositions = StringParsers.GetSeparatorPositions(_input, separators, 1, 0, -1);
		if (separatorPositions.TotalFound == 0)
		{
			float num;
			if (StringParsers.TryParseFloat(_input, out num))
			{
				_result = new FloatRange(num, _defaultMax ?? num);
				return true;
			}
			return false;
		}
		else
		{
			float num2;
			float num3;
			if (StringParsers.TryParseFloat(_input, out num2, 0, separatorPositions.Sep1 - 1, NumberStyles.Any) && StringParsers.TryParseFloat(_input, out num3, separatorPositions.Sep1 + 1, -1, NumberStyles.Any))
			{
				_result = new FloatRange(num2, num3);
				return true;
			}
			return false;
		}
	}

	// Token: 0x0600A54F RID: 42319 RVA: 0x003D7D7C File Offset: 0x003D5F7C
	public static bool TryParseRange(string _input, out IntRange _result, int? _defaultMax = null, char separators = ',')
	{
		_result = default(IntRange);
		StringParsers.SeparatorPositions separatorPositions = StringParsers.GetSeparatorPositions(_input, separators, 1, 0, -1);
		if (separatorPositions.TotalFound == 0)
		{
			int num;
			if (StringParsers.TryParseSInt32(_input, out num))
			{
				_result = new IntRange(num, _defaultMax ?? num);
				return true;
			}
			return false;
		}
		else
		{
			int num2;
			int num3;
			if (StringParsers.TryParseSInt32(_input, out num2, 0, separatorPositions.Sep1 - 1, NumberStyles.Integer) && StringParsers.TryParseSInt32(_input, out num3, separatorPositions.Sep1 + 1, -1, NumberStyles.Integer))
			{
				_result = new IntRange(num2, num3);
				return true;
			}
			return false;
		}
	}

	// Token: 0x0600A550 RID: 42320 RVA: 0x003D7E0C File Offset: 0x003D600C
	public static Vector2 ParseVector2(string _input)
	{
		StringParsers.SeparatorPositions separatorPositions = StringParsers.GetSeparatorPositions(_input, ',', 1, 0, -1);
		if (separatorPositions.TotalFound != 1)
		{
			return Vector2.zero;
		}
		return new Vector2(StringParsers.ParseFloat(_input, 0, separatorPositions.Sep1 - 1, NumberStyles.Any), StringParsers.ParseFloat(_input, separatorPositions.Sep1 + 1, -1, NumberStyles.Any));
	}

	// Token: 0x0600A551 RID: 42321 RVA: 0x003D7E64 File Offset: 0x003D6064
	[XuiXmlAttributeConverter]
	public static bool TryParseVector2(string _input, out Vector2 _output)
	{
		_output = default(Vector2);
		int num = 0;
		int num2 = -1;
		if (_input.Length > 0 && _input[0] == '(' && _input[_input.Length - 1] == ')')
		{
			num = 1;
			num2 = _input.Length - 2;
		}
		if (num2 < 0)
		{
			num2 = _input.Length - 1;
		}
		if (num2 < num || num2 >= _input.Length)
		{
			return false;
		}
		StringParsers.SeparatorPositions separatorPositions = StringParsers.GetSeparatorPositions(_input, ',', 1, num, num2);
		if (separatorPositions.TotalFound != 1)
		{
			return false;
		}
		float num3;
		if (!StringParsers.TryParseFloat(_input, out num3, num, separatorPositions.Sep1 - 1, NumberStyles.Any))
		{
			return false;
		}
		float num4;
		if (!StringParsers.TryParseFloat(_input, out num4, separatorPositions.Sep1 + 1, num2, NumberStyles.Any))
		{
			return false;
		}
		_output = new Vector2(num3, num4);
		return true;
	}

	// Token: 0x0600A552 RID: 42322 RVA: 0x003D7F24 File Offset: 0x003D6124
	public static Vector2 ParseVector2(string _input, float _defaultValue)
	{
		StringParsers.SeparatorPositions separatorPositions = StringParsers.GetSeparatorPositions(_input, ',', 1, 0, -1);
		if (separatorPositions.TotalFound == 0)
		{
			return new Vector2(StringParsers.ParseFloat(_input, 0, -1, NumberStyles.Any), _defaultValue);
		}
		return new Vector2(StringParsers.ParseFloat(_input, 0, separatorPositions.Sep1 - 1, NumberStyles.Any), StringParsers.ParseFloat(_input, separatorPositions.Sep1 + 1, -1, NumberStyles.Any));
	}

	// Token: 0x0600A553 RID: 42323 RVA: 0x003D7F88 File Offset: 0x003D6188
	public static Vector3 ParseVector3(string _input, int _startIndex = 0, int _endIndex = -1)
	{
		if (_startIndex == 0 && _endIndex < 0 && _input.Length > 0 && _input[0] == '(' && _input[_input.Length - 1] == ')')
		{
			_startIndex = 1;
			_endIndex = _input.Length - 2;
		}
		if (_endIndex < 0)
		{
			_endIndex = _input.Length - 1;
		}
		if (_endIndex < _startIndex || _endIndex >= _input.Length)
		{
			throw new ArgumentException("_endIndex out of range (input='" + _input + "')", "_endIndex");
		}
		StringParsers.SeparatorPositions separatorPositions = StringParsers.GetSeparatorPositions(_input, ',', 2, _startIndex, _endIndex);
		if (separatorPositions.TotalFound != 2)
		{
			return Vector3.zero;
		}
		return new Vector3(StringParsers.ParseFloat(_input, _startIndex, separatorPositions.Sep1 - 1, NumberStyles.Any), StringParsers.ParseFloat(_input, separatorPositions.Sep1 + 1, separatorPositions.Sep2 - 1, NumberStyles.Any), StringParsers.ParseFloat(_input, separatorPositions.Sep2 + 1, _endIndex, NumberStyles.Any));
	}

	// Token: 0x0600A554 RID: 42324 RVA: 0x003D8068 File Offset: 0x003D6268
	[XuiXmlAttributeConverter]
	public static bool TryParseVector3(string _input, out Vector3 _output)
	{
		_output = default(Vector3);
		int num = 0;
		int num2 = -1;
		if (_input.Length > 0 && _input[0] == '(' && _input[_input.Length - 1] == ')')
		{
			num = 1;
			num2 = _input.Length - 2;
		}
		if (num2 < 0)
		{
			num2 = _input.Length - 1;
		}
		if (num2 < num || num2 >= _input.Length)
		{
			return false;
		}
		StringParsers.SeparatorPositions separatorPositions = StringParsers.GetSeparatorPositions(_input, ',', 2, num, num2);
		if (separatorPositions.TotalFound != 2)
		{
			return false;
		}
		float num3;
		if (!StringParsers.TryParseFloat(_input, out num3, num, separatorPositions.Sep1 - 1, NumberStyles.Any))
		{
			return false;
		}
		float num4;
		if (!StringParsers.TryParseFloat(_input, out num4, separatorPositions.Sep1 + 1, separatorPositions.Sep2 - 1, NumberStyles.Any))
		{
			return false;
		}
		float num5;
		if (!StringParsers.TryParseFloat(_input, out num5, separatorPositions.Sep2 + 1, num2, NumberStyles.Any))
		{
			return false;
		}
		_output = new Vector3(num3, num4, num5);
		return true;
	}

	// Token: 0x0600A555 RID: 42325 RVA: 0x003D814C File Offset: 0x003D634C
	public static Vector3 ParseVector3(string _input, float _defaultValue)
	{
		StringParsers.SeparatorPositions separatorPositions = StringParsers.GetSeparatorPositions(_input, ',', 1, 0, -1);
		if (separatorPositions.TotalFound == 0)
		{
			return new Vector3(StringParsers.ParseFloat(_input, 0, -1, NumberStyles.Any), _defaultValue, _defaultValue);
		}
		if (separatorPositions.TotalFound == 1)
		{
			return new Vector3(StringParsers.ParseFloat(_input, 0, separatorPositions.Sep1 - 1, NumberStyles.Any), StringParsers.ParseFloat(_input, separatorPositions.Sep1 + 1, -1, NumberStyles.Any), _defaultValue);
		}
		return new Vector3(StringParsers.ParseFloat(_input, 0, separatorPositions.Sep1 - 1, NumberStyles.Any), StringParsers.ParseFloat(_input, separatorPositions.Sep1 + 1, separatorPositions.Sep2 - 1, NumberStyles.Any), StringParsers.ParseFloat(_input, separatorPositions.Sep2 + 1, -1, NumberStyles.Any));
	}

	// Token: 0x0600A556 RID: 42326 RVA: 0x003D8204 File Offset: 0x003D6404
	public static Vector4 ParseVector4(string _input)
	{
		StringParsers.SeparatorPositions separatorPositions = StringParsers.GetSeparatorPositions(_input, ',', 3, 0, -1);
		if (separatorPositions.TotalFound != 3)
		{
			return Vector4.zero;
		}
		return new Vector4(StringParsers.ParseFloat(_input, 0, separatorPositions.Sep1 - 1, NumberStyles.Any), StringParsers.ParseFloat(_input, separatorPositions.Sep1 + 1, separatorPositions.Sep2 - 1, NumberStyles.Any), StringParsers.ParseFloat(_input, separatorPositions.Sep2 + 1, separatorPositions.Sep3 - 1, NumberStyles.Any), StringParsers.ParseFloat(_input, separatorPositions.Sep3 + 1, -1, NumberStyles.Any));
	}

	// Token: 0x0600A557 RID: 42327 RVA: 0x003D8290 File Offset: 0x003D6490
	[XuiXmlAttributeConverter]
	public static bool TryParseVector4(string _input, out Vector4 _output)
	{
		_output = default(Vector4);
		int num = 0;
		int num2 = -1;
		if (_input.Length > 0 && _input[0] == '(' && _input[_input.Length - 1] == ')')
		{
			num = 1;
			num2 = _input.Length - 2;
		}
		if (num2 < 0)
		{
			num2 = _input.Length - 1;
		}
		if (num2 < num || num2 >= _input.Length)
		{
			return false;
		}
		StringParsers.SeparatorPositions separatorPositions = StringParsers.GetSeparatorPositions(_input, ',', 3, num, num2);
		if (separatorPositions.TotalFound != 3)
		{
			return false;
		}
		float num3;
		if (!StringParsers.TryParseFloat(_input, out num3, num, separatorPositions.Sep1 - 1, NumberStyles.Any))
		{
			return false;
		}
		float num4;
		if (!StringParsers.TryParseFloat(_input, out num4, separatorPositions.Sep1 + 1, separatorPositions.Sep2 - 1, NumberStyles.Any))
		{
			return false;
		}
		float num5;
		if (!StringParsers.TryParseFloat(_input, out num5, separatorPositions.Sep2 + 1, separatorPositions.Sep3 - 1, NumberStyles.Any))
		{
			return false;
		}
		float num6;
		if (!StringParsers.TryParseFloat(_input, out num6, separatorPositions.Sep3 + 1, num2, NumberStyles.Any))
		{
			return false;
		}
		_output = new Vector4(num3, num4, num5, num6);
		return true;
	}

	// Token: 0x0600A558 RID: 42328 RVA: 0x003D8398 File Offset: 0x003D6598
	public static BlockFaceFlag ParseWaterFlowMask(string _input)
	{
		if (_input.EqualsCaseInsensitive("permitted"))
		{
			return BlockFaceFlag.None;
		}
		if (_input.Contains(','))
		{
			string[] array = _input.Split(',', StringSplitOptions.None);
			BlockFaceFlag blockFaceFlag = BlockFaceFlag.All;
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				char c;
				if (char.TryParse(array2[i], out c))
				{
					blockFaceFlag &= ~BlockFaceFlags.FromBlockFace(BlockFaces.CharToFace(c));
				}
			}
			return blockFaceFlag;
		}
		return BlockFaceFlag.All;
	}

	// Token: 0x0600A559 RID: 42329 RVA: 0x003D83F8 File Offset: 0x003D65F8
	public static BlockFaceFlag ParseCoverFaceMask(string _input)
	{
		BlockFaceFlag blockFaceFlag = BlockFaceFlag.None;
		try
		{
			if (_input != null)
			{
				string[] array = _input.Split(',', StringSplitOptions.None);
				for (int i = 0; i < array.Length; i++)
				{
					char c;
					if (char.TryParse(array[i], out c))
					{
						blockFaceFlag |= BlockFaceFlags.FromBlockFace(BlockFaces.CharToFace(c));
					}
				}
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex);
		}
		return blockFaceFlag;
	}

	// Token: 0x0600A55A RID: 42330 RVA: 0x003D8458 File Offset: 0x003D6658
	public static Plane ParsePlane(string _input)
	{
		StringParsers.SeparatorPositions separatorPositions = StringParsers.GetSeparatorPositions(_input, ',', 3, 0, -1);
		if (separatorPositions.TotalFound != 3)
		{
			return default(Plane);
		}
		Vector3 vector = new Vector3(StringParsers.ParseFloat(_input, 0, separatorPositions.Sep1 - 1, NumberStyles.Any), StringParsers.ParseFloat(_input, separatorPositions.Sep1 + 1, separatorPositions.Sep2 - 1, NumberStyles.Any), StringParsers.ParseFloat(_input, separatorPositions.Sep2 + 1, separatorPositions.Sep3 - 1, NumberStyles.Any));
		float num = StringParsers.ParseFloat(_input, separatorPositions.Sep3 + 1, -1, NumberStyles.Any);
		return new Plane(vector, num);
	}

	// Token: 0x0600A55B RID: 42331 RVA: 0x003D84F0 File Offset: 0x003D66F0
	public static Quaternion ParseQuaternion(string _input)
	{
		StringParsers.SeparatorPositions separatorPositions = StringParsers.GetSeparatorPositions(_input, ',', 3, 0, -1);
		if (separatorPositions.TotalFound != 3)
		{
			return Quaternion.identity;
		}
		return new Quaternion(StringParsers.ParseFloat(_input, 0, separatorPositions.Sep1 - 1, NumberStyles.Any), StringParsers.ParseFloat(_input, separatorPositions.Sep1 + 1, separatorPositions.Sep2 - 1, NumberStyles.Any), StringParsers.ParseFloat(_input, separatorPositions.Sep2 + 1, separatorPositions.Sep3 - 1, NumberStyles.Any), StringParsers.ParseFloat(_input, separatorPositions.Sep3 + 1, -1, NumberStyles.Any));
	}

	// Token: 0x0600A55C RID: 42332 RVA: 0x003D857C File Offset: 0x003D677C
	public static Color ParseColor(string _input)
	{
		Color color;
		if (!StringParsers.TryParseColor(_input, out color))
		{
			return Color.white;
		}
		return color;
	}

	// Token: 0x0600A55D RID: 42333 RVA: 0x003D859C File Offset: 0x003D679C
	public static bool TryParseColor(string _input, out Color _output)
	{
		_output = default(Color);
		int num = 0;
		int num2 = _input.Length - 1;
		if (_input.StartsWith("RGBA("))
		{
			num = "RGBA(".Length;
			num2--;
		}
		StringParsers.SeparatorPositions separatorPositions = StringParsers.GetSeparatorPositions(_input, ',', 3, num, num2);
		if (separatorPositions.TotalFound < 2 || separatorPositions.TotalFound > 3)
		{
			return false;
		}
		float num3;
		if (!StringParsers.TryParseFloat(_input, out num3, num, separatorPositions.Sep1 - 1, NumberStyles.Any))
		{
			return false;
		}
		float num4;
		if (!StringParsers.TryParseFloat(_input, out num4, separatorPositions.Sep1 + 1, separatorPositions.Sep2 - 1, NumberStyles.Any))
		{
			return false;
		}
		float num5;
		if (!StringParsers.TryParseFloat(_input, out num5, separatorPositions.Sep2 + 1, separatorPositions.Sep3 - 1, NumberStyles.Any))
		{
			return false;
		}
		if (separatorPositions.TotalFound == 2)
		{
			_output = new Color(num3, num4, num5);
			return true;
		}
		float num6;
		if (!StringParsers.TryParseFloat(_input, out num6, separatorPositions.Sep3 + 1, num2, NumberStyles.Any))
		{
			return false;
		}
		_output = new Color(num3, num4, num5, num6);
		return true;
	}

	// Token: 0x0600A55E RID: 42334 RVA: 0x003D869C File Offset: 0x003D689C
	public static Color ParseHexColor(string _input)
	{
		if (_input == null)
		{
			return Color.clear;
		}
		if (_input.IndexOf(',') >= 0)
		{
			return StringParsers.ParseColor32(_input);
		}
		if (_input.Length < 6)
		{
			return Color.clear;
		}
		int num = 0;
		if (_input[0] == '#')
		{
			num = 1;
		}
		byte b = StringParsers.ParseUInt8(_input, num, num + 1, NumberStyles.HexNumber);
		byte b2 = StringParsers.ParseUInt8(_input, num + 2, num + 3, NumberStyles.HexNumber);
		byte b3 = StringParsers.ParseUInt8(_input, num + 4, num + 5, NumberStyles.HexNumber);
		return new Color32(b, b2, b3, byte.MaxValue);
	}

	// Token: 0x0600A55F RID: 42335 RVA: 0x003D8728 File Offset: 0x003D6928
	public static bool TryParseColor32(string _input, out Color32 _output)
	{
		_output = default(Color32);
		int num = 0;
		int num2 = _input.Length - 1;
		if (_input.StartsWith("RGBA("))
		{
			num = "RGBA(".Length;
			num2--;
		}
		StringParsers.SeparatorPositions separatorPositions = StringParsers.GetSeparatorPositions(_input, ',', 3, num, num2);
		if (separatorPositions.TotalFound < 2 || separatorPositions.TotalFound > 3)
		{
			return false;
		}
		int num3;
		if (!StringParsers.TryParseSInt32(_input, out num3, num, separatorPositions.Sep1 - 1, NumberStyles.Integer))
		{
			return false;
		}
		int num4;
		if (!StringParsers.TryParseSInt32(_input, out num4, separatorPositions.Sep1 + 1, separatorPositions.Sep2 - 1, NumberStyles.Integer))
		{
			return false;
		}
		int num5;
		if (!StringParsers.TryParseSInt32(_input, out num5, separatorPositions.Sep2 + 1, separatorPositions.Sep3 - 1, NumberStyles.Integer))
		{
			return false;
		}
		int num6 = 255;
		if (separatorPositions.TotalFound > 2 && !StringParsers.TryParseSInt32(_input, out num6, separatorPositions.Sep3 + 1, num2, NumberStyles.Integer))
		{
			return false;
		}
		_output = new Color32((byte)Mathf.Clamp(num3, 0, 255), (byte)Mathf.Clamp(num4, 0, 255), (byte)Mathf.Clamp(num5, 0, 255), (byte)Mathf.Clamp(num6, 0, 255));
		return true;
	}

	// Token: 0x0600A560 RID: 42336 RVA: 0x003D883C File Offset: 0x003D6A3C
	public static Color ParseColor32(string _input)
	{
		StringParsers.SeparatorPositions separatorPositions = StringParsers.GetSeparatorPositions(_input, ',', 3, 0, -1);
		if (separatorPositions.TotalFound < 2)
		{
			return Color.white;
		}
		float num = (float)StringParsers.ParseSInt32(_input, 0, separatorPositions.Sep1 - 1, NumberStyles.Integer);
		int num2 = StringParsers.ParseSInt32(_input, separatorPositions.Sep1 + 1, separatorPositions.Sep2 - 1, NumberStyles.Integer);
		int num3 = StringParsers.ParseSInt32(_input, separatorPositions.Sep2 + 1, separatorPositions.Sep3 - 1, NumberStyles.Integer);
		int num4 = 255;
		if (separatorPositions.TotalFound > 2)
		{
			num4 = StringParsers.ParseSInt32(_input, separatorPositions.Sep3 + 1, -1, NumberStyles.Integer);
		}
		return new Color(num / 255f, (float)num2 / 255f, (float)num3 / 255f, (float)num4 / 255f);
	}

	// Token: 0x0600A561 RID: 42337 RVA: 0x003D88E8 File Offset: 0x003D6AE8
	public static Bounds ParseBounds(string _input)
	{
		int num = _input.IndexOf('(');
		if (num < 0)
		{
			throw new FormatException("Bounds input string is not in the correct format. Expected \"([Center X], [Center Y], [Center Z]), ([Size X], [Size Y], [Size Z])\" - e.g. \"(-0.5,0.5,-0.5),(2,2,6)\" (input='" + _input + "')");
		}
		int num2 = _input.IndexOf(')', num + 1);
		if (num2 < 0)
		{
			throw new FormatException("Bounds input string is not in the correct format. Expected \"([Center X], [Center Y], [Center Z]), ([Size X], [Size Y], [Size Z])\" - e.g. \"(-0.5,0.5,-0.5),(2,2,6)\" (input='" + _input + "')");
		}
		int num3 = _input.IndexOf('(', num2 + 1);
		if (num3 < 0)
		{
			throw new FormatException("Bounds input string is not in the correct format. Expected \"([Center X], [Center Y], [Center Z]), ([Size X], [Size Y], [Size Z])\" - e.g. \"(-0.5,0.5,-0.5),(2,2,6)\" (input='" + _input + "')");
		}
		int num4 = _input.IndexOf(')', num3 + 1);
		if (num4 < 0)
		{
			throw new FormatException("Bounds input string is not in the correct format. Expected \"([Center X], [Center Y], [Center Z]), ([Size X], [Size Y], [Size Z])\" - e.g. \"(-0.5,0.5,-0.5),(2,2,6)\" (input='" + _input + "')");
		}
		Vector3 vector = StringParsers.ParseVector3(_input, num + 1, num2 - 1);
		Vector3 vector2 = StringParsers.ParseVector3(_input, num3 + 1, num4 - 1);
		return new Bounds(vector, vector2);
	}

	// Token: 0x0600A562 RID: 42338 RVA: 0x003D89AC File Offset: 0x003D6BAC
	public static Vector2d ParseVector2d(string _input)
	{
		StringParsers.SeparatorPositions separatorPositions = StringParsers.GetSeparatorPositions(_input, ',', 1, 0, -1);
		if (separatorPositions.TotalFound != 1)
		{
			return Vector2d.Zero;
		}
		return new Vector2d(StringParsers.ParseDouble(_input, 0, separatorPositions.Sep1 - 1, NumberStyles.Any), StringParsers.ParseDouble(_input, separatorPositions.Sep1 + 1, -1, NumberStyles.Any));
	}

	// Token: 0x0600A563 RID: 42339 RVA: 0x003D8A04 File Offset: 0x003D6C04
	public static Vector3d ParseVector3d(string _input)
	{
		StringParsers.SeparatorPositions separatorPositions = StringParsers.GetSeparatorPositions(_input, ',', 2, 0, -1);
		if (separatorPositions.TotalFound != 2)
		{
			return Vector3d.Zero;
		}
		return new Vector3d(StringParsers.ParseDouble(_input, 0, separatorPositions.Sep1 - 1, NumberStyles.Any), StringParsers.ParseDouble(_input, separatorPositions.Sep1 + 1, separatorPositions.Sep2 - 1, NumberStyles.Any), StringParsers.ParseDouble(_input, separatorPositions.Sep2 + 1, -1, NumberStyles.Any));
	}

	// Token: 0x0600A564 RID: 42340 RVA: 0x003D8A74 File Offset: 0x003D6C74
	public static Vector2i ParseVector2i(string _input, char _customSep = ',')
	{
		StringParsers.SeparatorPositions separatorPositions = StringParsers.GetSeparatorPositions(_input, _customSep, 1, 0, -1);
		if (separatorPositions.TotalFound != 1)
		{
			return Vector2i.zero;
		}
		return new Vector2i(StringParsers.ParseSInt32(_input, 0, separatorPositions.Sep1 - 1, NumberStyles.Integer), StringParsers.ParseSInt32(_input, separatorPositions.Sep1 + 1, -1, NumberStyles.Integer));
	}

	// Token: 0x0600A565 RID: 42341 RVA: 0x003D8AC0 File Offset: 0x003D6CC0
	[XuiXmlAttributeConverter]
	public static bool TryParseVector2i(string _input, out Vector2i _output)
	{
		_output = default(Vector2i);
		int num = 0;
		int num2 = -1;
		if (_input.Length > 0 && _input[0] == '(' && _input[_input.Length - 1] == ')')
		{
			num = 1;
			num2 = _input.Length - 2;
		}
		if (num2 < 0)
		{
			num2 = _input.Length - 1;
		}
		if (num2 < num || num2 >= _input.Length)
		{
			return false;
		}
		StringParsers.SeparatorPositions separatorPositions = StringParsers.GetSeparatorPositions(_input, ',', 1, num, num2);
		if (separatorPositions.TotalFound != 1)
		{
			return false;
		}
		int num3;
		if (!StringParsers.TryParseSInt32(_input, out num3, num, separatorPositions.Sep1 - 1, NumberStyles.Integer))
		{
			return false;
		}
		int num4;
		if (!StringParsers.TryParseSInt32(_input, out num4, separatorPositions.Sep1 + 1, num2, NumberStyles.Integer))
		{
			return false;
		}
		_output = new Vector2i(num3, num4);
		return true;
	}

	// Token: 0x0600A566 RID: 42342 RVA: 0x003D8B78 File Offset: 0x003D6D78
	public static Vector3i ParseVector3i(string _input, int _startIndex = 0, int _endIndex = -1, bool _errorOnFailure = false)
	{
		if (_endIndex < 0)
		{
			_endIndex = _input.Length - 1;
		}
		if (_endIndex < _startIndex || _endIndex >= _input.Length)
		{
			throw new ArgumentException("_endIndex out of range (input='" + _input + "')", "_endIndex");
		}
		StringParsers.SeparatorPositions separatorPositions = StringParsers.GetSeparatorPositions(_input, ',', 2, _startIndex, _endIndex);
		if (separatorPositions.TotalFound == 2)
		{
			return new Vector3i(StringParsers.ParseSInt32(_input, _startIndex, separatorPositions.Sep1 - 1, NumberStyles.Integer), StringParsers.ParseSInt32(_input, separatorPositions.Sep1 + 1, separatorPositions.Sep2 - 1, NumberStyles.Integer), StringParsers.ParseSInt32(_input, separatorPositions.Sep2 + 1, _endIndex, NumberStyles.Integer));
		}
		if (_errorOnFailure)
		{
			throw new FormatException("_input in invalid format (input='" + _input + "')");
		}
		return Vector3i.zero;
	}

	// Token: 0x0600A567 RID: 42343 RVA: 0x003D8C2C File Offset: 0x003D6E2C
	[XuiXmlAttributeConverter]
	public static bool TryParseVector3i(string _input, out Vector3i _output)
	{
		_output = default(Vector3i);
		int num = 0;
		int num2 = -1;
		if (_input.Length > 0 && _input[0] == '(' && _input[_input.Length - 1] == ')')
		{
			num = 1;
			num2 = _input.Length - 2;
		}
		if (num2 < 0)
		{
			num2 = _input.Length - 1;
		}
		if (num2 < num || num2 >= _input.Length)
		{
			return false;
		}
		StringParsers.SeparatorPositions separatorPositions = StringParsers.GetSeparatorPositions(_input, ',', 2, num, num2);
		if (separatorPositions.TotalFound != 2)
		{
			return false;
		}
		int num3;
		if (!StringParsers.TryParseSInt32(_input, out num3, num, separatorPositions.Sep1 - 1, NumberStyles.Integer))
		{
			return false;
		}
		int num4;
		if (!StringParsers.TryParseSInt32(_input, out num4, separatorPositions.Sep1 + 1, separatorPositions.Sep2 - 1, NumberStyles.Integer))
		{
			return false;
		}
		int num5;
		if (!StringParsers.TryParseSInt32(_input, out num5, separatorPositions.Sep2 + 1, num2, NumberStyles.Integer))
		{
			return false;
		}
		_output = new Vector3i(num3, num4, num5);
		return true;
	}

	// Token: 0x0600A568 RID: 42344 RVA: 0x003D8D04 File Offset: 0x003D6F04
	public static List<T> ParseList<T>(string _input, char _separator, Func<string, int, int, T> _parserFunc)
	{
		List<T> list = new List<T>();
		int num = -1;
		for (int i = _input.IndexOf(_separator, 0); i >= 0; i = _input.IndexOf(_separator, num + 1))
		{
			list.Add(_parserFunc(_input, num + 1, i - 1));
			num = i;
		}
		if (num + 1 < _input.Length)
		{
			list.Add(_parserFunc(_input, num + 1, _input.Length - 1));
		}
		return list;
	}

	// Token: 0x0600A569 RID: 42345 RVA: 0x003D8D6C File Offset: 0x003D6F6C
	public static void ParseMinMaxCount(string _input, out int _minCount, out int _maxCount)
	{
		StringParsers.SeparatorPositions separatorPositions = StringParsers.GetSeparatorPositions(_input, ',', 1, 0, -1);
		if (separatorPositions.TotalFound > 1)
		{
			throw new Exception("Parsing error count (input='" + _input + "')");
		}
		if (separatorPositions.TotalFound == 0)
		{
			if (!StringParsers.TryParseSInt32(_input, out _minCount))
			{
				throw new Exception("Parsing error count (input='" + _input + "')");
			}
			_maxCount = _minCount;
		}
		if (!StringParsers.TryParseSInt32(_input, out _minCount, 0, separatorPositions.Sep1 - 1, NumberStyles.Integer) || !StringParsers.TryParseSInt32(_input, out _maxCount, separatorPositions.Sep1 + 1, -1, NumberStyles.Integer))
		{
			throw new Exception("Parsing error count (input='" + _input + "')");
		}
	}

	// Token: 0x0600A56A RID: 42346 RVA: 0x003D8E0C File Offset: 0x003D700C
	public static void ParseMinMaxCount(string _input, out float _minCount, out float _maxCount)
	{
		StringParsers.SeparatorPositions separatorPositions = StringParsers.GetSeparatorPositions(_input, ',', 1, 0, -1);
		if (separatorPositions.TotalFound > 1)
		{
			throw new Exception("Parsing error count (input='" + _input + "')");
		}
		if (separatorPositions.TotalFound == 0)
		{
			if (!StringParsers.TryParseFloat(_input, out _minCount))
			{
				throw new Exception("Parsing error count (input='" + _input + "')");
			}
			_maxCount = _minCount;
		}
		if (!StringParsers.TryParseFloat(_input, out _minCount, 0, separatorPositions.Sep1 - 1, NumberStyles.Any) || !StringParsers.TryParseFloat(_input, out _maxCount, separatorPositions.Sep1 + 1, -1, NumberStyles.Any))
		{
			throw new Exception("Parsing error count (input='" + _input + "')");
		}
	}

	// Token: 0x0600A56B RID: 42347 RVA: 0x003D8EB4 File Offset: 0x003D70B4
	public static Vector2 ParseMinMaxCount(string _input)
	{
		StringParsers.SeparatorPositions separatorPositions = StringParsers.GetSeparatorPositions(_input, ',', 1, 0, -1);
		if (separatorPositions.TotalFound != 1)
		{
			return Vector2.zero;
		}
		float num = StringParsers.ParseFloat(_input, 0, separatorPositions.Sep1 - 1, NumberStyles.Any);
		float num2 = StringParsers.ParseFloat(_input, separatorPositions.Sep1 + 1, -1, NumberStyles.Any);
		if (num != num2)
		{
			return new Vector2(Mathf.Min(num, num2), Mathf.Max(num, num2));
		}
		return new Vector2(num, num2);
	}

	// Token: 0x0600A56C RID: 42348 RVA: 0x003D8F25 File Offset: 0x003D7125
	[PublicizedFrom(EAccessModifier.Private)]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool findOther(ref int _pos, string _input, char _other)
	{
		if (_input[_pos] == _other)
		{
			_pos++;
			return true;
		}
		return false;
	}

	// Token: 0x0600A56D RID: 42349 RVA: 0x003D8F3C File Offset: 0x003D713C
	public static StringParsers.SeparatorPositions GetSeparatorPositions(string _input, char _separator, int _expected, int _startIndex = 0, int _endIndex = -1)
	{
		StringParsers.SeparatorPositions separatorPositions = new StringParsers.SeparatorPositions(0);
		if (_expected <= 0)
		{
			throw new ArgumentException("_expected has to be greater than 0");
		}
		if (_input == null)
		{
			throw new ArgumentNullException("_input");
		}
		if (_input.Length == 0)
		{
			return separatorPositions;
		}
		if (_endIndex < 0)
		{
			_endIndex = _input.Length - 1;
		}
		if (_endIndex < _startIndex || _endIndex >= _input.Length)
		{
			throw new ArgumentException("_endIndex out of range (input='" + _input + "')", "_endIndex");
		}
		separatorPositions.Sep1 = _input.IndexOf(_separator, _startIndex, _endIndex - _startIndex + 1);
		if (separatorPositions.Sep1 < 0)
		{
			return separatorPositions;
		}
		separatorPositions.TotalFound++;
		separatorPositions.Sep2 = _input.IndexOf(_separator, separatorPositions.Sep1 + 1, _endIndex - separatorPositions.Sep1);
		if (separatorPositions.Sep2 < 0)
		{
			return separatorPositions;
		}
		separatorPositions.TotalFound++;
		if (_expected == 1)
		{
			return separatorPositions;
		}
		separatorPositions.Sep3 = _input.IndexOf(_separator, separatorPositions.Sep2 + 1, _endIndex - separatorPositions.Sep2);
		if (separatorPositions.Sep3 < 0)
		{
			return separatorPositions;
		}
		separatorPositions.TotalFound++;
		if (_expected == 2)
		{
			return separatorPositions;
		}
		separatorPositions.Sep4 = _input.IndexOf(_separator, separatorPositions.Sep3 + 1, _endIndex - separatorPositions.Sep3);
		if (separatorPositions.Sep4 < 0)
		{
			return separatorPositions;
		}
		separatorPositions.TotalFound++;
		if (_expected == 3)
		{
			return separatorPositions;
		}
		if (_input.IndexOf(_separator, separatorPositions.Sep4 + 1, _endIndex - separatorPositions.Sep4) < 0)
		{
			return separatorPositions;
		}
		separatorPositions.TotalFound++;
		return separatorPositions;
	}

	// Token: 0x0600A56E RID: 42350 RVA: 0x003D90BC File Offset: 0x003D72BC
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool internalParseDouble(string _input, NumberStyles _numberStyle, bool _tryParse, out double _result, out Exception _exception, int _startIndex, int _endIndex)
	{
		_result = 0.0;
		_exception = null;
		if (_input == null)
		{
			if (!_tryParse)
			{
				_exception = new ArgumentNullException("_input");
			}
			return false;
		}
		if (_input.Length == 0)
		{
			if (!_tryParse)
			{
				_exception = new FormatException("Empty input string");
			}
			return false;
		}
		if (_startIndex < 0 || _startIndex >= _input.Length)
		{
			if (!_tryParse)
			{
				_exception = new ArgumentException(string.Format("_startIndex ({0}) out of range (input='{1}')", _startIndex, _input), "_startIndex");
			}
			return false;
		}
		if (_endIndex < 0)
		{
			_endIndex = _input.Length - 1;
		}
		if (_endIndex >= _input.Length)
		{
			if (!_tryParse)
			{
				_exception = new ArgumentException(string.Format("_endIndex ({0}) out of range (input='{1}')", _endIndex, _input), "_endIndex");
			}
			return false;
		}
		if ((_numberStyle & NumberStyles.AllowHexSpecifier) != NumberStyles.None)
		{
			throw new ArgumentException("Double doesn't support parsing with 'AllowHexSpecifier' (input='" + _input + "')");
		}
		if (_numberStyle > NumberStyles.Any)
		{
			if (!_tryParse)
			{
				_exception = new ArgumentException();
			}
			return false;
		}
		bool flag = (_numberStyle & NumberStyles.AllowLeadingWhite) > NumberStyles.None;
		bool flag2 = (_numberStyle & NumberStyles.AllowTrailingWhite) > NumberStyles.None;
		bool flag3 = (_numberStyle & NumberStyles.AllowLeadingSign) > NumberStyles.None;
		bool flag4 = (_numberStyle & NumberStyles.AllowExponent) > NumberStyles.None;
		bool flag5 = (_numberStyle & NumberStyles.AllowDecimalPoint) > NumberStyles.None;
		bool flag6 = (_numberStyle & NumberStyles.AllowThousands) > NumberStyles.None;
		bool flag7 = (_numberStyle & NumberStyles.AllowCurrencySymbol) > NumberStyles.None;
		int i = _startIndex;
		if (flag)
		{
			while (i <= _endIndex && char.IsWhiteSpace(_input[i]))
			{
				i++;
			}
			if (i > _endIndex)
			{
				if (!_tryParse)
				{
					_exception = new FormatException("Input string was not in the correct format (input='" + _input + "')");
				}
				return false;
			}
		}
		if (flag2)
		{
			while (_endIndex >= 0 && char.IsWhiteSpace(_input[_endIndex]))
			{
				_endIndex--;
			}
		}
		if (i > _endIndex || _endIndex < 0)
		{
			if (!_tryParse)
			{
				_exception = new FormatException("Input string was not in the correct format (input='" + _input + "')");
			}
			return false;
		}
		if (_endIndex - _startIndex + 1 == StringParsers.NAN_SYMBOL.Length && string.Compare(StringParsers.NAN_SYMBOL, 0, _input, _startIndex, StringParsers.NAN_SYMBOL.Length, StringComparison.Ordinal) == 0)
		{
			_result = double.NaN;
			return true;
		}
		if (_endIndex - _startIndex + 1 == StringParsers.POSITIVE_INFINITY_SYMBOL.Length && string.Compare(StringParsers.POSITIVE_INFINITY_SYMBOL, 0, _input, _startIndex, StringParsers.POSITIVE_INFINITY_SYMBOL.Length, StringComparison.Ordinal) == 0)
		{
			_result = double.PositiveInfinity;
			return true;
		}
		if (_endIndex - _startIndex + 1 == StringParsers.NEGATIVE_INFINITY_SYMBOL.Length && string.Compare(StringParsers.NEGATIVE_INFINITY_SYMBOL, 0, _input, _startIndex, StringParsers.NEGATIVE_INFINITY_SYMBOL.Length, StringComparison.Ordinal) == 0)
		{
			_result = double.NegativeInfinity;
			return true;
		}
		double num = 0.0;
		bool flag8 = false;
		bool flag9 = false;
		int num2 = 0;
		int j = 0;
		StringParsers.EFloatParseState efloatParseState = StringParsers.EFloatParseState.SignOrIntegralDigit;
		while (i <= _endIndex)
		{
			char c = _input[i];
			int num3 = (int)(c ^ '0');
			bool flag10 = num3 <= 9;
			if (c == '\0')
			{
				break;
			}
			switch (efloatParseState)
			{
			case StringParsers.EFloatParseState.SignOrIntegralDigit:
				if (flag3 && c == '+')
				{
					efloatParseState = StringParsers.EFloatParseState.IntegralDigit;
				}
				else if (flag3 && c == '-')
				{
					efloatParseState = StringParsers.EFloatParseState.IntegralDigit;
					flag8 = true;
				}
				else
				{
					efloatParseState = StringParsers.EFloatParseState.IntegralDigit;
					i--;
				}
				break;
			case StringParsers.EFloatParseState.IntegralDigit:
				if (flag10)
				{
					num = num * 10.0 + (double)num3;
				}
				else if (c == 'e' || c == 'E')
				{
					efloatParseState = StringParsers.EFloatParseState.ExponentialTest;
					i--;
				}
				else if (flag5 && c == '.')
				{
					efloatParseState = StringParsers.EFloatParseState.DecimalDigit;
				}
				else if ((!flag6 || c != ',') && (!flag7 || c != '¤'))
				{
					if (!char.IsWhiteSpace(c))
					{
						if (!_tryParse)
						{
							_exception = new FormatException(string.Format("Unknown char: {0} (input: '{1}')", c, _input));
						}
						return false;
					}
					efloatParseState = StringParsers.EFloatParseState.TrailingWs;
					i--;
				}
				break;
			case StringParsers.EFloatParseState.DecimalDigit:
				if (flag10)
				{
					num = num * 10.0 + (double)num3;
					num2++;
				}
				else if (c == 'e' || c == 'E')
				{
					efloatParseState = StringParsers.EFloatParseState.ExponentialTest;
					i--;
				}
				else
				{
					if (!char.IsWhiteSpace(c))
					{
						if (!_tryParse)
						{
							_exception = new FormatException(string.Format("Unknown char: {0} (input: '{1}')", c, _input));
						}
						return false;
					}
					efloatParseState = StringParsers.EFloatParseState.TrailingWs;
					i--;
				}
				break;
			case StringParsers.EFloatParseState.ExponentialTest:
				if (!flag4)
				{
					if (!_tryParse)
					{
						_exception = new FormatException(string.Format("Unknown char: {0} (input: '{1}')", c, _input));
					}
					return false;
				}
				efloatParseState = StringParsers.EFloatParseState.ExponentialSignOrDigit;
				break;
			case StringParsers.EFloatParseState.ExponentialSignOrDigit:
				if (flag10)
				{
					efloatParseState = StringParsers.EFloatParseState.ExponentialDigit;
					i--;
				}
				else if (c == '+')
				{
					efloatParseState = StringParsers.EFloatParseState.ExponentialDigit;
				}
				else if (c == '-')
				{
					flag9 = true;
					efloatParseState = StringParsers.EFloatParseState.ExponentialDigit;
				}
				else
				{
					if (!char.IsWhiteSpace(c))
					{
						if (!_tryParse)
						{
							_exception = new FormatException(string.Format("Unknown char: {0} (input: '{1}')", c, _input));
						}
						return false;
					}
					efloatParseState = StringParsers.EFloatParseState.TrailingWs;
					i--;
				}
				break;
			case StringParsers.EFloatParseState.ExponentialDigit:
				if (flag10)
				{
					j = j * 10 + num3;
				}
				else
				{
					if (!char.IsWhiteSpace(c))
					{
						if (!_tryParse)
						{
							_exception = new FormatException(string.Format("Unknown char: {0} (input: '{1}')", c, _input));
						}
						return false;
					}
					efloatParseState = StringParsers.EFloatParseState.TrailingWs;
					i--;
				}
				break;
			case StringParsers.EFloatParseState.TrailingWs:
				if (!flag2 || !char.IsWhiteSpace(c))
				{
					if (!_tryParse)
					{
						_exception = new FormatException(string.Format("Unknown char: {0} (input: '{1}')", c, _input));
					}
					return false;
				}
				break;
			}
			if (efloatParseState > StringParsers.EFloatParseState.TrailingWs)
			{
				break;
			}
			i++;
		}
		if (num != 0.0)
		{
			if (flag8)
			{
				num *= -1.0;
			}
			if (flag9)
			{
				j *= -1;
			}
			j -= num2;
			if (j < 0)
			{
				flag9 = true;
				j *= -1;
			}
			double num4 = 1.0;
			double num5 = 10.0;
			while (j > 0)
			{
				if (j % 2 == 1)
				{
					num4 *= num5;
				}
				j >>= 1;
				num5 *= num5;
			}
			if (flag9)
			{
				num /= num4;
			}
			else
			{
				num *= num4;
			}
		}
		if (double.IsPositiveInfinity(num) || double.IsNegativeInfinity(num))
		{
			if (!_tryParse)
			{
				_exception = new OverflowException();
			}
			return false;
		}
		_result = num;
		return true;
	}

	// Token: 0x0600A56F RID: 42351 RVA: 0x003D9690 File Offset: 0x003D7890
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool internalParseBool(string _input, bool _tryParse, out bool _result, out Exception _exception, bool _ignoreCase, int _startIndex, int _endIndex)
	{
		_result = false;
		_exception = null;
		if (_input == null)
		{
			if (!_tryParse)
			{
				_exception = new ArgumentNullException("_input");
			}
			return false;
		}
		if (_input.Length == 0)
		{
			if (!_tryParse)
			{
				_exception = new FormatException("Value is not equivalent to either TrueString or FalseString (input='" + _input + "')");
			}
			return false;
		}
		if (_startIndex < 0 || _startIndex >= _input.Length)
		{
			if (!_tryParse)
			{
				_exception = new ArgumentException("_startIndex out of range (input='" + _input + "')", "_startIndex");
			}
			return false;
		}
		if (_endIndex >= _input.Length)
		{
			if (!_tryParse)
			{
				_exception = new ArgumentException("_endIndex out of range (input='" + _input + "')", "_endIndex");
			}
			return false;
		}
		if (_endIndex < 0)
		{
			_endIndex = _input.Length - 1;
		}
		while (_startIndex <= _endIndex && char.IsWhiteSpace(_input[_startIndex]))
		{
			_startIndex++;
		}
		if (_startIndex > _endIndex)
		{
			if (!_tryParse)
			{
				_exception = new FormatException("Input string was not in the correct format (input='" + _input + "')");
			}
			return false;
		}
		while (_endIndex >= 0 && char.IsWhiteSpace(_input[_endIndex]))
		{
			_endIndex--;
		}
		if (_startIndex > _endIndex || _endIndex < 0)
		{
			if (!_tryParse)
			{
				_exception = new FormatException("Input string was not in the correct format (input='" + _input + "')");
			}
			return false;
		}
		StringComparison stringComparison = (_ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
		if (string.Compare(_input, _startIndex, bool.TrueString, 0, _endIndex - _startIndex + 1, stringComparison) == 0)
		{
			_result = true;
			return true;
		}
		if (string.Compare(_input, _startIndex, bool.FalseString, 0, _endIndex - _startIndex + 1, stringComparison) == 0)
		{
			_result = false;
			return true;
		}
		if (!_tryParse)
		{
			_exception = new FormatException("Value is not equivalent to either TrueString or FalseString (input='" + _input + "')");
		}
		return false;
	}

	// Token: 0x0600A570 RID: 42352 RVA: 0x003D9824 File Offset: 0x003D7A24
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool internalParseInt64(string _input, NumberStyles _numberStyle, bool _tryParse, out long _resultSigned, out ulong _resultUnsigned, out Exception _exc, bool _signedResult, int _startIndex, int _endIndex)
	{
		if (_numberStyle != NumberStyles.Integer)
		{
			return StringParsers.internalParseInt64Advanced(_input, _numberStyle, _tryParse, out _resultSigned, out _resultUnsigned, out _exc, _signedResult, _startIndex, _endIndex);
		}
		_resultSigned = 0L;
		_resultUnsigned = 0UL;
		_exc = null;
		if (_input == null)
		{
			if (!_tryParse)
			{
				_exc = new ArgumentNullException("_input");
			}
			return false;
		}
		if (_input.Length == 0)
		{
			if (!_tryParse)
			{
				_exc = new FormatException("Input string was not in the correct format: s.Length==0.");
			}
			return false;
		}
		if (_startIndex < 0 || _startIndex >= _input.Length)
		{
			if (!_tryParse)
			{
				_exc = new ArgumentException("_startIndex out of range (input='" + _input + "')", "_startIndex");
			}
			return false;
		}
		if (_endIndex < 0)
		{
			_endIndex = _input.Length - 1;
		}
		if (_endIndex >= _input.Length)
		{
			if (!_tryParse)
			{
				_exc = new ArgumentException("_endIndex out of range (input='" + _input + "')", "_endIndex");
			}
			return false;
		}
		int num = _startIndex;
		while (num <= _endIndex && char.IsWhiteSpace(_input[num]))
		{
			num++;
		}
		if (num > _endIndex)
		{
			if (!_tryParse)
			{
				_exc = new FormatException("Input string was not in the correct format (input='" + _input + "')");
			}
			return false;
		}
		bool flag = false;
		if (_input[num] == '-')
		{
			flag = true;
			num++;
		}
		else if (_input[num] == '+')
		{
			num++;
		}
		if (num > _endIndex)
		{
			if (!_tryParse)
			{
				_exc = new FormatException("Input string only has a sign (input='" + _input + "')");
			}
			return false;
		}
		ulong num2 = 0UL;
		int num3 = 0;
		do
		{
			char c = _input[num];
			if ((c ^ '0') > '\t')
			{
				break;
			}
			num3++;
			checked
			{
				try
				{
					num2 = num2 * 10UL + (ulong)(unchecked((long)(c ^ '0')));
				}
				catch (OverflowException)
				{
					if (!_tryParse)
					{
						_exc = new OverflowException("Value too large or too small (input='" + _input + "')");
					}
					return false;
				}
			}
			num++;
		}
		while (num <= _endIndex);
		if (num3 == 0)
		{
			if (!_tryParse)
			{
				_exc = new FormatException("Input string was not in the correct format: nDigits == 0 (input='" + _input + "')");
			}
			return false;
		}
		if (num <= _endIndex)
		{
			while (num <= _endIndex && char.IsWhiteSpace(_input[num]))
			{
				num++;
			}
			if (num <= _endIndex && _input[num] != '\0')
			{
				if (!_tryParse)
				{
					_exc = new FormatException(string.Format("Input string was not in the correct format: Did not parse entire string. pos = {0} endIndex = {1} (input='{2}')", num, _endIndex, _input));
				}
				return false;
			}
		}
		if (_signedResult)
		{
			if (flag)
			{
				if (num2 > 9223372036854775808UL)
				{
					if (!_tryParse)
					{
						_exc = new OverflowException("Value too large or too small (input='" + _input + "')");
					}
					return false;
				}
				_resultSigned = (long)(ulong.MaxValue * (num2 - 1UL) - 1UL);
				return true;
			}
			else
			{
				if (num2 > 9223372036854775807UL)
				{
					if (!_tryParse)
					{
						_exc = new OverflowException("Value too large or too small (input='" + _input + "')");
					}
					return false;
				}
				_resultSigned = (long)num2;
				return true;
			}
		}
		else
		{
			if (flag && num2 > 0UL)
			{
				if (!_tryParse)
				{
					_exc = new OverflowException("Negative number (input='" + _input + "')");
				}
				return false;
			}
			_resultUnsigned = num2;
			return true;
		}
		bool flag2;
		return flag2;
	}

	// Token: 0x0600A571 RID: 42353 RVA: 0x003D9AE8 File Offset: 0x003D7CE8
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool internalParseInt64Advanced(string _input, NumberStyles _numberStyle, bool _tryParse, out long _resultSigned, out ulong _resultUnsigned, out Exception _exc, bool _signedResult, int _startIndex, int _endIndex)
	{
		_resultSigned = 0L;
		_resultUnsigned = 0UL;
		_exc = null;
		if (_input == null)
		{
			if (!_tryParse)
			{
				_exc = new ArgumentNullException("_input");
			}
			return false;
		}
		if (_input.Length == 0)
		{
			if (!_tryParse)
			{
				_exc = new FormatException("Input string was not in the correct format: s.Length==0.");
			}
			return false;
		}
		if (_startIndex < 0 || _startIndex >= _input.Length)
		{
			if (!_tryParse)
			{
				_exc = new ArgumentException("_startIndex out of range (input='" + _input + "')", "_startIndex");
			}
			return false;
		}
		if (_endIndex < 0)
		{
			_endIndex = _input.Length - 1;
		}
		if (_endIndex >= _input.Length)
		{
			if (!_tryParse)
			{
				_exc = new ArgumentException("_endIndex out of range (input='" + _input + "')", "_endIndex");
			}
			return false;
		}
		if ((_numberStyle & NumberStyles.AllowHexSpecifier) != NumberStyles.None)
		{
			NumberStyles numberStyles = _numberStyle ^ NumberStyles.AllowHexSpecifier;
			if ((numberStyles & NumberStyles.AllowLeadingWhite) != NumberStyles.None)
			{
				numberStyles ^= NumberStyles.AllowLeadingWhite;
			}
			if ((numberStyles & NumberStyles.AllowTrailingWhite) != NumberStyles.None)
			{
				numberStyles ^= NumberStyles.AllowTrailingWhite;
			}
			if (numberStyles != NumberStyles.None)
			{
				if (!_tryParse)
				{
					_exc = new ArgumentException("With AllowHexSpecifier only AllowLeadingWhite and AllowTrailingWhite are permitted (input='" + _input + "')");
				}
				return false;
			}
		}
		else if (_numberStyle > NumberStyles.Any)
		{
			if (!_tryParse)
			{
				_exc = new ArgumentException("Not a valid number style (input='" + _input + "')");
			}
			return false;
		}
		bool flag = (_numberStyle & NumberStyles.AllowCurrencySymbol) > NumberStyles.None;
		bool flag2 = (_numberStyle & NumberStyles.AllowHexSpecifier) > NumberStyles.None;
		bool flag3 = (_numberStyle & NumberStyles.AllowThousands) > NumberStyles.None;
		bool flag4 = (_numberStyle & NumberStyles.AllowDecimalPoint) > NumberStyles.None;
		bool flag5 = (_numberStyle & NumberStyles.AllowParentheses) > NumberStyles.None;
		bool flag6 = (_numberStyle & NumberStyles.AllowTrailingSign) > NumberStyles.None;
		bool flag7 = (_numberStyle & NumberStyles.AllowLeadingSign) > NumberStyles.None;
		bool flag8 = (_numberStyle & NumberStyles.AllowTrailingWhite) > NumberStyles.None;
		bool flag9 = (_numberStyle & NumberStyles.AllowLeadingWhite) > NumberStyles.None;
		int num = _startIndex;
		if (flag9)
		{
			while (num <= _endIndex && char.IsWhiteSpace(_input[num]))
			{
				num++;
			}
			if (num > _endIndex)
			{
				if (!_tryParse)
				{
					_exc = new FormatException("Input string was not in the correct format (input='" + _input + "')");
				}
				return false;
			}
		}
		bool flag10 = false;
		bool flag11 = false;
		bool flag12 = false;
		bool flag13 = false;
		if (flag5 && _input[num] == '(')
		{
			flag10 = true;
			flag12 = true;
			flag11 = true;
			num++;
			if (flag9)
			{
				while (num <= _endIndex && char.IsWhiteSpace(_input[num]))
				{
					num++;
				}
				if (num > _endIndex)
				{
					if (!_tryParse)
					{
						_exc = new FormatException("Input string was not in the correct format (input='" + _input + "')");
					}
					return false;
				}
			}
			if (_input[num] == '-')
			{
				if (!_tryParse)
				{
					_exc = new FormatException("Input string was not in the correct format: Has Negative Sign (input='" + _input + "')");
				}
				return false;
			}
			if (_input[num] == '+')
			{
				if (!_tryParse)
				{
					_exc = new FormatException("Input string was not in the correct format: Has Positive Sign (input='" + _input + "')");
				}
				return false;
			}
		}
		if (flag7 && !flag12)
		{
			if (_input[num] == '-')
			{
				flag11 = true;
				flag12 = true;
				num++;
			}
			else if (_input[num] == '+')
			{
				flag12 = true;
				num++;
			}
			if (flag12 && flag9)
			{
				while (num <= _endIndex && char.IsWhiteSpace(_input[num]))
				{
					num++;
				}
				if (num > _endIndex)
				{
					if (!_tryParse)
					{
						_exc = new FormatException("Input string was not in the correct format (input='" + _input + "')");
					}
					return false;
				}
			}
		}
		if (flag)
		{
			if (_input[num] == '¤')
			{
				flag13 = true;
				num++;
			}
			if (flag13)
			{
				if (flag9)
				{
					while (num <= _endIndex && char.IsWhiteSpace(_input[num]))
					{
						num++;
					}
					if (num > _endIndex)
					{
						if (!_tryParse)
						{
							_exc = new FormatException("Input string was not in the correct format (input='" + _input + "')");
						}
						return false;
					}
				}
				if (!flag12 && flag7)
				{
					if (_input[num] == '-')
					{
						flag11 = true;
						flag12 = true;
						num++;
					}
					else if (_input[num] == '+')
					{
						flag11 = false;
						flag12 = true;
						num++;
					}
					if (flag12 && flag9)
					{
						while (num <= _endIndex && char.IsWhiteSpace(_input[num]))
						{
							num++;
						}
						if (num > _endIndex)
						{
							if (!_tryParse)
							{
								_exc = new FormatException("Input string was not in the correct format (input='" + _input + "')");
							}
							return false;
						}
					}
				}
			}
		}
		ulong num2 = 0UL;
		int num3 = 0;
		bool flag14 = false;
		for (;;)
		{
			char c = _input[num];
			if ((c ^ '0') > '\t' && (!flag2 || ((c < 'A' || c > 'F') && (c < 'a' || c > 'f'))))
			{
				if (!flag3 || !StringParsers.findOther(ref num, _input, ','))
				{
					if (flag14 || !flag4 || !StringParsers.findOther(ref num, _input, '.'))
					{
						goto IL_052E;
					}
					flag14 = true;
				}
			}
			else if (flag2)
			{
				num3++;
				int num4;
				if ((c ^ '0') <= '\t')
				{
					num4 = (int)(c - '0');
				}
				else if (c < 'a')
				{
					num4 = (int)(c - 'A' + '\n');
				}
				else
				{
					num4 = (int)(c - 'a' + '\n');
				}
				try
				{
					num2 = checked(num2 * 16UL + (ulong)num4);
				}
				catch (OverflowException ex)
				{
					if (!_tryParse)
					{
						_exc = ex;
					}
					return false;
				}
				num++;
			}
			else if (flag14)
			{
				num3++;
				if (c != '0')
				{
					break;
				}
				num++;
			}
			else
			{
				num3++;
				checked
				{
					try
					{
						num2 = num2 * 10UL + (ulong)(unchecked((long)(checked(c - '0'))));
					}
					catch (OverflowException)
					{
						if (!_tryParse)
						{
							_exc = new OverflowException("Value too large or too small (input='" + _input + "')");
						}
						return false;
					}
				}
				num++;
			}
			if (num > _endIndex)
			{
				goto IL_052E;
			}
		}
		if (!_tryParse)
		{
			_exc = new OverflowException("Value too large or too small (input='" + _input + "')");
		}
		return false;
		IL_052E:
		if (num3 == 0)
		{
			if (!_tryParse)
			{
				_exc = new FormatException("Input string was not in the correct format: nDigits == 0 (input='" + _input + "')");
			}
			return false;
		}
		if (flag6 && !flag12)
		{
			if (_input[num] == '-')
			{
				flag11 = true;
				flag12 = true;
				num++;
			}
			else if (_input[num] == '+')
			{
				flag11 = false;
				flag12 = true;
				num++;
			}
			if (flag12)
			{
				if (flag8)
				{
					while (num <= _endIndex && char.IsWhiteSpace(_input[num]))
					{
						num++;
					}
					if (num > _endIndex)
					{
						if (!_tryParse)
						{
							_exc = new FormatException("Input string was not in the correct format (input='" + _input + "')");
						}
						return false;
					}
				}
				if (flag && _input[num] == '¤')
				{
					flag13 = true;
					num++;
				}
			}
		}
		if (flag && !flag13)
		{
			if (_input[num] == '¤')
			{
				flag13 = true;
				num++;
			}
			if (flag13 && num < _input.Length)
			{
				if (flag8)
				{
					while (num <= _endIndex && char.IsWhiteSpace(_input[num]))
					{
						num++;
					}
					if (num > _endIndex)
					{
						if (!_tryParse)
						{
							_exc = new FormatException("Input string was not in the correct format (input='" + _input + "')");
						}
						return false;
					}
				}
				if (!flag12 && flag6)
				{
					if (_input[num] == '-')
					{
						flag11 = true;
						flag12 = true;
						num++;
					}
					else if (_input[num] == '+')
					{
						flag11 = false;
						flag12 = true;
						num++;
					}
				}
			}
		}
		if (flag8 && num <= _endIndex)
		{
			while (num <= _endIndex && char.IsWhiteSpace(_input[num]))
			{
				num++;
			}
		}
		if (flag10)
		{
			if (num > _endIndex || _input[num++] != ')')
			{
				if (!_tryParse)
				{
					_exc = new FormatException("Input string was not in the correct format: No room for close parens (input='" + _input + "')");
				}
				return false;
			}
			if (flag8 && num <= _endIndex)
			{
				while (num <= _endIndex && char.IsWhiteSpace(_input[num]))
				{
					num++;
				}
			}
		}
		if (num <= _endIndex && _input[num] != '\0')
		{
			if (!_tryParse)
			{
				_exc = new FormatException(string.Format("Input string was not in the correct format: Did not parse entire string. pos = {0} endIndex = {1} (input='{2}')", num, _endIndex, _input));
			}
			return false;
		}
		if (_signedResult)
		{
			if (flag11)
			{
				ulong num5 = 9223372036854775808UL;
				if (num2 > num5)
				{
					if (!_tryParse)
					{
						_exc = new OverflowException("Value too large or too small (input='" + _input + "')");
					}
					return false;
				}
				_resultSigned = (long)(ulong.MaxValue * (num2 - 1UL) - 1UL);
				return true;
			}
			else
			{
				ulong num6 = 9223372036854775807UL;
				if (num2 > num6)
				{
					if (!_tryParse)
					{
						_exc = new OverflowException("Value too large or too small (input='" + _input + "')");
					}
					return false;
				}
				_resultSigned = (long)num2;
				return true;
			}
		}
		else
		{
			if (flag11)
			{
				if (!_tryParse)
				{
					_exc = new OverflowException("Negative number (input='" + _input + "')");
				}
				return false;
			}
			_resultUnsigned = num2;
			return true;
		}
		bool flag15;
		return flag15;
	}

	// Token: 0x04007ACA RID: 31434
	[PublicizedFrom(EAccessModifier.Private)]
	public const char NEGATIVE_SIGN = '-';

	// Token: 0x04007ACB RID: 31435
	[PublicizedFrom(EAccessModifier.Private)]
	public const char POSITIVE_SIGN = '+';

	// Token: 0x04007ACC RID: 31436
	[PublicizedFrom(EAccessModifier.Private)]
	public const char CURRENCY_SYMBOL = '¤';

	// Token: 0x04007ACD RID: 31437
	[PublicizedFrom(EAccessModifier.Private)]
	public const char DECIMAL_SEP = '.';

	// Token: 0x04007ACE RID: 31438
	[PublicizedFrom(EAccessModifier.Private)]
	public const char THOUSANDS_SEP = ',';

	// Token: 0x04007ACF RID: 31439
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly string NAN_SYMBOL = "NaN";

	// Token: 0x04007AD0 RID: 31440
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly string POSITIVE_INFINITY_SYMBOL = "Infinity";

	// Token: 0x04007AD1 RID: 31441
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly string NEGATIVE_INFINITY_SYMBOL = "-Infinity";

	// Token: 0x02001496 RID: 5270
	public struct SeparatorPositions
	{
		// Token: 0x0600A573 RID: 42355 RVA: 0x003DA33C File Offset: 0x003D853C
		public SeparatorPositions(int _tmp)
		{
			this.TotalFound = 0;
			this.Sep1 = -1;
			this.Sep2 = -1;
			this.Sep3 = -1;
			this.Sep4 = -1;
		}

		// Token: 0x04007AD2 RID: 31442
		public int TotalFound;

		// Token: 0x04007AD3 RID: 31443
		public int Sep1;

		// Token: 0x04007AD4 RID: 31444
		public int Sep2;

		// Token: 0x04007AD5 RID: 31445
		public int Sep3;

		// Token: 0x04007AD6 RID: 31446
		public int Sep4;
	}

	// Token: 0x02001497 RID: 5271
	[PublicizedFrom(EAccessModifier.Private)]
	public enum EFloatParseState
	{
		// Token: 0x04007AD8 RID: 31448
		SignOrIntegralDigit,
		// Token: 0x04007AD9 RID: 31449
		IntegralDigit,
		// Token: 0x04007ADA RID: 31450
		DecimalDigit,
		// Token: 0x04007ADB RID: 31451
		ExponentialTest,
		// Token: 0x04007ADC RID: 31452
		ExponentialSignOrDigit,
		// Token: 0x04007ADD RID: 31453
		ExponentialDigit,
		// Token: 0x04007ADE RID: 31454
		TrailingWs
	}
}
