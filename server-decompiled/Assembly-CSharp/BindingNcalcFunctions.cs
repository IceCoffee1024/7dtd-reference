using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using NCalc;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020010E5 RID: 4325
[XuiBindingNcalcFunction]
[Preserve]
public static class BindingNcalcFunctions
{
	// Token: 0x0600890D RID: 35085 RVA: 0x00344D3C File Offset: 0x00342F3C
	[XuiBindingNcalcFunction("<ERROR>", 1, null)]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void localization(IBindingNcalc _bindingInstance, FunctionArgs _args, object[] _evaluatedArguments)
	{
		_args.Result = Localization.Get(_evaluatedArguments[0].ToString(), false, null);
	}

	// Token: 0x0600890E RID: 35086 RVA: 0x00344D54 File Offset: 0x00342F54
	[XuiBindingNcalcFunction(1f, 1, null)]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void cvar(IBindingNcalc _bindingInstance, FunctionArgs _args, object[] _evaluatedArguments)
	{
		string text = _evaluatedArguments[0].ToString();
		if (!_bindingInstance.RegisterVariable(new BindingInfoNcalc.VariableStateCVar(text)))
		{
			_bindingInstance.SetIndeterministic();
		}
		if (GameManager.Instance == null || GameManager.Instance.World == null)
		{
			return;
		}
		EntityPlayer primaryPlayer = GameManager.Instance.World.GetPrimaryPlayer();
		if (primaryPlayer == null)
		{
			return;
		}
		_args.Result = primaryPlayer.GetCVar(text);
	}

	// Token: 0x0600890F RID: 35087 RVA: 0x00344DC4 File Offset: 0x00342FC4
	[XuiBindingNcalcFunction(0, 1, null)]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void gamepref(IBindingNcalc _bindingInstance, FunctionArgs _args, object[] _evaluatedArguments)
	{
		string text = _evaluatedArguments[0].ToString();
		if (string.IsNullOrEmpty(text))
		{
			BindingNcalcFunctions.LogFunctionError(_bindingInstance, string.Format("Argument 0 '{0}' is not a non-empty string.", _evaluatedArguments[0]), null, "gamepref");
			return;
		}
		EnumGamePrefs enumGamePrefs;
		if (!EnumUtils.TryParseIgnoreCase<EnumGamePrefs>(text, out enumGamePrefs))
		{
			BindingNcalcFunctions.LogFunctionError(_bindingInstance, string.Format("Argument 0 '{0}' is not a valid GamePref.", _evaluatedArguments[0]), null, "gamepref");
			return;
		}
		if (!_bindingInstance.RegisterVariable(new BindingInfoNcalc.VariableStateGamePref(enumGamePrefs)))
		{
			_bindingInstance.SetIndeterministic();
		}
		_args.Result = GamePrefs.GetObject(enumGamePrefs);
	}

	// Token: 0x06008910 RID: 35088 RVA: 0x00344E40 File Offset: 0x00343040
	[XuiBindingNcalcFunction(0, 1, null)]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void gamestat(IBindingNcalc _bindingInstance, FunctionArgs _args, object[] _evaluatedArguments)
	{
		string text = _evaluatedArguments[0].ToString();
		if (string.IsNullOrEmpty(text))
		{
			BindingNcalcFunctions.LogFunctionError(_bindingInstance, string.Format("Argument 0 '{0}' is not a non-empty string.", _evaluatedArguments[0]), null, "gamestat");
			return;
		}
		EnumGameStats enumGameStats;
		if (!EnumUtils.TryParseIgnoreCase<EnumGameStats>(text, out enumGameStats))
		{
			BindingNcalcFunctions.LogFunctionError(_bindingInstance, string.Format("Argument 0 '{0}' is not a valid GameStat.", _evaluatedArguments[0]), null, "gamestat");
			return;
		}
		if (!_bindingInstance.RegisterVariable(new BindingInfoNcalc.VariableStateGameStat(enumGameStats)))
		{
			_bindingInstance.SetIndeterministic();
		}
		_args.Result = GameStats.GetObject(enumGameStats);
	}

	// Token: 0x06008911 RID: 35089 RVA: 0x00344EBC File Offset: 0x003430BC
	[XuiBindingNcalcFunction(0, 1, null)]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void serverinfoint(IBindingNcalc _bindingInstance, FunctionArgs _args, object[] _evaluatedArguments)
	{
		string text = _evaluatedArguments[0].ToString();
		if (string.IsNullOrEmpty(text))
		{
			BindingNcalcFunctions.LogFunctionError(_bindingInstance, string.Format("Argument 0 '{0}' is not a non-empty string.", _evaluatedArguments[0]), null, "serverinfoint");
			return;
		}
		GameInfoInt gameInfoInt;
		if (!EnumUtils.TryParseIgnoreCase<GameInfoInt>(text, out gameInfoInt))
		{
			BindingNcalcFunctions.LogFunctionError(_bindingInstance, string.Format("Argument 0 '{0}' is not a valid GameInfoInt.", _evaluatedArguments[0]), null, "serverinfoint");
			return;
		}
		if (!_bindingInstance.RegisterVariable(new BindingInfoNcalc.VariableStateGameInfoInt(gameInfoInt)))
		{
			_bindingInstance.SetIndeterministic();
		}
		GameServerInfo gameServerInfo = (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer ? SingletonMonoBehaviour<ConnectionManager>.Instance.LocalServerInfo : SingletonMonoBehaviour<ConnectionManager>.Instance.LastGameServerInfo);
		if (gameServerInfo == null)
		{
			BindingNcalcFunctions.LogFunctionError(_bindingInstance, "No game loaded.", null, "serverinfoint");
			return;
		}
		_args.Result = gameServerInfo.GetValue(gameInfoInt);
	}

