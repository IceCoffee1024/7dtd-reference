using System;
using System.Collections.Generic;
using Audio;
using UnityEngine.Scripting;

// Token: 0x020001E5 RID: 485
[Preserve]
public class ConsoleCmdAudioManager : ConsoleCmdAbstract
{
	// Token: 0x06000EF1 RID: 3825 RVA: 0x00060F9B File Offset: 0x0005F19B
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "audio" };
	}

	// Token: 0x1700013A RID: 314
	// (get) Token: 0x06000EF2 RID: 3826 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06000EF3 RID: 3827 RVA: 0x00060FAB File Offset: 0x0005F1AB
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Watch audio stats";
	}

	// Token: 0x06000EF4 RID: 3828 RVA: 0x00060FB2 File Offset: 0x0005F1B2
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "Just type audio and hit enter for the info.\n";
	}

	// Token: 0x06000EF5 RID: 3829 RVA: 0x00060FB9 File Offset: 0x0005F1B9
	[PublicizedFrom(EAccessModifier.Private)]
	public void DisplayHelp()
	{
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("No help yet");
	}

	// Token: 0x06000EF6 RID: 3830 RVA: 0x00060FCC File Offset: 0x0005F1CC
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
