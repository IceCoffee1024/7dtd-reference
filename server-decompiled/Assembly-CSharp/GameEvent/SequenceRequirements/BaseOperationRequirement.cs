using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x0200194D RID: 6477
	[Preserve]
	public class BaseOperationRequirement : BaseRequirement
	{
		// Token: 0x0600C801 RID: 51201 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x1700193C RID: 6460
		// (get) Token: 0x0600C802 RID: 51202 RVA: 0x00494BB8 File Offset: 0x00492DB8
		// (set) Token: 0x0600C803 RID: 51203 RVA: 0x00494BC3 File Offset: 0x00492DC3
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

		// Token: 0x0600C804 RID: 51204 RVA: 0x00494BD4 File Offset: 0x00492DD4
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

		// Token: 0x0600C805 RID: 51205 RVA: 0x00494C64 File Offset: 0x00492E64
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

		// Token: 0x0600C806 RID: 51206 RVA: 0x00494D00 File Offset: 0x00492F00
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

		// Token: 0x0600C807 RID: 51207 RVA: 0x00478C22 File Offset: 0x00476E22
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual object LeftSide(Entity target)
		{
			return 0;
		}

		// Token: 0x0600C808 RID: 51208 RVA: 0x00478C22 File Offset: 0x00476E22
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual object RightSide(Entity target)
		{
			return 0;
		}

		// Token: 0x0600C809 RID: 51209 RVA: 0x00494D4A File Offset: 0x00492F4A
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseEnum<BaseOperationRequirement.OperationTypes>(BaseOperationRequirement.PropOperation, ref this.operation);
		}

		// Token: 0x0600C80A RID: 51210 RVA: 0x00494D64 File Offset: 0x00492F64
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseRequirement CloneChildSettings()
		{
			return new BaseOperationRequirement
			{
				Invert = this.Invert,
				operation = this.operation
			};
		}

		// Token: 0x04009678 RID: 38520
		[PublicizedFrom(EAccessModifier.Protected)]
		public BaseOperationRequirement.OperationTypes operation = BaseOperationRequirement.OperationTypes.Equals;

		// Token: 0x04009679 RID: 38521
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropOperation = "operation";

		// Token: 0x0400967A RID: 38522
		[PublicizedFrom(EAccessModifier.Private)]
		public StringComparison stringComparison;

		// Token: 0x0200194E RID: 6478
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum OperationTypes
		{
			// Token: 0x0400967C RID: 38524
			None,
			// Token: 0x0400967D RID: 38525
			Equals,
			// Token: 0x0400967E RID: 38526
			EQ,
			// Token: 0x0400967F RID: 38527
			E,
			// Token: 0x04009680 RID: 38528
			NotEquals,
			// Token: 0x04009681 RID: 38529
			NEQ,
			// Token: 0x04009682 RID: 38530
			NE,
			// Token: 0x04009683 RID: 38531
			Less,
			// Token: 0x04009684 RID: 38532
			LessThan,
			// Token: 0x04009685 RID: 38533
			LT,
			// Token: 0x04009686 RID: 38534
			Greater,
			// Token: 0x04009687 RID: 38535
			GreaterThan,
			// Token: 0x04009688 RID: 38536
			GT,
			// Token: 0x04009689 RID: 38537
			LessOrEqual,
			// Token: 0x0400968A RID: 38538
			LessThanOrEqualTo,
			// Token: 0x0400968B RID: 38539
			LTE,
			// Token: 0x0400968C RID: 38540
			GreaterOrEqual,
			// Token: 0x0400968D RID: 38541
			GreaterThanOrEqualTo,
			// Token: 0x0400968E RID: 38542
			GTE
		}
	}
}
