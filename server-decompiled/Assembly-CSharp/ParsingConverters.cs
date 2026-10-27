using System;
using InControl;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200112F RID: 4399
[XuiXmlAttributeConvertersClass]
[global::UnityEngine.Scripting.Preserve]
public static class ParsingConverters
{
	// Token: 0x06008B12 RID: 35602 RVA: 0x003514C0 File Offset: 0x0034F6C0
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

	// Token: 0x06008B13 RID: 35603 RVA: 0x00351514 File Offset: 0x0034F714
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

	// Token: 0x06008B14 RID: 35604 RVA: 0x0035156D File Offset: 0x0034F76D
	[XuiXmlAttributeConverter]
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool parseStringList(string _inputValue, out string[] _outputValue)
	{
		_outputValue = StringParsers.ParseList<string>(_inputValue, ';', (string _s, int _startIndex, int _endIndex) => _s.Substring(_startIndex, _endIndex - _startIndex + 1)).ToArray();
		return true;
	}

	// Token: 0x06008B15 RID: 35605 RVA: 0x0035159E File Offset: 0x0034F79E
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

	// Token: 0x06008B16 RID: 35606 RVA: 0x003515CE File Offset: 0x0034F7CE
	[XuiXmlAttributeConverter]
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool parseColorToColor32(Color _inputValue, out Color32 _outputValue)
	{
		_outputValue = _inputValue;
		return true;
	}

	// Token: 0x06008B17 RID: 35607 RVA: 0x003515DD File Offset: 0x0034F7DD
	[XuiXmlAttributeConverter]
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool parseColor32ToColor(Color32 _inputValue, out Color _outputValue)
	{
		_outputValue = _inputValue;
		return true;
	}

	// Token: 0x06008B18 RID: 35608 RVA: 0x003515EC File Offset: 0x0034F7EC
	[XuiXmlAttributeConverter]
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool parseDoubleToFloat(double _inputValue, out float _outputValue)
	{
		_outputValue = (float)_inputValue;
		return true;
	}
}
