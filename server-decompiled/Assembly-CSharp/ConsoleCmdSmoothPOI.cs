using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020002A0 RID: 672
[Preserve]
public class ConsoleCmdSmoothPOI : ConsoleCmdAbstract
{
	// Token: 0x0600136F RID: 4975 RVA: 0x00076D90 File Offset: 0x00074F90
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "smoothpoi" };
	}

	// Token: 0x17000210 RID: 528
	// (get) Token: 0x06001370 RID: 4976 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool AllowedInMainMenu
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06001371 RID: 4977 RVA: 0x00076DA0 File Offset: 0x00074FA0
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

	// Token: 0x06001372 RID: 4978 RVA: 0x00076E87 File Offset: 0x00075087
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Smoothens the POI";
	}

	// Token: 0x06001373 RID: 4979 RVA: 0x00076E8E File Offset: 0x0007508E
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "\r\n\t\t\t|Usage:\r\n\t\t\t|  smoothpoi [mode] [passes]\r\n\t\t\t|Mode defaults to \"land\", also accepts \"air\".\r\n            |Passes defaults to 1.\r\n            |Smoothed area can be restricted with the blue selection.\r\n\t\t\t".Unindent(true);
	}
}
