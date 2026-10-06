using System;
using Audio;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000D73 RID: 3443
[Preserve]
public class ItemActionEntrySell : BaseItemActionEntry
{
	// Token: 0x0600682F RID: 26671 RVA: 0x0029514C File Offset: 0x0029334C
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

	// Token: 0x06006830 RID: 26672 RVA: 0x00295218 File Offset: 0x00293418
	public override void RefreshEnabled()
	{
		IItemStackController itemStackController = (IItemStackController)base.ItemController;
		int count = itemStackController.InfoWindow.BuySellCounter.Count;
		this.state = ItemActionEntrySell.StateTypes.Normal;
		if (this.isOwner)
		{
			ItemStack itemStack = itemStackController.ItemStack.Clone();
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
		else if (!itemStackController.ItemStack.IsEmpty())
		{
			ItemStack itemStack2 = itemStackController.ItemStack.Clone();
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

	// Token: 0x06006831 RID: 26673 RVA: 0x0029536C File Offset: 0x0029356C
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

	// Token: 0x06006832 RID: 26674 RVA: 0x00295418 File Offset: 0x00293618
	public override void OnActivated()
	{
		TraderData traderData = base.ItemController.xui.Trader.TraderData;
		try
		{
			EntityPlayerLocal entityPlayer = base.ItemController.xui.playerUI.entityPlayer;
			IItemStackController itemStackController = (IItemStackController)base.ItemController;
			ItemValue itemValue = itemStackController.ItemStack.itemValue;
			ItemClass forId = ItemClass.GetForId(itemStackController.ItemStack.itemValue.type);
			ItemValue item = ItemClass.GetItem(TraderInfo.CurrencyItem, false);
			if (!itemStackController.ItemStack.IsEmpty() && forId != null)
			{
				int num = (forId.IsBlock() ? Block.list[itemValue.type].EconomicBundleSize : forId.EconomicBundleSize);
				if (itemStackController.ItemStack.count < num)
				{
					GameManager.ShowTooltip(entityPlayer, string.Format(Localization.Get("ttItemCountNotBundleSize", false, null), num), false, false, 0f);
					Manager.PlayInsidePlayerHead("ui_denied", -1, 0f, false, false);
				}
				else if (this.isOwner)
				{
					int num2 = itemStackController.InfoWindow.BuySellCounter.Count;
					if (itemStackController.ItemStack.itemValue.type == item.type)
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
								if (traderData.PrimaryInventory[i].Item.itemValue.type == itemStackController.ItemStack.itemValue.type)
								{
									int num5 = forId.MaxCount - traderData.PrimaryInventory[i].Item.count;
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
							itemStackController.InfoWindow.BuySellCounter.Count = num2;
							Manager.PlayInsidePlayerHead("ui_denied", -1, 0f, false, false);
							return;
						}
						ItemStack itemStack = itemStackController.ItemStack.Clone();
						itemStack = ItemActionEntryScrap.HandleRemoveAmmo(itemStack, base.ItemController.xui);
						itemStack.count = num2;
						traderData.AddToPrimaryInventory(itemStack, true);
					}
					base.ItemController.xui.Trader.TraderWindowGroup.RefreshTraderItems();
					Manager.PlayInsidePlayerHead("craft_place_item", -1, 0f, false, false);
					itemStackController.ItemStack.count -= num2;
					itemStackController.InfoWindow.BuySellCounter.MaxCount -= num2;
					if (itemStackController.ItemStack.count == 0)
					{
						itemStackController.ItemStack = ItemStack.Empty;
						itemStackController.IsSelected = false;
					}
					else
					{
						itemStackController.ForceRefreshItemStack();
					}
				}
				else
				{
					int count = itemStackController.InfoWindow.BuySellCounter.Count;
					int primaryItemCount = traderData.GetPrimaryItemCount(itemStackController.ItemStack.itemValue);
					int num7 = forId.MaxCount * TraderInfo.TraderBuyLimit;
					int num8 = Math.Min(num7 - primaryItemCount, count);
					if (num7 > 0 && num8 <= 0)
					{
						GameManager.ShowTooltip(entityPlayer, Localization.Get("ttCannotSellItem", false, null), false, false, 0f);
						Manager.PlayInsidePlayerHead("ui_denied", -1, 0f, false, false);
					}
					else if (num7 > 0 && num8 != count)
					{
						GameManager.ShowTooltip(entityPlayer, string.Format(Localization.Get("ttCanOnlySellAmount", false, null), num8), false, false, 0f);
						itemStackController.InfoWindow.BuySellCounter.Count = num8;
						Manager.PlayInsidePlayerHead("ui_denied", -1, 0f, false, false);
					}
					else
					{
						int num9 = num8 % num;
						if (num9 != 0)
						{
							GameManager.ShowTooltip(entityPlayer, string.Format(Localization.Get("ttItemCountNotBundleSize", false, null), num), false, false, 0f);
							num8 -= num9;
							itemStackController.InfoWindow.BuySellCounter.Count = num8;
							Manager.PlayInsidePlayerHead("ui_denied", -1, 0f, false, false);
						}
						else
						{
							ItemStack itemStack2 = itemStackController.ItemStack.Clone();
							int sellPrice = XUiM_Trader.GetSellPrice(base.ItemController.xui, itemStack2.itemValue, count, forId);
							XUiM_PlayerInventory playerInventory = base.ItemController.xui.PlayerInventory;
							ItemStack itemStack3 = new ItemStack(item.Clone(), sellPrice);
							itemStack2.count = count;
							int slotCount = itemStackController.xui.PlayerInventory.Backpack.SlotCount;
							int num10 = itemStackController.SlotNumber + ((itemStackController.StackLocation == XUiC_ItemStack.StackLocationTypes.ToolBelt) ? slotCount : 0);
							if (playerInventory.CanSwapItems(itemStack2, itemStack3, num10))
							{
								itemStack2 = ItemActionEntryScrap.HandleRemoveAmmo(itemStack2, base.ItemController.xui);
								itemStackController.ItemStack.count -= count;
								itemStackController.InfoWindow.BuySellCounter.MaxCount -= count;
								if (itemStackController.ItemStack.count == 0)
								{
									itemStackController.ItemStack = ItemStack.Empty;
									itemStackController.IsSelected = false;
								}
								else
								{
									itemStackController.ForceRefreshItemStack();
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

	// Token: 0x04005041 RID: 20545
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly bool isOwner;

	// Token: 0x04005042 RID: 20546
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemActionEntrySell.StateTypes state;

	// Token: 0x02000D74 RID: 3444
	[PublicizedFrom(EAccessModifier.Private)]
	public enum StateTypes
	{
		// Token: 0x04005044 RID: 20548
		Normal,
		// Token: 0x04005045 RID: 20549
		ItemNotSellable,
		// Token: 0x04005046 RID: 20550
		ItemNotSellableVending,
		// Token: 0x04005047 RID: 20551
		PriceTooLow,
		// Token: 0x04005048 RID: 20552
		NotEnoughItems,
		// Token: 0x04005049 RID: 20553
		NotBundleSize
	}
}
