using System;
using UnityEngine.Scripting;

// Token: 0x02000DA4 RID: 3492
[Preserve]
public class XUiC_OptionEntryCustom : XUiC_OptionEntryAbs
{
	// Token: 0x17000B69 RID: 2921
	// (get) Token: 0x060069B7 RID: 27063 RVA: 0x0029C5BA File Offset: 0x0029A7BA
	public override bool IsChanged
	{
		get
		{
			Func<bool> isChangedDelegate = this.IsChangedDelegate;
			return isChangedDelegate != null && isChangedDelegate();
		}
	}

	// Token: 0x17000B6A RID: 2922
	// (get) Token: 0x060069B8 RID: 27064 RVA: 0x0029C5CD File Offset: 0x0029A7CD
	public override bool IsDefault
	{
		get
		{
			Func<bool> isDefaultDelegate = this.IsDefaultDelegate;
			return isDefaultDelegate == null || isDefaultDelegate();
		}
	}

	// Token: 0x060069B9 RID: 27065 RVA: 0x0029C5E0 File Offset: 0x0029A7E0
	[XuiBindEvent("OnValueChangedGeneric", "comboGeneric")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnComboValueChanged(XUiController _sender)
	{
		this.SelectionChanged();
	}

	// Token: 0x060069BA RID: 27066 RVA: 0x0029C5E8 File Offset: 0x0029A7E8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void initCurrentValue()
	{
		if (this.GetSettingValue == null)
		{
			Log.Error("[XUi] " + base.GetType().Name + ": Can not init current value, delegate not assigned. Hierarchy: " + base.GetXuiHierarchy());
			return;
		}
		this.GetSettingValue();
		XUiC_OptionEntryAbs.DebugLog("GOT CURRENT CUSTOMVALUE FOR ");
	}

	// Token: 0x060069BB RID: 27067 RVA: 0x0029C638 File Offset: 0x0029A838
	public override void DiscardCurrentChange()
	{
		Action discardChanges = this.DiscardChanges;
		if (discardChanges != null)
		{
			discardChanges();
		}
		if (base.ApplyImmediately)
		{
			XUiC_OptionEntryCustom.ApplyChangesDelegate applyChanges = this.ApplyChanges;
			if (applyChanges != null)
			{
				applyChanges(true);
			}
		}
		XUiC_OptionEntryAbs.DebugLog("DISCARDED CUSTOMVALUE FOR ");
	}

	// Token: 0x060069BC RID: 27068 RVA: 0x0029C670 File Offset: 0x0029A870
	public override void ApplySelection()
	{
		if (this.ApplyChanges == null)
		{
			Log.Error("[XUi] " + base.GetType().Name + ": Can not apply changes, delegate not assigned. Hierarchy: " + base.GetXuiHierarchy());
			return;
		}
		this.ApplyChanges(false);
		XUiC_OptionEntryAbs.DebugLog("SAVED CUSTOMVALUE FOR ");
		this.IsDirty = true;
	}

	// Token: 0x060069BD RID: 27069 RVA: 0x0029C6C8 File Offset: 0x0029A8C8
	public override void ResetToDefault()
	{
		if (!base.ApplyDefaults)
		{
			return;
		}
		if (this.ResetDefaults == null)
		{
			Log.Error("[XUi] " + base.GetType().Name + ": Can not reset to defaults, delegate not assigned. Hierarchy: " + base.GetXuiHierarchy());
			return;
		}
		this.ResetDefaults();
		if (base.ApplyImmediately)
		{
			XUiC_OptionEntryCustom.ApplyChangesDelegate applyChanges = this.ApplyChanges;
			if (applyChanges != null)
			{
				applyChanges(true);
			}
		}
		XUiC_OptionEntryAbs.DebugLog("RESET CUSTOMVALUE FOR ");
		base.invokeValueChanged();
	}

	// Token: 0x060069BE RID: 27070 RVA: 0x0029C741 File Offset: 0x0029A941
	public void SelectionChanged()
	{
		if (base.ApplyImmediately)
		{
			XUiC_OptionEntryCustom.ApplyChangesDelegate applyChanges = this.ApplyChanges;
			if (applyChanges != null)
			{
				applyChanges(true);
			}
		}
		base.invokeValueChanged();
	}

	// Token: 0x04005154 RID: 20820
	public Func<bool> IsChangedDelegate;

	// Token: 0x04005155 RID: 20821
	public Func<bool> IsDefaultDelegate;

	// Token: 0x04005156 RID: 20822
	public Action GetSettingValue;

	// Token: 0x04005157 RID: 20823
	public Action DiscardChanges;

	// Token: 0x04005158 RID: 20824
	public XUiC_OptionEntryCustom.ApplyChangesDelegate ApplyChanges;

	// Token: 0x04005159 RID: 20825
	public Action ResetDefaults;

	// Token: 0x02000DA5 RID: 3493
	// (Invoke) Token: 0x060069C1 RID: 27073
	public delegate void ApplyChangesDelegate(bool _isImmediateApply);
}
