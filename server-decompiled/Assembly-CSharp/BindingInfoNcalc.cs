using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using NCalc;
using Unity.Profiling;

// Token: 0x02001106 RID: 4358
public class BindingInfoNcalc : IBindingInstance, IBindingNcalc
{
	// Token: 0x06008A39 RID: 35385 RVA: 0x0034D7A0 File Offset: 0x0034B9A0
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

	// Token: 0x06008A3A RID: 35386 RVA: 0x0034D800 File Offset: 0x0034BA00
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

	// Token: 0x06008A3B RID: 35387 RVA: 0x0034DBAC File Offset: 0x0034BDAC
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

	// Token: 0x06008A3C RID: 35388 RVA: 0x0034DE90 File Offset: 0x0034C090
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

	// Token: 0x06008A3D RID: 35389 RVA: 0x0034DF0C File Offset: 0x0034C10C
	[PublicizedFrom(EAccessModifier.Private)]
	public void nCalcEvaluateParameter(string _name, ParameterArgs _args)
	{
		object obj;
		if (this.FindParameter(_name, out obj))
		{
			_args.Result = obj;
		}
	}

	// Token: 0x06008A3E RID: 35390 RVA: 0x0034DF2B File Offset: 0x0034C12B
	[PublicizedFrom(EAccessModifier.Private)]
	public void nCalcEvaluateFunction(string _name, FunctionArgs _args, bool _ignoreCase)
	{
		BindingNcalcFunctions.EvaluateFunc(this, _name, _args);
	}

	// Token: 0x06008A3F RID: 35391 RVA: 0x0034DF38 File Offset: 0x0034C138
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

	// Token: 0x06008A40 RID: 35392 RVA: 0x0034E033 File Offset: 0x0034C233
	[PublicizedFrom(EAccessModifier.Private)]
	public string xuiHierarchy()
	{
		return this.targetViewOrTween.GetXuiHierarchy();
	}

	// Token: 0x17001033 RID: 4147
	// (get) Token: 0x06008A41 RID: 35393 RVA: 0x0034E040 File Offset: 0x0034C240
	public string SourceText
	{
		get
		{
			return this.sourceString;
		}
	}

	// Token: 0x17001034 RID: 4148
	// (get) Token: 0x06008A42 RID: 35394 RVA: 0x0034E048 File Offset: 0x0034C248
	public IXUiElement TargetElement
	{
		get
		{
			return this.targetViewOrTween;
		}
	}

	// Token: 0x17001035 RID: 4149
	// (get) Token: 0x06008A43 RID: 35395 RVA: 0x0034E050 File Offset: 0x0034C250
	public bool IsInitializing { get; }

	// Token: 0x06008A44 RID: 35396 RVA: 0x0034E058 File Offset: 0x0034C258
	public void SetIndeterministic()
	{
		this.usesIndeterministicFunctions = true;
	}

	// Token: 0x06008A45 RID: 35397 RVA: 0x0034E061 File Offset: 0x0034C261
	public bool RegisterVariable(BindingInfoNcalc.VariableStateAbs _var)
	{
		this.variables.Add(_var);
		return true;
	}

	// Token: 0x06008A47 RID: 35399 RVA: 0x0034E09C File Offset: 0x0034C29C
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

	// Token: 0x040066ED RID: 26349
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly IXUiElement targetViewOrTween;

	// Token: 0x040066EE RID: 26350
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly string attributeName;

	// Token: 0x040066EF RID: 26351
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly string sourceString;

	// Token: 0x040066F0 RID: 26352
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly IXUiElement targetParsingElement;

	// Token: 0x040066F1 RID: 26353
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XuiParsingDelegate targetParsingDelegate;

	// Token: 0x040066F2 RID: 26354
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<BindingInfoNcalc.VariableStateAbs> variables = new List<BindingInfoNcalc.VariableStateAbs>();

	// Token: 0x040066F3 RID: 26355
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<string, object> expressionParamDict = new Dictionary<string, object>(StringComparer.Ordinal);

