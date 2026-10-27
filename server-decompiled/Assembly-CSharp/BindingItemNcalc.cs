using System;
using System.Collections.Generic;
using NCalc;
using Unity.Profiling;

// Token: 0x02001116 RID: 4374
public class BindingItemNcalc : BindingItem, IBindingNcalc
{
	// Token: 0x06008A7C RID: 35452 RVA: 0x0034E8B8 File Offset: 0x0034CAB8
	public BindingItemNcalc(BindingInfo _parent, string _sourceText)
		: base(_sourceText)
	{
		if (this.fieldName[0] == '#')
		{
			this.fieldName = this.fieldName.Substring(1);
		}
		this.Parent = _parent;
		this.IsInitializing = true;
		this.expression = new Expression(this.fieldName, EvaluateOptions.IgnoreCase | EvaluateOptions.NoCache | EvaluateOptions.UseDoubleForAbsFunction | EvaluateOptions.ReuseInstances)
		{
			Parameters = this.expressionParamDict
		};
		this.expression.EvaluateFunction += this.nCalcEvaluateFunction;
		this.expression.EvaluateParameter += this.nCalcEvaluateParameter;
		this.currentValue = this.evaluateExpression();
		this.IsInitializing = false;
		if (this.usesIndeterministicFunctions || this.bindings.Count == 0)
		{
			for (XUiController xuiController = this.Parent.View.Controller; xuiController != null; xuiController = xuiController.Parent)
			{
				if (xuiController.GetType() != typeof(XUiController))
				{
					xuiController.Bindings.AddBinding(_parent);
					return;
				}
			}
		}
	}

	// Token: 0x06008A7D RID: 35453 RVA: 0x0034E9E8 File Offset: 0x0034CBE8
	public override string GetValue()
	{
		bool flag = this.usesIndeterministicFunctions;
		foreach (KeyValuePair<string, BindingItemNcalc.BindingState> keyValuePair in this.bindings)
		{
			if (keyValuePair.Value.RefreshValue())
			{
				this.expressionParamDict[keyValuePair.Key] = keyValuePair.Value.CurrentValue;
				flag = true;
			}
		}
		if (!flag)
		{
			return this.currentValue;
		}
		using (this.pmNcalcEval.Auto())
		{
			this.currentValue = this.evaluateExpression();
		}
		return this.currentValue;
	}

	// Token: 0x06008A7E RID: 35454 RVA: 0x0034EAB4 File Offset: 0x0034CCB4
	[PublicizedFrom(EAccessModifier.Private)]
	public string evaluateExpression()
	{
		string text = null;
		try
		{
			object obj = this.expression.Evaluate();
			string text2;
			if (obj != null)
			{
				if (obj is decimal)
				{
					decimal num = (decimal)obj;
					text2 = num.ToCultureInvariantString("0.########");
				}
				else if (obj is float)
				{
					float num2 = (float)obj;
					text2 = num2.ToCultureInvariantString();
				}
				else if (obj is double)
				{
					double num3 = (double)obj;
					text2 = num3.ToCultureInvariantString();
				}
				else
				{
					text2 = obj.ToString();
				}
			}
			else
			{
				text2 = "";
			}
			text = text2;
		}
		catch (ArgumentException ex)
		{
			Log.Error("[XUi] Binding expression can not be evaluated: " + this.SourceText + " --- hierarchy: " + this.Parent.View.GetXuiHierarchy());
			Log.Exception(ex);
		}
		catch (Exception ex2)
		{
			Log.Error("[XUi] Binding expression can not be evaluated: " + this.SourceText + " --- hierarchy: " + this.Parent.View.GetXuiHierarchy());
			Log.Exception(ex2);
			text = "";
		}
		return text;
	}

	// Token: 0x06008A7F RID: 35455 RVA: 0x0034EBC8 File Offset: 0x0034CDC8
	[PublicizedFrom(EAccessModifier.Private)]
	public void nCalcEvaluateParameter(string _name, ParameterArgs _args)
	{
		object obj;
		if (this.FindParameter(_name, out obj))
		{
			_args.Result = obj;
		}
	}

	// Token: 0x06008A80 RID: 35456 RVA: 0x0034EBE8 File Offset: 0x0034CDE8
	public bool FindParameter(string _name, out object _value)
	{
		BindingItemNcalc.BindingState bindingState;
		if (this.bindings.TryGetValue(_name, out bindingState))
		{
			_value = bindingState.CurrentValue;
			return true;
		}
		for (XUiController xuiController = this.Parent.View.Controller; xuiController != null; xuiController = xuiController.Parent)
		{
			if (xuiController.GetType() != typeof(XUiController))
			{
				string text = "";
				if (xuiController.GetBindingValue(ref text, _name))
				{
					xuiController.Bindings.AddBinding(this.Parent);
					bindingState = new BindingItemNcalc.BindingState(_name, xuiController, text, null);
					this.bindings[_name] = bindingState;
					this.expressionParamDict[_name] = text;
					_value = text;
					return true;
				}
				XuiBindingDelegate xuiBindingDelegate;
				if (BindingMethodCache.Instance.TryGetBindingDelegate(xuiController, _name, out xuiBindingDelegate))
				{
					xuiController.Bindings.AddBinding(this.Parent);
					text = xuiBindingDelegate(xuiController).ToString();
					bindingState = new BindingItemNcalc.BindingState(_name, xuiController, text, xuiBindingDelegate);
					this.bindings[_name] = bindingState;
					this.expressionParamDict[_name] = text;
					_value = xuiBindingDelegate(xuiController);
					return true;
				}
			}
		}
		_value = null;
		return false;
	}

