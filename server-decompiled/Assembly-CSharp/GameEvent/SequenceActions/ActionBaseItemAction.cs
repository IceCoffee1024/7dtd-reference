using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001994 RID: 6548
	[Preserve]
	public class ActionBaseItemAction : ActionBaseClientAction
	{
		// Token: 0x0600C964 RID: 51556 RVA: 0x00499C1C File Offset: 0x00497E1C
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

		// Token: 0x0600C965 RID: 51557 RVA: 0x00020119 File Offset: 0x0001E319
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual bool CheckEquipmentReplace(Equipment equipment, int slot)
		{
			return true;
		}

		// Token: 0x0600C966 RID: 51558 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void OnClientActionStarted(EntityPlayer player)
		{
		}

		// Token: 0x0600C967 RID: 51559 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void OnClientActionEnded(EntityPlayer player)
		{
		}

		// Token: 0x0600C968 RID: 51560 RVA: 0x00010E62 File Offset: 0x0000F062
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual bool HandleItemStackChange(ref ItemStack stack, EntityPlayer player)
		{
			return false;
		}

		// Token: 0x0600C969 RID: 51561 RVA: 0x00010E62 File Offset: 0x0000F062
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual bool HandleItemValueChange(ref ItemValue itemValue, EntityPlayer player)
		{
			return false;
		}

		// Token: 0x0600C96A RID: 51562 RVA: 0x00499ED0 File Offset: 0x004980D0
		[PublicizedFrom(EAccessModifier.Protected)]
		public void AddStack(EntityPlayerLocal player, ItemStack stack)
		{
			if (!LocalPlayerUI.GetUIForPlayer(player).xui.PlayerInventory.AddItem(stack))
			{
				GameManager.Instance.ItemDropServer(stack, player.GetPosition(), Vector3.zero, -1, 60f, false);
			}
		}

		// Token: 0x0600C96B RID: 51563 RVA: 0x00499F08 File Offset: 0x00498108
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

		// Token: 0x0600C96C RID: 51564 RVA: 0x00499F64 File Offset: 0x00498164
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

		// Token: 0x04009792 RID: 38802
		[PublicizedFrom(EAccessModifier.Protected)]
		public List<ActionBaseItemAction.ItemLocations> itemLocations = new List<ActionBaseItemAction.ItemLocations>();

		// Token: 0x04009793 RID: 38803
		[PublicizedFrom(EAccessModifier.Protected)]
		public string itemTags = "";

		// Token: 0x04009794 RID: 38804
		[PublicizedFrom(EAccessModifier.Protected)]
		public FastTags<TagGroup.Global> fastItemTags = FastTags<TagGroup.Global>.none;

		// Token: 0x04009795 RID: 38805
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionBaseItemAction.CountTypes countType;

		// Token: 0x04009796 RID: 38806
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool isFinished;

		// Token: 0x04009797 RID: 38807
		[PublicizedFrom(EAccessModifier.Protected)]
		public int count = -1;

		// Token: 0x04009798 RID: 38808
		[PublicizedFrom(EAccessModifier.Protected)]
		public string countText = "";

		// Token: 0x04009799 RID: 38809
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropItemLocation = "items_location";

		// Token: 0x0400979A RID: 38810
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropItemTag = "items_tags";

		// Token: 0x0400979B RID: 38811
		public static string PropFullCount = "count";

		// Token: 0x0400979C RID: 38812
		public static string PropCountType = "count_type";

		// Token: 0x02001995 RID: 6549
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum ItemLocations
		{
			// Token: 0x0400979E RID: 38814
			Toolbelt,
			// Token: 0x0400979F RID: 38815
			Backpack,
			// Token: 0x040097A0 RID: 38816
			Equipment,
			// Token: 0x040097A1 RID: 38817
			BiomeBadge,
			// Token: 0x040097A2 RID: 38818
			Held
		}

		// Token: 0x02001996 RID: 6550
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum CountTypes
		{
			// Token: 0x040097A4 RID: 38820
			Items,
			// Token: 0x040097A5 RID: 38821
			Slots
		}
	}
}
