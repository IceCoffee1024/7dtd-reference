using System;
using UnityEngine.Scripting;

// Token: 0x02000F0E RID: 3854
[Preserve]
public class XUiC_DialogStatementWindow : XUiController
{
	// Token: 0x17000E1E RID: 3614
	// (get) Token: 0x060078F6 RID: 30966 RVA: 0x002D9B60 File Offset: 0x002D7D60
	// (set) Token: 0x060078F7 RID: 30967 RVA: 0x002D9B68 File Offset: 0x002D7D68
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

	// Token: 0x060078F8 RID: 30968 RVA: 0x002D9B80 File Offset: 0x002D7D80
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

	// Token: 0x060078F9 RID: 30969 RVA: 0x002D9BD3 File Offset: 0x002D7DD3
	public override void OnClose()
	{
		base.OnClose();
		this.currentDialog = null;
	}

	// Token: 0x060078FA RID: 30970 RVA: 0x002D9BE2 File Offset: 0x002D7DE2
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			this.IsDirty = false;
		}
	}

	// Token: 0x060078FB RID: 30971 RVA: 0x002D9BFC File Offset: 0x002D7DFC
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

	// Token: 0x060078FC RID: 30972 RVA: 0x002D9599 File Offset: 0x002D7799
	public void Refresh()
	{
		base.RefreshBindings();
		this.IsDirty = true;
	}

	// Token: 0x04005948 RID: 22856
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label label;

	// Token: 0x04005949 RID: 22857
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite backgroundSprite;

	// Token: 0x0400594A RID: 22858
	[PublicizedFrom(EAccessModifier.Private)]
	public Dialog currentDialog;
}
