using System;
using UnityEngine.Scripting;

// Token: 0x02000F22 RID: 3874
[Preserve]
public class XUiC_DialogRespondentName : XUiController
{
	// Token: 0x17000E35 RID: 3637
	// (get) Token: 0x06007996 RID: 31126 RVA: 0x002DD495 File Offset: 0x002DB695
	// (set) Token: 0x06007997 RID: 31127 RVA: 0x002DD49D File Offset: 0x002DB69D
	public Dialog CurrentDialog
	{
		get
		{
			return this.currentDialog;
		}
		set
		{
			this.currentDialog = value;
			base.RefreshBindings();
			this.IsDirty = true;
		}
	}

	// Token: 0x06007998 RID: 31128 RVA: 0x002DD4B3 File Offset: 0x002DB6B3
	public override void OnClose()
	{
		base.OnClose();
		this.currentDialog = null;
	}

	// Token: 0x06007999 RID: 31129 RVA: 0x002DD4C4 File Offset: 0x002DB6C4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string value, string bindingName)
	{
		if (bindingName == "respondentname")
		{
			value = ((this.xui.Dialog.Respondent != null) ? Localization.Get(this.xui.Dialog.Respondent.EntityName, false, null) : "");
			return true;
		}
		return base.GetBindingValueInternal(ref value, bindingName);
	}

	// Token: 0x0600799A RID: 31130 RVA: 0x002DD525 File Offset: 0x002DB725
	public void Refresh()
	{
		base.RefreshBindings();
		this.IsDirty = true;
	}

	// Token: 0x040059E9 RID: 23017
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label label;

	// Token: 0x040059EA RID: 23018
	[PublicizedFrom(EAccessModifier.Private)]
	public Dialog currentDialog;
}
