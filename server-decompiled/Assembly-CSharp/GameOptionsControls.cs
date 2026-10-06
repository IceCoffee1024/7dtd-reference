using System;
using System.Collections.Generic;
using InControl;
using Platform;

// Token: 0x020011ED RID: 4589
public static class GameOptionsControls
{
	// Token: 0x170011B3 RID: 4531
	// (get) Token: 0x060092A8 RID: 37544 RVA: 0x003751B9 File Offset: 0x003733B9
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

	// Token: 0x060092A9 RID: 37545 RVA: 0x003751C4 File Offset: 0x003733C4
	public static void Save()
	{
		foreach (ValueTuple<PlayerActionsBase, string> valueTuple in GameOptionsControls.ActionSetPrefs)
		{
			PlayerActionsBase item = valueTuple.Item1;
			SdPlayerPrefs.SetString(valueTuple.Item2, item.Save());
		}
		try
		{
			SdPlayerPrefs.SetString("Controls", GameOptionsControls.Export());
		}
		catch (Exception)
		{
		}
		SdPlayerPrefs.SetInt("ActionSetsSaved", 0);
		GameOptionsControls.ApplyAllowControllerOption();
	}

	// Token: 0x060092AA RID: 37546 RVA: 0x00375250 File Offset: 0x00373450
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

	// Token: 0x060092AB RID: 37547 RVA: 0x00375344 File Offset: 0x00373544
	public static string Export()
	{
		string[] array = new string[PlatformManager.NativePlatform.Input.ActionSets.Count];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = PlatformManager.NativePlatform.Input.ActionSets[i].Save();
		}
		return string.Join(";", array);
	}

	// Token: 0x060092AC RID: 37548 RVA: 0x003753A1 File Offset: 0x003735A1
	public static void Import(string importString)
	{
		PlatformManager.NativePlatform.Input.LoadActionSetsFromStrings(importString.Split(';', StringSplitOptions.None));
		GameOptionsControls.ApplyAllowControllerOption();
		GameOptionsControls.RestoreNonBindableControllerActionsToDefaults();
	}

	// Token: 0x060092AD RID: 37549 RVA: 0x003753C8 File Offset: 0x003735C8
	[PublicizedFrom(EAccessModifier.Private)]
	public static void ApplyAllowControllerOption()
	{
		bool @bool = GamePrefs.GetBool(EnumGamePrefs.OptionsAllowController);
		for (int i = 0; i < PlatformManager.NativePlatform.Input.ActionSets.Count; i++)
		{
			PlatformManager.NativePlatform.Input.ActionSets[i].Device = (@bool ? null : InputDevice.Null);
		}
	}

	// Token: 0x060092AE RID: 37550 RVA: 0x00375424 File Offset: 0x00373624
	[PublicizedFrom(EAccessModifier.Private)]
	public static void RestoreNonBindableControllerActionsToDefaults()
	{
		PlatformManager.NativePlatform.Input.GetActionSetForName("gui").ResetControllerBindings();
		PlatformManager.NativePlatform.Input.GetActionSetForName("permanent").ResetControllerBindings();
	}

	// Token: 0x04006C62 RID: 27746
	public const string cActionSetSavePrefix = "ActionSet_";

	// Token: 0x04006C63 RID: 27747
	public const string ActionSetSaveModeFlagPref = "ActionSetsSaved";

	// Token: 0x04006C64 RID: 27748
	public const string LegacyControlsPref = "Controls";
}
