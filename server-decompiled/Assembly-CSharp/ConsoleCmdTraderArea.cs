using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020002B3 RID: 691
[Preserve]
public class ConsoleCmdTraderArea : ConsoleCmdAbstract
{
	// Token: 0x060013EF RID: 5103 RVA: 0x0007984E File Offset: 0x00077A4E
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "traderarea" };
	}

	// Token: 0x060013F0 RID: 5104 RVA: 0x0007985E File Offset: 0x00077A5E
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "...";
	}

	// Token: 0x060013F1 RID: 5105 RVA: 0x00079868 File Offset: 0x00077A68
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (_params.Count > 0)
		{
			bool flag = StringParsers.ParseBool(_params[0], 0, -1, true);
			for (int i = 0; i < GameManager.Instance.World.TraderAreas.Count; i++)
			{
				GameManager.Instance.World.TraderAreas[i].SetClosed(GameManager.Instance.World, flag, null, false);
			}
			return;
		}
		for (int j = 0; j < GameManager.Instance.World.TraderAreas.Count; j++)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output(string.Format("TraderArea: Position: {0} - IsClosed: {1}", GameManager.Instance.World.TraderAreas[j].Position, GameManager.Instance.World.TraderAreas[j].IsClosed));
		}
	}
}
