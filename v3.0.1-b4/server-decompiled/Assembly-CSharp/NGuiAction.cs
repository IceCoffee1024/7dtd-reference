using System;
using InControl;

// Token: 0x0200126F RID: 4719
public class NGuiAction
{
	// Token: 0x060095D5 RID: 38357 RVA: 0x00387010 File Offset: 0x00385210
	public NGuiAction()
	{
	}

	// Token: 0x060095D6 RID: 38358 RVA: 0x0038701F File Offset: 0x0038521F
	public NGuiAction(string _text, PlayerAction _hotkey)
		: this(_text, null, false)
	{
		this.hotkey = _hotkey;
	}

	// Token: 0x060095D7 RID: 38359 RVA: 0x00387031 File Offset: 0x00385231
	public NGuiAction(string _text, string _icon, bool _isToggle)
		: this(_text, _icon, null, _isToggle, null)
	{
	}

	// Token: 0x060095D8 RID: 38360 RVA: 0x0038703E File Offset: 0x0038523E
	public NGuiAction(string _text, string _icon, string _description, bool _isToggle, PlayerAction _hotkey)
	{
		this.text = _text;
		this.icon = _icon;
		this.description = _description;
		this.hotkey = _hotkey;
		this.bToggle = _isToggle;
		this.bEnabled = true;
	}

	// Token: 0x060095D9 RID: 38361 RVA: 0x00387079 File Offset: 0x00385279
	public virtual void OnClick()
	{
		if (this.IsEnabled() && this.clickActionDelegate != null)
		{
			this.clickActionDelegate();
		}
		this.UpdateUI();
	}

	// Token: 0x060095DA RID: 38362 RVA: 0x0038709C File Offset: 0x0038529C
	public virtual void OnRelease()
	{
		if (this.IsEnabled() && this.releaseActionDelegate != null)
		{
			this.releaseActionDelegate();
		}
		this.UpdateUI();
	}

	// Token: 0x060095DB RID: 38363 RVA: 0x003870BF File Offset: 0x003852BF
	public virtual void OnDoubleClick()
	{
		if (this.IsEnabled() && this.doubleClickActionDelegate != null)
		{
			this.doubleClickActionDelegate();
		}
		this.UpdateUI();
	}

	// Token: 0x060095DC RID: 38364 RVA: 0x003870E2 File Offset: 0x003852E2
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

	// Token: 0x060095DD RID: 38365 RVA: 0x00387115 File Offset: 0x00385315
	public virtual bool IsActive()
	{
		return this.isVisibleDelegate == null || this.isVisibleDelegate();
	}

	// Token: 0x060095DE RID: 38366 RVA: 0x0038712C File Offset: 0x0038532C
	public virtual string GetIcon()
	{
		return this.icon;
	}

	// Token: 0x060095DF RID: 38367 RVA: 0x00387134 File Offset: 0x00385334
	public virtual string GetText()
	{
		return this.text;
	}

	// Token: 0x060095E0 RID: 38368 RVA: 0x0038713C File Offset: 0x0038533C
	public void SetText(string _text)
	{
		this.text = _text;
		this.UpdateUI();
	}

	// Token: 0x060095E1 RID: 38369 RVA: 0x0038714B File Offset: 0x0038534B
	public virtual string GetTooltip()
	{
		return this.tooltip;
	}

	// Token: 0x060095E2 RID: 38370 RVA: 0x00387153 File Offset: 0x00385353
	public NGuiAction SetTooltip(string _tooltip)
	{
		this.tooltip = (string.IsNullOrEmpty(_tooltip) ? null : Localization.Get(_tooltip, false, null));
		return this;
	}

	// Token: 0x060095E3 RID: 38371 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual int GetColumnCount()
	{
		return 0;
	}

	// Token: 0x060095E4 RID: 38372 RVA: 0x0001FF1E File Offset: 0x0001E11E
	public virtual string GetColumnIcon(int _col)
	{
		return null;
	}

	// Token: 0x060095E5 RID: 38373 RVA: 0x0001FF1E File Offset: 0x0001E11E
	public virtual string GetColumnText(int _col)
	{
		return null;
	}

