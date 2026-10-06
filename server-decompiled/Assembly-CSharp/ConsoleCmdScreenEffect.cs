using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000205 RID: 517
[Preserve]
public class ConsoleCmdScreenEffect : ConsoleCmdAbstract
{
	// Token: 0x06000FB9 RID: 4025 RVA: 0x00065F72 File Offset: 0x00064172
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "ScreenEffect" };
	}

	// Token: 0x06000FBA RID: 4026 RVA: 0x00065F82 File Offset: 0x00064182
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Sets a screen effect";
	}

	// Token: 0x06000FBB RID: 4027 RVA: 0x00065F89 File Offset: 0x00064189
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "ScreenEffect [name] [intensity] [fade time]\nScreenEffect clear\nScreenEffect reload";
	}

	// Token: 0x06000FBC RID: 4028 RVA: 0x00065F90 File Offset: 0x00064190
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (_params.Count == 0)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output(this.GetHelp());
			return;
		}
		List<EntityPlayerLocal> localPlayers = GameManager.Instance.World.GetLocalPlayers();
		if (_params[0] == "clear")
		{
			for (int i = 0; i < localPlayers.Count; i++)
			{
				localPlayers[i].ScreenEffectManager.DisableScreenEffects();
			}
			return;
		}
		if (_params[0] == "reload")
		{
			for (int j = 0; j < localPlayers.Count; j++)
			{
				localPlayers[j].ScreenEffectManager.ResetEffects();
			}
			return;
		}
		float num = 0f;
		float num2 = 4f;
		if (_params.Count >= 2)
		{
			StringParsers.TryParseFloat(_params[1], out num);
		}
		if (_params.Count >= 3)
		{
			StringParsers.TryParseFloat(_params[2], out num2);
		}
		for (int k = 0; k < localPlayers.Count; k++)
		{
			localPlayers[k].ScreenEffectManager.SetScreenEffect(_params[0], num, num2);
		}
	}
}
