using System;
using InControl;

// Token: 0x02001289 RID: 4745
public class NGuiAction
{
	// Token: 0x060096D0 RID: 38608 RVA: 0x0038C16C File Offset: 0x0038A36C
	public NGuiAction()
	{
	}

	// Token: 0x060096D1 RID: 38609 RVA: 0x0038C17B File Offset: 0x0038A37B
	public NGuiAction(string _text, PlayerAction _hotkey)
		: this(_text, null, false)
	{
		this.hotkey = _hotkey;
	}

	// Token: 0x060096D2 RID: 38610 RVA: 0x0038C18D File Offset: 0x0038A38D
	public NGuiAction(string _text, string _icon, bool _isToggle)
		: this(_text, _icon, null, _isToggle, null)
	{
	}

	// Token: 0x060096D3 RID: 38611 RVA: 0x0038C19A File Offset: 0x0038A39A
	public NGuiAction(string _text, string _icon, string _description, bool _isToggle, PlayerAction _hotkey)
	{
		this.text = _text;
		this.icon = _icon;
		this.description = _description;
		this.hotkey = _hotkey;
		this.bToggle = _isToggle;
		this.bEnabled = true;
	}

	// Token: 0x060096D4 RID: 38612 RVA: 0x0038C1D5 File Offset: 0x0038A3D5
	public virtual void OnClick()
	{
		if (this.IsEnabled() && this.clickActionDelegate != null)
		{
			this.clickActionDelegate();
		}
		this.UpdateUI();
	}

	// Token: 0x060096D5 RID: 38613 RVA: 0x0038C1F8 File Offset: 0x0038A3F8
	public virtual void OnRelease()
	{
		if (this.IsEnabled() && this.releaseActionDelegate != null)
		{
			this.releaseActionDelegate();
		}
		this.UpdateUI();
	}

	// Token: 0x060096D6 RID: 38614 RVA: 0x0038C21B File Offset: 0x0038A41B
	public virtual void OnDoubleClick()
	{
		if (this.IsEnabled() && this.doubleClickActionDelegate != null)
		{
			this.doubleClickActionDelegate();
		}
		this.UpdateUI();
	}

	// Token: 0x060096D7 RID: 38615 RVA: 0x0038C23E File Offset: 0x0038A43E
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

	// Token: 0x060096D8 RID: 38616 RVA: 0x0038C271 File Offset: 0x0038A471
	public virtual bool IsActive()
	{
		return this.isVisibleDelegate == null || this.isVisibleDelegate();
	}

	// Token: 0x060096D9 RID: 38617 RVA: 0x0038C288 File Offset: 0x0038A488
	public virtual string GetIcon()
	{
		return this.icon;
	}

	// Token: 0x060096DA RID: 38618 RVA: 0x0038C290 File Offset: 0x0038A490
	public virtual string GetText()
	{
		return this.text;
	}

	// Token: 0x060096DB RID: 38619 RVA: 0x0038C298 File Offset: 0x0038A498
	public void SetText(string _text)
	{
		this.text = _text;
		this.UpdateUI();
	}

	// Token: 0x060096DC RID: 38620 RVA: 0x0038C2A7 File Offset: 0x0038A4A7
	public virtual string GetTooltip()
	{
		return this.tooltip;
	}

	// Token: 0x060096DD RID: 38621 RVA: 0x0038C2AF File Offset: 0x0038A4AF
	public NGuiAction SetTooltip(string _tooltip)
	{
		this.tooltip = (string.IsNullOrEmpty(_tooltip) ? null : Localization.Get(_tooltip, false, null));
		return this;
	}

	// Token: 0x060096DE RID: 38622 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual int GetColumnCount()
	{
		return 0;
	}

	// Token: 0x060096DF RID: 38623 RVA: 0x0001FFE2 File Offset: 0x0001E1E2
	public virtual string GetColumnIcon(int _col)
	{
		return null;
	}

	// Token: 0x060096E0 RID: 38624 RVA: 0x0001FFE2 File Offset: 0x0001E1E2
	public virtual string GetColumnText(int _col)
	{
		return null;
	}

	// Token: 0x060096E1 RID: 38625 RVA: 0x0038C2CB File Offset: 0x0038A4CB
	public virtual string GetDescription()
	{
		return this.description;
	}

