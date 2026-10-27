using System;
using System.Globalization;

// Token: 0x020011A7 RID: 4519
public readonly struct XUiSideSizes
{
	// Token: 0x1700118B RID: 4491
	// (get) Token: 0x06009080 RID: 36992 RVA: 0x00366102 File Offset: 0x00364302
	public int SumLeftRight
	{
		get
		{
			return this.Left + this.Right;
		}
	}

	// Token: 0x1700118C RID: 4492
	// (get) Token: 0x06009081 RID: 36993 RVA: 0x00366111 File Offset: 0x00364311
	public int SumTopBottom
	{
		get
		{
			return this.Top + this.Bottom;
		}
	}

	// Token: 0x06009082 RID: 36994 RVA: 0x00366120 File Offset: 0x00364320
	public XUiSideSizes(int _left, int _right, int _top, int _bottom)
	{
		this.Left = _left;
		this.Right = _right;
		this.Top = _top;
		this.Bottom = _bottom;
	}

	// Token: 0x06009083 RID: 36995 RVA: 0x0036613F File Offset: 0x0036433F
	public XUiSideSizes SetLeft(int _value)
	{
		return new XUiSideSizes(_value, this.Right, this.Top, this.Bottom);
	}

	// Token: 0x06009084 RID: 36996 RVA: 0x00366159 File Offset: 0x00364359
	public XUiSideSizes SetRight(int _value)
	{
		return new XUiSideSizes(this.Left, _value, this.Top, this.Bottom);
	}

	// Token: 0x06009085 RID: 36997 RVA: 0x00366173 File Offset: 0x00364373
	public XUiSideSizes SetTop(int _value)
	{
		return new XUiSideSizes(this.Left, this.Right, _value, this.Bottom);
	}

	// Token: 0x06009086 RID: 36998 RVA: 0x0036618D File Offset: 0x0036438D
	public XUiSideSizes SetBottom(int _value)
	{
		return new XUiSideSizes(this.Left, this.Right, this.Top, _value);
	}

	// Token: 0x06009087 RID: 36999 RVA: 0x003661A8 File Offset: 0x003643A8
	public static bool TryParse(string _value, out XUiSideSizes _result, string _valueName)
	{
		StringParsers.SeparatorPositions separatorPositions = StringParsers.GetSeparatorPositions(_value, ',', 3, 0, -1);
		if (separatorPositions.TotalFound > 3)
		{
			Log.Warning(string.Format("[XUi] Invalid number of values for {0}: {1}, max of 4 expected (input string: '{2}')", _valueName, separatorPositions.TotalFound, _value));
			_result = default(XUiSideSizes);
			return false;
		}
		int num;
		if (!StringParsers.TryParseSInt32(_value, out num, 0, separatorPositions.Sep1 - 1, NumberStyles.Integer))
		{
			Log.Warning(string.Concat(new string[] { "[XUi] ", _valueName, " can not be parsed, not an integer as 1st value (input string: '", _value, "')" }));
			_result = default(XUiSideSizes);
			return false;
		}
		if (separatorPositions.TotalFound == 0)
		{
			_result = new XUiSideSizes(num, num, num, num);
			return true;
		}
		int num2;
		if (!StringParsers.TryParseSInt32(_value, out num2, separatorPositions.Sep1 + 1, separatorPositions.Sep2 - 1, NumberStyles.Integer))
		{
			Log.Warning(string.Concat(new string[] { "[XUi] ", _valueName, " can not be parsed, not an integer as 2nd value (input string: '", _value, "')" }));
			_result = default(XUiSideSizes);
			return false;
		}
		if (separatorPositions.TotalFound == 1)
		{
			_result = new XUiSideSizes(num2, num2, num, num);
			return true;
		}
		int num3;
		if (!StringParsers.TryParseSInt32(_value, out num3, separatorPositions.Sep2 + 1, separatorPositions.Sep3 - 1, NumberStyles.Integer))
		{
			Log.Warning(string.Concat(new string[] { "[XUi] ", _valueName, " can not be parsed, not an integer as 3rd value (input string: '", _value, "')" }));
			_result = default(XUiSideSizes);
			return false;
		}
		if (separatorPositions.TotalFound == 2)
		{
			_result = new XUiSideSizes(num2, num2, num, num3);
			return true;
		}
		int num4;
		if (!StringParsers.TryParseSInt32(_value, out num4, separatorPositions.Sep3 + 1, separatorPositions.Sep4 - 1, NumberStyles.Integer))
		{
			Log.Warning(string.Concat(new string[] { "[XUi] ", _valueName, " can not be parsed, not an integer as 4th value (input string: '", _value, "')" }));
			_result = default(XUiSideSizes);
			return false;
		}
		_result = new XUiSideSizes(num4, num2, num, num3);
		return true;
	}

	// Token: 0x040069C5 RID: 27077
	public readonly int Left;

	// Token: 0x040069C6 RID: 27078
	public readonly int Right;

	// Token: 0x040069C7 RID: 27079
	public readonly int Top;

	// Token: 0x040069C8 RID: 27080
	public readonly int Bottom;
}
