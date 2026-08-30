using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x0200197E RID: 6526
	[Preserve]
	public class ActionRemoveItems : ActionBaseItemAction
	{
		// Token: 0x0600C786 RID: 51078 RVA: 0x00496708 File Offset: 0x00494908
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

		// Token: 0x0600C787 RID: 51079 RVA: 0x004967F4 File Offset: 0x004949F4
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

		// Token: 0x0600C788 RID: 51080 RVA: 0x00496860 File Offset: 0x00494A60
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionRemoveItems();
		}
	}
}
