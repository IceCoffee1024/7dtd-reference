using System;
using InControl;

// Token: 0x020012B3 RID: 4787
public class NGuiAction
{
	// Token: 0x060097B5 RID: 38837 RVA: 0x00392DBC File Offset: 0x00390FBC
	public NGuiAction()
	{
	}

	// Token: 0x060097B6 RID: 38838 RVA: 0x00392DCB File Offset: 0x00390FCB
	public NGuiAction(string _text, PlayerAction _hotkey)
		: this(_text, null, false)
	{
		this.hotkey = _hotkey;
	}

	// Token: 0x060097B7 RID: 38839 RVA: 0x00392DDD File Offset: 0x00390FDD
	public NGuiAction(string _text, string _icon, bool _isToggle)
		: this(_text, _icon, null, _isToggle, null)
	{
	}

	// Token: 0x060097B8 RID: 38840 RVA: 0x00392DEA File Offset: 0x00390FEA
	public NGuiAction(string _text, string _icon, string _description, bool _isToggle, PlayerAction _hotkey)
	{
		this.text = _text;
		this.icon = _icon;
		this.description = _description;
		this.hotkey = _hotkey;
		this.bToggle = _isToggle;
		this.bEnabled = true;
	}

	// Token: 0x060097B9 RID: 38841 RVA: 0x00392E25 File Offset: 0x00391025
	public virtual void OnClick()
	{
		if (this.IsEnabled() && this.clickActionDelegate != null)
		{
			this.clickActionDelegate();
		}
		this.UpdateUI();
	}

	// Token: 0x060097BA RID: 38842 RVA: 0x00392E48 File Offset: 0x00391048
	public virtual void OnRelease()
	{
		if (this.IsEnabled() && this.releaseActionDelegate != null)
		{
			this.releaseActionDelegate();
		}
		this.UpdateUI();
	}

	// Token: 0x060097BB RID: 38843 RVA: 0x00392E6B File Offset: 0x0039106B
	public virtual void OnDoubleClick()
	{
		if (this.IsEnabled() && this.doubleClickActionDelegate != null)
		{
			this.doubleClickActionDelegate();
		}
		this.UpdateUI();
	}

	// Token: 0x060097BC RID: 38844 RVA: 0x00392E8E File Offset: 0x0039108E
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

	// Token: 0x060097BD RID: 38845 RVA: 0x00392EC1 File Offset: 0x003910C1
	public virtual bool IsActive()
	{
		return this.isVisibleDelegate == null || this.isVisibleDelegate();
	}

	// Token: 0x060097BE RID: 38846 RVA: 0x00392ED8 File Offset: 0x003910D8
	public virtual string GetIcon()
	{
		return this.icon;
	}

	// Token: 0x060097BF RID: 38847 RVA: 0x00392EE0 File Offset: 0x003910E0
	public virtual string GetText()
	{
		return this.text;
	}

	// Token: 0x060097C0 RID: 38848 RVA: 0x00392EE8 File Offset: 0x003910E8
	public void SetText(string _text)
	{
		this.text = _text;
		this.UpdateUI();
	}

	// Token: 0x060097C1 RID: 38849 RVA: 0x00392EF7 File Offset: 0x003910F7
	public virtual string GetTooltip()
	{
		return this.tooltip;
	}

	// Token: 0x060097C2 RID: 38850 RVA: 0x00392EFF File Offset: 0x003910FF
	public NGuiAction SetTooltip(string _tooltip)
	{
		this.tooltip = (string.IsNullOrEmpty(_tooltip) ? null : Localization.Get(_tooltip, false, null));
		return this;
	}

	// Token: 0x060097C3 RID: 38851 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual int GetColumnCount()
	{
		return 0;
	}

	// Token: 0x060097C4 RID: 38852 RVA: 0x000200DA File Offset: 0x0001E2DA
	public virtual string GetColumnIcon(int _col)
	{
		return null;
	}

	// Token: 0x060097C5 RID: 38853 RVA: 0x000200DA File Offset: 0x0001E2DA
	public virtual string GetColumnText(int _col)
	{
		return null;
	}

	// Token: 0x060097C6 RID: 38854 RVA: 0x00392F1B File Offset: 0x0039111B
	public virtual string GetDescription()
	{
		return this.description;
	}

