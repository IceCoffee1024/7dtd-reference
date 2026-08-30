using System;

namespace SandboxOptions
{
	// Token: 0x02001893 RID: 6291
	public class SandboxOptionBoolean : BaseSandboxOption
	{
		// Token: 0x170017DB RID: 6107
		// (get) Token: 0x0600C207 RID: 49671 RVA: 0x00080550 File Offset: 0x0007E750
		public override BaseSandboxOption.OptionTypes OptionType
		{
			get
			{
				return BaseSandboxOption.OptionTypes.Bool;
			}
		}

		// Token: 0x0600C208 RID: 49672 RVA: 0x0047A800 File Offset: 0x00478A00
		public SandboxOptionBoolean(SandboxOptions option, string optionName, string categoryName, string valueSetName, bool defaultValue, bool newUISection = false, SandboxOptionBoolean.DisabledOptionsOnValue disabledOptions = null)
			: base(option, optionName, categoryName, valueSetName, newUISection)
		{
			this.DefaultValue = defaultValue;
			this.CurrentValue = defaultValue;
			this.DisabledOptions = disabledOptions;
		}

		// Token: 0x0600C209 RID: 49673 RVA: 0x0047A833 File Offset: 0x00478A33
		public override bool GetBoolValue()
		{
			return this.CurrentValue;
		}

		// Token: 0x0600C20A RID: 49674 RVA: 0x0047A83B File Offset: 0x00478A3B
		public override object GetValue()
		{
			return this.GetBoolValue();
		}

		// Token: 0x170017DC RID: 6108
		// (get) Token: 0x0600C20B RID: 49675 RVA: 0x0047A848 File Offset: 0x00478A48
		public override string ValueText
		{
			get
			{
				return this.CurrentValue.ToString();
			}
		}

		// Token: 0x0600C20C RID: 49676 RVA: 0x0047A858 File Offset: 0x00478A58
		public override void SetValue(string value)
		{
			bool flag;
			if (StringParsers.TryParseBool(value, out flag))
			{
				this.CurrentValue = flag;
			}
		}

		// Token: 0x0600C20D RID: 49677 RVA: 0x0047A876 File Offset: 0x00478A76
		public override void SetBool(bool value)
		{
			this.CurrentValue = value;
		}

		// Token: 0x0600C20E RID: 49678 RVA: 0x0047A87F File Offset: 0x00478A7F
		public override bool IsChanged()
		{
			return this.CurrentValue != this.DefaultValue;
		}

		// Token: 0x0600C20F RID: 49679 RVA: 0x0047A892 File Offset: 0x00478A92
		public override object GetDefaultValue()
		{
			return this.DefaultValue;
		}

		// Token: 0x0600C210 RID: 49680 RVA: 0x0047A89F File Offset: 0x00478A9F
		public override bool GetDefaultBoolValue()
		{
			return this.DefaultValue;
		}

		// Token: 0x0600C211 RID: 49681 RVA: 0x0047A8A7 File Offset: 0x00478AA7
		public override int GetDefaultIndex()
		{
			return base.GetValueSet().GetBoolIndex(this.DefaultValue);
		}

		// Token: 0x0600C212 RID: 49682 RVA: 0x0047A8BA File Offset: 0x00478ABA
		public override void SetToDefault()
		{
			this.CurrentValue = this.DefaultValue;
		}

		// Token: 0x0600C213 RID: 49683 RVA: 0x0047A8C8 File Offset: 0x00478AC8
		public override string GetValueText()
		{
			return string.Format("{0} | default:{1}", this.CurrentValue, this.DefaultValue);
		}

		// Token: 0x0600C214 RID: 49684 RVA: 0x0047A8EA File Offset: 0x00478AEA
		public override int GetValueIndex()
		{
			return base.GetValueSet().GetBoolIndex(this.CurrentValue);
		}

		// Token: 0x0600C215 RID: 49685 RVA: 0x0047A900 File Offset: 0x00478B00
		public override int GetValueIndex(string value)
		{
			bool flag;
			if (StringParsers.TryParseBool(value, out flag))
			{
				return base.GetValueSet().GetBoolIndex(flag);
			}
			return base.GetValueSet().GetBoolIndex(this.DefaultValue);
		}

		// Token: 0x0600C216 RID: 49686 RVA: 0x0047A935 File Offset: 0x00478B35
		public override void SetValueFromIndex(int index)
		{
			if (!base.GetValueSet().GetBoolValue(index, out this.CurrentValue))
			{
				this.CurrentValue = this.DefaultValue;
			}
		}

		// Token: 0x0600C217 RID: 49687 RVA: 0x0047A957 File Offset: 0x00478B57
		public override SandboxOptions[] GetAlwaysShowOptions()
		{
			if (this.DisabledOptions == null || !this.DisabledOptions.AlwaysShowValuesOnEnabled)
			{
				return null;
			}
			return this.DisabledOptions.DisabledOptions;
		}

		// Token: 0x040092C8 RID: 37576
		public bool DefaultValue;

		// Token: 0x040092C9 RID: 37577
		public bool CurrentValue;

		// Token: 0x040092CA RID: 37578
		public SandboxOptionBoolean.DisabledOptionsOnValue DisabledOptions;

		// Token: 0x02001894 RID: 6292
		public class DisabledOptionsOnValue
		{
			// Token: 0x0600C218 RID: 49688 RVA: 0x0047A97B File Offset: 0x00478B7B
			public DisabledOptionsOnValue(SandboxOptions[] options, bool value, bool inverted = false, bool alwaysShowValuesOnEnabled = false)
			{
				this.DisabledOptions = options;
				this.Value = value;
				this.Inverted = inverted;
				this.AlwaysShowValuesOnEnabled = alwaysShowValuesOnEnabled;
			}

			// Token: 0x040092CB RID: 37579
			public bool Value;

			// Token: 0x040092CC RID: 37580
			public bool Inverted;

			// Token: 0x040092CD RID: 37581
			public SandboxOptions[] DisabledOptions;

			// Token: 0x040092CE RID: 37582
			public bool AlwaysShowValuesOnEnabled;
		}
	}
}
