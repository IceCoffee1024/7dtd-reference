using System;
using System.Collections.Generic;
using InControl;
using Platform;
using UnityEngine;

// Token: 0x02000551 RID: 1361
public static class InputUtils
{
	// Token: 0x06002C77 RID: 11383 RVA: 0x00119888 File Offset: 0x00117A88
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

	// Token: 0x170004B6 RID: 1206
	// (get) Token: 0x06002C78 RID: 11384 RVA: 0x00119A1C File Offset: 0x00117C1C
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

	// Token: 0x170004B7 RID: 1207
	// (get) Token: 0x06002C79 RID: 11385 RVA: 0x00119A59 File Offset: 0x00117C59
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

	// Token: 0x170004B8 RID: 1208
	// (get) Token: 0x06002C7A RID: 11386 RVA: 0x00119A93 File Offset: 0x00117C93
	public static bool ShiftKeyPressed
	{
		get
		{
			return Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
		}
	}

	// Token: 0x170004B9 RID: 1209
	// (get) Token: 0x06002C7B RID: 11387 RVA: 0x00119AAD File Offset: 0x00117CAD
	public static bool AltKeyPressed
	{
		get
		{
			return Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt);
		}
	}

	// Token: 0x04002219 RID: 8729
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Dictionary<PlayerActionSet, bool> previousState = new Dictionary<PlayerActionSet, bool>();

	// Token: 0x0400221A RID: 8730
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool? isMac;
}
