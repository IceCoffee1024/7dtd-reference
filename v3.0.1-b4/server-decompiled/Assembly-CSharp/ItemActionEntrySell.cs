using System;
using Audio;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000D4F RID: 3407
[Preserve]
public class ItemActionEntrySell : BaseItemActionEntry
{
	// Token: 0x06006799 RID: 26521 RVA: 0x0028EC00 File Offset: 0x0028CE00
	public ItemActionEntrySell(XUiController _controller)
		: base(_controller, "OVERRIDDEN BELOW", "OVERRIDDEN BELOW", BaseItemActionEntry.GamepadShortCut.None, "", "ui/ui_denied")
	{
		TileEntityVendingMachine tileEntityVendingMachine = _controller.xui.Trader.Trader as TileEntityVendingMachine;
		if (tileEntityVendingMachine != null)
		{
			TraderInfo traderInfo = _controller.xui.Trader.TraderData.TraderInfo;
			bool playerOwned = traderInfo.PlayerOwned;
			bool rentable = traderInfo.Rentable;
			this.isOwner = (playerOwned || rentable) && tileEntityVendingMachine.IsUserAllowed(PlatformManager.InternalLocalUserIdentifier);
		}
		else
		{
			this.isOwner = false;
		}
		if (this.isOwner)
		{
			base.ActionName = Localization.Get("lblContextActionAdd", false, null);
			base.IconName = "ui_game_symbol_hand";
			return;
		}
		base.ActionName = Localization.Get("lblContextActionSell", false, null);
		base.IconName = "ui_game_symbol_coin";
	}

	// Token: 0x0600679A RID: 26522 RVA: 0x0028ECCC File Offset: 0x0028CECC
	public override void RefreshEnabled()
	{
		XUiC_ItemStack xuiC_ItemStack = (XUiC_ItemStack)base.ItemController;
		int count = xuiC_ItemStack.InfoWindow.BuySellCounter.Count;
		this.state = ItemActionEntrySell.StateTypes.Normal;
		if (this.isOwner)
		{
			ItemStack itemStack = xuiC_ItemStack.ItemStack.Clone();
			if (count <= 0)
			{
				this.state = ItemActionEntrySell.StateTypes.NotEnoughItems;
			}
			if (!itemStack.IsEmpty())
			{
				float num = (itemStack.itemValue.ItemClass.IsBlock() ? Block.list[itemStack.itemValue.type].EconomicValue : itemStack.itemValue.ItemClass.EconomicValue);
				if (!itemStack.itemValue.ItemClass.CanDrop(null) || num <= 0f)
				{
					this.state = ItemActionEntrySell.StateTypes.ItemNotSellableVending;
				}
			}
		}
		else if (!xuiC_ItemStack.ItemStack.IsEmpty())
		{
			ItemStack itemStack2 = xuiC_ItemStack.ItemStack.Clone();
			ItemClass itemClass = itemStack2.itemValue.ItemClass;
			if (!(itemClass.IsBlock() ? Block.list[itemStack2.itemValue.type].SellableToTrader : itemClass.SellableToTrader))
			{
				this.state = ItemActionEntrySell.StateTypes.ItemNotSellable;
			}
			else if (XUiM_Trader.GetSellPrice(base.ItemController.xui, itemStack2.itemValue, count, itemClass) <= 0)
			{
				this.state = ItemActionEntrySell.StateTypes.PriceTooLow;
			}
		}
		base.Enabled = this.state == ItemActionEntrySell.StateTypes.Normal;
	}

	// Token: 0x0600679B RID: 26523 RVA: 0x0028EE20 File Offset: 0x0028D020
	public override void OnDisabledActivate()
	{
		EntityPlayerLocal entityPlayer = base.ItemController.xui.playerUI.entityPlayer;
		switch (this.state)
		{
		case ItemActionEntrySell.StateTypes.ItemNotSellable:
		case ItemActionEntrySell.StateTypes.PriceTooLow:
		{
			GameManager.ShowTooltip(entityPlayer, Localization.Get("ttCannotSellItem", false, null), false, false, 0.5f);
			EntityTrader entityTrader = base.ItemController.xui.Trader.Trader as EntityTrader;
			if (entityTrader != null)
			{
				entityTrader.PlayVoiceSetEntry("refuse", entityPlayer, true, true);
			}
			break;
		}
		case ItemActionEntrySell.StateTypes.ItemNotSellableVending:
			GameManager.ShowTooltip(entityPlayer, "You cannot sell this item.", false, false, 0f);
			break;
		}
		Manager.PlayInsidePlayerHead("ui_denied", -1, 0f, false, false);
	}

