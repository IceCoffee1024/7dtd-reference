using System;
using UnityEngine.Scripting;

// Token: 0x02000D81 RID: 3457
[Preserve]
public class XUiC_OptionEntryCustom : XUiC_OptionEntryAbs
{
	// Token: 0x17000B36 RID: 2870
	// (get) Token: 0x06006923 RID: 26915 RVA: 0x002960DA File Offset: 0x002942DA
	public override bool IsChanged
	{
		get
		{
			Func<bool> isChangedDelegate = this.IsChangedDelegate;
			return isChangedDelegate != null && isChangedDelegate();
		}
	}

	// Token: 0x17000B37 RID: 2871
	// (get) Token: 0x06006924 RID: 26916 RVA: 0x002960ED File Offset: 0x002942ED
	public override bool IsDefault
	{
		get
		{
			Func<bool> isDefaultDelegate = this.IsDefaultDelegate;
			return isDefaultDelegate == null || isDefaultDelegate();
		}
	}

	// Token: 0x06006925 RID: 26917 RVA: 0x00296100 File Offset: 0x00294300
	[XuiBindEvent("OnValueChangedGeneric", "comboGeneric")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnComboValueChanged(XUiController _sender)
	{
		this.SelectionChanged();
	}

	// Token: 0x06006926 RID: 26918 RVA: 0x00296108 File Offset: 0x00294308
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

	// Token: 0x06006927 RID: 26919 RVA: 0x00296158 File Offset: 0x00294358
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

	// Token: 0x06006928 RID: 26920 RVA: 0x00296190 File Offset: 0x00294390
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

	// Token: 0x06006929 RID: 26921 RVA: 0x002961E8 File Offset: 0x002943E8
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

	// Token: 0x0600692A RID: 26922 RVA: 0x00296261 File Offset: 0x00294461
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

	// Token: 0x0400507B RID: 20603
	public Func<bool> IsChangedDelegate;

	// Token: 0x0400507C RID: 20604
	public Func<bool> IsDefaultDelegate;

	// Token: 0x0400507D RID: 20605
	public Action GetSettingValue;

	// Token: 0x0400507E RID: 20606
	public Action DiscardChanges;

	// Token: 0x0400507F RID: 20607
	public XUiC_OptionEntryCustom.ApplyChangesDelegate ApplyChanges;

	// Token: 0x04005080 RID: 20608
	public Action ResetDefaults;

	// Token: 0x02000D82 RID: 3458
	// (Invoke) Token: 0x0600692D RID: 26925
	public delegate void ApplyChangesDelegate(bool _isImmediateApply);
}
