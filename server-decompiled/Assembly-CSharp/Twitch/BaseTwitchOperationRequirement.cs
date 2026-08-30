using System;
using UnityEngine.Scripting;

namespace Twitch
{
	// Token: 0x02001830 RID: 6192
	[Preserve]
	public class BaseTwitchOperationRequirement : BaseTwitchRequirement
	{
		// Token: 0x0600BEFF RID: 48895 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x1700172A RID: 5930
		// (get) Token: 0x0600BF00 RID: 48896 RVA: 0x0046E7B7 File Offset: 0x0046C9B7
		// (set) Token: 0x0600BF01 RID: 48897 RVA: 0x0046E7C2 File Offset: 0x0046C9C2
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

		// Token: 0x0600BF02 RID: 48898 RVA: 0x0046E7D4 File Offset: 0x0046C9D4
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

		// Token: 0x0600BF03 RID: 48899 RVA: 0x0046E864 File Offset: 0x0046CA64
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

		// Token: 0x0600BF04 RID: 48900 RVA: 0x0046E900 File Offset: 0x0046CB00
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

		// Token: 0x0600BF05 RID: 48901 RVA: 0x0046E94A File Offset: 0x0046CB4A
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual object LeftSide(Entity target)
		{
			return 0;
		}

		// Token: 0x0600BF06 RID: 48902 RVA: 0x0046E94A File Offset: 0x0046CB4A
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual object RightSide(Entity target)
		{
			return 0;
		}

		// Token: 0x0600BF07 RID: 48903 RVA: 0x0046E952 File Offset: 0x0046CB52
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseEnum<BaseTwitchOperationRequirement.OperationTypes>(BaseTwitchOperationRequirement.PropOperation, ref this.operation);
		}

		// Token: 0x04008FFD RID: 36861
		[PublicizedFrom(EAccessModifier.Protected)]
		public BaseTwitchOperationRequirement.OperationTypes operation = BaseTwitchOperationRequirement.OperationTypes.Equals;

		// Token: 0x04008FFE RID: 36862
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropOperation = "operation";

		// Token: 0x04008FFF RID: 36863
		[PublicizedFrom(EAccessModifier.Private)]
		public StringComparison stringComparison;

		// Token: 0x02001831 RID: 6193
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum OperationTypes
		{
			// Token: 0x04009001 RID: 36865
			None,
			// Token: 0x04009002 RID: 36866
			Equals,
			// Token: 0x04009003 RID: 36867
			EQ,
			// Token: 0x04009004 RID: 36868
			E,
			// Token: 0x04009005 RID: 36869
			NotEquals,
			// Token: 0x04009006 RID: 36870
			NEQ,
			// Token: 0x04009007 RID: 36871
			NE,
			// Token: 0x04009008 RID: 36872
			Less,
			// Token: 0x04009009 RID: 36873
			LessThan,
			// Token: 0x0400900A RID: 36874
			LT,
			// Token: 0x0400900B RID: 36875
			Greater,
			// Token: 0x0400900C RID: 36876
			GreaterThan,
			// Token: 0x0400900D RID: 36877
			GT,
			// Token: 0x0400900E RID: 36878
			LessOrEqual,
			// Token: 0x0400900F RID: 36879
			LessThanOrEqualTo,
			// Token: 0x04009010 RID: 36880
			LTE,
			// Token: 0x04009011 RID: 36881
			GreaterOrEqual,
			// Token: 0x04009012 RID: 36882
			GreaterThanOrEqualTo,
			// Token: 0x04009013 RID: 36883
			GTE
		}
	}
}
