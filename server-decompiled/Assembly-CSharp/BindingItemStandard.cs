using System;

// Token: 0x020010DA RID: 4314
public class BindingItemStandard : BindingItem
{
	// Token: 0x060088E0 RID: 35040 RVA: 0x00343D9C File Offset: 0x00341F9C
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

	// Token: 0x060088E1 RID: 35041 RVA: 0x00343E6C File Offset: 0x0034206C
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

	// Token: 0x040065C5 RID: 26053
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiController dataContext;

	// Token: 0x040065C6 RID: 26054
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XuiBindingDelegate bindingDelegate;
}
