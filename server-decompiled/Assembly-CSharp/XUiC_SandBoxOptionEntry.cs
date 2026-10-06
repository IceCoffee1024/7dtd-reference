using System;
using SandboxOptions;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E45 RID: 3653
[Preserve]
public class XUiC_SandBoxOptionEntry : XUiController
{
	// Token: 0x17000CAB RID: 3243
	// (get) Token: 0x06006FBB RID: 28603 RVA: 0x002B4C41 File Offset: 0x002B2E41
	// (set) Token: 0x06006FBC RID: 28604 RVA: 0x002B4C49 File Offset: 0x002B2E49
	public BaseSandboxOption Option
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000CAC RID: 3244
	// (get) Token: 0x06006FBD RID: 28605 RVA: 0x002B4C52 File Offset: 0x002B2E52
	public XUiC_SandBoxOptionEntry.SandboxOptionValue OriginalValue
	{
		get
		{
			return this.originalValue;
		}
	}

	// Token: 0x17000CAD RID: 3245
	// (get) Token: 0x06006FBE RID: 28606 RVA: 0x002B4C5A File Offset: 0x002B2E5A
	[XuiXmlBinding("option_enabled")]
	public bool Enabled
	{
		get
		{
			return this.HasEntry && SandboxOptionManager.Current.IsEnabled(this.sandboxOption) && !SandboxOptionManager.Current.IsOverriden(this.sandboxOption);
		}
	}

	// Token: 0x17000CAE RID: 3246
	// (get) Token: 0x06006FBF RID: 28607 RVA: 0x002B4C8B File Offset: 0x002B2E8B
	[XuiXmlBinding("option_disabled_by_mod")]
	public bool DisabledByMod
	{
		get
		{
			return this.HasEntry && SandboxOptionManager.Current.IsOverriden(this.sandboxOption);
		}
	}

	// Token: 0x17000CAF RID: 3247
	// (get) Token: 0x06006FC0 RID: 28608 RVA: 0x002B4CA7 File Offset: 0x002B2EA7
	[XuiXmlBinding("option_disabled_by_option")]
	public bool DisabledByOption
	{
		get
		{
			return this.HasEntry && !SandboxOptionManager.Current.IsEnabled(this.sandboxOption);
		}
	}

	// Token: 0x17000CB0 RID: 3248
	// (get) Token: 0x06006FC1 RID: 28609 RVA: 0x002B4CC6 File Offset: 0x002B2EC6
	[XuiXmlBinding("option_changed")]
	public bool IsChanged
	{
		get
		{
			return this.HasEntry && !this.originalValue.Equals(this.controlCombo.Value);
		}
	}

	// Token: 0x17000CB1 RID: 3249
	// (get) Token: 0x06006FC2 RID: 28610 RVA: 0x002B4CEB File Offset: 0x002B2EEB
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

	// Token: 0x17000CB2 RID: 3250
	// (get) Token: 0x06006FC3 RID: 28611 RVA: 0x002B4D08 File Offset: 0x002B2F08
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

	// Token: 0x17000CB3 RID: 3251
	// (get) Token: 0x06006FC4 RID: 28612 RVA: 0x002B4D25 File Offset: 0x002B2F25
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

	// Token: 0x17000CB4 RID: 3252
	// (get) Token: 0x06006FC5 RID: 28613 RVA: 0x002B4D42 File Offset: 0x002B2F42
	// (set) Token: 0x06006FC6 RID: 28614 RVA: 0x002B4D4A File Offset: 0x002B2F4A
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