	// Token: 0x06008912 RID: 35090 RVA: 0x00344F78 File Offset: 0x00343178
	[XuiBindingNcalcFunction(false, 1, null)]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void serverinfobool(IBindingNcalc _bindingInstance, FunctionArgs _args, object[] _evaluatedArguments)
	{
		string text = _evaluatedArguments[0].ToString();
		if (string.IsNullOrEmpty(text))
		{
			BindingNcalcFunctions.LogFunctionError(_bindingInstance, string.Format("Argument 0 '{0}' is not a non-empty string.", _evaluatedArguments[0]), null, "serverinfobool");
			return;
		}
		GameInfoBool gameInfoBool;
		if (!EnumUtils.TryParseIgnoreCase<GameInfoBool>(text, out gameInfoBool))
		{
			BindingNcalcFunctions.LogFunctionError(_bindingInstance, string.Format("Argument 0 '{0}' is not a valid GameInfoBool.", _evaluatedArguments[0]), null, "serverinfobool");
			return;
		}
		if (!_bindingInstance.RegisterVariable(new BindingInfoNcalc.VariableStateGameInfoBool(gameInfoBool)))
		{
			_bindingInstance.SetIndeterministic();
		}
		GameServerInfo gameServerInfo = (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer ? SingletonMonoBehaviour<ConnectionManager>.Instance.LocalServerInfo : SingletonMonoBehaviour<ConnectionManager>.Instance.LastGameServerInfo);
		if (gameServerInfo == null)
		{
			BindingNcalcFunctions.LogFunctionError(_bindingInstance, "No game loaded.", null, "serverinfobool");
			return;
		}
		_args.Result = gameServerInfo.GetValue(gameInfoBool);
	}

	// Token: 0x06008913 RID: 35091 RVA: 0x00345034 File Offset: 0x00343234
	[XuiBindingNcalcFunction("", 1, null)]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void serverinfostring(IBindingNcalc _bindingInstance, FunctionArgs _args, object[] _evaluatedArguments)
	{
		string text = _evaluatedArguments[0].ToString();
		if (string.IsNullOrEmpty(text))
		{
			BindingNcalcFunctions.LogFunctionError(_bindingInstance, string.Format("Argument 0 '{0}' is not a non-empty string.", _evaluatedArguments[0]), null, "serverinfostring");
			return;
		}
		GameInfoString gameInfoString;
		if (!EnumUtils.TryParseIgnoreCase<GameInfoString>(text, out gameInfoString))
		{
			BindingNcalcFunctions.LogFunctionError(_bindingInstance, string.Format("Argument 0 '{0}' is not a valid GameInfoString.", _evaluatedArguments[0]), null, "serverinfostring");
			return;
		}
		if (!_bindingInstance.RegisterVariable(new BindingInfoNcalc.VariableStateGameInfoString(gameInfoString)))
		{
			_bindingInstance.SetIndeterministic();
		}
		GameServerInfo gameServerInfo = (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer ? SingletonMonoBehaviour<ConnectionManager>.Instance.LocalServerInfo : SingletonMonoBehaviour<ConnectionManager>.Instance.LastGameServerInfo);
		if (gameServerInfo == null)
		{
			BindingNcalcFunctions.LogFunctionError(_bindingInstance, "No game loaded.", null, "serverinfostring");
			return;
		}
		_args.Result = gameServerInfo.GetValue(gameInfoString);
	}

	// Token: 0x06008914 RID: 35092 RVA: 0x003450EC File Offset: 0x003432EC
	[XuiBindingNcalcFunction(null, 2, "dictvalue")]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void dictValue(IBindingNcalc _bindingInstance, FunctionArgs _args, object[] _evaluatedArguments)
	{
		_bindingInstance.SetIndeterministic();
		IDictionary dictionary = _evaluatedArguments[0] as IDictionary;
		if (dictionary == null)
		{
			BindingNcalcFunctions.LogFunctionError(_bindingInstance, string.Format("Argument 0 '{0}' does not evaluate to a dictionary.", _evaluatedArguments[0]), null, "dictValue");
			return;
		}
		object obj = _evaluatedArguments[1];
		if (obj == null)
		{
			BindingNcalcFunctions.LogFunctionError(_bindingInstance, "Argument 1 is null.", null, "dictValue");
			return;
		}
		if (!dictionary.Contains(obj))
		{
			_args.Result = "";
			return;
		}
		_args.Result = dictionary[obj];
	}

