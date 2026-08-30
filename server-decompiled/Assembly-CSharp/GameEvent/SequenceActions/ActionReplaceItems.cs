using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001985 RID: 6533
	[Preserve]
	public class ActionReplaceItems : ActionBaseItemAction
	{
		// Token: 0x0600C7AA RID: 51114 RVA: 0x0049744A File Offset: 0x0049564A
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnClientActionStarted(EntityPlayer player)
		{
			this.replaceItemTag = FastTags<TagGroup.Global>.Parse(this.itemTags);
		}

		// Token: 0x0600C7AB RID: 51115 RVA: 0x00497460 File Offset: 0x00495660
		[PublicizedFrom(EAccessModifier.Protected)]
		public override bool CheckEquipmentReplace(Equipment equipment, int slot)
		{
			ItemValue item = ItemClass.GetItem(this.ReplacedByItem, false);
			return equipment.PreferredItemSlot(item) == slot;
		}

		// Token: 0x0600C7AC RID: 51116 RVA: 0x00497484 File Offset: 0x00495684
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

		// Token: 0x0600C7AD RID: 51117 RVA: 0x004975D4 File Offset: 0x004957D4
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

		// Token: 0x0600C7AE RID: 51118 RVA: 0x00497653 File Offset: 0x00495853
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			if (properties.Values.ContainsKey(ActionReplaceItems.PropReplacedByItem))
			{
				this.ReplacedByItem = properties.Values[ActionReplaceItems.PropReplacedByItem];
			}
		}

		// Token: 0x0600C7AF RID: 51119 RVA: 0x00497684 File Offset: 0x00495884
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionReplaceItems
			{
				ReplacedByItem = this.ReplacedByItem
			};
		}

		// Token: 0x04009743 RID: 38723
		public string ReplacedByItem = "";

		// Token: 0x04009744 RID: 38724
		public static string PropReplacedByItem = "replaced_by_item";

		// Token: 0x04009745 RID: 38725
		[PublicizedFrom(EAccessModifier.Private)]
		public FastTags<TagGroup.Global> replaceItemTag = FastTags<TagGroup.Global>.none;
	}
}
