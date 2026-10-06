using System;
using GUI_2;
using UnityEngine.Scripting;

// Token: 0x0200107D RID: 4221
[Preserve]
public class XUiC_SkillWindowGroup : XUiController
{
	// Token: 0x17000FD0 RID: 4048
	// (get) Token: 0x060085F5 RID: 34293 RVA: 0x00333B8A File Offset: 0x00331D8A
	// (set) Token: 0x060085F6 RID: 34294 RVA: 0x00333B92 File Offset: 0x00331D92
	public ProgressionValue CurrentSkill
	{
		get
		{
			return this.currentSkill;
		}
		set
		{
			this.currentSkill = value;
		}
	}

	// Token: 0x060085F7 RID: 34295 RVA: 0x00333B9C File Offset: 0x00331D9C
	public override void Init()
	{
		base.Init();
		this.skillList = base.GetChildByType<XUiC_SkillList>();
		this.skillAttributeInfoWindow = base.GetChildByType<XUiC_SkillAttributeInfoWindow>();
		this.skillSkillInfoWindow = base.GetChildByType<XUiC_SkillSkillInfoWindow>();
		this.skillPerkInfoWindow = base.GetChildByType<XUiC_SkillPerkInfoWindow>();
		this.skillBookInfoWindow = base.GetChildByType<XUiC_SkillBookInfoWindow>();
		this.skillCraftingInfoWindow = base.GetChildByType<XUiC_SkillCraftingInfoWindow>();
		XUiController[] childrenByType = base.GetChildrenByType<XUiC_SkillEntry>(null);
		XUiController[] array = childrenByType;
		this.skillEntries = new XUiC_SkillEntry[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
			this.skillEntries[i] = (XUiC_SkillEntry)array[i];
			this.skillEntries[i].OnPress += this.XUiC_SkillEntry_OnPress;
		}
	}

	// Token: 0x060085F8 RID: 34296 RVA: 0x00333C46 File Offset: 0x00331E46
	[PublicizedFrom(EAccessModifier.Private)]
	public void CategoryList_CategoryChanged(XUiC_CategoryEntry _categoryEntry)
	{
		this.skillList.Category = _categoryEntry.CategoryName;
		this.skillList.RefreshSkillList();
		this.skillList.SelectFirstEntry();
		this.IsDirty = true;
	}

	// Token: 0x060085F9 RID: 34297 RVA: 0x00333C76 File Offset: 0x00331E76
	[PublicizedFrom(EAccessModifier.Private)]
	public void CategoryList_CategoryClickChanged(XUiC_CategoryEntry _categoryEntry)
	{
		this.skillList.Category = _categoryEntry.CategoryName;
		this.skillList.SetFilterText("");
		this.skillList.RefreshSkillList();
		this.skillList.SelectFirstEntry();
		this.IsDirty = true;
	}

	// Token: 0x060085FA RID: 34298 RVA: 0x0008A995 File Offset: 0x00088B95
	[PublicizedFrom(EAccessModifier.Private)]
	public void XUiC_SkillEntry_OnPress(XUiController _sender, int _mouseButton)
	{
		this.IsDirty = true;
	}

	// Token: 0x060085FB RID: 34299 RVA: 0x00333CB6 File Offset: 0x00331EB6
	public override void OnClose()
	{
		base.OnClose();
		this.xui.playerUI.windowManager.Close("windowpaging");
		this.xui.CalloutWindow.DisableCallouts(XUiC_GamepadCalloutWindow.CalloutType.Menu);
	}

