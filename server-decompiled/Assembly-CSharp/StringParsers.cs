using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020014E4 RID: 5348
[XuiXmlAttributeConvertersClass]
[Preserve]
public static class StringParsers
{
	// Token: 0x0600A733 RID: 42803 RVA: 0x003E3CF4 File Offset: 0x003E1EF4
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

	// Token: 0x0600A734 RID: 42804 RVA: 0x003E3D3C File Offset: 0x003E1F3C
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

	// Token: 0x0600A735 RID: 42805 RVA: 0x003E3D80 File Offset: 0x003E1F80
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

	// Token: 0x0600A736 RID: 42806 RVA: 0x003E3DBC File Offset: 0x003E1FBC
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

	// Token: 0x0600A737 RID: 42807 RVA: 0x003E3DF5 File Offset: 0x003E1FF5
	[XuiXmlAttributeConverter]
	public static bool TryParseSInt8(string _input, out sbyte _result)
	{
		return StringParsers.TryParseSInt8(_input, out _result, 0, -1, NumberStyles.Integer);
	}

	// Token: 0x0600A738 RID: 42808 RVA: 0x003E3E01 File Offset: 0x003E2001
	[XuiXmlAttributeConverter]
	public static bool TryParseUInt8(string _input, out byte _result)
	{
		return StringParsers.TryParseUInt8(_input, out _result, 0, -1, NumberStyles.Integer);
	}

	// Token: 0x0600A739 RID: 42809 RVA: 0x003E3E10 File Offset: 0x003E2010
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

	// Token: 0x0600A73A RID: 42810 RVA: 0x003E3E5C File Offset: 0x003E205C
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

	// Token: 0x0600A73B RID: 42811 RVA: 0x003E3EA0 File Offset: 0x003E20A0
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

	// Token: 0x0600A73C RID: 42812 RVA: 0x003E3EE4 File Offset: 0x003E20E4
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

	// Token: 0x0600A73D RID: 42813 RVA: 0x003E3F1D File Offset: 0x003E211D
	[XuiXmlAttributeConverter]
	public static bool TryParseSInt16(string _input, out short _result)
	{
		return StringParsers.TryParseSInt16(_input, out _result, 0, -1, NumberStyles.Integer);
	}

	// Token: 0x0600A73E RID: 42814 RVA: 0x003E3F29 File Offset: 0x003E2129
	[XuiXmlAttributeConverter]
	public static bool TryParseUInt16(string _input, out ushort _result)
	{
		return StringParsers.TryParseUInt16(_input, out _result, 0, -1, NumberStyles.Integer);
	}

	// Token: 0x0600A73F RID: 42815 RVA: 0x003E3F38 File Offset: 0x003E2138
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

	// Token: 0x0600A740 RID: 42816 RVA: 0x003E3F84 File Offset: 0x003E2184
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

	// Token: 0x0600A741 RID: 42817 RVA: 0x003E3FC4 File Offset: 0x003E21C4
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

	// Token: 0x0600A742 RID: 42818 RVA: 0x003E4008 File Offset: 0x003E2208
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

	// Token: 0x0600A743 RID: 42819 RVA: 0x003E403D File Offset: 0x003E223D
	[XuiXmlAttributeConverter]
	public static bool TryParseSInt32(string _input, out int _result)
	{
		return StringParsers.TryParseSInt32(_input, out _result, 0, -1, NumberStyles.Integer);
	}

	// Token: 0x0600A744 RID: 42820 RVA: 0x003E4049 File Offset: 0x003E2249
	[XuiXmlAttributeConverter]
	public static bool TryParseUInt32(string _input, out uint _result)
	{
		return StringParsers.TryParseUInt32(_input, out _result, 0, -1, NumberStyles.Integer);
	}

	// Token: 0x0600A745 RID: 42821 RVA: 0x003E4058 File Offset: 0x003E2258
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