	// Token: 0x06008915 RID: 35093 RVA: 0x00345160 File Offset: 0x00343360
	[XuiBindingNcalcFunction("", -1, null)]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void format(IBindingNcalc _bindingInstance, FunctionArgs _args, object[] _evaluatedArguments)
	{
		if (_args.Parameters.Length < 1)
		{
			BindingNcalcFunctions.LogFunctionError(_bindingInstance, string.Format("Invalid number of arguments ({0}, expected at least 1).", _args.Parameters.Length), null, "format");
			return;
		}
		object obj = _args.Parameters[0].Evaluate();
		if (obj == null)
		{
			BindingNcalcFunctions.LogFunctionError(_bindingInstance, "Can not evaluate argument.", null, "format");
			return;
		}
		object[] objectArray = BindingNcalcFunctions.GetObjectArray(_args.Parameters.Length - 1);
		for (int i = 1; i < _args.Parameters.Length; i++)
		{
			objectArray[i - 1] = _args.Parameters[i].Evaluate();
		}
		string text = string.Format(obj.ToString(), objectArray);
		BindingNcalcFunctions.ReturnObjectArray(objectArray);
		_args.Result = text;
	}

	// Token: 0x06008916 RID: 35094 RVA: 0x0034520E File Offset: 0x0034340E
	[XuiBindingNcalcFunction(0, 1, null)]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void length(IBindingNcalc _bindingInstance, FunctionArgs _args, object[] _evaluatedArguments)
	{
		_args.Result = _evaluatedArguments[0].ToString().Length;
	}

	// Token: 0x06008917 RID: 35095 RVA: 0x00345228 File Offset: 0x00343428
	[XuiBindingNcalcFunction(0, 1, "int")]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void toInt(IBindingNcalc _bindingInstance, FunctionArgs _args, object[] _evaluatedArguments)
	{
		try
		{
			_args.Result = Convert.ToInt32(_evaluatedArguments[0]);
		}
		catch (Exception ex)
		{
			BindingNcalcFunctions.LogFunctionError(_bindingInstance, string.Format("Argument '{0}' does not evaluate to an integer.", _evaluatedArguments[0]), ex, "toInt");
		}
	}

	// Token: 0x06008918 RID: 35096 RVA: 0x00345278 File Offset: 0x00343478
	[XuiBindingNcalcFunction(0, 1, "roundtoint")]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void roundToInt(IBindingNcalc _bindingInstance, FunctionArgs _args, object[] _evaluatedArguments)
	{
		float num;
		if (!BindingNcalcFunctions.tryArgAsFloat(_bindingInstance, _evaluatedArguments, 0, "v", out num, "roundToInt"))
		{
			return;
		}
		_args.Result = Mathf.RoundToInt(num);
	}

	// Token: 0x06008919 RID: 35097 RVA: 0x003452B0 File Offset: 0x003434B0
	[XuiBindingNcalcFunction(0.0, 1, "float")]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void toFloat(IBindingNcalc _bindingInstance, FunctionArgs _args, object[] _evaluatedArguments)
	{
		try
		{
			_args.Result = Convert.ToDouble(_evaluatedArguments[0]);
		}
		catch (Exception ex)
		{
			BindingNcalcFunctions.LogFunctionError(_bindingInstance, string.Format("Argument '{0}' does not evaluate to a number.", _evaluatedArguments[0]), ex, "toFloat");
		}
	}

	// Token: 0x0600891A RID: 35098 RVA: 0x00345300 File Offset: 0x00343500
	[XuiBindingNcalcFunction("", 1, "str")]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void toString(IBindingNcalc _bindingInstance, FunctionArgs _args, object[] _evaluatedArguments)
	{
		object obj = _evaluatedArguments[0];
		_args.Result = ((obj != null) ? obj.ToString() : null) ?? "";
	}

	// Token: 0x0600891B RID: 35099 RVA: 0x00345320 File Offset: 0x00343520
	[XuiBindingNcalcFunction("", 2, "itos")]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void intToString(IBindingNcalc _bindingInstance, FunctionArgs _args, object[] _evaluatedArguments)
	{
		int num;
		if (!BindingNcalcFunctions.tryArgAsInt(_bindingInstance, _evaluatedArguments, 0, "i", out num, "intToString"))
		{
			return;
		}
		_args.Result = num.ToString(_evaluatedArguments[1].ToString());
	}

	// Token: 0x0600891C RID: 35100 RVA: 0x0034535C File Offset: 0x0034355C
	[XuiBindingNcalcFunction("", 2, "ftos")]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void floatToString(IBindingNcalc _bindingInstance, FunctionArgs _args, object[] _evaluatedArguments)
	{
		float num;
		if (!BindingNcalcFunctions.tryArgAsFloat(_bindingInstance, _evaluatedArguments, 0, "f", out num, "floatToString"))
		{
			return;
		}
		_args.Result = num.ToString(_evaluatedArguments[1].ToString());
	}

	// Token: 0x0600891D RID: 35101 RVA: 0x00345398 File Offset: 0x00343598
	[XuiBindingNcalcFunction(false, 1, "bound")]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void isBound(IBindingNcalc _bindingInstance, FunctionArgs _args, object[] _evaluatedArguments)
	{
		throw new NotImplementedException();
	}

