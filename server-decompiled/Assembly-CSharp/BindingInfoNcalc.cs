using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using NCalc;
using Unity.Profiling;

// Token: 0x020010F6 RID: 4342
public class BindingInfoNcalc : IBindingInstance, IBindingNcalc
{
	// Token: 0x060089E5 RID: 35301 RVA: 0x0034B6F0 File Offset: 0x003498F0
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

	// Token: 0x060089E6 RID: 35302 RVA: 0x0034B750 File Offset: 0x00349950
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

	// Token: 0x060089E7 RID: 35303 RVA: 0x0034BAFC File Offset: 0x00349CFC
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

	// Token: 0x060089E8 RID: 35304 RVA: 0x0034BDE0 File Offset: 0x00349FE0
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

	// Token: 0x060089E9 RID: 35305 RVA: 0x0034BE5C File Offset: 0x0034A05C
	[PublicizedFrom(EAccessModifier.Private)]
	public void nCalcEvaluateParameter(string _name, ParameterArgs _args)
	{
		object obj;
		if (this.FindParameter(_name, out obj))
		{
			_args.Result = obj;
		}
	}

	// Token: 0x060089EA RID: 35306 RVA: 0x0034BE7B File Offset: 0x0034A07B
	[PublicizedFrom(EAccessModifier.Private)]
	public void nCalcEvaluateFunction(string _name, FunctionArgs _args, bool _ignoreCase)
	{
		BindingNcalcFunctions.EvaluateFunc(this, _name, _args);
	}

	// Token: 0x060089EB RID: 35307 RVA: 0x0034BE88 File Offset: 0x0034A088
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

	// Token: 0x060089EC RID: 35308 RVA: 0x0034BF83 File Offset: 0x0034A183
	[PublicizedFrom(EAccessModifier.Private)]
	public string xuiHierarchy()
	{
		return this.targetViewOrTween.GetXuiHierarchy();
	}

	// Token: 0x17001028 RID: 4136
	// (get) Token: 0x060089ED RID: 35309 RVA: 0x0034BF90 File Offset: 0x0034A190
	public string SourceText
	{
		get
		{
			return this.sourceString;
		}
	}

	// Token: 0x17001029 RID: 4137
	// (get) Token: 0x060089EE RID: 35310 RVA: 0x0034BF98 File Offset: 0x0034A198
	public IXUiElement TargetElement
	{
		get
		{
			return this.targetViewOrTween;
		}
	}

	// Token: 0x1700102A RID: 4138
	// (get) Token: 0x060089EF RID: 35311 RVA: 0x0034BFA0 File Offset: 0x0034A1A0
	public bool IsInitializing { get; }

	// Token: 0x060089F0 RID: 35312 RVA: 0x0034BFA8 File Offset: 0x0034A1A8
	public void SetIndeterministic()
	{
		this.usesIndeterministicFunctions = true;
	}

	// Token: 0x060089F1 RID: 35313 RVA: 0x0034BFB1 File Offset: 0x0034A1B1
	public bool RegisterVariable(BindingInfoNcalc.VariableStateAbs _var)
	{
		this.variables.Add(_var);
		return true;
	}

	// Token: 0x060089F3 RID: 35315 RVA: 0x0034BFEC File Offset: 0x0034A1EC
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

	// Token: 0x040066AD RID: 26285
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly IXUiElement targetViewOrTween;

	// Token: 0x040066AE RID: 26286
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly string attributeName;

	// Token: 0x040066AF RID: 26287
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly string sourceString;

	// Token: 0x040066B0 RID: 26288
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly IXUiElement targetParsingElement;

	// Token: 0x040066B1 RID: 26289
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XuiParsingDelegate targetParsingDelegate;

	// Token: 0x040066B2 RID: 26290
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<BindingInfoNcalc.VariableStateAbs> variables = new List<BindingInfoNcalc.VariableStateAbs>();

	// Token: 0x040066B3 RID: 26291
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<string, object> expressionParamDict = new Dictionary<string, object>(StringComparer.Ordinal);

