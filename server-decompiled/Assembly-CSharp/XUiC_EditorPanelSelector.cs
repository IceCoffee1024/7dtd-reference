using System;
using UnityEngine.Scripting;

// Token: 0x02000F4B RID: 3915
[Preserve]
public class XUiC_EditorPanelSelector : XUiController
{
	// Token: 0x06007A29 RID: 31273 RVA: 0x002E3408 File Offset: 0x002E1608
	public override void Init()
	{
		base.Init();
		XUiC_EditorPanelSelector.ID = base.WindowGroup.Id;
		this.buttons = base.GetChildByType<XUiC_CategoryList>();
		if (this.buttons != null)
		{
			this.buttons.CategoryClickChanged += this.ButtonsOnCategoryClickChanged;
			this.buttons.SetCategoryToFirst();
		}
	}

	// Token: 0x06007A2A RID: 31274 RVA: 0x002E3461 File Offset: 0x002E1661
	[PublicizedFrom(EAccessModifier.Private)]
	public void ButtonsOnCategoryClickChanged(XUiC_CategoryEntry _categoryEntry)
	{
		this.OpenSelectedWindow();
	}

	// Token: 0x06007A2B RID: 31275 RVA: 0x002E346C File Offset: 0x002E166C
	public void OpenSelectedWindow()
	{
		if (this.buttons == null)
		{
			return;
		}
		XUiC_CategoryEntry currentCategory = this.buttons.CurrentCategory;
		string text = ((currentCategory != null) ? currentCategory.CategoryName : null);
		foreach (XUiC_CategoryEntry xuiC_CategoryEntry in this.buttons.CategoryButtons)
		{
			if (text == null || text != xuiC_CategoryEntry.CategoryName)
			{
				this.xui.playerUI.windowManager.Close(xuiC_CategoryEntry.CategoryName);
			}
		}
		if (text != null)
		{
			this.xui.playerUI.windowManager.Open(text, false);
		}
	}

	// Token: 0x06007A2C RID: 31276 RVA: 0x002E3524 File Offset: 0x002E1724
	public void SetSelected(string _name)
	{
		XUiC_CategoryList xuiC_CategoryList = this.buttons;
		if (xuiC_CategoryList == null)
		{
			return;
		}
		xuiC_CategoryList.SetCategory(_name);
	}

	// Token: 0x06007A2D RID: 31277 RVA: 0x002E3537 File Offset: 0x002E1737
	public override void OnOpen()
	{
		base.OnOpen();
		if (PrefabEditModeManager.Instance.IsActive() && PrefabEditModeManager.Instance.VoxelPrefab != null)
		{
			PrefabEditModeManager.Instance.VoxelPrefab.RenderingCostStats = WorldStats.CaptureWorldStats();
		}
		this.OpenSelectedWindow();
	}

	// Token: 0x06007A2E RID: 31278 RVA: 0x002E3574 File Offset: 0x002E1774
	public override void OnClose()
	{
		base.OnClose();
		if (this.buttons != null)
		{
			foreach (XUiC_CategoryEntry xuiC_CategoryEntry in this.buttons.CategoryButtons)
			{
				this.xui.playerUI.windowManager.Close(xuiC_CategoryEntry.CategoryName);
			}
		}
	}

	// Token: 0x06007A2F RID: 31279 RVA: 0x0008ABF1 File Offset: 0x00088DF1
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			base.RefreshBindings();
			this.IsDirty = false;
		}
	}

	// Token: 0x06007A30 RID: 31280 RVA: 0x002E35F0 File Offset: 0x002E17F0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string _value, string _bindingName)
	{
		if (_bindingName == "panelname")
		{
			XUiC_CategoryList xuiC_CategoryList = this.buttons;
			string text;
			if (xuiC_CategoryList == null)
			{
				text = null;
			}
			else
			{
				XUiC_CategoryEntry currentCategory = xuiC_CategoryList.CurrentCategory;
				text = ((currentCategory != null) ? currentCategory.CategoryDisplayName : null);
			}
			_value = text ?? "";
			return true;
		}
		return base.GetBindingValueInternal(ref _value, _bindingName);
	}

	// Token: 0x04005A7D RID: 23165
	public static string ID = "";

	// Token: 0x04005A7E RID: 23166
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CategoryList buttons;
}
