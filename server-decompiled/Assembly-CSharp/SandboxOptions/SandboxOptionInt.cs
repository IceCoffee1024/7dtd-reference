using System;

namespace SandboxOptions
{
	// Token: 0x02001871 RID: 6257
	public class SandboxOptionInt : BaseSandboxOption
	{
		// Token: 0x1700176D RID: 5997
		// (get) Token: 0x0600C0A7 RID: 49319 RVA: 0x0001FF5D File Offset: 0x0001E15D
		public override BaseSandboxOption.OptionTypes OptionType
		{
			get
			{
				return BaseSandboxOption.OptionTypes.Int;
			}
		}

		// Token: 0x0600C0A8 RID: 49320 RVA: 0x00474DB4 File Offset: 0x00472FB4
		public SandboxOptionInt(SandboxOptions option, string optionName, string categoryName, string valueSetName, int defaultValue, bool newUISection = false, SandboxOptionInt.DisabledOptionsOnValue disabledOptions = null)
			: base(option, optionName, categoryName, valueSetName, newUISection)
		{
			this.DefaultValue = defaultValue;
			this.DisabledOptions = disabledOptions;
		}

		// Token: 0x0600C0A9 RID: 49321 RVA: 0x00474DDA File Offset: 0x00472FDA
		public override int GetIntValue()
		{
			return this.CurrentValue;
		}

		// Token: 0x1700176E RID: 5998
		// (get) Token: 0x0600C0AA RID: 49322 RVA: 0x00474DE2 File Offset: 0x00472FE2
		public override string ValueText
		{
			get
			{
				return this.CurrentValue.ToString();
			}
		}

		// Token: 0x0600C0AB RID: 49323 RVA: 0x00474DEF File Offset: 0x00472FEF
		public override float GetFloatValue()
		{
			return (float)this.CurrentValue / 100f;
		}

		// Token: 0x0600C0AC RID: 49324 RVA: 0x00474DFE File Offset: 0x00472FFE
		public override object GetValue()
		{
			return this.GetIntValue();
		}

		// Token: 0x0600C0AD RID: 49325 RVA: 0x00474E0C File Offset: 0x0047300C
		public override void SetValue(string value)
		{
			int num;
			if (StringParsers.TryParseSInt32(value, out num) && base.ValueOptions.GetIntIndex(num) != -1)
			{
				this.CurrentValue = num;
			}
		}

		// Token: 0x0600C0AE RID: 49326 RVA: 0x00474E39 File Offset: 0x00473039
		public override void SetInt(int value)
		{
			this.CurrentValue = value;
		}

		// Token: 0x0600C0AF RID: 49327 RVA: 0x00474E42 File Offset: 0x00473042
		public override bool IsChanged()
		{
			return this.CurrentValue != this.DefaultValue;
		}

		// Token: 0x0600C0B0 RID: 49328 RVA: 0x00474E55 File Offset: 0x00473055
		public override object GetDefaultValue()
		{
			return this.DefaultValue;
		}

		// Token: 0x0600C0B1 RID: 49329 RVA: 0x00474E62 File Offset: 0x00473062
		public override int GetDefaultIntValue()
		{
			return this.DefaultValue;
		}

		// Token: 0x0600C0B2 RID: 49330 RVA: 0x00474E6A File Offset: 0x0047306A
		public override int GetDefaultIndex()
		{
			return base.GetValueSet().GetIntIndex(this.DefaultValue);
		}

		// Token: 0x0600C0B3 RID: 49331 RVA: 0x00474E7D File Offset: 0x0047307D
		public override void SetToDefault()
		{
			this.CurrentValue = this.DefaultValue;
		}

		// Token: 0x0600C0B4 RID: 49332 RVA: 0x00474E8B File Offset: 0x0047308B
		public override string GetValueText()
		{
			return string.Format("{0} | default:{1}", this.CurrentValue, this.DefaultValue);
		}

		// Token: 0x0600C0B5 RID: 49333 RVA: 0x00474EAD File Offset: 0x004730AD
		public override int GetValueIndex()
		{
			return base.GetValueSet().GetIntIndex(this.CurrentValue);
		}

		// Token: 0x0600C0B6 RID: 49334 RVA: 0x00474EC0 File Offset: 0x004730C0
		public override int GetValueIndex(string value)
		{
			int num;
			if (StringParsers.TryParseSInt32(value, out num))
			{
				return base.GetValueSet().GetIntIndex(num);
			}
			return base.GetValueSet().GetIntIndex(this.DefaultValue);
		}

		// Token: 0x0600C0B7 RID: 49335 RVA: 0x00474EF5 File Offset: 0x004730F5
		public override void SetValueFromIndex(int index)
		{
			if (!base.GetValueSet().GetIntValue(index, out this.CurrentValue))
			{
				this.CurrentValue = this.DefaultValue;
			}
		}

		// Token: 0x0600C0B8 RID: 49336 RVA: 0x00474F17 File Offset: 0x00473117
		public override SandboxOptions[] GetAlwaysShowOptions()
		{
			if (this.DisabledOptions == null || !this.DisabledOptions.AlwaysShowValuesOnEnabled)
			{
				return null;
			}
			return this.DisabledOptions.DisabledOptions;
		}

		// Token: 0x040091BB RID: 37307
		public int DefaultValue;

		// Token: 0x040091BC RID: 37308
		public int CurrentValue = 1;

		// Token: 0x040091BD RID: 37309
		public SandboxOptionInt.DisabledOptionsOnValue DisabledOptions;

		// Token: 0x02001872 RID: 6258
		public class DisabledOptionsOnValue
		{
			// Token: 0x0600C0B9 RID: 49337 RVA: 0x00474F3B File Offset: 0x0047313B
			public DisabledOptionsOnValue(SandboxOptions[] options, int value, bool inverted = false, bool alwaysShowValuesOnEnabled = false)
			{
				this.DisabledOptions = options;
				this.Value = value;
				this.Inverted = inverted;
				this.AlwaysShowValuesOnEnabled = alwaysShowValuesOnEnabled;
			}

			// Token: 0x040091BE RID: 37310
			public int Value;

			// Token: 0x040091BF RID: 37311
			public bool Inverted;

			// Token: 0x040091C0 RID: 37312
			public SandboxOptions[] DisabledOptions;

			// Token: 0x040091C1 RID: 37313
			public bool AlwaysShowValuesOnEnabled;
		}
	}
}
