using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine.Scripting;

// Token: 0x02000212 RID: 530
[Preserve]
public class ConsoleCmdExhausted : ConsoleCmdAbstract
{
	// Token: 0x17000174 RID: 372
	// (get) Token: 0x0600101E RID: 4126 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x0600101F RID: 4127 RVA: 0x00066CEB File Offset: 0x00064EEB
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "exhausted" };
	}

	// Token: 0x06001020 RID: 4128 RVA: 0x00066CFC File Offset: 0x00064EFC
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

	// Token: 0x06001021 RID: 4129 RVA: 0x00066D67 File Offset: 0x00064F67
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Makes the player exhausted.";
	}
}
