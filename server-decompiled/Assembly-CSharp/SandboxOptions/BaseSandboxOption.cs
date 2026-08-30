using System;

namespace SandboxOptions
{
	// Token: 0x0200186B RID: 6251
	public class BaseSandboxOption
	{
		// Token: 0x17001764 RID: 5988
		// (get) Token: 0x0600C064 RID: 49252 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual BaseSandboxOption.OptionTypes OptionType
		{
			get
			{
				return BaseSandboxOption.OptionTypes.Invalid;
			}
		}

		// Token: 0x17001765 RID: 5989
		// (get) Token: 0x0600C065 RID: 49253 RVA: 0x0047483C File Offset: 0x00472A3C
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

		// Token: 0x17001766 RID: 5990
		// (get) Token: 0x0600C066 RID: 49254 RVA: 0x004748D0 File Offset: 0x00472AD0
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

		// Token: 0x17001767 RID: 5991
		// (get) Token: 0x0600C067 RID: 49255 RVA: 0x0047495C File Offset: 0x00472B5C
		// (set) Token: 0x0600C068 RID: 49256 RVA: 0x00474964 File Offset: 0x00472B64
		public SandboxOptionValueSet ValueOptions
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001768 RID: 5992
		// (get) Token: 0x0600C069 RID: 49257 RVA: 0x00031E8B File Offset: 0x0003008B
		public virtual string ValueText
		{
			get
			{
				return "";
			}
		}

		// Token: 0x0600C06A RID: 49258 RVA: 0x00474970 File Offset: 0x00472B70
		public BaseSandboxOption(SandboxOptions option, string name, string categoryName, string valueSetName, bool newUISection)
		{
			this.Option = option;
			this.OptionName = name;
			this.CategoryName = categoryName;
			this.ValueSetName = valueSetName;
			this.NewUISection = newUISection;
		}

		// Token: 0x0600C06B RID: 49259 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual bool GetBoolValue()
		{
			return false;
		}

		// Token: 0x0600C06C RID: 49260 RVA: 0x0003D066 File Offset: 0x0003B266
		public virtual float GetFloatValue()
		{
			return 0f;
		}

		// Token: 0x0600C06D RID: 49261 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual int GetIntValue()
		{
			return 0;
		}

		// Token: 0x0600C06E RID: 49262 RVA: 0x0001FF1E File Offset: 0x0001E11E
		public virtual object GetValue()
		{
			return null;
		}

		// Token: 0x0600C06F RID: 49263 RVA: 0x004749D0 File Offset: 0x00472BD0
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

		// Token: 0x0600C070 RID: 49264 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void SetValueFromIndex(int index)
		{
		}

		// Token: 0x0600C071 RID: 49265 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void SetValue(string value)
		{
		}

		// Token: 0x0600C072 RID: 49266 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void SetFloat(float value)
		{
		}

		// Token: 0x0600C073 RID: 49267 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void SetInt(int value)
		{
		}

		// Token: 0x0600C074 RID: 49268 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void SetBool(bool value)
		{
		}

		// Token: 0x0600C075 RID: 49269 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual bool IsChanged()
		{
			return false;
		}

		// Token: 0x0600C076 RID: 49270 RVA: 0x0001FF1E File Offset: 0x0001E11E
		public virtual object GetDefaultValue()
		{
			return null;
		}

		// Token: 0x0600C077 RID: 49271 RVA: 0x00474A3A File Offset: 0x00472C3A
		public virtual float GetDefaultFloatValue()
		{
			return -1f;
		}

		// Token: 0x0600C078 RID: 49272 RVA: 0x000FEFA9 File Offset: 0x000FD1A9
		public virtual int GetDefaultIntValue()
		{
			return -1;
		}

		// Token: 0x0600C079 RID: 49273 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual bool GetDefaultBoolValue()
		{
			return false;
		}

		// Token: 0x0600C07A RID: 49274 RVA: 0x000FEFA9 File Offset: 0x000FD1A9
		public virtual int GetDefaultIndex()
		{
			return -1;
		}

		// Token: 0x0600C07B RID: 49275 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void SetToDefault()
		{
		}

		// Token: 0x0600C07C RID: 49276 RVA: 0x00474A41 File Offset: 0x00472C41
		public virtual string GetDefaultValueText()
		{
			return this.GetValueSet().GetDisplayAtIndex(this.GetDefaultIndex());
		}

		// Token: 0x0600C07D RID: 49277 RVA: 0x00474A54 File Offset: 0x00472C54
		public virtual string GetValueTextFromIndex(int index)
		{
			return this.GetValueSet().GetDisplayAtIndex(index);
		}

		// Token: 0x0600C07E RID: 49278 RVA: 0x00031E8B File Offset: 0x0003008B
		public virtual string GetValueText()
		{
			return "";
		}

		// Token: 0x0600C07F RID: 49279 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual int GetValueIndex()
		{
			return 0;
		}

		// Token: 0x0600C080 RID: 49280 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual int GetValueIndex(string value)
		{
			return 0;
		}

		// Token: 0x0600C081 RID: 49281 RVA: 0x0001FF1E File Offset: 0x0001E11E
		public virtual SandboxOptions[] GetAlwaysShowOptions()
		{
			return null;
		}

		// Token: 0x0400919A RID: 37274
		public string OptionName;

		// Token: 0x0400919B RID: 37275
		[PublicizedFrom(EAccessModifier.Private)]
		public string optionNameText;

		// Token: 0x0400919C RID: 37276
		public string OverrideOptionName;

		// Token: 0x0400919D RID: 37277
		public string DisabledByText = "";

		// Token: 0x0400919E RID: 37278
		public bool IsEnabled = true;

		// Token: 0x0400919F RID: 37279
		[PublicizedFrom(EAccessModifier.Private)]
		public string descriptionText;

		// Token: 0x040091A0 RID: 37280
		public string OverrideDescriptionName;

		// Token: 0x040091A1 RID: 37281
		[PublicizedFrom(EAccessModifier.Private)]
		public string ValueSetName = "";

		// Token: 0x040091A3 RID: 37283
		public SandboxOptions Option;

		// Token: 0x040091A4 RID: 37284
		public string CategoryName = "";

		// Token: 0x040091A5 RID: 37285
		public bool NewUISection;

		// Token: 0x0200186C RID: 6252
		public enum OptionTypes
		{
			// Token: 0x040091A7 RID: 37287
			Invalid,
			// Token: 0x040091A8 RID: 37288
			Int,
			// Token: 0x040091A9 RID: 37289
			Float,
			// Token: 0x040091AA RID: 37290
			String,
			// Token: 0x040091AB RID: 37291
			Bool
		}
	}
}
