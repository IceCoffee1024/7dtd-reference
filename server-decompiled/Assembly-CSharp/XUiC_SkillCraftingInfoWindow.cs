using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine.Scripting;

// Token: 0x0200105D RID: 4189
[Preserve]
public class XUiC_SkillCraftingInfoWindow : XUiC_InfoWindow
{
	// Token: 0x17000F82 RID: 3970
	// (get) Token: 0x06008516 RID: 34070 RVA: 0x0032AA76 File Offset: 0x00328C76
	// (set) Token: 0x06008517 RID: 34071 RVA: 0x0032AA7E File Offset: 0x00328C7E
	public XUiC_SkillCraftingInfoEntry SelectedEntry
	{
		get
		{
			return this.selectedEntry;
		}
		set
		{
			if (this.selectedEntry != null)
			{
				this.selectedEntry.IsSelected = false;
			}
			this.selectedEntry = value;
			if (this.selectedEntry != null)
			{
				this.selectedEntry.IsSelected = true;
			}
		}
	}

	// Token: 0x17000F83 RID: 3971
	// (get) Token: 0x06008518 RID: 34072 RVA: 0x0032AAAF File Offset: 0x00328CAF
	public ProgressionValue CurrentSkill
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			if (this.xui.SelectedSkill == null || !this.xui.SelectedSkill.ProgressionClass.IsCrafting)
			{
				return null;
			}
			return this.xui.SelectedSkill;
		}
	}

	// Token: 0x17000F84 RID: 3972
	// (get) Token: 0x06008519 RID: 34073 RVA: 0x0032AAE2 File Offset: 0x00328CE2
	// (set) Token: 0x0600851A RID: 34074 RVA: 0x0032AAEA File Offset: 0x00328CEA
	public ProgressionClass.DisplayData HoveredData
	{
		get
		{
			return this.hoveredLevel;
		}
		set
		{
			if (this.hoveredLevel != value)
			{
				this.hoveredLevel = value;
				base.RefreshBindings();
			}
		}
	}

	// Token: 0x17000F85 RID: 3973
	// (get) Token: 0x0600851B RID: 34075 RVA: 0x0032AB02 File Offset: 0x00328D02
	// (set) Token: 0x0600851C RID: 34076 RVA: 0x0032AB0A File Offset: 0x00328D0A
	public ProgressionClass.DisplayData SelectedData
	{
		get
		{
			return this.selectedLevel;
		}
		set
		{
			if (this.selectedLevel != value)
			{
				this.selectedLevel = value;
				base.RefreshBindings();
			}
		}
	}

	// Token: 0x17000F86 RID: 3974
	// (get) Token: 0x0600851D RID: 34077 RVA: 0x0032AB22 File Offset: 0x00328D22
	public ProgressionClass.DisplayData CurrentData
	{
		get
		{
			if (this.hoveredLevel != null)
			{
				return this.hoveredLevel;
			}
			return this.selectedLevel;
		}
	}

	// Token: 0x0600851E RID: 34078 RVA: 0x0032AB3C File Offset: 0x00328D3C
	public override void Init()
	{
		base.Init();
		base.GetChildrenByType<XUiC_SkillCraftingInfoEntry>(this.levelEntries);
		int num = 1;
		foreach (XUiC_SkillCraftingInfoEntry xuiC_SkillCraftingInfoEntry in this.levelEntries)
		{
			xuiC_SkillCraftingInfoEntry.ListIndex = num - 1;
			xuiC_SkillCraftingInfoEntry.Data = null;
			xuiC_SkillCraftingInfoEntry.HiddenEntriesWithPaging = this.hiddenEntriesWithPaging;
			xuiC_SkillCraftingInfoEntry.MaxEntriesWithoutPaging = this.levelEntries.Count;
			xuiC_SkillCraftingInfoEntry.OnHover += this.Entry_OnHover;
			xuiC_SkillCraftingInfoEntry.OnPress += this.Entry_OnPress;
		}
		for (int i = 0; i < 14; i++)
		{
			XUiController childById = base.GetChildById(string.Format("itemIcon{0}", i + 1));
			childById.CustomData = i;
			childById.OnPress += this.Image_OnPress;
		}
		this.actionItemList = base.GetChildByType<XUiC_ItemActionList>();
		this.skillsPerPage = this.levelEntries.Count - this.hiddenEntriesWithPaging;
		this.pager = base.GetChildByType<XUiC_Paging>();
		if (this.pager != null)
		{
			this.pager.OnPageChanged += this.Pager_OnPageChanged;
		}
	}

	// Token: 0x0600851F RID: 34079 RVA: 0x0032AC80 File Offset: 0x00328E80
	[PublicizedFrom(EAccessModifier.Private)]
	public void Image_OnPress(XUiController _sender, int _mouseButton)
	{
		int num = (int)_sender.CustomData;
		XUi xui = _sender.xui;
		if (this.CurrentData == null || this.CurrentData.GetUnlockItemRecipes(num) == null)
		{
			return;
		}
		xui.playerUI.windowManager.Close(XUiC_LootWindowGroup.ID);
		List<XUiC_RecipeList> childrenByType = xui.GetChildrenByType<XUiC_RecipeList>();
		XUiC_RecipeList xuiC_RecipeList = null;
		for (int i = 0; i < childrenByType.Count; i++)
		{
			if (childrenByType[i].WindowGroup != null && childrenByType[i].WindowGroup.isShowing)
			{
				xuiC_RecipeList = childrenByType[i];
				break;
			}
		}
		if (xuiC_RecipeList == null)
		{
			XUiC_WindowSelector.OpenSelectorAndWindow(xui.playerUI.entityPlayer, "crafting");
			xuiC_RecipeList = xui.GetChildByType<XUiC_RecipeList>();
		}
		if (xuiC_RecipeList != null)
		{
			xuiC_RecipeList.SetRecipeDataByItems(this.CurrentData.GetUnlockItemRecipes(num));
		}
	}

	// Token: 0x06008520 RID: 34080 RVA: 0x0032AD4C File Offset: 0x00328F4C
	[PublicizedFrom(EAccessModifier.Private)]
	public void Entry_OnPress(XUiController _sender, int _mouseButton)
	{
		XUiC_SkillCraftingInfoEntry xuiC_SkillCraftingInfoEntry = _sender as XUiC_SkillCraftingInfoEntry;
		if (xuiC_SkillCraftingInfoEntry == null)
		{
			xuiC_SkillCraftingInfoEntry = _sender.Parent as XUiC_SkillCraftingInfoEntry;
		}
		if (xuiC_SkillCraftingInfoEntry == null)
		{
			this.SelectedEntry = null;
			this.SelectedData = null;
			return;
		}
		if (this.SelectedData != xuiC_SkillCraftingInfoEntry.Data && xuiC_SkillCraftingInfoEntry.Data != null)
		{
			this.SelectedEntry = xuiC_SkillCraftingInfoEntry;
			this.SelectedData = xuiC_SkillCraftingInfoEntry.Data;
			return;
		}
		this.SelectedEntry = null;
		this.SelectedData = null;
	}

	// Token: 0x06008521 RID: 34081 RVA: 0x0032ADBC File Offset: 0x00328FBC
	[PublicizedFrom(EAccessModifier.Private)]
	public void Entry_OnHover(XUiController _sender, bool _isOver)
	{
		XUiC_SkillCraftingInfoEntry xuiC_SkillCraftingInfoEntry = _sender as XUiC_SkillCraftingInfoEntry;
		if (xuiC_SkillCraftingInfoEntry == null)
		{
			xuiC_SkillCraftingInfoEntry = _sender.Parent as XUiC_SkillCraftingInfoEntry;
		}
		if (_isOver && xuiC_SkillCraftingInfoEntry != null)
		{
			this.HoveredData = xuiC_SkillCraftingInfoEntry.Data;
			return;
		}
		this.HoveredData = null;
	}

	// Token: 0x06008522 RID: 34082 RVA: 0x0032ADFC File Offset: 0x00328FFC
	public void SkillChanged()
	{
		XUiC_Paging xuiC_Paging = this.pager;
		if (xuiC_Paging != null)
		{
			xuiC_Paging.SetLastPageByElementsAndPageLength((this.CurrentSkill != null && this.CurrentSkill.ProgressionClass.MaxLevel > this.levelEntries.Count) ? (this.CurrentSkill.ProgressionClass.MaxLevel - 1) : 0, this.skillsPerPage);
		}
		XUiC_Paging xuiC_Paging2 = this.pager;
		if (xuiC_Paging2 != null)
		{
			xuiC_Paging2.Reset();
		}
		this.IsDirty = true;
		this.SelectedData = null;
		this.SelectedEntry = null;
	}

	// Token: 0x06008523 RID: 34083 RVA: 0x0032AE80 File Offset: 0x00329080
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateSkill()
	{
		if (this.CurrentSkill != null && this.actionItemList != null)
		{
			this.actionItemList.SetCraftingActionList(XUiC_ItemActionList.ItemActionListTypes.Skill, this);
		}
		XUiC_Paging xuiC_Paging = this.pager;
		int num = ((xuiC_Paging != null) ? xuiC_Paging.GetPage() : 0) * this.skillsPerPage;
		ProgressionClass progressionClass = ((this.CurrentSkill != null) ? this.CurrentSkill.ProgressionClass : null);
		if (progressionClass != null && progressionClass.DisplayDataList != null)
		{
			XUiC_SkillEntry entryForSkill = this.windowGroup.Controller.GetChildByType<XUiC_SkillList>().GetEntryForSkill(this.CurrentSkill);
			using (List<XUiC_SkillCraftingInfoEntry>.Enumerator enumerator = this.levelEntries.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					XUiC_SkillCraftingInfoEntry xuiC_SkillCraftingInfoEntry = enumerator.Current;
					ProgressionClass.DisplayData displayData = ((progressionClass.DisplayDataList.Count > num) ? progressionClass.DisplayDataList[num] : null);
					xuiC_SkillCraftingInfoEntry.Data = ((displayData != null && displayData.Enabled) ? displayData : null);
					xuiC_SkillCraftingInfoEntry.IsDirty = true;
					if (entryForSkill != null)
					{
						xuiC_SkillCraftingInfoEntry.ViewComponent.NavLeftTarget = entryForSkill.ViewComponent;
					}
					num++;
				}
				return;
			}
		}
		foreach (XUiC_SkillCraftingInfoEntry xuiC_SkillCraftingInfoEntry2 in this.levelEntries)
		{
			xuiC_SkillCraftingInfoEntry2.Data = null;
			xuiC_SkillCraftingInfoEntry2.IsDirty = true;
		}
	}

	// Token: 0x06008524 RID: 34084 RVA: 0x0008A715 File Offset: 0x00088915
	[PublicizedFrom(EAccessModifier.Private)]
	public void Pager_OnPageChanged()
	{
		this.IsDirty = true;
	}

	// Token: 0x06008525 RID: 34085 RVA: 0x0032AFE8 File Offset: 0x003291E8
	public override void OnOpen()
	{
		base.OnOpen();
		if (this.actionItemList != null)
		{
			this.actionItemList.SetCraftingActionList(XUiC_ItemActionList.ItemActionListTypes.Skill, this);
		}
		this.IsDirty = true;
	}

	// Token: 0x06008526 RID: 34086 RVA: 0x0032B00C File Offset: 0x0032920C
	public override void Update(float _dt)
	{
		if (this.IsDirty)
		{
			this.IsDirty = false;
			this.UpdateSkill();
			base.RefreshBindings();
		}
		base.Update(_dt);
	}

	// Token: 0x06008527 RID: 34087 RVA: 0x0032B030 File Offset: 0x00329230
	public override bool ParseAttribute(string _name, string _value)
	{
		if (_name == "hidden_entries_with_paging")
		{
			this.hiddenEntriesWithPaging = StringParsers.ParseSInt32(_value, 0, -1, NumberStyles.Integer);
			foreach (XUiC_SkillCraftingInfoEntry xuiC_SkillCraftingInfoEntry in this.levelEntries)
			{
				if (xuiC_SkillCraftingInfoEntry != null)
				{
					xuiC_SkillCraftingInfoEntry.HiddenEntriesWithPaging = this.hiddenEntriesWithPaging;
				}
			}
			this.skillsPerPage = this.levelEntries.Count - this.hiddenEntriesWithPaging;
			return true;
		}
		return base.ParseAttribute(_name, _value);
	}

	// Token: 0x06008528 RID: 34088 RVA: 0x0032B0CC File Offset: 0x003292CC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string _value, string _bindingName)
	{
		EntityPlayerLocal entityPlayer = this.xui.playerUI.entityPlayer;
		uint num = <PrivateImplementationDetails>.ComputeStringHash(_bindingName);
		if (num <= 1283949528U)
		{
			if (num <= 464048759U)
			{
				if (num != 443815844U)
				{
					if (num == 464048759U)
					{
						if (_bindingName == "alwaysfalse")
						{
							_value = "false";
							return true;
						}
					}
				}
				else if (_bindingName == "skillLevel")
				{
					_value = ((this.CurrentSkill != null) ? this.skillLevelFormatter.Format(this.CurrentSkill.GetCalculatedLevel(entityPlayer)) : "0");
					return true;
				}
			}
			else if (num != 1275709072U)
			{
				if (num == 1283949528U)
				{
					if (_bindingName == "currentlevel")
					{
						_value = Localization.Get("xuiSkillLevel", false, null);
						return true;
					}
				}
			}
			else if (_bindingName == "maxSkillLevel")
			{
				_value = ((this.CurrentSkill != null) ? this.maxSkillLevelFormatter.Format((float)ProgressionClass.GetCalculatedMaxLevel(entityPlayer, this.CurrentSkill)) : "0");
				return true;
			}
		}
		else if (num <= 3268933568U)
		{
			if (num != 2606420134U)
			{
				if (num == 3268933568U)
				{
					if (_bindingName == "showPaging")
					{
						_value = "false";
						return true;
					}
				}
			}
			else if (_bindingName == "groupdescription")
			{
				_value = ((this.CurrentSkill != null) ? Localization.Get(this.CurrentSkill.ProgressionClass.DescKey, false, null) : "");
				return true;
			}
		}
		else if (num != 3504806855U)
		{
			if (num != 4010384093U)
			{
				if (num == 4294521801U)
				{
					if (_bindingName == "detailsdescription")
					{
						_value = "";
						return true;
					}
				}
			}
			else if (_bindingName == "groupicon")
			{
				_value = ((this.CurrentSkill != null) ? this.CurrentSkill.ProgressionClass.Icon : "ui_game_symbol_skills");
				return true;
			}
		}
		else if (_bindingName == "groupname")
		{
			_value = ((this.CurrentSkill != null) ? Localization.Get(this.CurrentSkill.ProgressionClass.NameKey, false, null) : "Skill Info");
			return true;
		}
		if (_bindingName.StartsWith("unlock_icon_atlas"))
		{
			if (this.CurrentData != null)
			{
				int num2 = StringParsers.ParseSInt32(_bindingName.Replace("unlock_icon_atlas", ""), 0, -1, NumberStyles.Integer) - 1;
				_value = this.CurrentData.GetUnlockItemIconAtlas(entityPlayer, num2);
			}
			else
			{
				_value = "ItemIconAtlas";
			}
			return true;
		}
		if (_bindingName.StartsWith("unlock_icon_locked"))
		{
			if (this.CurrentData != null)
			{
				int num3 = StringParsers.ParseSInt32(_bindingName.Replace("unlock_icon_locked", ""), 0, -1, NumberStyles.Integer) - 1;
				_value = this.CurrentData.GetUnlockItemLocked(entityPlayer, num3).ToString();
			}
			else
			{
				_value = "false";
			}
			return true;
		}
		if (_bindingName.StartsWith("unlock_icon_tooltip"))
		{
			if (this.CurrentData != null)
			{
				int num4 = StringParsers.ParseSInt32(_bindingName.Replace("unlock_icon_tooltip", ""), 0, -1, NumberStyles.Integer) - 1;
				_value = this.CurrentData.GetUnlockItemName(num4);
			}
			else
			{
				_value = "";
			}
			return true;
		}
		if (_bindingName.StartsWith("unlock_icon"))
		{
			if (this.CurrentData != null)
			{
				int num5 = StringParsers.ParseSInt32(_bindingName.Replace("unlock_icon", ""), 0, -1, NumberStyles.Integer) - 1;
				_value = this.CurrentData.GetUnlockItemIconName(num5);
			}
			else
			{
				_value = "";
			}
			return true;
		}
		return base.GetBindingValueInternal(ref _value, _bindingName);
	}

	// Token: 0x040062D3 RID: 25299
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ItemActionList actionItemList;

	// Token: 0x040062D4 RID: 25300
	[PublicizedFrom(EAccessModifier.Private)]
	public int hiddenEntriesWithPaging = 1;

	// Token: 0x040062D5 RID: 25301
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<XUiC_SkillCraftingInfoEntry> levelEntries = new List<XUiC_SkillCraftingInfoEntry>();

	// Token: 0x040062D6 RID: 25302
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_Paging pager;

	// Token: 0x040062D7 RID: 25303
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SkillCraftingInfoEntry selectedEntry;

	// Token: 0x040062D8 RID: 25304
	[PublicizedFrom(EAccessModifier.Private)]
	public int skillsPerPage;

	// Token: 0x040062D9 RID: 25305
	[PublicizedFrom(EAccessModifier.Private)]
	public ProgressionClass.DisplayData hoveredLevel;

	// Token: 0x040062DA RID: 25306
	[PublicizedFrom(EAccessModifier.Private)]
	public ProgressionClass.DisplayData selectedLevel;

	// Token: 0x040062DB RID: 25307
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterFloat skillLevelFormatter = new CachedStringFormatterFloat(null);

	// Token: 0x040062DC RID: 25308
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterFloat maxSkillLevelFormatter = new CachedStringFormatterFloat(null);

	// Token: 0x040062DD RID: 25309
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string, float> attributeSetValueFormatter = new CachedStringFormatter<string, float>((string _s, float _f) => _s + ": " + _f.ToCultureInvariantString("0.#"));
}