	// Token: 0x040066F4 RID: 26356
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Expression expression;

	// Token: 0x040066F5 RID: 26357
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly bool singleIdentifierBinding;

	// Token: 0x040066F6 RID: 26358
	[PublicizedFrom(EAccessModifier.Private)]
	public bool usesIndeterministicFunctions;

	// Token: 0x040066F7 RID: 26359
	[PublicizedFrom(EAccessModifier.Private)]
	public int boundToControllers;

	// Token: 0x040066F8 RID: 26360
	[PublicizedFrom(EAccessModifier.Private)]
	public object currentValue;

	// Token: 0x040066F9 RID: 26361
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Regex fullBinding = new Regex("^\\{%.*\\}$", RegexOptions.Compiled | RegexOptions.CultureInvariant);

	// Token: 0x040066FA RID: 26362
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Regex singleIdentifierExpression = new Regex("^\\s*([a-zA-Z]\\w*)\\s*$", RegexOptions.Compiled | RegexOptions.CultureInvariant);

	// Token: 0x040066FB RID: 26363
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly ProfilerMarker pmCtor = new ProfilerMarker("BindingInfoNcalc.ctor");

	// Token: 0x040066FC RID: 26364
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly ProfilerMarker pmRefreshValueComplete = new ProfilerMarker("BindingInfoNcalc.RefreshValue");

	// Token: 0x040066FD RID: 26365
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly ProfilerMarker pmRefreshValueCompleteSourceText;

	// Token: 0x040066FE RID: 26366
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly ProfilerMarker pmRefreshBindings = new ProfilerMarker("Refresh Bindings");

	// Token: 0x040066FF RID: 26367
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly ProfilerMarker pmNcalcEval = new ProfilerMarker("NCalc.Evaluate");

	// Token: 0x04006700 RID: 26368
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly ProfilerMarker pmApplyAttribute = new ProfilerMarker("Apply Attribute");

	// Token: 0x02001107 RID: 4359
	public abstract class VariableStateAbs
	{
		// Token: 0x17001036 RID: 4150
		// (get) Token: 0x06008A48 RID: 35400
		public abstract object CurrentValue { get; }

		// Token: 0x06008A49 RID: 35401
		public abstract bool RefreshValue();

		// Token: 0x06008A4A RID: 35402 RVA: 0x0000640C File Offset: 0x0000460C
		[PublicizedFrom(EAccessModifier.Protected)]
		public VariableStateAbs()
		{
		}
	}

	// Token: 0x02001108 RID: 4360
	public abstract class VariableStateBinding : BindingInfoNcalc.VariableStateAbs
	{
		// Token: 0x06008A4B RID: 35403 RVA: 0x0034E0F6 File Offset: 0x0034C2F6
		[PublicizedFrom(EAccessModifier.Protected)]
		public VariableStateBinding(string _name, XUiController _controller, Dictionary<string, object> _expressionParamDict)
		{
			this.BindingName = _name;
			this.Controller = _controller;
			this.ExpressionParamDict = _expressionParamDict;
		}

		// Token: 0x04006702 RID: 26370
		[PublicizedFrom(EAccessModifier.Protected)]
		public readonly XUiController Controller;

		// Token: 0x04006703 RID: 26371
		[PublicizedFrom(EAccessModifier.Protected)]
		public readonly string BindingName;

		// Token: 0x04006704 RID: 26372
		[PublicizedFrom(EAccessModifier.Protected)]
		public readonly Dictionary<string, object> ExpressionParamDict;
	}

	// Token: 0x02001109 RID: 4361
	public class VariableStateParamBinding : BindingInfoNcalc.VariableStateBinding
	{
		// Token: 0x06008A4C RID: 35404 RVA: 0x0034E113 File Offset: 0x0034C313
		public VariableStateParamBinding(string _name, XUiController _controller, object _initialValue, XuiBindingDelegate _bindingDelegate, Dictionary<string, object> _expressionParamDict)
			: base(_name, _controller, _expressionParamDict)
		{
			this.currentValue = _initialValue;
			this.bindingDelegate = _bindingDelegate;
			this.ExpressionParamDict[this.BindingName] = _initialValue;
		}

