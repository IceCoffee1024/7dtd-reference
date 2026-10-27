using System;
using System.Collections;
using UnityEngine.Scripting;

// Token: 0x02000D85 RID: 3461
[Preserve]
public class ItemActionEntryUse : BaseItemActionEntry
{
	// Token: 0x06006867 RID: 26727 RVA: 0x002974AC File Offset: 0x002956AC
	public ItemActionEntryUse(XUiController _controller, ItemActionEntryUse.ConsumeType _consumeType)
		: base(_controller, "OVERRIDDEN BELOW", "OVERRIDDEN BELOW", BaseItemActionEntry.GamepadShortCut.DPadLeft, "crafting/craft_click_craft", "ui/ui_denied")
	{
		this.consumeType = _consumeType;
		switch (_consumeType)
		{
		case ItemActionEntryUse.ConsumeType.Eat:
			base.ActionName = Localization.Get("lblContextActionEat", false, null);
			base.IconName = "ui_game_symbol_fork";
			base.SoundName = "";
			break;
		case ItemActionEntryUse.ConsumeType.Drink:
			base.ActionName = Localization.Get("lblContextActionDrink", false, null);
			base.IconName = "ui_game_symbol_water";
			base.SoundName = "";
			break;
		case ItemActionEntryUse.ConsumeType.Heal:
			base.ActionName = Localization.Get("lblContextActionHeal", false, null);
			base.IconName = "ui_game_symbol_medical";
			break;
		case ItemActionEntryUse.ConsumeType.Read:
			base.ActionName = Localization.Get("lblContextActionRead", false, null);
			base.IconName = "ui_game_symbol_book";
			base.SoundName = "";
			break;
		case ItemActionEntryUse.ConsumeType.Quest:
			base.ActionName = Localization.Get("lblContextActionRead", false, null);
			base.IconName = "ui_game_symbol_quest";
			break;
		case ItemActionEntryUse.ConsumeType.Open:
			base.ActionName = Localization.Get("lblContextActionOpen", false, null);
			base.IconName = "ui_game_symbol_treasure";
			break;
		}
		this.RefreshEnabled();
	}

	// Token: 0x06006868 RID: 26728 RVA: 0x002975E8 File Offset: 0x002957E8
	public override void OnDisabledActivate()
	{
		EntityPlayerLocal entityPlayer = base.ItemController.xui.playerUI.entityPlayer;
		if (entityPlayer.AttachedToEntity)
		{
			GameManager.ShowTooltip(entityPlayer, Localization.Get("ttCannotUseWhileOnVehicle", false, null), false, false, 0f);
			return;
		}
		if (base.ItemController.xui.PlayerInventory.Toolbelt.IsHoldingItemActionRunning() || base.ItemController.xui.IsUsingItemActionEntryUse)
		{
			GameManager.ShowTooltip(entityPlayer, Localization.Get("isBusy", false, null), false, false, 0f);
			return;
		}
		if (XUiC_AssembleWindowGroup.GetWindowGroup(base.ItemController.xui).IsOpen)
		{
			GameManager.ShowTooltip(entityPlayer, Localization.Get("ttCannotUseWhileAssembling", false, null), false, false, 0f);
			return;
		}
		switch (this.consumeType)
		{
		case ItemActionEntryUse.ConsumeType.Eat:
			if (XUiM_Player.GetFoodPercent(entityPlayer) >= 1f)
			{
				GameManager.ShowTooltip(entityPlayer, Localization.Get("notHungry", false, null), false, false, 0f);
				return;
			}
			GameManager.ShowTooltip(entityPlayer, Localization.Get("isBusy", false, null), false, false, 0f);
			return;
		case ItemActionEntryUse.ConsumeType.Drink:
			if (XUiM_Player.GetWaterPercent(entityPlayer) >= 1f)
			{
				GameManager.ShowTooltip(entityPlayer, Localization.Get("notThirsty", false, null), false, false, 0f);
				return;
			}
			GameManager.ShowTooltip(entityPlayer, Localization.Get("isBusy", false, null), false, false, 0f);
			return;
		case ItemActionEntryUse.ConsumeType.Heal:
			GameManager.ShowTooltip(entityPlayer, Localization.Get("notHurt", false, null), false, false, 0f);
			return;
		case ItemActionEntryUse.ConsumeType.Read:
			switch (this.state)
			{
			case ItemActionEntryUse.StateTypes.RecipeKnown:
				GameManager.ShowTooltip(entityPlayer, Localization.Get("alreadyKnown", false, null), false, false, 0f);
				return;
			case ItemActionEntryUse.StateTypes.SkillRequirementsNotMet:
				GameManager.ShowTooltip(entityPlayer, Localization.Get("ttSkillRequirementsNotMet", false, null), false, false, 0f);
				return;
			case ItemActionEntryUse.StateTypes.SkillKnown:
				GameManager.ShowTooltip(entityPlayer, Localization.Get("ttSkillMaxLevel", false, null), false, false, 0f);
				return;
			default:
				return;
			}
			break;
		case ItemActionEntryUse.ConsumeType.Quest:
			GameManager.ShowTooltip(entityPlayer, Localization.Get("questunavailable", false, null), false, false, 0f);
			return;
		default:
			return;
		}
	}

