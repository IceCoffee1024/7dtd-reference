using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000293 RID: 659
[Preserve]
public class ConsoleCmdShowSwings : ConsoleCmdAbstract
{
	// Token: 0x17000210 RID: 528
	// (get) Token: 0x0600133F RID: 4927 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001340 RID: 4928 RVA: 0x000761C7 File Offset: 0x000743C7
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "showswings" };
	}

	// Token: 0x06001341 RID: 4929 RVA: 0x000761D8 File Offset: 0x000743D8
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

	// Token: 0x06001342 RID: 4930 RVA: 0x00076251 File Offset: 0x00074451
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Show melee swing arc rays";
	}
}
