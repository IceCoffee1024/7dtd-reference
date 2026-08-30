using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020002A4 RID: 676
[Preserve]
public class ConsoleCmdStarve : ConsoleCmdAbstract
{
	// Token: 0x1700021F RID: 543
	// (get) Token: 0x060013A1 RID: 5025 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060013A2 RID: 5026 RVA: 0x00077C7F File Offset: 0x00075E7F
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "starve", "hungry", "food" };
	}

	// Token: 0x060013A3 RID: 5027 RVA: 0x00077CA0 File Offset: 0x00075EA0
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

	// Token: 0x060013A4 RID: 5028 RVA: 0x00077D17 File Offset: 0x00075F17
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Makes the player starve (optionally specify the amount of food you want to have in percent).";
	}
}
