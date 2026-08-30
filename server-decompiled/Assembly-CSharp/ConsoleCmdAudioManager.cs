using System;
using System.Collections.Generic;
using Audio;
using UnityEngine.Scripting;

// Token: 0x020001E7 RID: 487
[Preserve]
public class ConsoleCmdAudioManager : ConsoleCmdAbstract
{
	// Token: 0x06000EFC RID: 3836 RVA: 0x000614E3 File Offset: 0x0005F6E3
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "audio" };
	}

	// Token: 0x1700013A RID: 314
	// (get) Token: 0x06000EFD RID: 3837 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06000EFE RID: 3838 RVA: 0x000614F3 File Offset: 0x0005F6F3
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Watch audio stats";
	}

	// Token: 0x06000EFF RID: 3839 RVA: 0x000614FA File Offset: 0x0005F6FA
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "Just type audio and hit enter for the info.\n";
	}

	// Token: 0x06000F00 RID: 3840 RVA: 0x00061501 File Offset: 0x0005F701
	[PublicizedFrom(EAccessModifier.Private)]
	public void DisplayHelp()
	{
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("No help yet");
	}

	// Token: 0x06000F01 RID: 3841 RVA: 0x00061514 File Offset: 0x0005F714
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (_params.Count == 0)
		{
			this.DisplayHelp();
			return;
		}
		if (_params.Count == 2)
		{
			if (_params[0].EqualsCaseInsensitive("occlusion"))
			{
				if (_params[1].EqualsCaseInsensitive("on"))
				{
					Manager.occlusionsOn = true;
					return;
				}
				if (_params[1].EqualsCaseInsensitive("off"))
				{
					Manager.occlusionsOn = false;
					return;
				}
			}
			else
			{
				if (_params[0].EqualsCaseInsensitive("hitdelay"))
				{
					int num = 0;
					int.TryParse(_params[1], out num);
					EntityAlive.HitDelay = (ulong)((long)num);
					return;
				}
				if (_params[0].EqualsCaseInsensitive("hitdis"))
				{
					float num2 = 0f;
					StringParsers.TryParseFloat(_params[1], out num2);
					EntityAlive.HitSoundDistance = num2;
					return;
				}
				if (_params[0].EqualsCaseInsensitive("play"))
				{
					Manager.Play(GameManager.Instance.World.GetPrimaryPlayer(), _params[1], 1f, false);
					return;
				}
			}
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Invalid Input");
			this.DisplayHelp();
		}
	}
}