	// Token: 0x060085FC RID: 34300 RVA: 0x00333CEC File Offset: 0x00331EEC
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			this.CurrentSkill = this.xui.SelectedSkill;
			this.skillAttributeInfoWindow.SkillChanged();
			this.skillSkillInfoWindow.IsDirty = true;
			this.skillPerkInfoWindow.SkillChanged();
			this.skillBookInfoWindow.SkillChanged();
			this.skillCraftingInfoWindow.SkillChanged();
			if (this.skillList.DisplayType == ProgressionClass.DisplayTypes.Book)
			{
				this.skillBookInfoWindow.ViewComponent.IsVisible = true;
			}
			else if (this.skillList.DisplayType == ProgressionClass.DisplayTypes.Crafting)
			{
				this.skillCraftingInfoWindow.ViewComponent.IsVisible = true;
			}
			else if (this.xui.SelectedSkill != null)
			{
				if (this.xui.SelectedSkill.ProgressionClass.IsAttribute)
				{
					this.skillAttributeInfoWindow.ViewComponent.IsVisible = true;
				}
				else if (this.xui.SelectedSkill.ProgressionClass.IsSkill)
				{
					this.skillSkillInfoWindow.ViewComponent.IsVisible = true;
				}
				else
				{
					this.skillPerkInfoWindow.ViewComponent.IsVisible = true;
				}
			}
			this.IsDirty = false;
		}
	}

	// Token: 0x060085FD RID: 34301 RVA: 0x00333E14 File Offset: 0x00332014
	public override void OnOpen()
	{
		base.OnOpen();
		if (this.categoryList == null)
		{
			XUiController childByType = base.GetChildByType<XUiC_CategoryList>();
			if (childByType != null)
			{
				this.categoryList = (XUiC_CategoryList)childByType;
				this.categoryList.SetupCategoriesByWorkstation("skills");
				this.categoryList.CategoryChanged += this.CategoryList_CategoryChanged;
				this.categoryList.CategoryClickChanged += this.CategoryList_CategoryClickChanged;
			}
		}
		this.xui.playerUI.windowManager.Open("windowpaging", false);
		this.xui.CalloutWindow.ClearCallouts(XUiC_GamepadCalloutWindow.CalloutType.Menu);
		this.xui.CalloutWindow.AddCallout(UIUtils.ButtonIcon.FaceButtonSouth, "igcoSelect", XUiC_GamepadCalloutWindow.CalloutType.Menu);
		this.xui.CalloutWindow.AddCallout(UIUtils.ButtonIcon.FaceButtonEast, "igcoExit", XUiC_GamepadCalloutWindow.CalloutType.Menu);
		this.xui.CalloutWindow.EnableCallouts(XUiC_GamepadCalloutWindow.CalloutType.Menu, 0f);
		XUiC_WindowSelector childByType2 = this.xui.FindWindowGroupByName("windowpaging").GetChildByType<XUiC_WindowSelector>();
		if (childByType2 != null)
		{
			childByType2.SetSelected("skills");
		}
		this.IsDirty = true;
		if (this.categoryList.CurrentCategory == null)
		{
			this.categoryList.SetCategoryToFirst();
		}
		this.skillList.Category = this.categoryList.CurrentCategory.CategoryName;
		this.skillList.RefreshSkillList();
		if (this.xui.SelectedSkill == null)
		{
			this.skillList.SelectFirstEntry();
		}
		else
		{
			this.skillList.SelectedEntry.SelectCursorElement(true, false);
		}
		this.IsDirty = true;
	}

	// Token: 0x040063A8 RID: 25512
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SkillEntry[] skillEntries;

	// Token: 0x040063A9 RID: 25513
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SkillList skillList;

	// Token: 0x040063AA RID: 25514
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CategoryList categoryList;

	// Token: 0x040063AB RID: 25515
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SkillAttributeInfoWindow skillAttributeInfoWindow;

	// Token: 0x040063AC RID: 25516
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SkillSkillInfoWindow skillSkillInfoWindow;

	// Token: 0x040063AD RID: 25517
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SkillPerkInfoWindow skillPerkInfoWindow;

	// Token: 0x040063AE RID: 25518
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SkillBookInfoWindow skillBookInfoWindow;

	// Token: 0x040063AF RID: 25519
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SkillCraftingInfoWindow skillCraftingInfoWindow;

	// Token: 0x040063B0 RID: 25520
	[PublicizedFrom(EAccessModifier.Private)]
	public ProgressionValue currentSkill;
}
