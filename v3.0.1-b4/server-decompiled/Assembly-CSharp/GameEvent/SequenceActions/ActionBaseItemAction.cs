using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001942 RID: 6466
	[Preserve]
	public class ActionBaseItemAction : ActionBaseClientAction
	{
		// Token: 0x0600C685 RID: 50821 RVA: 0x0048F3E0 File Offset: 0x0048D5E0
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

		// Token: 0x0600C686 RID: 50822 RVA: 0x0001FF5D File Offset: 0x0001E15D
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual bool CheckEquipmentReplace(Equipment equipment, int slot)
		{
			return true;
		}

		// Token: 0x0600C687 RID: 50823 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void OnClientActionStarted(EntityPlayer player)
		{
		}

		// Token: 0x0600C688 RID: 50824 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void OnClientActionEnded(EntityPlayer player)
		{
		}

		// Token: 0x0600C689 RID: 50825 RVA: 0x00010E62 File Offset: 0x0000F062
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual bool HandleItemStackChange(ref ItemStack stack, EntityPlayer player)
		{
			return false;
		}

		// Token: 0x0600C68A RID: 50826 RVA: 0x00010E62 File Offset: 0x0000F062
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual bool HandleItemValueChange(ref ItemValue itemValue, EntityPlayer player)
		{
			return false;
		}

		// Token: 0x0600C68B RID: 50827 RVA: 0x0048F6A6 File Offset: 0x0048D8A6
		[PublicizedFrom(EAccessModifier.Protected)]
		public void AddStack(EntityPlayerLocal player, ItemStack stack)
		{
			if (!LocalPlayerUI.GetUIForPlayer(player).xui.PlayerInventory.AddItem(stack))
			{
				GameManager.Instance.ItemDropServer(stack, player.GetPosition(), Vector3.zero, -1, 60f, false);
			}
		}

		// Token: 0x0600C68C RID: 50828 RVA: 0x0048F6E0 File Offset: 0x0048D8E0
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

		// Token: 0x0600C68D RID: 50829 RVA: 0x0048F73C File Offset: 0x0048D93C
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

		// Token: 0x040095A3 RID: 38307
		[PublicizedFrom(EAccessModifier.Protected)]
		public List<ActionBaseItemAction.ItemLocations> itemLocations = new List<ActionBaseItemAction.ItemLocations>();

		// Token: 0x040095A4 RID: 38308
		[PublicizedFrom(EAccessModifier.Protected)]
		public string itemTags = "";

		// Token: 0x040095A5 RID: 38309
		[PublicizedFrom(EAccessModifier.Protected)]
		public FastTags<TagGroup.Global> fastItemTags = FastTags<TagGroup.Global>.none;

		// Token: 0x040095A6 RID: 38310
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionBaseItemAction.CountTypes countType;

		// Token: 0x040095A7 RID: 38311
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool isFinished;

		// Token: 0x040095A8 RID: 38312
		[PublicizedFrom(EAccessModifier.Protected)]
		public int count = -1;

		// Token: 0x040095A9 RID: 38313
		[PublicizedFrom(EAccessModifier.Protected)]
		public string countText = "";

		// Token: 0x040095AA RID: 38314
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropItemLocation = "items_location";

		// Token: 0x040095AB RID: 38315
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropItemTag = "items_tags";

		// Token: 0x040095AC RID: 38316
		public static string PropFullCount = "count";

		// Token: 0x040095AD RID: 38317
		public static string PropCountType = "count_type";

		// Token: 0x02001943 RID: 6467
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum ItemLocations
		{
			// Token: 0x040095AF RID: 38319
			Toolbelt,
			// Token: 0x040095B0 RID: 38320
			Backpack,
			// Token: 0x040095B1 RID: 38321
			Equipment,
			// Token: 0x040095B2 RID: 38322
			BiomeBadge,
			// Token: 0x040095B3 RID: 38323
			Held
		}

		// Token: 0x02001944 RID: 6468
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum CountTypes
		{
			// Token: 0x040095B5 RID: 38325
			Items,
			// Token: 0x040095B6 RID: 38326
			Slots
		}
	}
}
