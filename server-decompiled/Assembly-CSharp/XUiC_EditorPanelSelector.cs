using System;
using UnityEngine.Scripting;

// Token: 0x02000F3C RID: 3900
[Preserve]
public class XUiC_EditorPanelSelector : XUiController
{
	// Token: 0x060079E6 RID: 31206 RVA: 0x002E1B18 File Offset: 0x002DFD18
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

	// Token: 0x060079E7 RID: 31207 RVA: 0x002E1B71 File Offset: 0x002DFD71
	[PublicizedFrom(EAccessModifier.Private)]
	public void ButtonsOnCategoryClickChanged(XUiC_CategoryEntry _categoryEntry)
	{
		this.OpenSelectedWindow();
	}

	// Token: 0x060079E8 RID: 31208 RVA: 0x002E1B7C File Offset: 0x002DFD7C
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

	// Token: 0x060079E9 RID: 31209 RVA: 0x002E1C34 File Offset: 0x002DFE34
	public void SetSelected(string _name)
	{
		XUiC_CategoryList xuiC_CategoryList = this.buttons;
		if (xuiC_CategoryList == null)
		{
			return;
		}
		xuiC_CategoryList.SetCategory(_name);
	}

	// Token: 0x060079EA RID: 31210 RVA: 0x002E1C47 File Offset: 0x002DFE47
	public override void OnOpen()
	{
		base.OnOpen();
		if (PrefabEditModeManager.Instance.IsActive() && PrefabEditModeManager.Instance.VoxelPrefab != null)
		{
			PrefabEditModeManager.Instance.VoxelPrefab.RenderingCostStats = WorldStats.CaptureWorldStats();
		}
		this.OpenSelectedWindow();
	}

	// Token: 0x060079EB RID: 31211 RVA: 0x002E1C84 File Offset: 0x002DFE84
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

	// Token: 0x060079EC RID: 31212 RVA: 0x0008A9BD File Offset: 0x00088BBD
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			base.RefreshBindings();
			this.IsDirty = false;
		}
	}

	// Token: 0x060079ED RID: 31213 RVA: 0x002E1D00 File Offset: 0x002DFF00
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

	// Token: 0x04005A4A RID: 23114
	public static string ID = "";

	// Token: 0x04005A4B RID: 23115
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CategoryList buttons;
}
