using System;
using System.Globalization;

// Token: 0x02001165 RID: 4453
public readonly struct XUiSideSizes
{
	// Token: 0x17001103 RID: 4355
	// (get) Token: 0x06008EA8 RID: 36520 RVA: 0x0035ADAA File Offset: 0x00358FAA
	public int SumLeftRight
	{
		get
		{
			return this.Left + this.Right;
		}
	}

	// Token: 0x17001104 RID: 4356
	// (get) Token: 0x06008EA9 RID: 36521 RVA: 0x0035ADB9 File Offset: 0x00358FB9
	public int SumTopBottom
	{
		get
		{
			return this.Top + this.Bottom;
		}
	}

	// Token: 0x06008EAA RID: 36522 RVA: 0x0035ADC8 File Offset: 0x00358FC8
	public XUiSideSizes(int _left, int _right, int _top, int _bottom)
	{
		this.Left = _left;
		this.Right = _right;
		this.Top = _top;
		this.Bottom = _bottom;
	}

	// Token: 0x06008EAB RID: 36523 RVA: 0x0035ADE7 File Offset: 0x00358FE7
	public XUiSideSizes SetLeft(int _value)
	{
		return new XUiSideSizes(_value, this.Right, this.Top, this.Bottom);
	}

	// Token: 0x06008EAC RID: 36524 RVA: 0x0035AE01 File Offset: 0x00359001
	public XUiSideSizes SetRight(int _value)
	{
		return new XUiSideSizes(this.Left, _value, this.Top, this.Bottom);
	}

	// Token: 0x06008EAD RID: 36525 RVA: 0x0035AE1B File Offset: 0x0035901B
	public XUiSideSizes SetTop(int _value)
	{
		return new XUiSideSizes(this.Left, this.Right, _value, this.Bottom);
	}

	// Token: 0x06008EAE RID: 36526 RVA: 0x0035AE35 File Offset: 0x00359035
	public XUiSideSizes SetBottom(int _value)
	{
		return new XUiSideSizes(this.Left, this.Right, this.Top, _value);
	}

	// Token: 0x06008EAF RID: 36527 RVA: 0x0035AE50 File Offset: 0x00359050
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

	// Token: 0x04006850 RID: 26704
	public readonly int Left;

	// Token: 0x04006851 RID: 26705
	public readonly int Right;

	// Token: 0x04006852 RID: 26706
	public readonly int Top;

	// Token: 0x04006853 RID: 26707
	public readonly int Bottom;
}
