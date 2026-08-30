using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using NCalc;
using Unity.Profiling;

// Token: 0x020010CC RID: 4300
public class BindingInfoNcalc : IBindingInstance, IBindingNcalc
{
	// Token: 0x060088A3 RID: 34979 RVA: 0x00342F3C File Offset: 0x0034113C
	public static bool IsFullNcalcBinding(string _input)
	{
		if (!BindingInfoNcalc.fullBinding.IsMatch(_input))
		{
			return false;
		}
		int num = 0;
		for (int i = 2; i < _input.Length - 1; i++)
		{
			if (_input[i] == '{')
			{
				num++;
			}
			else if (_input[i] == '}')
			{
				num--;
			}
			if (num < 0)
			{
				return false;
			}
		}
		return num <= 0;
	}

	// Token: 0x060088A4 RID: 34980 RVA: 0x00342F9C File Offset: 0x0034119C
	public BindingInfoNcalc(IXUiElement _targetViewOrTween, string _attribute, string _sourceString)
	{
		this.targetViewOrTween = _targetViewOrTween;
		this.attributeName = _attribute;
		this.sourceString = _sourceString;
		this.pmRefreshValueCompleteSourceText = new ProfilerMarker(_sourceString);
		int num = 1;
		if (_sourceString[1] == '%' || _sourceString[1] == '#')
		{
			num++;
		}
		string text = _sourceString.Substring(num, _sourceString.Length - num - 1);
		Match match = BindingInfoNcalc.singleIdentifierExpression.Match(text);
		if (match.Success)
		{
			string value = match.Groups[1].Value;
			this.singleIdentifierBinding = true;
			if (!this.FindParameter(value, out this.currentValue))
			{
				if (XUiFromXml.DebugXuiLoading != XUiFromXml.DebugLevel.Off)
				{
					Log.Warning("[XUi] Binding name '" + value + "' not found! Hierarchy: " + this.xuiHierarchy());
				}
				return;
			}
		}
		else
		{
			this.expression = new Expression(text, EvaluateOptions.NoCache | EvaluateOptions.UseDoubleForAbsFunction | EvaluateOptions.ReuseInstances)
			{
				Parameters = this.expressionParamDict
			};
			this.expression.EvaluateFunction += this.nCalcEvaluateFunction;
			this.expression.EvaluateParameter += this.nCalcEvaluateParameter;
			this.IsInitializing = true;
			this.currentValue = this.evaluateExpression();
			this.IsInitializing = false;
		}
		if (this.currentValue == null)
		{
			this.expression = null;
			Log.Error(string.Concat(new string[]
			{
				"[XUi] Failed evaluating binding expression, result is null. Binding ",
				this.attributeName,
				"=\"",
				this.sourceString,
				"\", view hierarchy: ",
				this.xuiHierarchy()
			}));
			return;
		}
		this.<.ctor>g__BindToController|16_0();
		Type type = this.currentValue.GetType();
		ParsingMethodCache.ParsingMethodData parsingMethodData;
		if (ParsingMethodCache.Instance.TryGetParsingDelegate(this.targetViewOrTween, this.attributeName, out parsingMethodData))
		{
			if (parsingMethodData.TryGetDelegateForSourceType(type, out this.targetParsingDelegate))
			{
				this.targetParsingElement = this.targetViewOrTween;
				return;
			}
			Log.Error(string.Concat(new string[]
			{
				"[XUi] Failed mapping binding to parser on view, no conversion possible. Binding ",
				this.attributeName,
				"=\"",
				this.sourceString,
				"\", binding result type ",
				type.FullName,
				", parser native type ",
				parsingMethodData.NativeParseType.FullName,
				", view hierarchy: ",
				this.xuiHierarchy()
			}));
			this.expression = null;
			return;
		}
		else
		{
			if (this.targetViewOrTween is XUiTweenAbs)
			{
				Log.Error("[XUi] Failed mapping binding to parser on XUiTween, no parse found for attribute name '" + this.attributeName + "', view hierarchy: " + this.xuiHierarchy());
				this.expression = null;
				return;
			}
			ParsingMethodCache.ParsingMethodData parsingMethodData2;
			if (!ParsingMethodCache.Instance.TryGetParsingDelegate(this.targetViewOrTween.Controller, this.attributeName, out parsingMethodData2))
			{
				this.targetParsingElement = this.targetViewOrTween.Controller;
				return;
			}
			if (parsingMethodData2.TryGetDelegateForSourceType(type, out this.targetParsingDelegate))
			{
				this.targetParsingElement = this.targetViewOrTween.Controller;
				return;
			}
			Log.Error(string.Concat(new string[]
			{
				"[XUi] Failed mapping binding to parser on controller, no conversion possible. Binding ",
				this.attributeName,
				"=\"",
				this.sourceString,
				"\", binding result type ",
				type.FullName,
				", parser native type ",
				parsingMethodData2.NativeParseType.FullName,
				", view hierarchy: ",
				this.xuiHierarchy()
			}));
			this.expression = null;
			return;
		}
	}

