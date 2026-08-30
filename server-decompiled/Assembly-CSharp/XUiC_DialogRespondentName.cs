using System;
using UnityEngine.Scripting;

// Token: 0x02000F0B RID: 3851
[Preserve]
public class XUiC_DialogRespondentName : XUiController
{
	// Token: 0x17000E1B RID: 3611
	// (get) Token: 0x060078DE RID: 30942 RVA: 0x002D9509 File Offset: 0x002D7709
	// (set) Token: 0x060078DF RID: 30943 RVA: 0x002D9511 File Offset: 0x002D7711
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

	// Token: 0x060078E0 RID: 30944 RVA: 0x002D9527 File Offset: 0x002D7727
	public override void OnClose()
	{
		base.OnClose();
		this.currentDialog = null;
	}

	// Token: 0x060078E1 RID: 30945 RVA: 0x002D9538 File Offset: 0x002D7738
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

	// Token: 0x060078E2 RID: 30946 RVA: 0x002D9599 File Offset: 0x002D7799
	public void Refresh()
	{
		base.RefreshBindings();
		this.IsDirty = true;
	}

	// Token: 0x04005937 RID: 22839
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label label;

	// Token: 0x04005938 RID: 22840
	[PublicizedFrom(EAccessModifier.Private)]
	public Dialog currentDialog;
}
