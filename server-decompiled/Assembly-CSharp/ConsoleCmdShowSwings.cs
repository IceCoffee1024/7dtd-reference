using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000298 RID: 664
[Preserve]
public class ConsoleCmdShowSwings : ConsoleCmdAbstract
{
	// Token: 0x17000208 RID: 520
	// (get) Token: 0x06001340 RID: 4928 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001341 RID: 4929 RVA: 0x0007664B File Offset: 0x0007484B
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "showswings" };
	}

	// Token: 0x06001342 RID: 4930 RVA: 0x0007665C File Offset: 0x0007485C
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

	// Token: 0x06001343 RID: 4931 RVA: 0x000766D5 File Offset: 0x000748D5
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Show melee swing arc rays";
	}
}
