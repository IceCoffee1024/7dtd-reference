using System;

namespace SandboxOptions
{
	// Token: 0x020018F0 RID: 6384
	public class SandboxOptionBoolean : BaseSandboxOption
	{
		// Token: 0x170018C5 RID: 6341
		// (get) Token: 0x0600C473 RID: 50291 RVA: 0x000809D4 File Offset: 0x0007EBD4
		public override BaseSandboxOption.OptionTypes OptionType
		{
			get
			{
				return BaseSandboxOption.OptionTypes.Bool;
			}
		}

		// Token: 0x0600C474 RID: 50292 RVA: 0x004826E8 File Offset: 0x004808E8
		public SandboxOptionBoolean(SandboxOptions option, string optionName, string categoryName, string valueSetName, bool defaultValue, bool newUISection = false, SandboxOptionBoolean.DisabledOptionsOnValue disabledOptions = null)
			: base(option, optionName, categoryName, valueSetName, newUISection)
		{
			this.DefaultValue = defaultValue;
			this.CurrentValue = defaultValue;
			this.DisabledOptions = disabledOptions;
		}

		// Token: 0x0600C475 RID: 50293 RVA: 0x0048271B File Offset: 0x0048091B
		public override bool GetBoolValue()
		{
			return this.CurrentValue;
		}

		// Token: 0x0600C476 RID: 50294 RVA: 0x00482723 File Offset: 0x00480923
		public override object GetValue()
		{
			return this.GetBoolValue();
		}

		// Token: 0x170018C6 RID: 6342
		// (get) Token: 0x0600C477 RID: 50295 RVA: 0x00482730 File Offset: 0x00480930
		public override string ValueText
		{
			get
			{
				return this.CurrentValue.ToString();
			}
		}

		// Token: 0x0600C478 RID: 50296 RVA: 0x00482740 File Offset: 0x00480940
		public override void SetValue(string value)
		{
			bool flag;
			if (StringParsers.TryParseBool(value, out flag))
			{
				this.CurrentValue = flag;
			}
		}

		// Token: 0x0600C479 RID: 50297 RVA: 0x0048275E File Offset: 0x0048095E
		public override void SetBool(bool value)
		{
			this.CurrentValue = value;
		}

		// Token: 0x0600C47A RID: 50298 RVA: 0x00482767 File Offset: 0x00480967
		public override bool IsChanged()
		{
			return this.CurrentValue != this.DefaultValue;
		}

		// Token: 0x0600C47B RID: 50299 RVA: 0x0048277A File Offset: 0x0048097A
		public override object GetDefaultValue()
		{
			return this.DefaultValue;
		}

		// Token: 0x0600C47C RID: 50300 RVA: 0x00482787 File Offset: 0x00480987
		public override bool GetDefaultBoolValue()
		{
			return this.DefaultValue;
		}

		// Token: 0x0600C47D RID: 50301 RVA: 0x0048278F File Offset: 0x0048098F
		public override int GetDefaultIndex()
		{
			return base.GetValueSet().GetBoolIndex(this.DefaultValue);
		}

		// Token: 0x0600C47E RID: 50302 RVA: 0x004827A2 File Offset: 0x004809A2
		public override void SetToDefault()
		{
			this.CurrentValue = this.DefaultValue;
		}

		// Token: 0x0600C47F RID: 50303 RVA: 0x004827B0 File Offset: 0x004809B0
		public override string GetValueText()
		{
			return string.Format("{0} | default:{1}", this.CurrentValue, this.DefaultValue);
		}

		// Token: 0x0600C480 RID: 50304 RVA: 0x004827D2 File Offset: 0x004809D2
		public override int GetValueIndex()
		{
			return base.GetValueSet().GetBoolIndex(this.CurrentValue);
		}

		// Token: 0x0600C481 RID: 50305 RVA: 0x004827E8 File Offset: 0x004809E8
		public override int GetValueIndex(string value)
		{
			bool flag;
			if (StringParsers.TryParseBool(value, out flag))
			{
				return base.GetValueSet().GetBoolIndex(flag);
			}
			return base.GetValueSet().GetBoolIndex(this.DefaultValue);
		}

		// Token: 0x0600C482 RID: 50306 RVA: 0x0048281D File Offset: 0x00480A1D
		public override void SetValueFromIndex(int index)
		{
			if (!base.GetValueSet().GetBoolValue(index, out this.CurrentValue))
			{
				this.CurrentValue = this.DefaultValue;
			}
		}

		// Token: 0x0600C483 RID: 50307 RVA: 0x0048283F File Offset: 0x00480A3F
		public override SandboxOptions[] GetAlwaysShowOptions()
		{
			if (this.DisabledOptions == null || !this.DisabledOptions.AlwaysShowValuesOnEnabled)
			{
				return null;
			}
			return this.DisabledOptions.DisabledOptions;
		}

		// Token: 0x04009477 RID: 38007
		public bool DefaultValue;

		// Token: 0x04009478 RID: 38008
		public bool CurrentValue;

		// Token: 0x04009479 RID: 38009
		public SandboxOptionBoolean.DisabledOptionsOnValue DisabledOptions;

		// Token: 0x020018F1 RID: 6385
		public class DisabledOptionsOnValue
		{
			// Token: 0x0600C484 RID: 50308 RVA: 0x00482863 File Offset: 0x00480A63
			public DisabledOptionsOnValue(SandboxOptions[] options, bool value, bool inverted = false, bool alwaysShowValuesOnEnabled = false)
			{
				this.DisabledOptions = options;
				this.Value = value;
				this.Inverted = inverted;
				this.AlwaysShowValuesOnEnabled = alwaysShowValuesOnEnabled;
			}

			// Token: 0x0400947A RID: 38010
			public bool Value;

			// Token: 0x0400947B RID: 38011
			public bool Inverted;

			// Token: 0x0400947C RID: 38012
			public SandboxOptions[] DisabledOptions;

			// Token: 0x0400947D RID: 38013
			public bool AlwaysShowValuesOnEnabled;
		}
	}
}
