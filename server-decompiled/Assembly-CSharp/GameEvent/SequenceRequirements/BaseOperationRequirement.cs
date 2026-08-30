using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001921 RID: 6433
	[Preserve]
	public class BaseOperationRequirement : BaseRequirement
	{
		// Token: 0x0600C6A9 RID: 50857 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x1700189A RID: 6298
		// (get) Token: 0x0600C6AA RID: 50858 RVA: 0x004906CC File Offset: 0x0048E8CC
		// (set) Token: 0x0600C6AB RID: 50859 RVA: 0x004906D7 File Offset: 0x0048E8D7
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

		// Token: 0x0600C6AC RID: 50860 RVA: 0x004906E8 File Offset: 0x0048E8E8
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual bool compare(float valueA, float valueB)
		{
			switch (this.operation)
			{
			case BaseOperationRequirement.OperationTypes.Equals:
			case BaseOperationRequirement.OperationTypes.EQ:
			case BaseOperationRequirement.OperationTypes.E:
				return valueA == valueB;
			case BaseOperationRequirement.OperationTypes.NotEquals:
			case BaseOperationRequirement.OperationTypes.NEQ:
			case BaseOperationRequirement.OperationTypes.NE:
				return valueA != valueB;
			case BaseOperationRequirement.OperationTypes.Less:
			case BaseOperationRequirement.OperationTypes.LessThan:
			case BaseOperationRequirement.OperationTypes.LT:
				return valueA < valueB;
			case BaseOperationRequirement.OperationTypes.Greater:
			case BaseOperationRequirement.OperationTypes.GreaterThan:
			case BaseOperationRequirement.OperationTypes.GT:
				return valueA > valueB;
			case BaseOperationRequirement.OperationTypes.LessOrEqual:
			case BaseOperationRequirement.OperationTypes.LessThanOrEqualTo:
			case BaseOperationRequirement.OperationTypes.LTE:
				return valueA <= valueB;
			case BaseOperationRequirement.OperationTypes.GreaterOrEqual:
			case BaseOperationRequirement.OperationTypes.GreaterThanOrEqualTo:
			case BaseOperationRequirement.OperationTypes.GTE:
				return valueA >= valueB;
			default:
				return true;
			}
		}

		// Token: 0x0600C6AD RID: 50861 RVA: 0x00490778 File Offset: 0x0048E978
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual bool compare(string valueA, string valueB)
		{
			int num = string.Compare(valueA, valueB, this.stringComparison);
			switch (this.operation)
			{
			case BaseOperationRequirement.OperationTypes.Equals:
			case BaseOperationRequirement.OperationTypes.EQ:
			case BaseOperationRequirement.OperationTypes.E:
				return num == 0;
			case BaseOperationRequirement.OperationTypes.NotEquals:
			case BaseOperationRequirement.OperationTypes.NEQ:
			case BaseOperationRequirement.OperationTypes.NE:
				return num != 0;
			case BaseOperationRequirement.OperationTypes.Less:
			case BaseOperationRequirement.OperationTypes.LessThan:
			case BaseOperationRequirement.OperationTypes.LT:
				return num < 0;
			case BaseOperationRequirement.OperationTypes.Greater:
			case BaseOperationRequirement.OperationTypes.GreaterThan:
			case BaseOperationRequirement.OperationTypes.GT:
				return num > 0;
			case BaseOperationRequirement.OperationTypes.LessOrEqual:
			case BaseOperationRequirement.OperationTypes.LessThanOrEqualTo:
			case BaseOperationRequirement.OperationTypes.LTE:
				return num <= 0;
			case BaseOperationRequirement.OperationTypes.GreaterOrEqual:
			case BaseOperationRequirement.OperationTypes.GreaterThanOrEqualTo:
			case BaseOperationRequirement.OperationTypes.GTE:
				return num <= 0;
			default:
				return true;
			}
		}

		// Token: 0x0600C6AE RID: 50862 RVA: 0x00490814 File Offset: 0x0048EA14
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

		// Token: 0x0600C6AF RID: 50863 RVA: 0x004746E6 File Offset: 0x004728E6
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual object LeftSide(Entity target)
		{
			return 0;
		}

		// Token: 0x0600C6B0 RID: 50864 RVA: 0x004746E6 File Offset: 0x004728E6
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual object RightSide(Entity target)
		{
			return 0;
		}

		// Token: 0x0600C6B1 RID: 50865 RVA: 0x0049085E File Offset: 0x0048EA5E
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseEnum<BaseOperationRequirement.OperationTypes>(BaseOperationRequirement.PropOperation, ref this.operation);
		}

		// Token: 0x0600C6B2 RID: 50866 RVA: 0x00490878 File Offset: 0x0048EA78
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseRequirement CloneChildSettings()
		{
			return new BaseOperationRequirement
			{
				Invert = this.Invert,
				operation = this.operation
			};
		}

		// Token: 0x040095B2 RID: 38322
		[PublicizedFrom(EAccessModifier.Protected)]
		public BaseOperationRequirement.OperationTypes operation = BaseOperationRequirement.OperationTypes.Equals;

		// Token: 0x040095B3 RID: 38323
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropOperation = "operation";

		// Token: 0x040095B4 RID: 38324
		[PublicizedFrom(EAccessModifier.Private)]
		public StringComparison stringComparison;

		// Token: 0x02001922 RID: 6434
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum OperationTypes
		{
			// Token: 0x040095B6 RID: 38326
			None,
			// Token: 0x040095B7 RID: 38327
			Equals,
			// Token: 0x040095B8 RID: 38328
			EQ,
			// Token: 0x040095B9 RID: 38329
			E,
			// Token: 0x040095BA RID: 38330
			NotEquals,
			// Token: 0x040095BB RID: 38331
			NEQ,
			// Token: 0x040095BC RID: 38332
			NE,
			// Token: 0x040095BD RID: 38333
			Less,
			// Token: 0x040095BE RID: 38334
			LessThan,
			// Token: 0x040095BF RID: 38335
			LT,
			// Token: 0x040095C0 RID: 38336
			Greater,
			// Token: 0x040095C1 RID: 38337
			GreaterThan,
			// Token: 0x040095C2 RID: 38338
			GT,
			// Token: 0x040095C3 RID: 38339
			LessOrEqual,
			// Token: 0x040095C4 RID: 38340
			LessThanOrEqualTo,
			// Token: 0x040095C5 RID: 38341
			LTE,
			// Token: 0x040095C6 RID: 38342
			GreaterOrEqual,
			// Token: 0x040095C7 RID: 38343
			GreaterThanOrEqualTo,
			// Token: 0x040095C8 RID: 38344
			GTE
		}
	}
}
