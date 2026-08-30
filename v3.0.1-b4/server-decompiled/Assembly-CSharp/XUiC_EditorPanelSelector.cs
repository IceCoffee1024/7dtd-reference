using System;
using UnityEngine.Scripting;

// Token: 0x02000F16 RID: 3862
[Preserve]
public class XUiC_EditorPanelSelector : XUiController
{
	// Token: 0x06007930 RID: 31024 RVA: 0x002DA7F8 File Offset: 0x002D89F8
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

	// Token: 0x06007931 RID: 31025 RVA: 0x002DA851 File Offset: 0x002D8A51
	[PublicizedFrom(EAccessModifier.Private)]
	public void ButtonsOnCategoryClickChanged(XUiC_CategoryEntry _categoryEntry)
	{
		this.OpenSelectedWindow();
	}

	// Token: 0x06007932 RID: 31026 RVA: 0x002DA85C File Offset: 0x002D8A5C
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

	// Token: 0x06007933 RID: 31027 RVA: 0x002DA914 File Offset: 0x002D8B14
	public void SetSelected(string _name)
	{
		XUiC_CategoryList xuiC_CategoryList = this.buttons;
		if (xuiC_CategoryList == null)
		{
			return;
		}
		xuiC_CategoryList.SetCategory(_name);
	}

	// Token: 0x06007934 RID: 31028 RVA: 0x002DA927 File Offset: 0x002D8B27
	public override void OnOpen()
	{
		base.OnOpen();
		if (PrefabEditModeManager.Instance.IsActive() && PrefabEditModeManager.Instance.VoxelPrefab != null)
		{
			PrefabEditModeManager.Instance.VoxelPrefab.RenderingCostStats = WorldStats.CaptureWorldStats();
		}
		this.OpenSelectedWindow();
	}

	// Token: 0x06007935 RID: 31029 RVA: 0x002DA964 File Offset: 0x002D8B64
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

	// Token: 0x06007936 RID: 31030 RVA: 0x0008A091 File Offset: 0x00088291
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			base.RefreshBindings();
			this.IsDirty = false;
		}
	}

	// Token: 0x06007937 RID: 31031 RVA: 0x002DA9E0 File Offset: 0x002D8BE0
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

	// Token: 0x0400595F RID: 22879
	public static string ID = "";

	// Token: 0x04005960 RID: 22880
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CategoryList buttons;
}
