using System;
using GUI_2;
using UnityEngine.Scripting;

// Token: 0x02001073 RID: 4211
[Preserve]
public class XUiC_SkillListWindow : XUiController
{
	// Token: 0x060085A4 RID: 34212 RVA: 0x00330D10 File Offset: 0x0032EF10
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

	// Token: 0x060085A5 RID: 34213 RVA: 0x00330DD3 File Offset: 0x0032EFD3
	[PublicizedFrom(EAccessModifier.Private)]
	public void CategoryList_CategoryChanged(XUiC_CategoryEntry _categoryEntry)
	{
		this.categoryName = _categoryEntry.CategoryDisplayName;
		this.categoryIcon = _categoryEntry.SpriteName;
		this.count = this.skillList.GetActiveCount();
		base.RefreshBindings();
	}

	// Token: 0x060085A6 RID: 34214 RVA: 0x00330E04 File Offset: 0x0032F004
	public override void OnOpen()
	{
		base.OnOpen();
		this.xui.CalloutWindow.ClearCallouts(XUiC_GamepadCalloutWindow.CalloutType.MenuShortcuts);
		this.xui.CalloutWindow.AddCallout(UIUtils.ButtonIcon.FaceButtonNorth, "igcoSpendPoints", XUiC_GamepadCalloutWindow.CalloutType.MenuShortcuts);
		this.xui.CalloutWindow.EnableCallouts(XUiC_GamepadCalloutWindow.CalloutType.MenuShortcuts, 0f);
	}

	// Token: 0x060085A7 RID: 34215 RVA: 0x00330E55 File Offset: 0x0032F055
	public override void OnClose()
	{
		base.OnClose();
		this.xui.CalloutWindow.ClearCallouts(XUiC_GamepadCalloutWindow.CalloutType.MenuShortcuts);
	}

	// Token: 0x060085A8 RID: 34216 RVA: 0x00330E70 File Offset: 0x0032F070
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

	// Token: 0x04006347 RID: 25415
	[PublicizedFrom(EAccessModifier.Private)]
	public string totalItems = "";

	// Token: 0x04006348 RID: 25416
	[PublicizedFrom(EAccessModifier.Private)]
	public int count;

	// Token: 0x04006349 RID: 25417
	[PublicizedFrom(EAccessModifier.Private)]
	public string categoryName = "Intellect";

	// Token: 0x0400634A RID: 25418
	[PublicizedFrom(EAccessModifier.Private)]
	public string categoryIcon = "";

	// Token: 0x0400634B RID: 25419
	[PublicizedFrom(EAccessModifier.Private)]
	public string pointsAvailable;

	// Token: 0x0400634C RID: 25420
	[PublicizedFrom(EAccessModifier.Private)]
	public string skillsTitle = "";

	// Token: 0x0400634D RID: 25421
	[PublicizedFrom(EAccessModifier.Private)]
	public string booksTitle = "";

	// Token: 0x0400634E RID: 25422
	[PublicizedFrom(EAccessModifier.Private)]
	public string craftingTitle;

	// Token: 0x0400634F RID: 25423
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CategoryList categoryList;

	// Token: 0x04006350 RID: 25424
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SkillList skillList;

	// Token: 0x04006351 RID: 25425
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string, int> totalSkillsFormatter = new CachedStringFormatter<string, int>((string _s, int _i) => string.Format(_s, _i));

	// Token: 0x04006352 RID: 25426
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string, int> skillPointsAvailableFormatter = new CachedStringFormatter<string, int>((string _s, int _i) => string.Format("{0} {1}", _s, _i));
}
