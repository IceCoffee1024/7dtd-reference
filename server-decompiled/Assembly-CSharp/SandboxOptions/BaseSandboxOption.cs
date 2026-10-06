using System;

namespace SandboxOptions
{
	// Token: 0x020018BD RID: 6333
	public class BaseSandboxOption
	{
		// Token: 0x17001878 RID: 6264
		// (get) Token: 0x0600C341 RID: 49985 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual BaseSandboxOption.OptionTypes OptionType
		{
			get
			{
				return BaseSandboxOption.OptionTypes.Invalid;
			}
		}

		// Token: 0x17001879 RID: 6265
		// (get) Token: 0x0600C342 RID: 49986 RVA: 0x0047EB14 File Offset: 0x0047CD14
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

		// Token: 0x1700187A RID: 6266
		// (get) Token: 0x0600C343 RID: 49987 RVA: 0x0047EBA8 File Offset: 0x0047CDA8
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

		// Token: 0x1700187B RID: 6267
		// (get) Token: 0x0600C344 RID: 49988 RVA: 0x0047EC34 File Offset: 0x0047CE34
		// (set) Token: 0x0600C345 RID: 49989 RVA: 0x0047EC3C File Offset: 0x0047CE3C
		public SandboxOptionValueSet ValueOptions
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x1700187C RID: 6268
		// (get) Token: 0x0600C346 RID: 49990 RVA: 0x0003223F File Offset: 0x0003043F
		public virtual string ValueText
		{
			get
			{
				return "";
			}
		}

		// Token: 0x0600C347 RID: 49991 RVA: 0x0047EC48 File Offset: 0x0047CE48
		public BaseSandboxOption(SandboxOptions option, string name, string categoryName, string valueSetName, bool newUISection)
		{
			this.Option = option;
			this.OptionName = name;
			this.CategoryName = categoryName;
			this.ValueSetName = valueSetName;
			this.NewUISection = newUISection;
		}

		// Token: 0x0600C348 RID: 49992 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual bool GetBoolValue()
		{
			return false;
		}

		// Token: 0x0600C349 RID: 49993 RVA: 0x0003D3B2 File Offset: 0x0003B5B2
		public virtual float GetFloatValue()
		{
			return 0f;
		}

		// Token: 0x0600C34A RID: 49994 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual int GetIntValue()
		{
			return 0;
		}

		// Token: 0x0600C34B RID: 49995 RVA: 0x000200DA File Offset: 0x0001E2DA
		public virtual object GetValue()
		{
			return null;
		}

		// Token: 0x0600C34C RID: 49996 RVA: 0x0047ECA8 File Offset: 0x0047CEA8
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

		// Token: 0x0600C34D RID: 49997 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void SetValueFromIndex(int index)
		{
		}

		// Token: 0x0600C34E RID: 49998 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void SetValue(string value)
		{
		}

		// Token: 0x0600C34F RID: 49999 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void SetFloat(float value)
		{
		}

		// Token: 0x0600C350 RID: 50000 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void SetInt(int value)
		{
		}

		// Token: 0x0600C351 RID: 50001 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void SetBool(bool value)
		{
		}

		// Token: 0x0600C352 RID: 50002 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual bool IsChanged()
		{
			return false;
		}

		// Token: 0x0600C353 RID: 50003 RVA: 0x000200DA File Offset: 0x0001E2DA
		public virtual object GetDefaultValue()
		{
			return null;
		}

		// Token: 0x0600C354 RID: 50004 RVA: 0x0047ED12 File Offset: 0x0047CF12
		public virtual float GetDefaultFloatValue()
		{
			return -1f;
		}

		// Token: 0x0600C355 RID: 50005 RVA: 0x0010087D File Offset: 0x000FEA7D
		public virtual int GetDefaultIntValue()
		{
			return -1;
		}

		// Token: 0x0600C356 RID: 50006 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual bool GetDefaultBoolValue()
		{
			return false;
		}

		// Token: 0x0600C357 RID: 50007 RVA: 0x0010087D File Offset: 0x000FEA7D
		public virtual int GetDefaultIndex()
		{
			return -1;
		}

		// Token: 0x0600C358 RID: 50008 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void SetToDefault()
		{
		}

		// Token: 0x0600C359 RID: 50009 RVA: 0x0047ED19 File Offset: 0x0047CF19
		public virtual string GetDefaultValueText(string languageName = null)
		{
			return this.GetValueSet().GetDisplayAtIndex(this.GetDefaultIndex(), languageName);
		}

		// Token: 0x0600C35A RID: 50010 RVA: 0x0047ED2D File Offset: 0x0047CF2D
		public virtual string GetValueTextFromIndex(int index, string languageName = null)
		{
			return this.GetValueSet().GetDisplayAtIndex(index, languageName);
		}

		// Token: 0x0600C35B RID: 50011 RVA: 0x0003223F File Offset: 0x0003043F
		public virtual string GetValueText()
		{
			return "";
		}

		// Token: 0x0600C35C RID: 50012 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual int GetValueIndex()
		{
			return 0;
		}

		// Token: 0x0600C35D RID: 50013 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual int GetValueIndex(string value)
		{
			return 0;
		}

		// Token: 0x0600C35E RID: 50014 RVA: 0x000200DA File Offset: 0x0001E2DA
		public virtual SandboxOptions[] GetAlwaysShowOptions()
		{
			return null;
		}

		// Token: 0x0400937C RID: 37756
		public string OptionName;

		// Token: 0x0400937D RID: 37757
		[PublicizedFrom(EAccessModifier.Private)]
		public string optionNameText;

		// Token: 0x0400937E RID: 37758
		public string OverrideOptionName;

		// Token: 0x0400937F RID: 37759
		public string DisabledByText = "";

		// Token: 0x04009380 RID: 37760
		public bool IsEnabled = true;

		// Token: 0x04009381 RID: 37761
		[PublicizedFrom(EAccessModifier.Private)]
		public string descriptionText;

		// Token: 0x04009382 RID: 37762
		public string OverrideDescriptionName;

		// Token: 0x04009383 RID: 37763
		[PublicizedFrom(EAccessModifier.Private)]
		public string ValueSetName = "";

		// Token: 0x04009385 RID: 37765
		public SandboxOptions Option;

		// Token: 0x04009386 RID: 37766
		public string CategoryName = "";

		// Token: 0x04009387 RID: 37767
		public bool NewUISection;

		// Token: 0x020018BE RID: 6334
		public enum OptionTypes
		{
			// Token: 0x04009389 RID: 37769
			Invalid,
			// Token: 0x0400938A RID: 37770
			Int,
			// Token: 0x0400938B RID: 37771
			Float,
			// Token: 0x0400938C RID: 37772
			String,
			// Token: 0x0400938D RID: 37773
			Bool
		}
	}
}
