using System;

namespace SandboxOptions
{
	// Token: 0x02001895 RID: 6293
	public class SandboxOptionFloat : BaseSandboxOption
	{
		// Token: 0x170017DD RID: 6109
		// (get) Token: 0x0600C219 RID: 49689 RVA: 0x0002F168 File Offset: 0x0002D368
		public override BaseSandboxOption.OptionTypes OptionType
		{
			get
			{
				return BaseSandboxOption.OptionTypes.Float;
			}
		}

		// Token: 0x0600C21A RID: 49690 RVA: 0x0047A9A0 File Offset: 0x00478BA0
		public SandboxOptionFloat(SandboxOptions option, string optionName, string categoryName, string valueSetName, float defaultValue, bool newUISection = false, SandboxOptionFloat.DisabledOptionsOnValue disabledOptions = null)
			: base(option, optionName, categoryName, valueSetName, newUISection)
		{
			this.DefaultValue = defaultValue;
			this.DisabledOptions = disabledOptions;
		}

		// Token: 0x0600C21B RID: 49691 RVA: 0x0047A9CA File Offset: 0x00478BCA
		public override float GetFloatValue()
		{
			return this.CurrentValue;
		}

		// Token: 0x0600C21C RID: 49692 RVA: 0x0047A9D2 File Offset: 0x00478BD2
		public override int GetIntValue()
		{
			return (int)(this.CurrentValue * 100f);
		}

		// Token: 0x0600C21D RID: 49693 RVA: 0x0047A9E1 File Offset: 0x00478BE1
		public override object GetValue()
		{
			return this.GetFloatValue();
		}

		// Token: 0x170017DE RID: 6110
		// (get) Token: 0x0600C21E RID: 49694 RVA: 0x0047A9EE File Offset: 0x00478BEE
		public override string ValueText
		{
			get
			{
				return this.CurrentValue.ToString();
			}
		}

		// Token: 0x0600C21F RID: 49695 RVA: 0x0047A9FC File Offset: 0x00478BFC
		public override void SetValue(string value)
		{
			float num;
			if (StringParsers.TryParseFloat(value, out num) && base.ValueOptions.GetFloatIndex(num) != -1)
			{
				this.CurrentValue = num;
			}
		}

		// Token: 0x0600C220 RID: 49696 RVA: 0x0047AA29 File Offset: 0x00478C29
		public override void SetFloat(float value)
		{
			this.CurrentValue = value;
		}

		// Token: 0x0600C221 RID: 49697 RVA: 0x0047AA32 File Offset: 0x00478C32
		public override bool IsChanged()
		{
			return this.CurrentValue != this.DefaultValue;
		}

		// Token: 0x0600C222 RID: 49698 RVA: 0x0047AA45 File Offset: 0x00478C45
		public override object GetDefaultValue()
		{
			return this.DefaultValue;
		}

		// Token: 0x0600C223 RID: 49699 RVA: 0x0047AA52 File Offset: 0x00478C52
		public override float GetDefaultFloatValue()
		{
			return this.DefaultValue;
		}

		// Token: 0x0600C224 RID: 49700 RVA: 0x0047AA5A File Offset: 0x00478C5A
		public override int GetDefaultIntValue()
		{
			return (int)(this.DefaultValue * 100f);
		}

		// Token: 0x0600C225 RID: 49701 RVA: 0x0047AA69 File Offset: 0x00478C69
		public override int GetDefaultIndex()
		{
			return base.GetValueSet().GetFloatIndex(this.DefaultValue);
		}

		// Token: 0x0600C226 RID: 49702 RVA: 0x0047AA7C File Offset: 0x00478C7C
		public override void SetToDefault()
		{
			this.CurrentValue = this.DefaultValue;
		}

		// Token: 0x0600C227 RID: 49703 RVA: 0x0047AA8A File Offset: 0x00478C8A
		public override string GetValueText()
		{
			return string.Format("{0} | default:{1}", this.CurrentValue, this.DefaultValue);
		}

		// Token: 0x0600C228 RID: 49704 RVA: 0x0047AAAC File Offset: 0x00478CAC
		public override int GetValueIndex()
		{
			return base.GetValueSet().GetFloatIndex(this.CurrentValue);
		}

		// Token: 0x0600C229 RID: 49705 RVA: 0x0047AAC0 File Offset: 0x00478CC0
		public override int GetValueIndex(string value)
		{
			float num;
			if (StringParsers.TryParseFloat(value, out num))
			{
				return base.GetValueSet().GetFloatIndex(num);
			}
			return base.GetValueSet().GetFloatIndex(this.DefaultValue);
		}

		// Token: 0x0600C22A RID: 49706 RVA: 0x0047AAF5 File Offset: 0x00478CF5
		public override void SetValueFromIndex(int index)
		{
			if (!base.GetValueSet().GetFloatValue(index, out this.CurrentValue))
			{
				this.CurrentValue = this.DefaultValue;
			}
		}

		// Token: 0x0600C22B RID: 49707 RVA: 0x0047AB17 File Offset: 0x00478D17
		public override SandboxOptions[] GetAlwaysShowOptions()
		{
			if (this.DisabledOptions == null || !this.DisabledOptions.AlwaysShowValuesOnEnabled)
			{
				return null;
			}
			return this.DisabledOptions.DisabledOptions;
		}

		// Token: 0x040092CF RID: 37583
		public float DefaultValue;

		// Token: 0x040092D0 RID: 37584
		public float CurrentValue = 1f;

		// Token: 0x040092D1 RID: 37585
		public bool IsPercent;

		// Token: 0x040092D2 RID: 37586
		public SandboxOptionFloat.DisabledOptionsOnValue DisabledOptions;

		// Token: 0x02001896 RID: 6294
		public class DisabledOptionsOnValue
		{
			// Token: 0x0600C22C RID: 49708 RVA: 0x0047AB3B File Offset: 0x00478D3B
			public DisabledOptionsOnValue(SandboxOptions[] options, float value, bool inverted = false, bool alwaysShowValuesOnEnabled = false)
			{
				this.DisabledOptions = options;
				this.Value = value;
				this.Inverted = inverted;
				this.AlwaysShowValuesOnEnabled = alwaysShowValuesOnEnabled;
			}

			// Token: 0x040092D3 RID: 37587
			public float Value;

			// Token: 0x040092D4 RID: 37588
			public bool Inverted;

			// Token: 0x040092D5 RID: 37589
			public SandboxOptions[] DisabledOptions;

			// Token: 0x040092D6 RID: 37590
			public bool AlwaysShowValuesOnEnabled;
		}
	}
}
