using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020001E5 RID: 485
[Preserve]
public class ConsoleCmdAIDirectorShowNextWanderingHordeTime : ConsoleCmdAbstract
{
	// Token: 0x06000EDD RID: 3805 RVA: 0x000612F8 File Offset: 0x0005F4F8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "shownexthordetime" };
	}

	// Token: 0x06000EDE RID: 3806 RVA: 0x00061308 File Offset: 0x0005F508
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (GameManager.Instance.World.aiDirector != null)
		{
			GameManager.Instance.World.aiDirector.GetComponent<AIDirectorWanderingHordeComponent>().LogTimes();
		}
	}

	// Token: 0x06000EDF RID: 3807 RVA: 0x00061334 File Offset: 0x0005F534
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Displays the wandering horde time";
	}
}
