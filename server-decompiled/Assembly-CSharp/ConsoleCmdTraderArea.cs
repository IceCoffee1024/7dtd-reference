using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020002B6 RID: 694
[Preserve]
public class ConsoleCmdTraderArea : ConsoleCmdAbstract
{
	// Token: 0x060013EC RID: 5100 RVA: 0x00079B02 File Offset: 0x00077D02
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "traderarea" };
	}

	// Token: 0x060013ED RID: 5101 RVA: 0x00079B12 File Offset: 0x00077D12
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "...";
	}

	// Token: 0x060013EE RID: 5102 RVA: 0x00079B1C File Offset: 0x00077D1C
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
