using System;
using System.Collections.Generic;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019A7 RID: 6567
	[Preserve]
	public class ActionDropItems : ActionBaseItemAction
	{
		// Token: 0x0600C9BE RID: 51646 RVA: 0x0049C85D File Offset: 0x0049AA5D
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnClientActionStarted(EntityPlayer player)
		{
			this.droppedItems = new List<ItemStack>();
			this.replaceItemTag = ((this.itemTags == "") ? FastTags<TagGroup.Global>.none : FastTags<TagGroup.Global>.Parse(this.itemTags));
		}

		// Token: 0x0600C9BF RID: 51647 RVA: 0x0049C894 File Offset: 0x0049AA94
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

		// Token: 0x0600C9C0 RID: 51648 RVA: 0x0049C908 File Offset: 0x0049AB08
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

		// Token: 0x0600C9C1 RID: 51649 RVA: 0x0049CB1A File Offset: 0x0049AD1A
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionDropItems.PropReplacedByItem, ref this.ReplacedByItem);
			properties.ParseString(ActionDropItems.PropDropSound, ref this.DropSound);
		}

		// Token: 0x0600C9C2 RID: 51650 RVA: 0x0049CB45 File Offset: 0x0049AD45
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionDropItems
			{
				ReplacedByItem = this.ReplacedByItem,
				DropSound = this.DropSound
			};
		}

		// Token: 0x0400981C RID: 38940
		public string ReplacedByItem = "";

		// Token: 0x0400981D RID: 38941
		public string DropSound = "";

		// Token: 0x0400981E RID: 38942
		public static string PropReplacedByItem = "replaced_by_item";

		// Token: 0x0400981F RID: 38943
		public static string PropDropSound = "drop_sound";

		// Token: 0x04009820 RID: 38944
		[PublicizedFrom(EAccessModifier.Private)]
		public List<ItemStack> droppedItems;

		// Token: 0x04009821 RID: 38945
		[PublicizedFrom(EAccessModifier.Private)]
		public FastTags<TagGroup.Global> replaceItemTag = FastTags<TagGroup.Global>.none;
	}
}
