using System;

namespace SandboxOptions
{
	// Token: 0x020018C3 RID: 6339
	public class SandboxOptionInt : BaseSandboxOption
	{
		// Token: 0x17001881 RID: 6273
		// (get) Token: 0x0600C385 RID: 50053 RVA: 0x00020119 File Offset: 0x0001E319
		public override BaseSandboxOption.OptionTypes OptionType
		{
			get
			{
				return BaseSandboxOption.OptionTypes.Int;
			}
		}

		// Token: 0x0600C386 RID: 50054 RVA: 0x0047F09C File Offset: 0x0047D29C
		public SandboxOptionInt(SandboxOptions option, string optionName, string categoryName, string valueSetName, int defaultValue, bool newUISection = false, SandboxOptionInt.DisabledOptionsOnValue disabledOptions = null)
			: base(option, optionName, categoryName, valueSetName, newUISection)
		{
			this.DefaultValue = defaultValue;
			this.DisabledOptions = disabledOptions;
		}

		// Token: 0x0600C387 RID: 50055 RVA: 0x0047F0C2 File Offset: 0x0047D2C2
		public override int GetIntValue()
		{
			return this.CurrentValue;
		}

		// Token: 0x17001882 RID: 6274
		// (get) Token: 0x0600C388 RID: 50056 RVA: 0x0047F0CA File Offset: 0x0047D2CA
		public override string ValueText
		{
			get
			{
				return this.CurrentValue.ToString();
			}
		}

		// Token: 0x0600C389 RID: 50057 RVA: 0x0047F0D7 File Offset: 0x0047D2D7
		public override float GetFloatValue()
		{
			return (float)this.CurrentValue / 100f;
		}

		// Token: 0x0600C38A RID: 50058 RVA: 0x0047F0E6 File Offset: 0x0047D2E6
		public override object GetValue()
		{
			return this.GetIntValue();
		}

		// Token: 0x0600C38B RID: 50059 RVA: 0x0047F0F4 File Offset: 0x0047D2F4
		public override void SetValue(string value)
		{
			int num;
			if (StringParsers.TryParseSInt32(value, out num) && base.ValueOptions.GetIntIndex(num) != -1)
			{
				this.CurrentValue = num;
			}
		}

		// Token: 0x0600C38C RID: 50060 RVA: 0x0047F121 File Offset: 0x0047D321
		public override void SetInt(int value)
		{
			this.CurrentValue = value;
		}

		// Token: 0x0600C38D RID: 50061 RVA: 0x0047F12A File Offset: 0x0047D32A
		public override bool IsChanged()
		{
			return this.CurrentValue != this.DefaultValue;
		}

		// Token: 0x0600C38E RID: 50062 RVA: 0x0047F13D File Offset: 0x0047D33D
		public override object GetDefaultValue()
		{
			return this.DefaultValue;
		}

		// Token: 0x0600C38F RID: 50063 RVA: 0x0047F14A File Offset: 0x0047D34A
		public override int GetDefaultIntValue()
		{
			return this.DefaultValue;
		}

		// Token: 0x0600C390 RID: 50064 RVA: 0x0047F152 File Offset: 0x0047D352
		public override int GetDefaultIndex()
		{
			return base.GetValueSet().GetIntIndex(this.DefaultValue);
		}

		// Token: 0x0600C391 RID: 50065 RVA: 0x0047F165 File Offset: 0x0047D365
		public override void SetToDefault()
		{
			this.CurrentValue = this.DefaultValue;
		}

		// Token: 0x0600C392 RID: 50066 RVA: 0x0047F173 File Offset: 0x0047D373
		public override string GetValueText()
		{
			return string.Format("{0} | default:{1}", this.CurrentValue, this.DefaultValue);
		}

		// Token: 0x0600C393 RID: 50067 RVA: 0x0047F195 File Offset: 0x0047D395
		public override int GetValueIndex()
		{
			return base.GetValueSet().GetIntIndex(this.CurrentValue);
		}

		// Token: 0x0600C394 RID: 50068 RVA: 0x0047F1A8 File Offset: 0x0047D3A8
		public override int GetValueIndex(string value)
		{
			int num;
			if (StringParsers.TryParseSInt32(value, out num))
			{
				return base.GetValueSet().GetIntIndex(num);
			}
			return base.GetValueSet().GetIntIndex(this.DefaultValue);
		}

		// Token: 0x0600C395 RID: 50069 RVA: 0x0047F1DD File Offset: 0x0047D3DD
		public override void SetValueFromIndex(int index)
		{
			if (!base.GetValueSet().GetIntValue(index, out this.CurrentValue))
			{
				this.CurrentValue = this.DefaultValue;
			}
		}

		// Token: 0x0600C396 RID: 50070 RVA: 0x0047F1FF File Offset: 0x0047D3FF
		public override SandboxOptions[] GetAlwaysShowOptions()
		{
			if (this.DisabledOptions == null || !this.DisabledOptions.AlwaysShowValuesOnEnabled)
			{
				return null;
			}
			return this.DisabledOptions.DisabledOptions;
		}

		// Token: 0x0400939D RID: 37789
		public int DefaultValue;

		// Token: 0x0400939E RID: 37790
		public int CurrentValue = 1;

		// Token: 0x0400939F RID: 37791
		public SandboxOptionInt.DisabledOptionsOnValue DisabledOptions;

		// Token: 0x020018C4 RID: 6340
		public class DisabledOptionsOnValue
		{
			// Token: 0x0600C397 RID: 50071 RVA: 0x0047F223 File Offset: 0x0047D423
			public DisabledOptionsOnValue(SandboxOptions[] options, int value, bool inverted = false, bool alwaysShowValuesOnEnabled = false)
			{
				this.DisabledOptions = options;
				this.Value = value;
				this.Inverted = inverted;
				this.AlwaysShowValuesOnEnabled = alwaysShowValuesOnEnabled;
			}

			// Token: 0x040093A0 RID: 37792
			public int Value;

			// Token: 0x040093A1 RID: 37793
			public bool Inverted;

			// Token: 0x040093A2 RID: 37794
			public SandboxOptions[] DisabledOptions;

			// Token: 0x040093A3 RID: 37795
			public bool AlwaysShowValuesOnEnabled;
		}
	}
}
