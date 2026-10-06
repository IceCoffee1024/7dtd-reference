using System;
using System.Collections.Generic;
using Unity.Profiling;

// Token: 0x020010F5 RID: 4341
public class BindingInfo : IBindingInstance
{
	// Token: 0x060089E1 RID: 35297 RVA: 0x0034AEF4 File Offset: 0x003490F4
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

	// Token: 0x060089E2 RID: 35298 RVA: 0x0034B11C File Offset: 0x0034931C
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

	// Token: 0x060089E3 RID: 35299 RVA: 0x0034B558 File Offset: 0x00349758
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

	// Token: 0x060089E4 RID: 35300 RVA: 0x0034B658 File Offset: 0x00349858
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

	// Token: 0x04006699 RID: 26265
	public readonly XUiView View;

	// Token: 0x0400669A RID: 26266
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly string attributeName;

	// Token: 0x0400669B RID: 26267
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly string sourceText;

	// Token: 0x0400669C RID: 26268
	[PublicizedFrom(EAccessModifier.Private)]
	public bool parsersDetected;

	// Token: 0x0400669D RID: 26269
	[PublicizedFrom(EAccessModifier.Private)]
	public XuiParsingDelegate parsingDelegateView;

	// Token: 0x0400669E RID: 26270
	[PublicizedFrom(EAccessModifier.Private)]
	public XuiParsingDelegate parsingDelegateController;

	// Token: 0x0400669F RID: 26271
	[PublicizedFrom(EAccessModifier.Private)]
	public bool singleTypedBinding;

	// Token: 0x040066A0 RID: 26272
	[PublicizedFrom(EAccessModifier.Private)]
	public bool typedParserResolved;

	// Token: 0x040066A1 RID: 26273
	[PublicizedFrom(EAccessModifier.Private)]
	public bool typedPathDisabled;

	// Token: 0x040066A2 RID: 26274
	[PublicizedFrom(EAccessModifier.Private)]
	public IXUiElement typedTargetElement;

	// Token: 0x040066A3 RID: 26275
	[PublicizedFrom(EAccessModifier.Private)]
	public XuiParsingDelegate typedParsingDelegate;

	// Token: 0x040066A4 RID: 26276
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<BindingItem> bindingList = new List<BindingItem>();

	// Token: 0x040066A5 RID: 26277
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<string> cachedBindingValues = new List<string>();

	// Token: 0x040066A6 RID: 26278
	[PublicizedFrom(EAccessModifier.Private)]
	public string cachedResultValue;

	// Token: 0x040066A7 RID: 26279
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly ProfilerMarker pmCtor = new ProfilerMarker("BindingInfo.ctor");

	// Token: 0x040066A8 RID: 26280
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly ProfilerMarker pmRefreshValueComplete = new ProfilerMarker("BindingInfo.RefreshValue");

	// Token: 0x040066A9 RID: 26281
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly ProfilerMarker pmRefreshValueCompleteSourceText;

	// Token: 0x040066AA RID: 26282
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly ProfilerMarker pmGetBindingValues = new ProfilerMarker("GetBindingValues");

	// Token: 0x040066AB RID: 26283
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly ProfilerMarker pmBuildAttributeValue = new ProfilerMarker("BuildAttributeValue");

	// Token: 0x040066AC RID: 26284
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly ProfilerMarker pmParseAttribute = new ProfilerMarker("ParseAttribute");
}