	// Token: 0x060088A5 RID: 34981 RVA: 0x00343348 File Offset: 0x00341548
	public void RefreshValue()
	{
		if ((this.singleIdentifierBinding && this.variables.Count == 0) || (this.expression == null && !this.singleIdentifierBinding))
		{
			return;
		}
		using (this.pmRefreshValueComplete.Auto())
		{
			using (this.pmRefreshValueCompleteSourceText.Auto())
			{
				bool flag = this.usesIndeterministicFunctions;
				using (this.pmRefreshBindings.Auto())
				{
					for (int i = 0; i < this.variables.Count; i++)
					{
						flag |= this.variables[i].RefreshValue();
					}
				}
				if (this.singleIdentifierBinding)
				{
					this.currentValue = this.variables[0].CurrentValue;
				}
				else
				{
					using (this.pmNcalcEval.Auto())
					{
						if (flag)
						{
							this.currentValue = this.evaluateExpression();
						}
					}
				}
				try
				{
					object obj = this.currentValue;
					string text = obj as string;
					if (text != null && text.Contains("{cvar("))
					{
						obj = BindingsManager.ReplaceCVars(text);
					}
					if (obj == null)
					{
						Log.Error(string.Concat(new string[]
						{
							"[XUi] Binding returned null value: ",
							this.attributeName,
							"=\"",
							this.sourceString,
							"\", view hierarchy: ",
							this.xuiHierarchy(),
							":"
						}));
					}
					else
					{
						using (this.pmApplyAttribute.Auto())
						{
							if (this.targetParsingDelegate == null)
							{
								((XUiController)this.targetParsingElement).CustomAttributes[this.attributeName] = obj;
							}
							else
							{
								this.targetParsingDelegate(this.targetParsingElement, obj);
							}
						}
					}
				}
				catch (Exception ex)
				{
					string text2 = "[XUi] Exception parsing result of binding. Binding {0}=\"{1}\", binding result: '{2}' (type: {3}), view hierarchy: {4}:";
					object[] array = new object[5];
					array[0] = this.attributeName;
					array[1] = this.sourceString;
					array[2] = this.currentValue;
					int num = 3;
					object obj2 = this.currentValue;
					array[num] = ((obj2 != null) ? obj2.GetType().FullName : null);
					array[4] = this.xuiHierarchy();
					Log.Error(string.Format(text2, array));
					Log.Exception(ex);
				}
			}
		}
	}

	// Token: 0x060088A6 RID: 34982 RVA: 0x0034362C File Offset: 0x0034182C
	[PublicizedFrom(EAccessModifier.Private)]
	public object evaluateExpression()
	{
		object obj;
		try
		{
			obj = this.expression.Evaluate();
		}
		catch (Exception ex)
		{
			Log.Error(string.Concat(new string[]
			{
				"[XUi] Binding expression can not be evaluated. Binding ",
				this.attributeName,
				"=\"",
				this.sourceString,
				"\" --- hierarchy: ",
				this.xuiHierarchy()
			}));
			Log.Exception(ex);
			obj = null;
		}
		return obj;
	}

