using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019D7 RID: 6615
	[Preserve]
	public class ActionReplaceItems : ActionBaseItemAction
	{
		// Token: 0x0600CA89 RID: 51849 RVA: 0x004A1C7E File Offset: 0x0049FE7E
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnClientActionStarted(EntityPlayer player)
		{
			this.replaceItemTag = FastTags<TagGroup.Global>.Parse(this.itemTags);
		}

		// Token: 0x0600CA8A RID: 51850 RVA: 0x004A1C94 File Offset: 0x0049FE94
		[PublicizedFrom(EAccessModifier.Protected)]
		public override bool CheckEquipmentReplace(Equipment equipment, int slot)
		{
			ItemValue item = ItemClass.GetItem(this.ReplacedByItem, false);
			return equipment.PreferredItemSlot(item) == slot;
		}

		// Token: 0x0600CA8B RID: 51851 RVA: 0x004A1CB8 File Offset: 0x0049FEB8
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

		// Token: 0x0600CA8C RID: 51852 RVA: 0x004A1E08 File Offset: 0x004A0008
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

		// Token: 0x0600CA8D RID: 51853 RVA: 0x004A1E87 File Offset: 0x004A0087
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			if (properties.Values.ContainsKey(ActionReplaceItems.PropReplacedByItem))
			{
				this.ReplacedByItem = properties.Values[ActionReplaceItems.PropReplacedByItem];
			}
		}

		// Token: 0x0600CA8E RID: 51854 RVA: 0x004A1EB8 File Offset: 0x004A00B8
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionReplaceItems
			{
				ReplacedByItem = this.ReplacedByItem
			};
		}

		// Token: 0x04009932 RID: 39218
		public string ReplacedByItem = "";

		// Token: 0x04009933 RID: 39219
		public static string PropReplacedByItem = "replaced_by_item";

		// Token: 0x04009934 RID: 39220
		[PublicizedFrom(EAccessModifier.Private)]
		public FastTags<TagGroup.Global> replaceItemTag = FastTags<TagGroup.Global>.none;
	}
}