	// Token: 0x060095E6 RID: 38374 RVA: 0x0038716F File Offset: 0x0038536F
	public virtual string GetDescription()
	{
		return this.description;
	}

	// Token: 0x060095E7 RID: 38375 RVA: 0x00387177 File Offset: 0x00385377
	public virtual NGuiAction SetDescription(string _desc)
	{
		this.description = _desc;
		return this;
	}

	// Token: 0x060095E8 RID: 38376 RVA: 0x00387181 File Offset: 0x00385381
	public virtual PlayerAction GetHotkey()
	{
		return this.hotkey;
	}

	// Token: 0x060095E9 RID: 38377 RVA: 0x00387189 File Offset: 0x00385389
	public virtual NGuiAction SetEnabled(bool _bEnabled)
	{
		this.bEnabled = _bEnabled;
		this.UpdateUI();
		return this;
	}

	// Token: 0x060095EA RID: 38378 RVA: 0x00387199 File Offset: 0x00385399
	public virtual bool IsEnabled()
	{
		if (this.isEnabledDelegate != null)
		{
			return this.isEnabledDelegate();
		}
		return this.bEnabled;
	}

	// Token: 0x060095EB RID: 38379 RVA: 0x003871B5 File Offset: 0x003853B5
	public virtual bool IsToggle()
	{
		return this.bToggle;
	}

	// Token: 0x060095EC RID: 38380 RVA: 0x003871BD File Offset: 0x003853BD
	public virtual void SetChecked(bool _bChecked)
	{
		this.bChecked = _bChecked;
		this.UpdateUI();
	}

	// Token: 0x060095ED RID: 38381 RVA: 0x003871CC File Offset: 0x003853CC
	public virtual bool IsChecked()
	{
		if (this.isCheckedDelegate != null)
		{
			return this.isCheckedDelegate();
		}
		return this.bChecked;
	}

	// Token: 0x060095EE RID: 38382 RVA: 0x003871E8 File Offset: 0x003853E8
	public virtual NGuiAction SetIsCheckedDelegate(NGuiAction.IsCheckedDelegate _checkedDelegate)
	{
		this.isCheckedDelegate = _checkedDelegate;
		return this;
	}

	// Token: 0x060095EF RID: 38383 RVA: 0x003871F2 File Offset: 0x003853F2
	public virtual NGuiAction SetIsVisibleDelegate(NGuiAction.IsVisibleDelegate _isVisibleDelegate)
	{
		this.isVisibleDelegate = _isVisibleDelegate;
		return this;
	}

	// Token: 0x060095F0 RID: 38384 RVA: 0x003871FC File Offset: 0x003853FC
	public virtual NGuiAction SetIsEnabledDelegate(NGuiAction.IsEnabledDelegate _isEnabledDelegate)
	{
		this.isEnabledDelegate = _isEnabledDelegate;
		return this;
	}

	// Token: 0x060095F1 RID: 38385 RVA: 0x00387206 File Offset: 0x00385406
	public virtual NGuiAction SetClickActionDelegate(NGuiAction.OnClickActionDelegate _actionDelegate)
	{
		this.clickActionDelegate = _actionDelegate;
		return this;
	}

	// Token: 0x060095F2 RID: 38386 RVA: 0x00387210 File Offset: 0x00385410
	public virtual NGuiAction SetReleaseActionDelegate(NGuiAction.OnReleaseActionDelegate _actionDelegate)
	{
		this.releaseActionDelegate = _actionDelegate;
		return this;
	}

	// Token: 0x060095F3 RID: 38387 RVA: 0x0038721A File Offset: 0x0038541A
	public virtual NGuiAction SetDoubleClickActionDelegate(NGuiAction.OnDoubleClickActionDelegate _actionDelegate)
	{
		this.doubleClickActionDelegate = _actionDelegate;
		return this;
	}

	// Token: 0x060095F4 RID: 38388 RVA: 0x00387224 File Offset: 0x00385424
	public virtual NGuiAction SetSelectActionDelegate(NGuiAction.OnSelectActionDelegate _selectActionDelegate)
	{
		this.selectActionDelegate = _selectActionDelegate;
		return this;
	}

	// Token: 0x060095F5 RID: 38389 RVA: 0x000027FC File Offset: 0x000009FC
	public void UpdateUI()
	{
	}

