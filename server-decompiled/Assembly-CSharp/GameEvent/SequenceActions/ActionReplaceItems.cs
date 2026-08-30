using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019AB RID: 6571
	[Preserve]
	public class ActionReplaceItems : ActionBaseItemAction
	{
		// Token: 0x0600C931 RID: 51505 RVA: 0x0049D78E File Offset: 0x0049B98E
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnClientActionStarted(EntityPlayer player)
		{
			this.replaceItemTag = FastTags<TagGroup.Global>.Parse(this.itemTags);
		}

		// Token: 0x0600C932 RID: 51506 RVA: 0x0049D7A4 File Offset: 0x0049B9A4
		[PublicizedFrom(EAccessModifier.Protected)]
		public override bool CheckEquipmentReplace(Equipment equipment, int slot)
		{
			ItemValue item = ItemClass.GetItem(this.ReplacedByItem, false);
			return equipment.PreferredItemSlot(item) == slot;
		}

		// Token: 0x0600C933 RID: 51507 RVA: 0x0049D7C8 File Offset: 0x0049B9C8
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

		// Token: 0x0600C934 RID: 51508 RVA: 0x0049D918 File Offset: 0x0049BB18
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

		// Token: 0x0600C935 RID: 51509 RVA: 0x0049D997 File Offset: 0x0049BB97
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			if (properties.Values.ContainsKey(ActionReplaceItems.PropReplacedByItem))
			{
				this.ReplacedByItem = properties.Values[ActionReplaceItems.PropReplacedByItem];
			}
		}

		// Token: 0x0600C936 RID: 51510 RVA: 0x0049D9C8 File Offset: 0x0049BBC8
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionReplaceItems
			{
				ReplacedByItem = this.ReplacedByItem
			};
		}

		// Token: 0x0400986C RID: 39020
		public string ReplacedByItem = "";

		// Token: 0x0400986D RID: 39021
		public static string PropReplacedByItem = "replaced_by_item";

		// Token: 0x0400986E RID: 39022
		[PublicizedFrom(EAccessModifier.Private)]
		public FastTags<TagGroup.Global> replaceItemTag = FastTags<TagGroup.Global>.none;
	}
}
