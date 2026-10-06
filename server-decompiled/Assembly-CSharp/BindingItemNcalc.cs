using System;
using System.Collections.Generic;
using NCalc;
using Unity.Profiling;

// Token: 0x02001106 RID: 4358
public class BindingItemNcalc : BindingItem, IBindingNcalc
{
	// Token: 0x06008A28 RID: 35368 RVA: 0x0034C808 File Offset: 0x0034AA08
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

	// Token: 0x06008A29 RID: 35369 RVA: 0x0034C938 File Offset: 0x0034AB38
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

	// Token: 0x06008A2A RID: 35370 RVA: 0x0034CA04 File Offset: 0x0034AC04
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

	// Token: 0x06008A2B RID: 35371 RVA: 0x0034CB18 File Offset: 0x0034AD18
	[PublicizedFrom(EAccessModifier.Private)]
	public void nCalcEvaluateParameter(string _name, ParameterArgs _args)
	{
		object obj;
		if (this.FindParameter(_name, out obj))
		{
			_args.Result = obj;
		}
	}

	// Token: 0x06008A2C RID: 35372 RVA: 0x0034CB38 File Offset: 0x0034AD38
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

	// Token: 0x06008A2D RID: 35373 RVA: 0x0034BE7B File Offset: 0x0034A07B
	[PublicizedFrom(EAccessModifier.Private)]
	public void nCalcEvaluateFunction(string _name, FunctionArgs _args, bool _ignoreCase)
	{
		BindingNcalcFunctions.EvaluateFunc(this, _name, _args);
	}

	// Token: 0x1700103C RID: 4156
	// (get) Token: 0x06008A2E RID: 35374 RVA: 0x0034CC48 File Offset: 0x0034AE48
	public new string SourceText
	{
		get
		{
			return this.SourceText;
		}
	}

	// Token: 0x1700103D RID: 4157
	// (get) Token: 0x06008A2F RID: 35375 RVA: 0x0034CC50 File Offset: 0x0034AE50
	public IXUiElement TargetElement
	{
		get
		{
			return this.Parent.View;
		}
	}

	// Token: 0x1700103E RID: 4158
	// (get) Token: 0x06008A30 RID: 35376 RVA: 0x0034CC5D File Offset: 0x0034AE5D
	public bool IsInitializing { get; }

	// Token: 0x06008A31 RID: 35377 RVA: 0x0034CC65 File Offset: 0x0034AE65
	public void SetIndeterministic()
	{
		this.usesIndeterministicFunctions = true;
	}

	// Token: 0x06008A32 RID: 35378 RVA: 0x00010E62 File Offset: 0x0000F062
	public bool RegisterVariable(BindingInfoNcalc.VariableStateAbs _var)
	{
		return false;
	}

	// Token: 0x040066D8 RID: 26328
	public readonly BindingInfo Parent;

	// Token: 0x040066D9 RID: 26329
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<string, BindingItemNcalc.BindingState> bindings = new Dictionary<string, BindingItemNcalc.BindingState>(StringComparer.Ordinal);

	// Token: 0x040066DA RID: 26330
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<string, object> expressionParamDict = new Dictionary<string, object>(StringComparer.Ordinal);

	// Token: 0x040066DB RID: 26331
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Expression expression;

	// Token: 0x040066DC RID: 26332
	[PublicizedFrom(EAccessModifier.Private)]
	public bool usesIndeterministicFunctions;

	// Token: 0x040066DD RID: 26333
	[PublicizedFrom(EAccessModifier.Private)]
	public string currentValue;

	// Token: 0x040066DE RID: 26334
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly ProfilerMarker pmNcalcEval = new ProfilerMarker("NCalc.Evaluate");

	// Token: 0x02001107 RID: 4359
	[PublicizedFrom(EAccessModifier.Private)]
	public class BindingState
	{
		// Token: 0x06008A33 RID: 35379 RVA: 0x0034CC6E File Offset: 0x0034AE6E
		public BindingState(string _bindingName, XUiController _controller, string _initialValue, XuiBindingDelegate _bindingDelegate)
		{
			this.bindingName = _bindingName;
			this.controller = _controller;
			this.CurrentValue = _initialValue;
			this.bindingDelegate = _bindingDelegate;
		}

		// Token: 0x06008A34 RID: 35380 RVA: 0x0034CC94 File Offset: 0x0034AE94
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

		// Token: 0x06008A35 RID: 35381 RVA: 0x0034CD68 File Offset: 0x0034AF68
		public override string ToString()
		{
			return "Binding:" + this.bindingName + "=" + this.CurrentValue;
		}

		// Token: 0x040066E0 RID: 26336
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string bindingName;

		// Token: 0x040066E1 RID: 26337
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly XUiController controller;

		// Token: 0x040066E2 RID: 26338
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly XuiBindingDelegate bindingDelegate;

		// Token: 0x040066E3 RID: 26339
		public string CurrentValue;
	}
}
