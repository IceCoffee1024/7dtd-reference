using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019BC RID: 6588
	[Preserve]
	public class ActionBlockHealth : ActionBaseBlockAction
	{
		// Token: 0x0600C8BC RID: 51388 RVA: 0x0049C2AA File Offset: 0x0049A4AA
		[PublicizedFrom(EAccessModifier.Protected)]
		public override bool NeedsDamage()
		{
			return this.healthState == ActionBlockHealth.HealthStates.Remove || this.healthState == ActionBlockHealth.HealthStates.RemoveNoBreak;
		}

		// Token: 0x0600C8BD RID: 51389 RVA: 0x0049C2C0 File Offset: 0x0049A4C0
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

		// Token: 0x0600C8BE RID: 51390 RVA: 0x0049C410 File Offset: 0x0049A610
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			this.Properties.ParseEnum<ActionBlockHealth.HealthStates>(ActionBlockHealth.PropHealthState, ref this.healthState);
			this.Properties.ParseInt(ActionBlockHealth.PropHealthAmount, ref this.amount);
		}

		// Token: 0x0600C8BF RID: 51391 RVA: 0x0049C445 File Offset: 0x0049A645
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionBlockHealth
			{
				healthState = this.healthState,
				amount = this.amount
			};
		}

		// Token: 0x04009861 RID: 39009
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionBlockHealth.HealthStates healthState = ActionBlockHealth.HealthStates.Full;

		// Token: 0x04009862 RID: 39010
		[PublicizedFrom(EAccessModifier.Protected)]
		public int amount;

		// Token: 0x04009863 RID: 39011
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropHealthState = "health_state";

		// Token: 0x04009864 RID: 39012
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropHealthAmount = "health_amount";

		// Token: 0x020019BD RID: 6589
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum HealthStates
		{
			// Token: 0x04009866 RID: 39014
			OneHealth,
			// Token: 0x04009867 RID: 39015
			Half,
			// Token: 0x04009868 RID: 39016
			Full,
			// Token: 0x04009869 RID: 39017
			Remove,
			// Token: 0x0400986A RID: 39018
			RemoveNoBreak
		}
	}
}
