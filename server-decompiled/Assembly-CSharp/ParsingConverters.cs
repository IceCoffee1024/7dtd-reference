using System;
using InControl;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200111F RID: 4383
[XuiXmlAttributeConvertersClass]
[global::UnityEngine.Scripting.Preserve]
public static class ParsingConverters
{
	// Token: 0x06008ABE RID: 35518 RVA: 0x0034F410 File Offset: 0x0034D610
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

	// Token: 0x06008ABF RID: 35519 RVA: 0x0034F464 File Offset: 0x0034D664
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

	// Token: 0x06008AC0 RID: 35520 RVA: 0x0034F4BD File Offset: 0x0034D6BD
	[XuiXmlAttributeConverter]
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool parseStringList(string _inputValue, out string[] _outputValue)
	{
		_outputValue = StringParsers.ParseList<string>(_inputValue, ';', (string _s, int _startIndex, int _endIndex) => _s.Substring(_startIndex, _endIndex - _startIndex + 1)).ToArray();
		return true;
	}

	// Token: 0x06008AC1 RID: 35521 RVA: 0x0034F4EE File Offset: 0x0034D6EE
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

	// Token: 0x06008AC2 RID: 35522 RVA: 0x0034F51E File Offset: 0x0034D71E
	[XuiXmlAttributeConverter]
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool parseColorToColor32(Color _inputValue, out Color32 _outputValue)
	{
		_outputValue = _inputValue;
		return true;
	}

	// Token: 0x06008AC3 RID: 35523 RVA: 0x0034F52D File Offset: 0x0034D72D
	[XuiXmlAttributeConverter]
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool parseColor32ToColor(Color32 _inputValue, out Color _outputValue)
	{
		_outputValue = _inputValue;
		return true;
	}

	// Token: 0x06008AC4 RID: 35524 RVA: 0x0034F53C File Offset: 0x0034D73C
	[XuiXmlAttributeConverter]
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool parseDoubleToFloat(double _inputValue, out float _outputValue)
	{
		_outputValue = (float)_inputValue;
		return true;
	}
}
