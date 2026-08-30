using System;

namespace SandboxOptions
{
	// Token: 0x02001891 RID: 6289
	public class BaseSandboxOption
	{
		// Token: 0x170017D6 RID: 6102
		// (get) Token: 0x0600C1E9 RID: 49641 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual BaseSandboxOption.OptionTypes OptionType
		{
			get
			{
				return BaseSandboxOption.OptionTypes.Invalid;
			}
		}

		// Token: 0x170017D7 RID: 6103
		// (get) Token: 0x0600C1EA RID: 49642 RVA: 0x0047A5D8 File Offset: 0x004787D8
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

		// Token: 0x170017D8 RID: 6104
		// (get) Token: 0x0600C1EB RID: 49643 RVA: 0x0047A66C File Offset: 0x0047886C
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

		// Token: 0x170017D9 RID: 6105
		// (get) Token: 0x0600C1EC RID: 49644 RVA: 0x0047A6F8 File Offset: 0x004788F8
		// (set) Token: 0x0600C1ED RID: 49645 RVA: 0x0047A700 File Offset: 0x00478900
		public SandboxOptionValueSet ValueOptions
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x170017DA RID: 6106
		// (get) Token: 0x0600C1EE RID: 49646 RVA: 0x00032147 File Offset: 0x00030347
		public virtual string ValueText
		{
			get
			{
				return "";
			}
		}

		// Token: 0x0600C1EF RID: 49647 RVA: 0x0047A70C File Offset: 0x0047890C
		public BaseSandboxOption(SandboxOptions option, string name, string categoryName, string valueSetName, bool newUISection)
		{
			this.Option = option;
			this.OptionName = name;
			this.CategoryName = categoryName;
			this.ValueSetName = valueSetName;
			this.NewUISection = newUISection;
		}

		// Token: 0x0600C1F0 RID: 49648 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual bool GetBoolValue()
		{
			return false;
		}

		// Token: 0x0600C1F1 RID: 49649 RVA: 0x0003D2C6 File Offset: 0x0003B4C6
		public virtual float GetFloatValue()
		{
			return 0f;
		}

		// Token: 0x0600C1F2 RID: 49650 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual int GetIntValue()
		{
			return 0;
		}

		// Token: 0x0600C1F3 RID: 49651 RVA: 0x0001FFE2 File Offset: 0x0001E1E2
		public virtual object GetValue()
		{
			return null;
		}

		// Token: 0x0600C1F4 RID: 49652 RVA: 0x0047A76C File Offset: 0x0047896C
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

		// Token: 0x0600C1F5 RID: 49653 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void SetValueFromIndex(int index)
		{
		}

		// Token: 0x0600C1F6 RID: 49654 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void SetValue(string value)
		{
		}

		// Token: 0x0600C1F7 RID: 49655 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void SetFloat(float value)
		{
		}

		// Token: 0x0600C1F8 RID: 49656 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void SetInt(int value)
		{
		}

		// Token: 0x0600C1F9 RID: 49657 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void SetBool(bool value)
		{
		}

		// Token: 0x0600C1FA RID: 49658 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual bool IsChanged()
		{
			return false;
		}

		// Token: 0x0600C1FB RID: 49659 RVA: 0x0001FFE2 File Offset: 0x0001E1E2
		public virtual object GetDefaultValue()
		{
			return null;
		}

		// Token: 0x0600C1FC RID: 49660 RVA: 0x0047A7D6 File Offset: 0x004789D6
		public virtual float GetDefaultFloatValue()
		{
			return -1f;
		}

		// Token: 0x0600C1FD RID: 49661 RVA: 0x000FFF9D File Offset: 0x000FE19D
		public virtual int GetDefaultIntValue()
		{
			return -1;
		}

		// Token: 0x0600C1FE RID: 49662 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual bool GetDefaultBoolValue()
		{
			return false;
		}

		// Token: 0x0600C1FF RID: 49663 RVA: 0x000FFF9D File Offset: 0x000FE19D
		public virtual int GetDefaultIndex()
		{
			return -1;
		}

		// Token: 0x0600C200 RID: 49664 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void SetToDefault()
		{
		}

		// Token: 0x0600C201 RID: 49665 RVA: 0x0047A7DD File Offset: 0x004789DD
		public virtual string GetDefaultValueText(string languageName = null)
		{
			return this.GetValueSet().GetDisplayAtIndex(this.GetDefaultIndex(), languageName);
		}

		// Token: 0x0600C202 RID: 49666 RVA: 0x0047A7F1 File Offset: 0x004789F1
		public virtual string GetValueTextFromIndex(int index, string languageName = null)
		{
			return this.GetValueSet().GetDisplayAtIndex(index, languageName);
		}

		// Token: 0x0600C203 RID: 49667 RVA: 0x00032147 File Offset: 0x00030347
		public virtual string GetValueText()
		{
			return "";
		}

		// Token: 0x0600C204 RID: 49668 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual int GetValueIndex()
		{
			return 0;
		}

		// Token: 0x0600C205 RID: 49669 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual int GetValueIndex(string value)
		{
			return 0;
		}

		// Token: 0x0600C206 RID: 49670 RVA: 0x0001FFE2 File Offset: 0x0001E1E2
		public virtual SandboxOptions[] GetAlwaysShowOptions()
		{
			return null;
		}

		// Token: 0x040092B6 RID: 37558
		public string OptionName;

		// Token: 0x040092B7 RID: 37559
		[PublicizedFrom(EAccessModifier.Private)]
		public string optionNameText;

		// Token: 0x040092B8 RID: 37560
		public string OverrideOptionName;

		// Token: 0x040092B9 RID: 37561
		public string DisabledByText = "";

		// Token: 0x040092BA RID: 37562
		public bool IsEnabled = true;

		// Token: 0x040092BB RID: 37563
		[PublicizedFrom(EAccessModifier.Private)]
		public string descriptionText;

		// Token: 0x040092BC RID: 37564
		public string OverrideDescriptionName;

		// Token: 0x040092BD RID: 37565
		[PublicizedFrom(EAccessModifier.Private)]
		public string ValueSetName = "";

		// Token: 0x040092BF RID: 37567
		public SandboxOptions Option;

		// Token: 0x040092C0 RID: 37568
		public string CategoryName = "";

		// Token: 0x040092C1 RID: 37569
		public bool NewUISection;

		// Token: 0x02001892 RID: 6290
		public enum OptionTypes
		{
			// Token: 0x040092C3 RID: 37571
			Invalid,
			// Token: 0x040092C4 RID: 37572
			Int,
			// Token: 0x040092C5 RID: 37573
			Float,
			// Token: 0x040092C6 RID: 37574
			String,
			// Token: 0x040092C7 RID: 37575
			Bool
		}
	}
}