	// Token: 0x040066B4 RID: 26292
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Expression expression;

	// Token: 0x040066B5 RID: 26293
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly bool singleIdentifierBinding;

	// Token: 0x040066B6 RID: 26294
	[PublicizedFrom(EAccessModifier.Private)]
	public bool usesIndeterministicFunctions;

	// Token: 0x040066B7 RID: 26295
	[PublicizedFrom(EAccessModifier.Private)]
	public int boundToControllers;

	// Token: 0x040066B8 RID: 26296
	[PublicizedFrom(EAccessModifier.Private)]
	public object currentValue;

	// Token: 0x040066B9 RID: 26297
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Regex fullBinding = new Regex("^\\{%.*\\}$", RegexOptions.Compiled | RegexOptions.CultureInvariant);

	// Token: 0x040066BA RID: 26298
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Regex singleIdentifierExpression = new Regex("^\\s*([a-zA-Z]\\w*)\\s*$", RegexOptions.Compiled | RegexOptions.CultureInvariant);

	// Token: 0x040066BB RID: 26299
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly ProfilerMarker pmCtor = new ProfilerMarker("BindingInfoNcalc.ctor");

	// Token: 0x040066BC RID: 26300
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly ProfilerMarker pmRefreshValueComplete = new ProfilerMarker("BindingInfoNcalc.RefreshValue");

	// Token: 0x040066BD RID: 26301
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly ProfilerMarker pmRefreshValueCompleteSourceText;

	// Token: 0x040066BE RID: 26302
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly ProfilerMarker pmRefreshBindings = new ProfilerMarker("Refresh Bindings");

	// Token: 0x040066BF RID: 26303
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly ProfilerMarker pmNcalcEval = new ProfilerMarker("NCalc.Evaluate");

	// Token: 0x040066C0 RID: 26304
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly ProfilerMarker pmApplyAttribute = new ProfilerMarker("Apply Attribute");

	// Token: 0x020010F7 RID: 4343
	public abstract class VariableStateAbs
	{
		// Token: 0x1700102B RID: 4139
		// (get) Token: 0x060089F4 RID: 35316
		public abstract object CurrentValue { get; }

		// Token: 0x060089F5 RID: 35317
		public abstract bool RefreshValue();

		// Token: 0x060089F6 RID: 35318 RVA: 0x0000640C File Offset: 0x0000460C
		[PublicizedFrom(EAccessModifier.Protected)]
		public VariableStateAbs()
		{
		}
	}

	// Token: 0x020010F8 RID: 4344
	public abstract class VariableStateBinding : BindingInfoNcalc.VariableStateAbs
	{
		// Token: 0x060089F7 RID: 35319 RVA: 0x0034C046 File Offset: 0x0034A246
		[PublicizedFrom(EAccessModifier.Protected)]
		public VariableStateBinding(string _name, XUiController _controller, Dictionary<string, object> _expressionParamDict)
		{
			this.BindingName = _name;
			this.Controller = _controller;
			this.ExpressionParamDict = _expressionParamDict;
		}

		// Token: 0x040066C2 RID: 26306
		[PublicizedFrom(EAccessModifier.Protected)]
		public readonly XUiController Controller;

		// Token: 0x040066C3 RID: 26307
		[PublicizedFrom(EAccessModifier.Protected)]
		public readonly string BindingName;

		// Token: 0x040066C4 RID: 26308
		[PublicizedFrom(EAccessModifier.Protected)]
		public readonly Dictionary<string, object> ExpressionParamDict;
	}

	// Token: 0x020010F9 RID: 4345
	public class VariableStateParamBinding : BindingInfoNcalc.VariableStateBinding
	{
		// Token: 0x060089F8 RID: 35320 RVA: 0x0034C063 File Offset: 0x0034A263
		public VariableStateParamBinding(string _name, XUiController _controller, object _initialValue, XuiBindingDelegate _bindingDelegate, Dictionary<string, object> _expressionParamDict)
			: base(_name, _controller, _expressionParamDict)
		{
			this.currentValue = _initialValue;
			this.bindingDelegate = _bindingDelegate;
			this.ExpressionParamDict[this.BindingName] = _initialValue;
		}

