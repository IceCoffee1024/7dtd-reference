using System;
using InControl;

// Token: 0x020012A1 RID: 4769
public class NGuiAction
{
	// Token: 0x0600975D RID: 38749 RVA: 0x003909B4 File Offset: 0x0038EBB4
	public NGuiAction()
	{
	}

	// Token: 0x0600975E RID: 38750 RVA: 0x003909C3 File Offset: 0x0038EBC3
	public NGuiAction(string _text, PlayerAction _hotkey)
		: this(_text, null, false)
	{
		this.hotkey = _hotkey;
	}

	// Token: 0x0600975F RID: 38751 RVA: 0x003909D5 File Offset: 0x0038EBD5
	public NGuiAction(string _text, string _icon, bool _isToggle)
		: this(_text, _icon, null, _isToggle, null)
	{
	}

	// Token: 0x06009760 RID: 38752 RVA: 0x003909E2 File Offset: 0x0038EBE2
	public NGuiAction(string _text, string _icon, string _description, bool _isToggle, PlayerAction _hotkey)
	{
		this.text = _text;
		this.icon = _icon;
		this.description = _description;
		this.hotkey = _hotkey;
		this.bToggle = _isToggle;
		this.bEnabled = true;
	}

	// Token: 0x06009761 RID: 38753 RVA: 0x00390A1D File Offset: 0x0038EC1D
	public virtual void OnClick()
	{
		if (this.IsEnabled() && this.clickActionDelegate != null)
		{
			this.clickActionDelegate();
		}
		this.UpdateUI();
	}

	// Token: 0x06009762 RID: 38754 RVA: 0x00390A40 File Offset: 0x0038EC40
	public virtual void OnRelease()
	{
		if (this.IsEnabled() && this.releaseActionDelegate != null)
		{
			this.releaseActionDelegate();
		}
		this.UpdateUI();
	}

	// Token: 0x06009763 RID: 38755 RVA: 0x00390A63 File Offset: 0x0038EC63
	public virtual void OnDoubleClick()
	{
		if (this.IsEnabled() && this.doubleClickActionDelegate != null)
		{
			this.doubleClickActionDelegate();
		}
		this.UpdateUI();
	}

	// Token: 0x06009764 RID: 38756 RVA: 0x00390A86 File Offset: 0x0038EC86
	public virtual void OnSelect(bool _bSelected)
	{
		if (this.IsEnabled())
		{
			if (this.IsToggle())
			{
				this.SetChecked(_bSelected);
			}
			if (this.selectActionDelegate != null)
			{
				this.selectActionDelegate(_bSelected);
			}
		}
		this.UpdateUI();
	}

	// Token: 0x06009765 RID: 38757 RVA: 0x00390AB9 File Offset: 0x0038ECB9
	public virtual bool IsActive()
	{
		return this.isVisibleDelegate == null || this.isVisibleDelegate();
	}

	// Token: 0x06009766 RID: 38758 RVA: 0x00390AD0 File Offset: 0x0038ECD0
	public virtual string GetIcon()
	{
		return this.icon;
	}

	// Token: 0x06009767 RID: 38759 RVA: 0x00390AD8 File Offset: 0x0038ECD8
	public virtual string GetText()
	{
		return this.text;
	}

	// Token: 0x06009768 RID: 38760 RVA: 0x00390AE0 File Offset: 0x0038ECE0
	public void SetText(string _text)
	{
		this.text = _text;
		this.UpdateUI();
	}

	// Token: 0x06009769 RID: 38761 RVA: 0x00390AEF File Offset: 0x0038ECEF
	public virtual string GetTooltip()
	{
		return this.tooltip;
	}

	// Token: 0x0600976A RID: 38762 RVA: 0x00390AF7 File Offset: 0x0038ECF7
	public NGuiAction SetTooltip(string _tooltip)
	{
		this.tooltip = (string.IsNullOrEmpty(_tooltip) ? null : Localization.Get(_tooltip, false, null));
		return this;
	}

	// Token: 0x0600976B RID: 38763 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual int GetColumnCount()
	{
		return 0;
	}

	// Token: 0x0600976C RID: 38764 RVA: 0x000200DA File Offset: 0x0001E2DA
	public virtual string GetColumnIcon(int _col)
	{
		return null;
	}

	// Token: 0x0600976D RID: 38765 RVA: 0x000200DA File Offset: 0x0001E2DA
	public virtual string GetColumnText(int _col)
	{
		return null;
	}

