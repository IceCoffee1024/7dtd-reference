using System;
using System.Collections.Generic;
using InControl;
using Platform;
using UnityEngine;

// Token: 0x02000556 RID: 1366
public static class InputUtils
{
	// Token: 0x06002CA1 RID: 11425 RVA: 0x0011A8FC File Offset: 0x00118AFC
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

	// Token: 0x170004BA RID: 1210
	// (get) Token: 0x06002CA2 RID: 11426 RVA: 0x0011AA90 File Offset: 0x00118C90
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

	// Token: 0x170004BB RID: 1211
	// (get) Token: 0x06002CA3 RID: 11427 RVA: 0x0011AACD File Offset: 0x00118CCD
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

	// Token: 0x170004BC RID: 1212
	// (get) Token: 0x06002CA4 RID: 11428 RVA: 0x0011AB07 File Offset: 0x00118D07
	public static bool ShiftKeyPressed
	{
		get
		{
			return Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
		}
	}

	// Token: 0x170004BD RID: 1213
	// (get) Token: 0x06002CA5 RID: 11429 RVA: 0x0011AB21 File Offset: 0x00118D21
	public static bool AltKeyPressed
	{
		get
		{
			return Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt);
		}
	}

	// Token: 0x0400224B RID: 8779
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Dictionary<PlayerActionSet, bool> previousState = new Dictionary<PlayerActionSet, bool>();

	// Token: 0x0400224C RID: 8780
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool? isMac;
}
