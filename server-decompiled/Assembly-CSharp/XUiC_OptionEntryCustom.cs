using System;
using UnityEngine.Scripting;

// Token: 0x02000D95 RID: 3477
[Preserve]
public class XUiC_OptionEntryCustom : XUiC_OptionEntryAbs
{
	// Token: 0x17000B4E RID: 2894
	// (get) Token: 0x060069BF RID: 27071 RVA: 0x0029922E File Offset: 0x0029742E
	public override bool IsChanged
	{
		get
		{
			Func<bool> isChangedDelegate = this.IsChangedDelegate;
			return isChangedDelegate != null && isChangedDelegate();
		}
	}

	// Token: 0x17000B4F RID: 2895
	// (get) Token: 0x060069C0 RID: 27072 RVA: 0x00299241 File Offset: 0x00297441
	public override bool IsDefault
	{
		get
		{
			Func<bool> isDefaultDelegate = this.IsDefaultDelegate;
			return isDefaultDelegate == null || isDefaultDelegate();
		}
	}

	// Token: 0x060069C1 RID: 27073 RVA: 0x00299254 File Offset: 0x00297454
	[XuiBindEvent("OnValueChangedGeneric", "comboGeneric")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnComboValueChanged(XUiController _sender)
	{
		this.SelectionChanged();
	}

	// Token: 0x060069C2 RID: 27074 RVA: 0x0029925C File Offset: 0x0029745C
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

	// Token: 0x060069C3 RID: 27075 RVA: 0x002992AC File Offset: 0x002974AC
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

	// Token: 0x060069C4 RID: 27076 RVA: 0x002992E4 File Offset: 0x002974E4
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

	// Token: 0x060069C5 RID: 27077 RVA: 0x0029933C File Offset: 0x0029753C
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

	// Token: 0x060069C6 RID: 27078 RVA: 0x002993B5 File Offset: 0x002975B5
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

	// Token: 0x0400511D RID: 20765
	public Func<bool> IsChangedDelegate;

	// Token: 0x0400511E RID: 20766
	public Func<bool> IsDefaultDelegate;

	// Token: 0x0400511F RID: 20767
	public Action GetSettingValue;

	// Token: 0x04005120 RID: 20768
	public Action DiscardChanges;

	// Token: 0x04005121 RID: 20769
	public XUiC_OptionEntryCustom.ApplyChangesDelegate ApplyChanges;

	// Token: 0x04005122 RID: 20770
	public Action ResetDefaults;

	// Token: 0x02000D96 RID: 3478
	// (Invoke) Token: 0x060069C9 RID: 27081
	public delegate void ApplyChangesDelegate(bool _isImmediateApply);
}
