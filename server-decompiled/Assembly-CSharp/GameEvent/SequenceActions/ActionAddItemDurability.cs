using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019B9 RID: 6585
	[Preserve]
	public class ActionAddItemDurability : ActionBaseItemAction
	{
		// Token: 0x0600CA3E RID: 51774 RVA: 0x0049C717 File Offset: 0x0049A917
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnClientActionStarted(EntityPlayer player)
		{
			base.OnClientActionStarted(player);
			this.amount = GameEventManager.GetFloatValue(player, this.amountText, 0.25f);
		}

		// Token: 0x0600CA3F RID: 51775 RVA: 0x0049C738 File Offset: 0x0049A938
		[PublicizedFrom(EAccessModifier.Protected)]
		public override bool HandleItemStackChange(ref ItemStack stack, EntityPlayer player)
		{
			if (stack.itemValue.MaxUseTimes <= 0 || EffectManager.GetValue(PassiveEffects.DegradationPerUse, stack.itemValue, 1f, player, null, stack.itemValue.ItemClass.ItemTags, true, true, true, true, true, 1, true, false) <= 0f)
			{
				return false;
			}
			if (this.itemTags != "" && !stack.itemValue.ItemClass.HasAnyTags(this.fastItemTags))
			{
				return false;
			}
			if (this.isNegative)
			{
				if (this.isPercent)
				{
					stack.itemValue.UseTimes += (float)stack.itemValue.MaxUseTimes * this.amount;
				}
				else
				{
					stack.itemValue.UseTimes += this.amount;
				}
			}
			else if (this.isPercent)
			{
				stack.itemValue.UseTimes -= (float)stack.itemValue.MaxUseTimes * this.amount;
			}
			else
			{
				stack.itemValue.UseTimes -= this.amount;
			}
			if (stack.itemValue.UseTimes < 0f)
			{
				stack.itemValue.UseTimes = 0f;
			}
			if (stack.itemValue.UseTimes > (float)stack.itemValue.MaxUseTimes)
			{
				stack.itemValue.UseTimes = (float)stack.itemValue.MaxUseTimes;
			}
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

		// Token: 0x0600CA40 RID: 51776 RVA: 0x0049C8DC File Offset: 0x0049AADC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override bool HandleItemValueChange(ref ItemValue itemValue, EntityPlayer player)
		{
			if (itemValue.MaxUseTimes <= 0 || EffectManager.GetValue(PassiveEffects.DegradationPerUse, itemValue, 1f, player, null, itemValue.ItemClass.ItemTags, true, true, true, true, true, 1, true, false) <= 0f)
			{
				return false;
			}
			if (this.itemTags != "" && !itemValue.ItemClass.HasAnyTags(this.fastItemTags))
			{
				return false;
			}
			if (this.isNegative)
			{
				if (this.isPercent)
				{
					itemValue.UseTimes += (float)itemValue.MaxUseTimes * this.amount;
				}
				else
				{
					itemValue.UseTimes += this.amount;
				}
			}
			else if (this.isPercent)
			{
				itemValue.UseTimes -= (float)itemValue.MaxUseTimes * this.amount;
			}
			else
			{
				itemValue.UseTimes -= this.amount;
			}
			if (itemValue.UseTimes < 0f)
			{
				itemValue.UseTimes = 0f;
			}
			if (itemValue.UseTimes > (float)itemValue.MaxUseTimes)
			{
				itemValue.UseTimes = (float)itemValue.MaxUseTimes;
			}
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

		// Token: 0x0600CA41 RID: 51777 RVA: 0x0049CA2E File Offset: 0x0049AC2E
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionAddItemDurability.PropAmount, ref this.amountText);
			properties.ParseBool(ActionAddItemDurability.PropIsPercent, ref this.isPercent);
			properties.ParseBool(ActionAddItemDurability.PropIsNegative, ref this.isNegative);
		}

		// Token: 0x0600CA42 RID: 51778 RVA: 0x0049CA6A File Offset: 0x0049AC6A
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionAddItemDurability
			{
				isPercent = this.isPercent,
				isNegative = this.isNegative,
				amountText = this.amountText
			};
		}

		// Token: 0x04009848 RID: 38984
		[PublicizedFrom(EAccessModifier.Protected)]
		public string amountText = "";

		// Token: 0x04009849 RID: 38985
		[PublicizedFrom(EAccessModifier.Protected)]
		public float amount = 0.25f;

		// Token: 0x0400984A RID: 38986
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool isPercent = true;

		// Token: 0x0400984B RID: 38987
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool isNegative;

		// Token: 0x0400984C RID: 38988
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropAmount = "amount";

		// Token: 0x0400984D RID: 38989
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropIsPercent = "is_percent";

		// Token: 0x0400984E RID: 38990
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropIsNegative = "is_negative";
	}
}
