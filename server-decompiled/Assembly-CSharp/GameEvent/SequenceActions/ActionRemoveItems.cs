using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019D0 RID: 6608
	[Preserve]
	public class ActionRemoveItems : ActionBaseItemAction
	{
		// Token: 0x0600CA65 RID: 51813 RVA: 0x004A0F10 File Offset: 0x0049F110
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

		// Token: 0x0600CA66 RID: 51814 RVA: 0x004A0FFC File Offset: 0x0049F1FC
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

		// Token: 0x0600CA67 RID: 51815 RVA: 0x004A1068 File Offset: 0x0049F268
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionRemoveItems();
		}
	}
}
