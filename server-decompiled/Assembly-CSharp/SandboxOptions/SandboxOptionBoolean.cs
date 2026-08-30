using System;

namespace SandboxOptions
{
	// Token: 0x0200186D RID: 6253
	public class SandboxOptionBoolean : BaseSandboxOption
	{
		// Token: 0x17001769 RID: 5993
		// (get) Token: 0x0600C082 RID: 49282 RVA: 0x0007FE9C File Offset: 0x0007E09C
		public override BaseSandboxOption.OptionTypes OptionType
		{
			get
			{
				return BaseSandboxOption.OptionTypes.Bool;
			}
		}

		// Token: 0x0600C083 RID: 49283 RVA: 0x00474A64 File Offset: 0x00472C64
		public SandboxOptionBoolean(SandboxOptions option, string optionName, string categoryName, string valueSetName, bool defaultValue, bool newUISection = false, SandboxOptionBoolean.DisabledOptionsOnValue disabledOptions = null)
			: base(option, optionName, categoryName, valueSetName, newUISection)
		{
			this.DefaultValue = defaultValue;
			this.CurrentValue = defaultValue;
			this.DisabledOptions = disabledOptions;
		}

		// Token: 0x0600C084 RID: 49284 RVA: 0x00474A97 File Offset: 0x00472C97
		public override bool GetBoolValue()
		{
			return this.CurrentValue;
		}

		// Token: 0x0600C085 RID: 49285 RVA: 0x00474A9F File Offset: 0x00472C9F
		public override object GetValue()
		{
			return this.GetBoolValue();
		}

		// Token: 0x1700176A RID: 5994
		// (get) Token: 0x0600C086 RID: 49286 RVA: 0x00474AAC File Offset: 0x00472CAC
		public override string ValueText
		{
			get
			{
				return this.CurrentValue.ToString();
			}
		}

		// Token: 0x0600C087 RID: 49287 RVA: 0x00474ABC File Offset: 0x00472CBC
		public override void SetValue(string value)
		{
			bool flag;
			if (StringParsers.TryParseBool(value, out flag))
			{
				this.CurrentValue = flag;
			}
		}

		// Token: 0x0600C088 RID: 49288 RVA: 0x00474ADA File Offset: 0x00472CDA
		public override void SetBool(bool value)
		{
			this.CurrentValue = value;
		}

		// Token: 0x0600C089 RID: 49289 RVA: 0x00474AE3 File Offset: 0x00472CE3
		public override bool IsChanged()
		{
			return this.CurrentValue != this.DefaultValue;
		}

		// Token: 0x0600C08A RID: 49290 RVA: 0x00474AF6 File Offset: 0x00472CF6
		public override object GetDefaultValue()
		{
			return this.DefaultValue;
		}

		// Token: 0x0600C08B RID: 49291 RVA: 0x00474B03 File Offset: 0x00472D03
		public override bool GetDefaultBoolValue()
		{
			return this.DefaultValue;
		}

		// Token: 0x0600C08C RID: 49292 RVA: 0x00474B0B File Offset: 0x00472D0B
		public override int GetDefaultIndex()
		{
			return base.GetValueSet().GetBoolIndex(this.DefaultValue);
		}

		// Token: 0x0600C08D RID: 49293 RVA: 0x00474B1E File Offset: 0x00472D1E
		public override void SetToDefault()
		{
			this.CurrentValue = this.DefaultValue;
		}

		// Token: 0x0600C08E RID: 49294 RVA: 0x00474B2C File Offset: 0x00472D2C
		public override string GetValueText()
		{
			return string.Format("{0} | default:{1}", this.CurrentValue, this.DefaultValue);
		}

		// Token: 0x0600C08F RID: 49295 RVA: 0x00474B4E File Offset: 0x00472D4E
		public override int GetValueIndex()
		{
			return base.GetValueSet().GetBoolIndex(this.CurrentValue);
		}

		// Token: 0x0600C090 RID: 49296 RVA: 0x00474B64 File Offset: 0x00472D64
		public override int GetValueIndex(string value)
		{
			bool flag;
			if (StringParsers.TryParseBool(value, out flag))
			{
				return base.GetValueSet().GetBoolIndex(flag);
			}
			return base.GetValueSet().GetBoolIndex(this.DefaultValue);
		}

		// Token: 0x0600C091 RID: 49297 RVA: 0x00474B99 File Offset: 0x00472D99
		public override void SetValueFromIndex(int index)
		{
			if (!base.GetValueSet().GetBoolValue(index, out this.CurrentValue))
			{
				this.CurrentValue = this.DefaultValue;
			}
		}

		// Token: 0x0600C092 RID: 49298 RVA: 0x00474BBB File Offset: 0x00472DBB
		public override SandboxOptions[] GetAlwaysShowOptions()
		{
			if (this.DisabledOptions == null || !this.DisabledOptions.AlwaysShowValuesOnEnabled)
			{
				return null;
			}
			return this.DisabledOptions.DisabledOptions;
		}

		// Token: 0x040091AC RID: 37292
		public bool DefaultValue;

		// Token: 0x040091AD RID: 37293
		public bool CurrentValue;

		// Token: 0x040091AE RID: 37294
		public SandboxOptionBoolean.DisabledOptionsOnValue DisabledOptions;

		// Token: 0x0200186E RID: 6254
		public class DisabledOptionsOnValue
		{
			// Token: 0x0600C093 RID: 49299 RVA: 0x00474BDF File Offset: 0x00472DDF
			public DisabledOptionsOnValue(SandboxOptions[] options, bool value, bool inverted = false, bool alwaysShowValuesOnEnabled = false)
			{
				this.DisabledOptions = options;
				this.Value = value;
				this.Inverted = inverted;
				this.AlwaysShowValuesOnEnabled = alwaysShowValuesOnEnabled;
			}

			// Token: 0x040091AF RID: 37295
			public bool Value;

			// Token: 0x040091B0 RID: 37296
			public bool Inverted;

			// Token: 0x040091B1 RID: 37297
			public SandboxOptions[] DisabledOptions;

			// Token: 0x040091B2 RID: 37298
			public bool AlwaysShowValuesOnEnabled;
		}
	}
}