		// Token: 0x06008A4D RID: 35405 RVA: 0x0034E140 File Offset: 0x0034C340
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

		// Token: 0x06008A4E RID: 35406 RVA: 0x0034E1E0 File Offset: 0x0034C3E0
		public override string ToString()
		{
			return string.Format("ParamBinding: {0}='{1}'", this.BindingName, this.currentValue);
		}

		// Token: 0x17001037 RID: 4151
		// (get) Token: 0x06008A4F RID: 35407 RVA: 0x0034E1F8 File Offset: 0x0034C3F8
		public override object CurrentValue
		{
			get
			{
				return this.currentValue;
			}
		}

		// Token: 0x04006705 RID: 26373
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly XuiBindingDelegate bindingDelegate;

		// Token: 0x04006706 RID: 26374
		[PublicizedFrom(EAccessModifier.Private)]
		public object currentValue;
	}

	// Token: 0x0200110A RID: 4362
	public class VariableStateLegacyBinding : BindingInfoNcalc.VariableStateBinding
	{
		// Token: 0x06008A50 RID: 35408 RVA: 0x0034E200 File Offset: 0x0034C400
		public VariableStateLegacyBinding(string _name, XUiController _controller, string _initialValue, Dictionary<string, object> _expressionParamDict)
			: base(_name, _controller, _expressionParamDict)
		{
			this.currentValue = _initialValue;
			this.ExpressionParamDict[this.BindingName] = _initialValue;
		}

		// Token: 0x06008A51 RID: 35409 RVA: 0x0034E228 File Offset: 0x0034C428
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

		// Token: 0x06008A52 RID: 35410 RVA: 0x0034E2BC File Offset: 0x0034C4BC
		public override string ToString()
		{
			return string.Concat(new string[] { "LegacyBinding: ", this.BindingName, "='", this.currentValue, "'" });
		}

		// Token: 0x17001038 RID: 4152
		// (get) Token: 0x06008A53 RID: 35411 RVA: 0x0034E2F3 File Offset: 0x0034C4F3
		public override object CurrentValue
		{
			get
			{
				return this.currentValue;
			}
		}

		// Token: 0x04006707 RID: 26375
		[PublicizedFrom(EAccessModifier.Private)]
		public string currentValue;
	}

	// Token: 0x0200110B RID: 4363
	public class VariableStateCVar : BindingInfoNcalc.VariableStateAbs
	{
		// Token: 0x06008A54 RID: 35412 RVA: 0x0034E2FB File Offset: 0x0034C4FB
		public VariableStateCVar(string _name)
		{
			this.cVarName = _name;
			this.currentValue = 0f;
		}

		// Token: 0x06008A55 RID: 35413 RVA: 0x0034E318 File Offset: 0x0034C518
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

		// Token: 0x06008A56 RID: 35414 RVA: 0x0034E37A File Offset: 0x0034C57A
		public override string ToString()
		{
			return string.Format("cVar: {0}={1}", this.cVarName, this.currentValue);
		}

		// Token: 0x17001039 RID: 4153
		// (get) Token: 0x06008A57 RID: 35415 RVA: 0x0034E397 File Offset: 0x0034C597
		public override object CurrentValue
		{
			get
			{
				return this.currentValue;
			}
		}

		// Token: 0x04006708 RID: 26376
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string cVarName;

		// Token: 0x04006709 RID: 26377
		[PublicizedFrom(EAccessModifier.Private)]
		public float currentValue;
	}

	// Token: 0x0200110C RID: 4364
	public abstract class VariableStateSimpleLookupAbs : BindingInfoNcalc.VariableStateAbs
	{
		// Token: 0x06008A58 RID: 35416
		[PublicizedFrom(EAccessModifier.Protected)]
		public abstract object getCurrentValue();

