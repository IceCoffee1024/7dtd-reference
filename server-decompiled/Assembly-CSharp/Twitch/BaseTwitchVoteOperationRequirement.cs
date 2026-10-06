using System;

namespace Twitch
{
	// Token: 0x0200189E RID: 6302
	public class BaseTwitchVoteOperationRequirement : BaseTwitchVoteRequirement
	{
		// Token: 0x0600C296 RID: 49814 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600C297 RID: 49815 RVA: 0x0047D504 File Offset: 0x0047B704
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

		// Token: 0x0600C298 RID: 49816 RVA: 0x0003D3B2 File Offset: 0x0003B5B2
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual float LeftSide(EntityPlayer player)
		{
			return 0f;
		}

		// Token: 0x0600C299 RID: 49817 RVA: 0x0003D3B2 File Offset: 0x0003B5B2
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual float RightSide(EntityPlayer player)
		{
			return 0f;
		}

		// Token: 0x0600C29A RID: 49818 RVA: 0x0047D5A2 File Offset: 0x0047B7A2
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseEnum<BaseTwitchVoteOperationRequirement.OperationTypes>(BaseTwitchVoteOperationRequirement.PropOperation, ref this.operation);
		}

		// Token: 0x04009308 RID: 37640
		[PublicizedFrom(EAccessModifier.Protected)]
		public BaseTwitchVoteOperationRequirement.OperationTypes operation;

		// Token: 0x04009309 RID: 37641
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropOperation = "operation";

		// Token: 0x0200189F RID: 6303
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum OperationTypes
		{
			// Token: 0x0400930B RID: 37643
			None,
			// Token: 0x0400930C RID: 37644
			Equals,
			// Token: 0x0400930D RID: 37645
			EQ,
			// Token: 0x0400930E RID: 37646
			E,
			// Token: 0x0400930F RID: 37647
			NotEquals,
			// Token: 0x04009310 RID: 37648
			NEQ,
			// Token: 0x04009311 RID: 37649
			NE,
			// Token: 0x04009312 RID: 37650
			Less,
			// Token: 0x04009313 RID: 37651
			LessThan,
			// Token: 0x04009314 RID: 37652
			LT,
			// Token: 0x04009315 RID: 37653
			Greater,
			// Token: 0x04009316 RID: 37654
			GreaterThan,
			// Token: 0x04009317 RID: 37655
			GT,
			// Token: 0x04009318 RID: 37656
			LessOrEqual,
			// Token: 0x04009319 RID: 37657
			LessThanOrEqualTo,
			// Token: 0x0400931A RID: 37658
			LTE,
			// Token: 0x0400931B RID: 37659
			GreaterOrEqual,
			// Token: 0x0400931C RID: 37660
			GreaterThanOrEqualTo,
			// Token: 0x0400931D RID: 37661
			GTE
		}
	}
}
