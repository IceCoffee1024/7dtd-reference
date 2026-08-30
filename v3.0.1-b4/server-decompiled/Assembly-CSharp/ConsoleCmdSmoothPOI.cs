using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000297 RID: 663
[Preserve]
public class ConsoleCmdSmoothPOI : ConsoleCmdAbstract
{
	// Token: 0x0600135C RID: 4956 RVA: 0x000762B0 File Offset: 0x000744B0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "smoothpoi" };
	}

	// Token: 0x17000217 RID: 535
	// (get) Token: 0x0600135D RID: 4957 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool AllowedInMainMenu
	{
		get
		{
			return false;
		}
	}

	// Token: 0x0600135E RID: 4958 RVA: 0x000762C0 File Offset: 0x000744C0
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (!PrefabEditModeManager.Instance.IsActive())
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Command can only be used in the prefab editor");
			return;
		}
		if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Command can only be used on the host");
			return;
		}
		string text = "land";
		if (_params.Count >= 1)
		{
			text = _params[0];
		}
		int num = 1;
		if (_params.Count == 2)
		{
			num = int.Parse(_params[1]);
		}
		bool flag = text.Equals("land");
		DateTime now = DateTime.Now;
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Starting POI smoothing pass");
		PrefabHelpers.SmoothPOI(num, flag);
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Finished POI smoothing at " + DateTime.Now.ToCultureInvariantString());
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output(string.Format("Smoothing action took {0} ms", (DateTime.Now - now).TotalMilliseconds));
	}

	// Token: 0x0600135F RID: 4959 RVA: 0x000763A7 File Offset: 0x000745A7
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Smoothens the POI";
	}

	// Token: 0x06001360 RID: 4960 RVA: 0x000763AE File Offset: 0x000745AE
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "\n\t\t\t|Usage:\n\t\t\t|  smoothpoi [mode] [passes]\n\t\t\t|Mode defaults to \"land\", also accepts \"air\".\n            |Passes defaults to 1.\n            |Smoothed area can be restricted with the blue selection.\n\t\t\t".Unindent(true);
	}
}
