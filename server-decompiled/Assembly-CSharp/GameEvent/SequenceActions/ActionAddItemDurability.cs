using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x0200195C RID: 6492
	[Preserve]
	public class ActionAddItemDurability : ActionBaseItemAction
	{
		// Token: 0x0600C7D2 RID: 51154 RVA: 0x00494837 File Offset: 0x00492A37
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnClientActionStarted(EntityPlayer player)
		{
			base.OnClientActionStarted(player);
			this.amount = GameEventManager.GetFloatValue(player, this.amountText, 0.25f);
		}

		// Token: 0x0600C7D3 RID: 51155 RVA: 0x00494858 File Offset: 0x00492A58
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

		// Token: 0x0600C7D4 RID: 51156 RVA: 0x004949FC File Offset: 0x00492BFC
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

		// Token: 0x0600C7D5 RID: 51157 RVA: 0x00494B4E File Offset: 0x00492D4E
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionAddItemDurability.PropAmount, ref this.amountText);
			properties.ParseBool(ActionAddItemDurability.PropIsPercent, ref this.isPercent);
			properties.ParseBool(ActionAddItemDurability.PropIsNegative, ref this.isNegative);
		}

		// Token: 0x0600C7D6 RID: 51158 RVA: 0x00494B8A File Offset: 0x00492D8A
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

		// Token: 0x04009699 RID: 38553
		[PublicizedFrom(EAccessModifier.Protected)]
		public string amountText = "";

		// Token: 0x0400969A RID: 38554
		[PublicizedFrom(EAccessModifier.Protected)]
		public float amount = 0.25f;

		// Token: 0x0400969B RID: 38555
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool isPercent = true;

		// Token: 0x0400969C RID: 38556
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool isNegative;

		// Token: 0x0400969D RID: 38557
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropAmount = "amount";

		// Token: 0x0400969E RID: 38558
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropIsPercent = "is_percent";

		// Token: 0x0400969F RID: 38559
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropIsNegative = "is_negative";
	}
}