	// Token: 0x0600891E RID: 35102 RVA: 0x003453AA File Offset: 0x003435AA
	[XuiBindingNcalcFunction("", 1, null)]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void always(IBindingNcalc _bindingInstance, FunctionArgs _args, object[] _evaluatedArguments)
	{
		_bindingInstance.SetIndeterministic();
		_args.Result = _evaluatedArguments[0];
	}

	// Token: 0x0600891F RID: 35103 RVA: 0x003453BC File Offset: 0x003435BC
	[XuiBindingNcalcFunction(false, 1, "defined")]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void isDefined(IBindingNcalc _bindingInstance, FunctionArgs _args, object[] _evaluatedArguments)
	{
		object obj;
		_args.Result = (_bindingInstance.FindParameter(_evaluatedArguments[0].ToString(), out obj) ? BindingNcalcFunctions.boxedTrue : BindingNcalcFunctions.boxedFalse);
	}

	// Token: 0x06008920 RID: 35104 RVA: 0x003453F0 File Offset: 0x003435F0
	[XuiBindingNcalcFunction(0, 2, "c_i")]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void color32_i(IBindingNcalc _bindingInstance, FunctionArgs _args, object[] _evaluatedArguments)
	{
		int num;
		if (!BindingNcalcFunctions.tryArgAsInt(_bindingInstance, _evaluatedArguments, 1, "i", out num, "color32_i"))
		{
			return;
		}
		if (num < 0 || num > 3)
		{
			BindingNcalcFunctions.LogFunctionError(_bindingInstance, string.Format("Argument '{0}' outside allowed range (0-3).", _evaluatedArguments[1]), null, "color32_i");
		}
		try
		{
			_args.Result = ((Color32)_evaluatedArguments[0])[num];
		}
		catch (Exception ex)
		{
			BindingNcalcFunctions.LogFunctionError(_bindingInstance, string.Format("Argument '{0}' does not evaluate to a Color.", _evaluatedArguments[0]), ex, "color32_i");
		}
	}

	// Token: 0x06008921 RID: 35105 RVA: 0x00345480 File Offset: 0x00343680
	[XuiBindingNcalcFunction(0, 1, "c_r")]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void color32_r(IBindingNcalc _bindingInstance, FunctionArgs _args, object[] _evaluatedArguments)
	{
		try
		{
			_args.Result = ((Color32)_evaluatedArguments[0]).r;
		}
		catch (Exception ex)
		{
			BindingNcalcFunctions.LogFunctionError(_bindingInstance, string.Format("Argument '{0}' does not evaluate to a Color.", _evaluatedArguments[0]), ex, "color32_r");
		}
	}

	// Token: 0x06008922 RID: 35106 RVA: 0x003454D4 File Offset: 0x003436D4
	[XuiBindingNcalcFunction(0, 1, "c_g")]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void color32_g(IBindingNcalc _bindingInstance, FunctionArgs _args, object[] _evaluatedArguments)
	{
		try
		{
			_args.Result = ((Color32)_evaluatedArguments[0]).g;
		}
		catch (Exception ex)
		{
			BindingNcalcFunctions.LogFunctionError(_bindingInstance, string.Format("Argument '{0}' does not evaluate to a Color.", _evaluatedArguments[0]), ex, "color32_g");
		}
	}

	// Token: 0x06008923 RID: 35107 RVA: 0x00345528 File Offset: 0x00343728
	[XuiBindingNcalcFunction(0, 1, "c_b")]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void color32_b(IBindingNcalc _bindingInstance, FunctionArgs _args, object[] _evaluatedArguments)
	{
		try
		{
			_args.Result = ((Color32)_evaluatedArguments[0]).b;
		}
		catch (Exception ex)
		{
			BindingNcalcFunctions.LogFunctionError(_bindingInstance, string.Format("Argument '{0}' does not evaluate to a Color.", _evaluatedArguments[0]), ex, "color32_b");
		}
	}

	// Token: 0x06008924 RID: 35108 RVA: 0x0034557C File Offset: 0x0034377C
	[XuiBindingNcalcFunction(0, 1, "c_a")]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void color32_a(IBindingNcalc _bindingInstance, FunctionArgs _args, object[] _evaluatedArguments)
	{
		try
		{
			_args.Result = ((Color32)_evaluatedArguments[0]).a;
		}
		catch (Exception ex)
		{
			BindingNcalcFunctions.LogFunctionError(_bindingInstance, string.Format("Argument '{0}' does not evaluate to a Color.", _evaluatedArguments[0]), ex, "color32_a");
		}
	}

