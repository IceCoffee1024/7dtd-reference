using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine.Scripting;

// Token: 0x0200028C RID: 652
[Preserve]
public class ConsoleCmdShowHits : ConsoleCmdAbstract
{
	// Token: 0x1700020B RID: 523
	// (get) Token: 0x0600131C RID: 4892 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700020C RID: 524
	// (get) Token: 0x0600131D RID: 4893 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x0600131E RID: 4894 RVA: 0x00075980 File Offset: 0x00073B80
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "showhits" };
	}

	// Token: 0x0600131F RID: 4895 RVA: 0x00075990 File Offset: 0x00073B90
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Show hit entity info";
	}

	// Token: 0x06001320 RID: 4896 RVA: 0x00075997 File Offset: 0x00073B97
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "Commands:\ndamage - toggle damage numbers\nhits <time> <size> - toggle hits";
	}

	// Token: 0x06001321 RID: 4897 RVA: 0x000759A0 File Offset: 0x00073BA0
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
