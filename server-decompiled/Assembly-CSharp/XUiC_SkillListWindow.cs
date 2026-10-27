using System;
using GUI_2;
using UnityEngine.Scripting;

// Token: 0x02001083 RID: 4227
[Preserve]
public class XUiC_SkillListWindow : XUiController
{
	// Token: 0x060085F6 RID: 34294 RVA: 0x00332B24 File Offset: 0x00330D24
	public override void Init()
	{
		base.Init();
		this.totalItems = Localization.Get("lblTotalItems", false, null);
		this.pointsAvailable = Localization.Get("xuiPointsAvailable", false, null);
		this.skillsTitle = Localization.Get("xuiSkills", false, null);
		this.booksTitle = Localization.Get("lblCategoryBooks", false, null);
		this.craftingTitle = Localization.Get("xuiCrafting", false, null);
		this.skillList = base.GetChildByType<XUiC_SkillList>();
		XUiController childByType = base.GetChildByType<XUiC_CategoryList>();
		if (childByType != null)
		{
			this.categoryList = (XUiC_CategoryList)childByType;
			this.categoryList.CategoryChanged += this.CategoryList_CategoryChanged;
		}
		this.skillList.CategoryList = this.categoryList;
		this.skillList.SkillListWindow = this;
	}

	// Token: 0x060085F7 RID: 34295 RVA: 0x00332BE7 File Offset: 0x00330DE7
	[PublicizedFrom(EAccessModifier.Private)]
	public void CategoryList_CategoryChanged(XUiC_CategoryEntry _categoryEntry)
	{
		this.categoryName = _categoryEntry.CategoryDisplayName;
		this.categoryIcon = _categoryEntry.SpriteName;
		this.count = this.skillList.GetActiveCount();
		base.RefreshBindings();
	}

	// Token: 0x060085F8 RID: 34296 RVA: 0x00332C18 File Offset: 0x00330E18
	public override void OnOpen()
	{
		base.OnOpen();
		this.xui.CalloutWindow.ClearCallouts(XUiC_GamepadCalloutWindow.CalloutType.MenuShortcuts);
		this.xui.CalloutWindow.AddCallout(UIUtils.ButtonIcon.FaceButtonNorth, "igcoSpendPoints", XUiC_GamepadCalloutWindow.CalloutType.MenuShortcuts);
		this.xui.CalloutWindow.EnableCallouts(XUiC_GamepadCalloutWindow.CalloutType.MenuShortcuts, 0f);
	}

	// Token: 0x060085F9 RID: 34297 RVA: 0x00332C69 File Offset: 0x00330E69
	public override void OnClose()
	{
		base.OnClose();
		this.xui.CalloutWindow.ClearCallouts(XUiC_GamepadCalloutWindow.CalloutType.MenuShortcuts);
	}

	// Token: 0x060085FA RID: 34298 RVA: 0x00332C84 File Offset: 0x00330E84
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string value, string bindingName)
	{
		uint num = <PrivateImplementationDetails>.ComputeStringHash(bindingName);
		if (num <= 2672683516U)
		{
			if (num <= 1237242696U)
			{
				if (num != 1050865026U)
				{
					if (num == 1237242696U)
					{
						if (bindingName == "isbook")
						{
							if (this.skillList != null)
							{
								value = (this.skillList.DisplayType == ProgressionClass.DisplayTypes.Book).ToString();
							}
							return true;
						}
					}
				}
				else if (bindingName == "isnormal")
				{
					if (this.skillList != null)
					{
						value = (this.skillList.DisplayType == ProgressionClass.DisplayTypes.Standard).ToString();
					}
					return true;
				}
			}
			else if (num != 1741938684U)
			{
				if (num == 2672683516U)
				{
					if (bindingName == "paging_visible")
					{
						if (this.skillList != null)
						{
							value = (this.skillList.PageCount > 0).ToString();
						}
						return true;
					}
				}
			}
			else if (bindingName == "categoryicon")
			{
				value = this.categoryIcon;
				return true;
			}
		}
		else if (num <= 3877939383U)
		{
			if (num != 3822843618U)
			{
				if (num == 3877939383U)
				{
					if (bindingName == "totalskills")
					{
						value = "";
						if (this.skillList != null)
						{
							value = this.totalSkillsFormatter.Format(this.totalItems, this.skillList.GetActiveCount());
						}
						return true;
					}
				}
			}
			else if (bindingName == "titlename")
			{
				value = "";
				if (this.skillList != null)
				{
					switch (this.skillList.DisplayType)
					{
					case ProgressionClass.DisplayTypes.Standard:
						value = this.skillsTitle;
						break;
					case ProgressionClass.DisplayTypes.Book:
						value = this.booksTitle;
						break;
					case ProgressionClass.DisplayTypes.Crafting:
						value = this.craftingTitle;
						break;
					}
				}
				return true;
			}
		}
		else if (num != 3983894959U)
		{
			if (num == 4035727244U)
			{
				if (bindingName == "skillpointsavailable")
				{
					string text = this.pointsAvailable;
					EntityPlayerLocal entityPlayer = this.xui.playerUI.entityPlayer;
					if (XUi.IsGameRunning() && entityPlayer != null)
					{
						value = this.skillPointsAvailableFormatter.Format(text, entityPlayer.Progression.SkillPoints);
					}
					return true;
				}
			}
		}
		else if (bindingName == "iscrafting")
		{
			if (this.skillList != null)
			{
				value = (this.skillList.DisplayType == ProgressionClass.DisplayTypes.Crafting).ToString();
			}
			return true;
		}
		return base.GetBindingValueInternal(ref value, bindingName);
	}

	// Token: 0x04006385 RID: 25477
	[PublicizedFrom(EAccessModifier.Private)]
	public string totalItems = "";

	// Token: 0x04006386 RID: 25478
	[PublicizedFrom(EAccessModifier.Private)]
	public int count;

	// Token: 0x04006387 RID: 25479
	[PublicizedFrom(EAccessModifier.Private)]
	public string categoryName = "Intellect";

	// Token: 0x04006388 RID: 25480
	[PublicizedFrom(EAccessModifier.Private)]
	public string categoryIcon = "";

	// Token: 0x04006389 RID: 25481
	[PublicizedFrom(EAccessModifier.Private)]
	public string pointsAvailable;

	// Token: 0x0400638A RID: 25482
	[PublicizedFrom(EAccessModifier.Private)]
	public string skillsTitle = "";

	// Token: 0x0400638B RID: 25483
	[PublicizedFrom(EAccessModifier.Private)]
	public string booksTitle = "";

	// Token: 0x0400638C RID: 25484
	[PublicizedFrom(EAccessModifier.Private)]
	public string craftingTitle;

	// Token: 0x0400638D RID: 25485
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CategoryList categoryList;

	// Token: 0x0400638E RID: 25486
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SkillList skillList;

	// Token: 0x0400638F RID: 25487
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string, int> totalSkillsFormatter = new CachedStringFormatter<string, int>((string _s, int _i) => string.Format(_s, _i));

	// Token: 0x04006390 RID: 25488
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string, int> skillPointsAvailableFormatter = new CachedStringFormatter<string, int>((string _s, int _i) => string.Format("{0} {1}", _s, _i));
}
