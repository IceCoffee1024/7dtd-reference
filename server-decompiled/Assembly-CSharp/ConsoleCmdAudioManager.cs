using System;
using System.Collections.Generic;
using Audio;
using UnityEngine.Scripting;

// Token: 0x020001E8 RID: 488
[Preserve]
public class ConsoleCmdAudioManager : ConsoleCmdAbstract
{
	// Token: 0x06000EEF RID: 3823 RVA: 0x000614D3 File Offset: 0x0005F6D3
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "audio" };
	}

	// Token: 0x17000132 RID: 306
	// (get) Token: 0x06000EF0 RID: 3824 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06000EF1 RID: 3825 RVA: 0x000614E3 File Offset: 0x0005F6E3
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Watch audio stats";
	}

	// Token: 0x06000EF2 RID: 3826 RVA: 0x000614EA File Offset: 0x0005F6EA
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "Just type audio and hit enter for the info.\n";
	}

	// Token: 0x06000EF3 RID: 3827 RVA: 0x000614F1 File Offset: 0x0005F6F1
	[PublicizedFrom(EAccessModifier.Private)]
	public void DisplayHelp()
	{
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("No help yet");
	}

	// Token: 0x06000EF4 RID: 3828 RVA: 0x00061504 File Offset: 0x0005F704
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
