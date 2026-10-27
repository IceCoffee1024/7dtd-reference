using System;

namespace Twitch
{
	// Token: 0x020018CF RID: 6351
	public class BaseTwitchVoteOperationRequirement : BaseTwitchVoteRequirement
	{
		// Token: 0x0600C3AA RID: 50090 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600C3AB RID: 50091 RVA: 0x00480EB0 File Offset: 0x0047F0B0
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

		// Token: 0x0600C3AC RID: 50092 RVA: 0x0003D372 File Offset: 0x0003B572
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual float LeftSide(EntityPlayer player)
		{
			return 0f;
		}

		// Token: 0x0600C3AD RID: 50093 RVA: 0x0003D372 File Offset: 0x0003B572
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual float RightSide(EntityPlayer player)
		{
			return 0f;
		}

		// Token: 0x0600C3AE RID: 50094 RVA: 0x00480F4E File Offset: 0x0047F14E
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseEnum<BaseTwitchVoteOperationRequirement.OperationTypes>(BaseTwitchVoteOperationRequirement.PropOperation, ref this.operation);
		}

		// Token: 0x040093F1 RID: 37873
		[PublicizedFrom(EAccessModifier.Protected)]
		public BaseTwitchVoteOperationRequirement.OperationTypes operation;

		// Token: 0x040093F2 RID: 37874
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropOperation = "operation";

		// Token: 0x020018D0 RID: 6352
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum OperationTypes
		{
			// Token: 0x040093F4 RID: 37876
			None,
			// Token: 0x040093F5 RID: 37877
			Equals,
			// Token: 0x040093F6 RID: 37878
			EQ,
			// Token: 0x040093F7 RID: 37879
			E,
			// Token: 0x040093F8 RID: 37880
			NotEquals,
			// Token: 0x040093F9 RID: 37881
			NEQ,
			// Token: 0x040093FA RID: 37882
			NE,
			// Token: 0x040093FB RID: 37883
			Less,
			// Token: 0x040093FC RID: 37884
			LessThan,
			// Token: 0x040093FD RID: 37885
			LT,
			// Token: 0x040093FE RID: 37886
			Greater,
			// Token: 0x040093FF RID: 37887
			GreaterThan,
			// Token: 0x04009400 RID: 37888
			GT,
			// Token: 0x04009401 RID: 37889
			LessOrEqual,
			// Token: 0x04009402 RID: 37890
			LessThanOrEqualTo,
			// Token: 0x04009403 RID: 37891
			LTE,
			// Token: 0x04009404 RID: 37892
			GreaterOrEqual,
			// Token: 0x04009405 RID: 37893
			GreaterThanOrEqualTo,
			// Token: 0x04009406 RID: 37894
			GTE
		}
	}
}