	// Token: 0x17000CB5 RID: 3253
	// (get) Token: 0x06006FC7 RID: 28615 RVA: 0x002B4D53 File Offset: 0x002B2F53
	[XuiXmlBinding("has_entry")]
	public bool HasEntry
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.Option != null;
		}
	}

	// Token: 0x06006FC8 RID: 28616 RVA: 0x002B4D5E File Offset: 0x002B2F5E
	public override void Init()
	{
		base.Init();
		this.sandboxOption = EnumUtils.Parse<global::SandboxOptions.SandboxOptions>(this.viewComponent.ID, false);
		this.setupOptions();
	}

	// Token: 0x06006FC9 RID: 28617 RVA: 0x002B4D83 File Offset: 0x002B2F83
	public void setupSeparator()
	{
		this.isSeparator = true;
		this.Option = null;
		this.sandboxOption = global::SandboxOptions.SandboxOptions.Max;
		this.IsDirty = true;
	}

	// Token: 0x06006FCA RID: 28618 RVA: 0x002B4DA8 File Offset: 0x002B2FA8
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

	// Token: 0x06006FCB RID: 28619 RVA: 0x002B4E1B File Offset: 0x002B301B
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (base.handleDirtyUpdateDefault() && this.HasEntry)
		{
			this.controlCombo.ValueTextOverride = (SandboxOptionManager.Current.IsEnabled(this.sandboxOption) ? null : "----");
		}
	}

	// Token: 0x06006FCC RID: 28620 RVA: 0x002B4E5C File Offset: 0x002B305C
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

	// Token: 0x06006FCD RID: 28621 RVA: 0x002B4F4F File Offset: 0x002B314F
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

	// Token: 0x06006FCE RID: 28622 RVA: 0x002B4F70 File Offset: 0x002B3170
	public void ForceOriginalValue(XUiC_SandBoxOptionEntry.SandboxOptionValue _value)
	{
		this.originalValue = _value;
	}

	// Token: 0x06006FCF RID: 28623 RVA: 0x002B4F79 File Offset: 0x002B3179
	public void ResetToDefault(bool _storeAsOriginal)
	{
		SandboxOptionManager.Current.SetOptionToDefault(this.sandboxOption);
		this.setComboBoxValue(false, _storeAsOriginal);
	}

	// Token: 0x06006FD0 RID: 28624 RVA: 0x002B4F94 File Offset: 0x002B3194
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

	// Token: 0x06006FD1 RID: 28625 RVA: 0x002B5094 File Offset: 0x002B3294
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

	// Token: 0x06006FD2 RID: 28626 RVA: 0x002B50F0 File Offset: 0x002B32F0
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

	// Token: 0x06006FD3 RID: 28627 RVA: 0x002B53B0 File Offset: 0x002B35B0
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

	// Token: 0x040054C0 RID: 21696
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<XUiC_SandBoxOptionEntry.SandboxOptionValue> controlCombo;

	// Token: 0x040054C1 RID: 21697
	public string CategoryName;

	// Token: 0x040054C2 RID: 21698
	[PublicizedFrom(EAccessModifier.Private)]
	public global::SandboxOptions.SandboxOptions sandboxOption = global::SandboxOptions.SandboxOptions.Max;

	// Token: 0x040054C3 RID: 21699
	[PublicizedFrom(EAccessModifier.Private)]
	public BaseSandboxOption.OptionTypes valueType;

	// Token: 0x040054C5 RID: 21701
	public Action<XUiC_SandBoxOptionEntry, global::SandboxOptions.SandboxOptions> OnValueChanged;

	// Token: 0x040054C6 RID: 21702
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SandBoxOptionEntry.SandboxOptionValue originalValue;

	// Token: 0x040054C7 RID: 21703
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isSeparator;

	// Token: 0x02000E46 RID: 3654
	public enum EOptionValueType
	{
		// Token: 0x040054C9 RID: 21705
		Null,
		// Token: 0x040054CA RID: 21706
		Custom,
		// Token: 0x040054CB RID: 21707
		Any,
		// Token: 0x040054CC RID: 21708
		Int,
		// Token: 0x040054CD RID: 21709
		Float,
		// Token: 0x040054CE RID: 21710
		String
	}

	// Token: 0x02000E47 RID: 3655
	public readonly struct SandboxOptionValue : IEquatable<XUiC_SandBoxOptionEntry.SandboxOptionValue>
	{
		// Token: 0x06006FD5 RID: 28629 RVA: 0x002B54B9 File Offset: 0x002B36B9
		public SandboxOptionValue(XUiC_SandBoxOptionEntry.EOptionValueType _type, string _displayName)
		{
			this.Type = _type;
			this.IntValue = -1;
			this.FloatValue = -1f;
			this.StringValue = null;
			this.DisplayName = _displayName;
		}

		// Token: 0x06006FD6 RID: 28630 RVA: 0x002B54E2 File Offset: 0x002B36E2
		public SandboxOptionValue(int _intValue, string _displayName)
		{
			this.Type = XUiC_SandBoxOptionEntry.EOptionValueType.Int;
			this.IntValue = _intValue;
			this.FloatValue = -1f;
			this.StringValue = null;
			this.DisplayName = _displayName;
		}

		// Token: 0x06006FD7 RID: 28631 RVA: 0x002B550B File Offset: 0x002B370B
		public SandboxOptionValue(float _floatValue, string _displayName)
		{
			this.Type = XUiC_SandBoxOptionEntry.EOptionValueType.Float;
			this.IntValue = -1;
			this.FloatValue = _floatValue;
			this.StringValue = null;
			this.DisplayName = _displayName;
		}

		// Token: 0x06006FD8 RID: 28632 RVA: 0x002B5530 File Offset: 0x002B3730
		public SandboxOptionValue(string _stringValue, string _displayName)
		{
			this.Type = XUiC_SandBoxOptionEntry.EOptionValueType.String;
			this.IntValue = -1;
			this.FloatValue = -1f;
			this.StringValue = _stringValue;
			this.DisplayName = _displayName;
		}

		// Token: 0x06006FD9 RID: 28633 RVA: 0x002B5559 File Offset: 0x002B3759
		public override string ToString()
		{
			return this.DisplayName;
		}

		// Token: 0x06006FDA RID: 28634 RVA: 0x002B5564 File Offset: 0x002B3764
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

		// Token: 0x06006FDB RID: 28635 RVA: 0x002B55EC File Offset: 0x002B37EC
		public override bool Equals(object _obj)
		{
			if (_obj is XUiC_SandBoxOptionEntry.SandboxOptionValue)
			{
				XUiC_SandBoxOptionEntry.SandboxOptionValue sandboxOptionValue = (XUiC_SandBoxOptionEntry.SandboxOptionValue)_obj;
				return this.Equals(sandboxOptionValue);
			}
			return false;
		}

		// Token: 0x06006FDC RID: 28636 RVA: 0x002B5614 File Offset: 0x002B3814
		public override int GetHashCode()
		{
			return (int)((((((this.Type * (XUiC_SandBoxOptionEntry.EOptionValueType)397) ^ (XUiC_SandBoxOptionEntry.EOptionValueType)this.IntValue) * (XUiC_SandBoxOptionEntry.EOptionValueType)397) ^ (XUiC_SandBoxOptionEntry.EOptionValueType)((this.StringValue != null) ? this.StringValue.GetHashCode() : 0)) * (XUiC_SandBoxOptionEntry.EOptionValueType)397) ^ (XUiC_SandBoxOptionEntry.EOptionValueType)((this.DisplayName != null) ? this.DisplayName.GetHashCode() : 0));
		}

		// Token: 0x040054CF RID: 21711
		public readonly XUiC_SandBoxOptionEntry.EOptionValueType Type;

		// Token: 0x040054D0 RID: 21712
		public readonly int IntValue;

		// Token: 0x040054D1 RID: 21713
		public readonly float FloatValue;

		// Token: 0x040054D2 RID: 21714
		public readonly string StringValue;

		// Token: 0x040054D3 RID: 21715
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string DisplayName;
	}
}
