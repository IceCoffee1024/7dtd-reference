using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine.Scripting;

// Token: 0x02000295 RID: 661
[Preserve]
public class ConsoleCmdShowHits : ConsoleCmdAbstract
{
	// Token: 0x17000204 RID: 516
	// (get) Token: 0x0600132F RID: 4911 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000205 RID: 517
	// (get) Token: 0x06001330 RID: 4912 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001331 RID: 4913 RVA: 0x00076460 File Offset: 0x00074660
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "showhits" };
	}

	// Token: 0x06001332 RID: 4914 RVA: 0x00076470 File Offset: 0x00074670
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Show hit entity info";
	}

	// Token: 0x06001333 RID: 4915 RVA: 0x00076477 File Offset: 0x00074677
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "Commands:\ndamage - toggle damage numbers\nhits <time> <size> - toggle hits";
	}

	// Token: 0x06001334 RID: 4916 RVA: 0x00076480 File Offset: 0x00074680
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