	// Token: 0x06008A81 RID: 35457 RVA: 0x0034DF2B File Offset: 0x0034C12B
	[PublicizedFrom(EAccessModifier.Private)]
	public void nCalcEvaluateFunction(string _name, FunctionArgs _args, bool _ignoreCase)
	{
		BindingNcalcFunctions.EvaluateFunc(this, _name, _args);
	}

	// Token: 0x17001047 RID: 4167
	// (get) Token: 0x06008A82 RID: 35458 RVA: 0x0034ECF8 File Offset: 0x0034CEF8
	public new string SourceText
	{
		get
		{
			return this.SourceText;
		}
	}

	// Token: 0x17001048 RID: 4168
	// (get) Token: 0x06008A83 RID: 35459 RVA: 0x0034ED00 File Offset: 0x0034CF00
	public IXUiElement TargetElement
	{
		get
		{
			return this.Parent.View;
		}
	}

	// Token: 0x17001049 RID: 4169
	// (get) Token: 0x06008A84 RID: 35460 RVA: 0x0034ED0D File Offset: 0x0034CF0D
	public bool IsInitializing { get; }

	// Token: 0x06008A85 RID: 35461 RVA: 0x0034ED15 File Offset: 0x0034CF15
	public void SetIndeterministic()
	{
		this.usesIndeterministicFunctions = true;
	}

	// Token: 0x06008A86 RID: 35462 RVA: 0x00010E62 File Offset: 0x0000F062
	public bool RegisterVariable(BindingInfoNcalc.VariableStateAbs _var)
	{
		return false;
	}

	// Token: 0x04006718 RID: 26392
	public readonly BindingInfo Parent;

	// Token: 0x04006719 RID: 26393
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<string, BindingItemNcalc.BindingState> bindings = new Dictionary<string, BindingItemNcalc.BindingState>(StringComparer.Ordinal);

	// Token: 0x0400671A RID: 26394
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<string, object> expressionParamDict = new Dictionary<string, object>(StringComparer.Ordinal);

	// Token: 0x0400671B RID: 26395
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Expression expression;

	// Token: 0x0400671C RID: 26396
	[PublicizedFrom(EAccessModifier.Private)]
	public bool usesIndeterministicFunctions;

	// Token: 0x0400671D RID: 26397
	[PublicizedFrom(EAccessModifier.Private)]
	public string currentValue;

	// Token: 0x0400671E RID: 26398
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly ProfilerMarker pmNcalcEval = new ProfilerMarker("NCalc.Evaluate");

	// Token: 0x02001117 RID: 4375
	[PublicizedFrom(EAccessModifier.Private)]
	public class BindingState
	{
		// Token: 0x06008A87 RID: 35463 RVA: 0x0034ED1E File Offset: 0x0034CF1E
		public BindingState(string _bindingName, XUiController _controller, string _initialValue, XuiBindingDelegate _bindingDelegate)
		{
			this.bindingName = _bindingName;
			this.controller = _controller;
			this.CurrentValue = _initialValue;
			this.bindingDelegate = _bindingDelegate;
		}

		// Token: 0x06008A88 RID: 35464 RVA: 0x0034ED44 File Offset: 0x0034CF44
		public bool RefreshValue()
		{
			string text = "";
			if (this.bindingDelegate == null)
			{
				if (!this.controller.GetBindingValue(ref text, this.bindingName))
				{
					Log.Error(string.Concat(new string[]
					{
						"[XUi] Refreshing binding failed: Controller's GetBindingValue no longer returns true! (Binding: ",
						this.bindingName,
						", hierarchy: ",
						this.controller.GetXuiHierarchy(),
						")"
					}));
					return false;
				}
			}
			else
			{
				object obj = this.bindingDelegate(this.controller);
				if (obj == null)
				{
					Log.Warning("[XUi] Binding '" + this.bindingName + "' returned null, should always return appropriate non-null value of same data type. Hierarchy: " + this.controller.GetXuiHierarchy());
					obj = "";
				}
				text = obj.ToString();
			}
			if (string.Equals(this.CurrentValue, text, StringComparison.Ordinal))
			{
				return false;
			}
			this.CurrentValue = text;
			return true;
		}

		// Token: 0x06008A89 RID: 35465 RVA: 0x0034EE18 File Offset: 0x0034D018
		public override string ToString()
		{
			return "Binding:" + this.bindingName + "=" + this.CurrentValue;
		}

		// Token: 0x04006720 RID: 26400
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string bindingName;

		// Token: 0x04006721 RID: 26401
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly XUiController controller;

		// Token: 0x04006722 RID: 26402
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly XuiBindingDelegate bindingDelegate;

		// Token: 0x04006723 RID: 26403
		public string CurrentValue;
	}
}
