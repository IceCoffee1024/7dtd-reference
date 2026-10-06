using System;
using UnityEngine.Scripting;

// Token: 0x02000F34 RID: 3892
[Preserve]
public class XUiC_DialogStatementWindow : XUiController
{
	// Token: 0x17000E55 RID: 3669
	// (get) Token: 0x060079A9 RID: 31145 RVA: 0x002E0E88 File Offset: 0x002DF088
	// (set) Token: 0x060079AA RID: 31146 RVA: 0x002E0E90 File Offset: 0x002DF090
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

	// Token: 0x060079AB RID: 31147 RVA: 0x002E0EA8 File Offset: 0x002DF0A8
	public override void Init()
	{
		base.Init();
		XUiController xuiController = base.GetChildById("statementLabel");
		if (xuiController != null)
		{
			this.label = (XUiV_Label)xuiController.ViewComponent;
		}
		xuiController = base.GetChildById("backgroundSprite");
		if (xuiController != null)
		{
			this.backgroundSprite = (XUiV_Sprite)xuiController.ViewComponent;
		}
	}

	// Token: 0x060079AC RID: 31148 RVA: 0x002E0EFB File Offset: 0x002DF0FB
	public override void OnClose()
	{
		base.OnClose();
		this.currentDialog = null;
	}

	// Token: 0x060079AD RID: 31149 RVA: 0x002E0F0A File Offset: 0x002DF10A
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			this.IsDirty = false;
		}
	}

	// Token: 0x060079AE RID: 31150 RVA: 0x002E0F24 File Offset: 0x002DF124
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string value, string bindingName)
	{
		if (bindingName == "statement")
		{
			value = ((this.currentDialog != null && this.currentDialog.CurrentStatement != null) ? this.currentDialog.CurrentStatement.Text : "");
			return true;
		}
		return base.GetBindingValueInternal(ref value, bindingName);
	}

	// Token: 0x060079AF RID: 31151 RVA: 0x002E08C1 File Offset: 0x002DEAC1
	public void Refresh()
	{
		base.RefreshBindings();
		this.IsDirty = true;
	}

	// Token: 0x04005A32 RID: 23090
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label label;

	// Token: 0x04005A33 RID: 23091
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite backgroundSprite;

	// Token: 0x04005A34 RID: 23092
	[PublicizedFrom(EAccessModifier.Private)]
	public Dialog currentDialog;
}