		// Token: 0x1700103A RID: 4154
		// (get) Token: 0x06008A59 RID: 35417
		public abstract string VarType
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get;
		}

		// Token: 0x1700103B RID: 4155
		// (get) Token: 0x06008A5A RID: 35418
		public abstract string VarName
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get;
		}

		// Token: 0x06008A5B RID: 35419 RVA: 0x0034E3A4 File Offset: 0x0034C5A4
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

		// Token: 0x06008A5C RID: 35420 RVA: 0x0034E40F File Offset: 0x0034C60F
		public override string ToString()
		{
			return string.Format("{0}: {1}={2}", this.VarType, this.VarName, this.currentValue);
		}

		// Token: 0x1700103C RID: 4156
		// (get) Token: 0x06008A5D RID: 35421 RVA: 0x0034E42D File Offset: 0x0034C62D
		public override object CurrentValue
		{
			get
			{
				return this.currentValue;
			}
		}

		// Token: 0x06008A5E RID: 35422 RVA: 0x0034E435 File Offset: 0x0034C635
		[PublicizedFrom(EAccessModifier.Protected)]
		public VariableStateSimpleLookupAbs()
		{
		}

		// Token: 0x0400670A RID: 26378
		[PublicizedFrom(EAccessModifier.Protected)]
		public object currentValue;
	}

	// Token: 0x0200110D RID: 4365
	public class VariableStateGamePref : BindingInfoNcalc.VariableStateSimpleLookupAbs
	{
		// Token: 0x06008A5F RID: 35423 RVA: 0x0034E43D File Offset: 0x0034C63D
		public VariableStateGamePref(EnumGamePrefs _pref)
		{
			this.pref = _pref;
			this.currentValue = this.getCurrentValue();
		}

		// Token: 0x06008A60 RID: 35424 RVA: 0x0034E458 File Offset: 0x0034C658
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object getCurrentValue()
		{
			return GamePrefs.GetObject(this.pref);
		}

		// Token: 0x1700103D RID: 4157
		// (get) Token: 0x06008A61 RID: 35425 RVA: 0x0034E465 File Offset: 0x0034C665
		public override string VarType
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return "GamePref";
			}
		}

		// Token: 0x1700103E RID: 4158
		// (get) Token: 0x06008A62 RID: 35426 RVA: 0x0034E46C File Offset: 0x0034C66C
		public override string VarName
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return this.pref.ToStringCached<EnumGamePrefs>();
			}
		}

		// Token: 0x0400670B RID: 26379
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly EnumGamePrefs pref;
	}

	// Token: 0x0200110E RID: 4366
	public class VariableStateGameStat : BindingInfoNcalc.VariableStateSimpleLookupAbs
	{
		// Token: 0x06008A63 RID: 35427 RVA: 0x0034E479 File Offset: 0x0034C679
		public VariableStateGameStat(EnumGameStats _stat)
		{
			this.stat = _stat;
			this.currentValue = this.getCurrentValue();
		}

		// Token: 0x06008A64 RID: 35428 RVA: 0x0034E494 File Offset: 0x0034C694
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object getCurrentValue()
		{
			return GameStats.GetObject(this.stat);
		}

		// Token: 0x1700103F RID: 4159
		// (get) Token: 0x06008A65 RID: 35429 RVA: 0x0034E4A1 File Offset: 0x0034C6A1
		public override string VarType
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return "GameStat";
			}
		}

		// Token: 0x17001040 RID: 4160
		// (get) Token: 0x06008A66 RID: 35430 RVA: 0x0034E4A8 File Offset: 0x0034C6A8
		public override string VarName
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return this.stat.ToStringCached<EnumGameStats>();
			}
		}

		// Token: 0x0400670C RID: 26380
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly EnumGameStats stat;
	}

	// Token: 0x0200110F RID: 4367
	public class VariableStateGameInfoInt : BindingInfoNcalc.VariableStateSimpleLookupAbs
	{
		// Token: 0x06008A67 RID: 35431 RVA: 0x0034E4B5 File Offset: 0x0034C6B5
		public VariableStateGameInfoInt(GameInfoInt _name)
		{
			this.name = _name;
			this.currentValue = this.getCurrentValue();
		}

		// Token: 0x06008A68 RID: 35432 RVA: 0x0034E4D0 File Offset: 0x0034C6D0
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object getCurrentValue()
		{
			return (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer ? SingletonMonoBehaviour<ConnectionManager>.Instance.LocalServerInfo : SingletonMonoBehaviour<ConnectionManager>.Instance.LastGameServerInfo).GetValue(this.name);
		}

		// Token: 0x17001041 RID: 4161
		// (get) Token: 0x06008A69 RID: 35433 RVA: 0x0034E504 File Offset: 0x0034C704
		public override string VarType
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return "GameInfoInt";
			}
		}

		// Token: 0x17001042 RID: 4162
		// (get) Token: 0x06008A6A RID: 35434 RVA: 0x0034E50B File Offset: 0x0034C70B
		public override string VarName
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return this.name.ToStringCached<GameInfoInt>();
			}
		}

		// Token: 0x0400670D RID: 26381
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly GameInfoInt name;
	}

	// Token: 0x02001110 RID: 4368
	public class VariableStateGameInfoBool : BindingInfoNcalc.VariableStateSimpleLookupAbs
	{
		// Token: 0x06008A6B RID: 35435 RVA: 0x0034E518 File Offset: 0x0034C718
		public VariableStateGameInfoBool(GameInfoBool _name)
		{
			this.name = _name;
			this.currentValue = this.getCurrentValue();
		}

		// Token: 0x06008A6C RID: 35436 RVA: 0x0034E533 File Offset: 0x0034C733
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object getCurrentValue()
		{
			return (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer ? SingletonMonoBehaviour<ConnectionManager>.Instance.LocalServerInfo : SingletonMonoBehaviour<ConnectionManager>.Instance.LastGameServerInfo).GetValue(this.name);
		}

		// Token: 0x17001043 RID: 4163
		// (get) Token: 0x06008A6D RID: 35437 RVA: 0x0034E567 File Offset: 0x0034C767
		public override string VarType
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return "GameInfoBool";
			}
		}

		// Token: 0x17001044 RID: 4164
		// (get) Token: 0x06008A6E RID: 35438 RVA: 0x0034E56E File Offset: 0x0034C76E
		public override string VarName
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return this.name.ToStringCached<GameInfoBool>();
			}
		}

		// Token: 0x0400670E RID: 26382
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly GameInfoBool name;
	}

	// Token: 0x02001111 RID: 4369
	public class VariableStateGameInfoString : BindingInfoNcalc.VariableStateSimpleLookupAbs
	{
		// Token: 0x06008A6F RID: 35439 RVA: 0x0034E57B File Offset: 0x0034C77B
		public VariableStateGameInfoString(GameInfoString _name)
		{
			this.name = _name;
			this.currentValue = this.getCurrentValue();
		}

		// Token: 0x06008A70 RID: 35440 RVA: 0x0034E596 File Offset: 0x0034C796
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object getCurrentValue()
		{
			return (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer ? SingletonMonoBehaviour<ConnectionManager>.Instance.LocalServerInfo : SingletonMonoBehaviour<ConnectionManager>.Instance.LastGameServerInfo).GetValue(this.name);
		}

		// Token: 0x17001045 RID: 4165
		// (get) Token: 0x06008A71 RID: 35441 RVA: 0x0034E5C5 File Offset: 0x0034C7C5
		public override string VarType
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return "GameInfoString";
			}
		}

		// Token: 0x17001046 RID: 4166
		// (get) Token: 0x06008A72 RID: 35442 RVA: 0x0034E5CC File Offset: 0x0034C7CC
		public override string VarName
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return this.name.ToStringCached<GameInfoString>();
			}
		}

		// Token: 0x0400670F RID: 26383
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly GameInfoString name;
	}
}
