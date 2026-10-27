using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019C5 RID: 6597
	[Preserve]
	public class ActionBaseItemAction : ActionBaseClientAction
	{
		// Token: 0x0600CA78 RID: 51832 RVA: 0x0049D71C File Offset: 0x0049B91C
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
					for (int i = 0; i < entityPlayer.inventory.Length; i++)
					{
						ItemStack itemStack = entityPlayer.inventory.GetItem(i).Clone();
						if (this.HandleItemStackChange(ref itemStack, entityPlayer))
						{
							entityPlayer.inventory.SetItem(i, itemStack);
							flag = true;
						}
						if (this.isFinished)
						{
							break;
						}
					}
					if (flag)
					{
						entityPlayer.bPlayerStatsChanged = true;
					}
				}
				flag = false;
				if (this.itemLocations.Contains(ActionBaseItemAction.ItemLocations.Equipment) || (this.itemLocations.Contains(ActionBaseItemAction.ItemLocations.BiomeBadge) && !this.isFinished))
				{
					int slotCount = entityPlayer.equipment.GetSlotCount();
					for (int j = 0; j < slotCount; j++)
					{
						if ((((EquipmentSlots)j).SlotType() != EquipmentSlotType.BiomeGear || this.itemLocations.Contains(ActionBaseItemAction.ItemLocations.BiomeBadge)) && (((EquipmentSlots)j).SlotType() != EquipmentSlotType.Armor || this.itemLocations.Contains(ActionBaseItemAction.ItemLocations.Equipment)))
						{
							if (this.CheckEquipmentReplace(entityPlayer.equipment, j))
							{
								ItemValue slotItem = entityPlayer.equipment.GetSlotItem(j);
								if (this.HandleItemValueChange(ref slotItem, entityPlayer))
								{
									entityPlayer.equipment.SetSlotItem(j, slotItem);
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
					for (int k = 0; k < entityPlayer.bag.SlotCount; k++)
					{
						ItemStack itemStack2 = entityPlayer.bag.ItemGrid[k].Clone();
						if (this.HandleItemStackChange(ref itemStack2, entityPlayer))
						{
							entityPlayer.bag.ItemGrid[k] = itemStack2;
							flag = true;
						}
						if (this.isFinished)
						{
							break;
						}
					}
					if (flag)
					{
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
					Inventory inventory = entityPlayer.inventory;
					if (inventory.holdingItem != inventory.Hand.BareHandItem)
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

		// Token: 0x0600CA79 RID: 51833 RVA: 0x00020119 File Offset: 0x0001E319
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual bool CheckEquipmentReplace(Equipment equipment, int slot)
		{
			return true;
		}

		// Token: 0x0600CA7A RID: 51834 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void OnClientActionStarted(EntityPlayer player)
		{
		}

		// Token: 0x0600CA7B RID: 51835 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void OnClientActionEnded(EntityPlayer player)
		{
		}

		// Token: 0x0600CA7C RID: 51836 RVA: 0x00010E62 File Offset: 0x0000F062
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual bool HandleItemStackChange(ref ItemStack stack, EntityPlayer player)
		{
			return false;
		}

		// Token: 0x0600CA7D RID: 51837 RVA: 0x00010E62 File Offset: 0x0000F062
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual bool HandleItemValueChange(ref ItemValue itemValue, EntityPlayer player)
		{
			return false;
		}

		// Token: 0x0600CA7E RID: 51838 RVA: 0x0049D9D0 File Offset: 0x0049BBD0
		[PublicizedFrom(EAccessModifier.Protected)]
		public void AddStack(EntityPlayerLocal player, ItemStack stack)
		{
			if (!LocalPlayerUI.GetUIForPlayer(player).xui.PlayerInventory.AddItem(stack))
			{
				GameManager.Instance.ItemDropServer(stack, player.GetPosition(), Vector3.zero, -1, 60f, false);
			}
		}

		// Token: 0x0600CA7F RID: 51839 RVA: 0x0049DA08 File Offset: 0x0049BC08
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

		// Token: 0x0600CA80 RID: 51840 RVA: 0x0049DA64 File Offset: 0x0049BC64
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

		// Token: 0x0400987B RID: 39035
		[PublicizedFrom(EAccessModifier.Protected)]
		public List<ActionBaseItemAction.ItemLocations> itemLocations = new List<ActionBaseItemAction.ItemLocations>();

		// Token: 0x0400987C RID: 39036
		[PublicizedFrom(EAccessModifier.Protected)]
		public string itemTags = "";

		// Token: 0x0400987D RID: 39037
		[PublicizedFrom(EAccessModifier.Protected)]
		public FastTags<TagGroup.Global> fastItemTags = FastTags<TagGroup.Global>.none;

		// Token: 0x0400987E RID: 39038
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionBaseItemAction.CountTypes countType;

		// Token: 0x0400987F RID: 39039
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool isFinished;

		// Token: 0x04009880 RID: 39040
		[PublicizedFrom(EAccessModifier.Protected)]
		public int count = -1;

		// Token: 0x04009881 RID: 39041
		[PublicizedFrom(EAccessModifier.Protected)]
		public string countText = "";

		// Token: 0x04009882 RID: 39042
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropItemLocation = "items_location";

		// Token: 0x04009883 RID: 39043
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropItemTag = "items_tags";

		// Token: 0x04009884 RID: 39044
		public static string PropFullCount = "count";

		// Token: 0x04009885 RID: 39045
		public static string PropCountType = "count_type";

		// Token: 0x020019C6 RID: 6598
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum ItemLocations
		{
			// Token: 0x04009887 RID: 39047
			Toolbelt,
			// Token: 0x04009888 RID: 39048
			Backpack,
			// Token: 0x04009889 RID: 39049
			Equipment,
			// Token: 0x0400988A RID: 39050
			BiomeBadge,
			// Token: 0x0400988B RID: 39051
			Held
		}

		// Token: 0x020019C7 RID: 6599
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum CountTypes
		{
			// Token: 0x0400988D RID: 39053
			Items,
			// Token: 0x0400988E RID: 39054
			Slots
		}
	}
}
