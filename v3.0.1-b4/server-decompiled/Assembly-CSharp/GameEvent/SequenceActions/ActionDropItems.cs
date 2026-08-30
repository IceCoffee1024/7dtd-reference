using System;
using System.Collections.Generic;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001955 RID: 6485
	[Preserve]
	public class ActionDropItems : ActionBaseItemAction
	{
		// Token: 0x0600C6DF RID: 50911 RVA: 0x00492051 File Offset: 0x00490251
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnClientActionStarted(EntityPlayer player)
		{
			this.droppedItems = new List<ItemStack>();
			this.replaceItemTag = ((this.itemTags == "") ? FastTags<TagGroup.Global>.none : FastTags<TagGroup.Global>.Parse(this.itemTags));
		}

		// Token: 0x0600C6E0 RID: 50912 RVA: 0x00492088 File Offset: 0x00490288
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnClientActionEnded(EntityPlayer player)
		{
			if (this.droppedItems.Count > 0)
			{
				Vector3 dropPosition = player.GetDropPosition();
				GameManager.Instance.DropContentInLootContainerServer(player.entityId, "DroppedLootContainerTwitch", dropPosition, this.droppedItems.ToArray(), false, null);
				if (this.DropSound != "")
				{
					Manager.BroadcastPlay(player, this.DropSound, false, 1f);
				}
			}
		}

		// Token: 0x0600C6E1 RID: 50913 RVA: 0x004920FC File Offset: 0x004902FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override bool HandleItemStackChange(ref ItemStack stack, EntityPlayer player)
		{
			if (!stack.IsEmpty() && (this.replaceItemTag.IsEmpty || stack.itemValue.ItemClass.HasAnyTags(this.replaceItemTag)) && stack.itemValue.ItemClass.GetItemName() != this.ReplacedByItem)
			{
				if (this.count != -1)
				{
					if (this.countType == ActionBaseItemAction.CountTypes.Slots)
					{
						this.droppedItems.Add(stack.Clone());
						if (this.ReplacedByItem == "")
						{
							stack = ItemStack.Empty;
						}
						else
						{
							stack = new ItemStack(ItemClass.GetItem(this.ReplacedByItem, false), stack.count);
						}
						this.count--;
						if (this.count == 0)
						{
							this.isFinished = true;
						}
						return true;
					}
					if (stack.count > this.count)
					{
						ItemStack itemStack = stack.Clone();
						itemStack.count = this.count;
						this.droppedItems.Add(itemStack);
						stack.count -= this.count;
						if (this.ReplacedByItem != "")
						{
							ItemStack itemStack2 = new ItemStack(ItemClass.GetItem(this.ReplacedByItem, false), this.count);
							base.AddStack(player as EntityPlayerLocal, itemStack2);
						}
						this.count = 0;
						this.isFinished = true;
					}
					else
					{
						this.count -= stack.count;
						this.droppedItems.Add(stack.Clone());
						if (this.ReplacedByItem == "")
						{
							stack = ItemStack.Empty;
						}
						else
						{
							stack = new ItemStack(ItemClass.GetItem(this.ReplacedByItem, false), stack.count);
						}
					}
				}
				else
				{
					this.droppedItems.Add(stack.Clone());
					if (this.ReplacedByItem == "")
					{
						stack = ItemStack.Empty;
					}
					else
					{
						stack = new ItemStack(ItemClass.GetItem(this.ReplacedByItem, false), stack.count);
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x0600C6E2 RID: 50914 RVA: 0x0049230E File Offset: 0x0049050E
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionDropItems.PropReplacedByItem, ref this.ReplacedByItem);
			properties.ParseString(ActionDropItems.PropDropSound, ref this.DropSound);
		}

		// Token: 0x0600C6E3 RID: 50915 RVA: 0x00492339 File Offset: 0x00490539
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionDropItems
			{
				ReplacedByItem = this.ReplacedByItem,
				DropSound = this.DropSound
			};
		}

		// Token: 0x0400962D RID: 38445
		public string ReplacedByItem = "";

		// Token: 0x0400962E RID: 38446
		public string DropSound = "";

		// Token: 0x0400962F RID: 38447
		public static string PropReplacedByItem = "replaced_by_item";

		// Token: 0x04009630 RID: 38448
		public static string PropDropSound = "drop_sound";

		// Token: 0x04009631 RID: 38449
		[PublicizedFrom(EAccessModifier.Private)]
		public List<ItemStack> droppedItems;

		// Token: 0x04009632 RID: 38450
		[PublicizedFrom(EAccessModifier.Private)]
		public FastTags<TagGroup.Global> replaceItemTag = FastTags<TagGroup.Global>.none;
	}
}
