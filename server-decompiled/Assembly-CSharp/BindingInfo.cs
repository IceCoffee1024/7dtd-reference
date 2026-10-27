using System;
using System.Collections.Generic;
using Unity.Profiling;

// Token: 0x02001105 RID: 4357
public class BindingInfo : IBindingInstance
{
	// Token: 0x06008A35 RID: 35381 RVA: 0x0034CFA4 File Offset: 0x0034B1A4
	public BindingInfo(XUiView _view, string _attribute, string _sourceText)
	{
		this.View = _view;
		this.attributeName = _attribute;
		this.sourceText = _sourceText;
		this.pmRefreshValueCompleteSourceText = new ProfilerMarker(this.sourceText);
		int num2;
		for (int num = this.sourceText.IndexOf("{", StringComparison.Ordinal); num != -1; num = this.sourceText.IndexOf("{", num2, StringComparison.Ordinal))
		{
			num2 = this.sourceText.IndexOf("}", num, StringComparison.Ordinal);
			if (num2 == -1)
			{
				return;
			}
			string text = this.sourceText.Substring(num, num2 - num + 1);
			bool flag = false;
			for (int i = 0; i < this.bindingList.Count; i++)
			{
				if (this.bindingList[i].SourceText == text)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				BindingItem bindingItem;
				if (text.StartsWith("{cvar("))
				{
					bindingItem = new BindingItemCvar(this, text);
				}
				else if (text.StartsWith("{#"))
				{
					bindingItem = new BindingItemNcalc(this, text);
				}
				else
				{
					if (text.StartsWith("{%"))
					{
						throw new Exception(string.Concat(new string[]
						{
							"[XUi] Using new NCalc binding expression for partial attribute evaluation. Attribute: '",
							this.attributeName,
							"', full attribute value: '",
							this.sourceText,
							"', binding string: '",
							text,
							"', view hierarchy: ",
							this.View.GetXuiHierarchy(),
							":"
						}));
					}
					bindingItem = new BindingItemStandard(this, text);
				}
				this.bindingList.Add(bindingItem);
			}
		}
		this.singleTypedBinding = this.bindingList.Count == 1 && this.sourceText.Equals(this.bindingList[0].SourceText, StringComparison.Ordinal);
	}

	// Token: 0x06008A36 RID: 35382 RVA: 0x0034D1CC File Offset: 0x0034B3CC
	public void RefreshValue()
	{
		using (this.pmRefreshValueComplete.Auto())
		{
			using (this.pmRefreshValueCompleteSourceText.Auto())
			{
				if (!this.singleTypedBinding || !this.refreshValueSingleTyped())
				{
					bool flag = this.cachedResultValue == null;
					using (this.pmGetBindingValues.Auto())
					{
						for (int i = 0; i < this.bindingList.Count; i++)
						{
							string text = this.bindingList[i].GetValue() ?? "";
							if (i < this.cachedBindingValues.Count)
							{
								flag |= !string.Equals(this.cachedBindingValues[i], text, StringComparison.Ordinal);
								this.cachedBindingValues[i] = text;
							}
							else
							{
								flag = true;
								this.cachedBindingValues.Add(text);
							}
						}
					}
					using (this.pmBuildAttributeValue.Auto())
					{
						if (flag)
						{
							string text2 = this.sourceText;
							if (this.bindingList.Count == 1 && text2.Equals(this.bindingList[0].SourceText, StringComparison.Ordinal))
							{
								text2 = this.cachedBindingValues[0] ?? "";
							}
							else
							{
								for (int j = 0; j < this.bindingList.Count; j++)
								{
									BindingItem bindingItem = this.bindingList[j];
									text2 = text2.Replace(bindingItem.SourceText, this.cachedBindingValues[j]);
								}
							}
							this.cachedResultValue = text2;
						}
					}
					using (this.pmParseAttribute.Auto())
					{
						string text3 = this.cachedResultValue;
						if (text3.Contains("{cvar("))
						{
							text3 = BindingsManager.ReplaceCVars(text3);
						}
						try
						{
							if (!this.parsersDetected)
							{
								ParsingMethodCache.ParsingMethodData parsingMethodData;
								if (ParsingMethodCache.Instance.TryGetParsingDelegate(this.View, this.attributeName, out parsingMethodData) && parsingMethodData.TryGetDelegateForSourceType(typeof(string), out this.parsingDelegateView))
								{
									this.parsingDelegateView(this.View, text3);
								}
								if (this.parsingDelegateView == null && this.View.Controller != null && ParsingMethodCache.Instance.TryGetParsingDelegate(this.View.Controller, this.attributeName, out parsingMethodData) && parsingMethodData.TryGetDelegateForSourceType(typeof(string), out this.parsingDelegateController))
								{
									this.parsingDelegateController(this.View.Controller, text3);
								}
								if (this.parsingDelegateView == null && this.parsingDelegateController == null)
								{
									this.View.ParseAttributeViewAndController(this.attributeName, text3);
								}
								this.parsersDetected = true;
							}
							else if (this.parsingDelegateView != null)
							{
								this.parsingDelegateView(this.View, text3);
							}
							else if (this.parsingDelegateController != null)
							{
								this.parsingDelegateController(this.View.Controller, text3);
							}
							else
							{
								this.View.ParseAttributeViewAndController(this.attributeName, text3);
							}
						}
						catch (Exception ex)
						{
							Log.Error(string.Concat(new string[]
							{
								"[XUi] Exception parsing result of binding. Attribute: '",
								this.attributeName,
								"', binding string: '",
								this.sourceText,
								"', binding result: '",
								text3,
								"', view hierarchy: ",
								this.View.GetXuiHierarchy(),
								":"
							}));
							Log.Exception(ex);
						}
					}
				}
			}
		}
	}

