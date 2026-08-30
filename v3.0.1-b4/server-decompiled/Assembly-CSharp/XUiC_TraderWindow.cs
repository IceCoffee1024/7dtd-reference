using System;
using System.Collections.Generic;
using Audio;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001075 RID: 4213
[Preserve]
public class XUiC_TraderWindow : XUiController
{
	// Token: 0x17000F7A RID: 3962
	// (get) Token: 0x060085BD RID: 34237 RVA: 0x00330D36 File Offset: 0x0032EF36
	public TileEntityVendingMachine CurrentVending
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.xui.Trader.Trader as TileEntityVendingMachine;
		}
	}

	// Token: 0x17000F7B RID: 3963
	// (get) Token: 0x060085BE RID: 34238 RVA: 0x00330D4D File Offset: 0x0032EF4D
	public EntityTrader CurrentTraderEntity
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.xui.Trader.Trader as EntityTrader;
		}
	}

	// Token: 0x17000F7C RID: 3964
	// (get) Token: 0x060085BF RID: 34239 RVA: 0x00330D64 File Offset: 0x0032EF64
	public bool IsVending
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.CurrentVending != null;
		}
	}

	// Token: 0x17000F7D RID: 3965
	// (get) Token: 0x060085C0 RID: 34240 RVA: 0x00330D6F File Offset: 0x0032EF6F
	public bool IsOwner
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.CurrentVending != null && this.CurrentVending.LocalPlayerIsOwner();
		}
	}

	// Token: 0x17000F7E RID: 3966
	// (get) Token: 0x060085C1 RID: 34241 RVA: 0x00330D88 File Offset: 0x0032EF88
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

	// Token: 0x17000F7F RID: 3967
	// (get) Token: 0x060085C2 RID: 34242 RVA: 0x00330DDC File Offset: 0x0032EFDC
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

	// Token: 0x17000F80 RID: 3968
	// (get) Token: 0x060085C3 RID: 34243 RVA: 0x00330E2E File Offset: 0x0032F02E
	// (set) Token: 0x060085C4 RID: 34244 RVA: 0x00330E36 File Offset: 0x0032F036
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

	// Token: 0x060085C5 RID: 34245 RVA: 0x00330E70 File Offset: 0x0032F070
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

	// Token: 0x060085C6 RID: 34246 RVA: 0x003310B6 File Offset: 0x0032F2B6
	[PublicizedFrom(EAccessModifier.Private)]
	public void ShowAll_OnPress(XUiController _sender, int _mouseButton)
	{
		this.showAll = !this.showAll;
		this.showAllButton.Selected = this.showAll;
		this.RefreshTraderItems();
	}

	// Token: 0x060085C7 RID: 34247 RVA: 0x003310E0 File Offset: 0x0032F2E0
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

	// Token: 0x060085C8 RID: 34248 RVA: 0x003311B0 File Offset: 0x0032F3B0
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

	// Token: 0x060085C9 RID: 34249 RVA: 0x0033124C File Offset: 0x0032F44C
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

	// Token: 0x060085CA RID: 34250 RVA: 0x003313D4 File Offset: 0x0032F5D4
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

	// Token: 0x060085CB RID: 34251 RVA: 0x00331420 File Offset: 0x0032F620
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

	// Token: 0x060085CC RID: 34252 RVA: 0x00331524 File Offset: 0x0032F724
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

	// Token: 0x060085CD RID: 34253 RVA: 0x0033158E File Offset: 0x0032F78E
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

	// Token: 0x060085CE RID: 34254 RVA: 0x003315BC File Offset: 0x0032F7BC
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

	// Token: 0x060085CF RID: 34255 RVA: 0x00331668 File Offset: 0x0032F868
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

	// Token: 0x060085D0 RID: 34256 RVA: 0x00331999 File Offset: 0x0032FB99
	public void SetCategory(string _category)
	{
		if (this.txtInput != null)
		{
			this.txtInput.Text = "";
		}
		this.category = _category;
		this.RefreshTraderItems();
	}

	// Token: 0x060085D1 RID: 34257 RVA: 0x003319C0 File Offset: 0x0032FBC0
	public string GetCategory()
	{
		return this.category;
	}

	// Token: 0x060085D2 RID: 34258 RVA: 0x003319C8 File Offset: 0x0032FBC8
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

	// Token: 0x060085D3 RID: 34259 RVA: 0x00331AB0 File Offset: 0x0032FCB0
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

	// Token: 0x060085D4 RID: 34260 RVA: 0x00331B94 File Offset: 0x0032FD94
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

	// Token: 0x060085D5 RID: 34261 RVA: 0x00331C2C File Offset: 0x0032FE2C
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

	// Token: 0x060085D6 RID: 34262 RVA: 0x00331D10 File Offset: 0x0032FF10
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

	// Token: 0x060085D7 RID: 34263 RVA: 0x003321D4 File Offset: 0x003303D4
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

	// Token: 0x060085D8 RID: 34264 RVA: 0x00332332 File Offset: 0x00330532
	public void Refresh()
	{
		this.RefreshHeader();
		base.RefreshBindings();
	}

	// Token: 0x04006355 RID: 25429
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ServiceInfoWindow serviceInfoWindow;

	// Token: 0x04006356 RID: 25430
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TraderItemList itemListGrid;

	// Token: 0x04006357 RID: 25431
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CategoryList categoryList;

	// Token: 0x04006358 RID: 25432
	[PublicizedFrom(EAccessModifier.Private)]
	public int page;

	// Token: 0x04006359 RID: 25433
	[PublicizedFrom(EAccessModifier.Private)]
	public int length;

	// Token: 0x0400635A RID: 25434
	[PublicizedFrom(EAccessModifier.Private)]
	public List<TraderData.Entry> buyInventory = new List<TraderData.Entry>();

	// Token: 0x0400635B RID: 25435
	[PublicizedFrom(EAccessModifier.Private)]
	public List<int> currentIndexList = new List<int>();

	// Token: 0x0400635C RID: 25436
	[PublicizedFrom(EAccessModifier.Private)]
	public List<ItemStack> currentInventory = new List<ItemStack>();

	// Token: 0x0400635D RID: 25437
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtInput;

	// Token: 0x0400635E RID: 25438
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController windowicon;

	// Token: 0x0400635F RID: 25439
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button rentButton;

	// Token: 0x04006360 RID: 25440
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button showAllButton;

	// Token: 0x04006361 RID: 25441
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_Paging pager;

	// Token: 0x04006362 RID: 25442
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_PlayerName playerName;

	// Token: 0x04006363 RID: 25443
	public bool CompletedTransaction;

	// Token: 0x04006364 RID: 25444
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblGeneralStock;

	// Token: 0x04006365 RID: 25445
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblSecretStash;

	// Token: 0x04006366 RID: 25446
	[PublicizedFrom(EAccessModifier.Private)]
	public string category = "";

	// Token: 0x04006367 RID: 25447
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isSecretStash;

	// Token: 0x04006368 RID: 25448
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasSecretStash;

	// Token: 0x04006369 RID: 25449
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showAll;

	// Token: 0x0400636A RID: 25450
	[PublicizedFrom(EAccessModifier.Private)]
	public int traderStage = -1;

	// Token: 0x0400636B RID: 25451
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<int> rentTimeLeftFormatter = new CachedStringFormatter<int>((int _i) => string.Format("{0}: {1} {2}", Localization.Get("xuiExpires", false, null), Localization.Get("xuiDay", false, null), _i));

	// Token: 0x0400636C RID: 25452
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<int> timeLeftFormatter = new CachedStringFormatter<int>((int _i) => string.Format("{0} {1}", Localization.Get("xuiDay", false, null), _i));

	// Token: 0x0400636D RID: 25453
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt availableMoneyFormatter = new CachedStringFormatterInt();

	// Token: 0x0400636E RID: 25454
	[PublicizedFrom(EAccessModifier.Private)]
	public float updateTime;

	// Token: 0x02001076 RID: 4214
	public enum TraderActionTypes
	{
		// Token: 0x04006370 RID: 25456
		Buy,
		// Token: 0x04006371 RID: 25457
		Sell,
		// Token: 0x04006372 RID: 25458
		BuyBack
	}
}
