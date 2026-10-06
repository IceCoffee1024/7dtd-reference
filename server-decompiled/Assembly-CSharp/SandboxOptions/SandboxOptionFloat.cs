using System;

namespace SandboxOptions
{
	// Token: 0x020018C1 RID: 6337
	public class SandboxOptionFloat : BaseSandboxOption
	{
		// Token: 0x1700187F RID: 6271
		// (get) Token: 0x0600C371 RID: 50033 RVA: 0x0002F260 File Offset: 0x0002D460
		public override BaseSandboxOption.OptionTypes OptionType
		{
			get
			{
				return BaseSandboxOption.OptionTypes.Float;
			}
		}

		// Token: 0x0600C372 RID: 50034 RVA: 0x0047EEDC File Offset: 0x0047D0DC
		public SandboxOptionFloat(SandboxOptions option, string optionName, string categoryName, string valueSetName, float defaultValue, bool newUISection = false, SandboxOptionFloat.DisabledOptionsOnValue disabledOptions = null)
			: base(option, optionName, categoryName, valueSetName, newUISection)
		{
			this.DefaultValue = defaultValue;
			this.DisabledOptions = disabledOptions;
		}

		// Token: 0x0600C373 RID: 50035 RVA: 0x0047EF06 File Offset: 0x0047D106
		public override float GetFloatValue()
		{
			return this.CurrentValue;
		}

		// Token: 0x0600C374 RID: 50036 RVA: 0x0047EF0E File Offset: 0x0047D10E
		public override int GetIntValue()
		{
			return (int)(this.CurrentValue * 100f);
		}

		// Token: 0x0600C375 RID: 50037 RVA: 0x0047EF1D File Offset: 0x0047D11D
		public override object GetValue()
		{
			return this.GetFloatValue();
		}

		// Token: 0x17001880 RID: 6272
		// (get) Token: 0x0600C376 RID: 50038 RVA: 0x0047EF2A File Offset: 0x0047D12A
		public override string ValueText
		{
			get
			{
				return this.CurrentValue.ToString();
			}
		}

		// Token: 0x0600C377 RID: 50039 RVA: 0x0047EF38 File Offset: 0x0047D138
		public override void SetValue(string value)
		{
			float num;
			if (StringParsers.TryParseFloat(value, out num) && base.ValueOptions.GetFloatIndex(num) != -1)
			{
				this.CurrentValue = num;
			}
		}

		// Token: 0x0600C378 RID: 50040 RVA: 0x0047EF65 File Offset: 0x0047D165
		public override void SetFloat(float value)
		{
			this.CurrentValue = value;
		}

		// Token: 0x0600C379 RID: 50041 RVA: 0x0047EF6E File Offset: 0x0047D16E
		public override bool IsChanged()
		{
			return this.CurrentValue != this.DefaultValue;
		}

		// Token: 0x0600C37A RID: 50042 RVA: 0x0047EF81 File Offset: 0x0047D181
		public override object GetDefaultValue()
		{
			return this.DefaultValue;
		}

		// Token: 0x0600C37B RID: 50043 RVA: 0x0047EF8E File Offset: 0x0047D18E
		public override float GetDefaultFloatValue()
		{
			return this.DefaultValue;
		}

		// Token: 0x0600C37C RID: 50044 RVA: 0x0047EF96 File Offset: 0x0047D196
		public override int GetDefaultIntValue()
		{
			return (int)(this.DefaultValue * 100f);
		}

		// Token: 0x0600C37D RID: 50045 RVA: 0x0047EFA5 File Offset: 0x0047D1A5
		public override int GetDefaultIndex()
		{
			return base.GetValueSet().GetFloatIndex(this.DefaultValue);
		}

		// Token: 0x0600C37E RID: 50046 RVA: 0x0047EFB8 File Offset: 0x0047D1B8
		public override void SetToDefault()
		{
			this.CurrentValue = this.DefaultValue;
		}

		// Token: 0x0600C37F RID: 50047 RVA: 0x0047EFC6 File Offset: 0x0047D1C6
		public override string GetValueText()
		{
			return string.Format("{0} | default:{1}", this.CurrentValue, this.DefaultValue);
		}

		// Token: 0x0600C380 RID: 50048 RVA: 0x0047EFE8 File Offset: 0x0047D1E8
		public override int GetValueIndex()
		{
			return base.GetValueSet().GetFloatIndex(this.CurrentValue);
		}

		// Token: 0x0600C381 RID: 50049 RVA: 0x0047EFFC File Offset: 0x0047D1FC
		public override int GetValueIndex(string value)
		{
			float num;
			if (StringParsers.TryParseFloat(value, out num))
			{
				return base.GetValueSet().GetFloatIndex(num);
			}
			return base.GetValueSet().GetFloatIndex(this.DefaultValue);
		}

		// Token: 0x0600C382 RID: 50050 RVA: 0x0047F031 File Offset: 0x0047D231
		public override void SetValueFromIndex(int index)
		{
			if (!base.GetValueSet().GetFloatValue(index, out this.CurrentValue))
			{
				this.CurrentValue = this.DefaultValue;
			}
		}

		// Token: 0x0600C383 RID: 50051 RVA: 0x0047F053 File Offset: 0x0047D253
		public override SandboxOptions[] GetAlwaysShowOptions()
		{
			if (this.DisabledOptions == null || !this.DisabledOptions.AlwaysShowValuesOnEnabled)
			{
				return null;
			}
			return this.DisabledOptions.DisabledOptions;
		}

		// Token: 0x04009395 RID: 37781
		public float DefaultValue;

		// Token: 0x04009396 RID: 37782
		public float CurrentValue = 1f;

		// Token: 0x04009397 RID: 37783
		public bool IsPercent;

		// Token: 0x04009398 RID: 37784
		public SandboxOptionFloat.DisabledOptionsOnValue DisabledOptions;

		// Token: 0x020018C2 RID: 6338
		public class DisabledOptionsOnValue
		{
			// Token: 0x0600C384 RID: 50052 RVA: 0x0047F077 File Offset: 0x0047D277
			public DisabledOptionsOnValue(SandboxOptions[] options, float value, bool inverted = false, bool alwaysShowValuesOnEnabled = false)
			{
				this.DisabledOptions = options;
				this.Value = value;
				this.Inverted = inverted;
				this.AlwaysShowValuesOnEnabled = alwaysShowValuesOnEnabled;
			}

			// Token: 0x04009399 RID: 37785
			public float Value;

			// Token: 0x0400939A RID: 37786
			public bool Inverted;

			// Token: 0x0400939B RID: 37787
			public SandboxOptions[] DisabledOptions;

			// Token: 0x0400939C RID: 37788
			public bool AlwaysShowValuesOnEnabled;
		}
	}
}