		// Token: 0x060089F9 RID: 35321 RVA: 0x0034C090 File Offset: 0x0034A290
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

		// Token: 0x060089FA RID: 35322 RVA: 0x0034C130 File Offset: 0x0034A330
		public override string ToString()
		{
			return string.Format("ParamBinding: {0}='{1}'", this.BindingName, this.currentValue);
		}

		// Token: 0x1700102C RID: 4140
		// (get) Token: 0x060089FB RID: 35323 RVA: 0x0034C148 File Offset: 0x0034A348
		public override object CurrentValue
		{
			get
			{
				return this.currentValue;
			}
		}

		// Token: 0x040066C5 RID: 26309
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly XuiBindingDelegate bindingDelegate;

		// Token: 0x040066C6 RID: 26310
		[PublicizedFrom(EAccessModifier.Private)]
		public object currentValue;
	}

	// Token: 0x020010FA RID: 4346
	public class VariableStateLegacyBinding : BindingInfoNcalc.VariableStateBinding
	{
		// Token: 0x060089FC RID: 35324 RVA: 0x0034C150 File Offset: 0x0034A350
		public VariableStateLegacyBinding(string _name, XUiController _controller, string _initialValue, Dictionary<string, object> _expressionParamDict)
			: base(_name, _controller, _expressionParamDict)
		{
			this.currentValue = _initialValue;
			this.ExpressionParamDict[this.BindingName] = _initialValue;
		}

		// Token: 0x060089FD RID: 35325 RVA: 0x0034C178 File Offset: 0x0034A378
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

		// Token: 0x060089FE RID: 35326 RVA: 0x0034C20C File Offset: 0x0034A40C
		public override string ToString()
		{
			return string.Concat(new string[] { "LegacyBinding: ", this.BindingName, "='", this.currentValue, "'" });
		}

		// Token: 0x1700102D RID: 4141
		// (get) Token: 0x060089FF RID: 35327 RVA: 0x0034C243 File Offset: 0x0034A443
		public override object CurrentValue
		{
			get
			{
				return this.currentValue;
			}
		}

		// Token: 0x040066C7 RID: 26311
		[PublicizedFrom(EAccessModifier.Private)]
		public string currentValue;
	}

	// Token: 0x020010FB RID: 4347
	public class VariableStateCVar : BindingInfoNcalc.VariableStateAbs
	{
		// Token: 0x06008A00 RID: 35328 RVA: 0x0034C24B File Offset: 0x0034A44B
		public VariableStateCVar(string _name)
		{
			this.cVarName = _name;
			this.currentValue = 0f;
		}

		// Token: 0x06008A01 RID: 35329 RVA: 0x0034C268 File Offset: 0x0034A468
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

		// Token: 0x06008A02 RID: 35330 RVA: 0x0034C2CA File Offset: 0x0034A4CA
		public override string ToString()
		{
			return string.Format("cVar: {0}={1}", this.cVarName, this.currentValue);
		}

		// Token: 0x1700102E RID: 4142
		// (get) Token: 0x06008A03 RID: 35331 RVA: 0x0034C2E7 File Offset: 0x0034A4E7
		public override object CurrentValue
		{
			get
			{
				return this.currentValue;
			}
		}

		// Token: 0x040066C8 RID: 26312
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string cVarName;

		// Token: 0x040066C9 RID: 26313
		[PublicizedFrom(EAccessModifier.Private)]
		public float currentValue;
	}

	// Token: 0x020010FC RID: 4348
	public abstract class VariableStateSimpleLookupAbs : BindingInfoNcalc.VariableStateAbs
	{
		// Token: 0x06008A04 RID: 35332
		[PublicizedFrom(EAccessModifier.Protected)]
		public abstract object getCurrentValue();

