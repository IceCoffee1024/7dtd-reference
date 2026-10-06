using System;
using UnityEngine.Scripting;

// Token: 0x02000F31 RID: 3889
[Preserve]
public class XUiC_DialogRespondentName : XUiController
{
	// Token: 0x17000E52 RID: 3666
	// (get) Token: 0x06007991 RID: 31121 RVA: 0x002E0831 File Offset: 0x002DEA31
	// (set) Token: 0x06007992 RID: 31122 RVA: 0x002E0839 File Offset: 0x002DEA39
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

	// Token: 0x06007993 RID: 31123 RVA: 0x002E084F File Offset: 0x002DEA4F
	public override void OnClose()
	{
		base.OnClose();
		this.currentDialog = null;
	}

	// Token: 0x06007994 RID: 31124 RVA: 0x002E0860 File Offset: 0x002DEA60
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

	// Token: 0x06007995 RID: 31125 RVA: 0x002E08C1 File Offset: 0x002DEAC1
	public void Refresh()
	{
		base.RefreshBindings();
		this.IsDirty = true;
	}

	// Token: 0x04005A21 RID: 23073
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label label;

	// Token: 0x04005A22 RID: 23074
	[PublicizedFrom(EAccessModifier.Private)]
	public Dialog currentDialog;
}
