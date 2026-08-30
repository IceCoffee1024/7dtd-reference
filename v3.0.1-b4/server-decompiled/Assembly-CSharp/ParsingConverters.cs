using System;
using InControl;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020010F0 RID: 4336
[XuiXmlAttributeConvertersClass]
[global::UnityEngine.Scripting.Preserve]
public static class ParsingConverters
{
	// Token: 0x06008968 RID: 35176 RVA: 0x003468F4 File Offset: 0x00344AF4
	[XuiXmlAttributeConverter]
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool parseColor(string _inputValue, out Color _outputValue)
	{
		if (string.IsNullOrEmpty(_inputValue))
		{
			_outputValue = Color.white;
			return true;
		}
		if (_inputValue.Contains('.'))
		{
			return StringParsers.TryParseColor(_inputValue, out _outputValue);
		}
		Color32 color;
		if (!StringParsers.TryParseColor32(_inputValue, out color))
		{
			_outputValue = default(Color);
			return false;
		}
		_outputValue = color;
		return true;
	}

	// Token: 0x06008969 RID: 35177 RVA: 0x00346948 File Offset: 0x00344B48
	[XuiXmlAttributeConverter]
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool parseColor32(string _inputValue, out Color32 _outputValue)
	{
		if (string.IsNullOrEmpty(_inputValue))
		{
			_outputValue = Color.white;
			return true;
		}
		if (!_inputValue.Contains('.'))
		{
			return StringParsers.TryParseColor32(_inputValue, out _outputValue);
		}
		Color color;
		if (!StringParsers.TryParseColor(_inputValue, out color))
		{
			_outputValue = default(Color32);
			return false;
		}
		_outputValue = color;
		return true;
	}

	// Token: 0x0600896A RID: 35178 RVA: 0x003469A1 File Offset: 0x00344BA1
	[XuiXmlAttributeConverter]
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool parseStringList(string _inputValue, out string[] _outputValue)
	{
		_outputValue = StringParsers.ParseList<string>(_inputValue, ';', (string _s, int _startIndex, int _endIndex) => _s.Substring(_startIndex, _endIndex - _startIndex + 1)).ToArray();
		return true;
	}

	// Token: 0x0600896B RID: 35179 RVA: 0x003469D2 File Offset: 0x00344BD2
	[XuiXmlAttributeConverter]
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool parseInputAction(string _value, out PlayerAction _outputValue)
	{
		if (_value == "")
		{
			_outputValue = null;
			return true;
		}
		_outputValue = LocalPlayerUI.primaryUI.playerInput.GUIActions.GetPlayerActionByName(_value);
		return _outputValue != null;
	}

	// Token: 0x0600896C RID: 35180 RVA: 0x00346A02 File Offset: 0x00344C02
	[XuiXmlAttributeConverter]
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool parseColorToColor32(Color _inputValue, out Color32 _outputValue)
	{
		_outputValue = _inputValue;
		return true;
	}

	// Token: 0x0600896D RID: 35181 RVA: 0x00346A11 File Offset: 0x00344C11
	[XuiXmlAttributeConverter]
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool parseColor32ToColor(Color32 _inputValue, out Color _outputValue)
	{
		_outputValue = _inputValue;
		return true;
	}

	// Token: 0x0600896E RID: 35182 RVA: 0x00346A20 File Offset: 0x00344C20
	[XuiXmlAttributeConverter]
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool parseDoubleToFloat(double _inputValue, out float _outputValue)
	{
		_outputValue = (float)_inputValue;
		return true;
	}
}
