using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using NCalc;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001123 RID: 4387
[XuiBindingNcalcFunction]
[Preserve]
public static class BindingNcalcFunctions
{
	// Token: 0x06008AB4 RID: 35508 RVA: 0x0034F8B7 File Offset: 0x0034DAB7
	[XuiBindingNcalcFunction("<ERROR>", 1, null)]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void localization(IBindingNcalc _bindingInstance, FunctionArgs _args, object[] _evaluatedArguments)
	{
		_args.Result = Localization.Get(_evaluatedArguments[0].ToString(), false, null);
	}

	// Token: 0x06008AB5 RID: 35509 RVA: 0x0034F8D0 File Offset: 0x0034DAD0
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

	// Token: 0x06008AB6 RID: 35510 RVA: 0x0034F940 File Offset: 0x0034DB40
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

	// Token: 0x06008AB7 RID: 35511 RVA: 0x0034F9BC File Offset: 0x0034DBBC
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

	// Token: 0x06008AB8 RID: 35512 RVA: 0x0034FA38 File Offset: 0x0034DC38
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

	// Token: 0x06008AB9 RID: 35513 RVA: 0x0034FAF4 File Offset: 0x0034DCF4
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

	// Token: 0x06008ABA RID: 35514 RVA: 0x0034FBB0 File Offset: 0x0034DDB0
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

	// Token: 0x06008ABB RID: 35515 RVA: 0x0034FC68 File Offset: 0x0034DE68
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

	// Token: 0x06008ABC RID: 35516 RVA: 0x0034FCDC File Offset: 0x0034DEDC
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

	// Token: 0x06008ABD RID: 35517 RVA: 0x0034FD8A File Offset: 0x0034DF8A
	[XuiBindingNcalcFunction(0, 1, null)]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void length(IBindingNcalc _bindingInstance, FunctionArgs _args, object[] _evaluatedArguments)
	{
		_args.Result = _evaluatedArguments[0].ToString().Length;
	}

	// Token: 0x06008ABE RID: 35518 RVA: 0x0034FDA4 File Offset: 0x0034DFA4
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

	// Token: 0x06008ABF RID: 35519 RVA: 0x0034FDF4 File Offset: 0x0034DFF4
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

	// Token: 0x06008AC0 RID: 35520 RVA: 0x0034FE2C File Offset: 0x0034E02C
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

	// Token: 0x06008AC1 RID: 35521 RVA: 0x0034FE7C File Offset: 0x0034E07C
	[XuiBindingNcalcFunction("", 1, "str")]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void toString(IBindingNcalc _bindingInstance, FunctionArgs _args, object[] _evaluatedArguments)
	{
		object obj = _evaluatedArguments[0];
		_args.Result = ((obj != null) ? obj.ToString() : null) ?? "";
	}

	// Token: 0x06008AC2 RID: 35522 RVA: 0x0034FE9C File Offset: 0x0034E09C
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

	// Token: 0x06008AC3 RID: 35523 RVA: 0x0034FED8 File Offset: 0x0034E0D8
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

	// Token: 0x06008AC4 RID: 35524 RVA: 0x0034FF14 File Offset: 0x0034E114
	[XuiBindingNcalcFunction(false, 1, "bound")]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void isBound(IBindingNcalc _bindingInstance, FunctionArgs _args, object[] _evaluatedArguments)
	{
		throw new NotImplementedException();
	}

	// Token: 0x06008AC5 RID: 35525 RVA: 0x0034FF26 File Offset: 0x0034E126
	[XuiBindingNcalcFunction("", 1, null)]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void always(IBindingNcalc _bindingInstance, FunctionArgs _args, object[] _evaluatedArguments)
	{
		_bindingInstance.SetIndeterministic();
		_args.Result = _evaluatedArguments[0];
	}

	// Token: 0x06008AC6 RID: 35526 RVA: 0x0034FF38 File Offset: 0x0034E138
	[XuiBindingNcalcFunction(false, 1, "defined")]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void isDefined(IBindingNcalc _bindingInstance, FunctionArgs _args, object[] _evaluatedArguments)
	{
		object obj;
		_args.Result = (_bindingInstance.FindParameter(_evaluatedArguments[0].ToString(), out obj) ? BindingNcalcFunctions.boxedTrue : BindingNcalcFunctions.boxedFalse);
	}

	// Token: 0x06008AC7 RID: 35527 RVA: 0x0034FF6C File Offset: 0x0034E16C
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

	// Token: 0x06008AC8 RID: 35528 RVA: 0x0034FFFC File Offset: 0x0034E1FC
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

	// Token: 0x06008AC9 RID: 35529 RVA: 0x00350050 File Offset: 0x0034E250
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

	// Token: 0x06008ACA RID: 35530 RVA: 0x003500A4 File Offset: 0x0034E2A4
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

	// Token: 0x06008ACB RID: 35531 RVA: 0x003500F8 File Offset: 0x0034E2F8
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

	// Token: 0x06008ACC RID: 35532 RVA: 0x0035014C File Offset: 0x0034E34C
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

