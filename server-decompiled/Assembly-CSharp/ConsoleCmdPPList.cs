using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200026A RID: 618
[Preserve]
public class ConsoleCmdPPList : ConsoleCmdAbstract
{
	// Token: 0x170001D8 RID: 472
	// (get) Token: 0x06001234 RID: 4660 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001235 RID: 4661 RVA: 0x000720CC File Offset: 0x000702CC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "pplist" };
	}

	// Token: 0x06001236 RID: 4662 RVA: 0x000720DC File Offset: 0x000702DC
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

	// Token: 0x06001237 RID: 4663 RVA: 0x00072194 File Offset: 0x00070394
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Lists all PersistentPlayer data";
	}
}
