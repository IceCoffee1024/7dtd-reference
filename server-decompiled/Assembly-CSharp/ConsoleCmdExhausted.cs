using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine.Scripting;

// Token: 0x02000215 RID: 533
[Preserve]
public class ConsoleCmdExhausted : ConsoleCmdAbstract
{
	// Token: 0x1700016C RID: 364
	// (get) Token: 0x06001015 RID: 4117 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001016 RID: 4118 RVA: 0x00066EAB File Offset: 0x000650AB
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "exhausted" };
	}

	// Token: 0x06001017 RID: 4119 RVA: 0x00066EBC File Offset: 0x000650BC
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
			primaryPlayer.Stats.Stamina.Value = num * primaryPlayer.Stats.Stamina.Max;
		}
	}

	// Token: 0x06001018 RID: 4120 RVA: 0x00066F27 File Offset: 0x00065127
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Makes the player exhausted.";
	}
}