	// Token: 0x060097C7 RID: 38855 RVA: 0x00392F23 File Offset: 0x00391123
	public virtual NGuiAction SetDescription(string _desc)
	{
		this.description = _desc;
		return this;
	}

	// Token: 0x060097C8 RID: 38856 RVA: 0x00392F2D File Offset: 0x0039112D
	public virtual PlayerAction GetHotkey()
	{
		return this.hotkey;
	}

	// Token: 0x060097C9 RID: 38857 RVA: 0x00392F35 File Offset: 0x00391135
	public virtual NGuiAction SetEnabled(bool _bEnabled)
	{
		this.bEnabled = _bEnabled;
		this.UpdateUI();
		return this;
	}

	// Token: 0x060097CA RID: 38858 RVA: 0x00392F45 File Offset: 0x00391145
	public virtual bool IsEnabled()
	{
		if (this.isEnabledDelegate != null)
		{
			return this.isEnabledDelegate();
		}
		return this.bEnabled;
	}

	// Token: 0x060097CB RID: 38859 RVA: 0x00392F61 File Offset: 0x00391161
	public virtual bool IsToggle()
	{
		return this.bToggle;
	}

	// Token: 0x060097CC RID: 38860 RVA: 0x00392F69 File Offset: 0x00391169
	public virtual void SetChecked(bool _bChecked)
	{
		this.bChecked = _bChecked;
		this.UpdateUI();
	}

	// Token: 0x060097CD RID: 38861 RVA: 0x00392F78 File Offset: 0x00391178
	public virtual bool IsChecked()
	{
		if (this.isCheckedDelegate != null)
		{
			return this.isCheckedDelegate();
		}
		return this.bChecked;
	}

	// Token: 0x060097CE RID: 38862 RVA: 0x00392F94 File Offset: 0x00391194
	public virtual NGuiAction SetIsCheckedDelegate(NGuiAction.IsCheckedDelegate _checkedDelegate)
	{
		this.isCheckedDelegate = _checkedDelegate;
		return this;
	}

	// Token: 0x060097CF RID: 38863 RVA: 0x00392F9E File Offset: 0x0039119E
	public virtual NGuiAction SetIsVisibleDelegate(NGuiAction.IsVisibleDelegate _isVisibleDelegate)
	{
		this.isVisibleDelegate = _isVisibleDelegate;
		return this;
	}

	// Token: 0x060097D0 RID: 38864 RVA: 0x00392FA8 File Offset: 0x003911A8
	public virtual NGuiAction SetIsEnabledDelegate(NGuiAction.IsEnabledDelegate _isEnabledDelegate)
	{
		this.isEnabledDelegate = _isEnabledDelegate;
		return this;
	}

	// Token: 0x060097D1 RID: 38865 RVA: 0x00392FB2 File Offset: 0x003911B2
	public virtual NGuiAction SetClickActionDelegate(NGuiAction.OnClickActionDelegate _actionDelegate)
	{
		this.clickActionDelegate = _actionDelegate;
		return this;
	}

	// Token: 0x060097D2 RID: 38866 RVA: 0x00392FBC File Offset: 0x003911BC
	public virtual NGuiAction SetReleaseActionDelegate(NGuiAction.OnReleaseActionDelegate _actionDelegate)
	{
		this.releaseActionDelegate = _actionDelegate;
		return this;
	}

	// Token: 0x060097D3 RID: 38867 RVA: 0x00392FC6 File Offset: 0x003911C6
	public virtual NGuiAction SetDoubleClickActionDelegate(NGuiAction.OnDoubleClickActionDelegate _actionDelegate)
	{
		this.doubleClickActionDelegate = _actionDelegate;
		return this;
	}

	// Token: 0x060097D4 RID: 38868 RVA: 0x00392FD0 File Offset: 0x003911D0
	public virtual NGuiAction SetSelectActionDelegate(NGuiAction.OnSelectActionDelegate _selectActionDelegate)
	{
		this.selectActionDelegate = _selectActionDelegate;
		return this;
	}

	// Token: 0x060097D5 RID: 38869 RVA: 0x000027FC File Offset: 0x000009FC
	public void UpdateUI()
	{
	}