	// Token: 0x0600A746 RID: 42822 RVA: 0x003E407C File Offset: 0x003E227C
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

	// Token: 0x0600A747 RID: 42823 RVA: 0x003E40A0 File Offset: 0x003E22A0
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

	// Token: 0x0600A748 RID: 42824 RVA: 0x003E40CC File Offset: 0x003E22CC
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

	// Token: 0x0600A749 RID: 42825 RVA: 0x003E40F7 File Offset: 0x003E22F7
	[XuiXmlAttributeConverter]
	public static bool TryParseSInt64(string _input, out long _result)
	{
		return StringParsers.TryParseSInt64(_input, out _result, 0, -1, NumberStyles.Integer);
	}

	// Token: 0x0600A74A RID: 42826 RVA: 0x003E4103 File Offset: 0x003E2303
	[XuiXmlAttributeConverter]
	public static bool TryParseUInt64(string _input, out ulong _result)
	{
		return StringParsers.TryParseUInt64(_input, out _result, 0, -1, NumberStyles.Integer);
	}

	// Token: 0x0600A74B RID: 42827 RVA: 0x003E4110 File Offset: 0x003E2310
	public static float ParseFloat(string _input, int _startIndex = 0, int _endIndex = -1, NumberStyles _style = NumberStyles.Any)
	{
		double num = StringParsers.ParseDouble(_input, _startIndex, _endIndex, _style);
		if (num - 3.4028234663852886E+38 > 3.6147112457961776E+29 && !double.IsPositiveInfinity(num))
		{
			throw new OverflowException();
		}
		return (float)num;
	}

	// Token: 0x0600A74C RID: 42828 RVA: 0x003E4150 File Offset: 0x003E2350
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

	// Token: 0x0600A74D RID: 42829 RVA: 0x003E41A3 File Offset: 0x003E23A3
	[XuiXmlAttributeConverter]
	public static bool TryParseFloat(string _input, out float _result)
	{
		return StringParsers.TryParseFloat(_input, out _result, 0, -1, NumberStyles.Any);
	}

	// Token: 0x0600A74E RID: 42830 RVA: 0x003E41B4 File Offset: 0x003E23B4
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

	// Token: 0x0600A74F RID: 42831 RVA: 0x003E41D4 File Offset: 0x003E23D4
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

	// Token: 0x0600A750 RID: 42832 RVA: 0x003E41FF File Offset: 0x003E23FF
	[XuiXmlAttributeConverter]
	public static bool TryParseDouble(string _input, out double _result)
	{
		return StringParsers.TryParseDouble(_input, out _result, 0, -1, NumberStyles.Any);
	}

	// Token: 0x0600A751 RID: 42833 RVA: 0x003E420F File Offset: 0x003E240F
	public static DateTime ParseDateTime(string _s)
	{
		return DateTime.Parse(_s, Utils.StandardCulture);
	}

	// Token: 0x0600A752 RID: 42834 RVA: 0x003E421C File Offset: 0x003E241C
	[XuiXmlAttributeConverter]
	public static bool TryParseDateTime(string _s, out DateTime _result)
	{
		return DateTime.TryParse(_s, Utils.StandardCulture, DateTimeStyles.None, out _result);
	}

	// Token: 0x0600A753 RID: 42835 RVA: 0x003E422C File Offset: 0x003E242C
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

	// Token: 0x0600A754 RID: 42836 RVA: 0x003E424C File Offset: 0x003E244C
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

	// Token: 0x0600A755 RID: 42837 RVA: 0x003E426F File Offset: 0x003E246F
	[XuiXmlAttributeConverter]
	public static bool TryParseBool(string _input, out bool _result)
	{
		return StringParsers.TryParseBool(_input, out _result, 0, -1, true);
	}

	// Token: 0x0600A756 RID: 42838 RVA: 0x003E427C File Offset: 0x003E247C
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

	// Token: 0x0600A757 RID: 42839 RVA: 0x003E4314 File Offset: 0x003E2514
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

