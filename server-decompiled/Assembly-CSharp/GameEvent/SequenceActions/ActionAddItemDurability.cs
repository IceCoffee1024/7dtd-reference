using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001936 RID: 6454
	[Preserve]
	public class ActionAddItemDurability : ActionBaseItemAction
	{
		// Token: 0x0600C64B RID: 50763 RVA: 0x0048E4EB File Offset: 0x0048C6EB
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnClientActionStarted(EntityPlayer player)
		{
			base.OnClientActionStarted(player);
			this.amount = GameEventManager.GetFloatValue(player, this.amountText, 0.25f);
		}

		// Token: 0x0600C64C RID: 50764 RVA: 0x0048E50C File Offset: 0x0048C70C
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

		// Token: 0x0600C64D RID: 50765 RVA: 0x0048E6B0 File Offset: 0x0048C8B0
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

		// Token: 0x0600C64E RID: 50766 RVA: 0x0048E802 File Offset: 0x0048CA02
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionAddItemDurability.PropAmount, ref this.amountText);
			properties.ParseBool(ActionAddItemDurability.PropIsPercent, ref this.isPercent);
			properties.ParseBool(ActionAddItemDurability.PropIsNegative, ref this.isNegative);
		}

		// Token: 0x0600C64F RID: 50767 RVA: 0x0048E83E File Offset: 0x0048CA3E
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

		// Token: 0x04009570 RID: 38256
		[PublicizedFrom(EAccessModifier.Protected)]
		public string amountText = "";

		// Token: 0x04009571 RID: 38257
		[PublicizedFrom(EAccessModifier.Protected)]
		public float amount = 0.25f;

		// Token: 0x04009572 RID: 38258
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool isPercent = true;

		// Token: 0x04009573 RID: 38259
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool isNegative;

		// Token: 0x04009574 RID: 38260
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropAmount = "amount";

		// Token: 0x04009575 RID: 38261
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropIsPercent = "is_percent";

		// Token: 0x04009576 RID: 38262
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropIsNegative = "is_negative";
	}
}
