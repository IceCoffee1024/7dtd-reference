using System;
using System.Collections.Generic;
using Challenges;
using Twitch;
using UnityEngine.Scripting;

// Token: 0x02000EC0 RID: 3776
[Preserve]
public class XUiC_CategoryList : XUiController
{
	// Token: 0x140000CD RID: 205
	// (add) Token: 0x06007658 RID: 30296 RVA: 0x002C6E54 File Offset: 0x002C5054
	// (remove) Token: 0x06007659 RID: 30297 RVA: 0x002C6E8C File Offset: 0x002C508C
	public event XUiEvent_CategoryChangedEventHandler CategoryChanged;

	// Token: 0x140000CE RID: 206
	// (add) Token: 0x0600765A RID: 30298 RVA: 0x002C6EC4 File Offset: 0x002C50C4
	// (remove) Token: 0x0600765B RID: 30299 RVA: 0x002C6EFC File Offset: 0x002C50FC
	public event XUiEvent_CategoryChangedEventHandler CategoryClickChanged;

	// Token: 0x17000DE7 RID: 3559
	// (get) Token: 0x0600765C RID: 30300 RVA: 0x002C6F31 File Offset: 0x002C5131
	// (set) Token: 0x0600765D RID: 30301 RVA: 0x002C6F3C File Offset: 0x002C513C
	public XUiC_CategoryEntry CurrentCategory
	{
		get
		{
			return this.currentCategory;
		}
		set
		{
			if (this.currentCategory != null)
			{
				this.currentCategory.Selected = false;
			}
			this.currentCategory = value;
			if (this.currentCategory != null)
			{
				this.currentCategory.Selected = true;
				this.currentIndex = this.CategoryButtons.IndexOf(this.currentCategory);
			}
			this.IsDirty = true;
		}
	}

	// Token: 0x17000DE8 RID: 3560
	// (get) Token: 0x0600765E RID: 30302 RVA: 0x002C6F96 File Offset: 0x002C5196
	public int MaxCategories
	{
		get
		{
			return this.CategoryButtons.Count;
		}
	}

	// Token: 0x0600765F RID: 30303 RVA: 0x002C6FA4 File Offset: 0x002C51A4
	public override void Init()
	{
		base.Init();
		base.GetChildrenByType<XUiC_CategoryEntry>(this.CategoryButtons);
		for (int i = 0; i < this.CategoryButtons.Count; i++)
		{
			this.CategoryButtons[i].CategoryList = this;
		}
	}

