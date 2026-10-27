using System;
using UnityEngine.Scripting;

namespace Twitch
{
	// Token: 0x020018B3 RID: 6323
	[Preserve]
	public class BaseTwitchOperationRequirement : BaseTwitchRequirement
	{
		// Token: 0x0600C2F0 RID: 49904 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x17001886 RID: 6278
		// (get) Token: 0x0600C2F1 RID: 49905 RVA: 0x0047C43B File Offset: 0x0047A63B
		// (set) Token: 0x0600C2F2 RID: 49906 RVA: 0x0047C446 File Offset: 0x0047A646
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

		// Token: 0x0600C2F3 RID: 49907 RVA: 0x0047C458 File Offset: 0x0047A658
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

		// Token: 0x0600C2F4 RID: 49908 RVA: 0x0047C4E8 File Offset: 0x0047A6E8
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

		// Token: 0x0600C2F5 RID: 49909 RVA: 0x0047C584 File Offset: 0x0047A784
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

		// Token: 0x0600C2F6 RID: 49910 RVA: 0x0047C5CE File Offset: 0x0047A7CE
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual object LeftSide(Entity target)
		{
			return 0;
		}

		// Token: 0x0600C2F7 RID: 49911 RVA: 0x0047C5CE File Offset: 0x0047A7CE
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual object RightSide(Entity target)
		{
			return 0;
		}

		// Token: 0x0600C2F8 RID: 49912 RVA: 0x0047C5D6 File Offset: 0x0047A7D6
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseEnum<BaseTwitchOperationRequirement.OperationTypes>(BaseTwitchOperationRequirement.PropOperation, ref this.operation);
		}

		// Token: 0x040092C8 RID: 37576
		[PublicizedFrom(EAccessModifier.Protected)]
		public BaseTwitchOperationRequirement.OperationTypes operation = BaseTwitchOperationRequirement.OperationTypes.Equals;

		// Token: 0x040092C9 RID: 37577
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropOperation = "operation";

		// Token: 0x040092CA RID: 37578
		[PublicizedFrom(EAccessModifier.Private)]
		public StringComparison stringComparison;

		// Token: 0x020018B4 RID: 6324
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum OperationTypes
		{
			// Token: 0x040092CC RID: 37580
			None,
			// Token: 0x040092CD RID: 37581
			Equals,
			// Token: 0x040092CE RID: 37582
			EQ,
			// Token: 0x040092CF RID: 37583
			E,
			// Token: 0x040092D0 RID: 37584
			NotEquals,
			// Token: 0x040092D1 RID: 37585
			NEQ,
			// Token: 0x040092D2 RID: 37586
			NE,
			// Token: 0x040092D3 RID: 37587
			Less,
			// Token: 0x040092D4 RID: 37588
			LessThan,
			// Token: 0x040092D5 RID: 37589
			LT,
			// Token: 0x040092D6 RID: 37590
			Greater,
			// Token: 0x040092D7 RID: 37591
			GreaterThan,
			// Token: 0x040092D8 RID: 37592
			GT,
			// Token: 0x040092D9 RID: 37593
			LessOrEqual,
			// Token: 0x040092DA RID: 37594
			LessThanOrEqualTo,
			// Token: 0x040092DB RID: 37595
			LTE,
			// Token: 0x040092DC RID: 37596
			GreaterOrEqual,
			// Token: 0x040092DD RID: 37597
			GreaterThanOrEqualTo,
			// Token: 0x040092DE RID: 37598
			GTE
		}
	}
}
