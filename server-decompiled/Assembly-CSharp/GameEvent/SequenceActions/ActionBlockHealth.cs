using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A40 RID: 6720
	[Preserve]
	public class ActionBlockHealth : ActionBaseBlockAction
	{
		// Token: 0x0600CCB1 RID: 52401 RVA: 0x004AA63E File Offset: 0x004A883E
		[PublicizedFrom(EAccessModifier.Protected)]
		public override bool NeedsDamage()
		{
			return this.healthState == ActionBlockHealth.HealthStates.Remove || this.healthState == ActionBlockHealth.HealthStates.RemoveNoBreak;
		}

		// Token: 0x0600CCB2 RID: 52402 RVA: 0x004AA654 File Offset: 0x004A8854
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

		// Token: 0x0600CCB3 RID: 52403 RVA: 0x004AA7A4 File Offset: 0x004A89A4
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			this.Properties.ParseEnum<ActionBlockHealth.HealthStates>(ActionBlockHealth.PropHealthState, ref this.healthState);
			this.Properties.ParseInt(ActionBlockHealth.PropHealthAmount, ref this.amount);
		}

		// Token: 0x0600CCB4 RID: 52404 RVA: 0x004AA7D9 File Offset: 0x004A89D9
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionBlockHealth
			{
				healthState = this.healthState,
				amount = this.amount
			};
		}

		// Token: 0x04009B41 RID: 39745
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionBlockHealth.HealthStates healthState = ActionBlockHealth.HealthStates.Full;

		// Token: 0x04009B42 RID: 39746
		[PublicizedFrom(EAccessModifier.Protected)]
		public int amount;

		// Token: 0x04009B43 RID: 39747
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropHealthState = "health_state";

		// Token: 0x04009B44 RID: 39748
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropHealthAmount = "health_amount";

		// Token: 0x02001A41 RID: 6721
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum HealthStates
		{
			// Token: 0x04009B46 RID: 39750
			OneHealth,
			// Token: 0x04009B47 RID: 39751
			Half,
			// Token: 0x04009B48 RID: 39752
			Full,
			// Token: 0x04009B49 RID: 39753
			Remove,
			// Token: 0x04009B4A RID: 39754
			RemoveNoBreak
		}
	}
}
