using System;

namespace SandboxOptions
{
	// Token: 0x020018F2 RID: 6386
	public class SandboxOptionFloat : BaseSandboxOption
	{
		// Token: 0x170018C7 RID: 6343
		// (get) Token: 0x0600C485 RID: 50309 RVA: 0x0002F260 File Offset: 0x0002D460
		public override BaseSandboxOption.OptionTypes OptionType
		{
			get
			{
				return BaseSandboxOption.OptionTypes.Float;
			}
		}

		// Token: 0x0600C486 RID: 50310 RVA: 0x00482888 File Offset: 0x00480A88
		public SandboxOptionFloat(SandboxOptions option, string optionName, string categoryName, string valueSetName, float defaultValue, bool newUISection = false, SandboxOptionFloat.DisabledOptionsOnValue disabledOptions = null)
			: base(option, optionName, categoryName, valueSetName, newUISection)
		{
			this.DefaultValue = defaultValue;
			this.DisabledOptions = disabledOptions;
		}

		// Token: 0x0600C487 RID: 50311 RVA: 0x004828B2 File Offset: 0x00480AB2
		public override float GetFloatValue()
		{
			return this.CurrentValue;
		}

		// Token: 0x0600C488 RID: 50312 RVA: 0x004828BA File Offset: 0x00480ABA
		public override int GetIntValue()
		{
			return (int)(this.CurrentValue * 100f);
		}

		// Token: 0x0600C489 RID: 50313 RVA: 0x004828C9 File Offset: 0x00480AC9
		public override object GetValue()
		{
			return this.GetFloatValue();
		}

		// Token: 0x170018C8 RID: 6344
		// (get) Token: 0x0600C48A RID: 50314 RVA: 0x004828D6 File Offset: 0x00480AD6
		public override string ValueText
		{
			get
			{
				return this.CurrentValue.ToString();
			}
		}

		// Token: 0x0600C48B RID: 50315 RVA: 0x004828E4 File Offset: 0x00480AE4
		public override void SetValue(string value)
		{
			float num;
			if (StringParsers.TryParseFloat(value, out num) && base.ValueOptions.GetFloatIndex(num) != -1)
			{
				this.CurrentValue = num;
			}
		}

		// Token: 0x0600C48C RID: 50316 RVA: 0x00482911 File Offset: 0x00480B11
		public override void SetFloat(float value)
		{
			this.CurrentValue = value;
		}

		// Token: 0x0600C48D RID: 50317 RVA: 0x0048291A File Offset: 0x00480B1A
		public override bool IsChanged()
		{
			return this.CurrentValue != this.DefaultValue;
		}

		// Token: 0x0600C48E RID: 50318 RVA: 0x0048292D File Offset: 0x00480B2D
		public override object GetDefaultValue()
		{
			return this.DefaultValue;
		}

		// Token: 0x0600C48F RID: 50319 RVA: 0x0048293A File Offset: 0x00480B3A
		public override float GetDefaultFloatValue()
		{
			return this.DefaultValue;
		}

		// Token: 0x0600C490 RID: 50320 RVA: 0x00482942 File Offset: 0x00480B42
		public override int GetDefaultIntValue()
		{
			return (int)(this.DefaultValue * 100f);
		}

		// Token: 0x0600C491 RID: 50321 RVA: 0x00482951 File Offset: 0x00480B51
		public override int GetDefaultIndex()
		{
			return base.GetValueSet().GetFloatIndex(this.DefaultValue);
		}

		// Token: 0x0600C492 RID: 50322 RVA: 0x00482964 File Offset: 0x00480B64
		public override void SetToDefault()
		{
			this.CurrentValue = this.DefaultValue;
		}

		// Token: 0x0600C493 RID: 50323 RVA: 0x00482972 File Offset: 0x00480B72
		public override string GetValueText()
		{
			return string.Format("{0} | default:{1}", this.CurrentValue, this.DefaultValue);
		}

		// Token: 0x0600C494 RID: 50324 RVA: 0x00482994 File Offset: 0x00480B94
		public override int GetValueIndex()
		{
			return base.GetValueSet().GetFloatIndex(this.CurrentValue);
		}

		// Token: 0x0600C495 RID: 50325 RVA: 0x004829A8 File Offset: 0x00480BA8
		public override int GetValueIndex(string value)
		{
			float num;
			if (StringParsers.TryParseFloat(value, out num))
			{
				return base.GetValueSet().GetFloatIndex(num);
			}
			return base.GetValueSet().GetFloatIndex(this.DefaultValue);
		}

		// Token: 0x0600C496 RID: 50326 RVA: 0x004829DD File Offset: 0x00480BDD
		public override void SetValueFromIndex(int index)
		{
			if (!base.GetValueSet().GetFloatValue(index, out this.CurrentValue))
			{
				this.CurrentValue = this.DefaultValue;
			}
		}

		// Token: 0x0600C497 RID: 50327 RVA: 0x004829FF File Offset: 0x00480BFF
		public override SandboxOptions[] GetAlwaysShowOptions()
		{
			if (this.DisabledOptions == null || !this.DisabledOptions.AlwaysShowValuesOnEnabled)
			{
				return null;
			}
			return this.DisabledOptions.DisabledOptions;
		}

		// Token: 0x0400947E RID: 38014
		public float DefaultValue;

		// Token: 0x0400947F RID: 38015
		public float CurrentValue = 1f;

		// Token: 0x04009480 RID: 38016
		public bool IsPercent;

		// Token: 0x04009481 RID: 38017
		public SandboxOptionFloat.DisabledOptionsOnValue DisabledOptions;

		// Token: 0x020018F3 RID: 6387
		public class DisabledOptionsOnValue
		{
			// Token: 0x0600C498 RID: 50328 RVA: 0x00482A23 File Offset: 0x00480C23
			public DisabledOptionsOnValue(SandboxOptions[] options, float value, bool inverted = false, bool alwaysShowValuesOnEnabled = false)
			{
				this.DisabledOptions = options;
				this.Value = value;
				this.Inverted = inverted;
				this.AlwaysShowValuesOnEnabled = alwaysShowValuesOnEnabled;
			}

			// Token: 0x04009482 RID: 38018
			public float Value;

			// Token: 0x04009483 RID: 38019
			public bool Inverted;

			// Token: 0x04009484 RID: 38020
			public SandboxOptions[] DisabledOptions;

			// Token: 0x04009485 RID: 38021
			public bool AlwaysShowValuesOnEnabled;
		}
	}
}