	// Token: 0x060088A7 RID: 34983 RVA: 0x003436A8 File Offset: 0x003418A8
	[PublicizedFrom(EAccessModifier.Private)]
	public void nCalcEvaluateParameter(string _name, ParameterArgs _args)
	{
		object obj;
		if (this.FindParameter(_name, out obj))
		{
			_args.Result = obj;
		}
	}

	// Token: 0x060088A8 RID: 34984 RVA: 0x003436C7 File Offset: 0x003418C7
	[PublicizedFrom(EAccessModifier.Private)]
	public void nCalcEvaluateFunction(string _name, FunctionArgs _args, bool _ignoreCase)
	{
		BindingNcalcFunctions.EvaluateFunc(this, _name, _args);
	}

	// Token: 0x060088A9 RID: 34985 RVA: 0x003436D4 File Offset: 0x003418D4
	public bool FindParameter(string _name, out object _value)
	{
		if (this.expressionParamDict.TryGetValue(_name, out _value))
		{
			return true;
		}
		for (XUiController xuiController = this.targetViewOrTween.Controller; xuiController != null; xuiController = xuiController.Parent)
		{
			if (xuiController.GetType() != typeof(XUiController))
			{
				object obj = null;
				BindingInfoNcalc.VariableStateAbs variableStateAbs = null;
				XuiBindingDelegate xuiBindingDelegate;
				if (BindingMethodCache.Instance.TryGetBindingDelegate(xuiController, _name, out xuiBindingDelegate))
				{
					obj = xuiBindingDelegate(xuiController);
					variableStateAbs = new BindingInfoNcalc.VariableStateParamBinding(_name, xuiController, obj, xuiBindingDelegate, this.expressionParamDict);
				}
				else
				{
					string text = "";
					if (xuiController.GetBindingValue(ref text, _name))
					{
						variableStateAbs = new BindingInfoNcalc.VariableStateLegacyBinding(_name, xuiController, text, this.expressionParamDict);
						obj = text;
					}
				}
				if (variableStateAbs != null)
				{
					xuiController.Bindings.AddBinding(this);
					this.boundToControllers++;
					this.variables.Add(variableStateAbs);
					_value = obj;
					return true;
				}
			}
		}
		if (XUiFromXml.DebugXuiLoading == XUiFromXml.DebugLevel.Verbose)
		{
			Log.Warning("[XUi] Binding name '" + _name + "' not found! Hierarchy: " + this.xuiHierarchy());
		}
		return false;
	}

	// Token: 0x060088AA RID: 34986 RVA: 0x003437CF File Offset: 0x003419CF
	[PublicizedFrom(EAccessModifier.Private)]
	public string xuiHierarchy()
	{
		return this.targetViewOrTween.GetXuiHierarchy();
	}

	// Token: 0x17000FBA RID: 4026
	// (get) Token: 0x060088AB RID: 34987 RVA: 0x003437DC File Offset: 0x003419DC
	public string SourceText
	{
		get
		{
			return this.sourceString;
		}
	}

	// Token: 0x17000FBB RID: 4027
	// (get) Token: 0x060088AC RID: 34988 RVA: 0x003437E4 File Offset: 0x003419E4
	public IXUiElement TargetElement
	{
		get
		{
			return this.targetViewOrTween;
		}
	}

	// Token: 0x17000FBC RID: 4028
	// (get) Token: 0x060088AD RID: 34989 RVA: 0x003437EC File Offset: 0x003419EC
	public bool IsInitializing { get; }

	// Token: 0x060088AE RID: 34990 RVA: 0x003437F4 File Offset: 0x003419F4
	public void SetIndeterministic()
	{
		this.usesIndeterministicFunctions = true;
	}

