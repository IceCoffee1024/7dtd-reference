using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019C3 RID: 6595
	[Preserve]
	public class ActionShuffleItems : ActionBaseClientAction
	{
		// Token: 0x0600C9A2 RID: 51618 RVA: 0x0049FA88 File Offset: 0x0049DC88
		public override void OnClientPerform(Entity target)
		{
			EntityPlayer entityPlayer = target as EntityPlayer;
			if (entityPlayer != null)
			{
				GameManager.Instance.StartCoroutine(this.handleShuffle(entityPlayer));
			}
		}

		// Token: 0x0600C9A3 RID: 51619 RVA: 0x0049FAB1 File Offset: 0x0049DCB1
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator handleShuffle(EntityPlayer player)
		{
			while (player.inventory.IsHoldingItemActionRunning() || !player.inventory.GetItem(player.inventory.DUMMY_SLOT_IDX).IsEmpty())
			{
				yield return new WaitForSeconds(0.25f);
			}
			List<ItemStack> list = new List<ItemStack>();
			if (this.itemLocations.Contains(ActionShuffleItems.ItemLocations.Toolbelt))
			{
				ItemStack[] array = ((player.AttachedToEntity != null && player.saveInventory != null) ? player.saveInventory.GetSlots() : player.inventory.GetSlots());
				list.AddRange(array);
				list.RemoveAt(list.Count - 1);
			}
			if (this.itemLocations.Contains(ActionShuffleItems.ItemLocations.Backpack))
			{
				ItemStack[] slots = player.bag.GetSlots();
				list.AddRange(slots);
			}
			GameRandom random = GameEventManager.Current.Random;
			for (int i = 0; i < list.Count * 2; i++)
			{
				int num = random.RandomRange(list.Count);
				int num2 = random.RandomRange(list.Count);
				ItemStack itemStack = list[num];
				list[num] = list[num2];
				list[num2] = itemStack;
			}
			int num3 = 0;
			if (this.itemLocations.Contains(ActionShuffleItems.ItemLocations.Toolbelt))
			{
				Inventory inventory = ((player.saveInventory != null) ? player.saveInventory : player.inventory);
				ItemStack[] slots2 = inventory.GetSlots();
				for (int j = 0; j < slots2.Length - 1; j++)
				{
					slots2[j] = list[num3++];
				}
				inventory.SetSlots(slots2, true);
				player.bPlayerStatsChanged = true;
			}
			if (this.itemLocations.Contains(ActionShuffleItems.ItemLocations.Backpack))
			{
				ItemStack[] slots3 = player.bag.GetSlots();
				for (int k = 0; k < slots3.Length; k++)
				{
					slots3[k] = list[num3++];
				}
				player.bag.SetSlots(slots3);
				player.bPlayerStatsChanged = true;
			}
			yield break;
		}

		// Token: 0x0600C9A4 RID: 51620 RVA: 0x0049FAC8 File Offset: 0x0049DCC8
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			ActionShuffleItems.ItemLocations itemLocations = ActionShuffleItems.ItemLocations.Toolbelt;
			if (properties.Values.ContainsKey(ActionShuffleItems.PropItemLocation))
			{
				string[] array = properties.Values[ActionShuffleItems.PropItemLocation].Split(',', StringSplitOptions.None);
				for (int i = 0; i < array.Length; i++)
				{
					if (Enum.TryParse<ActionShuffleItems.ItemLocations>(array[i], true, out itemLocations))
					{
						this.itemLocations.Add(itemLocations);
					}
				}
			}
		}

		// Token: 0x0600C9A5 RID: 51621 RVA: 0x0049FB30 File Offset: 0x0049DD30
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionShuffleItems
			{
				itemLocations = this.itemLocations,
				targetGroup = this.targetGroup
			};
		}

		// Token: 0x040098EC RID: 39148
		[PublicizedFrom(EAccessModifier.Protected)]
		public List<ActionShuffleItems.ItemLocations> itemLocations = new List<ActionShuffleItems.ItemLocations>();

		// Token: 0x040098ED RID: 39149
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropItemLocation = "items_location";

		// Token: 0x020019C4 RID: 6596
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum ItemLocations
		{
			// Token: 0x040098EF RID: 39151
			Toolbelt,
			// Token: 0x040098F0 RID: 39152
			Backpack
		}
	}
}
