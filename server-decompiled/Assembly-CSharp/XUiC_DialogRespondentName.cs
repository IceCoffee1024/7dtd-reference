using System;
using UnityEngine.Scripting;

// Token: 0x02000F40 RID: 3904
[Preserve]
public class XUiC_DialogRespondentName : XUiController
{
	// Token: 0x17000E5C RID: 3676
	// (get) Token: 0x060079D4 RID: 31188 RVA: 0x002E2121 File Offset: 0x002E0321
	// (set) Token: 0x060079D5 RID: 31189 RVA: 0x002E2129 File Offset: 0x002E0329
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

	// Token: 0x060079D6 RID: 31190 RVA: 0x002E213F File Offset: 0x002E033F
	public override void OnClose()
	{
		base.OnClose();
		this.currentDialog = null;
	}

	// Token: 0x060079D7 RID: 31191 RVA: 0x002E2150 File Offset: 0x002E0350
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

	// Token: 0x060079D8 RID: 31192 RVA: 0x002E21B1 File Offset: 0x002E03B1
	public void Refresh()
	{
		base.RefreshBindings();
		this.IsDirty = true;
	}

	// Token: 0x04005A54 RID: 23124
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label label;

	// Token: 0x04005A55 RID: 23125
	[PublicizedFrom(EAccessModifier.Private)]
	public Dialog currentDialog;
}
