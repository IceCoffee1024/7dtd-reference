using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine.Scripting;

// Token: 0x02000213 RID: 531
[Preserve]
public class ConsoleCmdExhausted : ConsoleCmdAbstract
{
	// Token: 0x1700016C RID: 364
	// (get) Token: 0x06001011 RID: 4113 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001012 RID: 4114 RVA: 0x00066CDB File Offset: 0x00064EDB
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "exhausted" };
	}

	// Token: 0x06001013 RID: 4115 RVA: 0x00066CEC File Offset: 0x00064EEC
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

	// Token: 0x06001014 RID: 4116 RVA: 0x00066D57 File Offset: 0x00064F57
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Makes the player exhausted.";
	}
}