	// Token: 0x06008925 RID: 35109 RVA: 0x003455D0 File Offset: 0x003437D0
	[XuiBindingNcalcFunction(null, 4, "color")]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void color32(IBindingNcalc _bindingInstance, FunctionArgs _args, object[] _evaluatedArguments)
	{
		byte b;
		if (!BindingNcalcFunctions.tryArgAsByteClamped(_bindingInstance, _evaluatedArguments, 0, "r", out b, "color32"))
		{
			return;
		}
		byte b2;
		if (!BindingNcalcFunctions.tryArgAsByteClamped(_bindingInstance, _evaluatedArguments, 1, "g", out b2, "color32"))
		{
			return;
		}
		byte b3;
		if (!BindingNcalcFunctions.tryArgAsByteClamped(_bindingInstance, _evaluatedArguments, 2, "b", out b3, "color32"))
		{
			return;
		}
		byte b4;
		if (!BindingNcalcFunctions.tryArgAsByteClamped(_bindingInstance, _evaluatedArguments, 3, "a", out b4, "color32"))
		{
			return;
		}
		_args.Result = new Color32(b, b2, b3, b4);
	}

	// Token: 0x06008926 RID: 35110 RVA: 0x00345650 File Offset: 0x00343850
	[XuiBindingNcalcFunction(0, 2, "v2i_i")]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void v2i_i(IBindingNcalc _bindingInstance, FunctionArgs _args, object[] _evaluatedArguments)
	{
		int num;
		if (!BindingNcalcFunctions.tryArgAsInt(_bindingInstance, _evaluatedArguments, 1, "i", out num, "v2i_i"))
		{
			return;
		}
		if (num < 0 || num > 1)
		{
			BindingNcalcFunctions.LogFunctionError(_bindingInstance, string.Format("Argument '{0}' outside allowed range (0-1).", _evaluatedArguments[1]), null, "v2i_i");
		}
		try
		{
			_args.Result = ((Vector2i)_evaluatedArguments[0])[num];
		}
		catch (Exception ex)
		{
			BindingNcalcFunctions.LogFunctionError(_bindingInstance, string.Format("Argument '{0}' does not evaluate to a Vector2i.", _evaluatedArguments[0]), ex, "v2i_i");
		}
	}

	// Token: 0x06008927 RID: 35111 RVA: 0x003456E0 File Offset: 0x003438E0
	[XuiBindingNcalcFunction(0, 1, "v2i_x")]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void v2i_x(IBindingNcalc _bindingInstance, FunctionArgs _args, object[] _evaluatedArguments)
	{
		try
		{
			_args.Result = ((Vector2i)_evaluatedArguments[0]).x;
		}
		catch (Exception ex)
		{
			BindingNcalcFunctions.LogFunctionError(_bindingInstance, string.Format("Argument '{0}' does not evaluate to a Vector2i.", _evaluatedArguments[0]), ex, "v2i_x");
		}
	}

	// Token: 0x06008928 RID: 35112 RVA: 0x00345734 File Offset: 0x00343934
	[XuiBindingNcalcFunction(0, 1, "v2i_y")]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void v2i_y(IBindingNcalc _bindingInstance, FunctionArgs _args, object[] _evaluatedArguments)
	{
		try
		{
			_args.Result = ((Vector2i)_evaluatedArguments[0]).y;
		}
		catch (Exception ex)
		{
			BindingNcalcFunctions.LogFunctionError(_bindingInstance, string.Format("Argument '{0}' does not evaluate to a Vector2i.", _evaluatedArguments[0]), ex, "v2i_y");
		}
	}

	// Token: 0x06008929 RID: 35113 RVA: 0x00345788 File Offset: 0x00343988
	[XuiBindingNcalcFunction(null, 2, "v2i")]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void vector2i(IBindingNcalc _bindingInstance, FunctionArgs _args, object[] _evaluatedArguments)
	{
		int num;
		if (!BindingNcalcFunctions.tryArgAsInt(_bindingInstance, _evaluatedArguments, 0, "x", out num, "vector2i"))
		{
			return;
		}
		int num2;
		if (!BindingNcalcFunctions.tryArgAsInt(_bindingInstance, _evaluatedArguments, 1, "y", out num2, "vector2i"))
		{
			return;
		}
		_args.Result = new Vector2i(num, num2);
	}

	// Token: 0x0600892A RID: 35114 RVA: 0x003457D8 File Offset: 0x003439D8
	[XuiBindingNcalcFunction(0, 2, "v3i_i")]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void v3i_i(IBindingNcalc _bindingInstance, FunctionArgs _args, object[] _evaluatedArguments)
	{
		int num;
		if (!BindingNcalcFunctions.tryArgAsInt(_bindingInstance, _evaluatedArguments, 1, "i", out num, "v3i_i"))
		{
			return;
		}
		if (num < 0 || num > 2)
		{
			BindingNcalcFunctions.LogFunctionError(_bindingInstance, string.Format("Argument '{0}' outside allowed range (0-2).", _evaluatedArguments[1]), null, "v3i_i");
		}
		try
		{
			_args.Result = ((Vector2i)_evaluatedArguments[0])[num];
		}
		catch (Exception ex)
		{
			BindingNcalcFunctions.LogFunctionError(_bindingInstance, string.Format("Argument '{0}' does not evaluate to a Vector3i.", _evaluatedArguments[0]), ex, "v3i_i");
		}
	}