		// Token: 0x1700102F RID: 4143
		// (get) Token: 0x06008A05 RID: 35333
		public abstract string VarType
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get;
		}

		// Token: 0x17001030 RID: 4144
		// (get) Token: 0x06008A06 RID: 35334
		public abstract string VarName
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get;
		}

		// Token: 0x06008A07 RID: 35335 RVA: 0x0034C2F4 File Offset: 0x0034A4F4
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

		// Token: 0x06008A08 RID: 35336 RVA: 0x0034C35F File Offset: 0x0034A55F
		public override string ToString()
		{
			return string.Format("{0}: {1}={2}", this.VarType, this.VarName, this.currentValue);
		}

		// Token: 0x17001031 RID: 4145
		// (get) Token: 0x06008A09 RID: 35337 RVA: 0x0034C37D File Offset: 0x0034A57D
		public override object CurrentValue
		{
			get
			{
				return this.currentValue;
			}
		}

		// Token: 0x06008A0A RID: 35338 RVA: 0x0034C385 File Offset: 0x0034A585
		[PublicizedFrom(EAccessModifier.Protected)]
		public VariableStateSimpleLookupAbs()
		{
		}

		// Token: 0x040066CA RID: 26314
		[PublicizedFrom(EAccessModifier.Protected)]
		public object currentValue;
	}

	// Token: 0x020010FD RID: 4349
	public class VariableStateGamePref : BindingInfoNcalc.VariableStateSimpleLookupAbs
	{
		// Token: 0x06008A0B RID: 35339 RVA: 0x0034C38D File Offset: 0x0034A58D
		public VariableStateGamePref(EnumGamePrefs _pref)
		{
			this.pref = _pref;
			this.currentValue = this.getCurrentValue();
		}

		// Token: 0x06008A0C RID: 35340 RVA: 0x0034C3A8 File Offset: 0x0034A5A8
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object getCurrentValue()
		{
			return GamePrefs.GetObject(this.pref);
		}

		// Token: 0x17001032 RID: 4146
		// (get) Token: 0x06008A0D RID: 35341 RVA: 0x0034C3B5 File Offset: 0x0034A5B5
		public override string VarType
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return "GamePref";
			}
		}

		// Token: 0x17001033 RID: 4147
		// (get) Token: 0x06008A0E RID: 35342 RVA: 0x0034C3BC File Offset: 0x0034A5BC
		public override string VarName
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return this.pref.ToStringCached<EnumGamePrefs>();
			}
		}

		// Token: 0x040066CB RID: 26315
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly EnumGamePrefs pref;
	}

	// Token: 0x020010FE RID: 4350
	public class VariableStateGameStat : BindingInfoNcalc.VariableStateSimpleLookupAbs
	{
		// Token: 0x06008A0F RID: 35343 RVA: 0x0034C3C9 File Offset: 0x0034A5C9
		public VariableStateGameStat(EnumGameStats _stat)
		{
			this.stat = _stat;
			this.currentValue = this.getCurrentValue();
		}

		// Token: 0x06008A10 RID: 35344 RVA: 0x0034C3E4 File Offset: 0x0034A5E4
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object getCurrentValue()
		{
			return GameStats.GetObject(this.stat);
		}

		// Token: 0x17001034 RID: 4148
		// (get) Token: 0x06008A11 RID: 35345 RVA: 0x0034C3F1 File Offset: 0x0034A5F1
		public override string VarType
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return "GameStat";
			}
		}

		// Token: 0x17001035 RID: 4149
		// (get) Token: 0x06008A12 RID: 35346 RVA: 0x0034C3F8 File Offset: 0x0034A5F8
		public override string VarName
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return this.stat.ToStringCached<EnumGameStats>();
			}
		}

		// Token: 0x040066CC RID: 26316
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly EnumGameStats stat;
	}

	// Token: 0x020010FF RID: 4351
	public class VariableStateGameInfoInt : BindingInfoNcalc.VariableStateSimpleLookupAbs
	{
		// Token: 0x06008A13 RID: 35347 RVA: 0x0034C405 File Offset: 0x0034A605
		public VariableStateGameInfoInt(GameInfoInt _name)
		{
			this.name = _name;
			this.currentValue = this.getCurrentValue();
		}

		// Token: 0x06008A14 RID: 35348 RVA: 0x0034C420 File Offset: 0x0034A620
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object getCurrentValue()
		{
			return (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer ? SingletonMonoBehaviour<ConnectionManager>.Instance.LocalServerInfo : SingletonMonoBehaviour<ConnectionManager>.Instance.LastGameServerInfo).GetValue(this.name);
		}

		// Token: 0x17001036 RID: 4150
		// (get) Token: 0x06008A15 RID: 35349 RVA: 0x0034C454 File Offset: 0x0034A654
		public override string VarType
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return "GameInfoInt";
			}
		}

		// Token: 0x17001037 RID: 4151
		// (get) Token: 0x06008A16 RID: 35350 RVA: 0x0034C45B File Offset: 0x0034A65B
		public override string VarName
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return this.name.ToStringCached<GameInfoInt>();
			}
		}

		// Token: 0x040066CD RID: 26317
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly GameInfoInt name;
	}

	// Token: 0x02001100 RID: 4352
	public class VariableStateGameInfoBool : BindingInfoNcalc.VariableStateSimpleLookupAbs
	{
		// Token: 0x06008A17 RID: 35351 RVA: 0x0034C468 File Offset: 0x0034A668
		public VariableStateGameInfoBool(GameInfoBool _name)
		{
			this.name = _name;
			this.currentValue = this.getCurrentValue();
		}

		// Token: 0x06008A18 RID: 35352 RVA: 0x0034C483 File Offset: 0x0034A683
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object getCurrentValue()
		{
			return (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer ? SingletonMonoBehaviour<ConnectionManager>.Instance.LocalServerInfo : SingletonMonoBehaviour<ConnectionManager>.Instance.LastGameServerInfo).GetValue(this.name);
		}

		// Token: 0x17001038 RID: 4152
		// (get) Token: 0x06008A19 RID: 35353 RVA: 0x0034C4B7 File Offset: 0x0034A6B7
		public override string VarType
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return "GameInfoBool";
			}
		}

		// Token: 0x17001039 RID: 4153
		// (get) Token: 0x06008A1A RID: 35354 RVA: 0x0034C4BE File Offset: 0x0034A6BE
		public override string VarName
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return this.name.ToStringCached<GameInfoBool>();
			}
		}

		// Token: 0x040066CE RID: 26318
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly GameInfoBool name;
	}

	// Token: 0x02001101 RID: 4353
	public class VariableStateGameInfoString : BindingInfoNcalc.VariableStateSimpleLookupAbs
	{
		// Token: 0x06008A1B RID: 35355 RVA: 0x0034C4CB File Offset: 0x0034A6CB
		public VariableStateGameInfoString(GameInfoString _name)
		{
			this.name = _name;
			this.currentValue = this.getCurrentValue();
		}

		// Token: 0x06008A1C RID: 35356 RVA: 0x0034C4E6 File Offset: 0x0034A6E6
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object getCurrentValue()
		{
			return (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer ? SingletonMonoBehaviour<ConnectionManager>.Instance.LocalServerInfo : SingletonMonoBehaviour<ConnectionManager>.Instance.LastGameServerInfo).GetValue(this.name);
		}

		// Token: 0x1700103A RID: 4154
		// (get) Token: 0x06008A1D RID: 35357 RVA: 0x0034C515 File Offset: 0x0034A715
		public override string VarType
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return "GameInfoString";
			}
		}

		// Token: 0x1700103B RID: 4155
		// (get) Token: 0x06008A1E RID: 35358 RVA: 0x0034C51C File Offset: 0x0034A71C
		public override string VarName
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return this.name.ToStringCached<GameInfoString>();
			}
		}

		// Token: 0x040066CF RID: 26319
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly GameInfoString name;
	}
}