	// Token: 0x0600A758 RID: 42840 RVA: 0x003E43A4 File Offset: 0x003E25A4
	public static Vector2 ParseVector2(string _input)
	{
		StringParsers.SeparatorPositions separatorPositions = StringParsers.GetSeparatorPositions(_input, ',', 1, 0, -1);
		if (separatorPositions.TotalFound != 1)
		{
			return Vector2.zero;
		}
		return new Vector2(StringParsers.ParseFloat(_input, 0, separatorPositions.Sep1 - 1, NumberStyles.Any), StringParsers.ParseFloat(_input, separatorPositions.Sep1 + 1, -1, NumberStyles.Any));
	}

	// Token: 0x0600A759 RID: 42841 RVA: 0x003E43FC File Offset: 0x003E25FC
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

	// Token: 0x0600A75A RID: 42842 RVA: 0x003E44BC File Offset: 0x003E26BC
	public static Vector2 ParseVector2(string _input, float _defaultValue)
	{
		StringParsers.SeparatorPositions separatorPositions = StringParsers.GetSeparatorPositions(_input, ',', 1, 0, -1);
		if (separatorPositions.TotalFound == 0)
		{
			return new Vector2(StringParsers.ParseFloat(_input, 0, -1, NumberStyles.Any), _defaultValue);
		}
		return new Vector2(StringParsers.ParseFloat(_input, 0, separatorPositions.Sep1 - 1, NumberStyles.Any), StringParsers.ParseFloat(_input, separatorPositions.Sep1 + 1, -1, NumberStyles.Any));
	}

	// Token: 0x0600A75B RID: 42843 RVA: 0x003E4520 File Offset: 0x003E2720
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

	// Token: 0x0600A75C RID: 42844 RVA: 0x003E4600 File Offset: 0x003E2800
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

	// Token: 0x0600A75D RID: 42845 RVA: 0x003E46E4 File Offset: 0x003E28E4
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

	// Token: 0x0600A75E RID: 42846 RVA: 0x003E479C File Offset: 0x003E299C
	public static Vector4 ParseVector4(string _input)
	{
		StringParsers.SeparatorPositions separatorPositions = StringParsers.GetSeparatorPositions(_input, ',', 3, 0, -1);
		if (separatorPositions.TotalFound != 3)
		{
			return Vector4.zero;
		}
		return new Vector4(StringParsers.ParseFloat(_input, 0, separatorPositions.Sep1 - 1, NumberStyles.Any), StringParsers.ParseFloat(_input, separatorPositions.Sep1 + 1, separatorPositions.Sep2 - 1, NumberStyles.Any), StringParsers.ParseFloat(_input, separatorPositions.Sep2 + 1, separatorPositions.Sep3 - 1, NumberStyles.Any), StringParsers.ParseFloat(_input, separatorPositions.Sep3 + 1, -1, NumberStyles.Any));
	}

	// Token: 0x0600A75F RID: 42847 RVA: 0x003E4828 File Offset: 0x003E2A28
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

	// Token: 0x0600A760 RID: 42848 RVA: 0x003E4930 File Offset: 0x003E2B30
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

	// Token: 0x0600A761 RID: 42849 RVA: 0x003E4990 File Offset: 0x003E2B90
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

	// Token: 0x0600A762 RID: 42850 RVA: 0x003E49F0 File Offset: 0x003E2BF0
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

	// Token: 0x0600A763 RID: 42851 RVA: 0x003E4A88 File Offset: 0x003E2C88
	public static Quaternion ParseQuaternion(string _input)
	{
		StringParsers.SeparatorPositions separatorPositions = StringParsers.GetSeparatorPositions(_input, ',', 3, 0, -1);
		if (separatorPositions.TotalFound != 3)
		{
			return Quaternion.identity;
		}
		return new Quaternion(StringParsers.ParseFloat(_input, 0, separatorPositions.Sep1 - 1, NumberStyles.Any), StringParsers.ParseFloat(_input, separatorPositions.Sep1 + 1, separatorPositions.Sep2 - 1, NumberStyles.Any), StringParsers.ParseFloat(_input, separatorPositions.Sep2 + 1, separatorPositions.Sep3 - 1, NumberStyles.Any), StringParsers.ParseFloat(_input, separatorPositions.Sep3 + 1, -1, NumberStyles.Any));
	}

