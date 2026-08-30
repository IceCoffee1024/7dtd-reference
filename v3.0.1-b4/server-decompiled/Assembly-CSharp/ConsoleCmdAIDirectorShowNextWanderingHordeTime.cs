using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020001E0 RID: 480
[Preserve]
public class ConsoleCmdAIDirectorShowNextWanderingHordeTime : ConsoleCmdAbstract
{
	// Token: 0x06000EDB RID: 3803 RVA: 0x00060AFC File Offset: 0x0005ECFC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "shownexthordetime" };
	}

	// Token: 0x06000EDC RID: 3804 RVA: 0x00060B0C File Offset: 0x0005ED0C
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (GameManager.Instance.World.aiDirector != null)
		{
			GameManager.Instance.World.aiDirector.GetComponent<AIDirectorWanderingHordeComponent>().LogTimes();
		}
	}

	// Token: 0x06000EDD RID: 3805 RVA: 0x00060B38 File Offset: 0x0005ED38
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Displays the wandering horde time";
	}
}