	// Token: 0x060096E2 RID: 38626 RVA: 0x0038C2D3 File Offset: 0x0038A4D3
	public virtual NGuiAction SetDescription(string _desc)
	{
		this.description = _desc;
		return this;
	}

	// Token: 0x060096E3 RID: 38627 RVA: 0x0038C2DD File Offset: 0x0038A4DD
	public virtual PlayerAction GetHotkey()
	{
		return this.hotkey;
	}

	// Token: 0x060096E4 RID: 38628 RVA: 0x0038C2E5 File Offset: 0x0038A4E5
	public virtual NGuiAction SetEnabled(bool _bEnabled)
	{
		this.bEnabled = _bEnabled;
		this.UpdateUI();
		return this;
	}

	// Token: 0x060096E5 RID: 38629 RVA: 0x0038C2F5 File Offset: 0x0038A4F5
	public virtual bool IsEnabled()
	{
		if (this.isEnabledDelegate != null)
		{
			return this.isEnabledDelegate();
		}
		return this.bEnabled;
	}

	// Token: 0x060096E6 RID: 38630 RVA: 0x0038C311 File Offset: 0x0038A511
	public virtual bool IsToggle()
	{
		return this.bToggle;
	}

	// Token: 0x060096E7 RID: 38631 RVA: 0x0038C319 File Offset: 0x0038A519
	public virtual void SetChecked(bool _bChecked)
	{
		this.bChecked = _bChecked;
		this.UpdateUI();
	}

	// Token: 0x060096E8 RID: 38632 RVA: 0x0038C328 File Offset: 0x0038A528
	public virtual bool IsChecked()
	{
		if (this.isCheckedDelegate != null)
		{
			return this.isCheckedDelegate();
		}
		return this.bChecked;
	}

	// Token: 0x060096E9 RID: 38633 RVA: 0x0038C344 File Offset: 0x0038A544
	public virtual NGuiAction SetIsCheckedDelegate(NGuiAction.IsCheckedDelegate _checkedDelegate)
	{
		this.isCheckedDelegate = _checkedDelegate;
		return this;
	}

	// Token: 0x060096EA RID: 38634 RVA: 0x0038C34E File Offset: 0x0038A54E
	public virtual NGuiAction SetIsVisibleDelegate(NGuiAction.IsVisibleDelegate _isVisibleDelegate)
	{
		this.isVisibleDelegate = _isVisibleDelegate;
		return this;
	}

	// Token: 0x060096EB RID: 38635 RVA: 0x0038C358 File Offset: 0x0038A558
	public virtual NGuiAction SetIsEnabledDelegate(NGuiAction.IsEnabledDelegate _isEnabledDelegate)
	{
		this.isEnabledDelegate = _isEnabledDelegate;
		return this;
	}

	// Token: 0x060096EC RID: 38636 RVA: 0x0038C362 File Offset: 0x0038A562
	public virtual NGuiAction SetClickActionDelegate(NGuiAction.OnClickActionDelegate _actionDelegate)
	{
		this.clickActionDelegate = _actionDelegate;
		return this;
	}

	// Token: 0x060096ED RID: 38637 RVA: 0x0038C36C File Offset: 0x0038A56C
	public virtual NGuiAction SetReleaseActionDelegate(NGuiAction.OnReleaseActionDelegate _actionDelegate)
	{
		this.releaseActionDelegate = _actionDelegate;
		return this;
	}

	// Token: 0x060096EE RID: 38638 RVA: 0x0038C376 File Offset: 0x0038A576
	public virtual NGuiAction SetDoubleClickActionDelegate(NGuiAction.OnDoubleClickActionDelegate _actionDelegate)
	{
		this.doubleClickActionDelegate = _actionDelegate;
		return this;
	}

	// Token: 0x060096EF RID: 38639 RVA: 0x0038C380 File Offset: 0x0038A580
	public virtual NGuiAction SetSelectActionDelegate(NGuiAction.OnSelectActionDelegate _selectActionDelegate)
	{
		this.selectActionDelegate = _selectActionDelegate;
		return this;
	}

	// Token: 0x060096F0 RID: 38640 RVA: 0x000027FC File Offset: 0x000009FC
	public void UpdateUI()
	{
	}

