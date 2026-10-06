using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using NCalc;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001113 RID: 4371
[XuiBindingNcalcFunction]
[Preserve]
public static class BindingNcalcFunctions
{
	// Token: 0x06008A60 RID: 35424 RVA: 0x0034D807 File Offset: 0x0034BA07
	[XuiBindingNcalcFunction("<ERROR>", 1, null)]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void localization(IBindingNcalc _bindingInstance, FunctionArgs _args, object[] _evaluatedArguments)
	{
		_args.Result = Localization.Get(_evaluatedArguments[0].ToString(), false, null);
	}

	// Token: 0x06008A61 RID: 35425 RVA: 0x0034D820 File Offset: 0x0034BA20
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

	// Token: 0x06008A62 RID: 35426 RVA: 0x0034D890 File Offset: 0x0034BA90
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

	// Token: 0x06008A63 RID: 35427 RVA: 0x0034D90C File Offset: 0x0034BB0C
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

	// Token: 0x06008A64 RID: 35428 RVA: 0x0034D988 File Offset: 0x0034BB88
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

	// Token: 0x06008A65 RID: 35429 RVA: 0x0034DA44 File Offset: 0x0034BC44
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

	// Token: 0x06008A66 RID: 35430 RVA: 0x0034DB00 File Offset: 0x0034BD00
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

	// Token: 0x06008A67 RID: 35431 RVA: 0x0034DBB8 File Offset: 0x0034BDB8
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

	// Token: 0x06008A68 RID: 35432 RVA: 0x0034DC2C File Offset: 0x0034BE2C
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

	// Token: 0x06008A69 RID: 35433 RVA: 0x0034DCDA File Offset: 0x0034BEDA
	[XuiBindingNcalcFunction(0, 1, null)]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void length(IBindingNcalc _bindingInstance, FunctionArgs _args, object[] _evaluatedArguments)
	{
		_args.Result = _evaluatedArguments[0].ToString().Length;
	}

	// Token: 0x06008A6A RID: 35434 RVA: 0x0034DCF4 File Offset: 0x0034BEF4
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

	// Token: 0x06008A6B RID: 35435 RVA: 0x0034DD44 File Offset: 0x0034BF44
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

	// Token: 0x06008A6C RID: 35436 RVA: 0x0034DD7C File Offset: 0x0034BF7C
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

	// Token: 0x06008A6D RID: 35437 RVA: 0x0034DDCC File Offset: 0x0034BFCC
	[XuiBindingNcalcFunction("", 1, "str")]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void toString(IBindingNcalc _bindingInstance, FunctionArgs _args, object[] _evaluatedArguments)
	{
		object obj = _evaluatedArguments[0];
		_args.Result = ((obj != null) ? obj.ToString() : null) ?? "";
	}

	// Token: 0x06008A6E RID: 35438 RVA: 0x0034DDEC File Offset: 0x0034BFEC
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

	// Token: 0x06008A6F RID: 35439 RVA: 0x0034DE28 File Offset: 0x0034C028
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

	// Token: 0x06008A70 RID: 35440 RVA: 0x0034DE64 File Offset: 0x0034C064
	[XuiBindingNcalcFunction(false, 1, "bound")]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void isBound(IBindingNcalc _bindingInstance, FunctionArgs _args, object[] _evaluatedArguments)
	{
		throw new NotImplementedException();
	}

	// Token: 0x06008A71 RID: 35441 RVA: 0x0034DE76 File Offset: 0x0034C076
	[XuiBindingNcalcFunction("", 1, null)]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void always(IBindingNcalc _bindingInstance, FunctionArgs _args, object[] _evaluatedArguments)
	{
		_bindingInstance.SetIndeterministic();
		_args.Result = _evaluatedArguments[0];
	}

	// Token: 0x06008A72 RID: 35442 RVA: 0x0034DE88 File Offset: 0x0034C088
	[XuiBindingNcalcFunction(false, 1, "defined")]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void isDefined(IBindingNcalc _bindingInstance, FunctionArgs _args, object[] _evaluatedArguments)
	{
		object obj;
		_args.Result = (_bindingInstance.FindParameter(_evaluatedArguments[0].ToString(), out obj) ? BindingNcalcFunctions.boxedTrue : BindingNcalcFunctions.boxedFalse);
	}

	// Token: 0x06008A73 RID: 35443 RVA: 0x0034DEBC File Offset: 0x0034C0BC
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

	// Token: 0x06008A74 RID: 35444 RVA: 0x0034DF4C File Offset: 0x0034C14C
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

	// Token: 0x06008A75 RID: 35445 RVA: 0x0034DFA0 File Offset: 0x0034C1A0
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

	// Token: 0x06008A76 RID: 35446 RVA: 0x0034DFF4 File Offset: 0x0034C1F4
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

	// Token: 0x06008A77 RID: 35447 RVA: 0x0034E048 File Offset: 0x0034C248
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

	// Token: 0x06008A78 RID: 35448 RVA: 0x0034E09C File Offset: 0x0034C29C
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

	// Token: 0x06008A79 RID: 35449 RVA: 0x0034E11C File Offset: 0x0034C31C
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

