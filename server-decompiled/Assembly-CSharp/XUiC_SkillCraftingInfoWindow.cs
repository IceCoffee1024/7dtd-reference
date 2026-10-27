using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine.Scripting;

// Token: 0x0200107E RID: 4222
[Preserve]
public class XUiC_SkillCraftingInfoWindow : XUiC_InfoWindow
{
	// Token: 0x17000FC3 RID: 4035
	// (get) Token: 0x060085AE RID: 34222 RVA: 0x00330496 File Offset: 0x0032E696
	// (set) Token: 0x060085AF RID: 34223 RVA: 0x0033049E File Offset: 0x0032E69E
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

	// Token: 0x17000FC4 RID: 4036
	// (get) Token: 0x060085B0 RID: 34224 RVA: 0x003304CF File Offset: 0x0032E6CF
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

	// Token: 0x17000FC5 RID: 4037
	// (get) Token: 0x060085B1 RID: 34225 RVA: 0x00330502 File Offset: 0x0032E702
	// (set) Token: 0x060085B2 RID: 34226 RVA: 0x0033050A File Offset: 0x0032E70A
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

	// Token: 0x17000FC6 RID: 4038
	// (get) Token: 0x060085B3 RID: 34227 RVA: 0x00330522 File Offset: 0x0032E722
	// (set) Token: 0x060085B4 RID: 34228 RVA: 0x0033052A File Offset: 0x0032E72A
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

	// Token: 0x17000FC7 RID: 4039
	// (get) Token: 0x060085B5 RID: 34229 RVA: 0x00330542 File Offset: 0x0032E742
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

	// Token: 0x060085B6 RID: 34230 RVA: 0x0033055C File Offset: 0x0032E75C
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

	// Token: 0x060085B7 RID: 34231 RVA: 0x003306A0 File Offset: 0x0032E8A0
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

	// Token: 0x060085B8 RID: 34232 RVA: 0x0033076C File Offset: 0x0032E96C
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

	// Token: 0x060085B9 RID: 34233 RVA: 0x003307DC File Offset: 0x0032E9DC
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

	// Token: 0x060085BA RID: 34234 RVA: 0x0033081C File Offset: 0x0032EA1C
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

	// Token: 0x060085BB RID: 34235 RVA: 0x003308A0 File Offset: 0x0032EAA0
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

	// Token: 0x060085BC RID: 34236 RVA: 0x0008ABC9 File Offset: 0x00088DC9
	[PublicizedFrom(EAccessModifier.Private)]
	public void Pager_OnPageChanged()
	{
		this.IsDirty = true;
	}

	// Token: 0x060085BD RID: 34237 RVA: 0x00330A08 File Offset: 0x0032EC08
	public override void OnOpen()
	{
		base.OnOpen();
		if (this.actionItemList != null)
		{
			this.actionItemList.SetCraftingActionList(XUiC_ItemActionList.ItemActionListTypes.Skill, this);
		}
		this.IsDirty = true;
	}

	// Token: 0x060085BE RID: 34238 RVA: 0x00330A2C File Offset: 0x0032EC2C
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

	// Token: 0x060085BF RID: 34239 RVA: 0x00330A50 File Offset: 0x0032EC50
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

	// Token: 0x060085C0 RID: 34240 RVA: 0x00330AEC File Offset: 0x0032ECEC
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

	// Token: 0x04006356 RID: 25430
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ItemActionList actionItemList;

	// Token: 0x04006357 RID: 25431
	[PublicizedFrom(EAccessModifier.Private)]
	public int hiddenEntriesWithPaging = 1;

	// Token: 0x04006358 RID: 25432
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<XUiC_SkillCraftingInfoEntry> levelEntries = new List<XUiC_SkillCraftingInfoEntry>();

	// Token: 0x04006359 RID: 25433
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_Paging pager;

	// Token: 0x0400635A RID: 25434
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SkillCraftingInfoEntry selectedEntry;

	// Token: 0x0400635B RID: 25435
	[PublicizedFrom(EAccessModifier.Private)]
	public int skillsPerPage;

	// Token: 0x0400635C RID: 25436
	[PublicizedFrom(EAccessModifier.Private)]
	public ProgressionClass.DisplayData hoveredLevel;

	// Token: 0x0400635D RID: 25437
	[PublicizedFrom(EAccessModifier.Private)]
	public ProgressionClass.DisplayData selectedLevel;

	// Token: 0x0400635E RID: 25438
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterFloat skillLevelFormatter = new CachedStringFormatterFloat(null);

	// Token: 0x0400635F RID: 25439
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterFloat maxSkillLevelFormatter = new CachedStringFormatterFloat(null);

	// Token: 0x04006360 RID: 25440
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string, float> attributeSetValueFormatter = new CachedStringFormatter<string, float>((string _s, float _f) => _s + ": " + _f.ToCultureInvariantString("0.#"));
}