	// Token: 0x0600976E RID: 38766 RVA: 0x00390B13 File Offset: 0x0038ED13
	public virtual string GetDescription()
	{
		return this.description;
	}

	// Token: 0x0600976F RID: 38767 RVA: 0x00390B1B File Offset: 0x0038ED1B
	public virtual NGuiAction SetDescription(string _desc)
	{
		this.description = _desc;
		return this;
	}

	// Token: 0x06009770 RID: 38768 RVA: 0x00390B25 File Offset: 0x0038ED25
	public virtual PlayerAction GetHotkey()
	{
		return this.hotkey;
	}

	// Token: 0x06009771 RID: 38769 RVA: 0x00390B2D File Offset: 0x0038ED2D
	public virtual NGuiAction SetEnabled(bool _bEnabled)
	{
		this.bEnabled = _bEnabled;
		this.UpdateUI();
		return this;
	}

	// Token: 0x06009772 RID: 38770 RVA: 0x00390B3D File Offset: 0x0038ED3D
	public virtual bool IsEnabled()
	{
		if (this.isEnabledDelegate != null)
		{
			return this.isEnabledDelegate();
		}
		return this.bEnabled;
	}

	// Token: 0x06009773 RID: 38771 RVA: 0x00390B59 File Offset: 0x0038ED59
	public virtual bool IsToggle()
	{
		return this.bToggle;
	}

	// Token: 0x06009774 RID: 38772 RVA: 0x00390B61 File Offset: 0x0038ED61
	public virtual void SetChecked(bool _bChecked)
	{
		this.bChecked = _bChecked;
		this.UpdateUI();
	}

	// Token: 0x06009775 RID: 38773 RVA: 0x00390B70 File Offset: 0x0038ED70
	public virtual bool IsChecked()
	{
		if (this.isCheckedDelegate != null)
		{
			return this.isCheckedDelegate();
		}
		return this.bChecked;
	}

	// Token: 0x06009776 RID: 38774 RVA: 0x00390B8C File Offset: 0x0038ED8C
	public virtual NGuiAction SetIsCheckedDelegate(NGuiAction.IsCheckedDelegate _checkedDelegate)
	{
		this.isCheckedDelegate = _checkedDelegate;
		return this;
	}

	// Token: 0x06009777 RID: 38775 RVA: 0x00390B96 File Offset: 0x0038ED96
	public virtual NGuiAction SetIsVisibleDelegate(NGuiAction.IsVisibleDelegate _isVisibleDelegate)
	{
		this.isVisibleDelegate = _isVisibleDelegate;
		return this;
	}

	// Token: 0x06009778 RID: 38776 RVA: 0x00390BA0 File Offset: 0x0038EDA0
	public virtual NGuiAction SetIsEnabledDelegate(NGuiAction.IsEnabledDelegate _isEnabledDelegate)
	{
		this.isEnabledDelegate = _isEnabledDelegate;
		return this;
	}

	// Token: 0x06009779 RID: 38777 RVA: 0x00390BAA File Offset: 0x0038EDAA
	public virtual NGuiAction SetClickActionDelegate(NGuiAction.OnClickActionDelegate _actionDelegate)
	{
		this.clickActionDelegate = _actionDelegate;
		return this;
	}

	// Token: 0x0600977A RID: 38778 RVA: 0x00390BB4 File Offset: 0x0038EDB4
	public virtual NGuiAction SetReleaseActionDelegate(NGuiAction.OnReleaseActionDelegate _actionDelegate)
	{
		this.releaseActionDelegate = _actionDelegate;
		return this;
	}

	// Token: 0x0600977B RID: 38779 RVA: 0x00390BBE File Offset: 0x0038EDBE
	public virtual NGuiAction SetDoubleClickActionDelegate(NGuiAction.OnDoubleClickActionDelegate _actionDelegate)
	{
		this.doubleClickActionDelegate = _actionDelegate;
		return this;
	}

	// Token: 0x0600977C RID: 38780 RVA: 0x00390BC8 File Offset: 0x0038EDC8
	public virtual NGuiAction SetSelectActionDelegate(NGuiAction.OnSelectActionDelegate _selectActionDelegate)
	{
		this.selectActionDelegate = _selectActionDelegate;
		return this;
	}

