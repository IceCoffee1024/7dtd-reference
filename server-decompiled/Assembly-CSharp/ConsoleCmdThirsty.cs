using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020002B5 RID: 693
[Preserve]
public class ConsoleCmdThirsty : ConsoleCmdAbstract
{
	// Token: 0x1700021E RID: 542
	// (get) Token: 0x060013E7 RID: 5095 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060013E8 RID: 5096 RVA: 0x00079A74 File Offset: 0x00077C74
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "thirsty" };
	}

	// Token: 0x060013E9 RID: 5097 RVA: 0x00079A84 File Offset: 0x00077C84
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

	// Token: 0x060013EA RID: 5098 RVA: 0x00079AFB File Offset: 0x00077CFB
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Makes the player thirsty (optionally specify the amount of water you want to have in percent).";
	}
}