	// Token: 0x06006869 RID: 26729 RVA: 0x002977F0 File Offset: 0x002959F0
	public override void RefreshEnabled()
	{
		this.state = ItemActionEntryUse.StateTypes.Normal;
		EntityPlayer entityPlayer = base.ItemController.xui.playerUI.entityPlayer;
		if (entityPlayer.AttachedToEntity)
		{
			base.Enabled = false;
			return;
		}
		if (entityPlayer.inventory.IsHoldingItemActionRunning())
		{
			base.Enabled = false;
			return;
		}
		if (XUiC_AssembleWindowGroup.GetWindowGroup(base.ItemController.xui).IsOpen)
		{
			base.Enabled = false;
			return;
		}
		ItemStack itemStack = ((XUiC_ItemStack)base.ItemController).ItemStack;
		switch (this.consumeType)
		{
		case ItemActionEntryUse.ConsumeType.Eat:
			base.Enabled = !base.ItemController.xui.IsUsingItemActionEntryUse && XUiM_Player.GetFoodPercent(entityPlayer) < 1f;
			break;
		case ItemActionEntryUse.ConsumeType.Drink:
			base.Enabled = !base.ItemController.xui.IsUsingItemActionEntryUse && XUiM_Player.GetWaterPercent(entityPlayer) < 1f;
			break;
		case ItemActionEntryUse.ConsumeType.Heal:
			base.Enabled = true;
			break;
		case ItemActionEntryUse.ConsumeType.Read:
			if (itemStack != null && !itemStack.IsEmpty())
			{
				ItemClass forId = ItemClass.GetForId(itemStack.itemValue.type);
				bool flag = false;
				for (int i = 0; i < forId.Actions.Length; i++)
				{
					ItemActionLearnRecipe itemActionLearnRecipe = forId.Actions[i] as ItemActionLearnRecipe;
					if (itemActionLearnRecipe != null)
					{
						for (int j = 0; j < itemActionLearnRecipe.RecipesToLearn.Length; j++)
						{
							this.state = ItemActionEntryUse.StateTypes.RecipeKnown;
							if (!XUiM_Recipes.GetRecipeIsUnlocked(base.ItemController.xui, itemActionLearnRecipe.RecipesToLearn[j]))
							{
								this.state = ItemActionEntryUse.StateTypes.Normal;
								flag = true;
								break;
							}
						}
						if (flag)
						{
							break;
						}
					}
				}
				base.Enabled = flag;
			}
			else
			{
				base.Enabled = false;
			}
			break;
		case ItemActionEntryUse.ConsumeType.Quest:
			if (itemStack != null && !itemStack.IsEmpty())
			{
				ItemClass forId2 = ItemClass.GetForId(itemStack.itemValue.type);
				int k = 0;
				while (k < forId2.Actions.Length)
				{
					ItemActionQuest itemActionQuest = forId2.Actions[k] as ItemActionQuest;
					if (itemActionQuest != null)
					{
						if (!QuestClass.GetQuest(itemActionQuest.QuestGiven).CanActivate())
						{
							base.Enabled = false;
							break;
						}
						Quest quest = base.ItemController.xui.playerUI.entityPlayer.QuestJournal.FindQuest(itemActionQuest.QuestGiven, -1);
						bool flag2 = base.ItemController.xui.playerUI.entityPlayer.QuestJournal.FindActiveQuest(itemActionQuest.QuestGiven, -1) != null;
						base.Enabled = quest == null || (QuestClass.GetQuest(itemActionQuest.QuestGiven).Repeatable && !flag2);
						break;
					}
					else
					{
						k++;
					}
				}
			}
			break;
		case ItemActionEntryUse.ConsumeType.Open:
			base.Enabled = true;
			break;
		}
		Inventory toolbelt = base.ItemController.xui.PlayerInventory.Toolbelt;
		base.Enabled = base.Enabled && !toolbelt.IsHandBusy();
	}