	// Token: 0x060095F6 RID: 38390 RVA: 0x0038722E File Offset: 0x0038542E
	public override string ToString()
	{
		if (this.text == null)
		{
			return string.Empty;
		}
		return this.text;
	}

	// Token: 0x04007020 RID: 28704
	[PublicizedFrom(EAccessModifier.Private)]
	public PlayerAction hotkey;

	// Token: 0x04007021 RID: 28705
	public static NGuiAction Separator = new NGuiAction("Sep", null);

	// Token: 0x04007022 RID: 28706
	[PublicizedFrom(EAccessModifier.Private)]
	public string text;

	// Token: 0x04007023 RID: 28707
	[PublicizedFrom(EAccessModifier.Private)]
	public string icon;

	// Token: 0x04007024 RID: 28708
	[PublicizedFrom(EAccessModifier.Private)]
	public string description;

	// Token: 0x04007025 RID: 28709
	[PublicizedFrom(EAccessModifier.Private)]
	public string tooltip;

	// Token: 0x04007026 RID: 28710
	[PublicizedFrom(EAccessModifier.Private)]
	public NGuiAction.OnClickActionDelegate clickActionDelegate;

	// Token: 0x04007027 RID: 28711
	[PublicizedFrom(EAccessModifier.Private)]
	public NGuiAction.OnReleaseActionDelegate releaseActionDelegate;

	// Token: 0x04007028 RID: 28712
	[PublicizedFrom(EAccessModifier.Private)]
	public NGuiAction.OnDoubleClickActionDelegate doubleClickActionDelegate;

	// Token: 0x04007029 RID: 28713
	[PublicizedFrom(EAccessModifier.Private)]
	public NGuiAction.OnSelectActionDelegate selectActionDelegate;

	// Token: 0x0400702A RID: 28714
	[PublicizedFrom(EAccessModifier.Private)]
	public NGuiAction.IsVisibleDelegate isVisibleDelegate;

	// Token: 0x0400702B RID: 28715
	[PublicizedFrom(EAccessModifier.Private)]
	public NGuiAction.IsCheckedDelegate isCheckedDelegate;

	// Token: 0x0400702C RID: 28716
	[PublicizedFrom(EAccessModifier.Private)]
	public NGuiAction.IsEnabledDelegate isEnabledDelegate;

	// Token: 0x0400702D RID: 28717
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bEnabled;

	// Token: 0x0400702E RID: 28718
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bToggle;

	// Token: 0x0400702F RID: 28719
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bChecked;

	// Token: 0x04007030 RID: 28720
	public NGuiAction.EnumKeyMode KeyMode = NGuiAction.EnumKeyMode.FireOnPress;

	// Token: 0x02001270 RID: 4720
	[Flags]
	public enum EnumKeyMode
	{
		// Token: 0x04007032 RID: 28722
		None = 0,
		// Token: 0x04007033 RID: 28723
		FireOnPress = 1,
		// Token: 0x04007034 RID: 28724
		FireOnRelease = 2,
		// Token: 0x04007035 RID: 28725
		FireOnRepeat = 4
	}

	// Token: 0x02001271 RID: 4721
	// (Invoke) Token: 0x060095F9 RID: 38393
	public delegate void OnClickActionDelegate();

	// Token: 0x02001272 RID: 4722
	// (Invoke) Token: 0x060095FD RID: 38397
	public delegate void OnReleaseActionDelegate();

	// Token: 0x02001273 RID: 4723
	// (Invoke) Token: 0x06009601 RID: 38401
	public delegate void OnDoubleClickActionDelegate();

	// Token: 0x02001274 RID: 4724
	// (Invoke) Token: 0x06009605 RID: 38405
	public delegate void OnSelectActionDelegate(bool _bSelected);

	// Token: 0x02001275 RID: 4725
	// (Invoke) Token: 0x06009609 RID: 38409
	public delegate bool IsEnabledDelegate();

	// Token: 0x02001276 RID: 4726
	// (Invoke) Token: 0x0600960D RID: 38413
	public delegate bool IsVisibleDelegate();

	// Token: 0x02001277 RID: 4727
	// (Invoke) Token: 0x06009611 RID: 38417
	public delegate bool IsCheckedDelegate();
}
