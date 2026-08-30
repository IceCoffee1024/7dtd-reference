using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000261 RID: 609
[Preserve]
public class ConsoleCmdPPList : ConsoleCmdAbstract
{
	// Token: 0x170001DF RID: 479
	// (get) Token: 0x06001221 RID: 4641 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001222 RID: 4642 RVA: 0x000715EC File Offset: 0x0006F7EC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "pplist" };
	}

	// Token: 0x06001223 RID: 4643 RVA: 0x000715FC File Offset: 0x0006F7FC
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

	// Token: 0x06001224 RID: 4644 RVA: 0x000716B4 File Offset: 0x0006F8B4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Lists all PersistentPlayer data";
	}
}