	// Token: 0x0600686A RID: 26730 RVA: 0x00297AF0 File Offset: 0x00295CF0
	public override void OnActivated()
	{
		ItemActionEntryUse.<>c__DisplayClass6_0 CS$<>8__locals1 = new ItemActionEntryUse.<>c__DisplayClass6_0();
		CS$<>8__locals1.<>4__this = this;
		if (base.ItemController.xui.IsUsingItemActionEntryUse)
		{
			return;
		}
		CS$<>8__locals1.stackControl = (XUiC_ItemStack)base.ItemController;
		ItemClass itemClass = CS$<>8__locals1.stackControl.ItemStack.itemValue.ItemClass;
		if (!itemClass.CanExecuteAction(0, base.ItemController.xui.playerUI.entityPlayer, CS$<>8__locals1.stackControl.ItemStack.itemValue))
		{
			GameManager.ShowTooltip(base.ItemController.xui.playerUI.entityPlayer, Localization.Get("ttCannotUseAtThisTime", false, null), string.Empty, "ui_denied", null, false, false, 0f);
			return;
		}
		base.ItemController.xui.IsUsingItemActionEntryUse = true;
		ItemStack itemStack = new ItemStack(CS$<>8__locals1.stackControl.ItemStack.itemValue.Clone(), 1);
		ItemStack itemStack2 = new ItemStack(CS$<>8__locals1.stackControl.ItemStack.itemValue.Clone(), CS$<>8__locals1.stackControl.ItemStack.count);
		CS$<>8__locals1.inventory = base.ItemController.xui.PlayerInventory.Toolbelt;
		if (this.consumeType == ItemActionEntryUse.ConsumeType.Quest)
		{
			base.ItemController.xui.FindWindowGroupByName("questOffer").GetChildByType<XUiC_QuestOfferWindow>().ItemStackController = CS$<>8__locals1.stackControl;
			CS$<>8__locals1.stackControl.QuestLock = true;
		}
		else
		{
			CS$<>8__locals1.stackControl.HiddenLock = true;
		}
		CS$<>8__locals1.stackControl.WindowGroup.Controller.SetAllChildrenDirty();
		this.RefreshEnabled();
		CS$<>8__locals1.actionIdx = 0;
		for (int i = 0; i < itemClass.Actions.Length; i++)
		{
			bool flag = false;
			switch (this.consumeType)
			{
			case ItemActionEntryUse.ConsumeType.Eat:
			case ItemActionEntryUse.ConsumeType.Drink:
			case ItemActionEntryUse.ConsumeType.Heal:
				if (itemClass.Actions[i] != null)
				{
					flag = true;
				}
				break;
			case ItemActionEntryUse.ConsumeType.Read:
				if (itemClass.Actions[i] is ItemActionLearnRecipe)
				{
					flag = true;
				}
				break;
			case ItemActionEntryUse.ConsumeType.Quest:
				if (itemClass.Actions[i] is ItemActionQuest)
				{
					flag = true;
				}
				break;
			case ItemActionEntryUse.ConsumeType.Open:
			{
				ItemAction itemAction = itemClass.Actions[i];
				if (itemAction is ItemActionOpenBundle || itemAction is ItemActionOpenLootBundle)
				{
					flag = true;
				}
				break;
			}
			}
			if (flag)
			{
				CS$<>8__locals1.actionIdx = i;
				break;
			}
		}
		if (!(itemStack.itemValue.ItemClass.Actions[CS$<>8__locals1.actionIdx] is ItemActionEat))
		{
			ItemStack itemStack3 = itemStack2;
			int count = itemStack3.count;
			itemStack3.count = count - 1;
			if (itemStack2.count == 0)
			{
				itemStack2 = ItemStack.Empty;
			}
		}
		if (this.consumeType != ItemActionEntryUse.ConsumeType.Quest)
		{
			CS$<>8__locals1.stackControl.ItemStack = itemStack2;
		}
		if (!itemStack.itemValue.ItemClass.Actions[CS$<>8__locals1.actionIdx].UseAnimation && itemStack.itemValue.ItemClass.Actions[CS$<>8__locals1.actionIdx].ExecuteInstantAction(base.ItemController.xui.playerUI.entityPlayer, itemStack, false, CS$<>8__locals1.stackControl))
		{
			if (this.consumeType != ItemActionEntryUse.ConsumeType.Quest)
			{
				CS$<>8__locals1.stackControl.HiddenLock = false;
				CS$<>8__locals1.stackControl.WindowGroup.Controller.SetAllChildrenDirty();
			}
			base.ItemController.xui.IsUsingItemActionEntryUse = false;
			return;
		}
		ItemActionEat itemActionEat = itemStack.itemValue.ItemClass.Actions[CS$<>8__locals1.actionIdx] as ItemActionEat;
		if (itemActionEat != null && itemActionEat.UsePrompt)
		{
			XUiC_MessageBoxWindowGroup.ShowOkCancel(base.ItemController.xui, Localization.Get(itemActionEat.PromptTitle, false, null), Localization.Get(itemActionEat.PromptDescription, false, null), "", new Action(CS$<>8__locals1.<OnActivated>g__UseItemWithAnimation|1), new Action(CS$<>8__locals1.<OnActivated>g__SwitchBack|2), true, true, false);
			return;
		}
		CS$<>8__locals1.<OnActivated>g__UseItemWithAnimation|1();
	}

