using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019A4 RID: 6564
	[Preserve]
	public class ActionRemoveItems : ActionBaseItemAction
	{
		// Token: 0x0600C90D RID: 51469 RVA: 0x0049CA4C File Offset: 0x0049AC4C
		[PublicizedFrom(EAccessModifier.Protected)]
		public override bool HandleItemStackChange(ref ItemStack stack, EntityPlayer player)
		{
			if (stack.IsEmpty() || (!(this.itemTags == "") && !stack.itemValue.ItemClass.HasAnyTags(this.fastItemTags)))
			{
				return false;
			}
			if (this.count != -1)
			{
				if (this.countType == ActionBaseItemAction.CountTypes.Items)
				{
					if (stack.count >= this.count)
					{
						stack.count -= this.count;
						this.count = 0;
						this.isFinished = true;
						if (stack.count == 0)
						{
							stack = ItemStack.Empty;
						}
					}
					else
					{
						this.count -= stack.count;
						stack = ItemStack.Empty;
					}
				}
				else
				{
					stack = ItemStack.Empty;
					this.count--;
					if (this.count == 0)
					{
						this.isFinished = true;
					}
				}
				return true;
			}
			stack = ItemStack.Empty;
			return true;
		}

		// Token: 0x0600C90E RID: 51470 RVA: 0x0049CB38 File Offset: 0x0049AD38
		[PublicizedFrom(EAccessModifier.Protected)]
		public override bool HandleItemValueChange(ref ItemValue itemValue, EntityPlayer player)
		{
			if (!itemValue.IsEmpty() && (this.itemTags == "" || itemValue.ItemClass.HasAnyTags(this.fastItemTags)))
			{
				itemValue = ItemValue.None;
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

		// Token: 0x0600C90F RID: 51471 RVA: 0x0049CBA4 File Offset: 0x0049ADA4
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionRemoveItems();
		}
	}
}
