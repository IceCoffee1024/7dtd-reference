using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020001E3 RID: 483
[Preserve]
public class ConsoleCmdAIDirectorShowNextWanderingHordeTime : ConsoleCmdAbstract
{
	// Token: 0x06000ED9 RID: 3801 RVA: 0x00061034 File Offset: 0x0005F234
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "shownexthordetime" };
	}

	// Token: 0x06000EDA RID: 3802 RVA: 0x00061044 File Offset: 0x0005F244
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (GameManager.Instance.World.aiDirector != null)
		{
			GameManager.Instance.World.aiDirector.GetComponent<AIDirectorWanderingHordeComponent>().LogTimes();
		}
	}

	// Token: 0x06000EDB RID: 3803 RVA: 0x00061070 File Offset: 0x0005F270
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Displays the wandering horde time";
	}
}