	// Token: 0x060088AF RID: 34991 RVA: 0x003437FD File Offset: 0x003419FD
	public bool RegisterVariable(BindingInfoNcalc.VariableStateAbs _var)
	{
		this.variables.Add(_var);
		return true;
	}

	// Token: 0x060088B1 RID: 34993 RVA: 0x00343838 File Offset: 0x00341A38
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public void <.ctor>g__BindToController|16_0()
	{
		if (!this.usesIndeterministicFunctions && this.boundToControllers != 0)
		{
			return;
		}
		for (XUiController xuiController = this.targetViewOrTween.Controller; xuiController != null; xuiController = xuiController.Parent)
		{
			if (xuiController.GetType() != typeof(XUiController))
			{
				xuiController.Bindings.AddBinding(this);
				return;
			}
		}
	}

	// Token: 0x0400659F RID: 26015
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly IXUiElement targetViewOrTween;

	// Token: 0x040065A0 RID: 26016
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly string attributeName;

	// Token: 0x040065A1 RID: 26017
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly string sourceString;

	// Token: 0x040065A2 RID: 26018
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly IXUiElement targetParsingElement;

	// Token: 0x040065A3 RID: 26019
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XuiParsingDelegate targetParsingDelegate;

	// Token: 0x040065A4 RID: 26020
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<BindingInfoNcalc.VariableStateAbs> variables = new List<BindingInfoNcalc.VariableStateAbs>();

	// Token: 0x040065A5 RID: 26021
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<string, object> expressionParamDict = new Dictionary<string, object>(StringComparer.Ordinal);

	// Token: 0x040065A6 RID: 26022
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Expression expression;

	// Token: 0x040065A7 RID: 26023
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly bool singleIdentifierBinding;

	// Token: 0x040065A8 RID: 26024
	[PublicizedFrom(EAccessModifier.Private)]
	public bool usesIndeterministicFunctions;

	// Token: 0x040065A9 RID: 26025
	[PublicizedFrom(EAccessModifier.Private)]
	public int boundToControllers;

	// Token: 0x040065AA RID: 26026
	[PublicizedFrom(EAccessModifier.Private)]
	public object currentValue;

	// Token: 0x040065AB RID: 26027
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Regex fullBinding = new Regex("^\\{%.*\\}$", RegexOptions.Compiled | RegexOptions.CultureInvariant);

	// Token: 0x040065AC RID: 26028
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Regex singleIdentifierExpression = new Regex("^\\s*([a-zA-Z]\\w*)\\s*$", RegexOptions.Compiled | RegexOptions.CultureInvariant);

	// Token: 0x040065AD RID: 26029
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly ProfilerMarker pmCtor = new ProfilerMarker("BindingInfoNcalc.ctor");

	// Token: 0x040065AE RID: 26030
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly ProfilerMarker pmRefreshValueComplete = new ProfilerMarker("BindingInfoNcalc.RefreshValue");

	// Token: 0x040065AF RID: 26031
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly ProfilerMarker pmRefreshValueCompleteSourceText;

	// Token: 0x040065B0 RID: 26032
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly ProfilerMarker pmRefreshBindings = new ProfilerMarker("Refresh Bindings");

	// Token: 0x040065B1 RID: 26033
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly ProfilerMarker pmNcalcEval = new ProfilerMarker("NCalc.Evaluate");

	// Token: 0x040065B2 RID: 26034
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly ProfilerMarker pmApplyAttribute = new ProfilerMarker("Apply Attribute");

	// Token: 0x020010CD RID: 4301
	public abstract class VariableStateAbs
	{
		// Token: 0x17000FBD RID: 4029
		// (get) Token: 0x060088B2 RID: 34994
		public abstract object CurrentValue { get; }

		// Token: 0x060088B3 RID: 34995
		public abstract bool RefreshValue();

