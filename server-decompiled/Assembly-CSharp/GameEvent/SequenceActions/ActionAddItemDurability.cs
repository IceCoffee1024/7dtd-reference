using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001988 RID: 6536
	[Preserve]
	public class ActionAddItemDurability : ActionBaseItemAction
	{
		// Token: 0x0600C92A RID: 51498 RVA: 0x00498D23 File Offset: 0x00496F23
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnClientActionStarted(EntityPlayer player)
		{
			base.OnClientActionStarted(player);
			this.amount = GameEventManager.GetFloatValue(player, this.amountText, 0.25f);
		}

		// Token: 0x0600C92B RID: 51499 RVA: 0x00498D44 File Offset: 0x00496F44
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

		// Token: 0x0600C92C RID: 51500 RVA: 0x00498EE8 File Offset: 0x004970E8
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

		// Token: 0x0600C92D RID: 51501 RVA: 0x0049903A File Offset: 0x0049723A
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionAddItemDurability.PropAmount, ref this.amountText);
			properties.ParseBool(ActionAddItemDurability.PropIsPercent, ref this.isPercent);
			properties.ParseBool(ActionAddItemDurability.PropIsNegative, ref this.isNegative);
		}

		// Token: 0x0600C92E RID: 51502 RVA: 0x00499076 File Offset: 0x00497276
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

		// Token: 0x0400975F RID: 38751
		[PublicizedFrom(EAccessModifier.Protected)]
		public string amountText = "";

		// Token: 0x04009760 RID: 38752
		[PublicizedFrom(EAccessModifier.Protected)]
		public float amount = 0.25f;

		// Token: 0x04009761 RID: 38753
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool isPercent = true;

		// Token: 0x04009762 RID: 38754
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool isNegative;

		// Token: 0x04009763 RID: 38755
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropAmount = "amount";

		// Token: 0x04009764 RID: 38756
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropIsPercent = "is_percent";

		// Token: 0x04009765 RID: 38757
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropIsNegative = "is_negative";
	}
}
