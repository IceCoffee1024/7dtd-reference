using System;
using GUI_2;
using UnityEngine.Scripting;

// Token: 0x0200106C RID: 4204
[Preserve]
public class XUiC_SkillWindowGroup : XUiController
{
	// Token: 0x17000F9A RID: 3994
	// (get) Token: 0x060085AF RID: 34223 RVA: 0x0032FF7E File Offset: 0x0032E17E
	// (set) Token: 0x060085B0 RID: 34224 RVA: 0x0032FF86 File Offset: 0x0032E186
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

	// Token: 0x060085B1 RID: 34225 RVA: 0x0032FF90 File Offset: 0x0032E190
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

	// Token: 0x060085B2 RID: 34226 RVA: 0x0033003A File Offset: 0x0032E23A
	[PublicizedFrom(EAccessModifier.Private)]
	public void CategoryList_CategoryChanged(XUiC_CategoryEntry _categoryEntry)
	{
		this.skillList.Category = _categoryEntry.CategoryName;
		this.skillList.RefreshSkillList();
		this.skillList.SelectFirstEntry();
		this.IsDirty = true;
	}

	// Token: 0x060085B3 RID: 34227 RVA: 0x0033006A File Offset: 0x0032E26A
	[PublicizedFrom(EAccessModifier.Private)]
	public void CategoryList_CategoryClickChanged(XUiC_CategoryEntry _categoryEntry)
	{
		this.skillList.Category = _categoryEntry.CategoryName;
		this.skillList.SetFilterText("");
		this.skillList.RefreshSkillList();
		this.skillList.SelectFirstEntry();
		this.IsDirty = true;
	}

	// Token: 0x060085B4 RID: 34228 RVA: 0x0008A715 File Offset: 0x00088915
	[PublicizedFrom(EAccessModifier.Private)]
	public void XUiC_SkillEntry_OnPress(XUiController _sender, int _mouseButton)
	{
		this.IsDirty = true;
	}

	// Token: 0x060085B5 RID: 34229 RVA: 0x003300AA File Offset: 0x0032E2AA
	public override void OnClose()
	{
		base.OnClose();
		this.xui.playerUI.windowManager.Close("windowpaging");
		this.xui.CalloutWindow.DisableCallouts(XUiC_GamepadCalloutWindow.CalloutType.Menu);
	}

	// Token: 0x060085B6 RID: 34230 RVA: 0x003300E0 File Offset: 0x0032E2E0
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

	// Token: 0x060085B7 RID: 34231 RVA: 0x00330208 File Offset: 0x0032E408
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

	// Token: 0x04006363 RID: 25443
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SkillEntry[] skillEntries;

	// Token: 0x04006364 RID: 25444
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SkillList skillList;

	// Token: 0x04006365 RID: 25445
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CategoryList categoryList;

	// Token: 0x04006366 RID: 25446
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SkillAttributeInfoWindow skillAttributeInfoWindow;

	// Token: 0x04006367 RID: 25447
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SkillSkillInfoWindow skillSkillInfoWindow;

	// Token: 0x04006368 RID: 25448
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SkillPerkInfoWindow skillPerkInfoWindow;

	// Token: 0x04006369 RID: 25449
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SkillBookInfoWindow skillBookInfoWindow;

	// Token: 0x0400636A RID: 25450
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SkillCraftingInfoWindow skillCraftingInfoWindow;

	// Token: 0x0400636B RID: 25451
	[PublicizedFrom(EAccessModifier.Private)]
	public ProgressionValue currentSkill;
}
