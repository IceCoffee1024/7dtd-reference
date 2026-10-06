using System;
using UnityEngine.Scripting;

namespace Twitch
{
	// Token: 0x02001882 RID: 6274
	[Preserve]
	public class BaseTwitchOperationRequirement : BaseTwitchRequirement
	{
		// Token: 0x0600C1DC RID: 49628 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x1700183E RID: 6206
		// (get) Token: 0x0600C1DD RID: 49629 RVA: 0x00478A8F File Offset: 0x00476C8F
		// (set) Token: 0x0600C1DE RID: 49630 RVA: 0x00478A9A File Offset: 0x00476C9A
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

		// Token: 0x0600C1DF RID: 49631 RVA: 0x00478AAC File Offset: 0x00476CAC
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

		// Token: 0x0600C1E0 RID: 49632 RVA: 0x00478B3C File Offset: 0x00476D3C
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

		// Token: 0x0600C1E1 RID: 49633 RVA: 0x00478BD8 File Offset: 0x00476DD8
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

		// Token: 0x0600C1E2 RID: 49634 RVA: 0x00478C22 File Offset: 0x00476E22
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual object LeftSide(Entity target)
		{
			return 0;
		}

		// Token: 0x0600C1E3 RID: 49635 RVA: 0x00478C22 File Offset: 0x00476E22
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual object RightSide(Entity target)
		{
			return 0;
		}

		// Token: 0x0600C1E4 RID: 49636 RVA: 0x00478C2A File Offset: 0x00476E2A
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseEnum<BaseTwitchOperationRequirement.OperationTypes>(BaseTwitchOperationRequirement.PropOperation, ref this.operation);
		}

		// Token: 0x040091DF RID: 37343
		[PublicizedFrom(EAccessModifier.Protected)]
		public BaseTwitchOperationRequirement.OperationTypes operation = BaseTwitchOperationRequirement.OperationTypes.Equals;

		// Token: 0x040091E0 RID: 37344
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropOperation = "operation";

		// Token: 0x040091E1 RID: 37345
		[PublicizedFrom(EAccessModifier.Private)]
		public StringComparison stringComparison;

		// Token: 0x02001883 RID: 6275
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum OperationTypes
		{
			// Token: 0x040091E3 RID: 37347
			None,
			// Token: 0x040091E4 RID: 37348
			Equals,
			// Token: 0x040091E5 RID: 37349
			EQ,
			// Token: 0x040091E6 RID: 37350
			E,
			// Token: 0x040091E7 RID: 37351
			NotEquals,
			// Token: 0x040091E8 RID: 37352
			NEQ,
			// Token: 0x040091E9 RID: 37353
			NE,
			// Token: 0x040091EA RID: 37354
			Less,
			// Token: 0x040091EB RID: 37355
			LessThan,
			// Token: 0x040091EC RID: 37356
			LT,
			// Token: 0x040091ED RID: 37357
			Greater,
			// Token: 0x040091EE RID: 37358
			GreaterThan,
			// Token: 0x040091EF RID: 37359
			GT,
			// Token: 0x040091F0 RID: 37360
			LessOrEqual,
			// Token: 0x040091F1 RID: 37361
			LessThanOrEqualTo,
			// Token: 0x040091F2 RID: 37362
			LTE,
			// Token: 0x040091F3 RID: 37363
			GreaterOrEqual,
			// Token: 0x040091F4 RID: 37364
			GreaterThanOrEqualTo,
			// Token: 0x040091F5 RID: 37365
			GTE
		}
	}
}
