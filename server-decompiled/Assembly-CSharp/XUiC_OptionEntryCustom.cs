using System;
using UnityEngine.Scripting;

// Token: 0x02000DAF RID: 3503
[Preserve]
public class XUiC_OptionEntryCustom : XUiC_OptionEntryAbs
{
	// Token: 0x17000B6F RID: 2927
	// (get) Token: 0x060069DC RID: 27100 RVA: 0x0029DAE2 File Offset: 0x0029BCE2
	public override bool IsChanged
	{
		get
		{
			Func<bool> isChangedDelegate = this.IsChangedDelegate;
			return isChangedDelegate != null && isChangedDelegate();
		}
	}

	// Token: 0x17000B70 RID: 2928
	// (get) Token: 0x060069DD RID: 27101 RVA: 0x0029DAF5 File Offset: 0x0029BCF5
	public override bool IsDefault
	{
		get
		{
			Func<bool> isDefaultDelegate = this.IsDefaultDelegate;
			return isDefaultDelegate == null || isDefaultDelegate();
		}
	}

	// Token: 0x060069DE RID: 27102 RVA: 0x0029DB08 File Offset: 0x0029BD08
	[XuiBindEvent("OnValueChangedGeneric", "comboGeneric")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnComboValueChanged(XUiController _sender)
	{
		this.SelectionChanged();
	}

	// Token: 0x060069DF RID: 27103 RVA: 0x0029DB10 File Offset: 0x0029BD10
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

	// Token: 0x060069E0 RID: 27104 RVA: 0x0029DB60 File Offset: 0x0029BD60
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

	// Token: 0x060069E1 RID: 27105 RVA: 0x0029DB98 File Offset: 0x0029BD98
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

	// Token: 0x060069E2 RID: 27106 RVA: 0x0029DBF0 File Offset: 0x0029BDF0
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

	// Token: 0x060069E3 RID: 27107 RVA: 0x0029DC69 File Offset: 0x0029BE69
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

	// Token: 0x04005179 RID: 20857
	public Func<bool> IsChangedDelegate;

	// Token: 0x0400517A RID: 20858
	public Func<bool> IsDefaultDelegate;

	// Token: 0x0400517B RID: 20859
	public Action GetSettingValue;

	// Token: 0x0400517C RID: 20860
	public Action DiscardChanges;

	// Token: 0x0400517D RID: 20861
	public XUiC_OptionEntryCustom.ApplyChangesDelegate ApplyChanges;

	// Token: 0x0400517E RID: 20862
	public Action ResetDefaults;

	// Token: 0x02000DB0 RID: 3504
	// (Invoke) Token: 0x060069E6 RID: 27110
	public delegate void ApplyChangesDelegate(bool _isImmediateApply);
}
