using System;

namespace SandboxOptions
{
	// Token: 0x020018F4 RID: 6388
	public class SandboxOptionInt : BaseSandboxOption
	{
		// Token: 0x170018C9 RID: 6345
		// (get) Token: 0x0600C499 RID: 50329 RVA: 0x00020119 File Offset: 0x0001E319
		public override BaseSandboxOption.OptionTypes OptionType
		{
			get
			{
				return BaseSandboxOption.OptionTypes.Int;
			}
		}

		// Token: 0x0600C49A RID: 50330 RVA: 0x00482A48 File Offset: 0x00480C48
		public SandboxOptionInt(SandboxOptions option, string optionName, string categoryName, string valueSetName, int defaultValue, bool newUISection = false, SandboxOptionInt.DisabledOptionsOnValue disabledOptions = null)
			: base(option, optionName, categoryName, valueSetName, newUISection)
		{
			this.DefaultValue = defaultValue;
			this.DisabledOptions = disabledOptions;
		}

		// Token: 0x0600C49B RID: 50331 RVA: 0x00482A6E File Offset: 0x00480C6E
		public override int GetIntValue()
		{
			return this.CurrentValue;
		}

		// Token: 0x170018CA RID: 6346
		// (get) Token: 0x0600C49C RID: 50332 RVA: 0x00482A76 File Offset: 0x00480C76
		public override string ValueText
		{
			get
			{
				return this.CurrentValue.ToString();
			}
		}

		// Token: 0x0600C49D RID: 50333 RVA: 0x00482A83 File Offset: 0x00480C83
		public override float GetFloatValue()
		{
			return (float)this.CurrentValue / 100f;
		}

		// Token: 0x0600C49E RID: 50334 RVA: 0x00482A92 File Offset: 0x00480C92
		public override object GetValue()
		{
			return this.GetIntValue();
		}

		// Token: 0x0600C49F RID: 50335 RVA: 0x00482AA0 File Offset: 0x00480CA0
		public override void SetValue(string value)
		{
			int num;
			if (StringParsers.TryParseSInt32(value, out num) && base.ValueOptions.GetIntIndex(num) != -1)
			{
				this.CurrentValue = num;
			}
		}

		// Token: 0x0600C4A0 RID: 50336 RVA: 0x00482ACD File Offset: 0x00480CCD
		public override void SetInt(int value)
		{
			this.CurrentValue = value;
		}

		// Token: 0x0600C4A1 RID: 50337 RVA: 0x00482AD6 File Offset: 0x00480CD6
		public override bool IsChanged()
		{
			return this.CurrentValue != this.DefaultValue;
		}

		// Token: 0x0600C4A2 RID: 50338 RVA: 0x00482AE9 File Offset: 0x00480CE9
		public override object GetDefaultValue()
		{
			return this.DefaultValue;
		}

		// Token: 0x0600C4A3 RID: 50339 RVA: 0x00482AF6 File Offset: 0x00480CF6
		public override int GetDefaultIntValue()
		{
			return this.DefaultValue;
		}

		// Token: 0x0600C4A4 RID: 50340 RVA: 0x00482AFE File Offset: 0x00480CFE
		public override int GetDefaultIndex()
		{
			return base.GetValueSet().GetIntIndex(this.DefaultValue);
		}

		// Token: 0x0600C4A5 RID: 50341 RVA: 0x00482B11 File Offset: 0x00480D11
		public override void SetToDefault()
		{
			this.CurrentValue = this.DefaultValue;
		}

		// Token: 0x0600C4A6 RID: 50342 RVA: 0x00482B1F File Offset: 0x00480D1F
		public override string GetValueText()
		{
			return string.Format("{0} | default:{1}", this.CurrentValue, this.DefaultValue);
		}

		// Token: 0x0600C4A7 RID: 50343 RVA: 0x00482B41 File Offset: 0x00480D41
		public override int GetValueIndex()
		{
			return base.GetValueSet().GetIntIndex(this.CurrentValue);
		}

		// Token: 0x0600C4A8 RID: 50344 RVA: 0x00482B54 File Offset: 0x00480D54
		public override int GetValueIndex(string value)
		{
			int num;
			if (StringParsers.TryParseSInt32(value, out num))
			{
				return base.GetValueSet().GetIntIndex(num);
			}
			return base.GetValueSet().GetIntIndex(this.DefaultValue);
		}

		// Token: 0x0600C4A9 RID: 50345 RVA: 0x00482B89 File Offset: 0x00480D89
		public override void SetValueFromIndex(int index)
		{
			if (!base.GetValueSet().GetIntValue(index, out this.CurrentValue))
			{
				this.CurrentValue = this.DefaultValue;
			}
		}

		// Token: 0x0600C4AA RID: 50346 RVA: 0x00482BAB File Offset: 0x00480DAB
		public override SandboxOptions[] GetAlwaysShowOptions()
		{
			if (this.DisabledOptions == null || !this.DisabledOptions.AlwaysShowValuesOnEnabled)
			{
				return null;
			}
			return this.DisabledOptions.DisabledOptions;
		}

		// Token: 0x04009486 RID: 38022
		public int DefaultValue;

		// Token: 0x04009487 RID: 38023
		public int CurrentValue = 1;

		// Token: 0x04009488 RID: 38024
		public SandboxOptionInt.DisabledOptionsOnValue DisabledOptions;

		// Token: 0x020018F5 RID: 6389
		public class DisabledOptionsOnValue
		{
			// Token: 0x0600C4AB RID: 50347 RVA: 0x00482BCF File Offset: 0x00480DCF
			public DisabledOptionsOnValue(SandboxOptions[] options, int value, bool inverted = false, bool alwaysShowValuesOnEnabled = false)
			{
				this.DisabledOptions = options;
				this.Value = value;
				this.Inverted = inverted;
				this.AlwaysShowValuesOnEnabled = alwaysShowValuesOnEnabled;
			}

			// Token: 0x04009489 RID: 38025
			public int Value;

			// Token: 0x0400948A RID: 38026
			public bool Inverted;

			// Token: 0x0400948B RID: 38027
			public SandboxOptions[] DisabledOptions;

			// Token: 0x0400948C RID: 38028
			public bool AlwaysShowValuesOnEnabled;
		}
	}
}
