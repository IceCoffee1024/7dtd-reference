using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020002AF RID: 687
[Preserve]
public class ConsoleCmdTraderArea : ConsoleCmdAbstract
{
	// Token: 0x060013DD RID: 5085 RVA: 0x000791F2 File Offset: 0x000773F2
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "traderarea" };
	}

	// Token: 0x060013DE RID: 5086 RVA: 0x00079202 File Offset: 0x00077402
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "...";
	}

	// Token: 0x060013DF RID: 5087 RVA: 0x0007920C File Offset: 0x0007740C
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
