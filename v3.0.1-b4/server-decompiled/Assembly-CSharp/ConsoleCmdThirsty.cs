using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020002AE RID: 686
[Preserve]
public class ConsoleCmdThirsty : ConsoleCmdAbstract
{
	// Token: 0x17000225 RID: 549
	// (get) Token: 0x060013D8 RID: 5080 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060013D9 RID: 5081 RVA: 0x00079164 File Offset: 0x00077364
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "thirsty" };
	}

	// Token: 0x060013DA RID: 5082 RVA: 0x00079174 File Offset: 0x00077374
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		EntityPlayer primaryPlayer = GameManager.Instance.World.GetPrimaryPlayer();
		if (primaryPlayer != null)
		{
			float num = 0f;
			if (_params.Count > 0)
			{
				num = StringParsers.ParseFloat(_params[0], 0, -1, NumberStyles.Any);
			}
			primaryPlayer.Stats.Water.Value = (float)Mathf.CeilToInt(num / 100f * primaryPlayer.Stats.Water.ModifiedMax);
		}
	}

	// Token: 0x060013DB RID: 5083 RVA: 0x000791EB File Offset: 0x000773EB
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Makes the player thirsty (optionally specify the amount of water you want to have in percent).";
	}
}
