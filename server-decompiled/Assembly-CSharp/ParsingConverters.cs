using System;
using InControl;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001109 RID: 4361
[XuiXmlAttributeConvertersClass]
[global::UnityEngine.Scripting.Preserve]
public static class ParsingConverters
{
	// Token: 0x06008A5A RID: 35418 RVA: 0x0034B118 File Offset: 0x00349318
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

	// Token: 0x06008A5B RID: 35419 RVA: 0x0034B16C File Offset: 0x0034936C
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

	// Token: 0x06008A5C RID: 35420 RVA: 0x0034B1C5 File Offset: 0x003493C5
	[XuiXmlAttributeConverter]
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool parseStringList(string _inputValue, out string[] _outputValue)
	{
		_outputValue = StringParsers.ParseList<string>(_inputValue, ';', (string _s, int _startIndex, int _endIndex) => _s.Substring(_startIndex, _endIndex - _startIndex + 1)).ToArray();
		return true;
	}

	// Token: 0x06008A5D RID: 35421 RVA: 0x0034B1F6 File Offset: 0x003493F6
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

	// Token: 0x06008A5E RID: 35422 RVA: 0x0034B226 File Offset: 0x00349426
	[XuiXmlAttributeConverter]
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool parseColorToColor32(Color _inputValue, out Color32 _outputValue)
	{
		_outputValue = _inputValue;
		return true;
	}

	// Token: 0x06008A5F RID: 35423 RVA: 0x0034B235 File Offset: 0x00349435
	[XuiXmlAttributeConverter]
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool parseColor32ToColor(Color32 _inputValue, out Color _outputValue)
	{
		_outputValue = _inputValue;
		return true;
	}

	// Token: 0x06008A60 RID: 35424 RVA: 0x0034B244 File Offset: 0x00349444
	[XuiXmlAttributeConverter]
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool parseDoubleToFloat(double _inputValue, out float _outputValue)
	{
		_outputValue = (float)_inputValue;
		return true;
	}
}