		// Token: 0x060088B4 RID: 34996 RVA: 0x0000640C File Offset: 0x0000460C
		[PublicizedFrom(EAccessModifier.Protected)]
		public VariableStateAbs()
		{
		}
	}

	// Token: 0x020010CE RID: 4302
	public abstract class VariableStateBinding : BindingInfoNcalc.VariableStateAbs
	{
		// Token: 0x060088B5 RID: 34997 RVA: 0x00343892 File Offset: 0x00341A92
		[PublicizedFrom(EAccessModifier.Protected)]
		public VariableStateBinding(string _name, XUiController _controller, Dictionary<string, object> _expressionParamDict)
		{
			this.BindingName = _name;
			this.Controller = _controller;
			this.ExpressionParamDict = _expressionParamDict;
		}

		// Token: 0x040065B4 RID: 26036
		[PublicizedFrom(EAccessModifier.Protected)]
		public readonly XUiController Controller;

		// Token: 0x040065B5 RID: 26037
		[PublicizedFrom(EAccessModifier.Protected)]
		public readonly string BindingName;

		// Token: 0x040065B6 RID: 26038
		[PublicizedFrom(EAccessModifier.Protected)]
		public readonly Dictionary<string, object> ExpressionParamDict;
	}

	// Token: 0x020010CF RID: 4303
	public class VariableStateParamBinding : BindingInfoNcalc.VariableStateBinding
	{
		// Token: 0x060088B6 RID: 34998 RVA: 0x003438AF File Offset: 0x00341AAF
		public VariableStateParamBinding(string _name, XUiController _controller, object _initialValue, XuiBindingDelegate _bindingDelegate, Dictionary<string, object> _expressionParamDict)
			: base(_name, _controller, _expressionParamDict)
		{
			this.currentValue = _initialValue;
			this.bindingDelegate = _bindingDelegate;
			this.ExpressionParamDict[this.BindingName] = _initialValue;
		}

		// Token: 0x060088B7 RID: 34999 RVA: 0x003438DC File Offset: 0x00341ADC
		public override bool RefreshValue()
		{
			object obj = this.bindingDelegate(this.Controller);
			if (obj == null)
			{
				Log.Warning(string.Concat(new string[]
				{
					"[XUi] Binding '",
					this.BindingName,
					"' on controller '",
					this.Controller.GetType().FullName,
					"' returned null, should always return appropriate non-null value of same data type. Hierarchy: ",
					this.Controller.GetXuiHierarchy()
				}));
				return false;
			}
			if (obj.Equals(this.CurrentValue))
			{
				return false;
			}
			this.currentValue = obj;
			this.ExpressionParamDict[this.BindingName] = obj;
			return true;
		}

		// Token: 0x060088B8 RID: 35000 RVA: 0x0034397C File Offset: 0x00341B7C
		public override string ToString()
		{
			return string.Format("ParamBinding: {0}='{1}'", this.BindingName, this.currentValue);
		}

		// Token: 0x17000FBE RID: 4030
		// (get) Token: 0x060088B9 RID: 35001 RVA: 0x00343994 File Offset: 0x00341B94
		public override object CurrentValue
		{
			get
			{
				return this.currentValue;
			}
		}

		// Token: 0x040065B7 RID: 26039
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly XuiBindingDelegate bindingDelegate;

		// Token: 0x040065B8 RID: 26040
		[PublicizedFrom(EAccessModifier.Private)]
		public object currentValue;
	}

	// Token: 0x020010D0 RID: 4304
	public class VariableStateLegacyBinding : BindingInfoNcalc.VariableStateBinding
	{
		// Token: 0x060088BA RID: 35002 RVA: 0x0034399C File Offset: 0x00341B9C
		public VariableStateLegacyBinding(string _name, XUiController _controller, string _initialValue, Dictionary<string, object> _expressionParamDict)
			: base(_name, _controller, _expressionParamDict)
		{
			this.currentValue = _initialValue;
			this.ExpressionParamDict[this.BindingName] = _initialValue;
		}