	// Token: 0x0600892B RID: 35115 RVA: 0x00345868 File Offset: 0x00343A68
	[XuiBindingNcalcFunction(0, 1, "v3i_x")]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void v3i_x(IBindingNcalc _bindingInstance, FunctionArgs _args, object[] _evaluatedArguments)
	{
		try
		{
			_args.Result = ((Vector3i)_evaluatedArguments[0]).x;
		}
		catch (Exception ex)
		{
			BindingNcalcFunctions.LogFunctionError(_bindingInstance, string.Format("Argument '{0}' does not evaluate to a Vector3i.", _evaluatedArguments[0]), ex, "v3i_x");
		}
	}

	// Token: 0x0600892C RID: 35116 RVA: 0x003458BC File Offset: 0x00343ABC
	[XuiBindingNcalcFunction(0, 1, "v3i_y")]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void v3i_y(IBindingNcalc _bindingInstance, FunctionArgs _args, object[] _evaluatedArguments)
	{
		try
		{
			_args.Result = ((Vector3i)_evaluatedArguments[0]).y;
		}
		catch (Exception ex)
		{
			BindingNcalcFunctions.LogFunctionError(_bindingInstance, string.Format("Argument '{0}' does not evaluate to a Vector3i.", _evaluatedArguments[0]), ex, "v3i_y");
		}
	}

	// Token: 0x0600892D RID: 35117 RVA: 0x00345910 File Offset: 0x00343B10
	[XuiBindingNcalcFunction(0, 1, "v3i_z")]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void v3i_z(IBindingNcalc _bindingInstance, FunctionArgs _args, object[] _evaluatedArguments)
	{
		try
		{
			_args.Result = ((Vector3i)_evaluatedArguments[0]).z;
		}
		catch (Exception ex)
		{
			BindingNcalcFunctions.LogFunctionError(_bindingInstance, string.Format("Argument '{0}' does not evaluate to a Vector3i.", _evaluatedArguments[0]), ex, "v3i_z");
		}
	}

	// Token: 0x0600892E RID: 35118 RVA: 0x00345964 File Offset: 0x00343B64
	[XuiBindingNcalcFunction(null, 3, "v3i")]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void vector3i(IBindingNcalc _bindingInstance, FunctionArgs _args, object[] _evaluatedArguments)
	{
		int num;
		if (!BindingNcalcFunctions.tryArgAsInt(_bindingInstance, _evaluatedArguments, 0, "x", out num, "vector3i"))
		{
			return;
		}
		int num2;
		if (!BindingNcalcFunctions.tryArgAsInt(_bindingInstance, _evaluatedArguments, 1, "y", out num2, "vector3i"))
		{
			return;
		}
		int num3;
		if (!BindingNcalcFunctions.tryArgAsInt(_bindingInstance, _evaluatedArguments, 2, "z", out num3, "vector3i"))
		{
			return;
		}
		_args.Result = new Vector3i(num, num2, num3);
	}

	// Token: 0x0600892F RID: 35119 RVA: 0x003459CC File Offset: 0x00343BCC
	[XuiBindingNcalcFunction(null, 2, "v2")]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void vector2(IBindingNcalc _bindingInstance, FunctionArgs _args, object[] _evaluatedArguments)
	{
		float num;
		if (!BindingNcalcFunctions.tryArgAsFloat(_bindingInstance, _evaluatedArguments, 0, "x", out num, "vector2"))
		{
			return;
		}
		float num2;
		if (!BindingNcalcFunctions.tryArgAsFloat(_bindingInstance, _evaluatedArguments, 1, "y", out num2, "vector2"))
		{
			return;
		}
		_args.Result = new Vector2(num, num2);
	}

	// Token: 0x06008930 RID: 35120 RVA: 0x00345A1C File Offset: 0x00343C1C
	[XuiBindingNcalcFunction(null, 3, "v3")]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void vector3(IBindingNcalc _bindingInstance, FunctionArgs _args, object[] _evaluatedArguments)
	{
		float num;
		if (!BindingNcalcFunctions.tryArgAsFloat(_bindingInstance, _evaluatedArguments, 0, "x", out num, "vector3"))
		{
			return;
		}
		float num2;
		if (!BindingNcalcFunctions.tryArgAsFloat(_bindingInstance, _evaluatedArguments, 1, "y", out num2, "vector3"))
		{
			return;
		}
		float num3;
		if (!BindingNcalcFunctions.tryArgAsFloat(_bindingInstance, _evaluatedArguments, 2, "z", out num3, "vector3"))
		{
			return;
		}
		_args.Result = new Vector3(num, num2, num3);
	}

	// Token: 0x06008931 RID: 35121 RVA: 0x00345A84 File Offset: 0x00343C84
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool tryArgAsByteClamped(IBindingNcalc _bindingInstance, object[] _evaluatedArguments, int _argIndex, string _argName, out byte _value, [CallerMemberName] string _funcName = null)
	{
		object obj = _evaluatedArguments[_argIndex];
		bool flag;
		try
		{
			_value = (byte)Mathf.Clamp((int)obj, 0, 255);
			flag = true;
		}
		catch (Exception ex)
		{
			BindingNcalcFunctions.LogFunctionError(_bindingInstance, string.Format("Argument {0}='{1}' (type {2}) does not evaluate to an integer.", _argName, obj, obj.GetType().FullName), ex, _funcName);
			_value = 0;
			flag = false;
		}
		return flag;
	}

