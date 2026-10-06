using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020001F1 RID: 497
[Preserve]
public class ConsoleCmdCamera : ConsoleCmdAbstract
{
	// Token: 0x06000F3D RID: 3901 RVA: 0x000637B3 File Offset: 0x000619B3
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "camera", "cam" };
	}

	// Token: 0x1700013E RID: 318
	// (get) Token: 0x06000F3E RID: 3902 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06000F3F RID: 3903 RVA: 0x000637CB File Offset: 0x000619CB
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Lock/unlock camera movement or load/save a specific camera position";
	}

	// Token: 0x06000F40 RID: 3904 RVA: 0x000637D2 File Offset: 0x000619D2
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "Usage:\n   1. cam save <name> [comment]\n   2. cam load <name>\n   3. cam list\n   4. cam lock\n   5. cam unlock\n1. Save the current player's position and camera view or the camera position\nand view if in detached mode under the given name. Optionally a more descriptive\ncomment can be supplied.\n2. Load the position and direction with the given name. If in detached camera\nmode the camera itself will be adjusted, otherwise the player will be teleported.\n3. List the saved camera positions.\n4/5. Lock/unlock the camera rotation. Can also be achieved with the \"Lock Camera\" key.";
	}

	// Token: 0x06000F41 RID: 3905 RVA: 0x000637DC File Offset: 0x000619DC
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (_params.Count < 1)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("No sub command given.");
			return;
		}
		if (!_senderInfo.IsLocalGame)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Command can only be used on clients");
			return;
		}
		EntityPlayerLocal primaryPlayer = GameManager.Instance.World.GetPrimaryPlayer();
		if (_params[0].EqualsCaseInsensitive("lock"))
		{
			this.ExecuteLock(_params, primaryPlayer);
			return;
		}
		if (_params[0].EqualsCaseInsensitive("unlock"))
		{
			this.ExecuteUnlock(_params, primaryPlayer);
			return;
		}
		if (_params[0].EqualsCaseInsensitive("save"))
		{
			this.ExecuteSave(_params, primaryPlayer);
			return;
		}
		if (_params[0].EqualsCaseInsensitive("load"))
		{
			this.ExecuteLoad(_params, primaryPlayer);
			return;
		}
		if (_params[0].EqualsCaseInsensitive("list"))
		{
			this.ExecuteList(_params);
			return;
		}
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Invalid sub command \"" + _params[0] + "\".");
	}

	// Token: 0x06000F42 RID: 3906 RVA: 0x000638D8 File Offset: 0x00061AD8
	[PublicizedFrom(EAccessModifier.Private)]
	public void ExecuteLock(List<string> _params, EntityPlayerLocal _epl)
	{
		_epl.movementInput.bCameraPositionLocked = true;
	}

	// Token: 0x06000F43 RID: 3907 RVA: 0x000638E6 File Offset: 0x00061AE6
	[PublicizedFrom(EAccessModifier.Private)]
	public void ExecuteUnlock(List<string> _params, EntityPlayerLocal _epl)
	{
		_epl.movementInput.bCameraPositionLocked = false;
	}

	// Token: 0x06000F44 RID: 3908 RVA: 0x000638F4 File Offset: 0x00061AF4
	[PublicizedFrom(EAccessModifier.Private)]
	public void ExecuteSave(List<string> _params, EntityPlayerLocal _epl)
	{
		if (_params.Count < 2)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Command requires a name for the position.");
			return;
		}
		string text = _params[1];
		string text2 = ((_params.Count > 2) ? _params[2] : null);
		CameraPerspectives cameraPerspectives = new CameraPerspectives(true);
		cameraPerspectives.Perspectives[text] = new CameraPerspectives.Perspective(text, _epl, text2);
		cameraPerspectives.Save();
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Position saved with name \"" + text + "\"");
	}

	// Token: 0x06000F45 RID: 3909 RVA: 0x00063970 File Offset: 0x00061B70
	[PublicizedFrom(EAccessModifier.Private)]
	public void ExecuteLoad(List<string> _params, EntityPlayerLocal _epl)
	{
		if (_params.Count < 2)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("No position name given.");
			return;
		}
		CameraPerspectives.Perspective perspective;
		if (!new CameraPerspectives(true).Perspectives.TryGetValue(_params[1], out perspective))
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Position name not found.");
			return;
		}
		perspective.ToPlayer(_epl);
	}

	// Token: 0x06000F46 RID: 3910 RVA: 0x000639C8 File Offset: 0x00061BC8
	[PublicizedFrom(EAccessModifier.Private)]
	public void ExecuteList(List<string> _params)
	{
		CameraPerspectives cameraPerspectives = new CameraPerspectives(true);
		string text = ((_params.Count > 1) ? _params[1] : null);
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Saved camera positions:");
		foreach (KeyValuePair<string, CameraPerspectives.Perspective> keyValuePair in cameraPerspectives.Perspectives)
		{
			string text2;
			CameraPerspectives.Perspective perspective;
			keyValuePair.Deconstruct(out text2, out perspective);
			CameraPerspectives.Perspective perspective2 = perspective;
			if (text == null || perspective2.Name.ContainsCaseInsensitive(text) || perspective2.Comment.ContainsCaseInsensitive(text))
			{
				string text3 = (string.IsNullOrEmpty(perspective2.Comment) ? "" : (" (" + perspective2.Comment + ")"));
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output("  " + perspective2.Name + text3);
			}
		}
	}
}
