using System;
using System.Collections.Generic;
using InControl;
using Platform;
using UnityEngine;

// Token: 0x0200055C RID: 1372
public static class InputUtils
{
	// Token: 0x06002C97 RID: 11415 RVA: 0x0011B6CC File Offset: 0x001198CC
	public static void EnableAllPlayerActions(bool _enable)
	{
		if (ActionSetManager.DebugLevel != ActionSetManager.EDebugLevel.Off)
		{
			Log.Out("EnableAllPlayerActions: " + _enable.ToString());
		}
		if (!_enable)
		{
			InputUtils.previousState.Clear();
			using (IEnumerator<PlayerActionsBase> enumerator = PlatformManager.NativePlatform.Input.ActionSets.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PlayerActionsBase playerActionsBase = enumerator.Current;
					if (ActionSetManager.DebugLevel == ActionSetManager.EDebugLevel.Verbose)
					{
						Log.Out(string.Format("PAS: {0} IsInDict {1}", playerActionsBase, InputUtils.previousState.ContainsKey(playerActionsBase)));
					}
					if (!InputUtils.previousState.ContainsKey(playerActionsBase))
					{
						if (ActionSetManager.DebugLevel == ActionSetManager.EDebugLevel.Verbose)
						{
							Log.Out(string.Format("Disabling: {0} was {1}", playerActionsBase, playerActionsBase.Enabled));
						}
						InputUtils.previousState.Add(playerActionsBase, playerActionsBase.Enabled);
						playerActionsBase.Enabled = false;
					}
				}
				return;
			}
		}
		foreach (PlayerActionsBase playerActionsBase2 in PlatformManager.NativePlatform.Input.ActionSets)
		{
			if (InputUtils.previousState.ContainsKey(playerActionsBase2))
			{
				if (ActionSetManager.DebugLevel == ActionSetManager.EDebugLevel.Verbose)
				{
					Log.Out(string.Format("PrevState contains: {0} was {1}", playerActionsBase2, InputUtils.previousState[playerActionsBase2]));
				}
				playerActionsBase2.Enabled = InputUtils.previousState[playerActionsBase2];
			}
			else
			{
				if (ActionSetManager.DebugLevel == ActionSetManager.EDebugLevel.Verbose)
				{
					Log.Out(string.Format("PrevState does not contain: {0}", playerActionsBase2));
				}
				playerActionsBase2.Enabled = true;
			}
		}
	}

	// Token: 0x170004B0 RID: 1200
	// (get) Token: 0x06002C98 RID: 11416 RVA: 0x0011B860 File Offset: 0x00119A60
	public static bool IsMac
	{
		get
		{
			if (InputUtils.isMac == null)
			{
				RuntimePlatform platform = Application.platform;
				InputUtils.isMac = new bool?(platform == RuntimePlatform.OSXEditor || platform == RuntimePlatform.OSXPlayer);
			}
			return InputUtils.isMac.Value;
		}
	}

	// Token: 0x170004B1 RID: 1201
	// (get) Token: 0x06002C99 RID: 11417 RVA: 0x0011B89D File Offset: 0x00119A9D
	public static bool ControlKeyPressed
	{
		get
		{
			if (!InputUtils.IsMac)
			{
				return Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
			}
			return Input.GetKey(KeyCode.LeftMeta) || Input.GetKey(KeyCode.RightMeta);
		}
	}

	// Token: 0x170004B2 RID: 1202
	// (get) Token: 0x06002C9A RID: 11418 RVA: 0x0011B8D7 File Offset: 0x00119AD7
	public static bool ShiftKeyPressed
	{
		get
		{
			return Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
		}
	}

	// Token: 0x170004B3 RID: 1203
	// (get) Token: 0x06002C9B RID: 11419 RVA: 0x0011B8F1 File Offset: 0x00119AF1
	public static bool AltKeyPressed
	{
		get
		{
			return Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt);
		}
	}

	// Token: 0x04002253 RID: 8787
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Dictionary<PlayerActionSet, bool> previousState = new Dictionary<PlayerActionSet, bool>();

	// Token: 0x04002254 RID: 8788
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool? isMac;
}
