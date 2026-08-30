using System;
using UnityEngine.Scripting;

// Token: 0x02000F25 RID: 3877
[Preserve]
public class XUiC_DialogStatementWindow : XUiController
{
	// Token: 0x17000E38 RID: 3640
	// (get) Token: 0x060079AE RID: 31150 RVA: 0x002DDAEC File Offset: 0x002DBCEC
	// (set) Token: 0x060079AF RID: 31151 RVA: 0x002DDAF4 File Offset: 0x002DBCF4
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

	// Token: 0x060079B0 RID: 31152 RVA: 0x002DDB0C File Offset: 0x002DBD0C
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

	// Token: 0x060079B1 RID: 31153 RVA: 0x002DDB5F File Offset: 0x002DBD5F
	public override void OnClose()
	{
		base.OnClose();
		this.currentDialog = null;
	}

	// Token: 0x060079B2 RID: 31154 RVA: 0x002DDB6E File Offset: 0x002DBD6E
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			this.IsDirty = false;
		}
	}

	// Token: 0x060079B3 RID: 31155 RVA: 0x002DDB88 File Offset: 0x002DBD88
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

	// Token: 0x060079B4 RID: 31156 RVA: 0x002DD525 File Offset: 0x002DB725
	public void Refresh()
	{
		base.RefreshBindings();
		this.IsDirty = true;
	}

	// Token: 0x040059FA RID: 23034
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label label;

	// Token: 0x040059FB RID: 23035
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite backgroundSprite;

	// Token: 0x040059FC RID: 23036
	[PublicizedFrom(EAccessModifier.Private)]
	public Dialog currentDialog;
}
