using System;
using UnityEngine.Scripting;

// Token: 0x02000DA6 RID: 3494
[Preserve]
public abstract class XUiC_OptionEntryGamePrefAbs : XUiC_OptionEntryComboAbs
{
	// Token: 0x17000B6B RID: 2923
	// (get) Token: 0x060069C4 RID: 27076 RVA: 0x0029C763 File Offset: 0x0029A963
	public EnumGamePrefs? GamePref
	{
		get
		{
			return this.gamePref;
		}
	}

	// Token: 0x17000B6C RID: 2924
	// (get) Token: 0x060069C5 RID: 27077
	public abstract GamePrefs.EnumType expectedPrefType
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get;
	}

	// Token: 0x17000B6D RID: 2925
	// (get) Token: 0x060069C6 RID: 27078
	// (set) Token: 0x060069C7 RID: 27079
	public abstract object SelectedValue { get; set; }

	// Token: 0x060069C8 RID: 27080 RVA: 0x0029C76B File Offset: 0x0029A96B
	public override void Init()
	{
		base.Init();
		this.parseGamePref();
	}

	// Token: 0x060069C9 RID: 27081 RVA: 0x0029C77C File Offset: 0x0029A97C
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

	// Token: 0x060069CA RID: 27082
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void applySelectionInternal();

	// Token: 0x060069CB RID: 27083 RVA: 0x0029C7D4 File Offset: 0x0029A9D4
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

	// Token: 0x060069CC RID: 27084 RVA: 0x0029C900 File Offset: 0x0029AB00
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_OptionEntryGamePrefAbs()
	{
	}

	// Token: 0x0400515A RID: 20826
	public Func<bool> DoSaveOverride;

	// Token: 0x0400515B RID: 20827
	[PublicizedFrom(EAccessModifier.Protected)]
	public EnumGamePrefs? gamePref;
}