	// Token: 0x06008932 RID: 35122 RVA: 0x00345AE8 File Offset: 0x00343CE8
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool tryArgAsInt(IBindingNcalc _bindingInstance, object[] _evaluatedArguments, int _argIndex, string _argName, out int _value, [CallerMemberName] string _funcName = null)
	{
		object obj = _evaluatedArguments[_argIndex];
		bool flag;
		try
		{
			_value = Convert.ToInt32(obj);
			flag = true;
		}
		catch (Exception ex)
		{
			BindingNcalcFunctions.LogFunctionError(_bindingInstance, string.Format("Argument {0}='{1}' (type {2}) does not evaluate to an integer.", _argName, obj, obj.GetType().FullName), ex, _funcName);
			_value = 0;
			flag = false;
		}
		return flag;
	}

	// Token: 0x06008933 RID: 35123 RVA: 0x00345B40 File Offset: 0x00343D40
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool tryArgAsFloat(IBindingNcalc _bindingInstance, object[] _evaluatedArguments, int _argIndex, string _argName, out float _value, [CallerMemberName] string _funcName = null)
	{
		object obj = _evaluatedArguments[_argIndex];
		bool flag;
		try
		{
			_value = Convert.ToSingle(obj);
			flag = true;
		}
		catch (Exception ex)
		{
			BindingNcalcFunctions.LogFunctionError(_bindingInstance, string.Format("Argument {0}='{1}' (type {2}) does not evaluate to a number.", _argName, obj, obj.GetType().FullName), ex, _funcName);
			_value = 0f;
			flag = false;
		}
		return flag;
	}

	// Token: 0x06008934 RID: 35124 RVA: 0x00345B9C File Offset: 0x00343D9C
	public static void LogFunctionError(IBindingNcalc _bindingInstance, string _message, Exception _e = null, [CallerMemberName] string _funcName = null)
	{
		Log.Error(string.Concat(new string[]
		{
			"[XUi] Binding expression calling function '",
			_funcName,
			"': ",
			_message,
			" Binding expression: '",
			_bindingInstance.SourceText,
			"' --- hierarchy: ",
			_bindingInstance.TargetElement.GetXuiHierarchy()
		}));
		if (_e != null)
		{
			Log.Exception(_e);
		}
	}

	// Token: 0x06008935 RID: 35125 RVA: 0x00345C04 File Offset: 0x00343E04
	public static void EvaluateFunc(IBindingNcalc _bindingInstance, string _name, FunctionArgs _args)
	{
		BindingNcalcFunctions.FunctionDefinition functionDefinition;
		if (!BindingNcalcFunctions.ncalcFunctions.TryGetValue(_name, out functionDefinition))
		{
			return;
		}
		object[] array = null;
		_args.Result = null;
		if (functionDefinition.ExpectedArgumentCount >= 0)
		{
			if (_args.Parameters.Length != functionDefinition.ExpectedArgumentCount)
			{
				BindingNcalcFunctions.LogFunctionError(_bindingInstance, string.Format("Invalid number of arguments ({0}, expected {1}).", _args.Parameters.Length, functionDefinition.ExpectedArgumentCount), null, _name);
				if (functionDefinition.ErrorResult != null)
				{
					_args.Result = functionDefinition.ErrorResult;
				}
				return;
			}
			if (_args.Parameters.Length != 0)
			{
				array = BindingNcalcFunctions.GetObjectArray(_args.Parameters.Length);
				for (int i = 0; i < _args.Parameters.Length; i++)
				{
					object obj = _args.Parameters[i].Evaluate();
					if (obj == null)
					{
						BindingNcalcFunctions.LogFunctionError(_bindingInstance, string.Format("Can not evaluate argument at index {0}.", i), null, _name);
						if (functionDefinition.ErrorResult != null)
						{
							_args.Result = functionDefinition.ErrorResult;
						}
						return;
					}
					array[i] = obj;
				}
			}
		}
		functionDefinition.Delegate(_bindingInstance, _args, array);
		if (_args.Result == null && functionDefinition.ErrorResult != null)
		{
			_args.Result = functionDefinition.ErrorResult;
		}
		BindingNcalcFunctions.ReturnObjectArray(array);
	}

	// Token: 0x06008936 RID: 35126 RVA: 0x00345D21 File Offset: 0x00343F21
	[PublicizedFrom(EAccessModifier.Private)]
	public static object[] GetObjectArray(int _length)
	{
		if (_length > 20)
		{
			return new object[_length];
		}
		if (BindingNcalcFunctions.objectArrays.Count == 0)
		{
			return new object[_length];
		}
		return BindingNcalcFunctions.objectArrays.Pop();
	}

