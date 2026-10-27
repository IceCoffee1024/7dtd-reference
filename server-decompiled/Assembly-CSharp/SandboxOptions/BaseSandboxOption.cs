using System;

namespace SandboxOptions
{
	// Token: 0x020018EE RID: 6382
	public class BaseSandboxOption
	{
		// Token: 0x170018C0 RID: 6336
		// (get) Token: 0x0600C455 RID: 50261 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual BaseSandboxOption.OptionTypes OptionType
		{
			get
			{
				return BaseSandboxOption.OptionTypes.Invalid;
			}
		}

		// Token: 0x170018C1 RID: 6337
		// (get) Token: 0x0600C456 RID: 50262 RVA: 0x004824C0 File Offset: 0x004806C0
		public string OptionNameText
		{
			get
			{
				if (this.optionNameText == null)
				{
					if (this.OverrideOptionName == null)
					{
						string text = "go" + this.Option.ToString();
						this.optionNameText = (Localization.Exists(text, false) ? Localization.Get(text, false, null) : ("*" + this.OptionName));
					}
					else
					{
						this.optionNameText = (Localization.Exists(this.OverrideOptionName, false) ? Localization.Get(this.OverrideOptionName, false, null) : "");
					}
				}
				return this.optionNameText;
			}
		}

		// Token: 0x170018C2 RID: 6338
		// (get) Token: 0x0600C457 RID: 50263 RVA: 0x00482554 File Offset: 0x00480754
		public string DescriptionText
		{
			get
			{
				if (this.descriptionText == null)
				{
					if (this.OverrideDescriptionName == null)
					{
						string text = "go" + this.Option.ToString() + "Desc";
						this.descriptionText = (Localization.Exists(text, false) ? Localization.Get(text, false, null) : "");
					}
					else
					{
						this.descriptionText = (Localization.Exists(this.OverrideDescriptionName, false) ? Localization.Get(this.OverrideDescriptionName, false, null) : "");
					}
				}
				return this.descriptionText;
			}
		}

		// Token: 0x170018C3 RID: 6339
		// (get) Token: 0x0600C458 RID: 50264 RVA: 0x004825E0 File Offset: 0x004807E0
		// (set) Token: 0x0600C459 RID: 50265 RVA: 0x004825E8 File Offset: 0x004807E8
		public SandboxOptionValueSet ValueOptions
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x170018C4 RID: 6340
		// (get) Token: 0x0600C45A RID: 50266 RVA: 0x000321FF File Offset: 0x000303FF
		public virtual string ValueText
		{
			get
			{
				return "";
			}
		}

		// Token: 0x0600C45B RID: 50267 RVA: 0x004825F4 File Offset: 0x004807F4
		public BaseSandboxOption(SandboxOptions option, string name, string categoryName, string valueSetName, bool newUISection)
		{
			this.Option = option;
			this.OptionName = name;
			this.CategoryName = categoryName;
			this.ValueSetName = valueSetName;
			this.NewUISection = newUISection;
		}

		// Token: 0x0600C45C RID: 50268 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual bool GetBoolValue()
		{
			return false;
		}

		// Token: 0x0600C45D RID: 50269 RVA: 0x0003D372 File Offset: 0x0003B572
		public virtual float GetFloatValue()
		{
			return 0f;
		}

		// Token: 0x0600C45E RID: 50270 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual int GetIntValue()
		{
			return 0;
		}

		// Token: 0x0600C45F RID: 50271 RVA: 0x000200DA File Offset: 0x0001E2DA
		public virtual object GetValue()
		{
			return null;
		}

		// Token: 0x0600C460 RID: 50272 RVA: 0x00482654 File Offset: 0x00480854
		public SandboxOptionValueSet GetValueSet()
		{
			if (this.ValueOptions == null)
			{
				if (!SandboxOptionManager.Current.ValueSets.ContainsKey(this.ValueSetName))
				{
					throw new Exception("BaseSandboxOption: ValueOption " + this.ValueSetName + " does not exist.");
				}
				this.ValueOptions = SandboxOptionManager.Current.ValueSets[this.ValueSetName];
			}
			return this.ValueOptions;
		}

