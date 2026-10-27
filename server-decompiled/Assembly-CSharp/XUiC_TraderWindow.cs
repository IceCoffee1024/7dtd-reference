using System;
using System.Collections.Generic;
using Audio;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020010AF RID: 4271
[Preserve]
public class XUiC_TraderWindow : XUiController
{
	// Token: 0x17000FF3 RID: 4083
	// (get) Token: 0x06008751 RID: 34641 RVA: 0x0033B3FA File Offset: 0x003395FA
	public TileEntityVendingMachine CurrentVending
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.xui.Trader.Trader as TileEntityVendingMachine;
		}
	}

	// Token: 0x17000FF4 RID: 4084
	// (get) Token: 0x06008752 RID: 34642 RVA: 0x0033B411 File Offset: 0x00339611
	public EntityTrader CurrentTraderEntity
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.xui.Trader.Trader as EntityTrader;
		}
	}

	// Token: 0x17000FF5 RID: 4085
	// (get) Token: 0x06008753 RID: 34643 RVA: 0x0033B428 File Offset: 0x00339628
	public bool IsVending
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.CurrentVending != null;
		}
	}

	// Token: 0x17000FF6 RID: 4086
	// (get) Token: 0x06008754 RID: 34644 RVA: 0x0033B433 File Offset: 0x00339633
	public bool IsOwner
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.CurrentVending != null && this.CurrentVending.LocalPlayerIsOwner();
		}
	}

	// Token: 0x17000FF7 RID: 4087
	// (get) Token: 0x06008755 RID: 34645 RVA: 0x0033B44C File Offset: 0x0033964C
	public bool IsPlayerOwned
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			TraderData traderData = this.xui.Trader.TraderData;
			bool? flag;
			if (traderData == null)
			{
				flag = null;
			}
			else
			{
				TraderInfo traderInfo = traderData.TraderInfo;
				flag = ((traderInfo != null) ? new bool?(traderInfo.PlayerOwned) : null);
			}
			bool? flag2 = flag;
			return flag2.GetValueOrDefault();
		}
	}

	// Token: 0x17000FF8 RID: 4088
	// (get) Token: 0x06008756 RID: 34646 RVA: 0x0033B4A0 File Offset: 0x003396A0
	public bool IsRentable
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			TraderData traderData = this.xui.Trader.TraderData;
			bool? flag;
			if (traderData == null)
			{
				flag = null;
			}
			else
			{
				TraderInfo traderInfo = traderData.TraderInfo;
				flag = ((traderInfo != null) ? new bool?(traderInfo.Rentable) : null);
			}
			bool? flag2 = flag;
			return flag2.GetValueOrDefault();
		}
	}

	// Token: 0x17000FF9 RID: 4089
	// (get) Token: 0x06008757 RID: 34647 RVA: 0x0033B4F2 File Offset: 0x003396F2
	// (set) Token: 0x06008758 RID: 34648 RVA: 0x0033B4FA File Offset: 0x003396FA
	public int Page
	{
		get
		{
			return this.page;
		}
		set
		{
			if (this.page != value)
			{
				this.page = value;
				this.itemListGrid.Page = this.page;
				XUiC_Paging xuiC_Paging = this.pager;
				if (xuiC_Paging == null)
				{
					return;
				}
				xuiC_Paging.SetPage(this.page);
			}
		}
	}

	// Token: 0x06008759 RID: 34649 RVA: 0x0033B534 File Offset: 0x00339734
	public override void Init()
	{
		base.Init();
		this.lblGeneralStock = Localization.Get("xuiGeneralStock", false, null);
		this.lblSecretStash = Localization.Get("xuiSecretStash", false, null);
		this.windowicon = base.GetChildById("windowicon");
		this.playerName = (XUiC_PlayerName)base.GetChildById("playerName");
		this.categoryList = this.windowGroup.Controller.GetChildByType<XUiC_CategoryList>();
		this.categoryList.CategoryChanged += this.HandleCategoryChanged;
		this.pager = base.GetChildByType<XUiC_Paging>();
		if (this.pager != null)
		{
			this.pager.OnPageChanged += delegate
			{
				this.Page = this.pager.CurrentPageNumber;
				this.GetItemStackData(this.txtInput.Text);
			};
		}
		for (int i = 0; i < this.children.Count; i++)
		{
			this.children[i].OnScroll += this.HandleOnScroll;
		}
		base.OnScroll += this.HandleOnScroll;
		this.itemListGrid = base.Parent.GetChildByType<XUiC_TraderItemList>();
		XUiController[] childrenByType = this.itemListGrid.GetChildrenByType<XUiC_TraderItemEntry>(null);
		XUiController[] array = childrenByType;
		for (int j = 0; j < array.Length; j++)
		{
			array[j].OnScroll += this.HandleOnScroll;
			((XUiC_TraderItemEntry)array[j]).TraderWindow = this;
		}
		this.txtInput = (XUiC_TextInput)this.windowGroup.Controller.GetChildById("searchInput");
		if (this.txtInput != null)
		{
			this.txtInput.OnChangeHandler += this.HandleOnChangedHandler;
			this.txtInput.Text = "";
		}
		XUiController xuiController = base.GetChildById("collect");
		if (xuiController != null)
		{
			xuiController.OnPress += this.Collect_OnPress;
		}
		xuiController = base.GetChildById("takeAll");
		if (xuiController != null)
		{
			xuiController.OnPress += this.TakeAll_OnPress;
		}
		xuiController = base.GetChildById("rent");
		if (xuiController != null)
		{
			xuiController.OnPress += this.Rent_OnPress;
			this.rentButton = (XUiV_Button)xuiController.ViewComponent;
		}
		xuiController = base.GetChildById("showAll");
		if (xuiController != null)
		{
			xuiController.OnPress += this.ShowAll_OnPress;
			this.showAllButton = (XUiV_Button)xuiController.ViewComponent;
		}
	}

	// Token: 0x0600875A RID: 34650 RVA: 0x0033B77A File Offset: 0x0033997A
	[PublicizedFrom(EAccessModifier.Private)]
	public void ShowAll_OnPress(XUiController _sender, int _mouseButton)
	{
		this.showAll = !this.showAll;
		this.showAllButton.Selected = this.showAll;
		this.RefreshTraderItems();
	}

	// Token: 0x0600875B RID: 34651 RVA: 0x0033B7A4 File Offset: 0x003399A4
	[PublicizedFrom(EAccessModifier.Private)]
	public void Rent_OnPress(XUiController _sender, int _mouseButton)
	{
		if (this.IsVending)
		{
			if (this.serviceInfoWindow == null)
			{
				this.serviceInfoWindow = (XUiC_ServiceInfoWindow)this.windowGroup.Controller.GetChildById("serviceInfoPanel");
			}
			InGameService inGameService = new InGameService
			{
				Name = Localization.Get("rentVendingMachine", false, null),
				Description = Localization.Get("rentVendingMachineDesc", false, null),
				Icon = "ui_game_symbol_vending",
				Price = this.CurrentVending.TraderData.TraderInfo.RentCost,
				VisibleChangedHandler = delegate(bool visible)
				{
					if (this.rentButton != null)
					{
						this.rentButton.Selected = visible;
					}
				}
			};
			if (this.xui.CurrentSelectedEntry != null)
			{
				this.xui.CurrentSelectedEntry.IsSelected = false;
			}
			this.serviceInfoWindow.SetInfo(inGameService, this);
		}
	}

	// Token: 0x0600875C RID: 34652 RVA: 0x0033B874 File Offset: 0x00339A74
	[PublicizedFrom(EAccessModifier.Private)]
	public void Collect_OnPress(XUiController _sender, int _mouseButton)
	{
		if (this.IsVending && (this.IsPlayerOwned || this.IsRentable) && this.CurrentVending.IsUserAllowed(PlatformManager.InternalLocalUserIdentifier))
		{
			ItemValue item = ItemClass.GetItem(TraderInfo.CurrencyItem, false);
			int availableMoney = this.xui.Trader.TraderData.AvailableMoney;
			XUiM_PlayerInventory playerInventory = this.xui.PlayerInventory;
			ItemStack itemStack = new ItemStack(item.Clone(), availableMoney);
			playerInventory.AddItem(itemStack);
			this.xui.Trader.TraderData.AvailableMoney = itemStack.count;
			base.RefreshBindings();
		}
	}

	// Token: 0x0600875D RID: 34653 RVA: 0x0033B910 File Offset: 0x00339B10
	[PublicizedFrom(EAccessModifier.Private)]
	public void TakeAll_OnPress(XUiController _sender, int _mouseButton)
	{
		if (this.IsVending && (this.IsPlayerOwned || this.IsRentable) && this.CurrentVending.IsUserAllowed(PlatformManager.InternalLocalUserIdentifier))
		{
			TraderData traderData = this.xui.Trader.TraderData;
			XUiM_PlayerInventory playerInventory = this.xui.PlayerInventory;
			bool flag = false;
			for (int i = 0; i < traderData.PrimaryInventory.Count; i++)
			{
				ItemStack item = traderData.PrimaryInventory[i].Item;
				int num = (item.itemValue.ItemClass.IsBlock() ? Block.list[item.itemValue.type].EconomicBundleSize : item.itemValue.ItemClass.EconomicBundleSize);
				int num2 = Math.Min(item.count, this.xui.PlayerInventory.CountAvailableSpaceForItem(item.itemValue, true)) / num * num;
				int num3 = item.count - num2;
				item.count = num2;
				if (playerInventory.AddItem(item, false))
				{
					flag = true;
				}
				item.count += num3;
				if (item.count == 0)
				{
					traderData.PrimaryInventory.RemoveAt(i--);
				}
			}
			if (flag && GameManager.Instance != null && GameManager.Instance.World != null)
			{
				Manager.Play(GameManager.Instance.World.GetPrimaryPlayer(), "UseActions/takeall1", 1f, false);
			}
			this.RefreshTraderItems();
		}
	}

	// Token: 0x0600875E RID: 34654 RVA: 0x0033BA98 File Offset: 0x00339C98
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleCategoryChanged(XUiC_CategoryEntry _categoryEntry)
	{
		string text = _categoryEntry.CategoryName;
		if (text == "SECRET STASH")
		{
			text = "";
			this.isSecretStash = true;
		}
		else
		{
			this.isSecretStash = false;
		}
		this.RefreshHeader();
		this.Page = 0;
		this.SetCategory(text);
	}

	// Token: 0x0600875F RID: 34655 RVA: 0x0033BAE4 File Offset: 0x00339CE4
	[PublicizedFrom(EAccessModifier.Private)]
	public void RefreshHeader()
	{
		if (this.IsVending)
		{
			if (this.IsRentable || this.IsPlayerOwned)
			{
				if (this.CurrentVending.GetOwner() != null)
				{
					string displayName = GameManager.Instance.persistentPlayers.GetPlayerData(this.CurrentVending.GetOwner()).PlayerName.DisplayName;
					this.playerName.SetGenericName(string.Format(Localization.Get("xuiVendingWithOwner", false, null), displayName));
				}
				else
				{
					this.playerName.SetGenericName(Localization.Get("xuiEmptyVendingMachine", false, null));
				}
			}
			else
			{
				this.playerName.SetGenericName(this.lblGeneralStock);
			}
			if (this.windowicon != null)
			{
				((XUiV_Sprite)this.windowicon.ViewComponent).SpriteName = "ui_game_symbol_vending";
				return;
			}
		}
		else
		{
			this.playerName.SetGenericName(this.isSecretStash ? this.lblSecretStash : this.lblGeneralStock);
			((XUiV_Sprite)this.windowicon.ViewComponent).SpriteName = "ui_game_symbol_map_trader";
		}
	}

	// Token: 0x06008760 RID: 34656 RVA: 0x0033BBE8 File Offset: 0x00339DE8
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleOnChangedHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		this.Page = 0;
		this.FilterByName(_text);
		this.itemListGrid.SetItems(this.currentInventory.ToArray(), this.currentIndexList);
		if (this.currentInventory.Count == 0 || this.currentInventory[0].IsEmpty())
		{
			base.GetChildById("searchControls").SelectCursorElement(true, false);
		}
	}

	// Token: 0x06008761 RID: 34657 RVA: 0x0033BC52 File Offset: 0x00339E52
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleOnScroll(XUiController _sender, float _delta)
	{
		if (_delta > 0f)
		{
			XUiC_Paging xuiC_Paging = this.pager;
			if (xuiC_Paging == null)
			{
				return;
			}
			xuiC_Paging.PageDown();
			return;
		}
		else
		{
			XUiC_Paging xuiC_Paging2 = this.pager;
			if (xuiC_Paging2 == null)
			{
				return;
			}
			xuiC_Paging2.PageUp();
			return;
		}
	}

	// Token: 0x06008762 RID: 34658 RVA: 0x0033BC80 File Offset: 0x00339E80
	[PublicizedFrom(EAccessModifier.Private)]
	public void GetItemStackData(string _name)
	{
		if (_name == null)
		{
			_name = "";
		}
		this.currentInventory.Clear();
		this.length = this.itemListGrid.Length;
		this.FilterByName(_name);
		this.itemListGrid.SetItems(this.currentInventory.ToArray(), this.currentIndexList);
		if (!this.isSecretStash)
		{
			this.categoryList.SetupCategoriesBasedOnItems(this.buyInventory, this.traderStage);
		}
		if (this.currentInventory.Count == 0 || this.currentInventory[0].IsEmpty())
		{
			base.GetChildById("searchControls").SelectCursorElement(true, false);
			return;
		}
	}

	// Token: 0x06008763 RID: 34659 RVA: 0x0033BD2C File Offset: 0x00339F2C
	public void FilterByName(string _name)
	{
		this.currentIndexList.Clear();
		this.currentInventory.Clear();
		for (int i = 0; i < this.buyInventory.Count; i++)
		{
			TraderData.Entry entry = this.buyInventory[i];
			ItemStack item = entry.Item;
			if (item != null && item.count != 0)
			{
				ItemClass itemClass = item.itemValue.ItemClass;
				string text = itemClass.GetLocalizedItemName();
				if (text == null)
				{
					text = Localization.Get(itemClass.Name, false, null);
				}
				if (this.category == "")
				{
					if (_name == "" || itemClass.Name.ContainsCaseInsensitive(_name) || text.ContainsCaseInsensitive(_name) || item.itemValue.GetItemOrBlockId().ToString() == _name.Trim())
					{
						TraderStageTemplateGroup traderStageTemplateGroup = null;
						if (itemClass.TraderStageTemplate != null)
						{
							if (!TraderManager.TraderStageTemplates.ContainsKey(itemClass.TraderStageTemplate))
							{
								throw new Exception(string.Concat(new string[]
								{
									"TraderStageTemplate ",
									itemClass.TraderStageTemplate,
									" for item: ",
									itemClass.GetLocalizedItemName(),
									" does not exist."
								}));
							}
							traderStageTemplateGroup = TraderManager.TraderStageTemplates[itemClass.TraderStageTemplate];
						}
						if (traderStageTemplateGroup == null || this.traderStage == -1 || traderStageTemplateGroup.IsWithin(this.traderStage, (int)item.itemValue.Quality) || this.showAll || entry.AddedByPlayer)
						{
							this.currentIndexList.Add(i);
							this.currentInventory.Add(item);
						}
					}
				}
				else
				{
					string[] array = itemClass.Groups;
					if (itemClass.IsBlock())
					{
						array = Block.list[item.itemValue.type].GroupNames;
					}
					for (int j = 0; j < array.Length; j++)
					{
						if (array[j] != null && array[j].EqualsCaseInsensitive(this.category) && (_name == "" || itemClass.Name.ContainsCaseInsensitive(_name) || text.ContainsCaseInsensitive(_name) || item.itemValue.GetItemOrBlockId().ToString() == _name.Trim()))
						{
							TraderStageTemplateGroup traderStageTemplateGroup2 = null;
							if (itemClass.TraderStageTemplate != null)
							{
								if (!TraderManager.TraderStageTemplates.ContainsKey(itemClass.TraderStageTemplate))
								{
									throw new Exception(string.Concat(new string[]
									{
										"TraderStageTemplate ",
										itemClass.TraderStageTemplate,
										" for item: ",
										itemClass.GetLocalizedItemName(),
										" does not exist."
									}));
								}
								traderStageTemplateGroup2 = TraderManager.TraderStageTemplates[itemClass.TraderStageTemplate];
							}
							if (traderStageTemplateGroup2 == null || this.traderStage == -1 || traderStageTemplateGroup2.IsWithin(this.traderStage, (int)item.itemValue.Quality) || this.showAll || entry.AddedByPlayer)
							{
								this.currentIndexList.Add(i);
								this.currentInventory.Add(item);
							}
						}
					}
				}
			}
		}
		XUiC_Paging xuiC_Paging = this.pager;
		if (xuiC_Paging == null)
		{
			return;
		}
		xuiC_Paging.SetLastPageByElementsAndPageLength(this.currentInventory.Count, this.length);
	}

	// Token: 0x06008764 RID: 34660 RVA: 0x0033C05D File Offset: 0x0033A25D
	public void SetCategory(string _category)
	{
		if (this.txtInput != null)
		{
			this.txtInput.Text = "";
		}
		this.category = _category;
		this.RefreshTraderItems();
	}

	// Token: 0x06008765 RID: 34661 RVA: 0x0033C084 File Offset: 0x0033A284
	public string GetCategory()
	{
		return this.category;
	}

	// Token: 0x06008766 RID: 34662 RVA: 0x0033C08C File Offset: 0x0033A28C
	public override void OnOpen()
	{
		base.OnOpen();
		if (this.xui.Trader.Trader != null && this.IsVending)
		{
			Manager.PlayInsidePlayerHead("open_vending", -1, 0f, false, false);
		}
		EntityPlayer entityPlayer = this.xui.playerUI.entityPlayer;
		if (this.CurrentTraderEntity != null)
		{
			this.traderStage = entityPlayer.GetTraderStage(entityPlayer.QuestJournal.GetCurrentFactionTier(this.CurrentTraderEntity.NPCInfo.QuestFaction, 0, false));
		}
		else
		{
			this.traderStage = -1;
		}
		this.CompletedTransaction = false;
		if (this.xui.Trader.TraderData != null)
		{
			this.categoryList.SetCategoryToFirst();
		}
		if (this.IsRentable && this.IsOwner && this.CurrentVending != null && this.CurrentVending.TryAutoBuy(true))
		{
			this.RefreshTraderItems();
		}
		this.Refresh();
	}

	// Token: 0x06008767 RID: 34663 RVA: 0x0033C174 File Offset: 0x0033A374
	public override void OnClose()
	{
		base.OnClose();
		if (this.CurrentTraderEntity != null)
		{
			if (this.CompletedTransaction)
			{
				this.CurrentTraderEntity.PlayVoiceSetEntry("sale_accepted", this.xui.playerUI.entityPlayer, true, true);
			}
			else
			{
				this.CurrentTraderEntity.PlayVoiceSetEntry("sale_declined", this.xui.playerUI.entityPlayer, true, true);
			}
		}
		else
		{
			Manager.PlayInsidePlayerHead("close_vending", -1, 0f, false, false);
		}
		XUiM_Trader trader = this.xui.Trader;
		if (trader != null)
		{
			TraderData traderData = trader.TraderData;
			if (traderData != null)
			{
				traderData.SetModified(this.xui.Trader.Trader);
			}
		}
		if (this.CurrentVending != null)
		{
			this.CurrentVending.SetUserAccessing(false);
		}
		LockManager.Instance.UnlockRequestLocal();
		this.xui.Trader.Trader = null;
	}

	// Token: 0x06008768 RID: 34664 RVA: 0x0033C258 File Offset: 0x0033A458
	public void RefreshTraderItems()
	{
		this.buyInventory.Clear();
		ItemStack[] stashSlots = this.GetStashSlots();
		this.hasSecretStash = stashSlots != null;
		if (this.isSecretStash && this.hasSecretStash)
		{
			for (int i = 0; i < stashSlots.Length; i++)
			{
				this.buyInventory.Add(new TraderData.Entry(stashSlots[i], 0, false));
			}
		}
		else
		{
			this.buyInventory.AddRange(this.xui.Trader.TraderData.PrimaryInventory);
		}
		this.GetItemStackData(this.txtInput.Text);
		this.IsDirty = true;
	}

	// Token: 0x06008769 RID: 34665 RVA: 0x0033C2F0 File Offset: 0x0033A4F0
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack[] GetStashSlots()
	{
		float value = EffectManager.GetValue(PassiveEffects.SecretStash, null, (float)this.xui.playerUI.entityPlayer.Progression.Level, this.xui.playerUI.entityPlayer, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
		for (int i = 0; i < this.xui.Trader.TraderData.TierItemGroups.Count; i++)
		{
			TraderInfo.TierItemGroup tierItemGroup = this.xui.Trader.TraderData.TraderInfo.TierItemGroups[i];
			if ((value >= (float)tierItemGroup.minLevel || tierItemGroup.minLevel == -1) && (value <= (float)tierItemGroup.maxLevel || tierItemGroup.maxLevel == -1))
			{
				return this.xui.Trader.TraderData.TierItemGroups[i];
			}
		}
		return null;
	}

	// Token: 0x0600876A RID: 34666 RVA: 0x0033C3D4 File Offset: 0x0033A5D4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string value, string bindingName)
	{
		uint num = <PrivateImplementationDetails>.ComputeStringHash(bindingName);
		if (num <= 1533283582U)
		{
			if (num <= 543700434U)
			{
				if (num != 142312684U)
				{
					if (num != 391295749U)
					{
						if (num == 543700434U)
						{
							if (bindingName == "renttimeleft")
							{
								if (this.IsVending)
								{
									if (this.IsOwner && this.CurrentVending.IsRentable)
									{
										int rentalEndDay = this.CurrentVending.RentalEndDay;
										value = this.rentTimeLeftFormatter.Format(rentalEndDay);
									}
								}
								else
								{
									value = "";
								}
								return true;
							}
						}
					}
					else if (bindingName == "timeleft")
					{
						if (this.xui.Trader.TraderData != null)
						{
							int num2 = GameUtils.WorldTimeToDays(this.xui.Trader.TraderData.NextResetTime);
							value = this.timeLeftFormatter.Format(num2);
						}
						return true;
					}
				}
				else if (bindingName == "availablemoney")
				{
					value = ((this.xui.Trader.TraderData != null) ? this.availableMoneyFormatter.Format(this.xui.Trader.TraderData.AvailableMoney) : "");
					return true;
				}
			}
			else if (num != 619389156U)
			{
				if (num != 916116330U)
				{
					if (num == 1533283582U)
					{
						if (bindingName == "isrentable")
						{
							if (this.IsVending)
							{
								if (this.CurrentVending == null)
								{
									value = "false";
									return true;
								}
								value = this.CurrentVending.IsRentable.ToString();
							}
							else
							{
								value = "false";
							}
							return true;
						}
					}
				}
				else if (bindingName == "tradername")
				{
					value = "";
					if (this.xui.Trader.TraderData != null)
					{
						if (this.CurrentTraderEntity != null)
						{
							value = this.CurrentTraderEntity.EntityName;
						}
						else if (this.IsVending)
						{
							value = Localization.Get("VendingMachine", false, null);
						}
					}
					return true;
				}
			}
			else if (bindingName == "restocklabel")
			{
				value = Localization.Get("xuiRestock", false, null);
				return true;
			}
		}
		else if (num <= 3170511085U)
		{
			if (num != 1549422821U)
			{
				if (num != 2585672714U)
				{
					if (num == 3170511085U)
					{
						if (bindingName == "isrenter")
						{
							if (this.IsVending)
							{
								if (this.CurrentVending == null)
								{
									value = "false";
									return true;
								}
								value = (this.CurrentVending.IsRentable && this.IsOwner).ToString();
							}
							else
							{
								value = "false";
							}
							return true;
						}
					}
				}
				else if (bindingName == "isownerorrentable")
				{
					if (this.IsVending)
					{
						if (this.CurrentVending == null)
						{
							value = "false";
							return true;
						}
						value = (this.CurrentVending.IsRentable || (this.IsPlayerOwned && (this.IsOwner || this.CurrentVending.IsUserAllowed(PlatformManager.InternalLocalUserIdentifier)))).ToString();
					}
					else
					{
						value = "false";
					}
					return true;
				}
			}
			else if (bindingName == "isnotowner")
			{
				if (this.IsVending)
				{
					if (this.CurrentVending == null)
					{
						value = "true";
						return true;
					}
					value = ((!this.IsPlayerOwned && !this.IsRentable) || !this.CurrentVending.IsUserAllowed(PlatformManager.InternalLocalUserIdentifier)).ToString();
				}
				else
				{
					value = "false";
				}
				return true;
			}
		}
		else if (num != 3218764573U)
		{
			if (num != 3392636521U)
			{
				if (num == 4243210436U)
				{
					if (bindingName == "isowner")
					{
						if (this.IsVending)
						{
							if (this.CurrentVending == null)
							{
								value = "false";
								return true;
							}
							value = ((this.IsPlayerOwned || this.IsRentable) && this.CurrentVending.IsUserAllowed(PlatformManager.InternalLocalUserIdentifier)).ToString();
						}
						else
						{
							value = "false";
						}
						return true;
					}
				}
			}
			else if (bindingName == "is_debug")
			{
				value = (GamePrefs.GetBool(EnumGamePrefs.DebugMenuEnabled) && !this.IsVending).ToString();
				return true;
			}
		}
		else if (bindingName == "showrestock")
		{
			value = (this.xui.Trader.TraderData != null && this.xui.Trader.TraderData.TraderInfo.ResetInterval > 0).ToString();
			return true;
		}
		return base.GetBindingValueInternal(ref value, bindingName);
	}

	// Token: 0x0600876B RID: 34667 RVA: 0x0033C898 File Offset: 0x0033AA98
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (Time.time > this.updateTime)
		{
			this.updateTime = Time.time + 1f;
			if (this.xui.Trader.TraderData != null)
			{
				if (this.IsVending && this.IsRentable && this.CurrentVending.GetOwner() != null && this.CurrentVending.RentTimeRemaining <= 0f)
				{
					this.CurrentVending.ClearVendingMachine();
					this.Refresh();
					this.RefreshTraderItems();
				}
				if (this.xui.Trader.TraderData.CurrentTime <= 0f && !this.xui.Trader.TraderData.TraderInfo.PlayerOwned && !this.xui.Trader.TraderData.TraderInfo.Rentable && GameManager.Instance.traderManager.TraderInventoryRequested(this.xui.Trader.TraderData, XUiM_Player.GetPlayer().entityId))
				{
					this.xui.Trader.TraderData.SetModified(this.xui.Trader.Trader);
					XUiM_Player.GetPlayer().PlayOneShot("ui_trader_inv_reset", false, false, false, null, 1f);
					this.RefreshTraderItems();
					base.RefreshBindings();
				}
			}
		}
	}

	// Token: 0x0600876C RID: 34668 RVA: 0x0033C9F6 File Offset: 0x0033ABF6
	public void Refresh()
	{
		this.RefreshHeader();
		base.RefreshBindings();
	}

	// Token: 0x0400649D RID: 25757
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ServiceInfoWindow serviceInfoWindow;

	// Token: 0x0400649E RID: 25758
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TraderItemList itemListGrid;

	// Token: 0x0400649F RID: 25759
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CategoryList categoryList;

	// Token: 0x040064A0 RID: 25760
	[PublicizedFrom(EAccessModifier.Private)]
	public int page;

	// Token: 0x040064A1 RID: 25761
	[PublicizedFrom(EAccessModifier.Private)]
	public int length;

	// Token: 0x040064A2 RID: 25762
	[PublicizedFrom(EAccessModifier.Private)]
	public List<TraderData.Entry> buyInventory = new List<TraderData.Entry>();

	// Token: 0x040064A3 RID: 25763
	[PublicizedFrom(EAccessModifier.Private)]
	public List<int> currentIndexList = new List<int>();

	// Token: 0x040064A4 RID: 25764
	[PublicizedFrom(EAccessModifier.Private)]
	public List<ItemStack> currentInventory = new List<ItemStack>();

	// Token: 0x040064A5 RID: 25765
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtInput;

	// Token: 0x040064A6 RID: 25766
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController windowicon;

	// Token: 0x040064A7 RID: 25767
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button rentButton;

	// Token: 0x040064A8 RID: 25768
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button showAllButton;

	// Token: 0x040064A9 RID: 25769
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_Paging pager;

	// Token: 0x040064AA RID: 25770
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_PlayerName playerName;

	// Token: 0x040064AB RID: 25771
	public bool CompletedTransaction;

	// Token: 0x040064AC RID: 25772
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblGeneralStock;

	// Token: 0x040064AD RID: 25773
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblSecretStash;

	// Token: 0x040064AE RID: 25774
	[PublicizedFrom(EAccessModifier.Private)]
	public string category = "";

	// Token: 0x040064AF RID: 25775
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isSecretStash;

	// Token: 0x040064B0 RID: 25776
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasSecretStash;

	// Token: 0x040064B1 RID: 25777
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showAll;

	// Token: 0x040064B2 RID: 25778
	[PublicizedFrom(EAccessModifier.Private)]
	public int traderStage = -1;

	// Token: 0x040064B3 RID: 25779
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<int> rentTimeLeftFormatter = new CachedStringFormatter<int>((int _i) => string.Format("{0}: {1} {2}", Localization.Get("xuiExpires", false, null), Localization.Get("xuiDay", false, null), _i));

	// Token: 0x040064B4 RID: 25780
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<int> timeLeftFormatter = new CachedStringFormatter<int>((int _i) => string.Format("{0} {1}", Localization.Get("xuiDay", false, null), _i));

	// Token: 0x040064B5 RID: 25781
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt availableMoneyFormatter = new CachedStringFormatterInt();

	// Token: 0x040064B6 RID: 25782
	[PublicizedFrom(EAccessModifier.Private)]
	public float updateTime;

	// Token: 0x020010B0 RID: 4272
	public enum TraderActionTypes
	{
		// Token: 0x040064B8 RID: 25784
		Buy,
		// Token: 0x040064B9 RID: 25785
		Sell,
		// Token: 0x040064BA RID: 25786
		BuyBack
	}
}
