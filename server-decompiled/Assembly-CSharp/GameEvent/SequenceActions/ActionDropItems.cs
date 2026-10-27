using System;
using System.Collections.Generic;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019D8 RID: 6616
	[Preserve]
	public class ActionDropItems : ActionBaseItemAction
	{
		// Token: 0x0600CAD2 RID: 51922 RVA: 0x004A035D File Offset: 0x0049E55D
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnClientActionStarted(EntityPlayer player)
		{
			this.droppedItems = new List<ItemStack>();
			this.replaceItemTag = ((this.itemTags == "") ? FastTags<TagGroup.Global>.none : FastTags<TagGroup.Global>.Parse(this.itemTags));
		}

		// Token: 0x0600CAD3 RID: 51923 RVA: 0x004A0394 File Offset: 0x0049E594
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

		// Token: 0x0600CAD4 RID: 51924 RVA: 0x004A0408 File Offset: 0x0049E608
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

		// Token: 0x0600CAD5 RID: 51925 RVA: 0x004A061A File Offset: 0x0049E81A
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionDropItems.PropReplacedByItem, ref this.ReplacedByItem);
			properties.ParseString(ActionDropItems.PropDropSound, ref this.DropSound);
		}

		// Token: 0x0600CAD6 RID: 51926 RVA: 0x004A0645 File Offset: 0x0049E845
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionDropItems
			{
				ReplacedByItem = this.ReplacedByItem,
				DropSound = this.DropSound
			};
		}

		// Token: 0x04009905 RID: 39173
		public string ReplacedByItem = "";

		// Token: 0x04009906 RID: 39174
		public string DropSound = "";

		// Token: 0x04009907 RID: 39175
		public static string PropReplacedByItem = "replaced_by_item";

		// Token: 0x04009908 RID: 39176
		public static string PropDropSound = "drop_sound";

		// Token: 0x04009909 RID: 39177
		[PublicizedFrom(EAccessModifier.Private)]
		public List<ItemStack> droppedItems;

		// Token: 0x0400990A RID: 39178
		[PublicizedFrom(EAccessModifier.Private)]
		public FastTags<TagGroup.Global> replaceItemTag = FastTags<TagGroup.Global>.none;
	}
}
