using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000268 RID: 616
[Preserve]
public class ConsoleCmdPois : ConsoleCmdAbstract
{
	// Token: 0x06001226 RID: 4646 RVA: 0x00071CE8 File Offset: 0x0006FEE8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "pois" };
	}

	// Token: 0x06001227 RID: 4647 RVA: 0x00071CF8 File Offset: 0x0006FEF8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Switches distant POIs on/off";
	}

	// Token: 0x06001228 RID: 4648 RVA: 0x00071CFF File Offset: 0x0006FEFF
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "Use on or off or only the command to toggle";
	}

	// Token: 0x06001229 RID: 4649 RVA: 0x00071D08 File Offset: 0x0006FF08
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		GameObject gameObject = GameObject.Find("/PrefabsLOD");
		if (gameObject != null)
		{
			ConsoleCmdPois.parentGO = gameObject;
		}
		if (ConsoleCmdPois.parentGO == null)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Distant POIs not active!");
			return;
		}
		if (_params.Count == 0)
		{
			ConsoleCmdPois.parentGO.SetActive(!ConsoleCmdPois.parentGO.activeSelf);
		}
		else if (_params[0] == "on")
		{
			ConsoleCmdPois.parentGO.SetActive(true);
		}
		else if (_params[0] == "off")
		{
			ConsoleCmdPois.parentGO.SetActive(true);
		}
		else
		{
			int num;
			if (int.TryParse(_params[0], out num))
			{
				GameManager.Instance.prefabLODManager.SetPOIDistance(128 * num);
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output(string.Concat(new string[]
				{
					"Setting to POI chunk distance ",
					num.ToString(),
					" =",
					(128 * num).ToString(),
					"m"
				}));
				return;
			}
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Unknown parameter");
			return;
		}
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("POIs set to " + (ConsoleCmdPois.parentGO.activeSelf ? "on" : "off"));
	}

	// Token: 0x04000D1C RID: 3356
	[PublicizedFrom(EAccessModifier.Private)]
	public static GameObject parentGO;
}
