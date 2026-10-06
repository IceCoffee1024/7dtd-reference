using System;
using System.Globalization;
using UnityEngine.Scripting;

// Token: 0x020009FC RID: 2556
[Preserve]
public class RewardLootItem : BaseReward
{
	// Token: 0x06004BD0 RID: 19408 RVA: 0x001D8974 File Offset: 0x001D6B74
	public override void SetupReward()
	{
		this.LootGameStage = Convert.ToInt32(base.Value);
		ItemClass itemClass = this.Item.itemValue.ItemClass;
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

	// Token: 0x17000838 RID: 2104
	// (get) Token: 0x06004BD1 RID: 19409 RVA: 0x001D8C5E File Offset: 0x001D6E5E
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

	// Token: 0x06004BD2 RID: 19410 RVA: 0x001D8C81 File Offset: 0x001D6E81
	public override void Read(PooledBinaryReader _br)
	{
		base.Read(_br);
		this.item = new ItemStack();
		this.item.Read(_br);
	}

	// Token: 0x06004BD3 RID: 19411 RVA: 0x001D8CA2 File Offset: 0x001D6EA2
	public override void Write(PooledBinaryWriter _bw)
	{
		base.Write(_bw);
		if (this.item == null)
		{
			this.item = ItemStack.Empty;
		}
		this.item.Write(_bw);
	}

	// Token: 0x06004BD4 RID: 19412 RVA: 0x001D8CCC File Offset: 0x001D6ECC
	public override void GiveReward(EntityPlayer player)
	{
		XUiM_PlayerInventory playerInventory = LocalPlayerUI.GetUIForPlayer(base.OwnerQuest.OwnerJournal.OwnerPlayer).xui.PlayerInventory;
		if (!playerInventory.AddItem(this.Item))
		{
			playerInventory.DropItem(this.Item);
		}
	}

	// Token: 0x06004BD5 RID: 19413 RVA: 0x001D8D13 File Offset: 0x001D6F13
	public override ItemStack GetRewardItem()
	{
		return this.item.Clone();
	}

	// Token: 0x06004BD6 RID: 19414 RVA: 0x001D8D20 File Offset: 0x001D6F20
	public void SetupItem()
	{
		string[] array = base.ID.Split(',', StringSplitOptions.None);
		int i = 10;
		if (!string.IsNullOrEmpty(base.Value))
		{
			this.LootGameStage = StringParsers.ParseSInt32(base.Value, 0, -1, NumberStyles.Integer);
		}
		while (i > 0)
		{
			if (array.Length > 1)
			{
				World world = GameManager.Instance.World;
				this.item = LootContainer.GetRewardItem(array[world.GetGameRandom().RandomRange(array.Length)], (float)this.LootGameStage);
			}
			else if (array.Length == 1)
			{
				this.item = LootContainer.GetRewardItem(base.ID, (float)this.LootGameStage);
			}
			bool flag = false;
			for (int j = 0; j < base.OwnerQuest.Rewards.Count; j++)
			{
				RewardLootItem rewardLootItem = base.OwnerQuest.Rewards[j] as RewardLootItem;
				if (rewardLootItem != null)
				{
					if (rewardLootItem == this)
					{
						flag = true;
						break;
					}
					if (rewardLootItem.Item.itemValue.type == this.item.itemValue.type)
					{
						break;
					}
				}
			}
			if (flag)
			{
				break;
			}
			i--;
		}
		this.item.itemValue.UseTimes = 0f;
		this.item.itemValue.MaxDurabilityModifier = 1f;
	}

	// Token: 0x06004BD7 RID: 19415 RVA: 0x001D8E60 File Offset: 0x001D7060
	public override BaseReward Clone()
	{
		RewardLootItem rewardLootItem = new RewardLootItem();
		base.CopyValues(rewardLootItem);
		rewardLootItem.LootGameStage = this.LootGameStage;
		if (this.item != null)
		{
			rewardLootItem.item = this.item.Clone();
		}
		return rewardLootItem;
	}

	// Token: 0x06004BD8 RID: 19416 RVA: 0x001D8EA0 File Offset: 0x001D70A0
	public override string GetRewardText()
	{
		string localizedItemName = this.Item.itemValue.ItemClass.GetLocalizedItemName();
		if (this.Item.itemValue.HasQuality)
		{
			return localizedItemName;
		}
		if (this.Item.count <= 1)
		{
			return localizedItemName;
		}
		return string.Format("{0} ({1})", localizedItemName, this.Item.count);
	}

	// Token: 0x06004BD9 RID: 19417 RVA: 0x001D8F02 File Offset: 0x001D7102
	public override void ParseProperties(DynamicProperties properties)
	{
		base.ParseProperties(properties);
		if (properties.Values.ContainsKey(RewardLootItem.PropLootTier))
		{
			base.Value = properties.Values[RewardLootItem.PropLootTier];
		}
	}

	// Token: 0x04003BD4 RID: 15316
	[PublicizedFrom(EAccessModifier.Private)]
	public int LootGameStage = 1;

	// Token: 0x04003BD5 RID: 15317
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack item;

	// Token: 0x04003BD6 RID: 15318
	public static string PropLootTier = "loot_tier";
}