	// Token: 0x06008A7A RID: 35450 RVA: 0x0034E1AC File Offset: 0x0034C3AC
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

	// Token: 0x06008A7B RID: 35451 RVA: 0x0034E200 File Offset: 0x0034C400
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

	// Token: 0x06008A7C RID: 35452 RVA: 0x0034E254 File Offset: 0x0034C454
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

	// Token: 0x06008A7D RID: 35453 RVA: 0x0034E2A4 File Offset: 0x0034C4A4
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

	// Token: 0x06008A7E RID: 35454 RVA: 0x0034E334 File Offset: 0x0034C534
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

	// Token: 0x06008A7F RID: 35455 RVA: 0x0034E388 File Offset: 0x0034C588
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

	// Token: 0x06008A80 RID: 35456 RVA: 0x0034E3DC File Offset: 0x0034C5DC
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

	// Token: 0x06008A81 RID: 35457 RVA: 0x0034E430 File Offset: 0x0034C630
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

	// Token: 0x06008A82 RID: 35458 RVA: 0x0034E498 File Offset: 0x0034C698
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

	// Token: 0x06008A83 RID: 35459 RVA: 0x0034E4E8 File Offset: 0x0034C6E8
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

	// Token: 0x06008A84 RID: 35460 RVA: 0x0034E550 File Offset: 0x0034C750
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

	// Token: 0x06008A85 RID: 35461 RVA: 0x0034E5B4 File Offset: 0x0034C7B4
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

	// Token: 0x06008A86 RID: 35462 RVA: 0x0034E60C File Offset: 0x0034C80C
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

	// Token: 0x06008A87 RID: 35463 RVA: 0x0034E668 File Offset: 0x0034C868
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

	// Token: 0x06008A88 RID: 35464 RVA: 0x0034E6D0 File Offset: 0x0034C8D0
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

	// Token: 0x06008A89 RID: 35465 RVA: 0x0034E7ED File Offset: 0x0034C9ED
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

	// Token: 0x06008A8A RID: 35466 RVA: 0x0034E818 File Offset: 0x0034CA18
	[PublicizedFrom(EAccessModifier.Private)]
	public static void ReturnObjectArray(object[] _arr)
	{
		if (_arr != null && _arr.Length == 20)
		{
			BindingNcalcFunctions.objectArrays.Push(_arr);
		}
	}

	// Token: 0x06008A8B RID: 35467 RVA: 0x0034E830 File Offset: 0x0034CA30
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

	// Token: 0x06008A8D RID: 35469 RVA: 0x0034E8A7 File Offset: 0x0034CAA7
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static void <RegisterNcalcFunctions>g__TypeFoundCallback|50_0(Type _type, bool _hasMultiple, XuiBindingNcalcFunctionAttribute _classAttribute)
	{
		ReflectionHelpers.GetMethodsWithAttribute<XuiBindingNcalcFunctionAttribute>(_type, new Action<MethodInfo, bool, XuiBindingNcalcFunctionAttribute>(BindingNcalcFunctions.<RegisterNcalcFunctions>g__MethodFoundCallback|50_1), true, true, true);
	}

	// Token: 0x06008A8E RID: 35470 RVA: 0x0034E8C0 File Offset: 0x0034CAC0
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

	// Token: 0x040066F9 RID: 26361
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly object boxedFalse = false;

	// Token: 0x040066FA RID: 26362
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly object boxedTrue = true;

	// Token: 0x040066FB RID: 26363
	[PublicizedFrom(EAccessModifier.Private)]
	public const int ObjectArrayLengthCached = 20;

	// Token: 0x040066FC RID: 26364
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Stack<object[]> objectArrays = new Stack<object[]>();

	// Token: 0x040066FD RID: 26365
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Dictionary<string, BindingNcalcFunctions.FunctionDefinition> ncalcFunctions = new Dictionary<string, BindingNcalcFunctions.FunctionDefinition>();

	// Token: 0x02001114 RID: 4372
	// (Invoke) Token: 0x06008A90 RID: 35472
	public delegate void CustomNcalcFunctionDelegate(IBindingNcalc _bindingInstance, FunctionArgs _args, object[] _evaluatedArguments);

	// Token: 0x02001115 RID: 4373
	[PublicizedFrom(EAccessModifier.Private)]
	public class FunctionDefinition
	{
		// Token: 0x06008A93 RID: 35475 RVA: 0x0034E9B7 File Offset: 0x0034CBB7
		public FunctionDefinition(BindingNcalcFunctions.CustomNcalcFunctionDelegate _delegate, int _expectedArgumentCount, object _errorResult)
		{
			this.Delegate = _delegate;
			this.ExpectedArgumentCount = _expectedArgumentCount;
			this.ErrorResult = _errorResult;
		}

		// Token: 0x040066FE RID: 26366
		public readonly BindingNcalcFunctions.CustomNcalcFunctionDelegate Delegate;

		// Token: 0x040066FF RID: 26367
		public readonly int ExpectedArgumentCount;

		// Token: 0x04006700 RID: 26368
		public readonly object ErrorResult;
	}
}
