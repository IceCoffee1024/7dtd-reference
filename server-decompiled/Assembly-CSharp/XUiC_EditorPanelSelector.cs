using System;
using UnityEngine.Scripting;

// Token: 0x02000F2D RID: 3885
[Preserve]
public class XUiC_EditorPanelSelector : XUiController
{
	// Token: 0x060079E9 RID: 31209 RVA: 0x002DE7AC File Offset: 0x002DC9AC
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

	// Token: 0x060079EA RID: 31210 RVA: 0x002DE805 File Offset: 0x002DCA05
	[PublicizedFrom(EAccessModifier.Private)]
	public void ButtonsOnCategoryClickChanged(XUiC_CategoryEntry _categoryEntry)
	{
		this.OpenSelectedWindow();
	}

	// Token: 0x060079EB RID: 31211 RVA: 0x002DE810 File Offset: 0x002DCA10
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

	// Token: 0x060079EC RID: 31212 RVA: 0x002DE8C8 File Offset: 0x002DCAC8
	public void SetSelected(string _name)
	{
		XUiC_CategoryList xuiC_CategoryList = this.buttons;
		if (xuiC_CategoryList == null)
		{
			return;
		}
		xuiC_CategoryList.SetCategory(_name);
	}

	// Token: 0x060079ED RID: 31213 RVA: 0x002DE8DB File Offset: 0x002DCADB
	public override void OnOpen()
	{
		base.OnOpen();
		if (PrefabEditModeManager.Instance.IsActive() && PrefabEditModeManager.Instance.VoxelPrefab != null)
		{
			PrefabEditModeManager.Instance.VoxelPrefab.RenderingCostStats = WorldStats.CaptureWorldStats();
		}
		this.OpenSelectedWindow();
	}

	// Token: 0x060079EE RID: 31214 RVA: 0x002DE918 File Offset: 0x002DCB18
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

	// Token: 0x060079EF RID: 31215 RVA: 0x0008A73D File Offset: 0x0008893D
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			base.RefreshBindings();
			this.IsDirty = false;
		}
	}

	// Token: 0x060079F0 RID: 31216 RVA: 0x002DE994 File Offset: 0x002DCB94
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

	// Token: 0x04005A12 RID: 23058
	public static string ID = "";

	// Token: 0x04005A13 RID: 23059
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CategoryList buttons;
}