	// Token: 0x0600977D RID: 38781 RVA: 0x000027FC File Offset: 0x000009FC
	public void UpdateUI()
	{
	}

	// Token: 0x0600977E RID: 38782 RVA: 0x00390BD2 File Offset: 0x0038EDD2
	public override string ToString()
	{
		if (this.text == null)
		{
			return string.Empty;
		}
		return this.text;
	}

	// Token: 0x0400715F RID: 29023
	[PublicizedFrom(EAccessModifier.Private)]
	public PlayerAction hotkey;

	// Token: 0x04007160 RID: 29024
	public static NGuiAction Separator = new NGuiAction("Sep", null);

	// Token: 0x04007161 RID: 29025
	[PublicizedFrom(EAccessModifier.Private)]
	public string text;

	// Token: 0x04007162 RID: 29026
	[PublicizedFrom(EAccessModifier.Private)]
	public string icon;

	// Token: 0x04007163 RID: 29027
	[PublicizedFrom(EAccessModifier.Private)]
	public string description;

	// Token: 0x04007164 RID: 29028
	[PublicizedFrom(EAccessModifier.Private)]
	public string tooltip;

	// Token: 0x04007165 RID: 29029
	[PublicizedFrom(EAccessModifier.Private)]
	public NGuiAction.OnClickActionDelegate clickActionDelegate;

	// Token: 0x04007166 RID: 29030
	[PublicizedFrom(EAccessModifier.Private)]
	public NGuiAction.OnReleaseActionDelegate releaseActionDelegate;

	// Token: 0x04007167 RID: 29031
	[PublicizedFrom(EAccessModifier.Private)]
	public NGuiAction.OnDoubleClickActionDelegate doubleClickActionDelegate;

	// Token: 0x04007168 RID: 29032
	[PublicizedFrom(EAccessModifier.Private)]
	public NGuiAction.OnSelectActionDelegate selectActionDelegate;

	// Token: 0x04007169 RID: 29033
	[PublicizedFrom(EAccessModifier.Private)]
	public NGuiAction.IsVisibleDelegate isVisibleDelegate;

	// Token: 0x0400716A RID: 29034
	[PublicizedFrom(EAccessModifier.Private)]
	public NGuiAction.IsCheckedDelegate isCheckedDelegate;

	// Token: 0x0400716B RID: 29035
	[PublicizedFrom(EAccessModifier.Private)]
	public NGuiAction.IsEnabledDelegate isEnabledDelegate;

	// Token: 0x0400716C RID: 29036
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bEnabled;

	// Token: 0x0400716D RID: 29037
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bToggle;

	// Token: 0x0400716E RID: 29038
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bChecked;

	// Token: 0x0400716F RID: 29039
	public NGuiAction.EnumKeyMode KeyMode = NGuiAction.EnumKeyMode.FireOnPress;

	// Token: 0x020012A2 RID: 4770
	[Flags]
	public enum EnumKeyMode
	{
		// Token: 0x04007171 RID: 29041
		None = 0,
		// Token: 0x04007172 RID: 29042
		FireOnPress = 1,
		// Token: 0x04007173 RID: 29043
		FireOnRelease = 2,
		// Token: 0x04007174 RID: 29044
		FireOnRepeat = 4
	}

	// Token: 0x020012A3 RID: 4771
	// (Invoke) Token: 0x06009781 RID: 38785
	public delegate void OnClickActionDelegate();

	// Token: 0x020012A4 RID: 4772
	// (Invoke) Token: 0x06009785 RID: 38789
	public delegate void OnReleaseActionDelegate();

	// Token: 0x020012A5 RID: 4773
	// (Invoke) Token: 0x06009789 RID: 38793
	public delegate void OnDoubleClickActionDelegate();

	// Token: 0x020012A6 RID: 4774
	// (Invoke) Token: 0x0600978D RID: 38797
	public delegate void OnSelectActionDelegate(bool _bSelected);

	// Token: 0x020012A7 RID: 4775
	// (Invoke) Token: 0x06009791 RID: 38801
	public delegate bool IsEnabledDelegate();

	// Token: 0x020012A8 RID: 4776
	// (Invoke) Token: 0x06009795 RID: 38805
	public delegate bool IsVisibleDelegate();

	// Token: 0x020012A9 RID: 4777
	// (Invoke) Token: 0x06009799 RID: 38809
	public delegate bool IsCheckedDelegate();
}
