using System;

namespace SandboxOptions
{
	// Token: 0x02001897 RID: 6295
	public class SandboxOptionInt : BaseSandboxOption
	{
		// Token: 0x170017DF RID: 6111
		// (get) Token: 0x0600C22D RID: 49709 RVA: 0x00020021 File Offset: 0x0001E221
		public override BaseSandboxOption.OptionTypes OptionType
		{
			get
			{
				return BaseSandboxOption.OptionTypes.Int;
			}
		}

		// Token: 0x0600C22E RID: 49710 RVA: 0x0047AB60 File Offset: 0x00478D60
		public SandboxOptionInt(SandboxOptions option, string optionName, string categoryName, string valueSetName, int defaultValue, bool newUISection = false, SandboxOptionInt.DisabledOptionsOnValue disabledOptions = null)
			: base(option, optionName, categoryName, valueSetName, newUISection)
		{
			this.DefaultValue = defaultValue;
			this.DisabledOptions = disabledOptions;
		}

		// Token: 0x0600C22F RID: 49711 RVA: 0x0047AB86 File Offset: 0x00478D86
		public override int GetIntValue()
		{
			return this.CurrentValue;
		}

		// Token: 0x170017E0 RID: 6112
		// (get) Token: 0x0600C230 RID: 49712 RVA: 0x0047AB8E File Offset: 0x00478D8E
		public override string ValueText
		{
			get
			{
				return this.CurrentValue.ToString();
			}
		}

		// Token: 0x0600C231 RID: 49713 RVA: 0x0047AB9B File Offset: 0x00478D9B
		public override float GetFloatValue()
		{
			return (float)this.CurrentValue / 100f;
		}

		// Token: 0x0600C232 RID: 49714 RVA: 0x0047ABAA File Offset: 0x00478DAA
		public override object GetValue()
		{
			return this.GetIntValue();
		}

		// Token: 0x0600C233 RID: 49715 RVA: 0x0047ABB8 File Offset: 0x00478DB8
		public override void SetValue(string value)
		{
			int num;
			if (StringParsers.TryParseSInt32(value, out num) && base.ValueOptions.GetIntIndex(num) != -1)
			{
				this.CurrentValue = num;
			}
		}

		// Token: 0x0600C234 RID: 49716 RVA: 0x0047ABE5 File Offset: 0x00478DE5
		public override void SetInt(int value)
		{
			this.CurrentValue = value;
		}

		// Token: 0x0600C235 RID: 49717 RVA: 0x0047ABEE File Offset: 0x00478DEE
		public override bool IsChanged()
		{
			return this.CurrentValue != this.DefaultValue;
		}

		// Token: 0x0600C236 RID: 49718 RVA: 0x0047AC01 File Offset: 0x00478E01
		public override object GetDefaultValue()
		{
			return this.DefaultValue;
		}

		// Token: 0x0600C237 RID: 49719 RVA: 0x0047AC0E File Offset: 0x00478E0E
		public override int GetDefaultIntValue()
		{
			return this.DefaultValue;
		}

		// Token: 0x0600C238 RID: 49720 RVA: 0x0047AC16 File Offset: 0x00478E16
		public override int GetDefaultIndex()
		{
			return base.GetValueSet().GetIntIndex(this.DefaultValue);
		}

		// Token: 0x0600C239 RID: 49721 RVA: 0x0047AC29 File Offset: 0x00478E29
		public override void SetToDefault()
		{
			this.CurrentValue = this.DefaultValue;
		}

		// Token: 0x0600C23A RID: 49722 RVA: 0x0047AC37 File Offset: 0x00478E37
		public override string GetValueText()
		{
			return string.Format("{0} | default:{1}", this.CurrentValue, this.DefaultValue);
		}

		// Token: 0x0600C23B RID: 49723 RVA: 0x0047AC59 File Offset: 0x00478E59
		public override int GetValueIndex()
		{
			return base.GetValueSet().GetIntIndex(this.CurrentValue);
		}

		// Token: 0x0600C23C RID: 49724 RVA: 0x0047AC6C File Offset: 0x00478E6C
		public override int GetValueIndex(string value)
		{
			int num;
			if (StringParsers.TryParseSInt32(value, out num))
			{
				return base.GetValueSet().GetIntIndex(num);
			}
			return base.GetValueSet().GetIntIndex(this.DefaultValue);
		}

		// Token: 0x0600C23D RID: 49725 RVA: 0x0047ACA1 File Offset: 0x00478EA1
		public override void SetValueFromIndex(int index)
		{
			if (!base.GetValueSet().GetIntValue(index, out this.CurrentValue))
			{
				this.CurrentValue = this.DefaultValue;
			}
		}

		// Token: 0x0600C23E RID: 49726 RVA: 0x0047ACC3 File Offset: 0x00478EC3
		public override SandboxOptions[] GetAlwaysShowOptions()
		{
			if (this.DisabledOptions == null || !this.DisabledOptions.AlwaysShowValuesOnEnabled)
			{
				return null;
			}
			return this.DisabledOptions.DisabledOptions;
		}

		// Token: 0x040092D7 RID: 37591
		public int DefaultValue;

		// Token: 0x040092D8 RID: 37592
		public int CurrentValue = 1;

		// Token: 0x040092D9 RID: 37593
		public SandboxOptionInt.DisabledOptionsOnValue DisabledOptions;

		// Token: 0x02001898 RID: 6296
		public class DisabledOptionsOnValue
		{
			// Token: 0x0600C23F RID: 49727 RVA: 0x0047ACE7 File Offset: 0x00478EE7
			public DisabledOptionsOnValue(SandboxOptions[] options, int value, bool inverted = false, bool alwaysShowValuesOnEnabled = false)
			{
				this.DisabledOptions = options;
				this.Value = value;
				this.Inverted = inverted;
				this.AlwaysShowValuesOnEnabled = alwaysShowValuesOnEnabled;
			}

			// Token: 0x040092DA RID: 37594
			public int Value;

			// Token: 0x040092DB RID: 37595
			public bool Inverted;

			// Token: 0x040092DC RID: 37596
			public SandboxOptions[] DisabledOptions;

			// Token: 0x040092DD RID: 37597
			public bool AlwaysShowValuesOnEnabled;
		}
	}
}
