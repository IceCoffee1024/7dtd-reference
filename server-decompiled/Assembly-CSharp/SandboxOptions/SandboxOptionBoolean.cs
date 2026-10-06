using System;

namespace SandboxOptions
{
	// Token: 0x020018BF RID: 6335
	public class SandboxOptionBoolean : BaseSandboxOption
	{
		// Token: 0x1700187D RID: 6269
		// (get) Token: 0x0600C35F RID: 50015 RVA: 0x00080804 File Offset: 0x0007EA04
		public override BaseSandboxOption.OptionTypes OptionType
		{
			get
			{
				return BaseSandboxOption.OptionTypes.Bool;
			}
		}

		// Token: 0x0600C360 RID: 50016 RVA: 0x0047ED3C File Offset: 0x0047CF3C
		public SandboxOptionBoolean(SandboxOptions option, string optionName, string categoryName, string valueSetName, bool defaultValue, bool newUISection = false, SandboxOptionBoolean.DisabledOptionsOnValue disabledOptions = null)
			: base(option, optionName, categoryName, valueSetName, newUISection)
		{
			this.DefaultValue = defaultValue;
			this.CurrentValue = defaultValue;
			this.DisabledOptions = disabledOptions;
		}

		// Token: 0x0600C361 RID: 50017 RVA: 0x0047ED6F File Offset: 0x0047CF6F
		public override bool GetBoolValue()
		{
			return this.CurrentValue;
		}

		// Token: 0x0600C362 RID: 50018 RVA: 0x0047ED77 File Offset: 0x0047CF77
		public override object GetValue()
		{
			return this.GetBoolValue();
		}

		// Token: 0x1700187E RID: 6270
		// (get) Token: 0x0600C363 RID: 50019 RVA: 0x0047ED84 File Offset: 0x0047CF84
		public override string ValueText
		{
			get
			{
				return this.CurrentValue.ToString();
			}
		}

		// Token: 0x0600C364 RID: 50020 RVA: 0x0047ED94 File Offset: 0x0047CF94
		public override void SetValue(string value)
		{
			bool flag;
			if (StringParsers.TryParseBool(value, out flag))
			{
				this.CurrentValue = flag;
			}
		}

		// Token: 0x0600C365 RID: 50021 RVA: 0x0047EDB2 File Offset: 0x0047CFB2
		public override void SetBool(bool value)
		{
			this.CurrentValue = value;
		}

		// Token: 0x0600C366 RID: 50022 RVA: 0x0047EDBB File Offset: 0x0047CFBB
		public override bool IsChanged()
		{
			return this.CurrentValue != this.DefaultValue;
		}

		// Token: 0x0600C367 RID: 50023 RVA: 0x0047EDCE File Offset: 0x0047CFCE
		public override object GetDefaultValue()
		{
			return this.DefaultValue;
		}

		// Token: 0x0600C368 RID: 50024 RVA: 0x0047EDDB File Offset: 0x0047CFDB
		public override bool GetDefaultBoolValue()
		{
			return this.DefaultValue;
		}

		// Token: 0x0600C369 RID: 50025 RVA: 0x0047EDE3 File Offset: 0x0047CFE3
		public override int GetDefaultIndex()
		{
			return base.GetValueSet().GetBoolIndex(this.DefaultValue);
		}

		// Token: 0x0600C36A RID: 50026 RVA: 0x0047EDF6 File Offset: 0x0047CFF6
		public override void SetToDefault()
		{
			this.CurrentValue = this.DefaultValue;
		}

		// Token: 0x0600C36B RID: 50027 RVA: 0x0047EE04 File Offset: 0x0047D004
		public override string GetValueText()
		{
			return string.Format("{0} | default:{1}", this.CurrentValue, this.DefaultValue);
		}

		// Token: 0x0600C36C RID: 50028 RVA: 0x0047EE26 File Offset: 0x0047D026
		public override int GetValueIndex()
		{
			return base.GetValueSet().GetBoolIndex(this.CurrentValue);
		}

		// Token: 0x0600C36D RID: 50029 RVA: 0x0047EE3C File Offset: 0x0047D03C
		public override int GetValueIndex(string value)
		{
			bool flag;
			if (StringParsers.TryParseBool(value, out flag))
			{
				return base.GetValueSet().GetBoolIndex(flag);
			}
			return base.GetValueSet().GetBoolIndex(this.DefaultValue);
		}

		// Token: 0x0600C36E RID: 50030 RVA: 0x0047EE71 File Offset: 0x0047D071
		public override void SetValueFromIndex(int index)
		{
			if (!base.GetValueSet().GetBoolValue(index, out this.CurrentValue))
			{
				this.CurrentValue = this.DefaultValue;
			}
		}

		// Token: 0x0600C36F RID: 50031 RVA: 0x0047EE93 File Offset: 0x0047D093
		public override SandboxOptions[] GetAlwaysShowOptions()
		{
			if (this.DisabledOptions == null || !this.DisabledOptions.AlwaysShowValuesOnEnabled)
			{
				return null;
			}
			return this.DisabledOptions.DisabledOptions;
		}

		// Token: 0x0400938E RID: 37774
		public bool DefaultValue;

		// Token: 0x0400938F RID: 37775
		public bool CurrentValue;

		// Token: 0x04009390 RID: 37776
		public SandboxOptionBoolean.DisabledOptionsOnValue DisabledOptions;

		// Token: 0x020018C0 RID: 6336
		public class DisabledOptionsOnValue
		{
			// Token: 0x0600C370 RID: 50032 RVA: 0x0047EEB7 File Offset: 0x0047D0B7
			public DisabledOptionsOnValue(SandboxOptions[] options, bool value, bool inverted = false, bool alwaysShowValuesOnEnabled = false)
			{
				this.DisabledOptions = options;
				this.Value = value;
				this.Inverted = inverted;
				this.AlwaysShowValuesOnEnabled = alwaysShowValuesOnEnabled;
			}

			// Token: 0x04009391 RID: 37777
			public bool Value;

			// Token: 0x04009392 RID: 37778
			public bool Inverted;

			// Token: 0x04009393 RID: 37779
			public SandboxOptions[] DisabledOptions;

			// Token: 0x04009394 RID: 37780
			public bool AlwaysShowValuesOnEnabled;
		}
	}
}
