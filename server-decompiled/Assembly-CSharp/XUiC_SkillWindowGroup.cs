using System;
using GUI_2;
using UnityEngine.Scripting;

// Token: 0x0200108D RID: 4237
[Preserve]
public class XUiC_SkillWindowGroup : XUiController
{
	// Token: 0x17000FDB RID: 4059
	// (get) Token: 0x06008648 RID: 34376 RVA: 0x00335C0E File Offset: 0x00333E0E
	// (set) Token: 0x06008649 RID: 34377 RVA: 0x00335C16 File Offset: 0x00333E16
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

	// Token: 0x0600864A RID: 34378 RVA: 0x00335C20 File Offset: 0x00333E20
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

	// Token: 0x0600864B RID: 34379 RVA: 0x00335CCA File Offset: 0x00333ECA
	[PublicizedFrom(EAccessModifier.Private)]
	public void CategoryList_CategoryChanged(XUiC_CategoryEntry _categoryEntry)
	{
		this.skillList.Category = _categoryEntry.CategoryName;
		this.skillList.RefreshSkillList();
		this.skillList.SelectFirstEntry();
		this.IsDirty = true;
	}

	// Token: 0x0600864C RID: 34380 RVA: 0x00335CFA File Offset: 0x00333EFA
	[PublicizedFrom(EAccessModifier.Private)]
	public void CategoryList_CategoryClickChanged(XUiC_CategoryEntry _categoryEntry)
	{
		this.skillList.Category = _categoryEntry.CategoryName;
		this.skillList.SetFilterText("");
		this.skillList.RefreshSkillList();
		this.skillList.SelectFirstEntry();
		this.IsDirty = true;
	}

	// Token: 0x0600864D RID: 34381 RVA: 0x0008ABC9 File Offset: 0x00088DC9
	[PublicizedFrom(EAccessModifier.Private)]
	public void XUiC_SkillEntry_OnPress(XUiController _sender, int _mouseButton)
	{
		this.IsDirty = true;
	}

	// Token: 0x0600864E RID: 34382 RVA: 0x00335D3A File Offset: 0x00333F3A
	public override void OnClose()
	{
		base.OnClose();
		this.xui.playerUI.windowManager.Close("windowpaging");
		this.xui.CalloutWindow.DisableCallouts(XUiC_GamepadCalloutWindow.CalloutType.Menu);
	}

	// Token: 0x0600864F RID: 34383 RVA: 0x00335D70 File Offset: 0x00333F70
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

	// Token: 0x06008650 RID: 34384 RVA: 0x00335E98 File Offset: 0x00334098
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

	// Token: 0x040063E7 RID: 25575
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SkillEntry[] skillEntries;

	// Token: 0x040063E8 RID: 25576
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SkillList skillList;

	// Token: 0x040063E9 RID: 25577
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CategoryList categoryList;

	// Token: 0x040063EA RID: 25578
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SkillAttributeInfoWindow skillAttributeInfoWindow;

	// Token: 0x040063EB RID: 25579
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SkillSkillInfoWindow skillSkillInfoWindow;

	// Token: 0x040063EC RID: 25580
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SkillPerkInfoWindow skillPerkInfoWindow;

	// Token: 0x040063ED RID: 25581
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SkillBookInfoWindow skillBookInfoWindow;

	// Token: 0x040063EE RID: 25582
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SkillCraftingInfoWindow skillCraftingInfoWindow;

	// Token: 0x040063EF RID: 25583
	[PublicizedFrom(EAccessModifier.Private)]
	public ProgressionValue currentSkill;
}
