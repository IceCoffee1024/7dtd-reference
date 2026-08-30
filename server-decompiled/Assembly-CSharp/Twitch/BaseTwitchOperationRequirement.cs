using System;
using UnityEngine.Scripting;

namespace Twitch
{
	// Token: 0x02001856 RID: 6230
	[Preserve]
	public class BaseTwitchOperationRequirement : BaseTwitchRequirement
	{
		// Token: 0x0600C084 RID: 49284 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x1700179C RID: 6044
		// (get) Token: 0x0600C085 RID: 49285 RVA: 0x00474553 File Offset: 0x00472753
		// (set) Token: 0x0600C086 RID: 49286 RVA: 0x0047455E File Offset: 0x0047275E
		public bool StringCompareCaseSensitive
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return this.stringComparison == StringComparison.CurrentCulture;
			}
			[PublicizedFrom(EAccessModifier.Protected)]
			set
			{
				this.stringComparison = (value ? StringComparison.CurrentCulture : StringComparison.OrdinalIgnoreCase);
			}
		}

		// Token: 0x0600C087 RID: 49287 RVA: 0x00474570 File Offset: 0x00472770
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual bool compare(float valueA, float valueB)
		{
			switch (this.operation)
			{
			case BaseTwitchOperationRequirement.OperationTypes.Equals:
			case BaseTwitchOperationRequirement.OperationTypes.EQ:
			case BaseTwitchOperationRequirement.OperationTypes.E:
				return valueA == valueB;
			case BaseTwitchOperationRequirement.OperationTypes.NotEquals:
			case BaseTwitchOperationRequirement.OperationTypes.NEQ:
			case BaseTwitchOperationRequirement.OperationTypes.NE:
				return valueA != valueB;
			case BaseTwitchOperationRequirement.OperationTypes.Less:
			case BaseTwitchOperationRequirement.OperationTypes.LessThan:
			case BaseTwitchOperationRequirement.OperationTypes.LT:
				return valueA < valueB;
			case BaseTwitchOperationRequirement.OperationTypes.Greater:
			case BaseTwitchOperationRequirement.OperationTypes.GreaterThan:
			case BaseTwitchOperationRequirement.OperationTypes.GT:
				return valueA > valueB;
			case BaseTwitchOperationRequirement.OperationTypes.LessOrEqual:
			case BaseTwitchOperationRequirement.OperationTypes.LessThanOrEqualTo:
			case BaseTwitchOperationRequirement.OperationTypes.LTE:
				return valueA <= valueB;
			case BaseTwitchOperationRequirement.OperationTypes.GreaterOrEqual:
			case BaseTwitchOperationRequirement.OperationTypes.GreaterThanOrEqualTo:
			case BaseTwitchOperationRequirement.OperationTypes.GTE:
				return valueA >= valueB;
			default:
				return true;
			}
		}

		// Token: 0x0600C088 RID: 49288 RVA: 0x00474600 File Offset: 0x00472800
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual bool compare(string valueA, string valueB)
		{
			int num = string.Compare(valueA, valueB, this.stringComparison);
			switch (this.operation)
			{
			case BaseTwitchOperationRequirement.OperationTypes.Equals:
			case BaseTwitchOperationRequirement.OperationTypes.EQ:
			case BaseTwitchOperationRequirement.OperationTypes.E:
				return num == 0;
			case BaseTwitchOperationRequirement.OperationTypes.NotEquals:
			case BaseTwitchOperationRequirement.OperationTypes.NEQ:
			case BaseTwitchOperationRequirement.OperationTypes.NE:
				return num != 0;
			case BaseTwitchOperationRequirement.OperationTypes.Less:
			case BaseTwitchOperationRequirement.OperationTypes.LessThan:
			case BaseTwitchOperationRequirement.OperationTypes.LT:
				return num < 0;
			case BaseTwitchOperationRequirement.OperationTypes.Greater:
			case BaseTwitchOperationRequirement.OperationTypes.GreaterThan:
			case BaseTwitchOperationRequirement.OperationTypes.GT:
				return num > 0;
			case BaseTwitchOperationRequirement.OperationTypes.LessOrEqual:
			case BaseTwitchOperationRequirement.OperationTypes.LessThanOrEqualTo:
			case BaseTwitchOperationRequirement.OperationTypes.LTE:
				return num <= 0;
			case BaseTwitchOperationRequirement.OperationTypes.GreaterOrEqual:
			case BaseTwitchOperationRequirement.OperationTypes.GreaterThanOrEqualTo:
			case BaseTwitchOperationRequirement.OperationTypes.GTE:
				return num <= 0;
			default:
				return true;
			}
		}

		// Token: 0x0600C089 RID: 49289 RVA: 0x0047469C File Offset: 0x0047289C
		public override bool CanPerform(Entity target)
		{
			object obj = this.LeftSide(target);
			object obj2 = this.RightSide(target);
			if (obj is string)
			{
				return this.compare((string)obj, (string)obj2);
			}
			return this.compare(Convert.ToSingle(obj), Convert.ToSingle(obj2));
		}

		// Token: 0x0600C08A RID: 49290 RVA: 0x004746E6 File Offset: 0x004728E6
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual object LeftSide(Entity target)
		{
			return 0;
		}

		// Token: 0x0600C08B RID: 49291 RVA: 0x004746E6 File Offset: 0x004728E6
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual object RightSide(Entity target)
		{
			return 0;
		}

		// Token: 0x0600C08C RID: 49292 RVA: 0x004746EE File Offset: 0x004728EE
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseEnum<BaseTwitchOperationRequirement.OperationTypes>(BaseTwitchOperationRequirement.PropOperation, ref this.operation);
		}

		// Token: 0x04009119 RID: 37145
		[PublicizedFrom(EAccessModifier.Protected)]
		public BaseTwitchOperationRequirement.OperationTypes operation = BaseTwitchOperationRequirement.OperationTypes.Equals;

		// Token: 0x0400911A RID: 37146
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropOperation = "operation";

		// Token: 0x0400911B RID: 37147
		[PublicizedFrom(EAccessModifier.Private)]
		public StringComparison stringComparison;

		// Token: 0x02001857 RID: 6231
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum OperationTypes
		{
			// Token: 0x0400911D RID: 37149
			None,
			// Token: 0x0400911E RID: 37150
			Equals,
			// Token: 0x0400911F RID: 37151
			EQ,
			// Token: 0x04009120 RID: 37152
			E,
			// Token: 0x04009121 RID: 37153
			NotEquals,
			// Token: 0x04009122 RID: 37154
			NEQ,
			// Token: 0x04009123 RID: 37155
			NE,
			// Token: 0x04009124 RID: 37156
			Less,
			// Token: 0x04009125 RID: 37157
			LessThan,
			// Token: 0x04009126 RID: 37158
			LT,
			// Token: 0x04009127 RID: 37159
			Greater,
			// Token: 0x04009128 RID: 37160
			GreaterThan,
			// Token: 0x04009129 RID: 37161
			GT,
			// Token: 0x0400912A RID: 37162
			LessOrEqual,
			// Token: 0x0400912B RID: 37163
			LessThanOrEqualTo,
			// Token: 0x0400912C RID: 37164
			LTE,
			// Token: 0x0400912D RID: 37165
			GreaterOrEqual,
			// Token: 0x0400912E RID: 37166
			GreaterThanOrEqualTo,
			// Token: 0x0400912F RID: 37167
			GTE
		}
	}
}
