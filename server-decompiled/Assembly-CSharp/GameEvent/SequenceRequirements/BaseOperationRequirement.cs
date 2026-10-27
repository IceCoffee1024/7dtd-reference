using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x0200197E RID: 6526
	[Preserve]
	public class BaseOperationRequirement : BaseRequirement
	{
		// Token: 0x0600C915 RID: 51477 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x17001984 RID: 6532
		// (get) Token: 0x0600C916 RID: 51478 RVA: 0x004985AC File Offset: 0x004967AC
		// (set) Token: 0x0600C917 RID: 51479 RVA: 0x004985B7 File Offset: 0x004967B7
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

		// Token: 0x0600C918 RID: 51480 RVA: 0x004985C8 File Offset: 0x004967C8
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

		// Token: 0x0600C919 RID: 51481 RVA: 0x00498658 File Offset: 0x00496858
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

		// Token: 0x0600C91A RID: 51482 RVA: 0x004986F4 File Offset: 0x004968F4
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

		// Token: 0x0600C91B RID: 51483 RVA: 0x0047C5CE File Offset: 0x0047A7CE
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual object LeftSide(Entity target)
		{
			return 0;
		}

		// Token: 0x0600C91C RID: 51484 RVA: 0x0047C5CE File Offset: 0x0047A7CE
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual object RightSide(Entity target)
		{
			return 0;
		}

		// Token: 0x0600C91D RID: 51485 RVA: 0x0049873E File Offset: 0x0049693E
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseEnum<BaseOperationRequirement.OperationTypes>(BaseOperationRequirement.PropOperation, ref this.operation);
		}

		// Token: 0x0600C91E RID: 51486 RVA: 0x00498758 File Offset: 0x00496958
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseRequirement CloneChildSettings()
		{
			return new BaseOperationRequirement
			{
				Invert = this.Invert,
				operation = this.operation
			};
		}

		// Token: 0x04009761 RID: 38753
		[PublicizedFrom(EAccessModifier.Protected)]
		public BaseOperationRequirement.OperationTypes operation = BaseOperationRequirement.OperationTypes.Equals;

		// Token: 0x04009762 RID: 38754
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropOperation = "operation";

		// Token: 0x04009763 RID: 38755
		[PublicizedFrom(EAccessModifier.Private)]
		public StringComparison stringComparison;

		// Token: 0x0200197F RID: 6527
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum OperationTypes
		{
			// Token: 0x04009765 RID: 38757
			None,
			// Token: 0x04009766 RID: 38758
			Equals,
			// Token: 0x04009767 RID: 38759
			EQ,
			// Token: 0x04009768 RID: 38760
			E,
			// Token: 0x04009769 RID: 38761
			NotEquals,
			// Token: 0x0400976A RID: 38762
			NEQ,
			// Token: 0x0400976B RID: 38763
			NE,
			// Token: 0x0400976C RID: 38764
			Less,
			// Token: 0x0400976D RID: 38765
			LessThan,
			// Token: 0x0400976E RID: 38766
			LT,
			// Token: 0x0400976F RID: 38767
			Greater,
			// Token: 0x04009770 RID: 38768
			GreaterThan,
			// Token: 0x04009771 RID: 38769
			GT,
			// Token: 0x04009772 RID: 38770
			LessOrEqual,
			// Token: 0x04009773 RID: 38771
			LessThanOrEqualTo,
			// Token: 0x04009774 RID: 38772
			LTE,
			// Token: 0x04009775 RID: 38773
			GreaterOrEqual,
			// Token: 0x04009776 RID: 38774
			GreaterThanOrEqualTo,
			// Token: 0x04009777 RID: 38775
			GTE
		}
	}
}