	// Token: 0x0600A764 RID: 42852 RVA: 0x003E4B14 File Offset: 0x003E2D14
	public static Color ParseColor(string _input)
	{
		Color color;
		if (!StringParsers.TryParseColor(_input, out color))
		{
			return Color.white;
		}
		return color;
	}

	// Token: 0x0600A765 RID: 42853 RVA: 0x003E4B34 File Offset: 0x003E2D34
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

	// Token: 0x0600A766 RID: 42854 RVA: 0x003E4C34 File Offset: 0x003E2E34
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

	// Token: 0x0600A767 RID: 42855 RVA: 0x003E4CC0 File Offset: 0x003E2EC0
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

	// Token: 0x0600A768 RID: 42856 RVA: 0x003E4DD4 File Offset: 0x003E2FD4
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

	// Token: 0x0600A769 RID: 42857 RVA: 0x003E4E80 File Offset: 0x003E3080
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

	// Token: 0x0600A76A RID: 42858 RVA: 0x003E4F44 File Offset: 0x003E3144
	public static Vector2d ParseVector2d(string _input)
	{
		StringParsers.SeparatorPositions separatorPositions = StringParsers.GetSeparatorPositions(_input, ',', 1, 0, -1);
		if (separatorPositions.TotalFound != 1)
		{
			return Vector2d.Zero;
		}
		return new Vector2d(StringParsers.ParseDouble(_input, 0, separatorPositions.Sep1 - 1, NumberStyles.Any), StringParsers.ParseDouble(_input, separatorPositions.Sep1 + 1, -1, NumberStyles.Any));
	}

	// Token: 0x0600A76B RID: 42859 RVA: 0x003E4F9C File Offset: 0x003E319C
	public static Vector3d ParseVector3d(string _input)
	{
		StringParsers.SeparatorPositions separatorPositions = StringParsers.GetSeparatorPositions(_input, ',', 2, 0, -1);
		if (separatorPositions.TotalFound != 2)
		{
			return Vector3d.Zero;
		}
		return new Vector3d(StringParsers.ParseDouble(_input, 0, separatorPositions.Sep1 - 1, NumberStyles.Any), StringParsers.ParseDouble(_input, separatorPositions.Sep1 + 1, separatorPositions.Sep2 - 1, NumberStyles.Any), StringParsers.ParseDouble(_input, separatorPositions.Sep2 + 1, -1, NumberStyles.Any));
	}

	// Token: 0x0600A76C RID: 42860 RVA: 0x003E500C File Offset: 0x003E320C
	public static Vector2i ParseVector2i(string _input, char _customSep = ',')
	{
		StringParsers.SeparatorPositions separatorPositions = StringParsers.GetSeparatorPositions(_input, _customSep, 1, 0, -1);
		if (separatorPositions.TotalFound != 1)
		{
			return Vector2i.zero;
		}
		return new Vector2i(StringParsers.ParseSInt32(_input, 0, separatorPositions.Sep1 - 1, NumberStyles.Integer), StringParsers.ParseSInt32(_input, separatorPositions.Sep1 + 1, -1, NumberStyles.Integer));
	}

	// Token: 0x0600A76D RID: 42861 RVA: 0x003E5058 File Offset: 0x003E3258
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

	// Token: 0x0600A76E RID: 42862 RVA: 0x003E5110 File Offset: 0x003E3310
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

	// Token: 0x0600A76F RID: 42863 RVA: 0x003E51C4 File Offset: 0x003E33C4
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

