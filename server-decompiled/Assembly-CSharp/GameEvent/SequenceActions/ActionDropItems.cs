using System;
using System.Collections.Generic;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x0200197B RID: 6523
	[Preserve]
	public class ActionDropItems : ActionBaseItemAction
	{
		// Token: 0x0600C866 RID: 51302 RVA: 0x00498399 File Offset: 0x00496599
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnClientActionStarted(EntityPlayer player)
		{
			this.droppedItems = new List<ItemStack>();
			this.replaceItemTag = ((this.itemTags == "") ? FastTags<TagGroup.Global>.none : FastTags<TagGroup.Global>.Parse(this.itemTags));
		}

		// Token: 0x0600C867 RID: 51303 RVA: 0x004983D0 File Offset: 0x004965D0
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

		// Token: 0x0600C868 RID: 51304 RVA: 0x00498444 File Offset: 0x00496644
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

		// Token: 0x0600C869 RID: 51305 RVA: 0x00498656 File Offset: 0x00496856
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionDropItems.PropReplacedByItem, ref this.ReplacedByItem);
			properties.ParseString(ActionDropItems.PropDropSound, ref this.DropSound);
		}

		// Token: 0x0600C86A RID: 51306 RVA: 0x00498681 File Offset: 0x00496881
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionDropItems
			{
				ReplacedByItem = this.ReplacedByItem,
				DropSound = this.DropSound
			};
		}

		// Token: 0x04009756 RID: 38742
		public string ReplacedByItem = "";

		// Token: 0x04009757 RID: 38743
		public string DropSound = "";

		// Token: 0x04009758 RID: 38744
		public static string PropReplacedByItem = "replaced_by_item";

		// Token: 0x04009759 RID: 38745
		public static string PropDropSound = "drop_sound";

		// Token: 0x0400975A RID: 38746
		[PublicizedFrom(EAccessModifier.Private)]
		public List<ItemStack> droppedItems;

		// Token: 0x0400975B RID: 38747
		[PublicizedFrom(EAccessModifier.Private)]
		public FastTags<TagGroup.Global> replaceItemTag = FastTags<TagGroup.Global>.none;
	}
}
