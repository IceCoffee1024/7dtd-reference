using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000A04 RID: 2564
[Preserve]
public class RewardItem : BaseReward
{
	// Token: 0x06004BE6 RID: 19430 RVA: 0x001D93F0 File Offset: 0x001D75F0
	public override void SetupReward()
	{
		ItemClass itemClass = ItemClass.GetItemClass(base.ID, false);
		base.Description = itemClass.GetLocalizedItemName();
		base.ValueText = base.Value;
		string text = itemClass.Groups[0].ToLower();
		uint num = <PrivateImplementationDetails>.ComputeStringHash(text);
		if (num <= 1893056498U)
		{
			if (num <= 332883202U)
			{
				if (num != 46554813U)
				{
					if (num != 131445777U)
					{
						if (num == 332883202U)
						{
							if (text == "food/cooking")
							{
								base.Icon = "ui_game_symbol_fork";
							}
						}
					}
					else if (text == "books")
					{
						base.Icon = "ui_game_symbol_book";
					}
				}
				else if (text == "science")
				{
					base.Icon = "ui_game_symbol_science";
				}
			}
			else if (num != 481379405U)
			{
				if (num != 954139509U)
				{
					if (num == 1893056498U)
					{
						if (text == "resources")
						{
							base.Icon = "ui_game_symbol_resource";
						}
					}
				}
				else if (text == "building")
				{
					base.Icon = "ui_game_symbol_map_house";
				}
			}
			else if (text == "clothing")
			{
				base.Icon = "ui_game_symbol_shirt";
			}
		}
		else if (num <= 2154995914U)
		{
			if (num != 1917176822U)
			{
				if (num != 2115735777U)
				{
					if (num == 2154995914U)
					{
						if (text == "basics")
						{
							base.Icon = "ui_game_symbol_shopping_cart";
						}
					}
				}
				else if (text == "decor/miscellaneous")
				{
					base.Icon = "ui_game_symbol_chair";
				}
			}
			else if (text == "chemicals")
			{
				base.Icon = "ui_game_symbol_water";
			}
		}
		else if (num <= 3292735525U)
		{
			if (num != 2816984135U)
			{
				if (num == 3292735525U)
				{
					if (text == "ammo/weapons")
					{
						base.Icon = "ui_game_symbol_knife";
					}
				}
			}
			else if (text == "tools/traps")
			{
				base.Icon = "ui_game_symbol_tool";
			}
		}
		else if (num != 4134465488U)
		{
			if (num == 4185622628U)
			{
				if (text == "special items")
				{
					base.Icon = "ui_game_symbol_book";
				}
			}
		}
		else if (text == "mods")
		{
			base.Icon = "ui_game_symbol_assemble";
		}
		base.IconAtlas = "ItemIconAtlas";
	}

	// Token: 0x1700083D RID: 2109
	// (get) Token: 0x06004BE7 RID: 19431 RVA: 0x001D96C5 File Offset: 0x001D78C5
	public ItemStack Item
	{
		get
		{
			if (this.item == null || this.item.IsEmpty())
			{
				this.SetupItem();
			}
			return this.item;
		}
	}

	// Token: 0x06004BE8 RID: 19432 RVA: 0x001D96E8 File Offset: 0x001D78E8
	public override void Read(PooledBinaryReader _br)
	{
		base.Read(_br);
		this.item = new ItemStack();
		this.item.Read(_br);
	}

	// Token: 0x06004BE9 RID: 19433 RVA: 0x001D9709 File Offset: 0x001D7909
	public override void Write(PooledBinaryWriter _bw)
	{
		base.Write(_bw);
		if (this.item == null)
		{
			this.item = ItemStack.Empty;
		}
		this.item.Write(_bw);
	}

	// Token: 0x06004BEA RID: 19434 RVA: 0x001D9734 File Offset: 0x001D7934
	public override void GiveReward(EntityPlayer player)
	{
		this.item = this.GetRewardItem();
		XUiM_PlayerInventory playerInventory = LocalPlayerUI.GetUIForPlayer(base.OwnerQuest.OwnerJournal.OwnerPlayer).xui.PlayerInventory;
		if (!playerInventory.AddItem(this.Item))
		{
			playerInventory.DropItem(this.Item);
		}
	}

	// Token: 0x06004BEB RID: 19435 RVA: 0x001D9788 File Offset: 0x001D7988
	public override ItemStack GetRewardItem()
	{
		ItemStack itemStack = this.Item.Clone();
		if (!itemStack.itemValue.ItemClass.HasAllTags(FastTags<TagGroup.Global>.Parse("dukes")))
		{
			return itemStack;
		}
		int num = Mathf.FloorToInt(EffectManager.GetValue(PassiveEffects.QuestBonusItemReward, null, (float)itemStack.count, base.OwnerQuest.OwnerJournal.OwnerPlayer, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false));
		itemStack.count = num;
		return itemStack;
	}

	// Token: 0x06004BEC RID: 19436 RVA: 0x001D9804 File Offset: 0x001D7A04
	public void SetupItem()
	{
		ItemValue itemValue = ItemClass.GetItem(base.ID, false);
		ItemValue itemValue2 = new ItemValue(ItemClass.GetItem(base.ID, false).type, true);
		int num = 1;
		if (base.Value != null && base.Value != "")
		{
			if (int.TryParse(base.Value, out num))
			{
				if (itemValue2.HasQuality)
				{
					itemValue2 = new ItemValue(itemValue.type, num, num, true, null, 1f);
					num = 1;
				}
				else
				{
					itemValue2 = new ItemValue(itemValue.type, true);
				}
			}
			else if (base.Value.Contains("-"))
			{
				string[] array = base.Value.Split('-', StringSplitOptions.None);
				int num2 = Convert.ToInt32(array[0]);
				int num3 = Convert.ToInt32(array[1]);
				if (itemValue2.HasQuality)
				{
					itemValue2 = new ItemValue(itemValue.type, num2, num3, true, null, 1f);
					num = 1;
				}
				else
				{
					itemValue2 = new ItemValue(itemValue.type, true);
					num = GameManager.Instance.World.GetGameRandom().RandomRange(num2, num3);
				}
			}
		}
		this.item = new ItemStack(itemValue2, num);
	}

	// Token: 0x06004BED RID: 19437 RVA: 0x001D9920 File Offset: 0x001D7B20
	public override BaseReward Clone()
	{
		RewardItem rewardItem = new RewardItem();
		base.CopyValues(rewardItem);
		if (this.item != null)
		{
			rewardItem.item = this.item.Clone();
		}
		return rewardItem;
	}

	// Token: 0x06004BEE RID: 19438 RVA: 0x001D9954 File Offset: 0x001D7B54
	public override string GetRewardText()
	{
		return this.Item.count.ToString() + " x " + this.Item.itemValue.ItemClass.GetLocalizedItemName();
	}

	// Token: 0x04003BF4 RID: 15348
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack item;
}
