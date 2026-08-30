using System;
using SandboxOptions;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E21 RID: 3617
[Preserve]
public class XUiC_SandBoxOptionEntry : XUiController
{
	// Token: 0x17000C77 RID: 3191
	// (get) Token: 0x06006F23 RID: 28451 RVA: 0x002AE36D File Offset: 0x002AC56D
	// (set) Token: 0x06006F24 RID: 28452 RVA: 0x002AE375 File Offset: 0x002AC575
	public BaseSandboxOption Option
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000C78 RID: 3192
	// (get) Token: 0x06006F25 RID: 28453 RVA: 0x002AE37E File Offset: 0x002AC57E
	public XUiC_SandBoxOptionEntry.SandboxOptionValue OriginalValue
	{
		get
		{
			return this.originalValue;
		}
	}

	// Token: 0x17000C79 RID: 3193
	// (get) Token: 0x06006F26 RID: 28454 RVA: 0x002AE386 File Offset: 0x002AC586
	[XuiXmlBinding("option_enabled")]
	public bool Enabled
	{
		get
		{
			return this.HasEntry && SandboxOptionManager.Current.IsEnabled(this.sandboxOption) && !SandboxOptionManager.Current.IsOverriden(this.sandboxOption);
		}
	}

	// Token: 0x17000C7A RID: 3194
	// (get) Token: 0x06006F27 RID: 28455 RVA: 0x002AE3B7 File Offset: 0x002AC5B7
	[XuiXmlBinding("option_disabled_by_mod")]
	public bool DisabledByMod
	{
		get
		{
			return this.HasEntry && SandboxOptionManager.Current.IsOverriden(this.sandboxOption);
		}
	}

	// Token: 0x17000C7B RID: 3195
	// (get) Token: 0x06006F28 RID: 28456 RVA: 0x002AE3D3 File Offset: 0x002AC5D3
	[XuiXmlBinding("option_disabled_by_option")]
	public bool DisabledByOption
	{
		get
		{
			return this.HasEntry && !SandboxOptionManager.Current.IsEnabled(this.sandboxOption);
		}
	}

	// Token: 0x17000C7C RID: 3196
	// (get) Token: 0x06006F29 RID: 28457 RVA: 0x002AE3F2 File Offset: 0x002AC5F2
	[XuiXmlBinding("option_changed")]
	public bool IsChanged
	{
		get
		{
			return this.HasEntry && !this.originalValue.Equals(this.controlCombo.Value);
		}
	}

