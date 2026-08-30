using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200029B RID: 667
[Preserve]
public class ConsoleCmdSmoothPOI : ConsoleCmdAbstract
{
	// Token: 0x0600136E RID: 4974 RVA: 0x0007690C File Offset: 0x00074B0C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "smoothpoi" };
	}

	// Token: 0x17000218 RID: 536
	// (get) Token: 0x0600136F RID: 4975 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool AllowedInMainMenu
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06001370 RID: 4976 RVA: 0x0007691C File Offset: 0x00074B1C
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

	// Token: 0x06001371 RID: 4977 RVA: 0x00076A03 File Offset: 0x00074C03
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Smoothens the POI";
	}

	// Token: 0x06001372 RID: 4978 RVA: 0x00076A0A File Offset: 0x00074C0A
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "\n\t\t\t|Usage:\n\t\t\t|  smoothpoi [mode] [passes]\n\t\t\t|Mode defaults to \"land\", also accepts \"air\".\n            |Passes defaults to 1.\n            |Smoothed area can be restricted with the blue selection.\n\t\t\t".Unindent(true);
	}
}
