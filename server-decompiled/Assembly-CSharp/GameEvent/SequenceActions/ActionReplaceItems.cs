using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A08 RID: 6664
	[Preserve]
	public class ActionReplaceItems : ActionBaseItemAction
	{
		// Token: 0x0600CB9D RID: 52125 RVA: 0x004A577E File Offset: 0x004A397E
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnClientActionStarted(EntityPlayer player)
		{
			this.replaceItemTag = FastTags<TagGroup.Global>.Parse(this.itemTags);
		}

		// Token: 0x0600CB9E RID: 52126 RVA: 0x004A5794 File Offset: 0x004A3994
		[PublicizedFrom(EAccessModifier.Protected)]
		public override bool CheckEquipmentReplace(Equipment equipment, int slot)
		{
			ItemValue item = ItemClass.GetItem(this.ReplacedByItem, false);
			return equipment.PreferredItemSlot(item) == slot;
		}

		// Token: 0x0600CB9F RID: 52127 RVA: 0x004A57B8 File Offset: 0x004A39B8
		[PublicizedFrom(EAccessModifier.Protected)]
		public override bool HandleItemStackChange(ref ItemStack stack, EntityPlayer player)
		{
			if (stack.IsEmpty() || !stack.itemValue.ItemClass.HasAnyTags(this.replaceItemTag) || !(stack.itemValue.ItemClass.GetItemName() != this.ReplacedByItem))
			{
				return false;
			}
			if (this.count != -1)
			{
				if (this.countType == ActionBaseItemAction.CountTypes.Items)
				{
					if (stack.count <= this.count)
					{
						this.count -= stack.count;
						stack = new ItemStack(ItemClass.GetItem(this.ReplacedByItem, false), stack.count);
					}
					else
					{
						stack.count -= this.count;
						ItemStack itemStack = new ItemStack(ItemClass.GetItem(this.ReplacedByItem, false), this.count);
						base.AddStack(player as EntityPlayerLocal, itemStack);
						this.count = 0;
						this.isFinished = true;
					}
				}
				else
				{
					stack = new ItemStack(ItemClass.GetItem(this.ReplacedByItem, false), stack.count);
					this.count--;
					if (this.count == 0)
					{
						this.isFinished = true;
					}
				}
				return true;
			}
			stack = new ItemStack(ItemClass.GetItem(this.ReplacedByItem, false), stack.count);
			return true;
		}

		// Token: 0x0600CBA0 RID: 52128 RVA: 0x004A5908 File Offset: 0x004A3B08
		[PublicizedFrom(EAccessModifier.Protected)]
		public override bool HandleItemValueChange(ref ItemValue itemValue, EntityPlayer player)
		{
			if (!itemValue.IsEmpty() && itemValue.ItemClass.HasAnyTags(this.replaceItemTag) && itemValue.ItemClass.GetItemName() != this.ReplacedByItem)
			{
				itemValue = ItemClass.GetItem(this.ReplacedByItem, false).Clone();
				if (this.count != -1)
				{
					this.count--;
					if (this.count == 0)
					{
						this.isFinished = true;
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x0600CBA1 RID: 52129 RVA: 0x004A5987 File Offset: 0x004A3B87
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			if (properties.Values.ContainsKey(ActionReplaceItems.PropReplacedByItem))
			{
				this.ReplacedByItem = properties.Values[ActionReplaceItems.PropReplacedByItem];
			}
		}

		// Token: 0x0600CBA2 RID: 52130 RVA: 0x004A59B8 File Offset: 0x004A3BB8
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionReplaceItems
			{
				ReplacedByItem = this.ReplacedByItem
			};
		}

		// Token: 0x04009A1B RID: 39451
		public string ReplacedByItem = "";

		// Token: 0x04009A1C RID: 39452
		public static string PropReplacedByItem = "replaced_by_item";

		// Token: 0x04009A1D RID: 39453
		[PublicizedFrom(EAccessModifier.Private)]
		public FastTags<TagGroup.Global> replaceItemTag = FastTags<TagGroup.Global>.none;
	}
}
