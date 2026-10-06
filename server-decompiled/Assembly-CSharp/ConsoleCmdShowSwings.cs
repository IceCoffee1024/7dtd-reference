using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000296 RID: 662
[Preserve]
public class ConsoleCmdShowSwings : ConsoleCmdAbstract
{
	// Token: 0x17000208 RID: 520
	// (get) Token: 0x0600133C RID: 4924 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x0600133D RID: 4925 RVA: 0x0007647B File Offset: 0x0007467B
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "showswings" };
	}

	// Token: 0x0600133E RID: 4926 RVA: 0x0007648C File Offset: 0x0007468C
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		EntityAlive.ShowDebugDisplayHit = !EntityAlive.ShowDebugDisplayHit;
		ItemActionDynamic.ShowDebugSwing = EntityAlive.ShowDebugDisplayHit;
		if (EntityAlive.ShowDebugDisplayHit)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Show Swings (ON)");
			return;
		}
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Show Swings (OFF)");
		for (int i = 0; i < ItemActionDynamic.DebugDisplayHits.Count; i++)
		{
			global::UnityEngine.Object.DestroyImmediate(ItemActionDynamic.DebugDisplayHits[i]);
		}
		ItemActionDynamic.DebugDisplayHits.Clear();
	}

	// Token: 0x0600133F RID: 4927 RVA: 0x00076505 File Offset: 0x00074705
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Show melee swing arc rays";
	}
}
