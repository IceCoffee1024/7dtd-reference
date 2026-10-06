using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000216 RID: 534
[Preserve]
public class ConsoleCmdExportPrefab : ConsoleCmdAbstract
{
	// Token: 0x06001021 RID: 4129 RVA: 0x00066F8F File Offset: 0x0006518F
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { ConsoleCmdExportPrefab.CommandName };
	}

	// Token: 0x17000170 RID: 368
	// (get) Token: 0x06001022 RID: 4130 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool AllowedInMainMenu
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06001023 RID: 4131 RVA: 0x00066F9F File Offset: 0x0006519F
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Exports a prefab from a world area";
	}

	// Token: 0x06001024 RID: 4132 RVA: 0x00066FA6 File Offset: 0x000651A6
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "Usage:\n  exportprefab <name> <x1> <y1> <z1> <x2> <y2> <z2> [part]\nExports a prefab with the given name from a box defined by the coordinate pair of two corners\nIf the optional parameter 'part' is 'true' it will export into the 'Parts' subfolder of the prefabs folder";
	}

	// Token: 0x06001025 RID: 4133 RVA: 0x00066FB0 File Offset: 0x000651B0
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (_params.Count < 7 || _params.Count > 8)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Illegal number of parameters");
			return;
		}
		bool flag = false;
		if (_params.Count == 8)
		{
			flag = ConsoleHelper.ParseParamBool(_params[7], true);
		}
		string text = _params[0];
		int num;
		if (!StringParsers.TryParseSInt32(_params[1], out num))
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("x1 coordinate is not a valid integer");
			return;
		}
		int num2;
		if (!StringParsers.TryParseSInt32(_params[2], out num2))
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("y1 coordinate is not a valid integer");
			return;
		}
		int num3;
		if (!StringParsers.TryParseSInt32(_params[3], out num3))
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("z1 coordinate is not a valid integer");
			return;
		}
		Vector3i vector3i = new Vector3i(num, num2, num3);
		if (!StringParsers.TryParseSInt32(_params[4], out num))
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("x2 coordinate is not a valid integer");
			return;
		}
		if (!StringParsers.TryParseSInt32(_params[5], out num2))
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("y2 coordinate is not a valid integer");
			return;
		}
		if (!StringParsers.TryParseSInt32(_params[6], out num3))
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("z2 coordinate is not a valid integer");
			return;
		}
		Vector3i vector3i2 = new Vector3i(num, num2, num3);
		if (Prefab.PrefabExists(text))
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("A prefab with the name \"" + text + "\" already exists.");
			return;
		}
		Prefab prefab = new Prefab();
		prefab.location = Prefab.LocationForNewPrefab(text, flag ? "Parts" : null);
		prefab.copyFromWorld(GameManager.Instance.World, vector3i, vector3i2);
		if (prefab.Save(prefab.location, true))
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Prefab saved to " + prefab.location.ToString());
			return;
		}
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Prefab could not be saved");
	}

	// Token: 0x06001026 RID: 4134 RVA: 0x00067178 File Offset: 0x00065378
	public static string BuildCommandString(string _prefabName, Vector3i _startPos, Vector3i _endPos, bool _asPart)
	{
		return string.Format("{0} {1} {2} {3} {4} {5} {6} {7} {8}", new object[]
		{
			ConsoleCmdExportPrefab.CommandName,
			_prefabName,
			_startPos.x,
			_startPos.y,
			_startPos.z,
			_endPos.x,
			_endPos.y,
			_endPos.z,
			_asPart
		});
	}

	// Token: 0x04000CA9 RID: 3241
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly string CommandName = "exportprefab";
}
