using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001968 RID: 6504
	[Preserve]
	public class ActionBaseItemAction : ActionBaseClientAction
	{
		// Token: 0x0600C80C RID: 51212 RVA: 0x0049572C File Offset: 0x0049392C
		public override void OnClientPerform(Entity target)
		{
			EntityPlayer entityPlayer = target as EntityPlayer;
			if (entityPlayer != null)
			{
				this.OnClientActionStarted(entityPlayer);
				this.count = GameEventManager.GetIntValue(entityPlayer, this.countText, -1);
				bool flag = false;
				FastTags<TagGroup.Global>.Parse(this.itemTags);
				if (this.itemLocations.Contains(ActionBaseItemAction.ItemLocations.Toolbelt) && !this.isFinished)
				{
					ItemStack[] array = ((entityPlayer.AttachedToEntity != null && entityPlayer.saveInventory != null) ? entityPlayer.saveInventory.GetSlots() : entityPlayer.inventory.GetSlots());
					for (int i = 0; i < array.Length; i++)
					{
						if (this.HandleItemStackChange(ref array[i], entityPlayer))
						{
							flag = true;
						}
						if (this.isFinished)
						{
							break;
						}
					}
					if (flag)
					{
						entityPlayer.inventory.SetSlots(array, true);
						entityPlayer.bPlayerStatsChanged = true;
					}
				}
				flag = false;
				if (this.itemLocations.Contains(ActionBaseItemAction.ItemLocations.Equipment) || (this.itemLocations.Contains(ActionBaseItemAction.ItemLocations.BiomeBadge) && !this.isFinished))
				{
					int slotCount = entityPlayer.equipment.GetSlotCount();
					int num = 4;
					for (int j = 0; j < slotCount; j++)
					{
						if ((j < num || this.itemLocations.Contains(ActionBaseItemAction.ItemLocations.BiomeBadge)) && (j >= num || this.itemLocations.Contains(ActionBaseItemAction.ItemLocations.Equipment)))
						{
							if (this.CheckEquipmentReplace(entityPlayer.equipment, j))
							{
								ItemValue slotItemOrNone = entityPlayer.equipment.GetSlotItemOrNone(j);
								if (this.HandleItemValueChange(ref slotItemOrNone, entityPlayer))
								{
									entityPlayer.equipment.SetSlotItem(j, slotItemOrNone, true);
									flag = true;
								}
							}
							if (this.isFinished)
							{
								break;
							}
						}
					}
					if (flag)
					{
						entityPlayer.bPlayerStatsChanged = true;
					}
				}
				flag = false;
				if (this.itemLocations.Contains(ActionBaseItemAction.ItemLocations.Backpack) && !this.isFinished)
				{
					ItemStack[] slots = entityPlayer.bag.GetSlots();
					for (int k = 0; k < slots.Length; k++)
					{
						if (this.HandleItemStackChange(ref slots[k], entityPlayer))
						{
							flag = true;
						}
						if (this.isFinished)
						{
							break;
						}
					}
					if (flag)
					{
						entityPlayer.bag.SetSlots(slots);
						entityPlayer.bPlayerStatsChanged = true;
					}
				}
				if (this.itemLocations.Contains(ActionBaseItemAction.ItemLocations.Backpack) && !this.isFinished)
				{
					XUiC_DragAndDropWindow dragAndDropWindow = LocalPlayerUI.GetUIForPrimaryPlayer().xui.DragAndDropWindow;
					if (!dragAndDropWindow.CurrentStack.IsEmpty())
					{
						ItemStack currentStack = dragAndDropWindow.CurrentStack;
						if (this.HandleItemStackChange(ref currentStack, entityPlayer))
						{
							entityPlayer.bPlayerStatsChanged = true;
						}
					}
				}
				if (!this.itemLocations.Contains(ActionBaseItemAction.ItemLocations.Toolbelt) && this.itemLocations.Contains(ActionBaseItemAction.ItemLocations.Held) && !this.isFinished)
				{
					Inventory inventory = ((entityPlayer.saveInventory != null) ? entityPlayer.saveInventory : entityPlayer.inventory);
					if (inventory.holdingItem != entityPlayer.inventory.GetBareHandItem())
					{
						ItemStack holdingItemStack = inventory.holdingItemStack;
						if (this.HandleItemStackChange(ref holdingItemStack, entityPlayer))
						{
							inventory.SetItem(inventory.holdingItemIdx, holdingItemStack);
							entityPlayer.bPlayerStatsChanged = true;
						}
					}
				}
				this.OnClientActionEnded(entityPlayer);
			}
		}

		// Token: 0x0600C80D RID: 51213 RVA: 0x00020021 File Offset: 0x0001E221
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual bool CheckEquipmentReplace(Equipment equipment, int slot)
		{
			return true;
		}

		// Token: 0x0600C80E RID: 51214 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void OnClientActionStarted(EntityPlayer player)
		{
		}

		// Token: 0x0600C80F RID: 51215 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void OnClientActionEnded(EntityPlayer player)
		{
		}

		// Token: 0x0600C810 RID: 51216 RVA: 0x00010E62 File Offset: 0x0000F062
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual bool HandleItemStackChange(ref ItemStack stack, EntityPlayer player)
		{
			return false;
		}

		// Token: 0x0600C811 RID: 51217 RVA: 0x00010E62 File Offset: 0x0000F062
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual bool HandleItemValueChange(ref ItemValue itemValue, EntityPlayer player)
		{
			return false;
		}

		// Token: 0x0600C812 RID: 51218 RVA: 0x004959F2 File Offset: 0x00493BF2
		[PublicizedFrom(EAccessModifier.Protected)]
		public void AddStack(EntityPlayerLocal player, ItemStack stack)
		{
			if (!LocalPlayerUI.GetUIForPlayer(player).xui.PlayerInventory.AddItem(stack))
			{
				GameManager.Instance.ItemDropServer(stack, player.GetPosition(), Vector3.zero, -1, 60f, false);
			}
		}

		// Token: 0x0600C813 RID: 51219 RVA: 0x00495A2C File Offset: 0x00493C2C
		public override BaseAction Clone()
		{
			ActionBaseItemAction actionBaseItemAction = (ActionBaseItemAction)base.Clone();
			actionBaseItemAction.countText = this.countText;
			actionBaseItemAction.countType = this.countType;
			actionBaseItemAction.itemTags = this.itemTags;
			actionBaseItemAction.fastItemTags = this.fastItemTags;
			actionBaseItemAction.itemLocations = new List<ActionBaseItemAction.ItemLocations>(this.itemLocations);
			return actionBaseItemAction;
		}

		// Token: 0x0600C814 RID: 51220 RVA: 0x00495A88 File Offset: 0x00493C88
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			ActionBaseItemAction.ItemLocations itemLocations = ActionBaseItemAction.ItemLocations.Toolbelt;
			if (properties.Values.ContainsKey(ActionBaseItemAction.PropItemLocation))
			{
				string[] array = properties.Values[ActionBaseItemAction.PropItemLocation].Split(',', StringSplitOptions.None);
				this.itemLocations.Clear();
				for (int i = 0; i < array.Length; i++)
				{
					if (Enum.TryParse<ActionBaseItemAction.ItemLocations>(array[i], true, out itemLocations))
					{
						this.itemLocations.Add(itemLocations);
					}
				}
			}
			properties.ParseString(ActionBaseItemAction.PropItemTag, ref this.itemTags);
			this.fastItemTags = FastTags<TagGroup.Global>.Parse(this.itemTags);
			properties.ParseString(ActionBaseItemAction.PropFullCount, ref this.countText);
			properties.ParseEnum<ActionBaseItemAction.CountTypes>(ActionBaseItemAction.PropCountType, ref this.countType);
		}

		// Token: 0x040096CC RID: 38604
		[PublicizedFrom(EAccessModifier.Protected)]
		public List<ActionBaseItemAction.ItemLocations> itemLocations = new List<ActionBaseItemAction.ItemLocations>();

		// Token: 0x040096CD RID: 38605
		[PublicizedFrom(EAccessModifier.Protected)]
		public string itemTags = "";

		// Token: 0x040096CE RID: 38606
		[PublicizedFrom(EAccessModifier.Protected)]
		public FastTags<TagGroup.Global> fastItemTags = FastTags<TagGroup.Global>.none;

		// Token: 0x040096CF RID: 38607
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionBaseItemAction.CountTypes countType;

		// Token: 0x040096D0 RID: 38608
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool isFinished;

		// Token: 0x040096D1 RID: 38609
		[PublicizedFrom(EAccessModifier.Protected)]
		public int count = -1;

		// Token: 0x040096D2 RID: 38610
		[PublicizedFrom(EAccessModifier.Protected)]
		public string countText = "";

		// Token: 0x040096D3 RID: 38611
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropItemLocation = "items_location";

		// Token: 0x040096D4 RID: 38612
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropItemTag = "items_tags";

		// Token: 0x040096D5 RID: 38613
		public static string PropFullCount = "count";

		// Token: 0x040096D6 RID: 38614
		public static string PropCountType = "count_type";

		// Token: 0x02001969 RID: 6505
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum ItemLocations
		{
			// Token: 0x040096D8 RID: 38616
			Toolbelt,
			// Token: 0x040096D9 RID: 38617
			Backpack,
			// Token: 0x040096DA RID: 38618
			Equipment,
			// Token: 0x040096DB RID: 38619
			BiomeBadge,
			// Token: 0x040096DC RID: 38620
			Held
		}

		// Token: 0x0200196A RID: 6506
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum CountTypes
		{
			// Token: 0x040096DE RID: 38622
			Items,
			// Token: 0x040096DF RID: 38623
			Slots
		}
	}
}