	// Token: 0x06008ACD RID: 35533 RVA: 0x003501CC File Offset: 0x0034E3CC
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

	// Token: 0x06008ACE RID: 35534 RVA: 0x0035025C File Offset: 0x0034E45C
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

	// Token: 0x06008ACF RID: 35535 RVA: 0x003502B0 File Offset: 0x0034E4B0
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

	// Token: 0x06008AD0 RID: 35536 RVA: 0x00350304 File Offset: 0x0034E504
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

	// Token: 0x06008AD1 RID: 35537 RVA: 0x00350354 File Offset: 0x0034E554
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

	// Token: 0x06008AD2 RID: 35538 RVA: 0x003503E4 File Offset: 0x0034E5E4
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

	// Token: 0x06008AD3 RID: 35539 RVA: 0x00350438 File Offset: 0x0034E638
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

	// Token: 0x06008AD4 RID: 35540 RVA: 0x0035048C File Offset: 0x0034E68C
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

	// Token: 0x06008AD5 RID: 35541 RVA: 0x003504E0 File Offset: 0x0034E6E0
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

	// Token: 0x06008AD6 RID: 35542 RVA: 0x00350548 File Offset: 0x0034E748
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

	// Token: 0x06008AD7 RID: 35543 RVA: 0x00350598 File Offset: 0x0034E798
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

	// Token: 0x06008AD8 RID: 35544 RVA: 0x00350600 File Offset: 0x0034E800
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

	// Token: 0x06008AD9 RID: 35545 RVA: 0x00350664 File Offset: 0x0034E864
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

	// Token: 0x06008ADA RID: 35546 RVA: 0x003506BC File Offset: 0x0034E8BC
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

	// Token: 0x06008ADB RID: 35547 RVA: 0x00350718 File Offset: 0x0034E918
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

	// Token: 0x06008ADC RID: 35548 RVA: 0x00350780 File Offset: 0x0034E980
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

	// Token: 0x06008ADD RID: 35549 RVA: 0x0035089D File Offset: 0x0034EA9D
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

	// Token: 0x06008ADE RID: 35550 RVA: 0x003508C8 File Offset: 0x0034EAC8
	[PublicizedFrom(EAccessModifier.Private)]
	public static void ReturnObjectArray(object[] _arr)
	{
		if (_arr != null && _arr.Length == 20)
		{
			BindingNcalcFunctions.objectArrays.Push(_arr);
		}
	}

	// Token: 0x06008ADF RID: 35551 RVA: 0x003508E0 File Offset: 0x0034EAE0
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

	// Token: 0x06008AE1 RID: 35553 RVA: 0x00350957 File Offset: 0x0034EB57
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static void <RegisterNcalcFunctions>g__TypeFoundCallback|50_0(Type _type, bool _hasMultiple, XuiBindingNcalcFunctionAttribute _classAttribute)
	{
		ReflectionHelpers.GetMethodsWithAttribute<XuiBindingNcalcFunctionAttribute>(_type, new Action<MethodInfo, bool, XuiBindingNcalcFunctionAttribute>(BindingNcalcFunctions.<RegisterNcalcFunctions>g__MethodFoundCallback|50_1), true, true, true);
	}

	// Token: 0x06008AE2 RID: 35554 RVA: 0x00350970 File Offset: 0x0034EB70
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

	// Token: 0x04006739 RID: 26425
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly object boxedFalse = false;

	// Token: 0x0400673A RID: 26426
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly object boxedTrue = true;

	// Token: 0x0400673B RID: 26427
	[PublicizedFrom(EAccessModifier.Private)]
	public const int ObjectArrayLengthCached = 20;

	// Token: 0x0400673C RID: 26428
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Stack<object[]> objectArrays = new Stack<object[]>();

	// Token: 0x0400673D RID: 26429
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Dictionary<string, BindingNcalcFunctions.FunctionDefinition> ncalcFunctions = new Dictionary<string, BindingNcalcFunctions.FunctionDefinition>();

	// Token: 0x02001124 RID: 4388
	// (Invoke) Token: 0x06008AE4 RID: 35556
	public delegate void CustomNcalcFunctionDelegate(IBindingNcalc _bindingInstance, FunctionArgs _args, object[] _evaluatedArguments);

	// Token: 0x02001125 RID: 4389
	[PublicizedFrom(EAccessModifier.Private)]
	public class FunctionDefinition
	{
		// Token: 0x06008AE7 RID: 35559 RVA: 0x00350A67 File Offset: 0x0034EC67
		public FunctionDefinition(BindingNcalcFunctions.CustomNcalcFunctionDelegate _delegate, int _expectedArgumentCount, object _errorResult)
		{
			this.Delegate = _delegate;
			this.ExpectedArgumentCount = _expectedArgumentCount;
			this.ErrorResult = _errorResult;
		}

		// Token: 0x0400673E RID: 26430
		public readonly BindingNcalcFunctions.CustomNcalcFunctionDelegate Delegate;

		// Token: 0x0400673F RID: 26431
		public readonly int ExpectedArgumentCount;

		// Token: 0x04006740 RID: 26432
		public readonly object ErrorResult;
	}
}