	// Token: 0x060097D6 RID: 38870 RVA: 0x00392FDA File Offset: 0x003911DA
	public override string ToString()
	{
		if (this.text == null)
		{
			return string.Empty;
		}
		return this.text;
	}

	// Token: 0x040071A0 RID: 29088
	[PublicizedFrom(EAccessModifier.Private)]
	public PlayerAction hotkey;

	// Token: 0x040071A1 RID: 29089
	public static NGuiAction Separator = new NGuiAction("Sep", null);

	// Token: 0x040071A2 RID: 29090
	[PublicizedFrom(EAccessModifier.Private)]
	public string text;

	// Token: 0x040071A3 RID: 29091
	[PublicizedFrom(EAccessModifier.Private)]
	public string icon;

	// Token: 0x040071A4 RID: 29092
	[PublicizedFrom(EAccessModifier.Private)]
	public string description;

	// Token: 0x040071A5 RID: 29093
	[PublicizedFrom(EAccessModifier.Private)]
	public string tooltip;

	// Token: 0x040071A6 RID: 29094
	[PublicizedFrom(EAccessModifier.Private)]
	public NGuiAction.OnClickActionDelegate clickActionDelegate;

	// Token: 0x040071A7 RID: 29095
	[PublicizedFrom(EAccessModifier.Private)]
	public NGuiAction.OnReleaseActionDelegate releaseActionDelegate;

	// Token: 0x040071A8 RID: 29096
	[PublicizedFrom(EAccessModifier.Private)]
	public NGuiAction.OnDoubleClickActionDelegate doubleClickActionDelegate;

	// Token: 0x040071A9 RID: 29097
	[PublicizedFrom(EAccessModifier.Private)]
	public NGuiAction.OnSelectActionDelegate selectActionDelegate;

	// Token: 0x040071AA RID: 29098
	[PublicizedFrom(EAccessModifier.Private)]
	public NGuiAction.IsVisibleDelegate isVisibleDelegate;

	// Token: 0x040071AB RID: 29099
	[PublicizedFrom(EAccessModifier.Private)]
	public NGuiAction.IsCheckedDelegate isCheckedDelegate;

	// Token: 0x040071AC RID: 29100
	[PublicizedFrom(EAccessModifier.Private)]
	public NGuiAction.IsEnabledDelegate isEnabledDelegate;

	// Token: 0x040071AD RID: 29101
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bEnabled;

	// Token: 0x040071AE RID: 29102
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bToggle;

	// Token: 0x040071AF RID: 29103
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bChecked;

	// Token: 0x040071B0 RID: 29104
	public NGuiAction.EnumKeyMode KeyMode = NGuiAction.EnumKeyMode.FireOnPress;

	// Token: 0x020012B4 RID: 4788
	[Flags]
	public enum EnumKeyMode
	{
		// Token: 0x040071B2 RID: 29106
		None = 0,
		// Token: 0x040071B3 RID: 29107
		FireOnPress = 1,
		// Token: 0x040071B4 RID: 29108
		FireOnRelease = 2,
		// Token: 0x040071B5 RID: 29109
		FireOnRepeat = 4
	}

	// Token: 0x020012B5 RID: 4789
	// (Invoke) Token: 0x060097D9 RID: 38873
	public delegate void OnClickActionDelegate();

	// Token: 0x020012B6 RID: 4790
	// (Invoke) Token: 0x060097DD RID: 38877
	public delegate void OnReleaseActionDelegate();

	// Token: 0x020012B7 RID: 4791
	// (Invoke) Token: 0x060097E1 RID: 38881
	public delegate void OnDoubleClickActionDelegate();

	// Token: 0x020012B8 RID: 4792
	// (Invoke) Token: 0x060097E5 RID: 38885
	public delegate void OnSelectActionDelegate(bool _bSelected);

	// Token: 0x020012B9 RID: 4793
	// (Invoke) Token: 0x060097E9 RID: 38889
	public delegate bool IsEnabledDelegate();

	// Token: 0x020012BA RID: 4794
	// (Invoke) Token: 0x060097ED RID: 38893
	public delegate bool IsVisibleDelegate();

	// Token: 0x020012BB RID: 4795
	// (Invoke) Token: 0x060097F1 RID: 38897
	public delegate bool IsCheckedDelegate();
}
