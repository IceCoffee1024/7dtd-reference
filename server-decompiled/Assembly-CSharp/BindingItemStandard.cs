using System;

// Token: 0x020010F3 RID: 4339
public class BindingItemStandard : BindingItem
{
	// Token: 0x060089D2 RID: 35282 RVA: 0x003485C0 File Offset: 0x003467C0
	public BindingItemStandard(BindingInfo _parent, string _sourceText)
		: base(_sourceText)
	{
		for (XUiController xuiController = _parent.View.Controller; xuiController != null; xuiController = xuiController.Parent)
		{
			if (xuiController.GetType() != typeof(XUiController))
			{
				this.dataContext = xuiController;
				string text = "";
				if (this.dataContext.GetBindingValue(ref text, this.fieldName))
				{
					this.dataContext.Bindings.AddBinding(_parent);
					return;
				}
				if (BindingMethodCache.Instance.TryGetBindingDelegate(xuiController, this.fieldName, out this.bindingDelegate))
				{
					this.dataContext.Bindings.AddBinding(_parent);
					return;
				}
			}
		}
		if (XUiFromXml.DebugXuiLoading == XUiFromXml.DebugLevel.Verbose)
		{
			Log.Warning("[XUi] Binding name '" + this.fieldName + "' not found! Hierarchy: " + _parent.View.GetXuiHierarchy());
		}
	}

	// Token: 0x060089D3 RID: 35283 RVA: 0x00348690 File Offset: 0x00346890
	public override string GetValue()
	{
		if (this.bindingDelegate != null)
		{
			object obj = this.bindingDelegate(this.dataContext);
			if (obj == null)
			{
				Log.Warning("[XUi] Binding '" + this.SourceText + "' returned null, should always return appropriate non-null value of same data type. Hierarchy: " + this.dataContext.GetXuiHierarchy());
				obj = "";
			}
			return obj.ToString();
		}
		string text = "";
		this.dataContext.GetBindingValue(ref text, this.fieldName);
		return text;
	}

	// Token: 0x0400668C RID: 26252
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiController dataContext;

	// Token: 0x0400668D RID: 26253
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XuiBindingDelegate bindingDelegate;
}
