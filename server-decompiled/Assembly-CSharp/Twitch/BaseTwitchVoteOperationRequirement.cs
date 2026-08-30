using System;

namespace Twitch
{
	// Token: 0x0200184C RID: 6220
	public class BaseTwitchVoteOperationRequirement : BaseTwitchVoteRequirement
	{
		// Token: 0x0600BFB9 RID: 49081 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600BFBA RID: 49082 RVA: 0x0047322C File Offset: 0x0047142C
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

		// Token: 0x0600BFBB RID: 49083 RVA: 0x0003D066 File Offset: 0x0003B266
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual float LeftSide(EntityPlayer player)
		{
			return 0f;
		}

		// Token: 0x0600BFBC RID: 49084 RVA: 0x0003D066 File Offset: 0x0003B266
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual float RightSide(EntityPlayer player)
		{
			return 0f;
		}

		// Token: 0x0600BFBD RID: 49085 RVA: 0x004732CA File Offset: 0x004714CA
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseEnum<BaseTwitchVoteOperationRequirement.OperationTypes>(BaseTwitchVoteOperationRequirement.PropOperation, ref this.operation);
		}

		// Token: 0x04009126 RID: 37158
		[PublicizedFrom(EAccessModifier.Protected)]
		public BaseTwitchVoteOperationRequirement.OperationTypes operation;

		// Token: 0x04009127 RID: 37159
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropOperation = "operation";

		// Token: 0x0200184D RID: 6221
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum OperationTypes
		{
			// Token: 0x04009129 RID: 37161
			None,
			// Token: 0x0400912A RID: 37162
			Equals,
			// Token: 0x0400912B RID: 37163
			EQ,
			// Token: 0x0400912C RID: 37164
			E,
			// Token: 0x0400912D RID: 37165
			NotEquals,
			// Token: 0x0400912E RID: 37166
			NEQ,
			// Token: 0x0400912F RID: 37167
			NE,
			// Token: 0x04009130 RID: 37168
			Less,
			// Token: 0x04009131 RID: 37169
			LessThan,
			// Token: 0x04009132 RID: 37170
			LT,
			// Token: 0x04009133 RID: 37171
			Greater,
			// Token: 0x04009134 RID: 37172
			GreaterThan,
			// Token: 0x04009135 RID: 37173
			GT,
			// Token: 0x04009136 RID: 37174
			LessOrEqual,
			// Token: 0x04009137 RID: 37175
			LessThanOrEqualTo,
			// Token: 0x04009138 RID: 37176
			LTE,
			// Token: 0x04009139 RID: 37177
			GreaterOrEqual,
			// Token: 0x0400913A RID: 37178
			GreaterThanOrEqualTo,
			// Token: 0x0400913B RID: 37179
			GTE
		}
	}
}
