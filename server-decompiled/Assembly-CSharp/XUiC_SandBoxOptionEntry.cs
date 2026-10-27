using System;
using SandboxOptions;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E50 RID: 3664
[Preserve]
public class XUiC_SandBoxOptionEntry : XUiController
{
	// Token: 0x17000CB1 RID: 3249
	// (get) Token: 0x06006FE0 RID: 28640 RVA: 0x002B61AD File Offset: 0x002B43AD
	// (set) Token: 0x06006FE1 RID: 28641 RVA: 0x002B61B5 File Offset: 0x002B43B5
	public BaseSandboxOption Option
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000CB2 RID: 3250
	// (get) Token: 0x06006FE2 RID: 28642 RVA: 0x002B61BE File Offset: 0x002B43BE
	public XUiC_SandBoxOptionEntry.SandboxOptionValue OriginalValue
	{
		get
		{
			return this.originalValue;
		}
	}

	// Token: 0x17000CB3 RID: 3251
	// (get) Token: 0x06006FE3 RID: 28643 RVA: 0x002B61C6 File Offset: 0x002B43C6
	[XuiXmlBinding("option_enabled")]
	public bool Enabled
	{
		get
		{
			return this.HasEntry && SandboxOptionManager.Current.IsEnabled(this.sandboxOption) && !SandboxOptionManager.Current.IsOverriden(this.sandboxOption);
		}
	}

	// Token: 0x17000CB4 RID: 3252
	// (get) Token: 0x06006FE4 RID: 28644 RVA: 0x002B61F7 File Offset: 0x002B43F7
	[XuiXmlBinding("option_disabled_by_mod")]
	public bool DisabledByMod
	{
		get
		{
			return this.HasEntry && SandboxOptionManager.Current.IsOverriden(this.sandboxOption);
		}
	}

	// Token: 0x17000CB5 RID: 3253
	// (get) Token: 0x06006FE5 RID: 28645 RVA: 0x002B6213 File Offset: 0x002B4413
	[XuiXmlBinding("option_disabled_by_option")]
	public bool DisabledByOption
	{
		get
		{
			return this.HasEntry && !SandboxOptionManager.Current.IsEnabled(this.sandboxOption);
		}
	}

	// Token: 0x17000CB6 RID: 3254
	// (get) Token: 0x06006FE6 RID: 28646 RVA: 0x002B6232 File Offset: 0x002B4432
	[XuiXmlBinding("option_changed")]
	public bool IsChanged
	{
		get
		{
			return this.HasEntry && !this.originalValue.Equals(this.controlCombo.Value);
		}
	}

	// Token: 0x17000CB7 RID: 3255
	// (get) Token: 0x06006FE7 RID: 28647 RVA: 0x002B6257 File Offset: 0x002B4457
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

	// Token: 0x17000CB8 RID: 3256
	// (get) Token: 0x06006FE8 RID: 28648 RVA: 0x002B6274 File Offset: 0x002B4474
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

	// Token: 0x17000CB9 RID: 3257
	// (get) Token: 0x06006FE9 RID: 28649 RVA: 0x002B6291 File Offset: 0x002B4491
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

	// Token: 0x17000CBA RID: 3258
	// (get) Token: 0x06006FEA RID: 28650 RVA: 0x002B62AE File Offset: 0x002B44AE
	// (set) Token: 0x06006FEB RID: 28651 RVA: 0x002B62B6 File Offset: 0x002B44B6
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

