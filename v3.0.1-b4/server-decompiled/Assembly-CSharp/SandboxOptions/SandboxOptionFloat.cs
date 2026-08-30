using System;

namespace SandboxOptions
{
	// Token: 0x0200186F RID: 6255
	public class SandboxOptionFloat : BaseSandboxOption
	{
		// Token: 0x1700176B RID: 5995
		// (get) Token: 0x0600C094 RID: 49300 RVA: 0x0002EEAC File Offset: 0x0002D0AC
		public override BaseSandboxOption.OptionTypes OptionType
		{
			get
			{
				return BaseSandboxOption.OptionTypes.Float;
			}
		}

		// Token: 0x0600C095 RID: 49301 RVA: 0x00474C04 File Offset: 0x00472E04
		public SandboxOptionFloat(SandboxOptions option, string optionName, string categoryName, string valueSetName, float defaultValue, bool newUISection = false, SandboxOptionFloat.DisabledOptionsOnValue disabledOptions = null)
			: base(option, optionName, categoryName, valueSetName, newUISection)
		{
			this.DefaultValue = defaultValue;
			this.DisabledOptions = disabledOptions;
		}

		// Token: 0x0600C096 RID: 49302 RVA: 0x00474C2E File Offset: 0x00472E2E
		public override float GetFloatValue()
		{
			return this.CurrentValue;
		}

		// Token: 0x0600C097 RID: 49303 RVA: 0x00474C36 File Offset: 0x00472E36
		public override int GetIntValue()
		{
			return (int)(this.CurrentValue * 100f);
		}

		// Token: 0x0600C098 RID: 49304 RVA: 0x00474C45 File Offset: 0x00472E45
		public override object GetValue()
		{
			return this.GetFloatValue();
		}

		// Token: 0x1700176C RID: 5996
		// (get) Token: 0x0600C099 RID: 49305 RVA: 0x00474C52 File Offset: 0x00472E52
		public override string ValueText
		{
			get
			{
				return this.CurrentValue.ToString();
			}
		}

		// Token: 0x0600C09A RID: 49306 RVA: 0x00474C60 File Offset: 0x00472E60
		public override void SetValue(string value)
		{
			float num;
			if (StringParsers.TryParseFloat(value, out num) && base.ValueOptions.GetFloatIndex(num) != -1)
			{
				this.CurrentValue = num;
			}
		}

		// Token: 0x0600C09B RID: 49307 RVA: 0x00474C8D File Offset: 0x00472E8D
		public override void SetFloat(float value)
		{
			this.CurrentValue = value;
		}

		// Token: 0x0600C09C RID: 49308 RVA: 0x00474C96 File Offset: 0x00472E96
		public override bool IsChanged()
		{
			return this.CurrentValue != this.DefaultValue;
		}

		// Token: 0x0600C09D RID: 49309 RVA: 0x00474CA9 File Offset: 0x00472EA9
		public override object GetDefaultValue()
		{
			return this.DefaultValue;
		}

		// Token: 0x0600C09E RID: 49310 RVA: 0x00474CB6 File Offset: 0x00472EB6
		public override float GetDefaultFloatValue()
		{
			return this.DefaultValue;
		}

		// Token: 0x0600C09F RID: 49311 RVA: 0x00474CBE File Offset: 0x00472EBE
		public override int GetDefaultIndex()
		{
			return base.GetValueSet().GetFloatIndex(this.DefaultValue);
		}

		// Token: 0x0600C0A0 RID: 49312 RVA: 0x00474CD1 File Offset: 0x00472ED1
		public override void SetToDefault()
		{
			this.CurrentValue = this.DefaultValue;
		}

		// Token: 0x0600C0A1 RID: 49313 RVA: 0x00474CDF File Offset: 0x00472EDF
		public override string GetValueText()
		{
			return string.Format("{0} | default:{1}", this.CurrentValue, this.DefaultValue);
		}

		// Token: 0x0600C0A2 RID: 49314 RVA: 0x00474D01 File Offset: 0x00472F01
		public override int GetValueIndex()
		{
			return base.GetValueSet().GetFloatIndex(this.CurrentValue);
		}

		// Token: 0x0600C0A3 RID: 49315 RVA: 0x00474D14 File Offset: 0x00472F14
		public override int GetValueIndex(string value)
		{
			float num;
			if (StringParsers.TryParseFloat(value, out num))
			{
				return base.GetValueSet().GetFloatIndex(num);
			}
			return base.GetValueSet().GetFloatIndex(this.DefaultValue);
		}

		// Token: 0x0600C0A4 RID: 49316 RVA: 0x00474D49 File Offset: 0x00472F49
		public override void SetValueFromIndex(int index)
		{
			if (!base.GetValueSet().GetFloatValue(index, out this.CurrentValue))
			{
				this.CurrentValue = this.DefaultValue;
			}
		}

		// Token: 0x0600C0A5 RID: 49317 RVA: 0x00474D6B File Offset: 0x00472F6B
		public override SandboxOptions[] GetAlwaysShowOptions()
		{
			if (this.DisabledOptions == null || !this.DisabledOptions.AlwaysShowValuesOnEnabled)
			{
				return null;
			}
			return this.DisabledOptions.DisabledOptions;
		}

		// Token: 0x040091B3 RID: 37299
		public float DefaultValue;

		// Token: 0x040091B4 RID: 37300
		public float CurrentValue = 1f;

		// Token: 0x040091B5 RID: 37301
		public bool IsPercent;

		// Token: 0x040091B6 RID: 37302
		public SandboxOptionFloat.DisabledOptionsOnValue DisabledOptions;

		// Token: 0x02001870 RID: 6256
		public class DisabledOptionsOnValue
		{
			// Token: 0x0600C0A6 RID: 49318 RVA: 0x00474D8F File Offset: 0x00472F8F
			public DisabledOptionsOnValue(SandboxOptions[] options, float value, bool inverted = false, bool alwaysShowValuesOnEnabled = false)
			{
				this.DisabledOptions = options;
				this.Value = value;
				this.Inverted = inverted;
				this.AlwaysShowValuesOnEnabled = alwaysShowValuesOnEnabled;
			}

			// Token: 0x040091B7 RID: 37303
			public float Value;

			// Token: 0x040091B8 RID: 37304
			public bool Inverted;

			// Token: 0x040091B9 RID: 37305
			public SandboxOptions[] DisabledOptions;

			// Token: 0x040091BA RID: 37306
			public bool AlwaysShowValuesOnEnabled;
		}
	}
}
