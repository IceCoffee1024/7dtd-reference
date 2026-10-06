using System;
using System.Collections.Generic;
using Audio;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200109F RID: 4255
[Preserve]
public class XUiC_TraderWindow : XUiController
{
	// Token: 0x17000FE8 RID: 4072
	// (get) Token: 0x060086FE RID: 34558 RVA: 0x00339376 File Offset: 0x00337576
	public TileEntityVendingMachine CurrentVending
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.xui.Trader.Trader as TileEntityVendingMachine;
		}
	}

	// Token: 0x17000FE9 RID: 4073
	// (get) Token: 0x060086FF RID: 34559 RVA: 0x0033938D File Offset: 0x0033758D
	public EntityTrader CurrentTraderEntity
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.xui.Trader.Trader as EntityTrader;
		}
	}

	// Token: 0x17000FEA RID: 4074
	// (get) Token: 0x06008700 RID: 34560 RVA: 0x003393A4 File Offset: 0x003375A4
	public bool IsVending
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.CurrentVending != null;
		}
	}

	// Token: 0x17000FEB RID: 4075
	// (get) Token: 0x06008701 RID: 34561 RVA: 0x003393AF File Offset: 0x003375AF
	public bool IsOwner
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.CurrentVending != null && this.CurrentVending.LocalPlayerIsOwner();
		}
	}

	// Token: 0x17000FEC RID: 4076
	// (get) Token: 0x06008702 RID: 34562 RVA: 0x003393C8 File Offset: 0x003375C8
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

	// Token: 0x17000FED RID: 4077
	// (get) Token: 0x06008703 RID: 34563 RVA: 0x0033941C File Offset: 0x0033761C
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

	// Token: 0x17000FEE RID: 4078
	// (get) Token: 0x06008704 RID: 34564 RVA: 0x0033946E File Offset: 0x0033766E
	// (set) Token: 0x06008705 RID: 34565 RVA: 0x00339476 File Offset: 0x00337676
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

	// Token: 0x06008706 RID: 34566 RVA: 0x003394B0 File Offset: 0x003376B0
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

	// Token: 0x06008707 RID: 34567 RVA: 0x003396F6 File Offset: 0x003378F6
	[PublicizedFrom(EAccessModifier.Private)]
	public void ShowAll_OnPress(XUiController _sender, int _mouseButton)
	{
		this.showAll = !this.showAll;
		this.showAllButton.Selected = this.showAll;
		this.RefreshTraderItems();
	}

	// Token: 0x06008708 RID: 34568 RVA: 0x00339720 File Offset: 0x00337920
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

	// Token: 0x06008709 RID: 34569 RVA: 0x003397F0 File Offset: 0x003379F0
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

	// Token: 0x0600870A RID: 34570 RVA: 0x0033988C File Offset: 0x00337A8C
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

	// Token: 0x0600870B RID: 34571 RVA: 0x00339A14 File Offset: 0x00337C14
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

	// Token: 0x0600870C RID: 34572 RVA: 0x00339A60 File Offset: 0x00337C60
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

	// Token: 0x0600870D RID: 34573 RVA: 0x00339B64 File Offset: 0x00337D64
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

	// Token: 0x0600870E RID: 34574 RVA: 0x00339BCE File Offset: 0x00337DCE
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

	// Token: 0x0600870F RID: 34575 RVA: 0x00339BFC File Offset: 0x00337DFC
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

	// Token: 0x06008710 RID: 34576 RVA: 0x00339CA8 File Offset: 0x00337EA8
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

	// Token: 0x06008711 RID: 34577 RVA: 0x00339FD9 File Offset: 0x003381D9
	public void SetCategory(string _category)
	{
		if (this.txtInput != null)
		{
			this.txtInput.Text = "";
		}
		this.category = _category;
		this.RefreshTraderItems();
	}

	// Token: 0x06008712 RID: 34578 RVA: 0x0033A000 File Offset: 0x00338200
	public string GetCategory()
	{
		return this.category;
	}

	// Token: 0x06008713 RID: 34579 RVA: 0x0033A008 File Offset: 0x00338208
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

	// Token: 0x06008714 RID: 34580 RVA: 0x0033A0F0 File Offset: 0x003382F0
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

	// Token: 0x06008715 RID: 34581 RVA: 0x0033A1D4 File Offset: 0x003383D4
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

	// Token: 0x06008716 RID: 34582 RVA: 0x0033A26C File Offset: 0x0033846C
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

	// Token: 0x06008717 RID: 34583 RVA: 0x0033A350 File Offset: 0x00338550
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

	// Token: 0x06008718 RID: 34584 RVA: 0x0033A814 File Offset: 0x00338A14
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

	// Token: 0x06008719 RID: 34585 RVA: 0x0033A972 File Offset: 0x00338B72
	public void Refresh()
	{
		this.RefreshHeader();
		base.RefreshBindings();
	}

	// Token: 0x0400645E RID: 25694
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ServiceInfoWindow serviceInfoWindow;

	// Token: 0x0400645F RID: 25695
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TraderItemList itemListGrid;

	// Token: 0x04006460 RID: 25696
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CategoryList categoryList;

	// Token: 0x04006461 RID: 25697
	[PublicizedFrom(EAccessModifier.Private)]
	public int page;

	// Token: 0x04006462 RID: 25698
	[PublicizedFrom(EAccessModifier.Private)]
	public int length;

	// Token: 0x04006463 RID: 25699
	[PublicizedFrom(EAccessModifier.Private)]
	public List<TraderData.Entry> buyInventory = new List<TraderData.Entry>();

	// Token: 0x04006464 RID: 25700
	[PublicizedFrom(EAccessModifier.Private)]
	public List<int> currentIndexList = new List<int>();

	// Token: 0x04006465 RID: 25701
	[PublicizedFrom(EAccessModifier.Private)]
	public List<ItemStack> currentInventory = new List<ItemStack>();

	// Token: 0x04006466 RID: 25702
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtInput;

	// Token: 0x04006467 RID: 25703
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController windowicon;

	// Token: 0x04006468 RID: 25704
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button rentButton;

	// Token: 0x04006469 RID: 25705
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button showAllButton;

	// Token: 0x0400646A RID: 25706
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_Paging pager;

	// Token: 0x0400646B RID: 25707
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_PlayerName playerName;

	// Token: 0x0400646C RID: 25708
	public bool CompletedTransaction;

	// Token: 0x0400646D RID: 25709
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblGeneralStock;

	// Token: 0x0400646E RID: 25710
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblSecretStash;

	// Token: 0x0400646F RID: 25711
	[PublicizedFrom(EAccessModifier.Private)]
	public string category = "";

	// Token: 0x04006470 RID: 25712
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isSecretStash;

	// Token: 0x04006471 RID: 25713
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasSecretStash;

	// Token: 0x04006472 RID: 25714
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showAll;

	// Token: 0x04006473 RID: 25715
	[PublicizedFrom(EAccessModifier.Private)]
	public int traderStage = -1;

	// Token: 0x04006474 RID: 25716
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<int> rentTimeLeftFormatter = new CachedStringFormatter<int>((int _i) => string.Format("{0}: {1} {2}", Localization.Get("xuiExpires", false, null), Localization.Get("xuiDay", false, null), _i));

	// Token: 0x04006475 RID: 25717
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<int> timeLeftFormatter = new CachedStringFormatter<int>((int _i) => string.Format("{0} {1}", Localization.Get("xuiDay", false, null), _i));

	// Token: 0x04006476 RID: 25718
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt availableMoneyFormatter = new CachedStringFormatterInt();

	// Token: 0x04006477 RID: 25719
	[PublicizedFrom(EAccessModifier.Private)]
	public float updateTime;

	// Token: 0x020010A0 RID: 4256
	public enum TraderActionTypes
	{
		// Token: 0x04006479 RID: 25721
		Buy,
		// Token: 0x0400647A RID: 25722
		Sell,
		// Token: 0x0400647B RID: 25723
		BuyBack
	}
}