	// Token: 0x0600A770 RID: 42864 RVA: 0x003E529C File Offset: 0x003E349C
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

	// Token: 0x0600A771 RID: 42865 RVA: 0x003E5304 File Offset: 0x003E3504
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

	// Token: 0x0600A772 RID: 42866 RVA: 0x003E53A4 File Offset: 0x003E35A4
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

	// Token: 0x0600A773 RID: 42867 RVA: 0x003E544C File Offset: 0x003E364C
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

	// Token: 0x0600A774 RID: 42868 RVA: 0x003E54BD File Offset: 0x003E36BD
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

	// Token: 0x0600A775 RID: 42869 RVA: 0x003E54D4 File Offset: 0x003E36D4
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

	// Token: 0x0600A776 RID: 42870 RVA: 0x003E5654 File Offset: 0x003E3854
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

	// Token: 0x0600A777 RID: 42871 RVA: 0x003E5C28 File Offset: 0x003E3E28
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

	// Token: 0x0600A778 RID: 42872 RVA: 0x003E5DBC File Offset: 0x003E3FBC
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

	// Token: 0x0600A779 RID: 42873 RVA: 0x003E6080 File Offset: 0x003E4280
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

	// Token: 0x04007C67 RID: 31847
	[PublicizedFrom(EAccessModifier.Private)]
	public const char NEGATIVE_SIGN = '-';

	// Token: 0x04007C68 RID: 31848
	[PublicizedFrom(EAccessModifier.Private)]
	public const char POSITIVE_SIGN = '+';

	// Token: 0x04007C69 RID: 31849
	[PublicizedFrom(EAccessModifier.Private)]
	public const char CURRENCY_SYMBOL = '¤';

	// Token: 0x04007C6A RID: 31850
	[PublicizedFrom(EAccessModifier.Private)]
	public const char DECIMAL_SEP = '.';

	// Token: 0x04007C6B RID: 31851
	[PublicizedFrom(EAccessModifier.Private)]
	public const char THOUSANDS_SEP = ',';

	// Token: 0x04007C6C RID: 31852
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly string NAN_SYMBOL = "NaN";

	// Token: 0x04007C6D RID: 31853
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly string POSITIVE_INFINITY_SYMBOL = "Infinity";

	// Token: 0x04007C6E RID: 31854
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly string NEGATIVE_INFINITY_SYMBOL = "-Infinity";

	// Token: 0x020014E5 RID: 5349
	public struct SeparatorPositions
	{
		// Token: 0x0600A77B RID: 42875 RVA: 0x003E68D4 File Offset: 0x003E4AD4
		public SeparatorPositions(int _tmp)
		{
			this.TotalFound = 0;
			this.Sep1 = -1;
			this.Sep2 = -1;
			this.Sep3 = -1;
			this.Sep4 = -1;
		}

		// Token: 0x04007C6F RID: 31855
		public int TotalFound;

		// Token: 0x04007C70 RID: 31856
		public int Sep1;

		// Token: 0x04007C71 RID: 31857
		public int Sep2;

		// Token: 0x04007C72 RID: 31858
		public int Sep3;

		// Token: 0x04007C73 RID: 31859
		public int Sep4;
	}

	// Token: 0x020014E6 RID: 5350
	[PublicizedFrom(EAccessModifier.Private)]
	public enum EFloatParseState
	{
		// Token: 0x04007C75 RID: 31861
		SignOrIntegralDigit,
		// Token: 0x04007C76 RID: 31862
		IntegralDigit,
		// Token: 0x04007C77 RID: 31863
		DecimalDigit,
		// Token: 0x04007C78 RID: 31864
		ExponentialTest,
		// Token: 0x04007C79 RID: 31865
		ExponentialSignOrDigit,
		// Token: 0x04007C7A RID: 31866
		ExponentialDigit,
		// Token: 0x04007C7B RID: 31867
		TrailingWs
	}
}
