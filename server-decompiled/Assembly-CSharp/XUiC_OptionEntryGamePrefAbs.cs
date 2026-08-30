using System;
using UnityEngine.Scripting;

// Token: 0x02000D83 RID: 3459
[Preserve]
public abstract class XUiC_OptionEntryGamePrefAbs : XUiC_OptionEntryComboAbs
{
	// Token: 0x17000B38 RID: 2872
	// (get) Token: 0x06006930 RID: 26928 RVA: 0x00296283 File Offset: 0x00294483
	public EnumGamePrefs? GamePref
	{
		get
		{
			return this.gamePref;
		}
	}

	// Token: 0x17000B39 RID: 2873
	// (get) Token: 0x06006931 RID: 26929
	public abstract GamePrefs.EnumType expectedPrefType
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get;
	}

	// Token: 0x17000B3A RID: 2874
	// (get) Token: 0x06006932 RID: 26930
	// (set) Token: 0x06006933 RID: 26931
	public abstract object SelectedValue { get; set; }

	// Token: 0x06006934 RID: 26932 RVA: 0x0029628B File Offset: 0x0029448B
	public override void Init()
	{
		base.Init();
		this.parseGamePref();
	}

	// Token: 0x06006935 RID: 26933 RVA: 0x0029629C File Offset: 0x0029449C
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

	// Token: 0x06006936 RID: 26934
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void applySelectionInternal();

	// Token: 0x06006937 RID: 26935 RVA: 0x002962F4 File Offset: 0x002944F4
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

	// Token: 0x06006938 RID: 26936 RVA: 0x00296420 File Offset: 0x00294620
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_OptionEntryGamePrefAbs()
	{
	}

	// Token: 0x04005081 RID: 20609
	public Func<bool> DoSaveOverride;

	// Token: 0x04005082 RID: 20610
	[PublicizedFrom(EAccessModifier.Protected)]
	public EnumGamePrefs? gamePref;
}
