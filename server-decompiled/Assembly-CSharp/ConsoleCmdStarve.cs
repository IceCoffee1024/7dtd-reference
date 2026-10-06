using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020002AB RID: 683
[Preserve]
public class ConsoleCmdStarve : ConsoleCmdAbstract
{
	// Token: 0x17000218 RID: 536
	// (get) Token: 0x060013B0 RID: 5040 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060013B1 RID: 5041 RVA: 0x0007858F File Offset: 0x0007678F
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "starve", "hungry", "food" };
	}

	// Token: 0x060013B2 RID: 5042 RVA: 0x000785B0 File Offset: 0x000767B0
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
			primaryPlayer.Stats.Food.Value = (float)Mathf.CeilToInt(num / 100f * primaryPlayer.Stats.Food.ModifiedMax);
		}
	}

	// Token: 0x060013B3 RID: 5043 RVA: 0x00078627 File Offset: 0x00076827
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Makes the player starve (optionally specify the amount of food you want to have in percent).";
	}
}
