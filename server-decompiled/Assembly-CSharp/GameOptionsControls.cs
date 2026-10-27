using System;
using System.Collections.Generic;
using InControl;
using Platform;

// Token: 0x020011FD RID: 4605
public static class GameOptionsControls
{
	// Token: 0x170011BE RID: 4542
	// (get) Token: 0x060092FE RID: 37630 RVA: 0x0037732D File Offset: 0x0037552D
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

	// Token: 0x060092FF RID: 37631 RVA: 0x00377338 File Offset: 0x00375538
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

	// Token: 0x06009300 RID: 37632 RVA: 0x003773C4 File Offset: 0x003755C4
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

	// Token: 0x06009301 RID: 37633 RVA: 0x003774B8 File Offset: 0x003756B8
	public static string Export()
	{
		string[] array = new string[PlatformManager.NativePlatform.Input.ActionSets.Count];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = PlatformManager.NativePlatform.Input.ActionSets[i].Save();
		}
		return string.Join(";", array);
	}

	// Token: 0x06009302 RID: 37634 RVA: 0x00377515 File Offset: 0x00375715
	public static void Import(string importString)
	{
		PlatformManager.NativePlatform.Input.LoadActionSetsFromStrings(importString.Split(';', StringSplitOptions.None));
		GameOptionsControls.ApplyAllowControllerOption();
		GameOptionsControls.RestoreNonBindableControllerActionsToDefaults();
	}

	// Token: 0x06009303 RID: 37635 RVA: 0x0037753C File Offset: 0x0037573C
	[PublicizedFrom(EAccessModifier.Private)]
	public static void ApplyAllowControllerOption()
	{
		bool @bool = GamePrefs.GetBool(EnumGamePrefs.OptionsAllowController);
		for (int i = 0; i < PlatformManager.NativePlatform.Input.ActionSets.Count; i++)
		{
			PlatformManager.NativePlatform.Input.ActionSets[i].Device = (@bool ? null : InputDevice.Null);
		}
	}

	// Token: 0x06009304 RID: 37636 RVA: 0x00377598 File Offset: 0x00375798
	[PublicizedFrom(EAccessModifier.Private)]
	public static void RestoreNonBindableControllerActionsToDefaults()
	{
		PlatformManager.NativePlatform.Input.GetActionSetForName("gui").ResetControllerBindings();
		PlatformManager.NativePlatform.Input.GetActionSetForName("permanent").ResetControllerBindings();
	}

	// Token: 0x04006C7A RID: 27770
	public const string cActionSetSavePrefix = "ActionSet_";

	// Token: 0x04006C7B RID: 27771
	public const string ActionSetSaveModeFlagPref = "ActionSetsSaved";

	// Token: 0x04006C7C RID: 27772
	public const string LegacyControlsPref = "Controls";
}
