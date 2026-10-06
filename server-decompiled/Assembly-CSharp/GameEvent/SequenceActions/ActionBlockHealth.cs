using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A0F RID: 6671
	[Preserve]
	public class ActionBlockHealth : ActionBaseBlockAction
	{
		// Token: 0x0600CB9D RID: 52125 RVA: 0x004A6B42 File Offset: 0x004A4D42
		[PublicizedFrom(EAccessModifier.Protected)]
		public override bool NeedsDamage()
		{
			return this.healthState == ActionBlockHealth.HealthStates.Remove || this.healthState == ActionBlockHealth.HealthStates.RemoveNoBreak;
		}

		// Token: 0x0600CB9E RID: 52126 RVA: 0x004A6B58 File Offset: 0x004A4D58
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

		// Token: 0x0600CB9F RID: 52127 RVA: 0x004A6CA8 File Offset: 0x004A4EA8
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			this.Properties.ParseEnum<ActionBlockHealth.HealthStates>(ActionBlockHealth.PropHealthState, ref this.healthState);
			this.Properties.ParseInt(ActionBlockHealth.PropHealthAmount, ref this.amount);
		}

		// Token: 0x0600CBA0 RID: 52128 RVA: 0x004A6CDD File Offset: 0x004A4EDD
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionBlockHealth
			{
				healthState = this.healthState,
				amount = this.amount
			};
		}

		// Token: 0x04009A58 RID: 39512
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionBlockHealth.HealthStates healthState = ActionBlockHealth.HealthStates.Full;

		// Token: 0x04009A59 RID: 39513
		[PublicizedFrom(EAccessModifier.Protected)]
		public int amount;

		// Token: 0x04009A5A RID: 39514
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropHealthState = "health_state";

		// Token: 0x04009A5B RID: 39515
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropHealthAmount = "health_amount";

		// Token: 0x02001A10 RID: 6672
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum HealthStates
		{
			// Token: 0x04009A5D RID: 39517
			OneHealth,
			// Token: 0x04009A5E RID: 39518
			Half,
			// Token: 0x04009A5F RID: 39519
			Full,
			// Token: 0x04009A60 RID: 39520
			Remove,
			// Token: 0x04009A61 RID: 39521
			RemoveNoBreak
		}
	}
}