	// Token: 0x17000CBB RID: 3259
	// (get) Token: 0x06006FEC RID: 28652 RVA: 0x002B62BF File Offset: 0x002B44BF
	[XuiXmlBinding("has_entry")]
	public bool HasEntry
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.Option != null;
		}
	}

	// Token: 0x06006FED RID: 28653 RVA: 0x002B62CA File Offset: 0x002B44CA
	public override void Init()
	{
		base.Init();
		this.sandboxOption = EnumUtils.Parse<global::SandboxOptions.SandboxOptions>(this.viewComponent.ID, false);
		this.setupOptions();
	}

	// Token: 0x06006FEE RID: 28654 RVA: 0x002B62EF File Offset: 0x002B44EF
	public void setupSeparator()
	{
		this.isSeparator = true;
		this.Option = null;
		this.sandboxOption = global::SandboxOptions.SandboxOptions.Max;
		this.IsDirty = true;
	}

	// Token: 0x06006FEF RID: 28655 RVA: 0x002B6314 File Offset: 0x002B4514
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

	// Token: 0x06006FF0 RID: 28656 RVA: 0x002B6387 File Offset: 0x002B4587
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (base.handleDirtyUpdateDefault() && this.HasEntry)
		{
			this.controlCombo.ValueTextOverride = (SandboxOptionManager.Current.IsEnabled(this.sandboxOption) ? null : "----");
		}
	}

	// Token: 0x06006FF1 RID: 28657 RVA: 0x002B63C8 File Offset: 0x002B45C8
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
					this.controlCombo.Elements.Add(new XUiC_SandBoxOptionEntry.SandboxOptionValue(num, valueSet.GetDisplayAtIndex(i, null)));
				}
				break;
			}
			case BaseSandboxOption.OptionTypes.Float:
			{
				float num2;
				if (valueSet.GetFloatValue(i, out num2))
				{
					this.controlCombo.Elements.Add(new XUiC_SandBoxOptionEntry.SandboxOptionValue(num2, valueSet.GetDisplayAtIndex(i, null)));
				}
				break;
			}
			case BaseSandboxOption.OptionTypes.Bool:
				this.controlCombo.Elements.Add(new XUiC_SandBoxOptionEntry.SandboxOptionValue(valueSet.GetBoolValue(i) ? 1 : 0, valueSet.GetDisplayAtIndex(i, null)));
				break;
			}
		}
	}

	// Token: 0x06006FF2 RID: 28658 RVA: 0x002B64BB File Offset: 0x002B46BB
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

	// Token: 0x06006FF3 RID: 28659 RVA: 0x002B64DC File Offset: 0x002B46DC
	public void ForceOriginalValue(XUiC_SandBoxOptionEntry.SandboxOptionValue _value)
	{
		this.originalValue = _value;
	}

	// Token: 0x06006FF4 RID: 28660 RVA: 0x002B64E5 File Offset: 0x002B46E5
	public void ResetToDefault(bool _storeAsOriginal)
	{
		SandboxOptionManager.Current.SetOptionToDefault(this.sandboxOption);
		this.setComboBoxValue(false, _storeAsOriginal);
	}

	// Token: 0x06006FF5 RID: 28661 RVA: 0x002B6500 File Offset: 0x002B4700
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

	// Token: 0x06006FF6 RID: 28662 RVA: 0x002B6600 File Offset: 0x002B4800
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

	// Token: 0x06006FF7 RID: 28663 RVA: 0x002B665C File Offset: 0x002B485C
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

	// Token: 0x06006FF8 RID: 28664 RVA: 0x002B691C File Offset: 0x002B4B1C
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

	// Token: 0x040054E5 RID: 21733
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<XUiC_SandBoxOptionEntry.SandboxOptionValue> controlCombo;

	// Token: 0x040054E6 RID: 21734
	public string CategoryName;

	// Token: 0x040054E7 RID: 21735
	[PublicizedFrom(EAccessModifier.Private)]
	public global::SandboxOptions.SandboxOptions sandboxOption = global::SandboxOptions.SandboxOptions.Max;

	// Token: 0x040054E8 RID: 21736
	[PublicizedFrom(EAccessModifier.Private)]
	public BaseSandboxOption.OptionTypes valueType;

	// Token: 0x040054EA RID: 21738
	public Action<XUiC_SandBoxOptionEntry, global::SandboxOptions.SandboxOptions> OnValueChanged;

	// Token: 0x040054EB RID: 21739
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SandBoxOptionEntry.SandboxOptionValue originalValue;

	// Token: 0x040054EC RID: 21740
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isSeparator;

	// Token: 0x02000E51 RID: 3665
	public enum EOptionValueType
	{
		// Token: 0x040054EE RID: 21742
		Null,
		// Token: 0x040054EF RID: 21743
		Custom,
		// Token: 0x040054F0 RID: 21744
		Any,
		// Token: 0x040054F1 RID: 21745
		Int,
		// Token: 0x040054F2 RID: 21746
		Float,
		// Token: 0x040054F3 RID: 21747
		String
	}

	// Token: 0x02000E52 RID: 3666
	public readonly struct SandboxOptionValue : IEquatable<XUiC_SandBoxOptionEntry.SandboxOptionValue>
	{
		// Token: 0x06006FFA RID: 28666 RVA: 0x002B6A25 File Offset: 0x002B4C25
		public SandboxOptionValue(XUiC_SandBoxOptionEntry.EOptionValueType _type, string _displayName)
		{
			this.Type = _type;
			this.IntValue = -1;
			this.FloatValue = -1f;
			this.StringValue = null;
			this.DisplayName = _displayName;
		}

		// Token: 0x06006FFB RID: 28667 RVA: 0x002B6A4E File Offset: 0x002B4C4E
		public SandboxOptionValue(int _intValue, string _displayName)
		{
			this.Type = XUiC_SandBoxOptionEntry.EOptionValueType.Int;
			this.IntValue = _intValue;
			this.FloatValue = -1f;
			this.StringValue = null;
			this.DisplayName = _displayName;
		}

		// Token: 0x06006FFC RID: 28668 RVA: 0x002B6A77 File Offset: 0x002B4C77
		public SandboxOptionValue(float _floatValue, string _displayName)
		{
			this.Type = XUiC_SandBoxOptionEntry.EOptionValueType.Float;
			this.IntValue = -1;
			this.FloatValue = _floatValue;
			this.StringValue = null;
			this.DisplayName = _displayName;
		}

		// Token: 0x06006FFD RID: 28669 RVA: 0x002B6A9C File Offset: 0x002B4C9C
		public SandboxOptionValue(string _stringValue, string _displayName)
		{
			this.Type = XUiC_SandBoxOptionEntry.EOptionValueType.String;
			this.IntValue = -1;
			this.FloatValue = -1f;
			this.StringValue = _stringValue;
			this.DisplayName = _displayName;
		}

		// Token: 0x06006FFE RID: 28670 RVA: 0x002B6AC5 File Offset: 0x002B4CC5
		public override string ToString()
		{
			return this.DisplayName;
		}

		// Token: 0x06006FFF RID: 28671 RVA: 0x002B6AD0 File Offset: 0x002B4CD0
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

		// Token: 0x06007000 RID: 28672 RVA: 0x002B6B58 File Offset: 0x002B4D58
		public override bool Equals(object _obj)
		{
			if (_obj is XUiC_SandBoxOptionEntry.SandboxOptionValue)
			{
				XUiC_SandBoxOptionEntry.SandboxOptionValue sandboxOptionValue = (XUiC_SandBoxOptionEntry.SandboxOptionValue)_obj;
				return this.Equals(sandboxOptionValue);
			}
			return false;
		}

		// Token: 0x06007001 RID: 28673 RVA: 0x002B6B80 File Offset: 0x002B4D80
		public override int GetHashCode()
		{
			return (int)((((((this.Type * (XUiC_SandBoxOptionEntry.EOptionValueType)397) ^ (XUiC_SandBoxOptionEntry.EOptionValueType)this.IntValue) * (XUiC_SandBoxOptionEntry.EOptionValueType)397) ^ (XUiC_SandBoxOptionEntry.EOptionValueType)((this.StringValue != null) ? this.StringValue.GetHashCode() : 0)) * (XUiC_SandBoxOptionEntry.EOptionValueType)397) ^ (XUiC_SandBoxOptionEntry.EOptionValueType)((this.DisplayName != null) ? this.DisplayName.GetHashCode() : 0));
		}

		// Token: 0x040054F4 RID: 21748
		public readonly XUiC_SandBoxOptionEntry.EOptionValueType Type;

		// Token: 0x040054F5 RID: 21749
		public readonly int IntValue;

		// Token: 0x040054F6 RID: 21750
		public readonly float FloatValue;

		// Token: 0x040054F7 RID: 21751
		public readonly string StringValue;

		// Token: 0x040054F8 RID: 21752
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string DisplayName;
	}
}
