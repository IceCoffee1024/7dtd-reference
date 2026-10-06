using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000268 RID: 616
[Preserve]
public class ConsoleCmdPPList : ConsoleCmdAbstract
{
	// Token: 0x170001D8 RID: 472
	// (get) Token: 0x06001230 RID: 4656 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001231 RID: 4657 RVA: 0x00071EFC File Offset: 0x000700FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "pplist" };
	}

	// Token: 0x06001232 RID: 4658 RVA: 0x00071F0C File Offset: 0x0007010C
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		PersistentPlayerList persistentPlayers = GameManager.Instance.persistentPlayers;
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output(persistentPlayers.Players.Count.ToString() + " Persistent Player(s)");
		foreach (KeyValuePair<PlatformUserIdentifierAbs, PersistentPlayerData> keyValuePair in persistentPlayers.Players)
		{
			SdtdConsole instance = SingletonMonoBehaviour<SdtdConsole>.Instance;
			string text = "   ";
			PlatformUserIdentifierAbs key = keyValuePair.Key;
			instance.Output(text + ((key != null) ? key.ToString() : null) + " -> " + keyValuePair.Value.EntityId.ToString());
		}
	}

	// Token: 0x06001233 RID: 4659 RVA: 0x00071FC4 File Offset: 0x000701C4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Lists all PersistentPlayer data";
	}
}