	// Token: 0x0600679C RID: 26524 RVA: 0x0028EECC File Offset: 0x0028D0CC
	public override void OnActivated()
	{
		TraderData traderData = base.ItemController.xui.Trader.TraderData;
		try
		{
			EntityPlayerLocal entityPlayer = base.ItemController.xui.playerUI.entityPlayer;
			XUiC_ItemStack xuiC_ItemStack = (XUiC_ItemStack)base.ItemController;
			ItemValue itemValue = xuiC_ItemStack.ItemStack.itemValue;
			ItemClass forId = ItemClass.GetForId(xuiC_ItemStack.ItemStack.itemValue.type);
			ItemValue item = ItemClass.GetItem(TraderInfo.CurrencyItem, false);
			if (!xuiC_ItemStack.ItemStack.IsEmpty() && forId != null)
			{
				int num = (forId.IsBlock() ? Block.list[itemValue.type].EconomicBundleSize : forId.EconomicBundleSize);
				if (xuiC_ItemStack.ItemStack.count < num)
				{
					GameManager.ShowTooltip(entityPlayer, string.Format(Localization.Get("ttItemCountNotBundleSize", false, null), num), false, false, 0f);
					Manager.PlayInsidePlayerHead("ui_denied", -1, 0f, false, false);
				}
				else if (this.isOwner)
				{
					int num2 = xuiC_ItemStack.InfoWindow.BuySellCounter.Count;
					if (xuiC_ItemStack.ItemStack.itemValue.type == item.type)
					{
						if (traderData.AvailableMoney + num2 > 6 * traderData.TraderInfo.RentCost)
						{
							GameManager.ShowTooltip(entityPlayer, Localization.Get("ttVendingMachineMaxCoin", false, null), false, false, 0f);
							Manager.PlayInsidePlayerHead("ui_denied", -1, 0f, false, false);
							return;
						}
						traderData.AvailableMoney += num2;
						base.ItemController.xui.Trader.TraderWindowGroup.RefreshTraderWindow();
					}
					else
					{
						int num3 = 50;
						if (traderData.PrimaryInventory.Count == num3)
						{
							int num4 = num2;
							for (int i = 0; i < traderData.PrimaryInventory.Count; i++)
							{
								if (traderData.PrimaryInventory[i].Item.itemValue.type == xuiC_ItemStack.ItemStack.itemValue.type)
								{
									int num5 = forId.Stacknumber.Value - traderData.PrimaryInventory[i].Item.count;
									num4 -= num5;
									if (num4 <= 0)
									{
										break;
									}
								}
							}
							if (num4 > 0)
							{
								GameManager.ShowTooltip(entityPlayer, Localization.Get("ttVendingMachineMaxItems", false, null), false, false, 0f);
								Manager.PlayInsidePlayerHead("ui_denied", -1, 0f, false, false);
								return;
							}
						}
						int num6 = num2 % num;
						if (num6 != 0)
						{
							GameManager.ShowTooltip(entityPlayer, string.Format(Localization.Get("ttItemCountNotBundleSize", false, null), num), false, false, 0f);
							num2 -= num6;
							xuiC_ItemStack.InfoWindow.BuySellCounter.Count = num2;
							Manager.PlayInsidePlayerHead("ui_denied", -1, 0f, false, false);
							return;
						}
						ItemStack itemStack = xuiC_ItemStack.ItemStack.Clone();
						itemStack = ItemActionEntryScrap.HandleRemoveAmmo(itemStack, base.ItemController.xui);
						itemStack.count = num2;
						traderData.AddToPrimaryInventory(itemStack, true);
					}
					base.ItemController.xui.Trader.TraderWindowGroup.RefreshTraderItems();
					Manager.PlayInsidePlayerHead("craft_place_item", -1, 0f, false, false);
					xuiC_ItemStack.ItemStack.count -= num2;
					xuiC_ItemStack.InfoWindow.BuySellCounter.MaxCount -= num2;
					if (xuiC_ItemStack.ItemStack.count == 0)
					{
						xuiC_ItemStack.ItemStack = ItemStack.Empty;
						xuiC_ItemStack.IsSelected = false;
					}
					else
					{
						xuiC_ItemStack.ForceRefreshItemStack();
					}
				}
				else
				{
					int count = xuiC_ItemStack.InfoWindow.BuySellCounter.Count;
					int primaryItemCount = traderData.GetPrimaryItemCount(xuiC_ItemStack.ItemStack.itemValue);
					int num7 = forId.Stacknumber.Value * TraderInfo.TraderBuyLimit;
					int num8 = Math.Min(num7 - primaryItemCount, count);
					if (num7 > 0 && num8 <= 0)
					{
						GameManager.ShowTooltip(entityPlayer, Localization.Get("ttCannotSellItem", false, null), false, false, 0f);
						Manager.PlayInsidePlayerHead("ui_denied", -1, 0f, false, false);
					}
					else if (num7 > 0 && num8 != count)
					{
						GameManager.ShowTooltip(entityPlayer, string.Format(Localization.Get("ttCanOnlySellAmount", false, null), num8), false, false, 0f);
						xuiC_ItemStack.InfoWindow.BuySellCounter.Count = num8;
						Manager.PlayInsidePlayerHead("ui_denied", -1, 0f, false, false);
					}
					else
					{
						int num9 = num8 % num;
						if (num9 != 0)
						{
							GameManager.ShowTooltip(entityPlayer, string.Format(Localization.Get("ttItemCountNotBundleSize", false, null), num), false, false, 0f);
							num8 -= num9;
							xuiC_ItemStack.InfoWindow.BuySellCounter.Count = num8;
							Manager.PlayInsidePlayerHead("ui_denied", -1, 0f, false, false);
						}
						else
						{
							ItemStack itemStack2 = xuiC_ItemStack.ItemStack.Clone();
							int sellPrice = XUiM_Trader.GetSellPrice(base.ItemController.xui, itemStack2.itemValue, count, forId);
							XUiM_PlayerInventory playerInventory = base.ItemController.xui.PlayerInventory;
							ItemStack itemStack3 = new ItemStack(item.Clone(), sellPrice);
							itemStack2.count = count;
							int slotCount = xuiC_ItemStack.xui.PlayerInventory.Backpack.SlotCount;
							int num10 = xuiC_ItemStack.SlotNumber + ((xuiC_ItemStack.StackLocation == XUiC_ItemStack.StackLocationTypes.ToolBelt) ? slotCount : 0);
							if (playerInventory.CanSwapItems(itemStack2, itemStack3, num10))
							{
								itemStack2 = ItemActionEntryScrap.HandleRemoveAmmo(itemStack2, base.ItemController.xui);
								xuiC_ItemStack.ItemStack.count -= count;
								xuiC_ItemStack.InfoWindow.BuySellCounter.MaxCount -= count;
								if (xuiC_ItemStack.ItemStack.count == 0)
								{
									xuiC_ItemStack.ItemStack = ItemStack.Empty;
									xuiC_ItemStack.IsSelected = false;
								}
								else
								{
									xuiC_ItemStack.ForceRefreshItemStack();
								}
								traderData.AddToPrimaryInventory(itemStack2, true);
								if (base.ItemController.xui.Trader.TraderWindowGroup != null)
								{
									base.ItemController.xui.Trader.TraderWindowGroup.RefreshTraderItems();
								}
								EntityTrader entityTrader = base.ItemController.xui.Trader.Trader as EntityTrader;
								if (entityTrader != null)
								{
									QuestEventManager.Current.SoldItems(entityTrader.EntityName, count);
									entityPlayer.Progression.AddLevelExp(Math.Max(Convert.ToInt32(sellPrice), 1), "_xpFromSelling", Progression.XPTypes.Selling, true, true, -1, null);
								}
								playerInventory.AddItem(itemStack3, false);
								Manager.PlayInsidePlayerHead("ui_trader_purchase", -1, 0f, false, false);
							}
							else
							{
								GameManager.ShowTooltip(entityPlayer, Localization.Get("ttNoSpaceForSelling", false, null), false, false, 0f);
								Manager.PlayInsidePlayerHead("ui_denied", -1, 0f, false, false);
							}
						}
					}
				}
			}
		}
		finally
		{
			traderData.SetModified(base.ItemController.xui.Trader.Trader);
		}
	}

	// Token: 0x04004F64 RID: 20324
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly bool isOwner;

	// Token: 0x04004F65 RID: 20325
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemActionEntrySell.StateTypes state;

	// Token: 0x02000D50 RID: 3408
	[PublicizedFrom(EAccessModifier.Private)]
	public enum StateTypes
	{
		// Token: 0x04004F67 RID: 20327
		Normal,
		// Token: 0x04004F68 RID: 20328
		ItemNotSellable,
		// Token: 0x04004F69 RID: 20329
		ItemNotSellableVending,
		// Token: 0x04004F6A RID: 20330
		PriceTooLow,
		// Token: 0x04004F6B RID: 20331
		NotEnoughItems,
		// Token: 0x04004F6C RID: 20332
		NotBundleSize
	}
}