	// Token: 0x060096F1 RID: 38641 RVA: 0x0038C38A File Offset: 0x0038A58A
	public override string ToString()
	{
		if (this.text == null)
		{
			return string.Empty;
		}
		return this.text;
	}

	// Token: 0x040070F5 RID: 28917
	[PublicizedFrom(EAccessModifier.Private)]
	public PlayerAction hotkey;

	// Token: 0x040070F6 RID: 28918
	public static NGuiAction Separator = new NGuiAction("Sep", null);

	// Token: 0x040070F7 RID: 28919
	[PublicizedFrom(EAccessModifier.Private)]
	public string text;

	// Token: 0x040070F8 RID: 28920
	[PublicizedFrom(EAccessModifier.Private)]
	public string icon;

	// Token: 0x040070F9 RID: 28921
	[PublicizedFrom(EAccessModifier.Private)]
	public string description;

	// Token: 0x040070FA RID: 28922
	[PublicizedFrom(EAccessModifier.Private)]
	public string tooltip;

	// Token: 0x040070FB RID: 28923
	[PublicizedFrom(EAccessModifier.Private)]
	public NGuiAction.OnClickActionDelegate clickActionDelegate;

	// Token: 0x040070FC RID: 28924
	[PublicizedFrom(EAccessModifier.Private)]
	public NGuiAction.OnReleaseActionDelegate releaseActionDelegate;

	// Token: 0x040070FD RID: 28925
	[PublicizedFrom(EAccessModifier.Private)]
	public NGuiAction.OnDoubleClickActionDelegate doubleClickActionDelegate;

	// Token: 0x040070FE RID: 28926
	[PublicizedFrom(EAccessModifier.Private)]
	public NGuiAction.OnSelectActionDelegate selectActionDelegate;

	// Token: 0x040070FF RID: 28927
	[PublicizedFrom(EAccessModifier.Private)]
	public NGuiAction.IsVisibleDelegate isVisibleDelegate;

	// Token: 0x04007100 RID: 28928
	[PublicizedFrom(EAccessModifier.Private)]
	public NGuiAction.IsCheckedDelegate isCheckedDelegate;

	// Token: 0x04007101 RID: 28929
	[PublicizedFrom(EAccessModifier.Private)]
	public NGuiAction.IsEnabledDelegate isEnabledDelegate;

	// Token: 0x04007102 RID: 28930
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bEnabled;

	// Token: 0x04007103 RID: 28931
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bToggle;

	// Token: 0x04007104 RID: 28932
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bChecked;

	// Token: 0x04007105 RID: 28933
	public NGuiAction.EnumKeyMode KeyMode = NGuiAction.EnumKeyMode.FireOnPress;

	// Token: 0x0200128A RID: 4746
	[Flags]
	public enum EnumKeyMode
	{
		// Token: 0x04007107 RID: 28935
		None = 0,
		// Token: 0x04007108 RID: 28936
		FireOnPress = 1,
		// Token: 0x04007109 RID: 28937
		FireOnRelease = 2,
		// Token: 0x0400710A RID: 28938
		FireOnRepeat = 4
	}

	// Token: 0x0200128B RID: 4747
	// (Invoke) Token: 0x060096F4 RID: 38644
	public delegate void OnClickActionDelegate();

	// Token: 0x0200128C RID: 4748
	// (Invoke) Token: 0x060096F8 RID: 38648
	public delegate void OnReleaseActionDelegate();

	// Token: 0x0200128D RID: 4749
	// (Invoke) Token: 0x060096FC RID: 38652
	public delegate void OnDoubleClickActionDelegate();

	// Token: 0x0200128E RID: 4750
	// (Invoke) Token: 0x06009700 RID: 38656
	public delegate void OnSelectActionDelegate(bool _bSelected);

	// Token: 0x0200128F RID: 4751
	// (Invoke) Token: 0x06009704 RID: 38660
	public delegate bool IsEnabledDelegate();

	// Token: 0x02001290 RID: 4752
	// (Invoke) Token: 0x06009708 RID: 38664
	public delegate bool IsVisibleDelegate();

	// Token: 0x02001291 RID: 4753
	// (Invoke) Token: 0x0600970C RID: 38668
	public delegate bool IsCheckedDelegate();
}
