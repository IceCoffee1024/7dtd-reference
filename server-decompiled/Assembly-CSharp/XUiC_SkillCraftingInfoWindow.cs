using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine.Scripting;

// Token: 0x02001044 RID: 4164
[Preserve]
public class XUiC_SkillCraftingInfoWindow : XUiC_InfoWindow
{
	// Token: 0x17000F49 RID: 3913
	// (get) Token: 0x06008424 RID: 33828 RVA: 0x0032624E File Offset: 0x0032444E
	// (set) Token: 0x06008425 RID: 33829 RVA: 0x00326256 File Offset: 0x00324456
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

	// Token: 0x17000F4A RID: 3914
	// (get) Token: 0x06008426 RID: 33830 RVA: 0x00326287 File Offset: 0x00324487
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

	// Token: 0x17000F4B RID: 3915
	// (get) Token: 0x06008427 RID: 33831 RVA: 0x003262BA File Offset: 0x003244BA
	// (set) Token: 0x06008428 RID: 33832 RVA: 0x003262C2 File Offset: 0x003244C2
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

	// Token: 0x17000F4C RID: 3916
	// (get) Token: 0x06008429 RID: 33833 RVA: 0x003262DA File Offset: 0x003244DA
	// (set) Token: 0x0600842A RID: 33834 RVA: 0x003262E2 File Offset: 0x003244E2
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

	// Token: 0x17000F4D RID: 3917
	// (get) Token: 0x0600842B RID: 33835 RVA: 0x003262FA File Offset: 0x003244FA
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

	// Token: 0x0600842C RID: 33836 RVA: 0x00326314 File Offset: 0x00324514
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

	// Token: 0x0600842D RID: 33837 RVA: 0x00326458 File Offset: 0x00324658
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

	// Token: 0x0600842E RID: 33838 RVA: 0x00326524 File Offset: 0x00324724
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

	// Token: 0x0600842F RID: 33839 RVA: 0x00326594 File Offset: 0x00324794
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

	// Token: 0x06008430 RID: 33840 RVA: 0x003265D4 File Offset: 0x003247D4
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

	// Token: 0x06008431 RID: 33841 RVA: 0x00326658 File Offset: 0x00324858
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

	// Token: 0x06008432 RID: 33842 RVA: 0x0008A069 File Offset: 0x00088269
	[PublicizedFrom(EAccessModifier.Private)]
	public void Pager_OnPageChanged()
	{
		this.IsDirty = true;
	}

	// Token: 0x06008433 RID: 33843 RVA: 0x003267C0 File Offset: 0x003249C0
	public override void OnOpen()
	{
		base.OnOpen();
		if (this.actionItemList != null)
		{
			this.actionItemList.SetCraftingActionList(XUiC_ItemActionList.ItemActionListTypes.Skill, this);
		}
		this.IsDirty = true;
	}

	// Token: 0x06008434 RID: 33844 RVA: 0x003267E4 File Offset: 0x003249E4
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

	// Token: 0x06008435 RID: 33845 RVA: 0x00326808 File Offset: 0x00324A08
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

	// Token: 0x06008436 RID: 33846 RVA: 0x003268A4 File Offset: 0x00324AA4
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

	// Token: 0x0400620C RID: 25100
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ItemActionList actionItemList;

	// Token: 0x0400620D RID: 25101
	[PublicizedFrom(EAccessModifier.Private)]
	public int hiddenEntriesWithPaging = 1;

	// Token: 0x0400620E RID: 25102
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<XUiC_SkillCraftingInfoEntry> levelEntries = new List<XUiC_SkillCraftingInfoEntry>();

	// Token: 0x0400620F RID: 25103
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_Paging pager;

	// Token: 0x04006210 RID: 25104
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SkillCraftingInfoEntry selectedEntry;

	// Token: 0x04006211 RID: 25105
	[PublicizedFrom(EAccessModifier.Private)]
	public int skillsPerPage;

	// Token: 0x04006212 RID: 25106
	[PublicizedFrom(EAccessModifier.Private)]
	public ProgressionClass.DisplayData hoveredLevel;

	// Token: 0x04006213 RID: 25107
	[PublicizedFrom(EAccessModifier.Private)]
	public ProgressionClass.DisplayData selectedLevel;

	// Token: 0x04006214 RID: 25108
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterFloat skillLevelFormatter = new CachedStringFormatterFloat(null);

	// Token: 0x04006215 RID: 25109
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterFloat maxSkillLevelFormatter = new CachedStringFormatterFloat(null);

	// Token: 0x04006216 RID: 25110
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string, float> attributeSetValueFormatter = new CachedStringFormatter<string, float>((string _s, float _f) => _s + ": " + _f.ToCultureInvariantString("0.#"));
}
