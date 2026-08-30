using System;
using System.Collections.Generic;
using InControl;
using Platform;

// Token: 0x020011B9 RID: 4537
public static class GameOptionsControls
{
	// Token: 0x17001134 RID: 4404
	// (get) Token: 0x06009116 RID: 37142 RVA: 0x0036B6D5 File Offset: 0x003698D5
	public static IEnumerable<ValueTuple<PlayerActionsBase, string>> ActionSetPrefs
	{
		get
		{
			foreach (PlayerActionsBase playerActionsBase in PlatformManager.NativePlatform.Input.ActionSets)
			{
				yield return new ValueTuple<PlayerActionsBase, string>(playerActionsBase, "ActionSet_" + playerActionsBase.Name);
			}
			IEnumerator<PlayerActionsBase> enumerator = null;
			yield break;
			yield break;
		}
	}

	// Token: 0x06009117 RID: 37143 RVA: 0x0036B6E0 File Offset: 0x003698E0
	public static void Save()
	{
		foreach (ValueTuple<PlayerActionsBase, string> valueTuple in GameOptionsControls.ActionSetPrefs)
		{
			PlayerActionsBase item = valueTuple.Item1;
			SdPlayerPrefs.SetString(valueTuple.Item2, item.Save());
		}
		SdPlayerPrefs.SetString("Controls", GameOptionsControls.Export());
		SdPlayerPrefs.SetInt("ActionSetsSaved", 0);
		GameOptionsControls.ApplyAllowControllerOption();
	}

	// Token: 0x06009118 RID: 37144 RVA: 0x0036B75C File Offset: 0x0036995C
	public static void Load()
	{
		bool flag = SdPlayerPrefs.HasKey("ActionSetsSaved");
		if (!flag && SdPlayerPrefs.HasKey("Controls"))
		{
			string @string = SdPlayerPrefs.GetString("Controls", string.Empty);
			PlatformManager.NativePlatform.Input.LoadActionSetsFromStrings(@string.Split(';', StringSplitOptions.None));
			GameOptionsControls.Save();
			GameOptionsControls.ApplyAllowControllerOption();
			GameOptionsControls.RestoreNonBindableControllerActionsToDefaults();
			Log.Out("Legacy controls data converted");
			return;
		}
		if (flag)
		{
			foreach (ValueTuple<PlayerActionsBase, string> valueTuple in GameOptionsControls.ActionSetPrefs)
			{
				PlayerActionsBase item = valueTuple.Item1;
				string item2 = valueTuple.Item2;
				if (!string.IsNullOrEmpty(SdPlayerPrefs.GetString(item2, string.Empty)))
				{
					item.Load(SdPlayerPrefs.GetString(item2));
				}
				else
				{
					Log.Warning("Loading controls: No data for action set " + item.Name);
				}
			}
		}
		GameOptionsControls.ApplyAllowControllerOption();
		GameOptionsControls.RestoreNonBindableControllerActionsToDefaults();
	}

	// Token: 0x06009119 RID: 37145 RVA: 0x0036B850 File Offset: 0x00369A50
	public static string Export()
	{
		string[] array = new string[PlatformManager.NativePlatform.Input.ActionSets.Count];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = PlatformManager.NativePlatform.Input.ActionSets[i].Save();
		}
		return string.Join(";", array);
	}

	// Token: 0x0600911A RID: 37146 RVA: 0x0036B8AD File Offset: 0x00369AAD
	public static void Import(string importString)
	{
		PlatformManager.NativePlatform.Input.LoadActionSetsFromStrings(importString.Split(';', StringSplitOptions.None));
		GameOptionsControls.ApplyAllowControllerOption();
		GameOptionsControls.RestoreNonBindableControllerActionsToDefaults();
	}

	// Token: 0x0600911B RID: 37147 RVA: 0x0036B8D4 File Offset: 0x00369AD4
	[PublicizedFrom(EAccessModifier.Private)]
	public static void ApplyAllowControllerOption()
	{
		bool @bool = GamePrefs.GetBool(EnumGamePrefs.OptionsAllowController);
		for (int i = 0; i < PlatformManager.NativePlatform.Input.ActionSets.Count; i++)
		{
			PlatformManager.NativePlatform.Input.ActionSets[i].Device = (@bool ? null : InputDevice.Null);
		}
	}

	// Token: 0x0600911C RID: 37148 RVA: 0x0036B930 File Offset: 0x00369B30
	[PublicizedFrom(EAccessModifier.Private)]
	public static void RestoreNonBindableControllerActionsToDefaults()
	{
		PlatformManager.NativePlatform.Input.GetActionSetForName("gui").ResetControllerBindings();
		PlatformManager.NativePlatform.Input.GetActionSetForName("permanent").ResetControllerBindings();
	}

	// Token: 0x04006B20 RID: 27424
	public const string cActionSetSavePrefix = "ActionSet_";

	// Token: 0x04006B21 RID: 27425
	public const string ActionSetSaveModeFlagPref = "ActionSetsSaved";

	// Token: 0x04006B22 RID: 27426
	public const string LegacyControlsPref = "Controls";
}
