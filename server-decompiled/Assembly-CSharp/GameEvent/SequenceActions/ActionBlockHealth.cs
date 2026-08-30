using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019E3 RID: 6627
	[Preserve]
	public class ActionBlockHealth : ActionBaseBlockAction
	{
		// Token: 0x0600CA45 RID: 51781 RVA: 0x004A269E File Offset: 0x004A089E
		[PublicizedFrom(EAccessModifier.Protected)]
		public override bool NeedsDamage()
		{
			return this.healthState == ActionBlockHealth.HealthStates.Remove || this.healthState == ActionBlockHealth.HealthStates.RemoveNoBreak;
		}

		// Token: 0x0600CA46 RID: 51782 RVA: 0x004A26B4 File Offset: 0x004A08B4
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BlockChangeInfo UpdateBlock(World world, Vector3i currentPos, BlockValue blockValue)
		{
			if (!blockValue.isair)
			{
				switch (this.healthState)
				{
				case ActionBlockHealth.HealthStates.OneHealth:
				{
					int num = blockValue.Block.MaxDamage - 1;
					if (blockValue.damage != num)
					{
						blockValue.damage = num;
						return new BlockChangeInfo(currentPos, blockValue);
					}
					break;
				}
				case ActionBlockHealth.HealthStates.Half:
				{
					int num2 = blockValue.Block.MaxDamage / 2;
					if (blockValue.damage != num2)
					{
						blockValue.damage = num2;
						return new BlockChangeInfo(currentPos, blockValue);
					}
					break;
				}
				case ActionBlockHealth.HealthStates.Full:
					if (blockValue.damage != 0)
					{
						blockValue.damage = 0;
						return new BlockChangeInfo(currentPos, blockValue);
					}
					break;
				case ActionBlockHealth.HealthStates.Remove:
				{
					int num3 = blockValue.damage + this.amount;
					if (blockValue.damage != num3)
					{
						blockValue.damage = num3;
						if (blockValue.damage >= blockValue.Block.MaxDamage)
						{
							blockValue = blockValue.Block.DowngradeBlock;
						}
						return new BlockChangeInfo(currentPos, blockValue);
					}
					break;
				}
				case ActionBlockHealth.HealthStates.RemoveNoBreak:
				{
					int num4 = Mathf.Min(blockValue.Block.MaxDamage - 1, blockValue.damage + this.amount);
					if (blockValue.damage != num4)
					{
						blockValue.damage = num4;
						return new BlockChangeInfo(currentPos, blockValue);
					}
					break;
				}
				}
			}
			return null;
		}

		// Token: 0x0600CA47 RID: 51783 RVA: 0x004A2804 File Offset: 0x004A0A04
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			this.Properties.ParseEnum<ActionBlockHealth.HealthStates>(ActionBlockHealth.PropHealthState, ref this.healthState);
			this.Properties.ParseInt(ActionBlockHealth.PropHealthAmount, ref this.amount);
		}

		// Token: 0x0600CA48 RID: 51784 RVA: 0x004A2839 File Offset: 0x004A0A39
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionBlockHealth
			{
				healthState = this.healthState,
				amount = this.amount
			};
		}

		// Token: 0x04009992 RID: 39314
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionBlockHealth.HealthStates healthState = ActionBlockHealth.HealthStates.Full;

		// Token: 0x04009993 RID: 39315
		[PublicizedFrom(EAccessModifier.Protected)]
		public int amount;

		// Token: 0x04009994 RID: 39316
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropHealthState = "health_state";

		// Token: 0x04009995 RID: 39317
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropHealthAmount = "health_amount";

		// Token: 0x020019E4 RID: 6628
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum HealthStates
		{
			// Token: 0x04009997 RID: 39319
			OneHealth,
			// Token: 0x04009998 RID: 39320
			Half,
			// Token: 0x04009999 RID: 39321
			Full,
			// Token: 0x0400999A RID: 39322
			Remove,
			// Token: 0x0400999B RID: 39323
			RemoveNoBreak
		}
	}
}