		// Token: 0x060088BB RID: 35003 RVA: 0x003439C4 File Offset: 0x00341BC4
		public override bool RefreshValue()
		{
			string text = "";
			if (!this.Controller.GetBindingValue(ref text, this.BindingName))
			{
				Log.Error(string.Concat(new string[]
				{
					"[XUi] Refreshing binding failed: Controller's GetBindingValue no longer returns true! (Binding: ",
					this.BindingName,
					", hierarchy: ",
					this.Controller.GetXuiHierarchy(),
					")"
				}));
				return false;
			}
			if (string.Equals(this.currentValue, text, StringComparison.Ordinal))
			{
				return false;
			}
			this.currentValue = text;
			this.ExpressionParamDict[this.BindingName] = text;
			return true;
		}

		// Token: 0x060088BC RID: 35004 RVA: 0x00343A58 File Offset: 0x00341C58
		public override string ToString()
		{
			return string.Concat(new string[] { "LegacyBinding: ", this.BindingName, "='", this.currentValue, "'" });
		}

		// Token: 0x17000FBF RID: 4031
		// (get) Token: 0x060088BD RID: 35005 RVA: 0x00343A8F File Offset: 0x00341C8F
		public override object CurrentValue
		{
			get
			{
				return this.currentValue;
			}
		}

		// Token: 0x040065B9 RID: 26041
		[PublicizedFrom(EAccessModifier.Private)]
		public string currentValue;
	}

	// Token: 0x020010D1 RID: 4305
	public class VariableStateCVar : BindingInfoNcalc.VariableStateAbs
	{
		// Token: 0x060088BE RID: 35006 RVA: 0x00343A97 File Offset: 0x00341C97
		public VariableStateCVar(string _name)
		{
			this.cVarName = _name;
			this.currentValue = 0f;
		}

		// Token: 0x060088BF RID: 35007 RVA: 0x00343AB4 File Offset: 0x00341CB4
		public override bool RefreshValue()
		{
			GameManager instance = GameManager.Instance;
			if (instance == null)
			{
				return false;
			}
			World world = instance.World;
			EntityPlayer entityPlayer = ((world != null) ? world.GetPrimaryPlayer() : null);
			if (entityPlayer == null)
			{
				return false;
			}
			float cvar = entityPlayer.GetCVar(this.cVarName);
			if (cvar.Equals(this.currentValue))
			{
				return false;
			}
			this.currentValue = cvar;
			return true;
		}

		// Token: 0x060088C0 RID: 35008 RVA: 0x00343B16 File Offset: 0x00341D16
		public override string ToString()
		{
			return string.Format("cVar: {0}={1}", this.cVarName, this.currentValue);
		}

		// Token: 0x17000FC0 RID: 4032
		// (get) Token: 0x060088C1 RID: 35009 RVA: 0x00343B33 File Offset: 0x00341D33
		public override object CurrentValue
		{
			get
			{
				return this.currentValue;
			}
		}

		// Token: 0x040065BA RID: 26042
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string cVarName;

		// Token: 0x040065BB RID: 26043
		[PublicizedFrom(EAccessModifier.Private)]
		public float currentValue;
	}

	// Token: 0x020010D2 RID: 4306
	public abstract class VariableStateSimpleLookupAbs : BindingInfoNcalc.VariableStateAbs
	{
		// Token: 0x060088C2 RID: 35010
		[PublicizedFrom(EAccessModifier.Protected)]
		public abstract object getCurrentValue();

		// Token: 0x17000FC1 RID: 4033
		// (get) Token: 0x060088C3 RID: 35011
		public abstract string VarType
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get;
		}

