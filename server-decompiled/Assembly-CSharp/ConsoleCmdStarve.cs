using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020002AD RID: 685
[Preserve]
public class ConsoleCmdStarve : ConsoleCmdAbstract
{
	// Token: 0x17000218 RID: 536
	// (get) Token: 0x060013B4 RID: 5044 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060013B5 RID: 5045 RVA: 0x0007875F File Offset: 0x0007695F
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "starve", "hungry", "food" };
	}

	// Token: 0x060013B6 RID: 5046 RVA: 0x00078780 File Offset: 0x00076980
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

	// Token: 0x060013B7 RID: 5047 RVA: 0x000787F7 File Offset: 0x000769F7
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Makes the player starve (optionally specify the amount of food you want to have in percent).";
	}
}