		// Token: 0x0600C461 RID: 50273 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void SetValueFromIndex(int index)
		{
		}

		// Token: 0x0600C462 RID: 50274 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void SetValue(string value)
		{
		}

		// Token: 0x0600C463 RID: 50275 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void SetFloat(float value)
		{
		}

		// Token: 0x0600C464 RID: 50276 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void SetInt(int value)
		{
		}

		// Token: 0x0600C465 RID: 50277 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void SetBool(bool value)
		{
		}

		// Token: 0x0600C466 RID: 50278 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual bool IsChanged()
		{
			return false;
		}

		// Token: 0x0600C467 RID: 50279 RVA: 0x000200DA File Offset: 0x0001E2DA
		public virtual object GetDefaultValue()
		{
			return null;
		}

		// Token: 0x0600C468 RID: 50280 RVA: 0x004826BE File Offset: 0x004808BE
		public virtual float GetDefaultFloatValue()
		{
			return -1f;
		}

		// Token: 0x0600C469 RID: 50281 RVA: 0x00100DD1 File Offset: 0x000FEFD1
		public virtual int GetDefaultIntValue()
		{
			return -1;
		}

		// Token: 0x0600C46A RID: 50282 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual bool GetDefaultBoolValue()
		{
			return false;
		}

		// Token: 0x0600C46B RID: 50283 RVA: 0x00100DD1 File Offset: 0x000FEFD1
		public virtual int GetDefaultIndex()
		{
			return -1;
		}

		// Token: 0x0600C46C RID: 50284 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void SetToDefault()
		{
		}

		// Token: 0x0600C46D RID: 50285 RVA: 0x004826C5 File Offset: 0x004808C5
		public virtual string GetDefaultValueText(string languageName = null)
		{
			return this.GetValueSet().GetDisplayAtIndex(this.GetDefaultIndex(), languageName);
		}

		// Token: 0x0600C46E RID: 50286 RVA: 0x004826D9 File Offset: 0x004808D9
		public virtual string GetValueTextFromIndex(int index, string languageName = null)
		{
			return this.GetValueSet().GetDisplayAtIndex(index, languageName);
		}

		// Token: 0x0600C46F RID: 50287 RVA: 0x000321FF File Offset: 0x000303FF
		public virtual string GetValueText()
		{
			return "";
		}

		// Token: 0x0600C470 RID: 50288 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual int GetValueIndex()
		{
			return 0;
		}

		// Token: 0x0600C471 RID: 50289 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual int GetValueIndex(string value)
		{
			return 0;
		}

		// Token: 0x0600C472 RID: 50290 RVA: 0x000200DA File Offset: 0x0001E2DA
		public virtual SandboxOptions[] GetAlwaysShowOptions()
		{
			return null;
		}

		// Token: 0x04009465 RID: 37989
		public string OptionName;

		// Token: 0x04009466 RID: 37990
		[PublicizedFrom(EAccessModifier.Private)]
		public string optionNameText;

		// Token: 0x04009467 RID: 37991
		public string OverrideOptionName;

		// Token: 0x04009468 RID: 37992
		public string DisabledByText = "";

		// Token: 0x04009469 RID: 37993
		public bool IsEnabled = true;

		// Token: 0x0400946A RID: 37994
		[PublicizedFrom(EAccessModifier.Private)]
		public string descriptionText;

		// Token: 0x0400946B RID: 37995
		public string OverrideDescriptionName;

		// Token: 0x0400946C RID: 37996
		[PublicizedFrom(EAccessModifier.Private)]
		public string ValueSetName = "";

		// Token: 0x0400946E RID: 37998
		public SandboxOptions Option;

		// Token: 0x0400946F RID: 37999
		public string CategoryName = "";

		// Token: 0x04009470 RID: 38000
		public bool NewUISection;

		// Token: 0x020018EF RID: 6383
		public enum OptionTypes
		{
			// Token: 0x04009472 RID: 38002
			Invalid,
			// Token: 0x04009473 RID: 38003
			Int,
			// Token: 0x04009474 RID: 38004
			Float,
			// Token: 0x04009475 RID: 38005
			String,
			// Token: 0x04009476 RID: 38006
			Bool
		}
	}
}
