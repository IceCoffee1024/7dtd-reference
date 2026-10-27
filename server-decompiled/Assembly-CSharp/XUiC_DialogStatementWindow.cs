using System;
using UnityEngine.Scripting;

// Token: 0x02000F43 RID: 3907
[Preserve]
public class XUiC_DialogStatementWindow : XUiController
{
	// Token: 0x17000E5F RID: 3679
	// (get) Token: 0x060079EC RID: 31212 RVA: 0x002E2778 File Offset: 0x002E0978
	// (set) Token: 0x060079ED RID: 31213 RVA: 0x002E2780 File Offset: 0x002E0980
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

	// Token: 0x060079EE RID: 31214 RVA: 0x002E2798 File Offset: 0x002E0998
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

	// Token: 0x060079EF RID: 31215 RVA: 0x002E27EB File Offset: 0x002E09EB
	public override void OnClose()
	{
		base.OnClose();
		this.currentDialog = null;
	}

	// Token: 0x060079F0 RID: 31216 RVA: 0x002E27FA File Offset: 0x002E09FA
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			this.IsDirty = false;
		}
	}

	// Token: 0x060079F1 RID: 31217 RVA: 0x002E2814 File Offset: 0x002E0A14
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

	// Token: 0x060079F2 RID: 31218 RVA: 0x002E21B1 File Offset: 0x002E03B1
	public void Refresh()
	{
		base.RefreshBindings();
		this.IsDirty = true;
	}

	// Token: 0x04005A65 RID: 23141
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label label;

	// Token: 0x04005A66 RID: 23142
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite backgroundSprite;

	// Token: 0x04005A67 RID: 23143
	[PublicizedFrom(EAccessModifier.Private)]
	public Dialog currentDialog;
}