	// Token: 0x06008A37 RID: 35383 RVA: 0x0034D608 File Offset: 0x0034B808
	[PublicizedFrom(EAccessModifier.Private)]
	public bool refreshValueSingleTyped()
	{
		if (this.typedPathDisabled)
		{
			return false;
		}
		object obj;
		if (!this.bindingList[0].TryGetTypedValue(out obj))
		{
			this.typedPathDisabled = true;
			return false;
		}
		if (!this.typedParserResolved)
		{
			this.typedParserResolved = true;
			if (!(obj is string))
			{
				this.resolveSingleTypedParser(obj.GetType());
			}
			if (this.typedParsingDelegate == null)
			{
				this.typedPathDisabled = true;
				return false;
			}
		}
		bool flag;
		using (this.pmParseAttribute.Auto())
		{
			try
			{
				this.typedParsingDelegate(this.typedTargetElement, obj);
			}
			catch (Exception ex)
			{
				Log.Error(string.Format("[XUi] Exception parsing result of binding. Attribute: '{0}', binding string: '{1}', binding result: '{2}', view hierarchy: {3}:", new object[]
				{
					this.attributeName,
					this.sourceText,
					obj,
					this.View.GetXuiHierarchy()
				}));
				Log.Exception(ex);
			}
			flag = true;
		}
		return flag;
	}

	// Token: 0x06008A38 RID: 35384 RVA: 0x0034D708 File Offset: 0x0034B908
	[PublicizedFrom(EAccessModifier.Private)]
	public void resolveSingleTypedParser(Type _resultType)
	{
		ParsingMethodCache.ParsingMethodData parsingMethodData;
		if (ParsingMethodCache.Instance.TryGetParsingDelegate(this.View, this.attributeName, out parsingMethodData) && parsingMethodData.TryGetDelegateForSourceType(_resultType, out this.typedParsingDelegate))
		{
			this.typedTargetElement = this.View;
			return;
		}
		if (this.View.Controller != null && ParsingMethodCache.Instance.TryGetParsingDelegate(this.View.Controller, this.attributeName, out parsingMethodData) && parsingMethodData.TryGetDelegateForSourceType(_resultType, out this.typedParsingDelegate))
		{
			this.typedTargetElement = this.View.Controller;
			return;
		}
		this.typedParsingDelegate = null;
	}

	// Token: 0x040066D9 RID: 26329
	public readonly XUiView View;

	// Token: 0x040066DA RID: 26330
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly string attributeName;

	// Token: 0x040066DB RID: 26331
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly string sourceText;

	// Token: 0x040066DC RID: 26332
	[PublicizedFrom(EAccessModifier.Private)]
	public bool parsersDetected;

	// Token: 0x040066DD RID: 26333
	[PublicizedFrom(EAccessModifier.Private)]
	public XuiParsingDelegate parsingDelegateView;

	// Token: 0x040066DE RID: 26334
	[PublicizedFrom(EAccessModifier.Private)]
	public XuiParsingDelegate parsingDelegateController;

	// Token: 0x040066DF RID: 26335
	[PublicizedFrom(EAccessModifier.Private)]
	public bool singleTypedBinding;

	// Token: 0x040066E0 RID: 26336
	[PublicizedFrom(EAccessModifier.Private)]
	public bool typedParserResolved;

	// Token: 0x040066E1 RID: 26337
	[PublicizedFrom(EAccessModifier.Private)]
	public bool typedPathDisabled;

	// Token: 0x040066E2 RID: 26338
	[PublicizedFrom(EAccessModifier.Private)]
	public IXUiElement typedTargetElement;

	// Token: 0x040066E3 RID: 26339
	[PublicizedFrom(EAccessModifier.Private)]
	public XuiParsingDelegate typedParsingDelegate;

	// Token: 0x040066E4 RID: 26340
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<BindingItem> bindingList = new List<BindingItem>();

	// Token: 0x040066E5 RID: 26341
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<string> cachedBindingValues = new List<string>();

	// Token: 0x040066E6 RID: 26342
	[PublicizedFrom(EAccessModifier.Private)]
	public string cachedResultValue;

	// Token: 0x040066E7 RID: 26343
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly ProfilerMarker pmCtor = new ProfilerMarker("BindingInfo.ctor");

	// Token: 0x040066E8 RID: 26344
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly ProfilerMarker pmRefreshValueComplete = new ProfilerMarker("BindingInfo.RefreshValue");

	// Token: 0x040066E9 RID: 26345
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly ProfilerMarker pmRefreshValueCompleteSourceText;

	// Token: 0x040066EA RID: 26346
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly ProfilerMarker pmGetBindingValues = new ProfilerMarker("GetBindingValues");

	// Token: 0x040066EB RID: 26347
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly ProfilerMarker pmBuildAttributeValue = new ProfilerMarker("BuildAttributeValue");

	// Token: 0x040066EC RID: 26348
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly ProfilerMarker pmParseAttribute = new ProfilerMarker("ParseAttribute");
}
