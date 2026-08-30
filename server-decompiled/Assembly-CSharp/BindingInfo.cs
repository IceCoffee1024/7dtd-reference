using System;
using System.Collections.Generic;
using Unity.Profiling;

// Token: 0x020010CB RID: 4299
public class BindingInfo : IBindingInstance
{
	// Token: 0x060088A1 RID: 34977 RVA: 0x00342920 File Offset: 0x00340B20
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
	}

	// Token: 0x060088A2 RID: 34978 RVA: 0x00342B14 File Offset: 0x00340D14
	public void RefreshValue()
	{
		using (this.pmRefreshValueComplete.Auto())
		{
			using (this.pmRefreshValueCompleteSourceText.Auto())
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

	// Token: 0x04006590 RID: 26000
	public readonly XUiView View;

	// Token: 0x04006591 RID: 26001
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly string attributeName;

	// Token: 0x04006592 RID: 26002
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly string sourceText;

	// Token: 0x04006593 RID: 26003
	[PublicizedFrom(EAccessModifier.Private)]
	public bool parsersDetected;

	// Token: 0x04006594 RID: 26004
	[PublicizedFrom(EAccessModifier.Private)]
	public XuiParsingDelegate parsingDelegateView;

	// Token: 0x04006595 RID: 26005
	[PublicizedFrom(EAccessModifier.Private)]
	public XuiParsingDelegate parsingDelegateController;

	// Token: 0x04006596 RID: 26006
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<BindingItem> bindingList = new List<BindingItem>();

	// Token: 0x04006597 RID: 26007
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<string> cachedBindingValues = new List<string>();

	// Token: 0x04006598 RID: 26008
	[PublicizedFrom(EAccessModifier.Private)]
	public string cachedResultValue;

	// Token: 0x04006599 RID: 26009
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly ProfilerMarker pmCtor = new ProfilerMarker("BindingInfo.ctor");

	// Token: 0x0400659A RID: 26010
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly ProfilerMarker pmRefreshValueComplete = new ProfilerMarker("BindingInfo.RefreshValue");

	// Token: 0x0400659B RID: 26011
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly ProfilerMarker pmRefreshValueCompleteSourceText;

	// Token: 0x0400659C RID: 26012
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly ProfilerMarker pmGetBindingValues = new ProfilerMarker("GetBindingValues");

	// Token: 0x0400659D RID: 26013
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly ProfilerMarker pmBuildAttributeValue = new ProfilerMarker("BuildAttributeValue");

	// Token: 0x0400659E RID: 26014
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly ProfilerMarker pmParseAttribute = new ProfilerMarker("ParseAttribute");
}