		// Token: 0x17000FC2 RID: 4034
		// (get) Token: 0x060088C4 RID: 35012
		public abstract string VarName
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get;
		}

		// Token: 0x060088C5 RID: 35013 RVA: 0x00343B40 File Offset: 0x00341D40
		public override bool RefreshValue()
		{
			object obj = this.getCurrentValue();
			if (obj == null)
			{
				Log.Warning(string.Concat(new string[] { "[XUi] ", this.VarType, " '", this.VarName, "' returned null." }));
				return false;
			}
			if (obj.Equals(this.CurrentValue))
			{
				return false;
			}
			this.currentValue = obj;
			return true;
		}

		// Token: 0x060088C6 RID: 35014 RVA: 0x00343BAB File Offset: 0x00341DAB
		public override string ToString()
		{
			return string.Format("{0}: {1}={2}", this.VarType, this.VarName, this.currentValue);
		}

		// Token: 0x17000FC3 RID: 4035
		// (get) Token: 0x060088C7 RID: 35015 RVA: 0x00343BC9 File Offset: 0x00341DC9
		public override object CurrentValue
		{
			get
			{
				return this.currentValue;
			}
		}

		// Token: 0x060088C8 RID: 35016 RVA: 0x00343BD1 File Offset: 0x00341DD1
		[PublicizedFrom(EAccessModifier.Protected)]
		public VariableStateSimpleLookupAbs()
		{
		}

		// Token: 0x040065BC RID: 26044
		[PublicizedFrom(EAccessModifier.Protected)]
		public object currentValue;
	}

	// Token: 0x020010D3 RID: 4307
	public class VariableStateGamePref : BindingInfoNcalc.VariableStateSimpleLookupAbs
	{
		// Token: 0x060088C9 RID: 35017 RVA: 0x00343BD9 File Offset: 0x00341DD9
		public VariableStateGamePref(EnumGamePrefs _pref)
		{
			this.pref = _pref;
			this.currentValue = this.getCurrentValue();
		}

		// Token: 0x060088CA RID: 35018 RVA: 0x00343BF4 File Offset: 0x00341DF4
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object getCurrentValue()
		{
			return GamePrefs.GetObject(this.pref);
		}

		// Token: 0x17000FC4 RID: 4036
		// (get) Token: 0x060088CB RID: 35019 RVA: 0x00343C01 File Offset: 0x00341E01
		public override string VarType
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return "GamePref";
			}
		}

		// Token: 0x17000FC5 RID: 4037
		// (get) Token: 0x060088CC RID: 35020 RVA: 0x00343C08 File Offset: 0x00341E08
		public override string VarName
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return this.pref.ToStringCached<EnumGamePrefs>();
			}
		}

		// Token: 0x040065BD RID: 26045
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly EnumGamePrefs pref;
	}

	// Token: 0x020010D4 RID: 4308
	public class VariableStateGameStat : BindingInfoNcalc.VariableStateSimpleLookupAbs
	{
		// Token: 0x060088CD RID: 35021 RVA: 0x00343C15 File Offset: 0x00341E15
		public VariableStateGameStat(EnumGameStats _stat)
		{
			this.stat = _stat;
			this.currentValue = this.getCurrentValue();
		}

		// Token: 0x060088CE RID: 35022 RVA: 0x00343C30 File Offset: 0x00341E30
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object getCurrentValue()
		{
			return GameStats.GetObject(this.stat);
		}

		// Token: 0x17000FC6 RID: 4038
		// (get) Token: 0x060088CF RID: 35023 RVA: 0x00343C3D File Offset: 0x00341E3D
		public override string VarType
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return "GameStat";
			}
		}

		// Token: 0x17000FC7 RID: 4039
		// (get) Token: 0x060088D0 RID: 35024 RVA: 0x00343C44 File Offset: 0x00341E44
		public override string VarName
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return this.stat.ToStringCached<EnumGameStats>();
			}
		}

		// Token: 0x040065BE RID: 26046
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly EnumGameStats stat;
	}

	// Token: 0x020010D5 RID: 4309
	public class VariableStateGameInfoInt : BindingInfoNcalc.VariableStateSimpleLookupAbs
	{
		// Token: 0x060088D1 RID: 35025 RVA: 0x00343C51 File Offset: 0x00341E51
		public VariableStateGameInfoInt(GameInfoInt _name)
		{
			this.name = _name;
			this.currentValue = this.getCurrentValue();
		}

		// Token: 0x060088D2 RID: 35026 RVA: 0x00343C6C File Offset: 0x00341E6C
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object getCurrentValue()
		{
			return (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer ? SingletonMonoBehaviour<ConnectionManager>.Instance.LocalServerInfo : SingletonMonoBehaviour<ConnectionManager>.Instance.LastGameServerInfo).GetValue(this.name);
		}

		// Token: 0x17000FC8 RID: 4040
		// (get) Token: 0x060088D3 RID: 35027 RVA: 0x00343CA0 File Offset: 0x00341EA0
		public override string VarType
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return "GameInfoInt";
			}
		}

		// Token: 0x17000FC9 RID: 4041
		// (get) Token: 0x060088D4 RID: 35028 RVA: 0x00343CA7 File Offset: 0x00341EA7
		public override string VarName
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return this.name.ToStringCached<GameInfoInt>();
			}
		}

		// Token: 0x040065BF RID: 26047
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly GameInfoInt name;
	}

	// Token: 0x020010D6 RID: 4310
	public class VariableStateGameInfoBool : BindingInfoNcalc.VariableStateSimpleLookupAbs
	{
		// Token: 0x060088D5 RID: 35029 RVA: 0x00343CB4 File Offset: 0x00341EB4
		public VariableStateGameInfoBool(GameInfoBool _name)
		{
			this.name = _name;
			this.currentValue = this.getCurrentValue();
		}

		// Token: 0x060088D6 RID: 35030 RVA: 0x00343CCF File Offset: 0x00341ECF
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object getCurrentValue()
		{
			return (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer ? SingletonMonoBehaviour<ConnectionManager>.Instance.LocalServerInfo : SingletonMonoBehaviour<ConnectionManager>.Instance.LastGameServerInfo).GetValue(this.name);
		}

		// Token: 0x17000FCA RID: 4042
		// (get) Token: 0x060088D7 RID: 35031 RVA: 0x00343D03 File Offset: 0x00341F03
		public override string VarType
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return "GameInfoBool";
			}
		}

		// Token: 0x17000FCB RID: 4043
		// (get) Token: 0x060088D8 RID: 35032 RVA: 0x00343D0A File Offset: 0x00341F0A
		public override string VarName
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return this.name.ToStringCached<GameInfoBool>();
			}
		}

		// Token: 0x040065C0 RID: 26048
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly GameInfoBool name;
	}

	// Token: 0x020010D7 RID: 4311
	public class VariableStateGameInfoString : BindingInfoNcalc.VariableStateSimpleLookupAbs
	{
		// Token: 0x060088D9 RID: 35033 RVA: 0x00343D17 File Offset: 0x00341F17
		public VariableStateGameInfoString(GameInfoString _name)
		{
			this.name = _name;
			this.currentValue = this.getCurrentValue();
		}

		// Token: 0x060088DA RID: 35034 RVA: 0x00343D32 File Offset: 0x00341F32
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object getCurrentValue()
		{
			return (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer ? SingletonMonoBehaviour<ConnectionManager>.Instance.LocalServerInfo : SingletonMonoBehaviour<ConnectionManager>.Instance.LastGameServerInfo).GetValue(this.name);
		}

		// Token: 0x17000FCC RID: 4044
		// (get) Token: 0x060088DB RID: 35035 RVA: 0x00343D61 File Offset: 0x00341F61
		public override string VarType
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return "GameInfoString";
			}
		}

		// Token: 0x17000FCD RID: 4045
		// (get) Token: 0x060088DC RID: 35036 RVA: 0x00343D68 File Offset: 0x00341F68
		public override string VarName
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return this.name.ToStringCached<GameInfoString>();
			}
		}

		// Token: 0x040065C1 RID: 26049
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly GameInfoString name;
	}
}
