using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020002B7 RID: 695
[Preserve]
public class ConsoleCmdThirsty : ConsoleCmdAbstract
{
	// Token: 0x1700021E RID: 542
	// (get) Token: 0x060013EB RID: 5099 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060013EC RID: 5100 RVA: 0x00079C44 File Offset: 0x00077E44
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "thirsty" };
	}

	// Token: 0x060013ED RID: 5101 RVA: 0x00079C54 File Offset: 0x00077E54
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

	// Token: 0x060013EE RID: 5102 RVA: 0x00079CCB File Offset: 0x00077ECB
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Makes the player thirsty (optionally specify the amount of water you want to have in percent).";
	}
}
