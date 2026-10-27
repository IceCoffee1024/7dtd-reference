using System;

// Token: 0x02001114 RID: 4372
public class BindingItemStandard : BindingItem
{
	// Token: 0x06008A77 RID: 35447 RVA: 0x0034E604 File Offset: 0x0034C804
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
				BindingMethodCache.BindingMethodData bindingMethodData;
				if (BindingMethodCache.Instance.TryGetBindingData(xuiController, this.fieldName, out bindingMethodData))
				{
					this.bindingDelegate = bindingMethodData.Delegate;
					if (bindingMethodData.BoxWriter != null)
					{
						this.boxWriter = bindingMethodData.BoxWriter;
						this.reusableBox = bindingMethodData.CreateBox();
					}
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

	// Token: 0x06008A78 RID: 35448 RVA: 0x0034E70C File Offset: 0x0034C90C
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

	// Token: 0x06008A79 RID: 35449 RVA: 0x0034E784 File Offset: 0x0034C984
	public override bool TryGetTypedValue(out object _value)
	{
		if (this.boxWriter != null)
		{
			this.boxWriter(this.dataContext, this.reusableBox);
			_value = this.reusableBox;
			return true;
		}
		if (this.bindingDelegate != null)
		{
			_value = this.bindingDelegate(this.dataContext);
			return _value != null;
		}
		_value = null;
		return false;
	}

	// Token: 0x04006713 RID: 26387
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiController dataContext;

	// Token: 0x04006714 RID: 26388
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XuiBindingDelegate bindingDelegate;

	// Token: 0x04006715 RID: 26389
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XuiBindingBoxWriterDelegate boxWriter;

	// Token: 0x04006716 RID: 26390
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly object reusableBox;
}
