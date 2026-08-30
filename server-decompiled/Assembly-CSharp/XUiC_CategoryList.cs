using System;
using System.Collections.Generic;
using Challenges;
using Twitch;
using UnityEngine.Scripting;

// Token: 0x02000ED5 RID: 3797
[Preserve]
public class XUiC_CategoryList : XUiController
{
	// Token: 0x140000CE RID: 206
	// (add) Token: 0x060076F9 RID: 30457 RVA: 0x002CA378 File Offset: 0x002C8578
	// (remove) Token: 0x060076FA RID: 30458 RVA: 0x002CA3B0 File Offset: 0x002C85B0
	public event XUiEvent_CategoryChangedEventHandler CategoryChanged;

	// Token: 0x140000CF RID: 207
	// (add) Token: 0x060076FB RID: 30459 RVA: 0x002CA3E8 File Offset: 0x002C85E8
	// (remove) Token: 0x060076FC RID: 30460 RVA: 0x002CA420 File Offset: 0x002C8620
	public event XUiEvent_CategoryChangedEventHandler CategoryClickChanged;

	// Token: 0x17000E01 RID: 3585
	// (get) Token: 0x060076FD RID: 30461 RVA: 0x002CA455 File Offset: 0x002C8655
	// (set) Token: 0x060076FE RID: 30462 RVA: 0x002CA460 File Offset: 0x002C8660
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

	// Token: 0x17000E02 RID: 3586
	// (get) Token: 0x060076FF RID: 30463 RVA: 0x002CA4BA File Offset: 0x002C86BA
	public int MaxCategories
	{
		get
		{
			return this.CategoryButtons.Count;
		}
	}

	// Token: 0x06007700 RID: 30464 RVA: 0x002CA4C8 File Offset: 0x002C86C8
	public override void Init()
	{
		base.Init();
		base.GetChildrenByType<XUiC_CategoryEntry>(this.CategoryButtons);
		for (int i = 0; i < this.CategoryButtons.Count; i++)
		{
			this.CategoryButtons[i].CategoryList = this;
		}
	}

	// Token: 0x06007701 RID: 30465 RVA: 0x002CA510 File Offset: 0x002C8710
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

	// Token: 0x06007702 RID: 30466 RVA: 0x002CA598 File Offset: 0x002C8798
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

	// Token: 0x06007703 RID: 30467 RVA: 0x002CA5C8 File Offset: 0x002C87C8
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

	// Token: 0x06007704 RID: 30468 RVA: 0x002CA618 File Offset: 0x002C8818
	public XUiC_CategoryEntry GetCategoryByIndex(int _index)
	{
		if (_index >= this.CategoryButtons.Count)
		{
			return null;
		}
		return this.CategoryButtons[_index];
	}

	// Token: 0x06007705 RID: 30469 RVA: 0x002CA636 File Offset: 0x002C8836
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

	// Token: 0x06007706 RID: 30470 RVA: 0x002CA660 File Offset: 0x002C8860
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

	// Token: 0x06007707 RID: 30471 RVA: 0x002CA6A0 File Offset: 0x002C88A0
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

	// Token: 0x06007708 RID: 30472 RVA: 0x002CA6DC File Offset: 0x002C88DC
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

	// Token: 0x06007709 RID: 30473 RVA: 0x002CA7A0 File Offset: 0x002C89A0
	public void SetCategoryEmpty(int _index)
	{
		XUiC_CategoryEntry xuiC_CategoryEntry = this.CategoryButtons[_index];
		xuiC_CategoryEntry.CategoryDisplayName = (xuiC_CategoryEntry.CategoryName = (xuiC_CategoryEntry.SpriteName = ""));
		xuiC_CategoryEntry.ViewComponent.IsVisible = false;
		xuiC_CategoryEntry.ViewComponent.IsNavigatable = false;
	}

	// Token: 0x0600770A RID: 30474 RVA: 0x002CA7F0 File Offset: 0x002C89F0
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

	// Token: 0x0600770B RID: 30475 RVA: 0x002CA85C File Offset: 0x002C8A5C
	public override void OnOpen()
	{
		base.OnOpen();
		if (!this.HideGamepadCallouts)
		{
			this.xui.CalloutWindow.EnableCallouts(XUiC_GamepadCalloutWindow.CalloutType.MenuCategory, 0f);
		}
	}

	// Token: 0x0600770C RID: 30476 RVA: 0x002CA882 File Offset: 0x002C8A82
	public override void OnClose()
	{
		base.OnClose();
		if (!this.HideGamepadCallouts)
		{
			this.xui.CalloutWindow.DisableCallouts(XUiC_GamepadCalloutWindow.CalloutType.MenuCategory);
		}
	}

	// Token: 0x0600770D RID: 30477 RVA: 0x002CA8A4 File Offset: 0x002C8AA4
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

	// Token: 0x0600770E RID: 30478 RVA: 0x002CA918 File Offset: 0x002C8B18
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

	// Token: 0x0600770F RID: 30479 RVA: 0x002CA988 File Offset: 0x002C8B88
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

	// Token: 0x06007710 RID: 30480 RVA: 0x002CAAFC File Offset: 0x002C8CFC
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

	// Token: 0x06007711 RID: 30481 RVA: 0x002CACAC File Offset: 0x002C8EAC
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

	// Token: 0x06007712 RID: 30482 RVA: 0x002CAD60 File Offset: 0x002C8F60
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

	// Token: 0x06007713 RID: 30483 RVA: 0x002CAEB4 File Offset: 0x002C90B4
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

	// Token: 0x06007714 RID: 30484 RVA: 0x002CAF68 File Offset: 0x002C9168
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

	// Token: 0x06007715 RID: 30485 RVA: 0x002CB01C File Offset: 0x002C921C
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

	// Token: 0x040057DC RID: 22492
	[PublicizedFrom(EAccessModifier.Private)]
	public string currentWorkstation = "*";

	// Token: 0x040057DF RID: 22495
	public readonly List<XUiC_CategoryEntry> CategoryButtons = new List<XUiC_CategoryEntry>();

	// Token: 0x040057E0 RID: 22496
	[PublicizedFrom(EAccessModifier.Private)]
	public int currentIndex;

	// Token: 0x040057E1 RID: 22497
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CategoryEntry currentCategory;

	// Token: 0x040057E2 RID: 22498
	public bool AllowUnselect;

	// Token: 0x040057E3 RID: 22499
	public bool AllowKeyPaging = true;

	// Token: 0x040057E4 RID: 22500
	[PublicizedFrom(EAccessModifier.Private)]
	public bool HideGamepadCallouts;
}
