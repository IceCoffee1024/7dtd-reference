using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A20 RID: 6688
	[Preserve]
	public class ActionShuffleItems : ActionBaseClientAction
	{
		// Token: 0x0600CC0E RID: 52238 RVA: 0x004A7A74 File Offset: 0x004A5C74
		public override void OnClientPerform(Entity target)
		{
			EntityPlayer entityPlayer = target as EntityPlayer;
			if (entityPlayer != null)
			{
				GameManager.Instance.StartCoroutine(this.handleShuffle(entityPlayer));
			}
		}

		// Token: 0x0600CC0F RID: 52239 RVA: 0x004A7A9D File Offset: 0x004A5C9D
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator handleShuffle(EntityPlayer player)
		{
			while (player.inventory.IsHandBusy())
			{
				yield return new WaitForSeconds(0.25f);
			}
			List<ItemStack> list = new List<ItemStack>();
			if (this.itemLocations.Contains(ActionShuffleItems.ItemLocations.Toolbelt))
			{
				for (int i = 0; i < player.inventory.Length; i++)
				{
					list.Add(player.inventory.GetItem(i).Clone());
				}
			}
			if (this.itemLocations.Contains(ActionShuffleItems.ItemLocations.Backpack))
			{
				for (int j = 0; j < player.bag.SlotCount; j++)
				{
					list.Add(player.bag.ItemGrid[j].Clone());
				}
			}
			GameRandom random = GameEventManager.Current.Random;
			for (int k = 0; k < list.Count * 2; k++)
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
				Inventory inventory = player.inventory;
				for (int l = 0; l < inventory.Length; l++)
				{
					inventory.SetItem(l, list[num3++]);
				}
				player.bPlayerStatsChanged = true;
			}
			if (this.itemLocations.Contains(ActionShuffleItems.ItemLocations.Backpack))
			{
				for (int m = 0; m < player.bag.SlotCount; m++)
				{
					player.bag.ItemGrid[m] = list[num3++];
				}
				player.bPlayerStatsChanged = true;
			}
			yield break;
		}

		// Token: 0x0600CC10 RID: 52240 RVA: 0x004A7AB4 File Offset: 0x004A5CB4
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

		// Token: 0x0600CC11 RID: 52241 RVA: 0x004A7B1C File Offset: 0x004A5D1C
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionShuffleItems
			{
				itemLocations = this.itemLocations,
				targetGroup = this.targetGroup
			};
		}

		// Token: 0x04009A9B RID: 39579
		[PublicizedFrom(EAccessModifier.Protected)]
		public List<ActionShuffleItems.ItemLocations> itemLocations = new List<ActionShuffleItems.ItemLocations>();

		// Token: 0x04009A9C RID: 39580
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropItemLocation = "items_location";

		// Token: 0x02001A21 RID: 6689
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum ItemLocations
		{
			// Token: 0x04009A9E RID: 39582
			Toolbelt,
			// Token: 0x04009A9F RID: 39583
			Backpack
		}
	}
}
