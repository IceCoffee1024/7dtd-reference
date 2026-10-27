using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A01 RID: 6657
	[Preserve]
	public class ActionRemoveItems : ActionBaseItemAction
	{
		// Token: 0x0600CB79 RID: 52089 RVA: 0x004A4A10 File Offset: 0x004A2C10
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

		// Token: 0x0600CB7A RID: 52090 RVA: 0x004A4AFC File Offset: 0x004A2CFC
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

		// Token: 0x0600CB7B RID: 52091 RVA: 0x004A4B68 File Offset: 0x004A2D68
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionRemoveItems();
		}
	}
}