	// Token: 0x17000C7D RID: 3197
	// (get) Token: 0x06006F2A RID: 28458 RVA: 0x002AE417 File Offset: 0x002AC617
	[XuiXmlBinding("title")]
	public string Title
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			BaseSandboxOption option = this.Option;
			return ((option != null) ? option.OptionNameText : null) ?? "";
		}
	}

	// Token: 0x17000C7E RID: 3198
	// (get) Token: 0x06006F2B RID: 28459 RVA: 0x002AE434 File Offset: 0x002AC634
	[XuiXmlBinding("tooltip_key")]
	public string TooltipKey
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			BaseSandboxOption option = this.Option;
			return ((option != null) ? option.DescriptionText : null) ?? "";
		}
	}

	// Token: 0x17000C7F RID: 3199
	// (get) Token: 0x06006F2C RID: 28460 RVA: 0x002AE451 File Offset: 0x002AC651
	[XuiXmlBinding("tooltip_key_disabledoption")]
	public string TooltipKeyDisabledOption
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			BaseSandboxOption option = this.Option;
			return ((option != null) ? option.DisabledByText : null) ?? "";
		}
	}

	// Token: 0x17000C80 RID: 3200
	// (get) Token: 0x06006F2D RID: 28461 RVA: 0x002AE46E File Offset: 0x002AC66E
	// (set) Token: 0x06006F2E RID: 28462 RVA: 0x002AE476 File Offset: 0x002AC676
	[XuiXmlBinding("is_separator")]
	public bool IsSeparator
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.isSeparator;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			this.isSeparator = value;
		}
	}

	// Token: 0x17000C81 RID: 3201
	// (get) Token: 0x06006F2F RID: 28463 RVA: 0x002AE47F File Offset: 0x002AC67F
	[XuiXmlBinding("has_entry")]
	public bool HasEntry
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.Option != null;
		}
	}

	// Token: 0x06006F30 RID: 28464 RVA: 0x002AE48A File Offset: 0x002AC68A
	public override void Init()
	{
		base.Init();
		this.sandboxOption = EnumUtils.Parse<global::SandboxOptions.SandboxOptions>(this.viewComponent.ID, false);
		this.setupOptions();
	}

	// Token: 0x06006F31 RID: 28465 RVA: 0x002AE4AF File Offset: 0x002AC6AF
	public void setupSeparator()
	{
		this.isSeparator = true;
		this.Option = null;
		this.sandboxOption = global::SandboxOptions.SandboxOptions.Max;
		this.IsDirty = true;
	}

	// Token: 0x06006F32 RID: 28466 RVA: 0x002AE4D4 File Offset: 0x002AC6D4
	[PublicizedFrom(EAccessModifier.Internal)]
	public void setupOption(BaseSandboxOption _baseSandboxOption)
	{
		this.isSeparator = false;
		this.Option = _baseSandboxOption;
		if (_baseSandboxOption == null)
		{
			this.sandboxOption = global::SandboxOptions.SandboxOptions.Max;
		}
		else
		{
			this.sandboxOption = this.Option.Option;
			this.valueType = this.Option.OptionType;
			this.CategoryName = this.Option.CategoryName;
			this.setupOptions();
			this.setComboBoxValue(false, false);
		}
		this.IsDirty = true;
	}

	// Token: 0x06006F33 RID: 28467 RVA: 0x002AE547 File Offset: 0x002AC747
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (base.handleDirtyUpdateDefault() && this.HasEntry)
		{
			this.controlCombo.ValueTextOverride = (SandboxOptionManager.Current.IsEnabled(this.sandboxOption) ? null : "----");
		}
	}

	// Token: 0x06006F34 RID: 28468 RVA: 0x002AE588 File Offset: 0x002AC788
	[PublicizedFrom(EAccessModifier.Private)]
	public void setupOptions()
	{
		this.controlCombo.Elements.Clear();
		if (this.Option == null)
		{
			return;
		}
		SandboxOptionValueSet valueSet = this.Option.GetValueSet();
		for (int i = 0; i < valueSet.GetValueCount(); i++)
		{
			switch (this.Option.OptionType)
			{
			case BaseSandboxOption.OptionTypes.Int:
			{
				int num;
				if (valueSet.GetIntValue(i, out num))
				{
					this.controlCombo.Elements.Add(new XUiC_SandBoxOptionEntry.SandboxOptionValue(num, valueSet.GetDisplayAtIndex(i)));
				}
				break;
			}
			case BaseSandboxOption.OptionTypes.Float:
			{
				float num2;
				if (valueSet.GetFloatValue(i, out num2))
				{
					this.controlCombo.Elements.Add(new XUiC_SandBoxOptionEntry.SandboxOptionValue(num2, valueSet.GetDisplayAtIndex(i)));
				}
				break;
			}
			case BaseSandboxOption.OptionTypes.Bool:
				this.controlCombo.Elements.Add(new XUiC_SandBoxOptionEntry.SandboxOptionValue(valueSet.GetBoolValue(i) ? 1 : 0, valueSet.GetDisplayAtIndex(i)));
				break;
			}
		}
	}

	// Token: 0x06006F35 RID: 28469 RVA: 0x002AE675 File Offset: 0x002AC875
	[XuiBindEvent("OnValueChanged", "controlCombo")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void ControlCombo_OnValueChanged(XUiController _sender, XUiC_SandBoxOptionEntry.SandboxOptionValue _oldValue, XUiC_SandBoxOptionEntry.SandboxOptionValue _newValue)
	{
		Action<XUiC_SandBoxOptionEntry, global::SandboxOptions.SandboxOptions> onValueChanged = this.OnValueChanged;
		if (onValueChanged != null)
		{
			onValueChanged(this, this.sandboxOption);
		}
		this.IsDirty = true;
	}

	// Token: 0x06006F36 RID: 28470 RVA: 0x002AE696 File Offset: 0x002AC896
	public void ForceOriginalValue(XUiC_SandBoxOptionEntry.SandboxOptionValue _value)
	{
		this.originalValue = _value;
	}

	// Token: 0x06006F37 RID: 28471 RVA: 0x002AE69F File Offset: 0x002AC89F
	public void ResetToDefault(bool _storeAsOriginal)
	{
		SandboxOptionManager.Current.SetOptionToDefault(this.sandboxOption);
		this.setComboBoxValue(false, _storeAsOriginal);
	}

	// Token: 0x06006F38 RID: 28472 RVA: 0x002AE6BC File Offset: 0x002AC8BC
	public void ApplyChange(bool _forceChange)
	{
		if (this.originalValue.Equals(this.controlCombo.Value) && !_forceChange)
		{
			return;
		}
		switch (this.valueType)
		{
		case BaseSandboxOption.OptionTypes.Int:
			SandboxOptionManager.Current.SetOption(this.sandboxOption, this.controlCombo.Value.IntValue);
			break;
		case BaseSandboxOption.OptionTypes.Float:
			SandboxOptionManager.Current.SetOption(this.sandboxOption, this.controlCombo.Value.FloatValue);
			break;
		case BaseSandboxOption.OptionTypes.String:
			SandboxOptionManager.Current.SetOption(this.sandboxOption, this.controlCombo.Value.StringValue);
			break;
		case BaseSandboxOption.OptionTypes.Bool:
			SandboxOptionManager.Current.SetOption(this.sandboxOption, this.controlCombo.Value.IntValue == 1);
			break;
		default:
			throw new Exception("Illegal option setup for " + this.sandboxOption.ToStringCached<global::SandboxOptions.SandboxOptions>());
		}
		this.IsDirty = true;
	}

	// Token: 0x06006F39 RID: 28473 RVA: 0x002AE7BC File Offset: 0x002AC9BC
	public void SetComboBoxIndex(int _index)
	{
		if (this.controlCombo.Elements.Count > _index)
		{
			this.controlCombo.SelectedIndex = _index;
		}
		this.originalValue = this.controlCombo.Value;
		Action<XUiC_SandBoxOptionEntry, global::SandboxOptions.SandboxOptions> onValueChanged = this.OnValueChanged;
		if (onValueChanged != null)
		{
			onValueChanged(this, this.sandboxOption);
		}
		this.IsDirty = true;
	}

	// Token: 0x06006F3A RID: 28474 RVA: 0x002AE818 File Offset: 0x002ACA18
	[PublicizedFrom(EAccessModifier.Private)]
	public void setComboBoxValue(bool _useCurrent, bool _storeAsOriginal)
	{
		try
		{
			switch (this.valueType)
			{
			case BaseSandboxOption.OptionTypes.Int:
			{
				int num = (_useCurrent ? this.controlCombo.Value.IntValue : SandboxOptionManager.GetInt(this.sandboxOption));
				bool flag = false;
				for (int i = 1; i < this.controlCombo.Elements.Count; i++)
				{
					if (this.controlCombo.Elements[i].IntValue == num)
					{
						this.controlCombo.SelectedIndex = i;
						flag = true;
						break;
					}
				}
				if (flag)
				{
					goto IL_0256;
				}
				int num2 = -1;
				int num3 = int.MaxValue;
				for (int j = 0; j < this.controlCombo.Elements.Count; j++)
				{
					int num4 = Math.Abs(this.controlCombo.Elements[j].IntValue - num);
					if (num2 < 0 || num4 < num3)
					{
						num2 = j;
						num3 = num4;
						if (num3 <= 0)
						{
							break;
						}
					}
				}
				if (num2 >= 0)
				{
					this.controlCombo.SelectedIndex = num2;
					goto IL_0256;
				}
				goto IL_0256;
			}
			case BaseSandboxOption.OptionTypes.Float:
			{
				float num5 = (_useCurrent ? this.controlCombo.Value.FloatValue : SandboxOptionManager.GetFloat(this.sandboxOption));
				bool flag2 = false;
				for (int k = 1; k < this.controlCombo.Elements.Count; k++)
				{
					if (Mathf.Approximately(this.controlCombo.Elements[k].FloatValue, num5))
					{
						this.controlCombo.SelectedIndex = k;
						flag2 = true;
						break;
					}
				}
				if (flag2)
				{
					goto IL_0256;
				}
				int num6 = -1;
				float num7 = float.MaxValue;
				for (int l = 0; l < this.controlCombo.Elements.Count; l++)
				{
					float num8 = Math.Abs(this.controlCombo.Elements[l].FloatValue - num5);
					if (num6 < 0 || num8 < num7)
					{
						num6 = l;
						num7 = num8;
						if (num7 <= 0f)
						{
							break;
						}
					}
				}
				if (num6 >= 0)
				{
					this.controlCombo.SelectedIndex = num6;
					goto IL_0256;
				}
				goto IL_0256;
			}
			case BaseSandboxOption.OptionTypes.Bool:
				this.controlCombo.SelectedIndex = (_useCurrent ? this.controlCombo.Value.IntValue : (SandboxOptionManager.GetBool(this.sandboxOption) ? 1 : 0));
				goto IL_0256;
			}
			throw new Exception("Illegal option setup for " + this.sandboxOption.ToStringCached<global::SandboxOptions.SandboxOptions>());
			IL_0256:;
		}
		catch (Exception ex)
		{
			Log.Exception(ex);
		}
		if (!_useCurrent && _storeAsOriginal)
		{
			this.originalValue = this.controlCombo.Value;
		}
		Action<XUiC_SandBoxOptionEntry, global::SandboxOptions.SandboxOptions> onValueChanged = this.OnValueChanged;
		if (onValueChanged != null)
		{
			onValueChanged(this, this.sandboxOption);
		}
		this.IsDirty = true;
	}

	// Token: 0x06006F3B RID: 28475 RVA: 0x002AEAD8 File Offset: 0x002ACCD8
	[XuiXmlBinding("option_default")]
	public bool IsDefaultValue()
	{
		if (this.Option == null)
		{
			this.Option = SandboxOptionManager.GetOption(this.sandboxOption);
		}
		if (!this.HasEntry)
		{
			return true;
		}
		bool flag;
		switch (this.valueType)
		{
		case BaseSandboxOption.OptionTypes.Int:
			flag = this.controlCombo.Value.IntValue == (int)this.Option.GetDefaultValue();
			break;
		case BaseSandboxOption.OptionTypes.Float:
			flag = Mathf.Approximately(this.controlCombo.Value.FloatValue, (float)this.Option.GetDefaultValue());
			break;
		case BaseSandboxOption.OptionTypes.String:
			flag = this.controlCombo.Value.StringValue == (string)this.Option.GetDefaultValue();
			break;
		case BaseSandboxOption.OptionTypes.Bool:
			flag = this.controlCombo.Value.IntValue == 1 == (bool)this.Option.GetDefaultValue();
			break;
		default:
			flag = false;
			break;
		}
		return flag;
	}

	// Token: 0x040053E5 RID: 21477
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<XUiC_SandBoxOptionEntry.SandboxOptionValue> controlCombo;

	// Token: 0x040053E6 RID: 21478
	public string CategoryName;

	// Token: 0x040053E7 RID: 21479
	[PublicizedFrom(EAccessModifier.Private)]
	public global::SandboxOptions.SandboxOptions sandboxOption = global::SandboxOptions.SandboxOptions.Max;

	// Token: 0x040053E8 RID: 21480
	[PublicizedFrom(EAccessModifier.Private)]
	public BaseSandboxOption.OptionTypes valueType;

	// Token: 0x040053EA RID: 21482
	public Action<XUiC_SandBoxOptionEntry, global::SandboxOptions.SandboxOptions> OnValueChanged;

	// Token: 0x040053EB RID: 21483
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SandBoxOptionEntry.SandboxOptionValue originalValue;

	// Token: 0x040053EC RID: 21484
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isSeparator;

	// Token: 0x02000E22 RID: 3618
	public enum EOptionValueType
	{
		// Token: 0x040053EE RID: 21486
		Null,
		// Token: 0x040053EF RID: 21487
		Custom,
		// Token: 0x040053F0 RID: 21488
		Any,
		// Token: 0x040053F1 RID: 21489
		Int,
		// Token: 0x040053F2 RID: 21490
		Float,
		// Token: 0x040053F3 RID: 21491
		String
	}

	// Token: 0x02000E23 RID: 3619
	public readonly struct SandboxOptionValue : IEquatable<XUiC_SandBoxOptionEntry.SandboxOptionValue>
	{
		// Token: 0x06006F3D RID: 28477 RVA: 0x002AEBE1 File Offset: 0x002ACDE1
		public SandboxOptionValue(XUiC_SandBoxOptionEntry.EOptionValueType _type, string _displayName)
		{
			this.Type = _type;
			this.IntValue = -1;
			this.FloatValue = -1f;
			this.StringValue = null;
			this.DisplayName = _displayName;
		}

		// Token: 0x06006F3E RID: 28478 RVA: 0x002AEC0A File Offset: 0x002ACE0A
		public SandboxOptionValue(int _intValue, string _displayName)
		{
			this.Type = XUiC_SandBoxOptionEntry.EOptionValueType.Int;
			this.IntValue = _intValue;
			this.FloatValue = -1f;
			this.StringValue = null;
			this.DisplayName = _displayName;
		}

		// Token: 0x06006F3F RID: 28479 RVA: 0x002AEC33 File Offset: 0x002ACE33
		public SandboxOptionValue(float _floatValue, string _displayName)
		{
			this.Type = XUiC_SandBoxOptionEntry.EOptionValueType.Float;
			this.IntValue = -1;
			this.FloatValue = _floatValue;
			this.StringValue = null;
			this.DisplayName = _displayName;
		}

		// Token: 0x06006F40 RID: 28480 RVA: 0x002AEC58 File Offset: 0x002ACE58
		public SandboxOptionValue(string _stringValue, string _displayName)
		{
			this.Type = XUiC_SandBoxOptionEntry.EOptionValueType.String;
			this.IntValue = -1;
			this.FloatValue = -1f;
			this.StringValue = _stringValue;
			this.DisplayName = _displayName;
		}

		// Token: 0x06006F41 RID: 28481 RVA: 0x002AEC81 File Offset: 0x002ACE81
		public override string ToString()
		{
			return this.DisplayName;
		}

		// Token: 0x06006F42 RID: 28482 RVA: 0x002AEC8C File Offset: 0x002ACE8C
		public bool Equals(XUiC_SandBoxOptionEntry.SandboxOptionValue _other)
		{
			if (this.Type != _other.Type)
			{
				return false;
			}
			bool flag;
			switch (this.Type)
			{
			case XUiC_SandBoxOptionEntry.EOptionValueType.Custom:
				flag = true;
				break;
			case XUiC_SandBoxOptionEntry.EOptionValueType.Any:
				flag = true;
				break;
			case XUiC_SandBoxOptionEntry.EOptionValueType.Int:
				flag = this.IntValue == _other.IntValue;
				break;
			case XUiC_SandBoxOptionEntry.EOptionValueType.Float:
				flag = Mathf.Approximately(this.FloatValue, _other.FloatValue);
				break;
			case XUiC_SandBoxOptionEntry.EOptionValueType.String:
				flag = this.StringValue == _other.StringValue;
				break;
			default:
				flag = false;
				break;
			}
			return flag;
		}

		// Token: 0x06006F43 RID: 28483 RVA: 0x002AED14 File Offset: 0x002ACF14
		public override bool Equals(object _obj)
		{
			if (_obj is XUiC_SandBoxOptionEntry.SandboxOptionValue)
			{
				XUiC_SandBoxOptionEntry.SandboxOptionValue sandboxOptionValue = (XUiC_SandBoxOptionEntry.SandboxOptionValue)_obj;
				return this.Equals(sandboxOptionValue);
			}
			return false;
		}

		// Token: 0x06006F44 RID: 28484 RVA: 0x002AED3C File Offset: 0x002ACF3C
		public override int GetHashCode()
		{
			return (int)((((((this.Type * (XUiC_SandBoxOptionEntry.EOptionValueType)397) ^ (XUiC_SandBoxOptionEntry.EOptionValueType)this.IntValue) * (XUiC_SandBoxOptionEntry.EOptionValueType)397) ^ (XUiC_SandBoxOptionEntry.EOptionValueType)((this.StringValue != null) ? this.StringValue.GetHashCode() : 0)) * (XUiC_SandBoxOptionEntry.EOptionValueType)397) ^ (XUiC_SandBoxOptionEntry.EOptionValueType)((this.DisplayName != null) ? this.DisplayName.GetHashCode() : 0));
		}

		// Token: 0x040053F4 RID: 21492
		public readonly XUiC_SandBoxOptionEntry.EOptionValueType Type;

		// Token: 0x040053F5 RID: 21493
		public readonly int IntValue;

		// Token: 0x040053F6 RID: 21494
		public readonly float FloatValue;

		// Token: 0x040053F7 RID: 21495
		public readonly string StringValue;

		// Token: 0x040053F8 RID: 21496
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string DisplayName;
	}
}
