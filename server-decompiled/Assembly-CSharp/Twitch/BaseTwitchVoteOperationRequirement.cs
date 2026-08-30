using System;

namespace Twitch
{
	// Token: 0x02001872 RID: 6258
	public class BaseTwitchVoteOperationRequirement : BaseTwitchVoteRequirement
	{
		// Token: 0x0600C13E RID: 49470 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600C13F RID: 49471 RVA: 0x00478FC8 File Offset: 0x004771C8
		public override bool CanPerform(EntityPlayer player)
		{
			float num = this.LeftSide(player);
			float num2 = this.RightSide(player);
			switch (this.operation)
			{
			case BaseTwitchVoteOperationRequirement.OperationTypes.Equals:
			case BaseTwitchVoteOperationRequirement.OperationTypes.EQ:
			case BaseTwitchVoteOperationRequirement.OperationTypes.E:
				return num == num2;
			case BaseTwitchVoteOperationRequirement.OperationTypes.NotEquals:
			case BaseTwitchVoteOperationRequirement.OperationTypes.NEQ:
			case BaseTwitchVoteOperationRequirement.OperationTypes.NE:
				return num != num2;
			case BaseTwitchVoteOperationRequirement.OperationTypes.Less:
			case BaseTwitchVoteOperationRequirement.OperationTypes.LessThan:
			case BaseTwitchVoteOperationRequirement.OperationTypes.LT:
				return num < num2;
			case BaseTwitchVoteOperationRequirement.OperationTypes.Greater:
			case BaseTwitchVoteOperationRequirement.OperationTypes.GreaterThan:
			case BaseTwitchVoteOperationRequirement.OperationTypes.GT:
				return num > num2;
			case BaseTwitchVoteOperationRequirement.OperationTypes.LessOrEqual:
			case BaseTwitchVoteOperationRequirement.OperationTypes.LessThanOrEqualTo:
			case BaseTwitchVoteOperationRequirement.OperationTypes.LTE:
				return num <= num2;
			case BaseTwitchVoteOperationRequirement.OperationTypes.GreaterOrEqual:
			case BaseTwitchVoteOperationRequirement.OperationTypes.GreaterThanOrEqualTo:
			case BaseTwitchVoteOperationRequirement.OperationTypes.GTE:
				return num >= num2;
			default:
				return true;
			}
		}

		// Token: 0x0600C140 RID: 49472 RVA: 0x0003D2C6 File Offset: 0x0003B4C6
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual float LeftSide(EntityPlayer player)
		{
			return 0f;
		}

		// Token: 0x0600C141 RID: 49473 RVA: 0x0003D2C6 File Offset: 0x0003B4C6
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual float RightSide(EntityPlayer player)
		{
			return 0f;
		}

		// Token: 0x0600C142 RID: 49474 RVA: 0x00479066 File Offset: 0x00477266
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseEnum<BaseTwitchVoteOperationRequirement.OperationTypes>(BaseTwitchVoteOperationRequirement.PropOperation, ref this.operation);
		}

		// Token: 0x04009242 RID: 37442
		[PublicizedFrom(EAccessModifier.Protected)]
		public BaseTwitchVoteOperationRequirement.OperationTypes operation;

		// Token: 0x04009243 RID: 37443
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropOperation = "operation";

		// Token: 0x02001873 RID: 6259
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum OperationTypes
		{
			// Token: 0x04009245 RID: 37445
			None,
			// Token: 0x04009246 RID: 37446
			Equals,
			// Token: 0x04009247 RID: 37447
			EQ,
			// Token: 0x04009248 RID: 37448
			E,
			// Token: 0x04009249 RID: 37449
			NotEquals,
			// Token: 0x0400924A RID: 37450
			NEQ,
			// Token: 0x0400924B RID: 37451
			NE,
			// Token: 0x0400924C RID: 37452
			Less,
			// Token: 0x0400924D RID: 37453
			LessThan,
			// Token: 0x0400924E RID: 37454
			LT,
			// Token: 0x0400924F RID: 37455
			Greater,
			// Token: 0x04009250 RID: 37456
			GreaterThan,
			// Token: 0x04009251 RID: 37457
			GT,
			// Token: 0x04009252 RID: 37458
			LessOrEqual,
			// Token: 0x04009253 RID: 37459
			LessThanOrEqualTo,
			// Token: 0x04009254 RID: 37460
			LTE,
			// Token: 0x04009255 RID: 37461
			GreaterOrEqual,
			// Token: 0x04009256 RID: 37462
			GreaterThanOrEqualTo,
			// Token: 0x04009257 RID: 37463
			GTE
		}
	}
}