	// Token: 0x06007660 RID: 30304 RVA: 0x002C6FEC File Offset: 0x002C51EC
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			base.RefreshBindings();
			this.IsDirty = false;
		}
		if (!this.AllowKeyPaging || !this.xui.playerUI.windowManager.IsKeyShortcutsAllowed())
		{
			return;
		}
		PlayerActionsGUI guiactions = this.xui.playerUI.playerInput.GUIActions;
		if (guiactions.PageUp.WasReleased)
		{
			this.IncrementCategory(1);
			return;
		}
		if (guiactions.PageDown.WasReleased)
		{
			this.IncrementCategory(-1);
		}
	}

	// Token: 0x06007661 RID: 30305 RVA: 0x002C7074 File Offset: 0x002C5274
	[PublicizedFrom(EAccessModifier.Internal)]
	public void HandleCategoryChanged()
	{
		XUiEvent_CategoryChangedEventHandler categoryChanged = this.CategoryChanged;
		if (categoryChanged != null)
		{
			categoryChanged(this.CurrentCategory);
		}
		XUiEvent_CategoryChangedEventHandler categoryClickChanged = this.CategoryClickChanged;
		if (categoryClickChanged == null)
		{
			return;
		}
		categoryClickChanged(this.CurrentCategory);
	}

	// Token: 0x06007662 RID: 30306 RVA: 0x002C70A4 File Offset: 0x002C52A4
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CategoryEntry GetCategoryByName(string _category, out int _index)
	{
		_index = 0;
		for (int i = 0; i < this.CategoryButtons.Count; i++)
		{
			if (this.CategoryButtons[i].CategoryName == _category)
			{
				_index = i;
				return this.CategoryButtons[i];
			}
		}
		return null;
	}

	// Token: 0x06007663 RID: 30307 RVA: 0x002C70F4 File Offset: 0x002C52F4
	public XUiC_CategoryEntry GetCategoryByIndex(int _index)
	{
		if (_index >= this.CategoryButtons.Count)
		{
			return null;
		}
		return this.CategoryButtons[_index];
	}

	// Token: 0x06007664 RID: 30308 RVA: 0x002C7112 File Offset: 0x002C5312
	public void SetCategoryToFirst()
	{
		this.CurrentCategory = this.CategoryButtons[0];
		XUiEvent_CategoryChangedEventHandler categoryChanged = this.CategoryChanged;
		if (categoryChanged == null)
		{
			return;
		}
		categoryChanged(this.CurrentCategory);
	}

	// Token: 0x06007665 RID: 30309 RVA: 0x002C713C File Offset: 0x002C533C
	public void SetCategory(string _category)
	{
		int num;
		XUiC_CategoryEntry categoryByName = this.GetCategoryByName(_category, out num);
		if (categoryByName == null && !this.AllowUnselect)
		{
			return;
		}
		this.CurrentCategory = categoryByName;
		XUiEvent_CategoryChangedEventHandler categoryChanged = this.CategoryChanged;
		if (categoryChanged == null)
		{
			return;
		}
		categoryChanged(this.CurrentCategory);
	}

	// Token: 0x06007666 RID: 30310 RVA: 0x002C717C File Offset: 0x002C537C
	public bool TrySetCategory(string _category)
	{
		int num;
		XUiC_CategoryEntry categoryByName = this.GetCategoryByName(_category, out num);
		if (categoryByName == null)
		{
			return false;
		}
		this.CurrentCategory = categoryByName;
		XUiEvent_CategoryChangedEventHandler categoryChanged = this.CategoryChanged;
		if (categoryChanged != null)
		{
			categoryChanged(this.CurrentCategory);
		}
		return true;
	}

	// Token: 0x06007667 RID: 30311 RVA: 0x002C71B8 File Offset: 0x002C53B8
	[PublicizedFrom(EAccessModifier.Private)]
	public void IncrementCategory(int _offset)
	{
		if (_offset == 0)
		{
			return;
		}
		int num = 0;
		int num2 = NGUIMath.RepeatIndex(this.currentIndex + _offset, this.CategoryButtons.Count);
		XUiC_CategoryEntry xuiC_CategoryEntry = this.CategoryButtons[num2];
		while (num < this.CategoryButtons.Count && (xuiC_CategoryEntry == null || xuiC_CategoryEntry.SpriteName == ""))
		{
			num2 = NGUIMath.RepeatIndex((_offset > 0) ? (num2 + 1) : (num2 - 1), this.CategoryButtons.Count);
			xuiC_CategoryEntry = this.CategoryButtons[num2];
			num++;
		}
		if (xuiC_CategoryEntry != null && xuiC_CategoryEntry.SpriteName != "" && xuiC_CategoryEntry.ViewComponent.Enabled)
		{
			this.CurrentCategory = xuiC_CategoryEntry;
			xuiC_CategoryEntry.PlayButtonClickSound();
			this.HandleCategoryChanged();
		}
	}

	// Token: 0x06007668 RID: 30312 RVA: 0x002C727C File Offset: 0x002C547C
	public void SetCategoryEmpty(int _index)
	{
		XUiC_CategoryEntry xuiC_CategoryEntry = this.CategoryButtons[_index];
		xuiC_CategoryEntry.CategoryDisplayName = (xuiC_CategoryEntry.CategoryName = (xuiC_CategoryEntry.SpriteName = ""));
		xuiC_CategoryEntry.ViewComponent.IsVisible = false;
		xuiC_CategoryEntry.ViewComponent.IsNavigatable = false;
	}

	// Token: 0x06007669 RID: 30313 RVA: 0x002C72CC File Offset: 0x002C54CC
	public void SetCategoryEntry(int _index, string _categoryName, string _spriteName, string _displayName = null)
	{
		XUiC_CategoryEntry xuiC_CategoryEntry = this.CategoryButtons[_index];
		xuiC_CategoryEntry.CategoryDisplayName = _displayName ?? _categoryName;
		xuiC_CategoryEntry.CategoryName = _categoryName;
		xuiC_CategoryEntry.SpriteName = _spriteName ?? "";
		xuiC_CategoryEntry.ViewComponent.IsVisible = true;
		xuiC_CategoryEntry.ViewComponent.IsNavigatable = true;
		xuiC_CategoryEntry.ViewComponent.Enabled = true;
		xuiC_CategoryEntry.ViewComponent.DisabledToolTip = null;
	}

	// Token: 0x0600766A RID: 30314 RVA: 0x002C7338 File Offset: 0x002C5538
	public override void OnOpen()
	{
		base.OnOpen();
		if (!this.HideGamepadCallouts)
		{
			this.xui.CalloutWindow.EnableCallouts(XUiC_GamepadCalloutWindow.CalloutType.MenuCategory, 0f);
		}
	}

	// Token: 0x0600766B RID: 30315 RVA: 0x002C735E File Offset: 0x002C555E
	public override void OnClose()
	{
		base.OnClose();
		if (!this.HideGamepadCallouts)
		{
			this.xui.CalloutWindow.DisableCallouts(XUiC_GamepadCalloutWindow.CalloutType.MenuCategory);
		}
	}

	// Token: 0x0600766C RID: 30316 RVA: 0x002C7380 File Offset: 0x002C5580
	public override bool ParseAttribute(string _name, string _value)
	{
		if (_name == "allow_unselect")
		{
			this.AllowUnselect = StringParsers.ParseBool(_value, 0, -1, true);
			return true;
		}
		if (_name == "allow_key_paging")
		{
			this.AllowKeyPaging = StringParsers.ParseBool(_value, 0, -1, true);
			return true;
		}
		if (!(_name == "hide_gamepad_callouts"))
		{
			return base.ParseAttribute(_name, _value);
		}
		this.HideGamepadCallouts = StringParsers.ParseBool(_value, 0, -1, true);
		return true;
	}

	// Token: 0x0600766D RID: 30317 RVA: 0x002C73F4 File Offset: 0x002C55F4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string _value, string _bindingName)
	{
		if (_bindingName == "currentcategoryicon")
		{
			XUiC_CategoryEntry xuiC_CategoryEntry = this.CurrentCategory;
			_value = ((xuiC_CategoryEntry != null) ? xuiC_CategoryEntry.SpriteName : null) ?? "";
			return true;
		}
		if (!(_bindingName == "currentcategorydisplayname"))
		{
			return base.GetBindingValueInternal(ref _value, _bindingName);
		}
		XUiC_CategoryEntry xuiC_CategoryEntry2 = this.CurrentCategory;
		_value = ((xuiC_CategoryEntry2 != null) ? xuiC_CategoryEntry2.CategoryDisplayName : null) ?? "";
		return true;
	}

	// Token: 0x0600766E RID: 30318 RVA: 0x002C7464 File Offset: 0x002C5664
	public bool SetupCategoriesByWorkstation(string _workstation)
	{
		if (this.currentWorkstation != _workstation)
		{
			this.currentWorkstation = _workstation;
			if (_workstation == "skills")
			{
				int num = 0;
				using (Dictionary<string, ProgressionClass>.Enumerator enumerator = Progression.ProgressionClasses.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<string, ProgressionClass> keyValuePair = enumerator.Current;
						if (keyValuePair.Value.IsAttribute && (XUiM_Recipes.CraftingProgression || !(keyValuePair.Value.Name == "attcrafting")))
						{
							this.SetCategoryEntry(num, keyValuePair.Value.Name, keyValuePair.Value.Icon, Localization.Get(keyValuePair.Value.Name, false, null));
							num++;
						}
					}
					return true;
				}
			}
			List<CraftingCategoryDisplayEntry> craftingCategoryDisplayList = UIDisplayInfoManager.Current.GetCraftingCategoryDisplayList(_workstation);
			if (craftingCategoryDisplayList != null)
			{
				int num2 = 0;
				int num3 = 0;
				while (num3 < craftingCategoryDisplayList.Count && num3 < this.CategoryButtons.Count)
				{
					this.SetCategoryEntry(num2, craftingCategoryDisplayList[num3].Name, craftingCategoryDisplayList[num3].Icon, craftingCategoryDisplayList[num3].DisplayName);
					num2++;
					num3++;
				}
				for (int i = num2; i < this.CategoryButtons.Count; i++)
				{
					this.SetCategoryEmpty(num2++);
				}
			}
			return true;
		}
		return false;
	}

	// Token: 0x0600766F RID: 30319 RVA: 0x002C75D8 File Offset: 0x002C57D8
	public bool SetupCategoriesBasedOnItems(List<TraderData.Entry> _entries, int _traderStage)
	{
		if (_entries == null)
		{
			return false;
		}
		List<string> list = new List<string>();
		this.SetCategoryEntry(0, "", "ui_game_symbol_shopping_cart", Localization.Get("lblAll", false, null));
		list.Add("");
		for (int i = 0; i < _entries.Count; i++)
		{
			ItemClass itemClass = _entries[i].Item.itemValue.ItemClass;
			if (itemClass == null)
			{
				Log.Error("TraderData.Entry #{0} null", new object[] { i });
			}
			else
			{
				TraderStageTemplateGroup traderStageTemplateGroup = null;
				if (itemClass.TraderStageTemplate != null && TraderManager.TraderStageTemplates.ContainsKey(itemClass.TraderStageTemplate))
				{
					traderStageTemplateGroup = TraderManager.TraderStageTemplates[itemClass.TraderStageTemplate];
				}
				if (traderStageTemplateGroup == null || traderStageTemplateGroup.IsWithin(_traderStage, (int)_entries[i].Item.itemValue.Quality) || _entries[i].AddedByPlayer)
				{
					string[] array = itemClass.Groups;
					if (itemClass.IsBlock())
					{
						array = Block.list[_entries[i].Item.itemValue.type].GroupNames;
					}
					for (int j = 0; j < array.Length; j++)
					{
						if (!list.Contains(array[j]))
						{
							CraftingCategoryDisplayEntry traderCategoryDisplay = UIDisplayInfoManager.Current.GetTraderCategoryDisplay(array[j]);
							if (traderCategoryDisplay != null)
							{
								int count = list.Count;
								this.SetCategoryEntry(count, traderCategoryDisplay.Name, traderCategoryDisplay.Icon, traderCategoryDisplay.DisplayName);
								list.Add(array[j]);
							}
						}
					}
				}
			}
		}
		for (int k = list.Count; k < this.CategoryButtons.Count; k++)
		{
			this.SetCategoryEmpty(k);
		}
		return true;
	}

	// Token: 0x06007670 RID: 30320 RVA: 0x002C7788 File Offset: 0x002C5988
	public bool SetupCategoriesBasedOnTwitchCategories(List<TwitchActionManager.ActionCategory> _items)
	{
		List<string> list = new List<string>();
		this.SetCategoryEntry(0, "", "ui_game_symbol_twitch_actions", Localization.Get("lblAll", false, null));
		list.Add("");
		for (int i = 0; i < _items.Count; i++)
		{
			TwitchActionManager.ActionCategory actionCategory = _items[i];
			if (actionCategory.Icon != "")
			{
				this.SetCategoryEntry(list.Count, actionCategory.Name, actionCategory.Icon, actionCategory.Name);
				list.Add(actionCategory.Name);
			}
		}
		for (int j = list.Count; j < this.CategoryButtons.Count; j++)
		{
			this.SetCategoryEmpty(j);
		}
		return true;
	}

	// Token: 0x06007671 RID: 30321 RVA: 0x002C783C File Offset: 0x002C5A3C
	public bool SetupCategoriesBasedOnTwitchActions(List<TwitchAction> _items)
	{
		List<string> list = new List<string>();
		this.SetCategoryEntry(0, "", "ui_game_symbol_twitch_actions", Localization.Get("lblAll", false, null));
		list.Add("");
		Dictionary<TwitchActionManager.ActionCategory, int> dictionary = new Dictionary<TwitchActionManager.ActionCategory, int>();
		List<TwitchActionManager.ActionCategory> categoryList = TwitchActionManager.Current.CategoryList;
		for (int i = 0; i < categoryList.Count; i++)
		{
			dictionary.Add(categoryList[i], 0);
		}
		for (int j = 0; j < _items.Count; j++)
		{
			TwitchAction twitchAction = _items[j];
			if (twitchAction.DisplayCategory != null)
			{
				Dictionary<TwitchActionManager.ActionCategory, int> dictionary2 = dictionary;
				TwitchActionManager.ActionCategory displayCategory = twitchAction.DisplayCategory;
				int num = dictionary2[displayCategory];
				dictionary2[displayCategory] = num + 1;
			}
		}
		foreach (TwitchActionManager.ActionCategory actionCategory in dictionary.Keys)
		{
			if (dictionary[actionCategory] > 0)
			{
				this.SetCategoryEntry(list.Count, actionCategory.Name, actionCategory.Icon, actionCategory.DisplayName);
				list.Add(actionCategory.Name);
			}
		}
		for (int k = list.Count; k < this.CategoryButtons.Count; k++)
		{
			this.SetCategoryEmpty(k);
		}
		return true;
	}

	// Token: 0x06007672 RID: 30322 RVA: 0x002C7990 File Offset: 0x002C5B90
	public bool SetupCategoriesBasedOnTwitchVoteCategories(List<TwitchVoteType> _items)
	{
		List<string> list = new List<string>();
		this.SetCategoryEntry(0, "", "ui_game_symbol_twitch_vote", Localization.Get("lblAll", false, null));
		list.Add("");
		for (int i = 0; i < _items.Count; i++)
		{
			TwitchVoteType twitchVoteType = _items[i];
			if (twitchVoteType.Icon != "")
			{
				this.SetCategoryEntry(list.Count, twitchVoteType.Name, twitchVoteType.Icon, twitchVoteType.Title);
				list.Add(twitchVoteType.Name);
			}
		}
		for (int j = list.Count; j < this.CategoryButtons.Count; j++)
		{
			this.SetCategoryEmpty(j);
		}
		return true;
	}

	// Token: 0x06007673 RID: 30323 RVA: 0x002C7A44 File Offset: 0x002C5C44
	public bool SetupCategoriesBasedOnGameEventCategories(List<GameEventManager.Category> _items)
	{
		List<string> list = new List<string>();
		this.SetCategoryEntry(0, "", "ui_game_symbol_airdrop", Localization.Get("lblAll", false, null));
		list.Add("");
		for (int i = 0; i < _items.Count; i++)
		{
			GameEventManager.Category category = _items[i];
			if (category.Icon != "")
			{
				this.SetCategoryEntry(list.Count, category.Name, category.Icon, category.Name);
				list.Add(category.Name);
			}
		}
		for (int j = list.Count; j < this.CategoryButtons.Count; j++)
		{
			this.SetCategoryEmpty(j);
		}
		return true;
	}

	// Token: 0x06007674 RID: 30324 RVA: 0x002C7AF8 File Offset: 0x002C5CF8
	public bool SetupCategoriesBasedOnChallengeCategories(List<ChallengeCategory> _items)
	{
		List<string> list = new List<string>();
		EntityPlayerLocal entityPlayer = this.xui.playerUI.entityPlayer;
		for (int i = 0; i < _items.Count; i++)
		{
			ChallengeCategory challengeCategory = _items[i];
			if (challengeCategory.Icon != "" && challengeCategory.CanShow(entityPlayer))
			{
				this.SetCategoryEntry(list.Count, challengeCategory.Name, challengeCategory.Icon, challengeCategory.Title);
				list.Add(challengeCategory.Name);
			}
		}
		for (int j = list.Count; j < this.CategoryButtons.Count; j++)
		{
			this.SetCategoryEmpty(j);
		}
		return true;
	}

	// Token: 0x04005737 RID: 22327
	[PublicizedFrom(EAccessModifier.Private)]
	public string currentWorkstation = "*";

	// Token: 0x0400573A RID: 22330
	public readonly List<XUiC_CategoryEntry> CategoryButtons = new List<XUiC_CategoryEntry>();

	// Token: 0x0400573B RID: 22331
	[PublicizedFrom(EAccessModifier.Private)]
	public int currentIndex;

	// Token: 0x0400573C RID: 22332
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CategoryEntry currentCategory;

	// Token: 0x0400573D RID: 22333
	public bool AllowUnselect;

	// Token: 0x0400573E RID: 22334
	public bool AllowKeyPaging = true;

	// Token: 0x0400573F RID: 22335
	[PublicizedFrom(EAccessModifier.Private)]
	public bool HideGamepadCallouts;
}
