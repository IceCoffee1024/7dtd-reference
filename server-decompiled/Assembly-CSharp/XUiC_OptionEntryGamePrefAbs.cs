using System;
using UnityEngine.Scripting;

// Token: 0x02000DB1 RID: 3505
[Preserve]
public abstract class XUiC_OptionEntryGamePrefAbs : XUiC_OptionEntryComboAbs
{
	// Token: 0x17000B71 RID: 2929
	// (get) Token: 0x060069E9 RID: 27113 RVA: 0x0029DC8B File Offset: 0x0029BE8B
	public EnumGamePrefs? GamePref
	{
		get
		{
			return this.gamePref;
		}
	}

	// Token: 0x17000B72 RID: 2930
	// (get) Token: 0x060069EA RID: 27114
	public abstract GamePrefs.EnumType expectedPrefType
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get;
	}

	// Token: 0x17000B73 RID: 2931
	// (get) Token: 0x060069EB RID: 27115
	// (set) Token: 0x060069EC RID: 27116
	public abstract object SelectedValue { get; set; }

	// Token: 0x060069ED RID: 27117 RVA: 0x0029DC93 File Offset: 0x0029BE93
	public override void Init()
	{
		base.Init();
		this.parseGamePref();
	}

	// Token: 0x060069EE RID: 27118 RVA: 0x0029DCA4 File Offset: 0x0029BEA4
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

	// Token: 0x060069EF RID: 27119
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void applySelectionInternal();

	// Token: 0x060069F0 RID: 27120 RVA: 0x0029DCFC File Offset: 0x0029BEFC
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

	// Token: 0x060069F1 RID: 27121 RVA: 0x0029DE28 File Offset: 0x0029C028
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_OptionEntryGamePrefAbs()
	{
	}

	// Token: 0x0400517F RID: 20863
	public Func<bool> DoSaveOverride;

	// Token: 0x04005180 RID: 20864
	[PublicizedFrom(EAccessModifier.Protected)]
	public EnumGamePrefs? gamePref;
}
