using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine.Scripting;

// Token: 0x02000293 RID: 659
[Preserve]
public class ConsoleCmdShowHits : ConsoleCmdAbstract
{
	// Token: 0x17000204 RID: 516
	// (get) Token: 0x0600132B RID: 4907 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000205 RID: 517
	// (get) Token: 0x0600132C RID: 4908 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x0600132D RID: 4909 RVA: 0x00076290 File Offset: 0x00074490
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "showhits" };
	}

	// Token: 0x0600132E RID: 4910 RVA: 0x000762A0 File Offset: 0x000744A0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Show hit entity info";
	}

	// Token: 0x0600132F RID: 4911 RVA: 0x000762A7 File Offset: 0x000744A7
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "Commands:\ndamage - toggle damage numbers\nhits <time> <size> - toggle hits";
	}

	// Token: 0x06001330 RID: 4912 RVA: 0x000762B0 File Offset: 0x000744B0
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (_params.Count == 0)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output(this.GetHelp());
			return;
		}
		string text = _params[0].ToLower();
		if (text == "damage")
		{
			DamageText.Enabled = !DamageText.Enabled;
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Damage " + (DamageText.Enabled ? "on" : "off"));
			return;
		}
		if (!(text == "hits"))
		{
			return;
		}
		EntityAlive.ShowDebugDisplayHit = !EntityAlive.ShowDebugDisplayHit;
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Hits " + (EntityAlive.ShowDebugDisplayHit ? "on" : "off"));
		if (_params.Count >= 2)
		{
			EntityAlive.DebugDisplayHitTime = StringParsers.ParseFloat(_params[1], 0, -1, NumberStyles.Any);
		}
		if (_params.Count >= 3)
		{
			EntityAlive.DebugDisplayHitSize = StringParsers.ParseFloat(_params[2], 0, -1, NumberStyles.Any);
		}
		ItemAction.ShowDebugDisplayHit = EntityAlive.ShowDebugDisplayHit;
		ItemAction.DebugDisplayHitTime = EntityAlive.DebugDisplayHitTime;
		ItemAction.DebugDisplayHitSize = EntityAlive.DebugDisplayHitSize;
	}
}
