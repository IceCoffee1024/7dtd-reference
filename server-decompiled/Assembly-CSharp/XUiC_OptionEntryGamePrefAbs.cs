using System;
using UnityEngine.Scripting;

// Token: 0x02000D97 RID: 3479
[Preserve]
public abstract class XUiC_OptionEntryGamePrefAbs : XUiC_OptionEntryComboAbs
{
	// Token: 0x17000B50 RID: 2896
	// (get) Token: 0x060069CC RID: 27084 RVA: 0x002993D7 File Offset: 0x002975D7
	public EnumGamePrefs? GamePref
	{
		get
		{
			return this.gamePref;
		}
	}

	// Token: 0x17000B51 RID: 2897
	// (get) Token: 0x060069CD RID: 27085
	public abstract GamePrefs.EnumType expectedPrefType
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get;
	}

	// Token: 0x17000B52 RID: 2898
	// (get) Token: 0x060069CE RID: 27086
	// (set) Token: 0x060069CF RID: 27087
	public abstract object SelectedValue { get; set; }

	// Token: 0x060069D0 RID: 27088 RVA: 0x002993DF File Offset: 0x002975DF
	public override void Init()
	{
		base.Init();
		this.parseGamePref();
	}

	// Token: 0x060069D1 RID: 27089 RVA: 0x002993F0 File Offset: 0x002975F0
	public sealed override void ApplySelection()
	{
		if (this.gamePref == null)
		{
			return;
		}
		if (this.DoSaveOverride != null && !this.DoSaveOverride())
		{
			XUiC_OptionEntryAbs.DebugLog(string.Format("*NOT* SAVING VALUE FOR {0}, disabled by override", this.gamePref));
			return;
		}
		this.applySelectionInternal();
		this.IsDirty = true;
	}

	// Token: 0x060069D2 RID: 27090
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void applySelectionInternal();

	// Token: 0x060069D3 RID: 27091 RVA: 0x00299448 File Offset: 0x00297648
	[PublicizedFrom(EAccessModifier.Private)]
	public void parseGamePref()
	{
		if (string.IsNullOrEmpty(base.PrefName))
		{
			Log.Error("[XUi] " + base.GetType().Name + ": No GamePref specified. Hierarchy: " + base.GetXuiHierarchy());
			return;
		}
		EnumGamePrefs enumGamePrefs;
		if (!EnumUtils.TryParse<EnumGamePrefs>(base.PrefName, out enumGamePrefs, true))
		{
			Log.Error(string.Concat(new string[]
			{
				"[XUi] ",
				base.GetType().Name,
				": Given GamePref '",
				base.PrefName,
				"' does not exist. Hierarchy: ",
				base.GetXuiHierarchy()
			}));
			return;
		}
		GamePrefs.EnumType? prefType = GamePrefs.GetPrefType(enumGamePrefs);
		if (prefType == null)
		{
			Log.Error(string.Format("[XUi] {0}: Given GamePref '{1}' does not have a pref type. Hierarchy: {2}", base.GetType().Name, enumGamePrefs, base.GetXuiHierarchy()));
			return;
		}
		GamePrefs.EnumType expectedPrefType = this.expectedPrefType;
		if (prefType.Value != expectedPrefType)
		{
			Log.Error(string.Format("[XUi] {0}: Given GamePref '{1}' is not a {2} pref. Hierarchy: {3}", new object[]
			{
				base.GetType().Name,
				enumGamePrefs,
				expectedPrefType.ToStringCached<GamePrefs.EnumType>(),
				base.GetXuiHierarchy()
			}));
			return;
		}
		this.gamePref = new EnumGamePrefs?(enumGamePrefs);
	}

	// Token: 0x060069D4 RID: 27092 RVA: 0x00299574 File Offset: 0x00297774
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_OptionEntryGamePrefAbs()
	{
	}

	// Token: 0x04005123 RID: 20771
	public Func<bool> DoSaveOverride;

	// Token: 0x04005124 RID: 20772
	[PublicizedFrom(EAccessModifier.Protected)]
	public EnumGamePrefs? gamePref;
}
