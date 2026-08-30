using System;
using System.Collections.Generic;
using NCalc;
using Unity.Profiling;

// Token: 0x020010DC RID: 4316
public class BindingItemNcalc : BindingItem, IBindingNcalc
{
	// Token: 0x060088E4 RID: 35044 RVA: 0x00343FBC File Offset: 0x003421BC
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

	// Token: 0x060088E5 RID: 35045 RVA: 0x003440EC File Offset: 0x003422EC
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

	// Token: 0x060088E6 RID: 35046 RVA: 0x003441B8 File Offset: 0x003423B8
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

	// Token: 0x060088E7 RID: 35047 RVA: 0x003442CC File Offset: 0x003424CC
	[PublicizedFrom(EAccessModifier.Private)]
	public void nCalcEvaluateParameter(string _name, ParameterArgs _args)
	{
		object obj;
		if (this.FindParameter(_name, out obj))
		{
			_args.Result = obj;
		}
	}

	// Token: 0x060088E8 RID: 35048 RVA: 0x003442EC File Offset: 0x003424EC
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

	// Token: 0x060088E9 RID: 35049 RVA: 0x003436C7 File Offset: 0x003418C7
	[PublicizedFrom(EAccessModifier.Private)]
	public void nCalcEvaluateFunction(string _name, FunctionArgs _args, bool _ignoreCase)
	{
		BindingNcalcFunctions.EvaluateFunc(this, _name, _args);
	}

	// Token: 0x17000FCE RID: 4046
	// (get) Token: 0x060088EA RID: 35050 RVA: 0x003443FC File Offset: 0x003425FC
	public new string SourceText
	{
		get
		{
			return this.SourceText;
		}
	}

	// Token: 0x17000FCF RID: 4047
	// (get) Token: 0x060088EB RID: 35051 RVA: 0x00344404 File Offset: 0x00342604
	public IXUiElement TargetElement
	{
		get
		{
			return this.Parent.View;
		}
	}

	// Token: 0x17000FD0 RID: 4048
	// (get) Token: 0x060088EC RID: 35052 RVA: 0x00344411 File Offset: 0x00342611
	public bool IsInitializing { get; }

	// Token: 0x060088ED RID: 35053 RVA: 0x00344419 File Offset: 0x00342619
	public void SetIndeterministic()
	{
		this.usesIndeterministicFunctions = true;
	}

	// Token: 0x060088EE RID: 35054 RVA: 0x00010E62 File Offset: 0x0000F062
	public bool RegisterVariable(BindingInfoNcalc.VariableStateAbs _var)
	{
		return false;
	}

	// Token: 0x040065C8 RID: 26056
	public readonly BindingInfo Parent;

	// Token: 0x040065C9 RID: 26057
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<string, BindingItemNcalc.BindingState> bindings = new Dictionary<string, BindingItemNcalc.BindingState>(StringComparer.Ordinal);

	// Token: 0x040065CA RID: 26058
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<string, object> expressionParamDict = new Dictionary<string, object>(StringComparer.Ordinal);

	// Token: 0x040065CB RID: 26059
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Expression expression;

	// Token: 0x040065CC RID: 26060
	[PublicizedFrom(EAccessModifier.Private)]
	public bool usesIndeterministicFunctions;

	// Token: 0x040065CD RID: 26061
	[PublicizedFrom(EAccessModifier.Private)]
	public string currentValue;

	// Token: 0x040065CE RID: 26062
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly ProfilerMarker pmNcalcEval = new ProfilerMarker("NCalc.Evaluate");

	// Token: 0x020010DD RID: 4317
	[PublicizedFrom(EAccessModifier.Private)]
	public class BindingState
	{
		// Token: 0x060088EF RID: 35055 RVA: 0x00344422 File Offset: 0x00342622
		public BindingState(string _bindingName, XUiController _controller, string _initialValue, XuiBindingDelegate _bindingDelegate)
		{
			this.bindingName = _bindingName;
			this.controller = _controller;
			this.CurrentValue = _initialValue;
			this.bindingDelegate = _bindingDelegate;
		}

		// Token: 0x060088F0 RID: 35056 RVA: 0x00344448 File Offset: 0x00342648
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

		// Token: 0x060088F1 RID: 35057 RVA: 0x0034451C File Offset: 0x0034271C
		public override string ToString()
		{
			return "Binding:" + this.bindingName + "=" + this.CurrentValue;
		}

		// Token: 0x040065D0 RID: 26064
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string bindingName;

		// Token: 0x040065D1 RID: 26065
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly XUiController controller;

		// Token: 0x040065D2 RID: 26066
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly XuiBindingDelegate bindingDelegate;

		// Token: 0x040065D3 RID: 26067
		public string CurrentValue;
	}
}
