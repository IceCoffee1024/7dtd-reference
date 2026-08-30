using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200025F RID: 607
[Preserve]
public class ConsoleCmdPois : ConsoleCmdAbstract
{
	// Token: 0x06001213 RID: 4627 RVA: 0x00071208 File Offset: 0x0006F408
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "pois" };
	}

	// Token: 0x06001214 RID: 4628 RVA: 0x00071218 File Offset: 0x0006F418
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Switches distant POIs on/off";
	}

	// Token: 0x06001215 RID: 4629 RVA: 0x0007121F File Offset: 0x0006F41F
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "Use on or off or only the command to toggle";
	}

	// Token: 0x06001216 RID: 4630 RVA: 0x00071228 File Offset: 0x0006F428
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

	// Token: 0x04000D11 RID: 3345
	[PublicizedFrom(EAccessModifier.Private)]
	public static GameObject parentGO;
}