	// Token: 0x06008937 RID: 35127 RVA: 0x00345D4C File Offset: 0x00343F4C
	[PublicizedFrom(EAccessModifier.Private)]
	public static void ReturnObjectArray(object[] _arr)
	{
		if (_arr != null && _arr.Length == 20)
		{
			BindingNcalcFunctions.objectArrays.Push(_arr);
		}
	}

	// Token: 0x06008938 RID: 35128 RVA: 0x00345D64 File Offset: 0x00343F64
	public static void RegisterNcalcFunctions()
	{
		if (BindingNcalcFunctions.ncalcFunctions.Count > 0)
		{
			return;
		}
		for (int i = 0; i < 10; i++)
		{
			BindingNcalcFunctions.objectArrays.Push(new object[20]);
		}
		ReflectionHelpers.FindTypesWithAttribute<XuiBindingNcalcFunctionAttribute>(new Action<Type, bool, XuiBindingNcalcFunctionAttribute>(BindingNcalcFunctions.<RegisterNcalcFunctions>g__TypeFoundCallback|50_0), true);
	}

	// Token: 0x0600893A RID: 35130 RVA: 0x00345DDB File Offset: 0x00343FDB
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static void <RegisterNcalcFunctions>g__TypeFoundCallback|50_0(Type _type, bool _hasMultiple, XuiBindingNcalcFunctionAttribute _classAttribute)
	{
		ReflectionHelpers.GetMethodsWithAttribute<XuiBindingNcalcFunctionAttribute>(_type, new Action<MethodInfo, bool, XuiBindingNcalcFunctionAttribute>(BindingNcalcFunctions.<RegisterNcalcFunctions>g__MethodFoundCallback|50_1), true, true, true);
	}

	// Token: 0x0600893B RID: 35131 RVA: 0x00345DF4 File Offset: 0x00343FF4
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static void <RegisterNcalcFunctions>g__MethodFoundCallback|50_1(MethodInfo _method, bool _hasMultiple, XuiBindingNcalcFunctionAttribute _methodAttribute)
	{
		if (!ReflectionHelpers.MethodCompatibleWithDelegate<BindingNcalcFunctions.CustomNcalcFunctionDelegate>(_method, true))
		{
			Log.Error(string.Concat(new string[]
			{
				"[XUi] Binding NCalc function method ",
				_method.DeclaringType.FullName,
				".",
				_method.Name,
				" not compatible with delegate signature"
			}));
			return;
		}
		string text = _methodAttribute.FunctionName ?? _method.Name.ToLowerInvariant();
		BindingNcalcFunctions.CustomNcalcFunctionDelegate customNcalcFunctionDelegate = (BindingNcalcFunctions.CustomNcalcFunctionDelegate)_method.CreateDelegate(typeof(BindingNcalcFunctions.CustomNcalcFunctionDelegate));
		if (BindingNcalcFunctions.ncalcFunctions.ContainsKey(text))
		{
			Log.Warning(string.Concat(new string[]
			{
				"[XUi] Binding NCalc function method ",
				_method.DeclaringType.FullName,
				".",
				_method.Name,
				" overriding previously defined method for NCalc function '",
				text,
				"'"
			}));
		}
		BindingNcalcFunctions.ncalcFunctions[text] = new BindingNcalcFunctions.FunctionDefinition(customNcalcFunctionDelegate, _methodAttribute.ExpectedArgumentCount, _methodAttribute.ErrorResult);
	}

	// Token: 0x040065E3 RID: 26083
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly object boxedFalse = false;

	// Token: 0x040065E4 RID: 26084
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly object boxedTrue = true;

	// Token: 0x040065E5 RID: 26085
	[PublicizedFrom(EAccessModifier.Private)]
	public const int ObjectArrayLengthCached = 20;

	// Token: 0x040065E6 RID: 26086
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Stack<object[]> objectArrays = new Stack<object[]>();

	// Token: 0x040065E7 RID: 26087
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Dictionary<string, BindingNcalcFunctions.FunctionDefinition> ncalcFunctions = new Dictionary<string, BindingNcalcFunctions.FunctionDefinition>();

	// Token: 0x020010E6 RID: 4326
	// (Invoke) Token: 0x0600893D RID: 35133
	public delegate void CustomNcalcFunctionDelegate(IBindingNcalc _bindingInstance, FunctionArgs _args, object[] _evaluatedArguments);

	// Token: 0x020010E7 RID: 4327
	[PublicizedFrom(EAccessModifier.Private)]
	public class FunctionDefinition
	{
		// Token: 0x06008940 RID: 35136 RVA: 0x00345EEB File Offset: 0x003440EB
		public FunctionDefinition(BindingNcalcFunctions.CustomNcalcFunctionDelegate _delegate, int _expectedArgumentCount, object _errorResult)
		{
			this.Delegate = _delegate;
			this.ExpectedArgumentCount = _expectedArgumentCount;
			this.ErrorResult = _errorResult;
		}

		// Token: 0x040065E8 RID: 26088
		public readonly BindingNcalcFunctions.CustomNcalcFunctionDelegate Delegate;

		// Token: 0x040065E9 RID: 26089
		public readonly int ExpectedArgumentCount;

		// Token: 0x040065EA RID: 26090
		public readonly object ErrorResult;
	}
}