	// Token: 0x0600686B RID: 26731 RVA: 0x00297EA1 File Offset: 0x002960A1
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator SwitchBackCoroutine(Inventory _inventory)
	{
		while (_inventory.IsHandBusy())
		{
			yield return null;
		}
		((XUiC_ItemStack)base.ItemController).HiddenLock = false;
		base.ParentActionList.RefreshActionList();
		base.ItemController.xui.IsUsingItemActionEntryUse = false;
		this.RefreshEnabled();
		yield break;
	}

	// Token: 0x0600686C RID: 26732 RVA: 0x000027FC File Offset: 0x000009FC
	public override void OnTimerCompleted()
	{
	}

	// Token: 0x04005072 RID: 20594
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly ItemActionEntryUse.ConsumeType consumeType;

	// Token: 0x04005073 RID: 20595
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemActionEntryUse.StateTypes state;

	// Token: 0x02000D86 RID: 3462
	[PublicizedFrom(EAccessModifier.Private)]
	public enum StateTypes
	{
		// Token: 0x04005075 RID: 20597
		Normal,
		// Token: 0x04005076 RID: 20598
		RecipeKnown,
		// Token: 0x04005077 RID: 20599
		SkillRequirementsNotMet,
		// Token: 0x04005078 RID: 20600
		SkillKnown
	}

	// Token: 0x02000D87 RID: 3463
	public enum ConsumeType
	{
		// Token: 0x0400507A RID: 20602
		None,
		// Token: 0x0400507B RID: 20603
		Eat,
		// Token: 0x0400507C RID: 20604
		Drink,
		// Token: 0x0400507D RID: 20605
		Heal,
		// Token: 0x0400507E RID: 20606
		Read,
		// Token: 0x0400507F RID: 20607
		Quest,
		// Token: 0x04005080 RID: 20608
		Open
	}
}
